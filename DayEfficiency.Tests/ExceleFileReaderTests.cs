using DayEfficiency;
using DayEfficiency.Tests;

public class ExceleFileReaderTests
{
	private ExcelFileReader _excelFileReader;
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
		_excelFileReader.TryReadFromExcelFile("C:\\Users\\Avdos\\OneDrive\\Рабочий стол\\DayEfficiency\\TestResources\\ExcelTest.xlsx", "A1");
		var result = _excelFileReader.CellValue;
		//Assert
		Assert.Equal(11.22m, result);
	}

	[Fact]
	public void TryReadFromExcelFile_NotNull()
	{
		//Act
		_excelFileReader.TryReadFromExcelFile("C:\\Users\\Avdos\\OneDrive\\Рабочий стол\\DayEfficiency\\TestResources\\ExcelTest.xlsx", "E1");
		var result = _excelFileReader.CellValue;
		//Assert
		Assert.Equal(-1m, result);
	}
}
