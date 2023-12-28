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
        private FileInfo _sourseExcel = null;
        private double _cellValue = 0;
        private int _cellColumnNumber = 0;
        private int _cellRowNumber = 0;
        public double CellValue { get { return _cellValue; } }

        public ExcelFile(string filePaht)
        {
            if (File.Exists(filePaht)) _sourseExcel = new FileInfo(filePaht);
            else Console.WriteLine("Путь к файлу с данными, неверен. Измените путь и перезапустите приложение.");
        }        

        public DateTime GetLastUpdeteDate() { return _sourseExcel.LastWriteTime; }
        
        public double GetCellValue(string cellAddress)
        {
            ConvertCellAddress(cellAddress);

            if (IsFileFree())
            {
                using (FastExcel.FastExcel excel = new FastExcel.FastExcel(_sourseExcel, true))
                {
                    var worksheet = excel.Read(1);

                    var rows = worksheet.Rows.ToArray();

                    var cell = rows[_cellRowNumber].Cells.ToArray()[_cellColumnNumber];
                    _cellValue = Double.Parse(cell.Value.ToString().Replace('.', ','));
                    _cellValue = Math.Round(_cellValue, 2);
                }
            }
           
            return _cellValue;
        }

        private bool IsFileFree()
        {
            try
            {
                var stream = new FileStream(_sourseExcel.FullName, FileMode.Open, FileAccess.Write);
                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Файл с данными занят. Закройте файл и перезапустите приложение.");
                return false;
            }
        }
        private void ConvertCellAddress(string cellAddress)
        {
            int _columnNumber = 0;
            int _rowNumber = 0;
            string _letterColun = null;


            for (int i = 0; i < cellAddress.Length; i++)
            {
                if (char.IsLetter(cellAddress[i])) _letterColun += cellAddress[i];
                else
                {
                    _rowNumber = Convert.ToInt32(cellAddress.Substring(i));
                    break;
                }
            }
            _columnNumber = ConvertColumnLatterToItn(_letterColun.ToUpper());

            _cellColumnNumber = _columnNumber - 1;
            _cellRowNumber = _rowNumber - 1;

        }
        private int ConvertColumnLatterToItn(string columnLatter)
        {
            double _lettersInAlphabet = 26;
            double _columnNumber = 0;

            for (int i = columnLatter.Length - 1; i >= 0; i--)
            {
                double rank = columnLatter.Length - i - 1;


                _columnNumber += ((int)columnLatter[i] - (int)'A' + 1) * Math.Pow(_lettersInAlphabet, rank);
            }
            return (int)_columnNumber;
        }

    }
}
