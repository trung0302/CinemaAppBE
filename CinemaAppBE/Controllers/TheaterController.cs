using CinemaAppBE.Data;
using CinemaAppBE.DTO.Theater;
using CinemaAppBE.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheaterController : ControllerBase
    {
        private readonly DataContext _db;
        public TheaterController(DataContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        public ActionResult GetAllTheaters()
        {
            try
            {
                var theaters = from theater in _db.Theaters
                             select new Theater
                             {
                                 Id = theater.Id,
                                 Name = theater.Name,
                                 Location = theater.Location,
                                 ImageUrl = theater.ImageUrl,
                             };
                
                return StatusCode(StatusCodes.Status200OK, theaters);

            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> PostTheater([FromBody] AddTheaterDTO theaterObj)
        {
            try
            {
                var theater = new Theater()
                {
                    Id = Guid.NewGuid(),
                    Name = theaterObj.Name,
                    Location = theaterObj.Location,
                    ImageUrl = theaterObj.ImageUrl,
                };

                await _db.Theaters.AddAsync(theater);
                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created, theater);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Xóa
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var theater = await _db.Theaters.FindAsync(id);
                if (theater != null)
                {
                    _db.Theaters.Remove(theater);
                    await _db.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, theater);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
                }
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

    }
}
