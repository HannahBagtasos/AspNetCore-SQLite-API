using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using storageApp.Data;
using storageApp.DTO;
using Microsoft.EntityFrameworkCore;

namespace storageApp.Controllers
{
    [Route("api/[controller]")] //path of application => api/query
    [ApiController] //sdk utilizes 
    public class queryController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult Get()
        //{
        //            return Ok("Test run success");
        // }

        private readonly myDbContext _context;
        //constructor
        public queryController(myDbContext context)
        {
            _context = context;
        }

        //retrieving all data
        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var data = await _context.Logs.ToListAsync();
            //send back the data
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> getFromSqlRaw(queryWriteDTO data)
        {

            //fetch log data
            try
            {
             

                if (ModelState.IsValid)
                {
                    //im missing types
                    var result = _context.logData.FromSqlRaw(data.Name);
                    await _context.SaveChangesAsync();
                    var response = new queryResultReadListDTO
                    {
                        data = new queryResultReadDTO
                        {
                            value1 = result.Timestamp,
                            value2 = result.Level
                        }.ToList()
                    };
                    return Ok(response);
                }
                return BadRequest("Invalid");


            }
            catch (System.Exception)
            {
                throw;
            }
            
        }




    }
}