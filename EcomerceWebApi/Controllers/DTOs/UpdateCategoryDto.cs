using System.ComponentModel.DataAnnotations;

namespace EcommerceWebApi.Controllers.DTOs
{
    public class UpdateCategoryDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Description { get; set; }
    }
}