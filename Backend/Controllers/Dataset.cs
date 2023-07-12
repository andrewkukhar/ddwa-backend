using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Backend.Models;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/dataset")]
    public class DataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DataController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetData()
        {
            var data = new List<DataModel>
            {
                new DataModel { Id = 1, Name = "Data 1" },
                new DataModel { Id = 2, Name = "Data 2" },
                new DataModel { Id = 3, Name = "Data 3" }
            };

            return Ok(data);
        }
    }
}
