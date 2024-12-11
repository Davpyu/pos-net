namespace Pos.Entities;

public class Brand : BaseEntity
{
    public string Name { get; set; }
    public virtual List<Variant> Variants { get; set; }
}