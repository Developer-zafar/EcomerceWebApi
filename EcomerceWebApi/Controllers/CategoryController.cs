using EcommerceWebApi.Controllers.DTOs;
using EcommerceWebApi.Model;
using Microsoft.EntityFrameworkCore;    
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public CategoryController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // PUBLIC: Har koi categories dekh sakta hai
        [HttpGet]
        [Route("api/categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();

            return Ok(categories);
        }

        // PUBLIC: Specific category details koi bhi dekh sakta hai
        [HttpGet]
        [Route("api/categories/{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category =await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return Ok(categoryDto);
        }

        // PROTECTED: Sirf logged-in user create kar sakta hai
        [HttpPost]
        [Authorize(Roles ="Admin")]
        [Route("api/categories")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newCategory = new Model.Category
            {
                Name = category.Name,
                Description = category.Description
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = newCategory.Id },
                newCategory
            );
        }

        // PROTECTED: Sirf logged-in user update kar sakta hai
        [HttpPut]
        [Authorize]
        [Route("api/categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDto category)
        {
            if (category == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isDuplicateName = await _context.Categories.AnyAsync(c => c.Name == category.Name && c.Id != id);
            if (isDuplicateName)
            {
                return BadRequest("Another category with this name already exists.");
            }

            var existingCategory =await _context.Categories.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = category.Name;
            existingCategory.Description = category.Description;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PROTECTED: Sirf logged-in user delete kar sakta hai
        [HttpDelete]
        [Authorize]
        [Route("api/categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category =await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}