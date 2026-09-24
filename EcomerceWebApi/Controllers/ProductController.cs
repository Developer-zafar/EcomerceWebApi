using EcommerceWebApi.Controllers.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace EcommerceWebApi.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;
        public ProductController(Data.ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        
        [Route("api/products")]
        public async Task <IActionResult> GetProducts() 
        {
            var products =await _context.Products.Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    Stock = p.Stock,
                    Category = new CategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    }
                }).ToListAsync();
            return Ok(products);
        }
        [HttpGet]
        [Route("api/products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product =await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    Stock = p.Stock,
                    Category = new CategoryDto
                    {
                        Id = p.Category.Id,
                        Name = p.Category.Name
                    }
                }).FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpPost]
        [Authorize]
        [Route("api/products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
            if (!categoryExists)
            {
                return BadRequest($"Category with Id {product.CategoryId} does not exist.");
            }
            var newProduct = new Model.Product
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Stock = product.Stock
            };
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }
        [HttpDelete]
        [Route("api/products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut]
        [Route("api/products/{id}")]
        public async  Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto product)
        {
            if (product == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            bool categoryExists = await _context.Categories.AnyAsync(c => c.Id == product.CategoryId);
            if (!categoryExists)
            {
                return BadRequest($"Category with Id {product.CategoryId} does not exist.");
            }

            var existingProduct =await _context.Products.FindAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;
            existingProduct.Stock = product.Stock;

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
