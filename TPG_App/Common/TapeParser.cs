using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using TPG_App.Models;

namespace TPG_App.Common
{
    public class TapeParser
    {
        public const string APP_SETTINGS_FAILED_TAPES_PATH = "FailedTapesPath";
        public const string APP_SETTINGS_CREATED_TAPES_PATH = "CreatedTapesPath";
        public const string APP_SETTINGS_IMPORTED_TAPES_PATH = "ImportedTapesPath";

        public const string SELLER_BID_TAPE_NAME = "SellerBidTape";
        public const string SELLER_PRICING_TAPE = "SellerPricingTape";
        
        //private TapeErrorInfo tapeErrors;
        private TradeTape tape;
        private string worksheetname = "";
        private List<TradeTapeColumnDef> columnDefs;
        private ExcelQueryFactory excelFile;
        
        TradeModels db = new TradeModels();

        public TapeParser(TradeTape tape)
        {
            this.tape = tape;
            columnDefs = db.TradeTapeColumnDefs.Where(d => d.TapeName == tape.Name).ToList();
        }

        /// <summary>
        /// Parser's and validates the tape.
        /// </summary>
        /// <returns>True if validation is successful.  False if validation fails.</returns>
        public async Task<TapeErrorInfo> ParseTapeAsync()
        {
            TapeErrorInfo tapeErrors = new TapeErrorInfo();
            
            excelFile = new ExcelQueryFactory(tape.StoragePath);
            var worksheets = excelFile.GetWorksheetNames();

            if (worksheets.Count() > 0)
            {
                worksheetname = worksheets.First();

                var validSheetColumns = ValidateSheetColumnNames(tapeErrors);
                if(!tapeErrors.HaveErrors)
                {
                    
                    var rowEnumerator = excelFile.Worksheet(worksheetname).GetEnumerator();
                    {
                        int lineCount = 0;
                        while(rowEnumerator.MoveNext())
                        {
                            lineCount++;
                            Row currentRow = rowEnumerator.Current;

                            LoanErrors err = await ValidateSheetRowAsync(validSheetColumns, lineCount, currentRow);
                            if (err.HaveLoanErrors)
                            {
                                tapeErrors.LoanLevelErrors.Add(err);
                                tapeErrors.HaveErrors = true;
                            }

                        }
                    }
                }
            }
            else
            {
                tapeErrors.WorksheetErrors.Add(new KeyValuePair<string, string>("worksheet", "no work sheet found"));
            }
            
            if(tapeErrors.HaveErrors)
            {
                Parallel.Invoke(() => LogTapeError(tape.TapeID, tapeErrors));
            }
            return tapeErrors;
        }

        public static void LogTapeError(int tapeId, TapeErrorInfo tapeErrors)
        {
            var dirPath = ConfigurationManager.AppSettings[APP_SETTINGS_FAILED_TAPES_PATH];
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var errorFileName = dirPath + "\\" + tapeId + ".parse";
            File.WriteAllText(errorFileName, JsonConvert.SerializeObject(tapeErrors));
        }

        public static void BackUpTape(TradeTape tradeTape, bool importErrors)
        {
            var dirPath = ConfigurationManager.AppSettings[APP_SETTINGS_IMPORTED_TAPES_PATH];
            if (importErrors)
            {
                dirPath = ConfigurationManager.AppSettings[APP_SETTINGS_FAILED_TAPES_PATH];
            }

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var moveFullFileName = dirPath + "\\" + tradeTape.TapeID + "_" + DateTime.UtcNow.ToFileTimeUtc() + "_" + tradeTape.StoragePath.Split('\\').Last();
            File.Move(tradeTape.StoragePath, moveFullFileName);
        }

