using System.ComponentModel.DataAnnotations;

namespace Pos.App.Sales.Models.Brand;

public class CreateBrandRequest
{
    [Required]
    public string Name { get; set; }
}