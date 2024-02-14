using DayEfficiency;

public class EfficiencyData
{
    public decimal LastMonthEfficiency { get; set; }
    public decimal CurrentDayEfficiency { get; set; }
    public decimal CurrentMonthEfficiency { get; set; }
    public DateTime LastProcessedDate { get; set; }
    public TimeOnly ProcessedTime { get; set; }
    public DateTime LastExcelFileUpdateDate { get; set; }
    public string DestinationFilePath { get; set; }
}

