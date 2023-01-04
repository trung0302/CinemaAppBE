using CinemaAppBE.Data;
using CinemaAppBE.DTO.Reservation;
using CinemaAppBE.Models;
using CinemaAppBE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CinemaAppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly DataContext _db;
        private readonly IEmailService _email;
        public ReservationController(DataContext db, IEmailService email)
        {
            _db = db;
            _email = email;
        }

        //[Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> Post([FromBody] AddReservationDTO reservationObj)
        {
            try
            {
                var reservation = new Reservation()
                {
                    Id = Guid.NewGuid(),
                    Phone = reservationObj.Phone,
                    Price = reservationObj.Price,
                    Quantity = reservationObj.Quantity,
                    PhuongThuc = reservationObj.PhuongThuc,
                    Theater = reservationObj.Theater,
                    UserId = reservationObj.UserId,
                    MovieId = reservationObj.MovieId,
                    ReservationTime = DateTime.Now,
                };

                await _db.Reservations.AddAsync(reservation);
                await _db.SaveChangesAsync();

                var customer = _db.Users.FirstOrDefault(u => u.Id == reservation.UserId);

                _email.SendTicketEmail(customer.Email, reservation);

                return StatusCode(StatusCodes.Status201Created, reservation);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }
        
        //Xem chi tiết vé
        [HttpGet("[action]")]
        public async Task<ActionResult> GetTickets([FromQuery] Guid userId)
        {
            try
            {
                var tickets = (from reservation in _db.Reservations
                               join movie in _db.Movies on reservation.MovieId equals movie.Id
                               where reservation.UserId.Equals(userId)
                               select new
                               {
                                   Name = movie.Name,
                                   Duration = movie.Duration,
                                   PlayingDate = movie.PlayingDate,
                                   PlayingTime = movie.PlayingTime,
                                   Price = reservation.Price,
                                   Quantity = reservation.Quantity,
                                   ImageUrl = movie.ImageUrl,
                                   ReservationTime = reservation.ReservationTime,
                               })
                               .OrderByDescending(i => i.ReservationTime)
                               .ToList();

                return StatusCode(StatusCodes.Status200OK, tickets);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public ActionResult GetReservations()
        {
            try
            {
                var reservations = from reservation in _db.Reservations
                                   join customer in _db.Users on reservation.UserId equals customer.Id
                                   join movie in _db.Movies on reservation.MovieId equals movie.Id
                                   select new
                                   {
                                       Id = reservation.Id,
                                       ReservationTime = reservation.ReservationTime,
                                       CustomerName = customer.Name,
                                       MovieName = movie.Name
                                   };

                return StatusCode(StatusCodes.Status200OK, reservations);
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status400BadRequest, err.Message);
            }
        }


        //[Authorize(Roles = "Admin")]
        [HttpGet("{id:Guid}")]
        public ActionResult GetReservationDetail([FromBody] Guid id)
        {
            var reservationResult = (from reservation in _db.Reservations
                                     join customer in _db.Users on reservation.UserId equals customer.Id
                                     join movie in _db.Movies on reservation.MovieId equals movie.Id
                                     where reservation.Id == id
                                     select new
                                     {
                                         Id = reservation.Id,
                                         ReservationTime = reservation.ReservationTime,
                                         CustomerName = customer.Name,
                                         MovieName = movie.Name,
                                         Email = customer.Email,
                                         Quantity = reservation.Quantity,
                                         Price = reservation.Price,
                                         Phone = reservation.Phone,
                                         PlayingDate = movie.PlayingDate,
                                         PlayingTime = movie.PlayingTime,
                                     }).FirstOrDefault();
            if (reservationResult != null)
            {
                return StatusCode(StatusCodes.Status200OK ,reservationResult);
            }

            return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
        }

        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> Delete([FromBody] Guid id)
        {
            var reservation = await _db.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Not Found!");
            }
            else
            {
                _db.Reservations.Remove(reservation);
                await _db.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, reservation);
            }
        }
    }
}
