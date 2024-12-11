using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Pos.Enums;

[DefaultValue(Newest)]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemSort
{
    Newest,
    Oldest,
    Alphabetical,
    ReverseAlphabetical,
    LowestPrice,
    HighestPrice,
    LowestQuantity,
    HighestQuantity
}