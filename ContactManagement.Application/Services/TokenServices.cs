using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ContactManagement.Application.Configurations;
using ContactManagement.Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ContactManagement.Application.Services;

public class TokenServices
{
    private readonly ConfigurationTokenSettings _settings;

    public TokenServices(IOptions<ConfigurationTokenSettings> settings)
    {
        _settings = settings.Value;
    }
    public string GenerateToken(User user)
    {
        //Create an instance for JwtSecurityTokenHandler
        var handler = new JwtSecurityTokenHandler();
        
       //Get the privateKey from appSettings and enconding it
        var key = Encoding.ASCII.GetBytes(_settings.Key);

    
        // Create the credentiasl
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = DateTime.UtcNow.AddHours(1),
        };
        
        var token = handler.CreateToken(tokenDescriptor);
        
        return handler.WriteToken(token);
        
    }
}