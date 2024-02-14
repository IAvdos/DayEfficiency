using System.Globalization;
using System.Text;
using DayEfficiency;



public class TextGenerator
{
    public string BuildRecord(decimal hours, int freeDays, bool isNewMonth)
    {
        var record = new StringBuilder();
        if (isNewMonth)
        {
            record.Append(BuildNewMonthLine());
        }
        record.Append("\t");
        record.Append(String.Format(CultureInfo.GetCultureInfo("ru-RU"),"{0:0.00}", hours));
        if(freeDays > 0)
        {
            record.Append("\t");
        }
        record.Append(GetWhiteSpace(freeDays));

        return record.ToString();
    }

    public string BuildWhiteSpaceRecord(int freeDays)
    {
        StringBuilder record = new StringBuilder();
        record.Append("\t");
        record.Append(GetWhiteSpace(freeDays));

        return record.ToString();
    }

    public string BuildWhiteSpaceInNewMonthLine(int freeDays)
    {
        StringBuilder record = new StringBuilder();
        record.Append(BuildNewMonthLine());
        record.Append("\t");
        record.Append(GetWhiteSpace(freeDays));
        return record.ToString();
    }

    public string BuildFirstRecord(decimal hours, int startDay)
    {
        StringBuilder record = new StringBuilder();
        record.Append(BuildHeadLine());
        record.Append(BuildNewMonthLine());
        record.Append("\t");
        record.Append(GetWhiteSpace(startDay - 1));
        record.Append("\t");
        record.Append(String.Format(CultureInfo.GetCultureInfo("ru-RU"),"{0:0.00}", hours));

        return record.ToString();
    }
    
    private string BuildNewMonthLine()
    {
        string date = DateTime.Now.Date.ToString("yyyy\tMM");
      
        return  "\r\n" + date;
    }

    private string BuildHeadLine()
    {
        const int _dayInMonth = 31;
        const string _title = "year\tmonth\t";
        StringBuilder head = new StringBuilder(_title);

        for (int i = 1; i <= _dayInMonth; i++)
        {
            if (i == _dayInMonth)
            {
                head.Append(i);
            }
            else
            { 
            head.Append(i + "\t");
            }
        }
        return head.ToString();
    }

    private string GetWhiteSpace(int days)
    {
        string _zeroHours = "0,00";
        StringBuilder _whiteSpace = new StringBuilder();
        for(int i = 1; i <= days; i++)
        {
            _whiteSpace.Append(_zeroHours);
            if (i != days)
            {
                _whiteSpace.Append("\t"); 
            }
        }
        return _whiteSpace.ToString();
    }
}

