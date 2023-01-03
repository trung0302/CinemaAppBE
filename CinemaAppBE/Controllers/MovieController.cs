using CinemaAppBE.Data;
using CinemaAppBE.DTO.Movie;
using CinemaAppBE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CinemaAppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly DataContext _db;
        public MovieController(DataContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        public IActionResult AllMovies([FromQuery] string? sort, [FromQuery] int? pageNumber , [FromQuery] int? pageSize)
        {
            try
            {
                var currentPageNumber = pageNumber ?? 1;
                var currentPageSize = pageSize ?? 5;
                var movies = from movie in _db.Movies
                             select new
                             {
                                 Id = movie.Id,
                                 Name = movie.Name,
                                 Duration = movie.Duration,
                                 Language = movie.Language,
                                 Rating = movie.Rating,
                                 Genre = movie.Genre,
                                 ImageUrl = movie.ImageUrl,
                                 PlayingDate = movie.PlayingDate,
                                 PlayingTime = movie.PlayingTime,
                                 Advice = movie.Advice
                             };

                switch (sort)
                {
                    case "desc":
                        return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderByDescending(m => m.Rating));
                    case "asc":
                        return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).OrderBy(m => m.Rating));
                    default:
                        return Ok(movies.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize));
                }
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Get 3 phim mới nhất

        [HttpGet("[action]")]
        public async Task<ActionResult> GetThreeFilms()
        {
            try
            {
                var films = (from movie in _db.Movies
                             select new
                             {
                                 Id = movie.Id,
                                 Name = movie.Name,
                                 Language = movie.Language,
                                 PlayingDate = movie.PlayingDate,
                                 FullImageUrl = movie.FullImageUrl,
                                 CreatedAt = movie.CreatedAt,
                                 UpdatedAt = movie.UpdatedAt,
                             })
                             .ToList()
                             .OrderByDescending(mv => DateTime.Parse(mv.PlayingDate))
                             .Skip(0)
                             .Take(3)
                             .ToList();

                return StatusCode(StatusCodes.Status200OK, films);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Tìm phim theo Id
        //[Authorize]
        [HttpGet("[action]/{id:Guid}")]
        public async Task<ActionResult> MovieDetail([FromRoute] Guid id)
        {
            try
            {
                var movie = await _db.Movies.FindAsync(id);
                if (movie != null)
                {
                    return StatusCode(StatusCodes.Status200OK, movie);
                }

                return StatusCode(StatusCodes.Status404NotFound, "Not Found!"); ;
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Phim theo thể loại
        //[Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult> MovieGenre([FromQuery] string genre)
        {
            try
            {
                var movies = (from movie in _db.Movies
                             where movie.Genre.Equals(genre)
                             select new
                             {
                                 Id = movie.Id,
                                 ImageUrl = movie.ImageUrl,
                                 Name = movie.Name,
                                 Duration = movie.Duration,
                                 Language = movie.Language,
                                 Rating = movie.Rating,
                                 Genre = movie.Genre,
                             })
                             .ToList();
                return StatusCode(StatusCodes.Status200OK, movies);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Phim theo advice true
        //[Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult> MovieAdvice()
        {
            try
            {
                var movies = (from movie in _db.Movies
                              where movie.Advice.Equals(true)
                              select new
                              {
                                  Id = movie.Id,
                                  ImageUrl = movie.ImageUrl,
                                  Name = movie.Name,
                                  Duration = movie.Duration,
                                  Language = movie.Language,
                                  Rating = movie.Rating,
                                  Genre = movie.Genre,
                              })
                             .ToList();
                return StatusCode(StatusCodes.Status200OK, movies);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Phim rating cao
        //[Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult> MovieRating()
        {
            try
            {
                var movies = (from movie in _db.Movies
                              select new
                              {
                                  Id = movie.Id,
                                  ImageUrl = movie.ImageUrl,
                                  Name = movie.Name,
                                  Duration = movie.Duration,
                                  Language = movie.Language,
                                  Rating = movie.Rating,
                                  Genre = movie.Genre,
                              })
                              .OrderByDescending(i => i.Rating)
                              .Skip(0).Take(6)
                              .ToList();

                return StatusCode(StatusCodes.Status200OK, movies);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Tìm phim theo tên
        //[Authorize]
        [HttpGet("[action]")]
        public ActionResult FindMovies([FromQuery] string? movieName)
        {
            try
            {
                var movies = from movie in _db.Movies
                             where movie.Name.ToLower().Contains(movieName.ToLower())
                             select new
                             {
                                 Id = movie.Id,
                                 Name = movie.Name,
                                 ImageUrl = movie.ImageUrl,
                             };
                return StatusCode(StatusCodes.Status200OK, movies);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
            
        }

        //Thêm phim
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddMovieDTO movieObj)
        {
            try
            {
                var movie = new Movie()
                {
                    Id = Guid.NewGuid(),
                    Name = movieObj.Name,
                    Description = movieObj.Description,
                    Language = movieObj.Language,
                    Duration = movieObj.Duration,
                    PlayingDate = movieObj.PlayingDate,
                    PlayingTime = movieObj.PlayingTime,
                    TicketPrice = movieObj.TicketPrice,
                    Rating = movieObj.Rating,
                    Genre = movieObj.Genre,
                    TrailorUrl = movieObj.TrailorUrl,
                    ImageUrl = movieObj.ImageUrl,
                    FullImageUrl = movieObj.FullImageUrl,
                    Advice = movieObj.Advice
                };

                await _db.Movies.AddAsync(movie);
                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status201Created, movie);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }

        //Xóa phim
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                var movie = await _db.Movies.FindAsync(id);
                if (movie != null)
                {
                    _db.Movies.Remove(movie);
                    await _db.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, movie);
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
