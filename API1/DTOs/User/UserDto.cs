namespace API1.DTOs.User
{
    public class UserDto
    {
        public int  UserID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool? IsActive { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; } = string.Empty;


    }
}
