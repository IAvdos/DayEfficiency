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
        private ExcelFile _excelFile = null;
        private DestinationFile _txtFile = null;
        private TextGenerator _textGenerator = null;
        private DataInfo _dataInfo = null;        
        
        public Presenter()
        {
            _excelFile = new ExcelFile(ConfigData.SourceFile);
            _txtFile = new DestinationFile(ConfigData.DestinationFile);
            _dataInfo = new DataInfoGenerator().CreateDataInfo();
            _textGenerator = new TextGenerator();            
        }
        public bool DefineAndExecuteStrategy()
        {
            //if()
            if (_dataInfo.LastUpdateDate == new DateTime(2023, 01, 01) && _dataInfo.LastMonthEfficiency == 0)
            {
                ExecuteFirstLounchStrategy();
                return true;
            }
            else if (_dataInfo.LastUpdateDate.Month == _dataInfo.CurentDate.Month)
            {
                ExecuteActualMonthStrategy();
                return true;
            }
            else if (_dataInfo.LastUpdateDate.Month < _dataInfo.CurentDate.Month)
            {
                if (_dataInfo.LastUpdateDate.Day != DateTime.DaysInMonth(_dataInfo.LastUpdateDate.Year, _dataInfo.LastUpdateDate.Month))
                {
                    ExecuteNewMonthWithRecordInLastStrategy();
                    return true;
                }
                else
                {
                    ExecuteNewMonthStrategy();
                    return true;
                }                
            }
            else
                { return false; }
        }
            private void UpdeteConfig(DateTime currentDate, double currentEfficiency)
            {
                ConfigData.UpdateConfig(ConfigKeys.lastProcessedDate, currentDate.ToString());
                ConfigData.UpdateConfig(ConfigKeys.lastCellValue, currentEfficiency.ToString());
            }
            private void ExecuteFirstLounchStrategy()
        {
			Console.WriteLine("It's firs program lounch.");
			_txtFile.WriteRecord(_textGenerator.BuildFirstRecord(0, _dataInfo.CurentDate.Day));
			UpdeteConfig(_dataInfo.CurentDate, _dataInfo.CurrentMonthEfficiency);
		}
            private void ExecuteActualMonthStrategy()
            {
                UpdeteConfig(_dataInfo.CurentDate, _dataInfo.CurrentMonthEfficiency);
                int freeDays = _dataInfo.CurentDate.Day - _dataInfo.LastUpdateDate.Day - 1;
                _txtFile.WriteRecord(_textGenerator.BuildRecord(_dataInfo.CurrentEfficiency, freeDays, false));
                UpdeteConfig(_dataInfo.CurentDate, _dataInfo.CurrentEfficiency);
            }
            private void ExecuteNewMonthWithRecordInLastStrategy()
            {
                int freeDaysInLastMonth = DateTime.DaysInMonth(_dataInfo.LastUpdateDate.Year, _dataInfo.LastUpdateDate.Month) - _dataInfo.LastUpdateDate.Day;

                _txtFile.WriteRecord(_textGenerator.BuildWhiteSpaceRecord(freeDaysInLastMonth));
                ExecuteNewMonthStrategy();
            }
            private void ExecuteNewMonthStrategy()
            {
			    if(_dataInfo.CurentDate.Day != 1)
                {
                    int freeDaysInNewMonth = _dataInfo.CurentDate.Day - 1;
                    _txtFile.WriteRecord(_textGenerator.BuildWhiteSpaceInNewMonthLine(freeDaysInNewMonth));
				    _txtFile.WriteRecord(_textGenerator.BuildRecord(_dataInfo.CurrentMonthEfficiency, 0, false));
			    }
                else 
                {
                    _txtFile.WriteRecord(_textGenerator.BuildRecord(_dataInfo.CurrentMonthEfficiency, 0, true));
                }
                UpdeteConfig(_dataInfo.CurentDate, _dataInfo.CurrentMonthEfficiency);
            }
        }
		
	}
