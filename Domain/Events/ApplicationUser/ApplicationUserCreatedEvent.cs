using CRUD.Domain.Common;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Events;

public class ApplicationUserCreatedEvent : ApplicationUserEvent
{
    public ApplicationUserCreatedEvent(ApplicationUser item, ApplicationUser creator)
        : base(item, creator) { }
}
