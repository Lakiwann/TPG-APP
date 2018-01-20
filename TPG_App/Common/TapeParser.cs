using LinqToExcel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TPG_App.Models;

namespace TPG_App.Common
{
    public class TapeParser
    {
        const string APP_SETTINGS_FAILED_TAPES_PATH = "FailedTapesPath";
        private TapeErrorInfo tapeErrors;
        private TradeTape tape;
        private string worksheetname = "";
        private List<TradeTapeColumnDef> columnDefs;
        private ExcelQueryFactory excelFile;
        private bool haveErrors = false;

        TradeModels db = new TradeModels();

        public TapeParser(TradeTape tape)
        {
            this.tape = tape;
            tapeErrors = new TapeErrorInfo();
            columnDefs = db.TradeTapeColumnDefs.Where(d => d.TapeName == "SellerBidTape").ToList();
        }

        public bool ParseTape()
        {
            bool haveErrors = false;

            excelFile = new ExcelQueryFactory(tape.StoragePath);
            var worksheets = excelFile.GetWorksheetNames();

            if (worksheets.Count() > 0)
            {
                worksheetname = worksheets.First();

                haveErrors = ValidateSheetColumnNames();
                if(!haveErrors)
                {
                    AddColDefinitionMappings();

                    var rowEnumerator = excelFile.Worksheet<TradeAsset>(worksheetname).GetEnumerator();
                    {
                        int lineCount = 0;
                        while(rowEnumerator.MoveNext())
                        {
                            lineCount++;
                            try
                            {
                                TradeAsset asset = rowEnumerator.Current;
                            }
                            catch(Exception ex)
                            {
                                LoanErrors err = new LoanErrors();
                                err.LoanLine = lineCount;
                                err.columnErrors.Add(new KeyValuePair<string, string>("Error Message", ex.Message));
                                tapeErrors.LoanLevelErrors.Add(err);
                                haveErrors = true;
                                continue;
                            }
                        }
                    }
                }
            }
            else
            {
                tapeErrors.WorksheetErrors.Add(new KeyValuePair<string, string>("worksheet", "no work sheet found"));
            }
            //List<TradeTapeColumnDef> bidTapeColumnDefs = db.TradeTapeColumnDefs.Where(d => d.TapeName == "SellerBidTape").ToList();

            if(haveErrors)
            {
                var dirPath = ConfigurationManager.AppSettings[APP_SETTINGS_FAILED_TAPES_PATH];
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }

                var errorFileName = dirPath + "\\" + tape.TapeID + ".parse";
                File.WriteAllText(errorFileName, JsonConvert.SerializeObject(tapeErrors));
                
            }
            return haveErrors;
        }

        public TapeErrorInfo GetErrors
        {
            get { return tapeErrors; }
        }

        private void AddColDefinitionMappings()
        {
            try
            {
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
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
            }
            
        }

        private bool ValidateSheetColumnNames()
        {
            bool haveErrors = false;
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
                    haveErrors = true;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return haveErrors;
        }
    }

    public class TapeErrorInfo
    {
        
        public TapeErrorInfo()
        {
            WorksheetErrors = new List<KeyValuePair<string, string>>();
            LoanLevelErrors = new List<LoanErrors>();

            
        }
        public List<KeyValuePair<string, string>> WorksheetErrors { get; set; }



        public List<LoanErrors> LoanLevelErrors { get; set; }

    }

    public class LoanErrors
    {
        public LoanErrors()
        {
            columnErrors = new List<KeyValuePair<string, string>>();
        }

        public int LoanLine { get; set; }
        public List<KeyValuePair<string, string>> columnErrors { get; set; }
    }
}