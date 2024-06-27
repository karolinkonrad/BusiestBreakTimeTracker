using CustomExceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace BusiestBreakTimeTracker
{
    internal class Program
    {
        private static BreakTimeTracker breakTimeTracker = new BreakTimeTracker();

        // Command line application for finding the busiest time interval for breaks. 
        static void Main(string[] args)
        {
            // Read entries from file.
            if (args.Length == 2 && args[0] == "filename")
            {    
                try
                {
                    ReadFromFile(args[1]);
                    DisplayBusiestInterval(breakTimeTracker.FindBusiestIntervals());
                }
                catch (TimeEntryExeption ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            // Add entries one-by-one.
            while (true)
            {
                Console.WriteLine("<start time><end time> - add entry\nq - quit");

                string input = Console.ReadLine();
                if (input != null)
                {
                    if (input == "q")
                    {
                        break;
                    }

                    try
                    {
                        breakTimeTracker.AddTimeInterval(ParseEntry(input));
                        DisplayBusiestInterval(breakTimeTracker.FindBusiestIntervals());
                    }
                    catch (TimeEntryExeption ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
        
        // Print the busiest start and end time and the number of drivers taking a break.
        private static void DisplayBusiestInterval(List<TimeInterval> busiestTimeIntervals)
        {
            Console.WriteLine($"Busiest times:");
            foreach (var interval in busiestTimeIntervals)
            {
                Console.WriteLine($"{interval.start:HH:mm} - {interval.end:HH:mm}" +
                $" with {interval.drivers} drivers taking a break.");
                
            }
            Console.WriteLine($"----------------------------------");
        }

        // Read entries from file.
        static void ReadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new TimeEntryExeption("File not found.");
            }
            var lines = File.ReadLines(filePath);

            if (!lines.Any())
            {
                throw new TimeEntryExeption("File is empty.");
            }

            foreach (var line in lines)
            {
                breakTimeTracker.AddTimeInterval(ParseEntry(line));
            }            
        }

        // Parse entry into DateTimies.
        static (DateTime, DateTime) ParseEntry(string timeEntry)
        {
            try
            {
                DateTime start = DateTime.ParseExact(timeEntry.Substring(0, 5), "HH:mm", null);
                DateTime end = DateTime.ParseExact(timeEntry.Substring(5), "HH:mm", null);
                return (start, end);

            }
            catch (Exception)
            {
                throw new TimeEntryExeption($"Error parsing time entry: {timeEntry}\n" +
                    $"Time entry format is <start time><end time> (example 13:1514:00)");
            }
        }
    }
}
