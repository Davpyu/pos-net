using System.Globalization;

namespace Pos.Exceptions;

public class AppException : Exception
{
    public string Code = "";
    public AppException() : base() { }

    public AppException(string message, string code) : base(message)
    {
        Console.Error.WriteLine(message, code);
        Console.WriteLine(message, code);
        Code = code;
    }

    public AppException(string message, params object[] args)
        : base(string.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}