﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using GeoCoordinatePortable;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace BackendService
{
    public class DataHelper
    {
        static string url = @"https://jsonplaceholder.typicode.com/users";

        public static Users Getdata()
        {
            //ToDo: implement some caching capability and make ReadSource private
            throw new NotImplementedException();
        }
       
        public static Users ReadSource()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";

           
            string responseString;

            //ToDo: Add exception handling 

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseString = reader.ReadToEnd();
                }

            Users users = new Users();
            users.users = JsonConvert.DeserializeObject<User[]>(responseString);                

            return users;
        }

        public static string FindFarthest(Users allUsers)
        {
            double max = Double.MinValue;
            int[] ids = new int[2];

            for(int u= 0; u < allUsers.users.Length; u++)//.users)
            {
                Geo loc = allUsers.users[u].address.geo;
                for(int i=0; i<allUsers.users.Length; i++)
                {
                    double dist = GetDistance(loc, allUsers.users[i].address.geo);
                    if(dist > max)
                    {
                        max = dist;
                        ids[0] = u;
                        ids[1] = i;
                    }
                }
            }

            Response response = new Response();
            response.users = new ResponseUser[2];

            response.users[0] = CreateResponse(allUsers.users[ids[0]], max);
            response.users[1] = CreateResponse(allUsers.users[ids[1]], max);

            return JsonConvert.SerializeObject(response);
        }

        private static double GetDistance(Geo l1, Geo l2)
        {
            double lat1 = double.Parse(l1.lat);
            double lon1 = double.Parse(l1.lng);
            double lat2 = double.Parse(l2.lat);
            double lon2 = double.Parse(l2.lng);

            if (lat1 == lat2 && lon1 == lon2) 
                return 0;

            return new GeoCoordinate(lat1, lon1).GetDistanceTo(new GeoCoordinate(lat2, lon2));
        }

        private static ResponseUser CreateResponse(User inputUser, double distance)
        {
            ResponseUser ru = new ResponseUser();
            ru.name = inputUser.name;
            ru.address = inputUser.address;
            ru.companyName = inputUser.company.name;
            ru.phone = inputUser.phone;
            ru.distance = distance;

            return ru;
        }
    }
}