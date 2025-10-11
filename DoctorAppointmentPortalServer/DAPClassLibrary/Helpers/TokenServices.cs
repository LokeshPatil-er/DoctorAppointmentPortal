using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DAPServerLibrary;
using Microsoft.IdentityModel.Tokens;

namespace DAPClassLibrary
{
    public class TokenServices
    {
        private static readonly string key = ConfigurationManager.AppSettings["secretKey"];

        public static string TokenGenertor(string userEmail,string userRoleShortCode,int userId)
        {
            string token = "";
            try
            {
                int expiry = Convert.ToInt32(ConfigurationManager.AppSettings["expiryTime"]);
                var secretKey = Encoding.UTF8.GetBytes(key);
                var tokenHandler = new JwtSecurityTokenHandler();

                var descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, userEmail),
                    new Claim(ClaimTypes.Role,userRoleShortCode)
                }),
                    Expires = DateTime.UtcNow.AddMinutes(expiry),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                };

                token = tokenHandler.WriteToken(tokenHandler.CreateToken(descriptor));

            }
            catch (Exception ex)
            {
                throw new Exception("JWT generation failed internally.", ex);
            }
            return token;
        }
    }
}
