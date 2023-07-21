using System.ComponentModel.DataAnnotations.Schema;
using CRUD.Domain.Common;
using CRUD.Domain.Entities;
namespace CRUD.Domain.Events;

public class ApplicationUserEvent : BaseEvent
{
    public ApplicationUserEvent() : base()
    {
        ApplicationUserId = string.Empty;
    }
    public ApplicationUserEvent(ApplicationUser item, ApplicationUser creator)
        : base(creator)
    {
        ApplicationUser = item;
        ApplicationUserId = string.Empty;
    }

    [ForeignKey("ApplicationUserId")]
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public string ApplicationUserId { get; set; }
}
