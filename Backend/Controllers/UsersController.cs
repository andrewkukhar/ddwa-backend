using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Backend.Controllers
{
  [ApiController]
  [Route("/api/")]
  public class UsersController : Controller
  {
    private readonly DatabaseContext _context;
    private readonly JwtService _jwtService;

    public UsersController(DatabaseContext context, JwtService jwtService)
    {
      _context = context;
      _jwtService = jwtService;
    }

    [HttpGet]
    public async Task<IEnumerable<User>> Get()
    {
      return await _context.Users.Find(_ => true).ToListAsync();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User loginData)
    {
      var user = _context.Users.Find(u => u.Email == loginData.Email && u.Password == loginData.Password).FirstOrDefault();

      if (user == null)
      {
        return Unauthorized();
      }

      var token = _jwtService.GenerateSecurityToken(user.Email);

      var response = new
      {
        token,
        user.Id,
        user.Email,
      };

      return Ok(response);
    }

    [HttpPost("signup")]
    public IActionResult Signup([FromBody] User signupData)
    {
      if (_context.Users.Find(u => u.Email == signupData.Email).Any())
      {
        return BadRequest("Email already exists");
      }

      _context.Users.InsertOne(signupData);

      var token = _jwtService.GenerateSecurityToken(signupData.Email);

      var response = new
      {
        token,
        signupData.Id,
        signupData.Email,
      };

      return Ok(response);
    }
  }
}
