using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayEfficiency
{
    internal class DataInfoGenerator
    {
        private DestinationFile _destinationFile = null;
        private ExcelFile _sourceFile = null;
        private DateTime _currentDate;

        public DataInfoGenerator()
        {
            ConfigData.ReadAppConfig();
            _destinationFile = new DestinationFile(ConfigData.DestinationFile);
            _sourceFile = new ExcelFile(ConfigData.SourceFile);
            _currentDate = DateTime.Now;
        }
        public DataInfo CreateDataInfo()
        {
            var dataInfo = new DataInfo()
            {
                CurentDate = _currentDate,
                LastUpdateDate = ConfigData.LastProcessedDate,
                CurrentEfficiency = CalculateCurrentEfficiency(),
                LastMonthEfficiency = ConfigData.LastCellValue,
                CurrentMonthEfficiency = _sourceFile.GetCellValue(ConfigData.CellAddress),
                ProcessedTime = ConfigData.ProcessedTime,
                LastWriteDateSourceFile = _sourceFile.GetLastUpdateDate()
            };
            return dataInfo;
        }
        
        private double CalculateCurrentEfficiency()
        {
            return _sourceFile.GetCellValue(ConfigData.CellAddress) - ConfigData.LastCellValue;
        }
        
    }
}
