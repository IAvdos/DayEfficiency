using DayEfficiency;

public class ExceleFileReaderTests
{
	ExcelFileReader _excelFileReader;
	//"F:\\Development\\DayEfficiency\\DayEfficiency.Tests\\TestResources\\ExcelTest.xlsx"
	string _excelFile = ".\\..\\..\\..\\TestResources\\ExcelTest.xlsx";

	public ExceleFileReaderTests() 
	{
		_excelFileReader = new ExcelFileReader(new ConsoleLogWriter());
	}

	[Fact]
	public void TryReadFromExcelFile_False_FileNotFound()
	{
		var result = _excelFileReader.TryReadFromExcelFile("C:\\feiled file name.xlsx", "A1");


		Assert.False(result);
	}
	
	[Fact]
	public void TryReadFromExcelFile_CorrectResult()
	{
		_excelFileReader.TryReadFromExcelFile(_excelFile, "A1");
		var result = _excelFileReader.CellValue;


		Assert.Equal(11.22m, result);
	}

	[Fact]
	public void TryReadFromExcelFile_NotNull()
	{
		_excelFileReader.TryReadFromExcelFile(_excelFile, "E1");
		var result = _excelFileReader.CellValue;


		Assert.Equal(-1m, result);
	}
}
