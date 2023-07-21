using CRUD.Domain.Common;
using CRUD.Domain.Entities;

namespace CRUD.Domain.Events;

public class ApplicationUserUpdateEvent : ApplicationUserEvent
{
    public ApplicationUserUpdateEvent(ApplicationUser item, ApplicationUser creator)
        : base(item, creator) { }
}
