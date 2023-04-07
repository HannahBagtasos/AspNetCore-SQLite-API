using Microsoft.AspNetCore.Mvc;
using storageApp.Data;
using Microsoft.EntityFrameworkCore;
using storageApp.Models;

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
        public async Task<IActionResult> createQuery(logData data)
        {
            //make sure our model is valid
            if (ModelState.IsValid)
            {
                //saves to memory of server
                await _context.Logs.AddAsync(data);
                //save those items to the db
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetData", new { data, Id }, data);
            }
            return new JsonResult("Something went wrong.") { StatusCode = 500 };
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> getSingleData(int id)
        {

            //match by id
            var data = await _context.Logs.FirstOrDefaultAsync(z => z.Id == id);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        //post to recieve query and one to execute back

    }
}