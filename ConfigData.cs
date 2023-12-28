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
        private static string _cellAddress;      
        private static DateTime _lastProcessedDate = new DateTime(2023, 01, 01, 00, 00, 00);
        private static TimeOnly _processedTime = new TimeOnly(05, 00);
        private static double _lastCellValue = 0;

        public static string SourceFile { get { return _sourseFile; } } 
        public static string DestinationFile { get { return _destinationFile; } }
        public static string CellAddress { get { return _cellAddress; } }
       
        public static DateTime LastProcessedDate { get { return _lastProcessedDate; } set { _lastProcessedDate = value; } }
        public static TimeOnly ProcessedTime { get {  return _processedTime; } }
        public static double LastCellValue { get { return _lastCellValue; } set {  _lastCellValue = value; } }
        
        
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
                        _cellAddress = allConfiguration[keys]; break;
                    case "last_processed_date":
                        _lastProcessedDate = DateTime.Parse(allConfiguration[keys]); break;
                    case "processing_time":
                        _processedTime = TimeOnly.Parse(allConfiguration[keys]); break;
                    case "last_cell_value":
                        _lastCellValue = Double.Parse(allConfiguration[keys]); break;

                }
            
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
              


    }
}
