using System;
using DayEfficiency;


internal class DataInfoGenerator
{
    private DestinationFile _destinationFile = null;
    private ExcelFile _sourceFile = null;
    private DateTime _currentDate;
    private double _curentCellValue = 0;

    public DataInfoGenerator()
    {
        ConfigData.ReadAppConfig();
        _destinationFile = new DestinationFile(ConfigData.DestinationFile);
        _sourceFile = new ExcelFile(ConfigData.SourceFile);
        _currentDate = DateTime.Now;
    }

    public DataInfo CreateDataInfo()
    {
        var dataInfo = new DataInfo()
        {
            CurentDate = _currentDate,
            LastUpdateDate = ConfigData.LastProcessedDate,
            CurrentEfficiency = CalculateCurrentEfficiency(),
            LastMonthEfficiency = ConfigData.LastCellValue,
            CurrentMonthEfficiency = _curentCellValue,
            ProcessedTime = ConfigData.ProcessedTime,
            LastWriteDateSourceFile = _sourceFile.GetLastUpdateDate()
        };
        return dataInfo;
    }

    private double CalculateCurrentEfficiency()
    {
        _curentCellValue = _sourceFile.GetCellValue(ConfigData.CellAddress);
       
        return _curentCellValue - ConfigData.LastCellValue;
    }

}

