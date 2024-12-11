using System.ComponentModel.DataAnnotations;
using Pos.App.BaseModule.Models;
using Pos.Enums;

namespace Pos.App.Sales.Models.Variant;

public class VariantFilter : BaseFilter
{
    [EnumDataType(typeof(DefaultSort))]
    public DefaultSort sort { get; set; }
}