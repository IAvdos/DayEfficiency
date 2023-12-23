using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using FastExcel;

namespace DayEfficiency
{
    internal class ExcelFile
    {
        private FileInfo _sourseExcel = new FileInfo(ConfigData.SourceFile);
        private double _cellValue = 0;
        private string _cellAddress = ConfigData.CellAddress;

        public double CellValue { get { return _cellValue; } }

        public double GetCellValue()
        {

            using(FastExcel.FastExcel excel = new FastExcel.FastExcel(_sourseExcel, true))
            {
                var worksheet = excel.Read(1);
                

                
                var rows = worksheet.Rows.ToArray();

                var cell = rows[17].Cells.ToArray()[2];
                Console.WriteLine(cell);
            }
            return 0;
        }
        
    }
}
