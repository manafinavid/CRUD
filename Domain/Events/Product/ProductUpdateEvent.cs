using CRUD.Domain.Common;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Events;

public class ProductUpdateEvent : ProductEvent
{
    public ProductUpdateEvent(Product item, ApplicationUser creator)
        : base(item, creator) { }
}
