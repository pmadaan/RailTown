using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendService
{
    public class Response
    {
        public ResponseUser[] users { get; set; }
    }
    
    public class ResponseUser
    {
        public string name { get; set; }
        public Address address { get; set; }
        public string companyName { get; set; }
        public string phone { get; set; }
        public double distance { get; set; }
       
    }
}
