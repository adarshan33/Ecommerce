using Microsoft.EntityFrameworkCore;
using ECommerce.API.Models;

namespace ECommerce.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Enquiry> Enquiries => Set<Enquiry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Category).HasColumnName("category");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.IsRental).HasColumnName("isrental");
            entity.Property(e => e.Featured).HasColumnName("featured");
            entity.Property(e => e.ImagePath).HasColumnName("imagepath");
            entity.Property(e => e.ImagePublicId).HasColumnName("imagepublicid");

        });

        modelBuilder.Entity<Enquiry>().ToTable("enquiries");
        modelBuilder.Entity<Enquiry>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("productid");
            entity.Property(e => e.CustomerName).HasColumnName("name");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.CreatedAt).HasColumnName("createdat");
        });
    }
}
