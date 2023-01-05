using CinemaAppBE.Data;
using CinemaAppBE.DTO.User;
using CinemaAppBE.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaAppBE.Auth
{
    public class Authentication
    {
        public string GenerateToken(string email, string role, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(720),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateTokenRSPW(string email, string role, IConfiguration _config)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(20),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public GetUser? Authenticate(UserLoginDTO userLogin, DataContext _db)
        {
            try
            {
                var currentUser = _db.Users.FirstOrDefault(u => u.Email == userLogin.Email);

                if (currentUser != null)
                {

                    if (!BCrypt.Net.BCrypt.Verify(userLogin.Password, currentUser.Password))
                    {
                        return null;
                    }

                    var getUser = new GetUser
                    {
                        Id = currentUser.Id,
                        Name = currentUser.Name,
                        Email = currentUser.Email,
                        Role = currentUser.Role,
                        Password = currentUser.Password
                    };
                    return getUser;
                }

                return null;
            }
            catch
            {
                return null;
            }

        }
        public Token? CheckTokenLogout(string token, DataContext _db)
        {
            var tokenUser = _db.Tokens.SingleOrDefault(t => t.token == token);

            if (tokenUser == null)
            {
                return null;
            }
            return tokenUser;
        }
    }
}
