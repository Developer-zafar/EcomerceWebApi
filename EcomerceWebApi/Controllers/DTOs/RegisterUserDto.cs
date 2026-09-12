using System.ComponentModel.DataAnnotations;
namespace EcommerceWebApi.Controllers.DTOs

{
    public class RegisterUserDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

    }
}
