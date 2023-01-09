using CinemaAppBE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerifyCodeController : ControllerBase
    {
        private readonly DataContext _db;
        public VerifyCodeController(DataContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetCode([FromQuery] string email, [FromQuery] string code)
        {
            try
            {
                var verify = (from verifyCode in _db.VerifyCodes
                              where verifyCode.Email.Equals(email) && verifyCode.Code.Equals(code)
                              select new
                              {
                                  Id = verifyCode.Id,
                                  UserId = verifyCode.UserId,
                                  Code = verifyCode.Code,
                                  Email = verifyCode.Email,
                              })
                              .ToList();

                if (verify.Count() != 0)
                {
                    return StatusCode(StatusCodes.Status200OK, verify);
                }

                return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteCode([FromQuery] string code)
        {
            try
            {
                var verify = _db.VerifyCodes.FirstOrDefault(i => i.Code == code);

                if (verify != null)
                {
                    _db.VerifyCodes.Remove(verify);
                    await _db.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, verify);
                }

                return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetAllCode()
        {
            try
            {
                var verify = (from verifyCode in _db.VerifyCodes
                              select new
                              {
                                  Id = verifyCode.Id,
                                  UserId = verifyCode.UserId,
                                  Code = verifyCode.Code,
                              })
                              .ToList(); ;
                if (verify != null)
                {
                    return StatusCode(StatusCodes.Status200OK, verify);
                }

                return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }
    }
}
