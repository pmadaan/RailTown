﻿using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;

namespace FrontendCode
{
    class Program
    {
        private static System.Timers.Timer timer;
        private static StreamWriter output;

        static void Main(string[] args)
        {
            int key = -1;
            while (true)
            {
                while (key < 0)
                    key = ReadInput();

                switch (key)
                {
                    case 1:
                        Console.WriteLine(onDemand());
                        Console.ReadLine();
                        break;

                    case 2:
                        timed();
                        break;

                    case 3: return;
                }

                key = ReadInput();
            }

        }

        private static int ReadInput()
        {
            Console.Clear();
            Console.WriteLine("Please select one (Enter your selection): ");
            Console.WriteLine("1. One time execution");
            Console.WriteLine("2. Repeated execution");
            Console.WriteLine("3. Exit");

            int response = int.Parse(Console.ReadLine());
            if (response != 1 && response != 2 && response != 3)
            {
                Console.WriteLine("Invalid input");
                return -1;
            }

            return response;
        }

       private static string onDemand()
        {
            string result = findFarthestUsers();
            //return result;

            //Processing the result here for demonstration purposes
            Response ru = JsonConvert.DeserializeObject<Response>(result);
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("Farthest users: {0}, {1}", ru.users[0].name, ru.users[1].name));
            sb.Append(string.Format("\nDistnace in meters: {0:0.00}", ru.users[0].distance));

            return sb.ToString();
        }

        private static void timed()
        {
             Console.WriteLine("Please enter time interval in minutes: ");
            double minutes;

            while (!double.TryParse(Console.ReadLine(), out minutes))
            {
                Console.WriteLine("Invalid input.");
                Console.WriteLine("Please enter time interval in minutes: ");
            }

            output = new StreamWriter("users.tsv");
            output.AutoFlush = true;

            double milliseconds = TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            SetTimer(milliseconds);
            Console.WriteLine("Executing every {0} minutes. Press enter key at any time to terminate", minutes);
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
