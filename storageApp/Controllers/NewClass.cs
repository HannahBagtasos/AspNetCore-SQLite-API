using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;


namespace storageApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        [HttpPost("query")]
        public ActionResult query()
        {
            {
                var data = new List<List<string>>
            {
            new List<string> { "value 1", "value 2" },
            new List<string> { "value 3", "value 4" }
            };

                var results = new { Data = data };

                return Ok();
            }
        }

    }
}