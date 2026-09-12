namespace EcommerceWebApi.Controllers.DTOs;
    using System.ComponentModel.DataAnnotations;
    public class CreateOrderDto
    {
        [Required]
        public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
    }

