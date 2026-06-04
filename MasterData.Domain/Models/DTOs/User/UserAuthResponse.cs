namespace MasterData.Domain.Models.DTOs.User
{
    public class UserAuthResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string Roles { get; set; } 
    }
}
