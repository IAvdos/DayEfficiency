using System;
using DayEfficiency;


internal class DataInfo
{
    public decimal LastMonthEfficiency { get; set; }
    public decimal CurrentEfficiency { get; set; }
    public decimal CurrentMonthEfficiency { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public DateTime CurentDate { get; set; }
    public TimeOnly ProcessedTime { get; set; }
    public DateTime LastWriteDateSourceFile { get; set; }
}

