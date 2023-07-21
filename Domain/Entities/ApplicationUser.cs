using CRUD.Domain.Events;
using Microsoft.AspNetCore.Identity;

namespace CRUD.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser() : base()
    {
        Events = new();
    }

    public List<ApplicationUserEvent> Events { get; set; }
}
