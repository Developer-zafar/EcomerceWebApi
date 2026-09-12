using EcommerceWebApi.Controllers.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace EcommerceWebApi.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;
        public OrderController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        [Route("api/orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            // 1. Validation check
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            {
                return BadRequest("Order must contain at least one item.");
            }
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            int userId = int.Parse(userIdClaim.Value);
            decimal totalAmount = 0;
            var orderItemsList = new List<Model.OrderItem>();
            foreach (var item in orderDto.OrderItems)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    return BadRequest($"Product with Id {item.ProductId} does not exist.");
                }

                if (product.Stock < item.Quantity)
                {
                    return BadRequest($"Insufficient stock for product '{product.Name}'. Available: {product.Stock}");
                }

                product.Stock -= item.Quantity;

                totalAmount += product.Price * item.Quantity;

                orderItemsList.Add(new Model.OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }
            var order = new Model.Order
            {
                UserId = userId,
                TotalAmount = totalAmount,
                OrderDate = DateTime.UtcNow,
                OrderItems = orderItemsList
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = "Order placed successfully!",
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate
            });
        }
        [HttpGet]
        [Authorize]
        [Route("api/orders")]
        public async Task<IActionResult> GetOrders()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            int userId = int.Parse(userIdClaim.Value);
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();
            var orderDtos = orders.Select(o => new
            {
                o.Id,
                o.OrderDate,
                o.TotalAmount,
                Items = o.OrderItems.Select(oi => new
                {
                    oi.ProductId,
                    ProductName = oi.Product.Name,
                    oi.Quantity,
                    oi.UnitPrice
                })
            });
            return Ok(orderDtos);
        }
    } 
}
