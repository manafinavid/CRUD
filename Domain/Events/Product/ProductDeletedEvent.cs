using CRUD.Domain.Common;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Events;

public class ProductDeletedEvent : ProductEvent
{
    public ProductDeletedEvent(Product item, ApplicationUser creator)
        : base(item, creator) { }
}
