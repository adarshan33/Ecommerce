using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerce.API.Data;
using ECommerce.API.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IWebHostEnvironment _env;

    public ProductsController(AppDbContext db, IWebHostEnvironment env)
    {
        _db = db; _env = env;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_db.Products
            .OrderByDescending(p => p.Id)  // newest at top
            .ToList());
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
    {
        var p = new Product {
            Name = dto.Name ?? "",
            Category = dto.Category ?? "",
            Description = dto.Description ?? "",
            Price = dto.Price,
            IsRental = dto.IsRental
        };

        if (dto.Image != null)
        {
            var uploadsPath = Path.Combine(_env.ContentRootPath, "uploads");

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var fn = Guid.NewGuid().ToString() + Path.GetExtension(dto.Image.FileName);
            var fp = Path.Combine(uploadsPath, fn);

            using var stream = System.IO.File.Create(fp);
            await dto.Image.CopyToAsync(stream);

            p.ImagePath = "uploads/" + fn;
        }


        _db.Products.Add(p);
        await _db.SaveChangesAsync();
        return Ok(p);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] ProductCreateDto dto)
    {
        var p = await _db.Products.FindAsync(id);

        if (p == null)
            return NotFound();

        p.Name = dto.Name ?? p.Name;
        p.Category = dto.Category ?? p.Category;
        p.Description = dto.Description ?? p.Description;
        p.Price = dto.Price;
        p.IsRental = dto.IsRental;

        // Image upload
        if (dto.Image != null)
        {
            var uploadsPath = Path.Combine(_env.ContentRootPath, "uploads");
            Directory.CreateDirectory(uploadsPath);

            var fn = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            var fp = Path.Combine(uploadsPath, fn);

            using var stream = System.IO.File.Create(fp);
            await dto.Image.CopyToAsync(stream);

            p.ImagePath = "uploads/" + fn;
        }

        await _db.SaveChangesAsync();
        return Ok(p);
    }


    [Authorize]
    [HttpPost("delete")]
    public async Task<IActionResult> DeleteProducts([FromBody] int[] ids)
    {
        //var isAdmin = Request.Headers["X-ADMIN-KEY"] == "supersecret123";

        //if (!isAdmin)
        //    return Unauthorized("You are not allowed to delete products.");
        var products = _db.Products.Where(p => ids.Contains(p.Id)).ToList();

        if (products.Count == 0)
            return NotFound();

        foreach (var product in products)
        {
            if (!string.IsNullOrWhiteSpace(product.ImagePath))
            {
                var fullPath = Path.Combine(_env.ContentRootPath, product.ImagePath);
                if (System.IO.File.Exists(fullPath))
                    System.IO.File.Delete(fullPath);
            }
        }

        _db.Products.RemoveRange(products);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Product(s) deleted successfully" });
    }

}

public class ProductCreateDto
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public bool IsRental { get; set; }
    public IFormFile? Image { get; set; }
}
