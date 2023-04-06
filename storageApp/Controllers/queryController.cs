using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace storageApp.Controllers
{
    [Route("api/[controller]")] //path of application => api/query
    [ApiController] //sdk utilizes 
    public class queryController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Test run success");
        }
    }
}