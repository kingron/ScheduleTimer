using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScheduleTimer;
using System.Timers;

namespace ScheduleTimer
{
    class Program
    {

        static void Logs(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(DateTime.Now + ": ST1 Trigger");
        }
        static void ExpiredOfDay(object sender, ElapsedEventArgs e)
        {
            ScheduleTimer st = (ScheduleTimer) sender;
            Console.WriteLine(DateTime.Now + st.Tag + "ST1 Expired!");
        }

        static void St2TimerExpired(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(DateTime.Now + ((ScheduleTimer) sender).Tag + "ST2 Expired!");
        }
        static void St2Log(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine(DateTime.Now + ((ScheduleTimer)sender).Tag + ": ST2 Triggered!");
        }
        static void Main(string[] args)
        {
            ScheduleTimer st = new ScheduleTimer();
            TimeSpan Start = new TimeSpan(8, 0, 0);
            TimeSpan End = new TimeSpan(23, 59, 59);
            st.Init(Start, End, 30, "cmd", "/c dir");
            st.Tag = "Schedule ST1: From 8 to 18 clock, run every 30 seconds";
            Console.WriteLine("Lanched time: " + DateTime.Now);
            Console.WriteLine(st.Tag);
            st.OnScheduleTimer += Logs;
            st.run();

            ScheduleTimer st2 = new ScheduleTimer();
            Console.WriteLine("Schedule ST2: From 12:10 to 12:12 clock, run every 20 seconds");
            st2.Init(new TimeSpan(13, 10, 0), new TimeSpan(13, 12, 0), 20, "", "");
            st2.OnScheduleTimer += St2Log;
            st2.OnExpiredOfDay += St2TimerExpired;
            st2.run();
            Console.ReadLine();
        }
    }
}
