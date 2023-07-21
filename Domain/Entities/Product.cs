using System.ComponentModel.DataAnnotations;
using CRUD.Domain.Events;

namespace CRUD.Domain.Entities;

public class Product
{
    public Product()
    {
        Name = string.Empty;
        ProductDate = DateTime.UtcNow;
        ManufacturePhone = string.Empty;
        ManufactureEmail = string.Empty;
        Events = new();
    }
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }
    public DateTime ProductDate { get; set; }
    public string ManufacturePhone { get; set; }
    public string ManufactureEmail { get; set; }

    public bool IsAvailable { get; set; }

    public List<ProductEvent> Events { get; set; }

}
