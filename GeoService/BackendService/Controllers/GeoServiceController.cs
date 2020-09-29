using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GeoServiceController : ControllerBase
    {        
        [HttpGet]
        public string GetFarthestUsers()
        {
            //read data from the source
            Users users = DataHelper.ReadSource();

            //find the users that are farthest from each other
            return DataHelper.FindFarthest(users);
        }
    }
}
