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

                ValidateSheetColumnNames(tapeErrors);
                if(!tapeErrors.HaveErrors)
                {
                    
                    var rowEnumerator = excelFile.Worksheet(worksheetname).GetEnumerator();
                    {
                        int lineCount = 0;
                        while(rowEnumerator.MoveNext())
                        {
                            lineCount++;
                            Row currentRow = rowEnumerator.Current;

                            LoanErrors err = await ValidateSheetRowAsync(lineCount, currentRow);
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

        private async Task<LoanErrors> ValidateSheetRowAsync(int lineCount, Row currentRow)
        {
            LoanErrors err = new LoanErrors();
            err.LoanLine = lineCount;
            foreach (var colDef in this.columnDefs)
            {
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
                        if ((this.tape.Name == "SellerPricingTape") && (currentRow[colDef.PalFieldName] == "SellerAssetID"))
                        {
                            var sellerAssetId = currentRow[colDef.ColumnName];

                            List<TradeAsset> assets = await db.TradeAssets.Where(a => (a.TradeID == this.tape.TradeID) && (a.SellerAssetID == sellerAssetId)).ToListAsync();//await db.TradeAssets.Where(a => (a.TradeID == this.tape.TradeID) && (a.SellerAssetID == sellerAssetId)).
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
                switch (tape.Name)
                {
                    case "SellerBidTape":
                        excelFile.AddMapping<TradeAsset>(x => x.Bpo, columnDefs.Where(d => d.PalFieldName == "Bpo").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.BpoDate, columnDefs.Where(d => d.PalFieldName == "BpoDate").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.Cbsa, columnDefs.Where(d => d.PalFieldName == "Cbsa").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CbsaName, columnDefs.Where(d => d.PalFieldName == "CbsaName").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.City, columnDefs.Where(d => d.PalFieldName == "City").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CurrentBalance, columnDefs.Where(d => d.PalFieldName == "CurrentBalance").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CurrentPmt, columnDefs.Where(d => d.PalFieldName == "CurrentPmt").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CurrFico, columnDefs.Where(d => d.PalFieldName == "CurrFico").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.CurrFicoDate, columnDefs.Where(d => d.PalFieldName == "CurrFicoDate").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.ForebearBalance, columnDefs.Where(d => d.PalFieldName == "ForebearBalance").First().ColumnName);
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
                        excelFile.AddMapping<TradeAsset>(x => x.SellerAssetID, columnDefs.Where(d => d.PalFieldName == "SellerAssetID").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.State, columnDefs.Where(d => d.PalFieldName == "State").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.StreetAddress1, columnDefs.Where(d => d.PalFieldName == "StreetAddress1").First().ColumnName);
                        excelFile.AddMapping<TradeAsset>(x => x.Zip, columnDefs.Where(d => d.PalFieldName == "Zip").First().ColumnName);
                        break;

                    case "SellerPricingTape":
                        excelFile.AddMapping<TradeAssetPricing>(x => x.SellerAssetID, columnDefs.Where(d => d.PalFieldName == "SellerAssetID").First().ColumnName);
                        excelFile.AddMapping<TradeAssetPricing>(x => x.BidPercentage, columnDefs.Where(d => d.PalFieldName == "BidPercentage").First().ColumnName);
                        excelFile.AddMapping<TradeAssetPricing>(x => x.UnpaidBalance, columnDefs.Where(d => d.PalFieldName == "UnpaidBalance").First().ColumnName);
                        break;
                }
                
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
            }
            
        }

        internal List<TradeAssetPricing> GetTradeAssetPrices()
        {
            List<TradeAssetPricing> sellerLoanPricingItems = new List<TradeAssetPricing>();
            excelFile = new ExcelQueryFactory(tape.StoragePath);
            AddColDefinitionMappings();

            var worksheets = excelFile.GetWorksheetNames();
            if (worksheets.Count() > 0)
            {
                worksheetname = worksheets.First();
                sellerLoanPricingItems = excelFile.Worksheet<TradeAssetPricing>(worksheetname).ToList();
                foreach (var item in sellerLoanPricingItems)
                {
                    var assetId = db.TradeAssets.Where(a => a.TradeID == tape.TradeID && a.SellerAssetID == item.SellerAssetID).First().AssetID;
                    item.AssetID = assetId;
                    item.Source = "tape:" + tape.TapeID;
                }
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

        private void ValidateSheetColumnNames(TapeErrorInfo tapeErrors)
        {
            var sheetColumnNames = excelFile.GetColumnNames(worksheetname);
            var requiredColumnNames = columnDefs.Select(d => d.ColumnName).ToList();
            var missingColumnNames = columnDefs.Select(d => d.ColumnName).ToList();
            try
            {
                foreach (var reqCol in requiredColumnNames)
                {
                    if (sheetColumnNames.Where(cn => cn == reqCol).Count() > 0)
                    {
                        missingColumnNames.Remove(reqCol);
                    }
                }

                if (missingColumnNames.Count() > 0)
                {
                    string missingNames = missingColumnNames.Aggregate((s1, s2) => s1 + ", " + s2);
                    tapeErrors.WorksheetErrors.Add(new KeyValuePair<string, string>("Missing Columns in worksheet", missingNames));
                    tapeErrors.HaveErrors = true;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
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