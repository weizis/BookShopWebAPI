using System.ComponentModel.DataAnnotations;

namespace API1.DTOs.User
{
    public class CreateUserDto
    {
        [Required]
        public int RoleId { get; set; }
        [Required]
        public string FirstName{ get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
