namespace ECommerce.API.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public bool IsRental { get; set; }

    public bool Featured { get; set; }
    public string ImagePath { get; set; } = "";

    public string? ImagePublicId { get; set; }

}