        private async Task<LoanErrors> ValidateSheetRowAsync(List<string> columnsToValidate, int lineCount, Row currentRow)
        {
            LoanErrors err = new LoanErrors();
            err.LoanLine = lineCount;
            foreach (var colDef in this.columnDefs)
            {
                if (!columnsToValidate.Contains(colDef.ColumnName))
                    continue;

                switch (colDef.ColumnType)
                {
                    case "datetime":
                        try
                        {
                            Convert.ToDateTime(currentRow[colDef.ColumnName]);
                        }
                        catch (Exception)
                        {
                            err.columnErrors.Add(new KeyValuePair<string, string>(colDef.ColumnName, "Not a valid date"));
                            err.HaveLoanErrors = true;
                        }
                        break;
                    case "decimal":
                        try
                        {
                            Convert.ToDecimal(currentRow[colDef.ColumnName]);
                        }
                        catch (Exception)
                        {
                            err.columnErrors.Add(new KeyValuePair<string, string>(colDef.ColumnName, "Not a valid decimal value"));
                            err.HaveLoanErrors = true;
                        }
                        break;
                    case "percent":
                        try
                        {
                            Convert.ToDecimal(currentRow[colDef.ColumnName]);
                        }
                        catch (Exception)
                        {
                            err.columnErrors.Add(new KeyValuePair<string, string>(colDef.ColumnName, "Not a valid percent value"));
                            err.HaveLoanErrors = true;
                        }
                        break;
                    default:
                        if ((this.tape.Name == SELLER_PRICING_TAPE) && (colDef.PalFieldName == "SellerAssetID"))
                        {
                            var sellerAssetId = currentRow[colDef.ColumnName];

                            List<TradeAsset> assets = await db.TradeAssets.Where(a => (a.TradeID == this.tape.TradeID) && (a.SellerAssetId == sellerAssetId)).ToListAsync();//await db.TradeAssets.Where(a => (a.TradeID == this.tape.TradeID) && (a.SellerAssetID == sellerAssetId)).
                            if (assets.Count() == 0)
                            {
                                err.columnErrors.Add(new KeyValuePair<string, string>(colDef.ColumnName, "The seller loan ID " + sellerAssetId + " is not found in the current trade assets collection"));
                                err.HaveLoanErrors = true;
                            }
                        }
                        break;
                }
            }

            return err;
        }


