using System.Configuration;
using System.IO;
using System.Text;
namespace DayEfficiency.Tests;

public class DayEfficiencyManagerTests
{
	//"F:\\Development\\DayEfficiency\\DayEfficiency.Tests\\TestResources\\Test.txt"
	string _testFile = ".\\..\\..\\..\\TestResources\\Test.txt";
	//"F:\\Development\\DayEfficiency\\DayEfficiency.Tests\\TestResources\\TestRecordSamples.txt"
	string _testRecordSamples = ".\\..\\..\\..\\TestResources\\TestRecordSamples.txt";

	[Fact]
	public void ProduceEfficiency_DestinationFileNotExists_False()
	{
		string tempPath = _testFile;
		_testFile = "C:\\feiled file name.txt";
		var efficiencyData = GetTestEfficiencyData(
			currentDayEfficiency: 0m,
			lastExcelFileUpdateDate: new DateTime(2024, 01, 07),
			lastProcessedDate: new DateTime(2024, 01, 07),
			currentMonthEfficiency: 45m,
			lastMonthEfficiency: -1m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);


		var result = efficiencyManager.ProduceEfficiency(new DateTime(2024, 01, 08));


		Assert.False(result);
		_testFile = tempPath;
	}

	[Fact]
	public void ProduceEfficiency_FirstLaunch_CorrectWrite()
	{
		ClearTestFile();

		var efficiencyData = GetTestEfficiencyData(
			currentDayEfficiency : 0m,
			lastExcelFileUpdateDate : new DateTime(2024, 01, 07),
			lastProcessedDate : new DateTime(2024, 01, 07),
			currentMonthEfficiency : 45m,
			lastMonthEfficiency : -1m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);


		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 01, 08));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("FirstLaunch");


		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	[Fact]
	public void ProduceEfficiency_ActualMonthLaunch_CorrectWrite()
	{
		ClearTestFile();

		var efficiencyData = GetTestEfficiencyData(
			currentDayEfficiency : 3.55m,
			lastExcelFileUpdateDate : new DateTime(2024, 01, 09),
			lastProcessedDate : new DateTime(2024, 01, 08),
			currentMonthEfficiency : 45m,
			lastMonthEfficiency : 48.55m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);


		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 01, 09));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("ActualMonthLaunch");


		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	[Fact]
	public void ProduceEfficiency_ActualMounthLaunchWithTwoDaysFree_CorrectWrite()
	{
		ClearTestFile();

		var efficiencyData = GetTestEfficiencyData(
			currentDayEfficiency : 11.22m,
			lastExcelFileUpdateDate : new DateTime(2024, 01, 13),
			lastProcessedDate : new DateTime(2024, 01, 10),
			currentMonthEfficiency : 21.22m,
			lastMonthEfficiency : 11m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);


		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 01, 13));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("ActualMaunthLaunchWithTreeDaysFree");


		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	[Fact]
	public void ProduceEfficiency_NewMonthLaunch_CorrectWrite()
	{
		ClearTestFile();

		var efficiencyData = GetTestEfficiencyData(
			currentDayEfficiency : 11.22m,
			lastExcelFileUpdateDate : new DateTime(2024, 02, 01),
			lastProcessedDate : new DateTime(2024, 01, 31),
			currentMonthEfficiency : 21.22m,
			lastMonthEfficiency : 11m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);


		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 02, 01));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("NewMonthLaunch");


		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	[Fact]
	public void ProduceEfficiency_NewMonthLaunchWithTreeDaysFreeInPrevious_CorrectWrite()
	{
		ClearTestFile();

		var efficiencyData = GetTestEfficiencyData(
			currentDayEfficiency: 11.22m,
			lastExcelFileUpdateDate: new DateTime(2024, 02, 01),
			lastProcessedDate: new DateTime(2024, 01, 28),
			currentMonthEfficiency: 21.22m,
			lastMonthEfficiency: 11m);
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);


		var isFine = efficiencyManager.ProduceEfficiency(new DateTime(2024, 02, 01));
		var testRecord = ReadTestRecord();
		var sampleRecord = ReadSampleRecord("NewMonthLaunchWithTreeDaysFreeInPrevious");


		Assert.True(isFine);
		Assert.Equal(sampleRecord, testRecord);
	}

	void ClearTestFile()
	{
		File.WriteAllText(_testFile, String.Empty);
	}
	
	EfficiencyData GetTestEfficiencyData(
		decimal currentDayEfficiency,
		DateTime lastExcelFileUpdateDate,
		DateTime lastProcessedDate,
		decimal currentMonthEfficiency,
		decimal lastMonthEfficiency)
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

	DayEfficiencyManager GetDayEfficiencyManager(EfficiencyData efficiencyData)
	{
		var map = new ExeConfigurationFileMap();
		map.ExeConfigFilename = ".\\..\\..\\..\\App.config";

		var configFile = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
		var logWriter = new ConsoleLogWriter();
		var destinationFile = new DestinationFile(logWriter);
		var configData = new AppConfigProcessor(configFile);
		var textGenerator = new TextGenerator();
		var efficiencyManager = new DayEfficiencyManager(textGenerator, destinationFile, efficiencyData, configData);

		return efficiencyManager;
	}

	string ReadSampleRecord(string sampleName)
	{
		string samples = null;

		using(var reader = new StreamReader(_testRecordSamples))
		{
			samples = reader.ReadToEnd();
		}

		return GetRecordSample(sampleName, samples);
	}

	string GetRecordSample(string sampleName, string samples)
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

	string ReadTestRecord()
	{
		string record;
		using (var reader = new StreamReader(_testFile))
		{
			record = reader.ReadToEnd();
		}

		return record;
	}
}

