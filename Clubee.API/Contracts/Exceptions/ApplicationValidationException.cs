namespace Clubee.API.Contracts.Exceptions
{
    /// <summary>
    /// Represents a validation fail.
    /// </summary>
    public class ApplicationValidationException : BadRequestException
    {
        public ApplicationValidationException(string message) : base(message) { }
    }
}
