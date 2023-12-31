using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayEfficiency
{
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
}
