using System;
using System.IO;

namespace DayEfficiency;

class Program
{
    static void Main() 
    {/*
        DataInfo dataInfo = new DataInfo
        {
            CurentDate = new DateTime(2023,08,03,15,00,00),
            LastUpdateDate = new DateTime(2023, 08, 03),
            LastMonthEfficiency = 19.12,
            CurrentEfficiency = 3,
            CurrentMonthEfficiency = 22.12
        };
*/
        Presenter presenter = new Presenter();
        //Presenter presenter = new Presenter(dataInfo);
        if(presenter.IsSourseFileChenged() || presenter.IsItFirstLounch())
        {
            if (presenter.IsProcessedTime())
            {
                var start = presenter.DefineAndExecuteStrategy();
				if (start)
				{
					Console.WriteLine("Program completed successfully.");
				}
				else
				{
					Console.WriteLine("Program failed.");
				}
			}
            else
            {
				Console.WriteLine("Wrong start time profram. Please lounch it between 5:00 and 24:00");
			}
        }
        else
        {
            Console.WriteLine("Data in source file is not changed.");
        } 
        Console.WriteLine("Push any button.");
        Console.ReadKey();
    }
}