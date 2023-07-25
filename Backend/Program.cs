using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Backend.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Services.Configure<DatabaseContext.Settings>(
    builder.Configuration.GetSection(nameof(DatabaseContext)));

builder.Services.AddSingleton<DatabaseContext>();
Console.WriteLine(builder.Configuration["DatabaseContext:ConnectionString"]);


services.AddScoped<JwtService>();
services.AddAuthentication(x =>
{
  x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
  var secret = builder.Configuration["JwtConfig:secret"];
  if (string.IsNullOrEmpty(secret))
  {
    throw new InvalidOperationException("JwtConfig:secret is not set in the configuration");
  }

  x.TokenValidationParameters = new TokenValidationParameters
  {
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
    ValidateIssuer = false,
    ValidateAudience = false
  };
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
  options.AddPolicy("MyAllowedOrigins",
      policy =>
      {
        policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod();
      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseCors();
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("MyAllowedOrigins");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
