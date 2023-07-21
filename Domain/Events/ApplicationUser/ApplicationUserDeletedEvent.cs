using CRUD.Domain.Common;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Events;

public class ApplicationUserDeletedEvent : ApplicationUserEvent
{
    public ApplicationUserDeletedEvent(ApplicationUser item, ApplicationUser creator)
        : base(item, creator) { }
}
