using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidFileStructureException : Exception
    {
        private const string DefaultMessage = "Invalid zip file structure: {0}";
        public InvalidFileStructureException()
        {
        }

        public InvalidFileStructureException(string? message) : base(message)
        {
        }

        public InvalidFileStructureException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidFileStructureException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
