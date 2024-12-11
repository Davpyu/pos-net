using System.ComponentModel.DataAnnotations;

namespace Pos.Entities;

public class BaseEntity : IBaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
}