using Pos.App.BaseModule.Models;

namespace Pos.App.BaseModule.Models;

public class PaginatedResponse<T>
{
    public string Message { get; set; }
    public IEnumerable<T> Data { get; set; }
    public PageResponse Pages { get; set; }
}