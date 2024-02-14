using System.Text;
using DayEfficiency;

public class DestinationFile
{
    private string? _filePaht;
    private ILogWriter _logWriter;
    private DestinationFile() { }
    public DestinationFile(ILogWriter logWriter)
    {
        _logWriter = logWriter;
    }
    public bool IsFileExistAndFree(string filePaht)
    {
		if (File.Exists(filePaht))
		{
            _filePaht = filePaht;
            if (IsFileFree())
            {
				return true;
			}
			else
			{
				_logWriter.WriteMessege("Destination .txt file busy, please close file and relaunch DayEfficiency");
                return false;
			}
		}
		else
		{
			_logWriter.WriteMessege("Destination .txt file not exist, or incorect file paht.");
            return false;
		}
	}

    public void WriteRecord(string value)
    {
        using (var writer = new StreamWriter(_filePaht, true, Encoding.UTF8))
        {
            writer.Write(value);
        }
    }

    private bool IsFileFree()
    {
        try
        {
            var stream = new FileStream(_filePaht, FileMode.Open, FileAccess.Write);
            stream.Close();
            return true;
        } catch(Exception ex)
        {
			_logWriter.WriteMessege("Файл недоступен для записи. Закройте файл, либо измените свойство файла (Только для чтения).");
            return false;
        }
    }
}

