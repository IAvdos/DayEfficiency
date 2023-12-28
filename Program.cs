using System;
using System.IO;

namespace DayEfficiency
{
    class Program
    {
        static void Main() 
        {
            ConfigData.ReadAppConfig();
            ConfigData.UpdateConfig(ConfigKeys.last_processed_date, DateTime.Now.ToString());
            var excel = new ExcelFile(ConfigData.SourceFile);
           var value = excel.GetCellValue(ConfigData.CellAddress);
            
           DestinationFile file = new DestinationFile(ConfigData.DestinationFile);
            TextGenerator generator = new TextGenerator();
            file.WriteRecord(generator.BuildFirstRecord(value, 7));
            //file.WriteRecord(generator.BuildRecord(23.1, 0, false));
            Console.WriteLine(new DateTime(2023,12,01).Subtract(new DateTime(2023,11, 30)).Days);
        }
    }
}