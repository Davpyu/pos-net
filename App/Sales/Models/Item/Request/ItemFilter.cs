using System.ComponentModel.DataAnnotations;
using Pos.App.BaseModule.Models;
using Pos.Enums;

namespace Pos.App.Sales.Models.Item;

public class ItemFilter : BaseFilter
{
    [EnumDataType(typeof(ItemSort))]
    public ItemSort sort { get; set; }
    public Guid? brandId { get; set; }
}