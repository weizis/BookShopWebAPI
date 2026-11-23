using System.ComponentModel.DataAnnotations;

namespace API1.DTOs.Author
{
    public class CreateAuthorDto
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Country { get; set; }
    }
}