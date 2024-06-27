using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusiestBreakTimeTracker;
using System;
using CustomExceptions;
using System.Collections.Generic;

namespace BusiestBreakTimeTrackerTests
{
    [TestClass]
    public class BreakTimeTrackerTests
    {


        [TestMethod]
        public void OneBusiestInteval()
        {

            BreakTimeTracker breakTimeTracker = new BreakTimeTracker();

            string[] times = new string[] { "10:30-11:35",
                                            "10:15-11:15", 
                                            "11:20-11:50", 
                                            "10:35-11:40",
                                            "10:20-11:20" };

            foreach (string time in times)
            {
                breakTimeTracker.AddTimeInterval(StringToTimeInterval(time));
            }

            TimeInterval expected = new TimeInterval(StringToTime("10:35"), StringToTime("11:15"), 4);

            List<TimeInterval> actual = breakTimeTracker.FindBusiestIntervals();

            Assert.AreEqual(1, actual.Count, $"Found {actual.Count} busiest intervals, but expected 1");
            Assert.AreEqual(expected, actual[0], "Busiest interval not found correctly.");

        }

        [TestMethod]
        public void TwoBusiestIntervals()
        {

            BreakTimeTracker breakTimeTracker = new BreakTimeTracker();

            string[] times = new string[] { "10:30-11:35", 
                                            "10:15-11:15",
                                            "11:20-11:50",
                                            "11:35-11:40",
                                            "12:20-13:20" };

            foreach (string time in times)
            {
                breakTimeTracker.AddTimeInterval(StringToTimeInterval(time));
            }

            TimeInterval expected1 = new TimeInterval(StringToTime("10:30"), StringToTime("11:15"), 2);
            TimeInterval expected2 = new TimeInterval(StringToTime("11:35"), StringToTime("11:40"), 2);

            List<TimeInterval> actual = breakTimeTracker.FindBusiestIntervals();

            Assert.AreEqual(2, actual.Count, $"Found {actual.Count} busiest intervals, but expected 2");
            Assert.AreEqual(expected1, actual[0], "First busiest interval not found correctly.");
            Assert.AreEqual(expected2, actual[1], "Second busiest interval not found correctly.");


        }


        static DateTime StringToTime(string time)
        {
            return DateTime.ParseExact(time, "HH:mm", null);

        }

        // Parse entry into DateTimies.
        static (DateTime, DateTime) StringToTimeInterval(string timeEntry)
        {
            try
            {
                DateTime start = DateTime.ParseExact(timeEntry.Substring(0, 5), "HH:mm", null);
                DateTime end = DateTime.ParseExact(timeEntry.Substring(6), "HH:mm", null);
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
