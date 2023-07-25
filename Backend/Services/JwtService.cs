using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
  public class JwtService
  {
    private readonly string _secret;
    private readonly string _expDate;

    public JwtService(IConfiguration config)
    {
      _secret = config["JwtConfig:secret"] ?? throw new InvalidOperationException("JwtConfig:secret is not set in the configuration");
      _expDate = config["JwtConfig:expirationInMinutes"] ?? throw new InvalidOperationException("JwtConfig:expirationInMinutes is not set in the configuration");
    }

    public string GenerateSecurityToken(string email)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var secretKey = _secret.PadRight(16, '*');
      var key = Encoding.ASCII.GetBytes(secretKey);
      var signingKey = new SymmetricSecurityKey(key);
      var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[]
          {
                    new Claim(ClaimTypes.Name, email)
                }),
        Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate)),
        SigningCredentials = signingCredentials
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      return tokenHandler.WriteToken(token);
    }
  }
}
