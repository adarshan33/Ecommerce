using System;

namespace ECommerce.API.Models;

public class Enquiry
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public int? ProductId { get; set; }
    public string Type { get; set; } = "Buy"; // "Buy" or "Rent"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
