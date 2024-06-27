using System;

namespace CustomExceptions
{
	public class TimeEntryExeption : Exception
	{
		public TimeEntryExeption(string message)
			: base(message)
		{
		}
	}
}