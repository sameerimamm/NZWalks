namespace NZWalks.API.Models.DTO
{
    public class LoginResponse
    {
        public string jwtToken { get; set; }
        public string[] Roles { get; set; }
    }
}
