using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DAPServerLibrary;
using DAPClassLibrary.Helpers.Services;
using Microsoft.IdentityModel.Tokens;

namespace DAPClassLibrary
{
    public class TokenServices
    {
        private static readonly string key = ConfigurationManager.AppSettings["secretKey"];

        public static string TokenGenertor(UsersOps user)
        {
            string token = "";
            try
            {
                int expiry = Convert.ToInt32(ConfigurationManager.AppSettings["expiryTime"]);
                var secretKey = Encoding.UTF8.GetBytes(key);
                var tokenHandler = new JwtSecurityTokenHandler();

                var claims = new List<Claim>
                {
                    new Claim("userId", user.UserId.ToString()),
                    new Claim("email", user.Email),
                    new Claim("role", user.RoleShortCode),
                };

                if (user.DoctorId > 0)
                {
                    claims.Add(new Claim("doctorId", user.DoctorId.ToString()));
                }

                var descriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(expiry),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
                };

                token = tokenHandler.WriteToken(tokenHandler.CreateToken(descriptor));
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TokenServices), nameof(TokenGenertor));
                throw new Exception("JWT generation failed internally.", ex);
            }
            return token;
        }

        public static ClaimsPrincipal TokenVerify(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.UTF8.GetBytes(key);

                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                return tokenHandler.ValidateToken(token, parameters, out validatedToken);
            }
            catch (SecurityTokenExpiredException ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TokenServices), nameof(TokenVerify));
                throw new Exception("Token expired.");
            }
            catch (SecurityTokenException ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TokenServices), nameof(TokenVerify));
                throw new Exception("Invalid token.");
            }
            catch (Exception ex)
            {
                ExceptionLogService.LogExceptionInDB(ex, nameof(TokenServices), nameof(TokenVerify));
                throw new Exception("Token validation failed.", ex);
            }
        }
    }
}
