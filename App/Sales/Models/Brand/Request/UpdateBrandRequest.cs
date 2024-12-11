using System.ComponentModel.DataAnnotations;

namespace Pos.App.Sales.Models.Brand;

public class UpdateBrandRequest
{
    private string _name { get; set; }

    [Required]
    public string Name {
        get => _name;
        set => _name = replaceEmptyWithNull(value);
    }

    private string replaceEmptyWithNull(string value)
    {
        // replace empty string with null to make field optional
        return string.IsNullOrEmpty(value) ? null : value;
    }
}