using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Backend.Controllers
{
  [ApiController]
  [Route("/api/")]
  public class DatasetController : Controller
  {
    private readonly DatabaseContext _context;

    public DatasetController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("dataset")]
    public async Task<IEnumerable<Dataset>> GetDatasets()
    {
      return await _context.Datasets.Find(_ => true).ToListAsync();
    }
  }
}
