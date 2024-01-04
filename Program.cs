using System;
using System.Text;

namespace DayEfficiency;

class Program
{
    static void Main() 
    {
        Console.OutputEncoding = Encoding.UTF8;
        
        Presenter presenter = new Presenter();
                
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
				Console.WriteLine("Wrong start time program. Please lounch it between 5:00 and 24:00");
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