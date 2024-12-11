namespace Pos.App.BaseModule.Models;

public class BaseResponse<T>
{
    public string Message { get; set; }
    public T Data { get; set; }
}