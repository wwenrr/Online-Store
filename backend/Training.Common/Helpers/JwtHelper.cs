using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Training.CustomException;

namespace Training.Common.Helpers
{
    public class JwtHelper
    {
        private static string _secretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
        private static string __issuer = "MK";
        private static string __audience = ";:]]]]]";

        public static JwtSecurityToken DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken;
        }
        public static void ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = __issuer,
                    ValidAudience = __audience, 
                    IssuerSigningKey = key,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);
            }
            catch (SecurityTokenException)
            {
                throw new CustomHttpException("Token không hợp lệ!", 403);
            }
        }
        public static string GenToken(string email, string passwd)
        {
            var claims = new List<Claim>
            {
                new Claim("email", email),
                new Claim("key", passwd)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: __issuer,
                audience: __audience,
                claims: claims,
                expires: DateTime.Now.AddHours(10),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
