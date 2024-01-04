using System;
using System.Text;

namespace DayEfficiency;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Presenter presenter = new Presenter();

        if(presenter.IsSourseFileChanged() || presenter.IsItFirstLaunch())
        {
            if (presenter.IsProcessedTime())
            {
                var isSuccsessfully = presenter.DefineAndExecuteStrategy();
				if (isSuccsessfully)
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
				Console.WriteLine("Wrong start time program. Please launch it between 5:00 and 24:00");
			}
        }
        else
        {
            Console.WriteLine("Data in source file is not changed.");
        } 
        Console.WriteLine("Push any button to exit.");
        Console.ReadKey();
    }
}