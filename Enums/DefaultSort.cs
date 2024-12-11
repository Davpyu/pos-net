using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Pos.Enums;

[DefaultValue(Newest)]
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DefaultSort
{
    Newest,
    Oldest,
    Alphabetical,
    ReverseAlphabetical,
}