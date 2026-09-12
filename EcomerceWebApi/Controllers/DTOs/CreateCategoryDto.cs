using System.ComponentModel.DataAnnotations;
namespace EcommerceWebApi.Controllers.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public String Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public String Description { get; set; }
    }
}
