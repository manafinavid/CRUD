using CRUD.Domain.Common;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Events;

public class ProductCreatedEvent : ProductEvent
{
    public ProductCreatedEvent(Product item, ApplicationUser creator)
        : base(item, creator) { }
}
