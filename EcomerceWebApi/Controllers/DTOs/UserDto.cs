using System.ComponentModel.DataAnnotations;
namespace EcommerceWebApi.Controllers.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }   
    }
}
