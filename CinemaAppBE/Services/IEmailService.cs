using CinemaAppBE.Models;

namespace CinemaAppBE.Services
{
    public interface IEmailService
    {
        public void SendTicketEmail(string emailTo, Reservation reservation);
        public void SendCodeEmail(string emailTo, string code);
    }
}
