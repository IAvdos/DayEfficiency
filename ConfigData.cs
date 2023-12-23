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
        private static string _cellAddress = "C18";
        private static DateTime _lastProcessedDate = new DateTime(2023, 01, 01, 00, 00, 00);

        public static string SourceFile { get { return _sourseFile; } } 
        public static string DestinationFile { get { return _destinationFile; } }
        public static string CellAddress { get { return _cellAddress; } }
        public static DateTime LastProcessedDate { get { return _lastProcessedDate; } set { _lastProcessedDate = value; } }
        
        
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
                        _cellAddress= allConfiguration[keys]; break;
                    case "last_processed_date":
                        _lastProcessedDate = DateTime.Parse(allConfiguration[keys]); break;

                }
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
            int _lettersInAlphabet = 26;

        }
        private static int ConvertRowLatterToItn(string columnLatter) 
        {
        }

        private static int GetNumberOfLatter(char latter)
        {
            return (int)latter - (char)latter + 1;
        }


    }
}
