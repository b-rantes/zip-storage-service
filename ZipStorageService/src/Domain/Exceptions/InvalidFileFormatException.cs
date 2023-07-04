using System.Runtime.Serialization;

namespace Domain.Exceptions
{
    public class InvalidFileFormatException : Exception
    {
        private const string DefaultMessage = "Invalid zip file format: {0}";
        public InvalidFileFormatException()
        {
        }

        public InvalidFileFormatException(string? message) : base(message)
        {
        }

        public InvalidFileFormatException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidFileFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
