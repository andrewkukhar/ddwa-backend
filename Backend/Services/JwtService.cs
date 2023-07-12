using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
  private readonly string? _secret;
  private readonly string? _expDate;

  public JwtService(IConfiguration config)
  {
    _secret = config.GetSection("JwtConfig").GetSection("secret").Value;
    if (_secret == null)
    {
      throw new InvalidOperationException("JwtConfig:secret is not set in the configuration");
    }

    _expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
    if (_expDate == null)
    {
      throw new InvalidOperationException("JwtConfig:expirationInMinutes is not set in the configuration");
    }
  }

  public string GenerateSecurityToken(string email)
  {
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = Encoding.ASCII.GetBytes(_secret!);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, email)
        }),
      Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expDate!)),
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
  }
}
