using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Pos.Enums;

[DefaultValue(Pcs)]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ItemQuantityType
{
    Pcs,
    Kg,
}