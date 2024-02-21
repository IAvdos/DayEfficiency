using System.Configuration;
using System.Diagnostics;
using DayEfficiency;

public class AppConfigProcessor
{
    AppConfigProcessor() { }
    public AppConfigProcessor(Configuration configuration)
    {
        _appConfiguration = configuration;
    }
    private Configuration _appConfiguration;
    public string SourceFile { get; private set; } = @"C:\Users\Public\Documents\balance_debug.xlsx";
    public string DestinationFile { get; private set; } = @"C:\Users\Avdos\source\repos\DayEfficiency\DayEfficiency.txt";
    public string CellAddress { get; private set; } = "C18";
    public DateTime LastProcessedDate { get; private set; } = new DateTime(2023, 01, 01, 00, 00, 00);
    public TimeOnly ProcessedTime { get; private set; } = new TimeOnly(05, 00);
    public decimal LastCellValue { get; set; } = -1;
    public DateTime LastSourceFileChanged { get; private set; } = new DateTime(2023, 01, 01, 00, 00, 00);

    public void ReadConfig()
    {
        var allConfiguration = ConfigurationManager.AppSettings;
        
        foreach (var key in allConfiguration.AllKeys)
        {
            switch (key)
            {
                case ConfigKeys.sourseFile:
                    SourceFile = allConfiguration[key]; break;

                case ConfigKeys.destinationFile:
                    DestinationFile = allConfiguration[key]; break;

                case ConfigKeys.cellAddress: 
                    CellAddress = allConfiguration[key]; break;

                case ConfigKeys.lastProcessedDate:
                    LastProcessedDate = DateTime.Parse(allConfiguration[key]); break;

                case ConfigKeys.processedTime:
                    ProcessedTime = TimeOnly.Parse(allConfiguration[key]); break;

                case ConfigKeys.lastCellValue:
                    LastCellValue = Decimal.Parse(allConfiguration[key]); break;

                case ConfigKeys.lastSourceFileChanged:
                    LastSourceFileChanged = DateTime.Parse(allConfiguration[key]); break;
            }
        }
    }

    public void UpdateConfig(string key, string value)
    {
        var settings = _appConfiguration.AppSettings.Settings;

        settings[key].Value = value;

		_appConfiguration.Save(ConfigurationSaveMode.Full);
        ConfigurationManager.RefreshSection(_appConfiguration.AppSettings.SectionInformation.Name);

        ReadConfig();
    }
}

