using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DayEfficiency
{
    internal class Presenter
    {
        private double _currentDayEfficiency = 0;
        private ExcelFile _excelFile = null;
        private DestinationFile _txtFile = null;
        private TextGenerator _textGenerator = null;
        private DateTime _currentDate = DateTime.Now;
        private DateTime _lastExecutingDate = ConfigData.LastProcessedDate;
        private DateTime _sourceFileUpdatingDate;

        public Presenter() 
        {
            _excelFile = new ExcelFile(ConfigData.SourceFile);
            _txtFile = new DestinationFile(ConfigData.DestinationFile);   
            _sourceFileUpdatingDate = _excelFile.GetLastUpdeteDate();
            _excelFile.GetCellValue(ConfigKeys.source_file.ToString());
            _currentDayEfficiency = _excelFile.CellValue - ConfigData.LastCellValue;
        }
        public string ReadStatus()
        {
            if(_currentDate.Hour > 4)
                //ghbljf


            if (ConfigData.LastProcessedDate == new DateTime(2023, 01, 01))
                return "First executing";
            else if ((_currentDate.Day - _lastExecutingDate.Day) == 1 && (_currentDate.Day != 1))
                return "Regular executing";
            else if ((_currentDate.Subtract(_lastExecutingDate).Days == 1))
                return "Regular new month executing";
            else if (( _currentDate.Day - _lastExecutingDate.Day) > 1)
                return "Executing with free days";
            else if ((_currentDate.Day - _lastExecutingDate.Day) < 0)
                return "Executing with free days and new month";
            return "Somthing wrong";
        }

        public void ProduceFirstTime()
        {
            UpdateConfiguration();          

            Console.WriteLine("Первый запуск приложения, успешно произведен.");
        }
        private void Regular()
        {
            _txtFile.WriteRecord(_textGenerator.BuildRecord(_currentDayEfficiency, 0, false));
            UpdateConfiguration();
        }

        private void RegularNewMonth()
        {
            _txtFile.WriteRecord(_textGenerator.BuildRecord(_currentDayEfficiency, 0, true));
            UpdateConfiguration();
        }

        private void NewMonthWithFreeDays() 
        {
            _txtFile.WriteRecord(_textGenerator.BuildRecord(_currentDayEfficiency))
        }

        private void UpdateConfiguration()
        {
            ConfigData.UpdateConfig(ConfigKeys.last_processed_date, _currentDate.ToString());
            ConfigData.UpdateConfig(ConfigKeys.last_cell_value, _currentDayEfficiency.ToString());
        }

    }
}
