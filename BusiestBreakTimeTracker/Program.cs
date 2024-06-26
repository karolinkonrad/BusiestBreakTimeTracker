using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace BusiestBreakTimeTracker
{
    internal class Program
    {
        static List<(DateTime time, int type)> timePoints = new List<(DateTime time, int type)>();

        static void Main(string[] args)
        {

            if (args.Length == 2 && args[0] == "filename")
            {
                if (ReadFromFile(args[1]))
                {
                    DisplayBusiestPeriod();
                }
            }

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

                    if (ParseEntry(input))
                    {
                        DisplayBusiestPeriod();
                    }

                }
            }
        }


        static bool ReadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found.");
                return false;
            }

            foreach (var line in File.ReadLines(filePath))
            {
                ParseEntry(line);
            }
            return timePoints.Any();
        }

        static bool ParseEntry(string timeEntry)
        {
            try
            {
                DateTime start = DateTime.ParseExact(timeEntry.Substring(0, 5), "HH:mm", null);
                DateTime end = DateTime.ParseExact(timeEntry.Substring(5), "HH:mm", null);
                timePoints.Add((start, 1));  // Start of break
                timePoints.Add((end, -1));   // End of break
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing time entry: {timeEntry}\n" +
                    $"Time entry format is <start time><end time> (example 13:1514:00)");
            }
            return false;
        }



        static void DisplayBusiestPeriod()
        {
            timePoints = timePoints.OrderBy(x => x.time).ThenBy(x => x.type).ToList();

            int maxDrivers = 0;
            int currentDrivers = 0;
            DateTime? busiestStart = null;
            DateTime? busiestEnd = null;

            DateTime lastTime = timePoints[0].time;

            foreach (var (time, type) in timePoints)
            {

                if (currentDrivers > maxDrivers)
                {
                    maxDrivers = currentDrivers;
                    busiestStart = lastTime;
                    busiestEnd = lastTime;
                }
                if (currentDrivers == maxDrivers && busiestEnd == lastTime)
                {
                    busiestEnd = time;
                }
                currentDrivers += type;
                lastTime = time;
            }

            Console.WriteLine($"Busiest time is {busiestStart:HH:mm} - {busiestEnd:HH:mm} with {maxDrivers} drivers taking a break.\n" +
                            $"----------------------------------");
        }
    }
}
