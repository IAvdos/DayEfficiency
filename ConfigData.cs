﻿using System;
using System.Configuration;
using DayEfficiency;


internal static class ConfigData
{
    public static string SourceFile { get; private set; } = @"C:\Users\Public\Documents\balance_debug.xlsx";
    public static string DestinationFile { get; private set; } = @"C:\Users\Avdos\source\repos\DayEfficiency\DayEfficiency.txt";
    public static string CellAddress { get; private set; } = "C18";
    public static DateTime LastProcessedDate { get; private set; } = new DateTime(2023, 01, 01, 00, 00, 00);
    public static TimeOnly ProcessedTime { get; private set; } = new TimeOnly(05, 00);
    public static decimal LastCellValue { get; set; } = -1;


    public static void ReadAppConfig()
    {
        var allConfiguration = ConfigurationManager.AppSettings;
        
        foreach (var keys in allConfiguration.AllKeys)
        {
            switch (keys)
            {
                case ConfigKeys.sourseFile:
                    SourceFile = allConfiguration[keys]; break;

                case ConfigKeys.destinationFile:
                    DestinationFile = allConfiguration[keys]; break;

                case ConfigKeys.cellAddress: 
                    CellAddress = allConfiguration[keys]; break;

                case ConfigKeys.lastProcessedDate:
                    LastProcessedDate = DateTime.Parse(allConfiguration[keys]); break;

                case ConfigKeys.processedTime:
                    ProcessedTime = TimeOnly.Parse(allConfiguration[keys]); break;

                case ConfigKeys.lastCellValue:
                    LastCellValue = Decimal.Parse(allConfiguration[keys]); break;
            }
        }
    }

    public static void UpdateConfig(string key, string value)
    {
        var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        var settings = configFile.AppSettings.Settings;
      
        settings[key].Value = value;

        configFile.Save(ConfigurationSaveMode.Full);
        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        ReadAppConfig();
    }

}

