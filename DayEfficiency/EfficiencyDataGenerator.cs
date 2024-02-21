using DayEfficiency;

public class EfficiencyDataGenerator
{
	AppConfigProcessor _config;
	ExcelFileReader _excelFileReader;

	decimal _currentMonthEfficiency = 0;
	decimal _lastMonthEfficiency = 0;
	decimal _currentDayEfficiency = 0;
	DateTime _currentExcelFileUpdateDate;
	DateTime _lastExcelFileUpdateDate;
	DateTime _lastProcessedDate;
	string _cellAddress;
	string _sourseExcelFilePath;
	string _destinationFilePath;
	TimeOnly _processedTime;


	public EfficiencyDataGenerator(AppConfigProcessor configData, ExcelFileReader excelFile)
	{
		_config = configData;
		_excelFileReader = excelFile;
	}

	public bool ReadEfficiencyData()
	{
		GetDataFromConfig();

		if(_excelFileReader.TryReadFromExcelFile(_sourseExcelFilePath, _cellAddress))
		{
			_currentMonthEfficiency = _excelFileReader.CellValue;
			_currentExcelFileUpdateDate = _excelFileReader.CurrentUpdateDate;
			_currentDayEfficiency = CallculateDayEfficiency();
			return true;
		}
		else return false;
	}

	private void GetDataFromConfig()
	{
		_config.ReadConfig();

		_lastMonthEfficiency = _config.LastCellValue;
		_lastExcelFileUpdateDate = _config.LastSourceFileChanged;
		_cellAddress = _config.CellAddress;
		_sourseExcelFilePath = _config.SourceFile;
		_lastProcessedDate = _config.LastProcessedDate;
		_processedTime = _config.ProcessedTime;
		_destinationFilePath = _config.DestinationFile;
	}

	private decimal CallculateDayEfficiency()
	{
		return _currentMonthEfficiency - _lastMonthEfficiency;
	}

	public EfficiencyData GetEfficiencyData()
	{
		var data = new EfficiencyData()
		{
			CurrentMonthEfficiency = _currentMonthEfficiency,
			LastMonthEfficiency = _lastMonthEfficiency,
			CurrentDayEfficiency = _currentDayEfficiency,
			LastProcessedDate = _lastProcessedDate,
			CurrentExcelFileUpdateDate = _currentExcelFileUpdateDate,
			ProcessedTime = _processedTime,
			DestinationFilePath = _destinationFilePath,
			LastExcelFileUpdateDate= _lastExcelFileUpdateDate
		};
		
		return data;
	}

}

