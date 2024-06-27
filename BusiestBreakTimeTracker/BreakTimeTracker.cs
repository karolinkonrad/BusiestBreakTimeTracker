
using System;
using System.Collections.Generic;
using System.Linq;


namespace BusiestBreakTimeTracker
{
    // Logic for finding the busiest time intervals for breaks.
    public class BreakTimeTracker
    {
        private List<(DateTime time, int type)> timePoints = new List<(DateTime time, int type)>();


        // Save break time.
        public void AddTimeInterval((DateTime start, DateTime end) entry)
        {
            timePoints.Add((entry.start, 1));
            timePoints.Add((entry.end, -1));
        }

        // Find the time intervals where the most drivers where taking a break.
        public List<TimeInterval> FindBusiestIntervals()
        {
            timePoints = timePoints.OrderBy(x => x.time).ThenBy(x => x.type).ToList();

            List<TimeInterval> busiestIntervals = new List<TimeInterval>();
            TimeInterval busiestInterval = new TimeInterval();

            int currentDrivers = 0;
            DateTime lastTime = timePoints[0].time;


            foreach (var (time, type) in timePoints)
            {

                if (currentDrivers > busiestInterval.drivers)
                {
                    busiestIntervals.Clear();
                    busiestInterval.drivers = currentDrivers;
                    busiestInterval.start = lastTime;
                    busiestInterval.end = lastTime;
                }
                if (currentDrivers == busiestInterval.drivers)
                {
                    // There is a new time period with the same number of drivers.
                    if (busiestInterval.end != lastTime)
                    {
                        busiestIntervals.Add(busiestInterval);

                        busiestInterval = new TimeInterval();
                        busiestInterval.drivers = currentDrivers;
                        busiestInterval.start = time;
                    }
                    busiestInterval.end = time;                    
                }
                currentDrivers += type;
                lastTime = time;
            }

            busiestIntervals.Add(busiestInterval);

            return busiestIntervals;
        }
    }

    // Structure for storing data of a timeinterval.
    public struct TimeInterval
    {
        public DateTime start;
        public DateTime end;
        public int drivers;

        public TimeInterval(DateTime start, DateTime end, int drivers)
        {
            this.start = start;
            this.end = end;
            this.drivers = drivers;
        }

        
        public override string ToString()
        {
            return $"{start:HH:mm} - {end:HH:mm}, {drivers}";
        }
    }
}
