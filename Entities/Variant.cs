namespace Pos.Entities;

public class Variant : BaseEntity
{
    public string Name { get; set; }
    public Guid BrandId { get; set; }
    public Brand Brand { get; set; }
}