using DayEfficiency;

public class EfficiencyDataGenerator
{
	private AppConfigProcessor _config;
	private ExcelFileReader _excelFileReader;

	private decimal _currentMonthEfficiency = 0;
	private decimal _lastMonthEfficiency = 0;
	private decimal _currentDayEfficiency = 0;
	private DateTime _lastExcelFileUpdateDate;
	private DateTime _lastProcessedDate;
	private string _cellAddress;
	private string _sourseExcelFilePath;
	private string _destinationFilePath;
	private TimeOnly _processedTime;


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
			_lastExcelFileUpdateDate = _excelFileReader.LastUpdateDate;
			_currentDayEfficiency = CallculateDayEfficiency();
			return true;
		}
		else return false;
	}

	private void GetDataFromConfig()
	{
		_config.ReadConfig();
		_lastMonthEfficiency = _config.LastCellValue;
		_lastExcelFileUpdateDate = _config.LastProcessedDate;
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
			LastExcelFileUpdateDate = _lastExcelFileUpdateDate,
			ProcessedTime = _processedTime,
			DestinationFilePath = _destinationFilePath
		};
		
		return data;
	}

}

