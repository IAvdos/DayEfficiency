using System.Configuration;
using System.Diagnostics.Eventing.Reader;
using System.Reflection.Metadata;
using System.Text;

namespace DayEfficiency;

class Program
{
	static void Main()
	{
		Console.OutputEncoding = Encoding.UTF8;
		var logWriter = new ConsoleLogWriter();

		var _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		var _configData = new AppConfigProcessor(_configuration);
		var _excelFile = new ExcelFileReader(logWriter);
		var _dataGenerator = new EfficiencyDataGenerator(_configData, _excelFile);

		if (_dataGenerator.ReadEfficiencyData())
		{
			var _textGenerator = new TextGenerator();
			var _efficiencyData = _dataGenerator.GetEfficiencyData();
			var _destinationFile = new DestinationFile(logWriter);
			var _efficiencyManager = new DayEfficiencyManager(_textGenerator, _destinationFile, _efficiencyData, _configData);

			var curentDate = DateTime.Now;

			if (_efficiencyManager.IsProcessedTime(curentDate))
			{
				if (_efficiencyManager.IsItFirstLaunch())
				{
					_efficiencyManager.ProduceEfficiency(curentDate);
					logWriter.WriteMessege("It's first program launch.");
				}
				else
				{
					if (_efficiencyManager.IsSourсeFileChanged())
					{
						if (_efficiencyManager.ProduceEfficiency(curentDate))
						{
							logWriter.WriteMessege("Program completed successfully.");
						}
					}
					else
					{
						logWriter.WriteMessege("Data in source file is not changed.");
					}
				}
			}
			else
			{
				logWriter.WriteMessege($"Wrong start time program. Please launch it between {_efficiencyData.ProcessedTime} and 24:00");
			}
		}

		logWriter.WriteMessege("Push any button to exit.");
		Console.ReadKey();
	}
}