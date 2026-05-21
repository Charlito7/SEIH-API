using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Commons;

public class BaseEntity
{
    [Key]
    public Guid? Id { get; set; }
}
