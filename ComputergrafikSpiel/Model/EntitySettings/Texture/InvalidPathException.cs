using System;
using System.Runtime.Serialization;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    [Serializable]
    internal class InvalidPathException : Exception
    {
        private string v1;
        private string v2;

        public InvalidPathException()
        {
        }

        public InvalidPathException(string message) : base(message)
        {
        }

        public InvalidPathException(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public InvalidPathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidPathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}