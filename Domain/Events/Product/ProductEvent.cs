using CRUD.Domain.Common;
using CRUD.Domain.Entities;
namespace CRUD.Domain.Events;

public class ProductEvent : BaseEvent
{
    public ProductEvent() : base() { }
    public ProductEvent(Product item, ApplicationUser creator)
        : base(creator)
    {
        Product = item;
    }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
