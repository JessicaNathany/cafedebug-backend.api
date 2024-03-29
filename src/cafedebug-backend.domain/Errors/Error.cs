using cafedebug_backend.domain.Enums;
using Microsoft.Extensions.Localization;

namespace cafedebug_backend.domain.Errors;

public class Error : IEquatable<Error>
{
    public static readonly Error None = new(ErrorType.None, string.Empty);
    public static Error NullValue(IStringLocalizer localizer) => new(ErrorType.ExistingRegister, localizer["NullValue"]);

    public Error(ErrorType code, string message)
    {
        Code = code.ToString();
        Message = message;
    }
    
    private Error(string property, string message)
    {
        Code = property;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static Error ValidationError(string name, string message) => new(name, message);

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null) 
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b)
    {
        return !(a == b);
    }

    public bool Equals(Error? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        
        return obj.GetType() == GetType() && Equals((Error) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }
}