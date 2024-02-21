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
	public void ProduceEfficiency_FileIsNotChenged_False()
	{
		ClearTestFile();

		var efficiencyData = GetTestEfficiencyData(
			currentDayEfficiency: 0m,
			currentExcelFileUpdateDate: new DateTime(2024, 01, 07),
			lastProcessedDate: new DateTime(2024, 01, 07),
			currentMonthEfficiency: 45m,
			lastMonthEfficiency: 33m,
			lastExcelFileUpdateDate: new DateTime(2024, 01, 07));
		var efficiencyManager = GetDayEfficiencyManager(efficiencyData);


		var isFine = efficiencyManager.IsSourсeFileChanged();


		Assert.False(isFine);
	}

	[Fact]
	public void ProduceEfficiency_DestinationFileNotExists_False()
	{
		string tempPath = _testFile;
		_testFile = "C:\\feiled file name.txt";
		var efficiencyData = GetTestEfficiencyData(
			currentDayEfficiency: 0m,
			currentExcelFileUpdateDate: new DateTime(2024, 01, 07),
			lastProcessedDate: new DateTime(2024, 01, 07),
			currentMonthEfficiency: 45m,
			lastMonthEfficiency: -1m,
			lastExcelFileUpdateDate: new DateTime(2024, 01, 07));
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
			currentExcelFileUpdateDate : new DateTime(2024, 01, 07),
			lastProcessedDate : new DateTime(2024, 01, 07),
			currentMonthEfficiency : 45m,
			lastMonthEfficiency : -1m,
			lastExcelFileUpdateDate : new DateTime(2024, 01, 07));
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
			currentExcelFileUpdateDate : new DateTime(2024, 01, 09),
			lastProcessedDate : new DateTime(2024, 01, 08),
			currentMonthEfficiency : 45m,
			lastMonthEfficiency : 48.55m,
			lastExcelFileUpdateDate : new DateTime(2024, 01, 08));
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
			currentExcelFileUpdateDate : new DateTime(2024, 01, 13),
			lastProcessedDate : new DateTime(2024, 01, 10),
			currentMonthEfficiency : 21.22m,
			lastMonthEfficiency : 11m,
			lastExcelFileUpdateDate : new DateTime(2024, 01, 10));
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
			currentExcelFileUpdateDate : new DateTime(2024, 02, 01),
			lastProcessedDate : new DateTime(2024, 01, 31),
			currentMonthEfficiency : 21.22m,
			lastMonthEfficiency : 11m,
			lastExcelFileUpdateDate : new DateTime(2024, 01, 31));
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
			currentExcelFileUpdateDate: new DateTime(2024, 02, 01),
			lastProcessedDate: new DateTime(2024, 01, 28),
			currentMonthEfficiency: 21.22m,
			lastMonthEfficiency: 11m,
			lastExcelFileUpdateDate : new DateTime(2024, 01, 27));
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
		DateTime currentExcelFileUpdateDate,
		DateTime lastProcessedDate,
		decimal currentMonthEfficiency,
		decimal lastMonthEfficiency,
		DateTime lastExcelFileUpdateDate)
	{
		var efficiencyData = new EfficiencyData()
		{
			DestinationFilePath = _testFile,
			CurrentDayEfficiency = currentDayEfficiency,
			CurrentExcelFileUpdateDate = currentExcelFileUpdateDate,
			LastProcessedDate = lastProcessedDate,
			CurrentMonthEfficiency = currentMonthEfficiency,
			LastMonthEfficiency = lastMonthEfficiency,
			ProcessedTime = new TimeOnly(5, 00),
			LastExcelFileUpdateDate= lastExcelFileUpdateDate,
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