        private void AddColDefinitionMappings()
        {
            try
            {

                //foreach (var colDef in columnDefs)
                //{
                //    excelFile.AddMapping<TradeAsset>(x => x.GetType().GetProperty(colDef.PalFieldName), colDef.ColumnName);
                //}

                switch (tape.Name)
                {

                    case SELLER_BID_TAPE_NAME:
                        //var assetPropertyInfo = Type.GetType(typeof(TradeAsset).ToString()).GetProperties();
                        //foreach(var prop in assetPropertyInfo)
                        //{
                        //    var colDef = columnDefs.Where(d => d.PalFieldName == prop.ToString()).ToList();
                        //    if (colDef.Count() > 0)
                        //    {

                        //    }
                        //}



                        excelFile.AddMapping<TradeAsset>(x => x.Bpo, columnDefs.Where(d => d.PalFieldName == "Bpo").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.BpoDate, columnDefs.Where(d => d.PalFieldName == "BpoDate").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.Cbsa, columnDefs.Where(d => d.PalFieldName == "Cbsa").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CbsaName, columnDefs.Where(d => d.PalFieldName == "CbsaName").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.City, columnDefs.Where(d => d.PalFieldName == "City").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CurrentBalance, columnDefs.Where(d => d.PalFieldName == "CurrentBalance").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CurrentPmt, columnDefs.Where(d => d.PalFieldName == "CurrentPmt").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CurrFico, columnDefs.Where(d => d.PalFieldName == "CurrFico").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CurrFicoDate, columnDefs.Where(d => d.PalFieldName == "CurrFicoDate").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.ForebearanceBalance, columnDefs.Where(d => d.PalFieldName == "ForebearanceBalance").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.LoanPurp, columnDefs.Where(d => d.PalFieldName == "LoanPurp").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.MaturityDate, columnDefs.Where(d => d.PalFieldName == "MaturityDate").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.NextDueDate, columnDefs.Where(d => d.PalFieldName == "NextDueDate").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.OrigFico, columnDefs.Where(d => d.PalFieldName == "OrigFico").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.OriginalBalance, columnDefs.Where(d => d.PalFieldName == "OriginalBalance").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.OriginalDate, columnDefs.Where(d => d.PalFieldName == "OriginalDate").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.OriginalPmt, columnDefs.Where(d => d.PalFieldName == "OriginalPmt").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.PaidToDate, columnDefs.Where(d => d.PalFieldName == "PaidToDate").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.PayString, columnDefs.Where(d => d.PalFieldName == "PayString").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.ProdType, columnDefs.Where(d => d.PalFieldName == "ProdType").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.SellerAssetId, columnDefs.Where(d => d.PalFieldName == "SellerAssetID").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.State, columnDefs.Where(d => d.PalFieldName == "State").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.StreetAddress1, columnDefs.Where(d => d.PalFieldName == "StreetAddress1").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.Zip, columnDefs.Where(d => d.PalFieldName == "Zip").First().ColumnName);
                        break;

                    case SELLER_PRICING_TAPE:
                        excelFile.AddMapping<TradeAssetPricing>(x => x.SellerAssetID, columnDefs.Where(d => d.PalFieldName == "SellerAssetID").First().ColumnName);
                        excelFile.AddMapping<TradeAssetPricing>(x => x.BidPercentage, columnDefs.Where(d => d.PalFieldName == "BidPercentage").First().ColumnName);
                        excelFile.AddMapping<TradeAssetPricing>(x => x.CurrentBalance, columnDefs.Where(d => d.PalFieldName == "CurrentBalance").First().ColumnName);
                        excelFile.AddMapping<TradeAssetPricing>(x => x.CurrentPrice, columnDefs.Where(d => d.PalFieldName == "CurrentPrice").First().ColumnName);
                        excelFile.AddMapping<TradeAssetPricing>(x => x.ForebearanceBalance, columnDefs.Where(d => d.PalFieldName == "ForebearanceBalance").First().ColumnName);
                        break;
                }

            }
            catch(Exception ex)
            {
                string msg = ex.Message;
            }
            
        }

        internal List<TradeAssetPricing> GetTradeAssetPricesFromTape(out List<string> columnsInTape)
        {
            List<TradeAssetPricing> sellerLoanPricingItems = new List<TradeAssetPricing>();
            columnsInTape = new List<string>();
            excelFile = new ExcelQueryFactory(tape.StoragePath);

            AddColDefinitionMappings();

            var worksheets = excelFile.GetWorksheetNames();
            if (worksheets.Count() > 0)
            {
                worksheetname = worksheets.First();
                sellerLoanPricingItems = excelFile.Worksheet<TradeAssetPricing>(worksheetname).ToList();
                columnsInTape = excelFile.GetColumnNames(worksheetname).Join(columnDefs, colName => colName, cd => cd.ColumnName, (colName, cd) => cd.PalFieldName).ToList();
              
                //foreach (var item in sellerLoanPricingItems)
                //{
                //    var assetId = db.TradeAssets.Where(a => a.TradeID == tape.TradeID && a.SellerAssetID == item.SellerAssetID).First().AssetID;
                //    item.AssetID = assetId;
                //    item.Source = sourceString;
                //}
            }

            return sellerLoanPricingItems;
        }

        internal List<TradeAsset> GetTradeAssets()
        {
            List<TradeAsset> assets = new List<TradeAsset>();
            excelFile = new ExcelQueryFactory(tape.StoragePath);
            AddColDefinitionMappings();
            var counterPartyID = db.TradePools.Where(p => p.TradeID == tape.TradeID).First().CounterPartyID;

            var worksheets = excelFile.GetWorksheetNames();
            if (worksheets.Count() > 0)
            {
                worksheetname = worksheets.First();
                assets = excelFile.Worksheet<TradeAsset>(worksheetname).ToList();
                foreach(var asset in assets)
                {
                    asset.TradeID = tape.TradeID;
                    asset.TapeID = tape.TapeID;
                    asset.Seller_CounterPartyID = (short)counterPartyID;
                }
            }

            return assets;
        }

