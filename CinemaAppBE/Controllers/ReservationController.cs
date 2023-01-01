//using CinemaAppBE.Data;
//using CinemaAppBE.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Data;

//namespace CinemaAppBE.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ReservationController : ControllerBase
//    {
//        private readonly DataContext _db;
//        public ReservationController(DataContext db)
//        {
//            _db = db;
//        }

//        //[Authorize]
//        [HttpPost("[action]")]
//        public async Task<ActionResult> Post([FromBody] Reservation reservationObj)
//        {
//            try
//            {
//                reservationObj.ReservationTime = DateTime.Now;
//                await _db.Reservations.AddAsync(reservationObj);
//                await _db.SaveChangesAsync();

//                return StatusCode(StatusCodes.Status201Created, reservationObj);
//            }
//            catch (Exception err)
//            {
//                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
//            }
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpGet]
//        public IActionResult GetReservations()
//        {
//            var reservations = from reservation in _db.Reservations
//                               join customer in _db.Users on reservation.UserId equals customer.Id
//                               join movie in _db.Movies on reservation.MovieId equals movie.Id
//                               select new
//                               {
//                                   Id = reservation.Id,
//                                   ReservationTime = reservation.ReservationTime,
//                                   CustomerName = customer.Name,
//                                   MovieName = movie.Name
//                               };
//            return Ok(reservations);
//        }


//        [Authorize(Roles = "Admin")]
//        [HttpGet("{id}")]
//        public IActionResult GetReservationDetail(int id)
//        {
//            var reservationResult = (from reservation in _db.Reservations
//                                     join customer in _db.Users on reservation.UserId equals customer.Id
//                                     join movie in _db.Movies on reservation.MovieId equals movie.Id
//                                     where reservation.Id == id
//                                     select new
//                                     {
//                                         Id = reservation.Id,
//                                         ReservationTime = reservation.ReservationTime,
//                                         CustomerName = customer.Name,
//                                         MovieName = movie.Name,
//                                         Email = customer.Email,
//                                         Qty = reservation.Qty,
//                                         Price = reservation.Price,
//                                         Phone = reservation.Phone,
//                                         PlayingDate = movie.PlayingDate,
//                                         PlayingTime = movie.PlayingTime,
//                                     }).FirstOrDefault();
//            return Ok(reservationResult);
//        }

//        [Authorize(Roles = "Admin")]
//        [HttpDelete("{id}")]
//        public IActionResult Delete(int id)
//        {
//            var reservation = _db.Reservations.Find(id);
//            if (reservation == null)
//            {
//                return NotFound("No record found against this Id");
//            }
//            else
//            {
//                _db.Reservations.Remove(reservation);
//                _db.SaveChanges();
//                return Ok("Record deleted");
//            }
//        }
//    }
//}
