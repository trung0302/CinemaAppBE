using CinemaAppBE.Data;
using CinemaAppBE.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace CinemaAppBE.Services
{
    public class EmailService : IEmailService
    {
        private readonly DataContext _db;
        public EmailService(DataContext db)
        {
            _db = db;
        }
        //Gửi hóa đơn đã thanh toán
        public void SendTicketEmail(string emailTo, Reservation reservation)
        {
            var valueInvoiceString = "";
            var theater = _db.Theaters.FirstOrDefault(t => t.Name == reservation.Theater);
            var movie = _db.Movies.FirstOrDefault(i => i.Id == reservation.MovieId);
            valueInvoiceString = valueInvoiceString + $"<tr style=\"text-align:center;\">" +
                $"<td style=\" border: 1px solid #ddd;\" >{movie.Name}</td>" +
                $"<td style=\" border: 1px solid #ddd;\" >Vé</td>" +
                $"<td style=\" border: 1px solid #ddd;\" >{reservation.Quantity}</td>" +
                $"<td style=\" border: 1px solid #ddd;\" >{string.Format("{0:n0}", (reservation.Price/reservation.Quantity))} VNĐ</td>" +
                $"<td style=\" border: 1px solid #ddd;\" >{string.Format("{0:n0}", reservation.Price)} VNĐ</td>" +
                $"</tr>";
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("btcinemaa@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = "Thông tin vé xem phim tại hệ thống rạp phim BTCinema";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $"<h2>BTCinema xin kính chào quý khách!</h2>" +
                $"<div style=\"font-size: 14px; color: #333;\">{reservation.Theater}</div>" +
                $"<div style=\"font-size: 14p; color: #333;\">{theater.Location}</div>" +
                $"<div style=\"font-size: 14p; color: #333;\">1900 1088</div>" +
                $"<br>" +
                $"<h3 style=\"color: #04AA6D\">XÁC NHẬN ĐẶT VÉ THÀNH CÔNG</h3>" +
                $"<table style=\"margin-left: 30px;\">" +
                $"<tr>" +
                $"<th style=\"text-align:left\">Phim: </th>" +
                $"<td>{movie.Name.ToUpper()}</td>" +
                $"</tr>" +
                $"<tr>" +
                $"<th style=\"text-align:left\">Suất chiếu: </th>" +
                $"<td>{movie.PlayingDate}</td>" +
                $"</tr>" +
                $"<tr>" +
                $"<th style=\"text-align:left\">Giờ chiếu: </th>" +
                $"<td>{movie.PlayingTime}</td>" +
                $"</tr>" +
                $"<tr>" +
                $"<th style=\"text-align:left\">Rạp phim: </th>" +
                $"<td>{reservation.Theater}</td>" +
                $"</tr>" +
                $"<tr>" +
                $"<th style=\"text-align:left\">Thời gian đặt vé: </th>" +
                $"<td>{String.Format("{0:dd/MM/yyyy HH:mm:ss}", reservation.ReservationTime)}</td>" +
                $"</tr>" +
                $"</table>" +
                $"<br>" +
                $"<br>" +
                $"<table style=\"border: 1px solid #ddd; border-collapse: collapse; margin-left: 30px;\">" +
                $"<thead style=\" background-color: #04AA6D; color: white;\" >" +
                $"<th style=\" border: 1px solid #ddd;\" width=\"200px\">Tên phim</th>" +
                $"<th style=\" border: 1px solid #ddd;\" width=\"100px\">ĐVT</th>" +
                $"<th style=\" border: 1px solid #ddd;\" width=\"100px\">Số lượng</th>" +
                $"<th style=\" border: 1px solid #ddd;\" width=\"200px\">Đơn Giá</th>" +
                $"<th style=\" border: 1px solid #ddd;\"width=\"200px\">Thành tiền (VNĐ)</th>" +
                $"</thead>" +
                $"<tbody style=\"background-color: #f2f2f2;\">{valueInvoiceString}</tbody>" +
                $"<tfoot style=\" background-color: #04AA6D; color: white;\">" +
                $"<tr>" +
                $"<td colspan=\"5\" style=\"text-align: center\">TỔNG TIỀN (VNĐ): {string.Format("{0:n0}", reservation.Price)}</td>" +
                $"</tr>" +
                $"</tfoot>" +
                $"</table>" +
                $"<h4>Cảm ơn Quý khách đã xem phim tại BTCinema. Chúc Quý khách có một buổi xem phim vui vẻ.</h3>" +
                $"<h4 style=\"color: #035e21;\">Vui lòng liên hệ đến Hotline 19001088 nếu quý khách có thắc mắc cần giải đáp.</h4>" +
                $"<p style=\"font-style: italic; font-size: 12px; color: #777;\">Đây là email tự động. Quý khách vui lòng không trả lời email này.</p>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("btcinemaa@gmail.com", "tqnpuzezcdugmmvi");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
        public void SendCodeEmail(string emailTo, string code)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("btcinemaa@gmail.com"));
            email.To.Add(MailboxAddress.Parse(emailTo));
            email.Subject = "Mã xác minh cho email khôi phục";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = "<h2>BTCinema xin kính chào quý khách!</h2>" +
                    "<br>" +
                    "<p>Đây là mã xác nhận để khôi phục mật khẩu của bạn!</p>" +
                    $"<h2>{code}</h2>" +
                    $"<br>" +
                    "<p style=\"font-style: italic; font-size: 14px; color: #777;\">Vui lòng không chia sẻ mã code này.</p>"
            };

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("btcinemaa@gmail.com", "tqnpuzezcdugmmvi");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
