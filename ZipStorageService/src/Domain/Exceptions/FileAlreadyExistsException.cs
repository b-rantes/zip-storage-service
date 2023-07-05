using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class FileAlreadyExistsException : Exception
    {
        private const string DefaultMessage = "File {0} conflict - already exists";

        public FileAlreadyExistsException(string? fileName) : base(string.Format(DefaultMessage, fileName))
        {
        }

        public FileAlreadyExistsException(string? fileName, Exception? innerException) : 
            base(string.Format(DefaultMessage, fileName), innerException)
        {
        }
    }
}
