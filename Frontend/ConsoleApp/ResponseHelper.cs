using GeoCoordinatePortable;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace FrontendCode
{
    class ResponseHelper
    {
        public static Users getdata()
        {
            string url = @"https://localhost:44394/geoservice";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json; charset=utf-8";


            string responseString;
            Users users = new Users();

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    responseString = reader.ReadToEnd();
                }


                users = JsonConvert.DeserializeObject<Users>(responseString);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Read data unsuccessful. Exception: " + e.Message);
            }

            return users;
        }

        public static string CreateResponse(Users users)
        {
            Response response = new Response();
            response.users = new ResponseUser[2];

            double dist = GetDistance(users.users[0].address.geo, users.users[1].address.geo);

            response.users[0] = CreateResponse(users.users[0], dist);
            response.users[1] = CreateResponse(users.users[1], dist);

            return JsonConvert.SerializeObject(response);
        }

        private static ResponseUser CreateResponse(User inputUser, double distance)
        {
            ResponseUser ru = new ResponseUser();

            ru.name = inputUser?.name;
            ru.address = inputUser?.address;
            ru.companyName = inputUser?.company?.name;
            ru.phone = inputUser?.phone;
            ru.distance = distance;

            return ru;
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
    }
}
