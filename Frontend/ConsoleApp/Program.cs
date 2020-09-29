using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Timers;

namespace FrontendCode
{
    class Program
    {
        private static System.Timers.Timer timer;
        private static StreamWriter output;

        static void Main(string[] args)
        {
            string s1 = onDemand();
            Console.WriteLine(s1);

            Console.WriteLine("Starting timed event");
            timed();
        }

       private static string onDemand()
        {
           return  findFarthestUsers();
        }

        private static void timed()
        {
            output = new StreamWriter("users.tsv");
            output.AutoFlush = true;

            double milliseconds = TimeSpan.FromSeconds(30).TotalMilliseconds;
            SetTimer(milliseconds);
            Console.WriteLine("press enter key to terminate");
            Console.ReadLine();
            timer.Stop();
            timer.Dispose();
            output.Close();
        }

        private static void SetTimer(double milliseconds)
        {
            timer = new System.Timers.Timer(milliseconds);
            timer.Elapsed += timedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void timedEvent(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Writing data to users.tsv at " + DateTime.Now);
            output.WriteLine(findFarthestUsers());
        }

        private static string findFarthestUsers()
        {
            Users farthestUsers = ResponseHelper.getdata();
            return ResponseHelper.CreateResponse(farthestUsers);
        }

    }
}
