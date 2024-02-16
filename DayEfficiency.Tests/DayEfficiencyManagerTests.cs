using System.Configuration;
using System.Text;
namespace DayEfficiency.Tests;

public class DayEfficiencyManagerTests
{
	//"F:\\Development\\DayEfficiency\\DayEfficiency.Tests\\TestResources\\Test.txt"
	private string _testFile = ".\\..\\..\\..\\TestResources\\Test.txt";
	//"F:\\Development\\DayEfficiency\\DayEfficiency.Tests\\TestResources\\TestRecordSamples.txt"
	private string _testRecordSamples = ".\\..\\..\\..\\TestResources\\TestRecordSamples.txt";

	[Fact]
	public void ProduceEfficiency_FirstLaunch_CorrectWrite()
	{
		//Arrange
		ClearTestFile();
		var efficiencyData = GetTestEfficiencyData(0m, new DateTime(2024, 01, 07), new DateTime(2024, 01, 07), 45m, -1m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);
		//Act
		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 01, 08));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("FirstLaunch");
		//Assert
		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	[Fact]
	public void ProduceEfficiency_ActualMonthLaunch_CorrectWrite()
	{
		//Arrange
		ClearTestFile();
		var efficiencyData = GetTestEfficiencyData(3.55m, new DateTime(2024, 01, 09), new DateTime(2024, 01, 08), 45m, 48.55m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);
		//Act
		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 01, 09));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("ActualMonthLaunch");
		//Assert
		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	[Fact]
	public void ProduceEfficiency_ActualMaunthLaunchWithTwoDaysFree_CorrectWrite()
	{
		//Arrange
		ClearTestFile();
		var efficiencyData = GetTestEfficiencyData(11.22m, new DateTime(2024, 01, 13), new DateTime(2024, 01, 10), 21.22m, 11m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);
		//Act
		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 01, 13));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("ActualMaunthLaunchWithTreeDaysFree");
		//Assert
		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	[Fact]
	public void ProduceEfficiency_NewMonthLaunch_CorrectWrite()
	{
		//Arrange
		ClearTestFile();
		var efficiencyData = GetTestEfficiencyData(11.22m, new DateTime(2024, 02, 01), new DateTime(2024, 01, 31), 21.22m, 11m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);
		//Act
		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 02, 01));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("NewMonthLaunch");
		//Assert
		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	[Fact]
	public void ProduceEfficiency_NewMonthLaunchWithTreeDaysFreeInPrevious_CorrectWrite()
	{
		//Arrange
		ClearTestFile();
		var efficiencyData = GetTestEfficiencyData(11.22m, new DateTime(2024, 02, 01), new DateTime(2024, 01, 28), 21.22m, 11m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);
		//Act
		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 02, 01));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("NewMonthLaunchWithTreeDaysFreeInPrevious");
		//Assert
		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	private void ClearTestFile()
	{
		var stream = new StreamWriter(_testFile, false, Encoding.UTF8);
		stream.Close();
	}
	
	private EfficiencyData GetTestEfficiencyData(decimal currentDayEfficiency, DateTime lastExcelFileUpdateDate, DateTime lastProcessedDate,
		decimal currentMonthEfficiency, decimal lastMonthEfficiency)
	{
		var efficiencyData = new EfficiencyData()
		{
			DestinationFilePath = _testFile,
			CurrentDayEfficiency = currentDayEfficiency,
			LastExcelFileUpdateDate = lastExcelFileUpdateDate,
			LastProcessedDate = lastProcessedDate,
			CurrentMonthEfficiency = currentMonthEfficiency,
			LastMonthEfficiency = lastMonthEfficiency,
			ProcessedTime = new TimeOnly(5, 00)
		};

		return efficiencyData;
	}

	private DayEfficiencyManager GetDayEfficiencyManager(EfficiencyData efficiencyData)
	{
		var map = new ExeConfigurationFileMap();
		map.ExeConfigFilename = "F:\\Development\\DayEfficiency\\DayEfficiency.Tests\\App.config";

		var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
		var logWriter = new ConsoleLogWriter();
		var destinationFile = new DestinationFile(logWriter);
		var configData = new AppConfigProcessor(configFile);
		var textGenerator = new TextGenerator();
		var efficiencyManager = new DayEfficiencyManager(textGenerator, destinationFile, efficiencyData, configData);

		return efficiencyManager;
	}

	private string ReadSampleRecord(string sampleName)
	{
		string record = null;
		string samples = null;
		using(var reader = new StreamReader(_testRecordSamples))
		{
			samples = reader.ReadToEnd();
		}

		return GetRecordSample(sampleName, samples);
	}

	private string GetRecordSample(string sampleName, string samples)
	{
		var samplesArray = samples.Split('#');
		string record = null;

		foreach(var sample in samplesArray)
		{
			if(sample.StartsWith(sampleName + "\r"))
			{
				record = sample.Substring(sampleName.Length + 2);
				record = record.TrimEnd('\n', '\r');
			}
		}

		return record;
	}

	private string ReadTestRecord()
	{
		string record;
		using (var reader = new StreamReader(_testFile))
		{
			record = reader.ReadToEnd();
		}

		return record;
	}
}

