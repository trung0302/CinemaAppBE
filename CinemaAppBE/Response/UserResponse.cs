using CinemaAppBE.DTO;

namespace CinemaAppBE.Response
{
    public class UserResponse
    {
        public GetUser? User { get; set; }
        public string? Token { get; set; }
    }
}
