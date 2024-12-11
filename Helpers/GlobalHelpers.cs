

using Pos.App.BaseModule.Models;

namespace Pos.Helpers;

public static class GlobalHelpers
{
    public static int ValidatePageSize(int pageSize)
    {
        List<int> validPageSizes = new List<int>() { 10, 25, 50, 100 };
        return validPageSizes.Contains(pageSize) ? pageSize : 10;
    }

    public static string RandomString(int length)
    {
        Random random = new();

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static BaseResponse<T> CreateBaseResponse<T>(string message, T data)
    {
        return new()
        {
            Message = message,
            Data = data
        };
    }

    public static PaginatedResponse<T> CreatePaginatedResponse<T>(string message, IEnumerable<T> data, int page, int totalPage, int totalCount)
    {
        return new()
        {
            Message = message,
            Data = data,
            Pages = new()
            {
                Page = page,
                TotalPage = totalPage,
                TotalCount = totalCount
            }
        };
    }
}