        private List<string> ValidateSheetColumnNames(TapeErrorInfo tapeErrors)
        {
            List<string> availableColumns = null;
            try
            {
                var requiredColumnNames = columnDefs.Select(d => d.ColumnName).ToList();
                
                List<string> missingColumnNames = GetMissingColumnsFromSheet(requiredColumnNames);

                if (missingColumnNames.Count() > 0)
                {
                    switch (tape.Name)
                    {
                        //For the bid tape all columns are required
                        case SELLER_BID_TAPE_NAME:
                            string missingNames = missingColumnNames.Aggregate((s1, s2) => s1 + ", " + s2);
                            tapeErrors.WorksheetErrors.Add(new KeyValuePair<string, string>("Missing Columns in worksheet", missingNames));
                            tapeErrors.HaveErrors = true;
                            break;

                        //For the pricing tape all columns are optional.  As long as we have the SellerAssetID and atlease one other valid column it should be imported
                        case SELLER_PRICING_TAPE:
                            string sellerAssetIDColumnName = columnDefs.Where(cd => cd.PalFieldName == "SellerAssetID").First().ColumnName;
                            bool isSellerAssetIDMissing = missingColumnNames.Where(cn => cn == sellerAssetIDColumnName).Count() > 0 ? true : false;
                            if (isSellerAssetIDMissing || (missingColumnNames.Count() == (requiredColumnNames.Count() - 1))) //SellerAssetID missing or no other valid column is found
                            {
                                missingNames = missingColumnNames.Aggregate((s1, s2) => s1 + ", " + s2);
                                tapeErrors.WorksheetErrors.Add(new KeyValuePair<string, string>("No valid columns to import.  Import needs the " + sellerAssetIDColumnName + " and atleast one of the columns from:", missingNames));
                                tapeErrors.HaveErrors = true;
                            }
                            break;
                    }
                }

                //Remove the missing columns from the required columns
                availableColumns = requiredColumnNames.ToList();
                availableColumns.RemoveAll(cn => missingColumnNames.Contains(cn));
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return (tapeErrors.HaveErrors ? null : availableColumns);
        }

        private List<string> GetMissingColumnsFromSheet(List<string> requiredColumnNames)
        {
            var sheetColumnNames = excelFile.GetColumnNames(worksheetname);

            var missingColumnNames = columnDefs.Select(d => d.ColumnName).ToList();
            foreach (var reqCol in requiredColumnNames)
            {
                if (sheetColumnNames.Where(cn => cn == reqCol).Count() > 0)
                {
                    missingColumnNames.Remove(reqCol);
                }
            }

            return missingColumnNames;
        }

        public List<TradeTapeColumnDef> GetValidColumnsFromSheet()
        {
            var validColumnsList = columnDefs.Select(cd => cd).ToList();
            var missingColumnsList = GetMissingColumnsFromSheet(validColumnsList.Select(cd => cd.ColumnName).ToList());

            validColumnsList.RemoveAll(vc => missingColumnsList.Contains(vc.ColumnName));
            return validColumnsList;
        }
    }

    public class TapeErrorInfo
    {
        
        public TapeErrorInfo()
        {
            WorksheetErrors = new List<KeyValuePair<string, string>>();
            LoanLevelErrors = new List<LoanErrors>();
            HaveErrors = false;
        }

        public bool HaveErrors { get; set; }

        public List<KeyValuePair<string, string>> WorksheetErrors { get; set; }



        public List<LoanErrors> LoanLevelErrors { get; set; }

    }

    public class LoanErrors
    {
        public LoanErrors()
        {
            columnErrors = new List<KeyValuePair<string, string>>();
            HaveLoanErrors = false;
        }

        public bool HaveLoanErrors { get; set; }
        public int LoanLine { get; set; }
        public List<KeyValuePair<string, string>> columnErrors { get; set; }
    }
}