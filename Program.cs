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
            var excel = new ExcelFile();
            excel.GetCellValue();

           
        }
    }
}