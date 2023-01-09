using AuthenticationPlugin;
using CinemaAppBE.Auth;
using CinemaAppBE.Data;
using CinemaAppBE.DTO.User;
using CinemaAppBE.Models;
using CinemaAppBE.Response;
using CinemaAppBE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CinemaAppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _db;
        private readonly IEmailService _email;
        private Authentication _auth = new Authentication();
        private IConfiguration _config;

        public UserController(DataContext db, IConfiguration configuration, IEmailService email)
        {
            _config = configuration;
            _db = db;
            _email = email;
        }

        //Đăng ký
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Register([FromBody] UserRegisterDTO user)
        {
            var userWithSameEmail = _db.Users.FirstOrDefault(u => u.Email == user.Email);
            if (userWithSameEmail != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Email đã tồn tại!");
            }

            var idUser = Guid.NewGuid();
            var userObj = new User
            {
                Id = idUser,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = "Users",
            };
            userObj.Password = BCrypt.Net.BCrypt.HashPassword(userObj.Password);

            var token = _auth.GenerateToken(userObj.Email, userObj.Role, _config);
            var userToken = new Token
            {
                Id = Guid.NewGuid(),
                token = token,
                UserId = userObj.Id
            };
            _db.Users.Add(userObj);
            _db.Tokens.Add(userToken);
            _db.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        //Đăng nhập
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO user)
        {
            var userAuth = _auth.Authenticate(user, _db);
            if (userAuth != null)
            {
                var token = _auth.GenerateToken(userAuth.Email, userAuth.Role, _config);
                var userResponse = new UserResponse
                {
                    Id = userAuth.Id,
                    Name = userAuth.Name,
                    Email = userAuth.Email,
                    Password = userAuth.Password,
                    Role = userAuth.Role,
                    Token = token
                };

                var userToken = new Token
                {
                    Id = Guid.NewGuid(),
                    token = token,
                    UserId = userAuth.Id,
                };

                _db.Tokens.Add(userToken);
                await _db.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, userResponse);
            }
            return StatusCode(StatusCodes.Status400BadRequest, "Error Email Or Password");

        }

        //Logout
        [HttpPost("[action]/{id:Guid}")]
        public async Task<ActionResult<GetUser>> Logout([FromRoute] Guid id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user == null)
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
                }
                var token = _db.Tokens.FirstOrDefault(t => t.UserId == user.Id);
                _db.Tokens.Remove(token);

                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, "Ok");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //Lấy tất cả User

        [HttpGet("[action]")]
        public async Task<ActionResult> GetAllUser()
        {
            try
            {
                var users = (from user in _db.Users
                             select new
                             {
                                 Id = user.Id,
                                 Name = user.Name,
                                 Email = user.Email,
                                 Role = user.Role,
                             }).ToList();

                return StatusCode(StatusCodes.Status200OK, users);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        [HttpGet("[action]/{id:Guid}")]
        public async Task<ActionResult> GetUserById([FromRoute] Guid id)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user != null)
                {
                    return StatusCode(StatusCodes.Status200OK, user);
                }

                return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Update User
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserDTO userObj)
        {
            try
            {
                var user = await _db.Users.FindAsync(id);
                if (user != null)
                {
                    user.Name = userObj.Name;
                    user.Password = BCrypt.Net.BCrypt.HashPassword(userObj.Password);
                    await _db.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, user);
                }

                return StatusCode(StatusCodes.Status404NotFound, "Not Found");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Confirm User Reset Password
        [HttpGet("[action]")]
        public async Task<ActionResult> ConfirmUser([FromQuery] string email)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    Random rdn = new Random();
                    var code = rdn.Next(1001, 9999);
                    var verifyCode = new VerifyCode()
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        Code = code.ToString(),
                        Email = email,
                    };

                    await _db.VerifyCodes.AddAsync(verifyCode);
                    await _db.SaveChangesAsync();

                    _email.SendCodeEmail(email, code.ToString());

                    return StatusCode(StatusCodes.Status200OK, user);
                }

                return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Get User By Email
        [HttpGet("[action]")]
        public async Task<ActionResult> GetUserByEmail([FromQuery] string email)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(u => u.Email == email);

                if (user != null)
                {
                    return StatusCode(StatusCodes.Status200OK, user);
                }

                return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }


        //private DataContext _dbContext;
        //private IConfiguration _configuration;
        //private readonly AuthService _auth;
        //public UserController(DataContext dbContext, IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //    _auth = new AuthService(_configuration);
        //    _dbContext = dbContext;
        //}

        //[HttpPost("[action]")]
        //public ActionResult Register([FromBody] UserRegisterDTO user)
        //{
        //    var userWithSameEmail = _dbContext.Users.Where(u => u.Email == user.Email).SingleOrDefault();
        //    if (userWithSameEmail != null)
        //    {
        //        return BadRequest("User with same email already exists");
        //    }
        //    var userObj = new User
        //    {
        //        Name = user.Name,
        //        Email = user.Email,
        //        Password = SecurePasswordHasherHelper.Hash(user.Password),
        //        Role = "Users"
        //    };
        //    _dbContext.Users.Add(userObj);
        //    _dbContext.SaveChanges();
        //    return StatusCode(StatusCodes.Status201Created);
        //}

        //[HttpPost("[action]")]
        //public ActionResult Login([FromBody] UserLoginDTO user)
        //{
        //    var userEmail = _dbContext.Users.FirstOrDefault(u => u.Email == user.Email);
        //    if (userEmail == null)
        //    {
        //        return NotFound();
        //    }
        //    if (!SecurePasswordHasherHelper.Verify(user.Password, userEmail.Password))
        //    {
        //        return null;
        //    }

        //    var claim = new List<Claim>()
        //    {
        //       new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //       new Claim(ClaimTypes.Email, user.Email),
        //       new Claim(ClaimTypes.Role, userEmail.Role),
        //    };

        //    var token = _auth.GenerateAccessToken(claim);
        //    return new ObjectResult(
        //        new
        //        {
        //            access_token = token.AccessToken,
        //            expires_in = token.ExpiresIn,
        //            token_type = token.TokenType,
        //            creation_Time = token.ValidFrom,
        //            expiration_Time = token.ValidTo,
        //            user_id = userEmail.Id,
        //            user_Name = userEmail.Name
        //        });
        //}
    }
}
