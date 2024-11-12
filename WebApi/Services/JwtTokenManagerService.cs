using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebApi.Dtos;
using WebApi.Interfaces;
using WebApi.Utilyties;

namespace WebApi.Services
{
    internal sealed class JwtTokenManagerService :IJwtTokenManagerService
    {
        private readonly AppSettings appSettings;
        public JwtTokenManagerService(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings.Value;
        }

        public async Task<string> GenerateJwtToken(GenerateJwtTokenDto payload)
        {

            var tokenHander = new JwtSecurityTokenHandler();
            var seevcreateKey = Encoding.UTF8.GetBytes(appSettings.Jwt!.SecreteKey!);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(seevcreateKey), SecurityAlgorithms.HmacSha256Signature);

            var claims = new List<Claim>();
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, payload.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim("UserName", payload.UserName));
            claims.Add(new Claim("Email", payload.Email));
            claims.Add(new Claim("FirstName", payload.FirstName));
            claims.Add(new Claim("LastName", payload.LastName));

            if(payload.Roles is not null)
            {
                foreach (var role in payload.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var tokenDescriptor = new JwtSecurityToken(

                appSettings.Jwt.Insuer,
                appSettings.Jwt.Audience,
                claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(appSettings.Jwt.TokenValidityInMinutes),
                signingCredentials: signingCredentials
                );

            return tokenHander.WriteToken(tokenDescriptor);
        }
    }
}
