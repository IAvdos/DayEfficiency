using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace DayEfficiency
{
    internal static class ConfigData
    {
        private static string _sourseFile = @"C:\Users\Public\Documents\balance_debug.xlsx";
        private static string _destinationFile = @"C:\Users\Public\Documents\DayEfficiency.txt";
        private static int _cellColumnNumber = 0;
        private static int _cellRowNumber = 0;
        private static DateTime _lastProcessedDate = new DateTime(2023, 01, 01, 00, 00, 00);
        private static TimeOnly _processedTime = new TimeOnly(05, 00);

        public static string SourceFile { get { return _sourseFile; } } 
        public static string DestinationFile { get { return _destinationFile; } }
        public static int CellColumnNumber { get { return _cellColumnNumber; } }
        public static int CellRowNumber {  get { return _cellRowNumber; } }
        public static DateTime LastProcessedDate { get { return _lastProcessedDate; } set { _lastProcessedDate = value; } }
        public static TimeOnly ProcessedTime { get {  return _processedTime; } }
        
        
        public static void ReadAppConfig() 
        {
            var allConfiguration = ConfigurationManager.AppSettings;
            
            foreach (var keys in allConfiguration.AllKeys)
            {
                switch (keys)
                {
                    case "source_file":
                        _sourseFile = allConfiguration[keys]; break;
                    case "destination_file":
                        _destinationFile = allConfiguration[keys]; break;
                    case "cell_name": 
                        ConvertCellAddress(allConfiguration[keys]); break;
                    case "last_processed_date":
                        _lastProcessedDate = DateTime.Parse(allConfiguration[keys]); break;
                    case "processing_time":
                        _processedTime = TimeOnly.Parse(allConfiguration[keys]); break;

                }
                //TEST
                Console.WriteLine($"key = {keys}, value = {allConfiguration[keys]}");
            }           
        }

        public static void UpdateConfig(ConfigKeys key, string value)
        {
            
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
          
            settings[key.ToString()].Value = value;            

            configFile.Save(ConfigurationSaveMode.Full);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);            
                
            
        }

        
        private static void ConvertCellAddress(string cellAddress)
        {
            int _columnNumber = 0;
            int _rowNumber = 0;
            string _letterColun = null;
            

            for(int i = 0; i < cellAddress.Length; i++)
            {
                if (char.IsLetter(cellAddress[i])) _letterColun += cellAddress[i];
                else
                {
                    _rowNumber = Convert.ToInt32(cellAddress.Substring(i));
                    break;
                }
            }
            _columnNumber = ConvertColumnLatterToItn(_letterColun.ToUpper());

            _cellColumnNumber = _columnNumber;
            _cellRowNumber = _rowNumber;
           
        }
        private static int ConvertColumnLatterToItn(string columnLatter) 
        {
            double _lettersInAlphabet = 26;
            double _columnNumber = 0;

            for(int i = columnLatter.Length - 1 ;i >= 0; i--)
            {
                double rank = columnLatter.Length - i - 1;
                
               
                _columnNumber += ((int)columnLatter[i] - (int)'A' + 1) * Math.Pow(_lettersInAlphabet, rank);
            }
            return (int) _columnNumber;
        }

        


    }
}
