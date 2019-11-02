using System;

namespace Clubee.API.Contracts.Exceptions
{
    /// <summary>
    /// Exception format to handle missing environment variables.
    /// </summary>
    public class MissingEnvironmentVariableException : Exception
    {
        public MissingEnvironmentVariableException(string variableName)
            : base($"Missing environment variable {variableName}.") { }
    }
}
