using System.ComponentModel.DataAnnotations;
using Pos.App.BaseModule.Models;
using Pos.Enums;

namespace Pos.App.Sales.Models.Brand;

public class BrandFilter : BaseFilter
{
    [EnumDataType(typeof(DefaultSort))]
    public DefaultSort sort { get; set; }
}