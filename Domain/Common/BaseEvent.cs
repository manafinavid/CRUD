using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CRUD.Domain.Entities;
using MediatR;

namespace CRUD.Domain.Common;

public abstract class BaseEvent : INotification
{
    public BaseEvent()
    {
        EventTime = DateTime.UtcNow;
        CreatorId = string.Empty;
    }

    public BaseEvent(ApplicationUser creator)
    {
        Creator = creator;
        EventTime = DateTime.UtcNow;
        CreatorId = string.Empty;

    }
    [Key]
    public Guid Id { get; set; }

    [ForeignKey("CreatorId")]
    public ApplicationUser Creator { get; set; } = null!;
    public string CreatorId { get; set; }

    public DateTime EventTime { get; }
}