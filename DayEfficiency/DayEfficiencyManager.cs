using DayEfficiency;

public class DayEfficiencyManager
{
	private TextGenerator _textGenerator = null;
	private DestinationFile _txtFile = null;
	private EfficiencyData _efficiencyData = null;
	private AppConfigProcessor _configData = null;

	public DayEfficiencyManager(TextGenerator generator, DestinationFile destinationFile, EfficiencyData data, AppConfigProcessor configData) 
	{
		_textGenerator = generator;
		_txtFile = destinationFile;
		_efficiencyData = data;
		_configData = configData;
	}

	public bool IsSourсeFileChanged()
	{
		return _efficiencyData.LastProcessedDate.Date != _efficiencyData.LastExcelFileUpdateDate.Date;
	}

	public bool IsItFirstLaunch()
	{
		return _efficiencyData.LastMonthEfficiency == -1;
	}

	public bool IsProcessedTime(DateTime currentDate)
	{
		if (currentDate.Hour > _efficiencyData.ProcessedTime.Hour && currentDate.Hour < 24 &&
				currentDate != _efficiencyData.LastProcessedDate)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool ProduceEfficiency(DateTime currentDate)
	{
		var _efficiencyContext = DefineEfficiencyContext(currentDate);

		if (_txtFile.IsFileExistAndFree(_efficiencyData.DestinationFilePath))
		{
			if (_efficiencyContext == EfficiencyContext.FirstLaunchContext)
			{
				ExecuteFirstLaunchStrategy(currentDate, _efficiencyData.CurrentMonthEfficiency);
				return true;
			}
			else if (_efficiencyContext == EfficiencyContext.ActualMonthContext)
			{
				ExecuteActualMonthStrategy(currentDate, _efficiencyData.CurrentDayEfficiency);
				return true;
			}
			else if (_efficiencyContext == EfficiencyContext.NewMonthWithRecordInLastContext)
			{
				ExecuteNewMonthWithRecordInLastStrategy(currentDate, _efficiencyData.CurrentMonthEfficiency);
				return true;
			}
			else if (_efficiencyContext == EfficiencyContext.NewMonthContext)
			{
				ExecuteNewMonthStrategy(currentDate, _efficiencyData.CurrentMonthEfficiency);
				return true;
			}
			else return false;
		}
		else 
		{
			return false; 
		}
	}

	public EfficiencyContext DefineEfficiencyContext(DateTime currentDate)
	{
		if (IsItFirstLaunch())
		{
			return EfficiencyContext.FirstLaunchContext;
		}
		else if (_efficiencyData.LastProcessedDate.Month == currentDate.Month)
		{
			return EfficiencyContext.ActualMonthContext;
		}
		else if (_efficiencyData.LastProcessedDate.Month < currentDate.Month)
		{
			if (_efficiencyData.LastProcessedDate.Day != DateTime.DaysInMonth(_efficiencyData.LastProcessedDate.Year, _efficiencyData.LastProcessedDate.Month))
			{
				return EfficiencyContext.NewMonthWithRecordInLastContext;
			}
			else
			{
				return EfficiencyContext.NewMonthContext;
			}
		}

		return EfficiencyContext.FaultContext;
	}

	private void UpdateConfig(DateTime currentDate, decimal currentEfficiency)
	{
		_configData.UpdateConfig(ConfigKeys.lastProcessedDate, currentDate.ToString());
		_configData.UpdateConfig(ConfigKeys.lastCellValue, currentEfficiency.ToString());
	}

	private void ExecuteFirstLaunchStrategy(DateTime currentDate, decimal currentMonthEfficiency)
	{
		_txtFile.WriteRecord(_textGenerator.BuildFirstRecord(0, currentDate));
		UpdateConfig(currentDate.Date, currentMonthEfficiency);
	}

	private void ExecuteActualMonthStrategy(DateTime currentDate, decimal currentDayEfficiency)
	{
		int freeDays = currentDate.Day - _efficiencyData.LastProcessedDate.Day - 1;
		_txtFile.WriteRecord(_textGenerator.BuildRecord(currentDayEfficiency, freeDays, false, currentDate));
		UpdateConfig(currentDate.Date, currentDayEfficiency);
	}

	private void ExecuteNewMonthWithRecordInLastStrategy(DateTime currentDate, decimal currentMonthEfficiency)
	{
		int freeDaysInLastMonth = DateTime.DaysInMonth(_efficiencyData.LastProcessedDate.Year, _efficiencyData.LastProcessedDate.Month) - _efficiencyData.LastProcessedDate.Day;

		_txtFile.WriteRecord(_textGenerator.BuildWhiteSpaceRecord(freeDaysInLastMonth));
		ExecuteNewMonthStrategy(currentDate, currentMonthEfficiency);
		UpdateConfig(currentDate.Date, currentMonthEfficiency);
	}

	private void ExecuteNewMonthStrategy(DateTime currentDate, decimal currentMonthEfficiency)
	{
		if (currentDate.Day != 1)
		{
			//int freeDaysInNewMonth = currentDate.Day - 1;
			_txtFile.WriteRecord(_textGenerator.BuildWhiteSpaceInNewMonthLine(currentDate));
			_txtFile.WriteRecord(_textGenerator.BuildRecord(currentMonthEfficiency, 0, false, currentDate));
		}
		else
		{
			_txtFile.WriteRecord(_textGenerator.BuildRecord(currentMonthEfficiency, 0, true, currentDate));
		}
		UpdateConfig(currentDate.Date, currentMonthEfficiency);
	}

}

