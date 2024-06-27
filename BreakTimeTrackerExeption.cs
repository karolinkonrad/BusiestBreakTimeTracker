using System;

namespace CustomExceptions
{
	public class BreakTimeException : Exception
	{
		
		// Constructor that takes a custom message
		public BreakTimeException(string message)
			: base(message)
		{
		}
	}
}