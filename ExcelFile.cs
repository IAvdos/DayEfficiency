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
        private int _cellColumnNumber = ConfigData.CellColumnNumber - 1;
        private int _cellRowNumber = ConfigData.CellRowNumber - 1;

        public double CellValue { get { return _cellValue; } }

        public double GetCellValue()
        {

            using(FastExcel.FastExcel excel = new FastExcel.FastExcel(_sourseExcel, true))
            {
                var worksheet = excel.Read(1);
                

                
                var rows = worksheet.Rows.ToArray();
                
                var cell = rows[_cellRowNumber].Cells.ToArray()[_cellColumnNumber];
                _cellValue =  Double.Parse(cell.Value.ToString().Substring(0,5));
                Console.WriteLine(_cellValue);
            }
            return 0;
        }
        
    }
}
