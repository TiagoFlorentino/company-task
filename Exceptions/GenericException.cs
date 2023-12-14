namespace CompanyApi.Exceptions;

public class GenericException : Exception
{
    public GenericException(string message) : base(message)
    {
    }
}