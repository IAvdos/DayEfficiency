using System;
using System.Text;
using DayEfficiency;

internal class DestinationFile
{
    private string? _filePaht;
    private DestinationFile() { }

    public DestinationFile(string filePaht)
    {
        if (File.Exists(filePaht))
        {
            _filePaht = filePaht;
        }
        else
        {
            Console.WriteLine("Путь к файлу для записи, неверен. Измените путь и перезапустите приложение.");
        }
    }

    public void WriteRecord(string value)
    {
        if (IsFileFree())
        {
            using (var writer = new StreamWriter(_filePaht, true, Encoding.UTF8))
            {
                writer.Write(value);
            }
        }
        else
        {
            Console.WriteLine("Destination file busy.");
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
            Console.WriteLine("Файл недоступен для записи. Закройте файл, либо измените свойство файла (Только для чтения).");
            return false;
        }
    }

}

