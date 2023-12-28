using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayEfficiency
{
    internal class DestinationFile
    {
        private string? _filePaht;
        private DestinationFile() { }
        
        public DestinationFile(string filePaht)
        {
            if(File.Exists(filePaht)) _filePaht = filePaht;
            else Console.WriteLine("Путь к файлу для записи, неверен. Измените путь и перезапустите приложение.");
            FileInfo file = new FileInfo(filePaht);
        }


        public void WriteRecord(string value)
        {
            IsFileFree();
            using (var writer = new StreamWriter(_filePaht, true))
            {
                writer.Write(value);
            }

        }
        //не факт что отслеживает открытые текстовые файлы
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
}
