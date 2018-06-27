/*
 * The Simple Schedule Timer
 * Simple, Easy, Fast, High Performance to run task repeated task every day.
 * For example, run tasks every hour every day from 08:00 to 18:00
 * Copyright Kingron, 2018
 */

using System;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace ScheduleTimer
{
    class ScheduleTimer
    {
        private string Command { get; set; }
        private string Arguments { get; set; }

        private DateTime scheduledTime;
        private System.Timers.Timer timer = null;

        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public int Interval { get; set; }
        public string Tag { get; set; }
        public event ElapsedEventHandler OnScheduleTimer = null;
        public event ElapsedEventHandler OnExpiredOfDay = null;


        public void Init(TimeSpan start, TimeSpan end, int interval_seconds, string command, string arguments)
        {
            Start = start;
            End = end;
            Interval = interval_seconds;
            Command = command;
            Arguments = arguments;

            scheduledTime = DateTime.Today + start;
            timer = new System.Timers.Timer();
            timer.Elapsed += OnTimer;
        }

        public void Stop()
        {
            if (timer != null) timer.Stop();
        }

        public void run()
        {
            if (timer == null) return;

            timer.Stop();

            // 如果开始时间已过，不断计算直到超过当前时间
            while (scheduledTime < DateTime.Now)
            {
                scheduledTime = scheduledTime.AddSeconds(Interval);
            }
            timer.Interval = (double)(scheduledTime - DateTime.Now).TotalMilliseconds;
            timer.Start();
            scheduledTime = scheduledTime.AddSeconds(Interval);
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            DateTime now = DateTime.Now;

            // 判断是否当天是否到达开始时间
            if (now < now.Date + Start)
            {
                scheduledTime = now.Date + Start;
            }
            else if (now <= now.Date + End)
            {
                // Time between Start & End, should do something
                // 时间是在推送开始和结束时间范围内，需要调用事件
                // 在这里写调用代码
                if (Command != "" && Arguments != "")
                {
                    Process push = new Process();
                    push.StartInfo.UseShellExecute = false;
                    push.StartInfo.FileName = Command;
                    push.StartInfo.CreateNoWindow = true;
                    push.StartInfo.WorkingDirectory = Application.StartupPath;
                    push.StartInfo.Arguments = Arguments;
                    push.Start();
                }
                OnScheduleTimer?.Invoke(this, e);
            }
            else
            {
                // 今天过期
                OnExpiredOfDay?.Invoke(this, e);
            }
            run();
        }
    }
}
