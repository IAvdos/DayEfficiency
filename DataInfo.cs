using System;
using DayEfficiency;


internal class DataInfo
{
    public double LastMonthEfficiency { get; set; }
    public double CurrentEfficiency { get; set; }
    public double CurrentMonthEfficiency { get; set; }
    public DateTime LastUpdateDate { get; set; }
    public DateTime CurentDate { get; set; }        
    public TimeOnly ProcessedTime { get; set; }
    public DateTime LastWriteDateSourceFile { get; set; }
}

