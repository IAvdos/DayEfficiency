using DayEfficiency;

public class ExceleFileReaderTests
{
	private ExcelFileReader _excelFileReader;
	//"F:\\Development\\DayEfficiency\\DayEfficiency.Tests\\TestResources\\ExcelTest.xlsx"
	private string _excelFile = ".\\..\\..\\..\\TestResources\\ExcelTest.xlsx";

	public ExceleFileReaderTests() 
	{
		_excelFileReader = new ExcelFileReader(new ConsoleLogWriter());
	}
	[Fact]
	public void TryReadFromExcelFile_False_FileNotFound()
	{
		//Act
		var result = _excelFileReader.TryReadFromExcelFile("C:\\Users\\Avdos\\OneDrive\\fail.xlsx", "A1");
		//Assert
		Assert.False(result);
	}
	
	[Fact]
	public void TryReadFromExcelFile_CorrectResult()
	{
		//Act
		_excelFileReader.TryReadFromExcelFile(_excelFile, "A1");
		var result = _excelFileReader.CellValue;
		//Assert
		Assert.Equal(11.22m, result);
	}

	[Fact]
	public void TryReadFromExcelFile_NotNull()
	{
		//Act
		_excelFileReader.TryReadFromExcelFile(_excelFile, "E1");
		var result = _excelFileReader.CellValue;
		//Assert
		Assert.Equal(-1m, result);
	}
}
