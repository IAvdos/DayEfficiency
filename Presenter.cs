using System;
using DayEfficiency;


internal class Presenter
{
    private DestinationFile _txtFile = null;
    private TextGenerator _textGenerator = null;
    private DataInfo _dataInfo = null;

    public Presenter()
    {
        _dataInfo = new DataInfoGenerator().CreateDataInfo();
        _txtFile = new DestinationFile(ConfigData.DestinationFile);
        _textGenerator = new TextGenerator();
    }

    //Constructor just for test
    public Presenter(DataInfo dataInfo)
    {
        _dataInfo = dataInfo;
        _txtFile = new DestinationFile(ConfigData.DestinationFile);
        _textGenerator = new TextGenerator();
    }

    public bool IsSourseFileChenged()
    {
        if (_dataInfo.LastWriteDateSourceFile != _dataInfo.LastUpdateDate)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool IsProcessedTime()
    {
        if (_dataInfo.CurentDate.Hour > _dataInfo.ProcessedTime.Hour && _dataInfo.CurentDate.Hour < 24 && _dataInfo.LastUpdateDate != _dataInfo.CurentDate)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsItFirstLounch()
    {
        if (_dataInfo.LastMonthEfficiency == -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DefineAndExecuteStrategy()
    {
        if (IsItFirstLounch())
        {
            ExecuteFirstLounchStrategy();
            return true;
        }
        else if (_dataInfo.LastUpdateDate.Month == _dataInfo.CurentDate.Month)
        {
            ExecuteActualMonthStrategy();
            return true;
        }
        else if (_dataInfo.LastUpdateDate.Month < _dataInfo.CurentDate.Month)
        {
            if (_dataInfo.LastUpdateDate.Day != DateTime.DaysInMonth(_dataInfo.LastUpdateDate.Year, _dataInfo.LastUpdateDate.Month))
            {
                ExecuteNewMonthWithRecordInLastStrategy();
                return true;
            }
            else
            {
                ExecuteNewMonthStrategy();
                return true;
            }
        }
        return false;
    }

    private void UpdateConfig(DateTime currentDate, double currentEfficiency)
    {
        ConfigData.UpdateConfig(ConfigKeys.lastProcessedDate, currentDate.ToString());
        ConfigData.UpdateConfig(ConfigKeys.lastCellValue, currentEfficiency.ToString());
    }

    private void ExecuteFirstLounchStrategy()
    {
        Console.WriteLine("It's firs program lounch.");
        _txtFile.WriteRecord(_textGenerator.BuildFirstRecord(0, _dataInfo.CurentDate.Day));
        UpdateConfig(_dataInfo.CurentDate.Date, _dataInfo.CurrentMonthEfficiency);
    }

    private void ExecuteActualMonthStrategy()
    {
        int freeDays = _dataInfo.CurentDate.Day - _dataInfo.LastUpdateDate.Day - 1;
        _txtFile.WriteRecord(_textGenerator.BuildRecord(_dataInfo.CurrentEfficiency, freeDays, false));
        UpdateConfig(_dataInfo.CurentDate.Date, _dataInfo.CurrentEfficiency);
    }

    private void ExecuteNewMonthWithRecordInLastStrategy()
    {
        int freeDaysInLastMonth = DateTime.DaysInMonth(_dataInfo.LastUpdateDate.Year, _dataInfo.LastUpdateDate.Month) - _dataInfo.LastUpdateDate.Day;

        _txtFile.WriteRecord(_textGenerator.BuildWhiteSpaceRecord(freeDaysInLastMonth));
        ExecuteNewMonthStrategy();
        UpdateConfig(_dataInfo.CurentDate.Date, _dataInfo.CurrentEfficiency);
    }

    private void ExecuteNewMonthStrategy()
    {
        if (_dataInfo.CurentDate.Day != 1)
        {
            int freeDaysInNewMonth = _dataInfo.CurentDate.Day - 1;
            _txtFile.WriteRecord(_textGenerator.BuildWhiteSpaceInNewMonthLine(freeDaysInNewMonth));
            _txtFile.WriteRecord(_textGenerator.BuildRecord(_dataInfo.CurrentMonthEfficiency, 0, false));
        }
        else
        {
            _txtFile.WriteRecord(_textGenerator.BuildRecord(_dataInfo.CurrentMonthEfficiency, 0, true));
        }
        UpdateConfig(_dataInfo.CurentDate.Date, _dataInfo.CurrentMonthEfficiency);
    }
}
