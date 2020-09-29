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
            Users users = DataHelper.ReadSource();
            return DataHelper.FindFarthest(users);
        }
    }
}
