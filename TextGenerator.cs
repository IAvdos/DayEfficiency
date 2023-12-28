using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayEfficiency
{
    internal class TextGenerator
    {
        public string BuildRecord(double hours, int freeDays, bool isNewMonth)
        {
            StringBuilder record = new StringBuilder();
            if (isNewMonth) record.Append(BuildNewMonthLine());
            record.Append("\t");
            record.Append(String.Format("{0:00.00}", hours));            
            record.Append(GetWhiteSpace(freeDays));                        

            return record.ToString();
        }
        public string BuildWhiteSpaceRecord(int freeDays) 
        {
            StringBuilder record = new StringBuilder();
            record.Append(BuildNewMonthLine());
            record.Append("\t");            
            record.Append(GetWhiteSpace(freeDays));

            return record.ToString();
        }
        public string BuildFirstRecord(double hours, int startDay)
        {
            StringBuilder record = new StringBuilder();
            record.Append(BuildHeadLine());
            record.Append(BuildNewMonthLine());
            record.Append(GetWhiteSpace(startDay - 1));
            record.Append("\t");
            record.Append(String.Format("{0:00.00}", hours));            

            return record.ToString();
        }
        
        private string BuildNewMonthLine()
        {
            const int _recordLength = 16;
            string date = DateTime.Now.Date.ToString("yyyyг MMMM");
          
            return date + new String(' ', _recordLength - date.Length);            
        }

        private string BuildHeadLine()
        {
            const int _dayInMonth = 31;
            const int _leftSidePadding = 18;
            string _whiteSpase = new String('+', 7);
            string _whiteSpaseSmall = new String('+', 6);
            StringBuilder head = new StringBuilder(new string(' ', _leftSidePadding));

            for(int i = 1; i <= _dayInMonth; i++)
            {               
                    head.Append(i);
                    if(i < 10)
                        head.Append(_whiteSpase);
                    else 
                        head.Append(_whiteSpaseSmall);
                if (i == _dayInMonth) head.Append("\n");             
                
            }

            return head.ToString();
        }

        private string GetWhiteSpace(int days)
        {
            StringBuilder _whiteSpace = new StringBuilder();
            for(int i = 1; i <= days; i++)
            {
                    _whiteSpace.Append(new String('+', 5));
                    if (i != days)
                        _whiteSpace.Append("\t");             
            }

            return _whiteSpace.ToString();
        }

        
    }
}
