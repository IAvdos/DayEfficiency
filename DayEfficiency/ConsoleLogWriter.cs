using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayEfficiency
{
	public class ConsoleLogWriter : ILogWriter
	{
		public void WriteMessege(string messege)
		{
			Console.WriteLine(messege);
		}
	}
}
