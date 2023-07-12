
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
  [ApiController]
  [Route("/api/")]
  public class UsersController : Controller
  {
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public UsersController(AppDbContext context, JwtService jwtService)
    {
      _context = context;
      _jwtService = jwtService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User loginData)
    {
      var user = _context.Users.FirstOrDefault(u => u.Email == loginData.Email && u.Password == loginData.Password);

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
      if (_context.Users.Any(u => u.Email == signupData.Email))
      {
        return BadRequest("Email already exists");
      }

      _context.Users.Add(signupData);
      _context.SaveChanges();

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