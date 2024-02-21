using FastExcel;
using System.Globalization;
using DayEfficiency;
public class ExcelFileReader
{
    ExcelFileReader() { }
    public ExcelFileReader(ILogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    ILogWriter _logWriter;
    public decimal CellValue { get; set; }
    public DateTime CurrentUpdateDate { get; set; }
    decimal _cellValue = 0;
    int _cellColumnNumber = 0;
    int _cellRowNumber = 0;
 

	public bool TryReadFromExcelFile(string sourceExcelFilePath, string cellAddress)
	{
		if (File.Exists(sourceExcelFilePath))
		{
			var _sourseExcel = new FileInfo(sourceExcelFilePath);
			if (IsFileFree(_sourseExcel))
			{
                CurrentUpdateDate = _sourseExcel.LastWriteTime;
				CellValue = GetCellValue(_sourseExcel,  cellAddress);
                if(CellValue == -1)
                {
                    _logWriter.WriteMessege("Cell is't exist, please check CellName");
                    return false;
                }
				return true;
			}
			else
			{
				_logWriter.WriteMessege("Source excel file busy, please close file and relaunch DayEfficiency");
				return false;
			}
		}
		else
		{
			_logWriter.WriteMessege("Source excel file not exist, or incorect file paht.");
			return false;
		}
	}

	public decimal GetCellValue(FileInfo excelFile, string cellAddress)
    {
        ConvertCellAddress(cellAddress);

        using (FastExcel.FastExcel excel = new FastExcel.FastExcel(excelFile, true))
        {
            var worksheet = excel.Read(1);
            if(!IsCellExist(worksheet, cellAddress))
            {
                return -1m;
            }

            var rows = worksheet.Rows.ToArray();
            var cell = rows[_cellRowNumber].Cells.ToArray()[_cellColumnNumber];

            _cellValue = ParseCellValue(cell.ToString());
            _cellValue = Math.Round(_cellValue, 2);
        }

        return _cellValue; 
    }

	bool IsCellExist(Worksheet worksheet, string cellName)
	{
        var rows = worksheet.Rows;

        foreach (var row in rows)
        {
            if(row.RowNumber == _cellRowNumber + 1)
            {
                var cells = row.Cells;

                if (cells.Select(cell => cell.CellName).Contains(cellName))
                {
                    return true;
                }
            }
        }

        return false;
	}

	decimal ParseCellValue(string cellValue)
    {
        if(CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator == ".")
        {
            return Convert.ToDecimal(cellValue);
        }
        else
        {
            return Convert.ToDecimal(cellValue.Replace(".", ","));
        }
    }

    public bool IsFileFree(FileInfo sourceExcel)
    {
        try
        {
            var stream = new FileStream(sourceExcel.FullName, FileMode.Open, FileAccess.Write);
            stream.Close();
            return true;
        }
        catch (Exception ex)
        {
			_logWriter.WriteMessege("Файл с данными занят. Закройте файл и перезапустите приложение.");
        }

        return false;
    }

    void ConvertCellAddress(string cellAddress)
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

    int ConvertColumnLatterToItn(string columnLatter)
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

