using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerce.API.Data;
using ECommerce.API.Models;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class EnquiriesController : ControllerBase
{
    private readonly AppDbContext _db;
    public EnquiriesController(AppDbContext db) => _db = db;

    // Public endpoint to create enquiry (Buy/Rent)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EnquiryCreateDto dto)
    {
        var e = new Enquiry
        {
            CustomerName = dto.CustomerName ?? "",
            PhoneNumber = dto.PhoneNumber ?? "",
            ProductId = dto.ProductId,
            Type = dto.Type ?? "Buy"
        };
        _db.Enquiries.Add(e);
        await _db.SaveChangesAsync();
        return Ok(e);
    }

    // Admin-only retrieve
    [Authorize]
    [HttpGet]
    public IActionResult GetAll() => Ok(_db.Enquiries.OrderByDescending(e => e.CreatedAt).ToList());
}

public class EnquiryCreateDto
{
    public string? CustomerName { get; set; }
    public string? PhoneNumber { get; set; }
    public int ProductId { get; set; }
    public string? Type { get; set; } // Buy or Rent
}
