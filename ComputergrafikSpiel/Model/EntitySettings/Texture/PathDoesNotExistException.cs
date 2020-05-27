using System;
using System.Runtime.Serialization;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    [Serializable]
    internal class PathDoesNotExistException : Exception
    {
        private string v1;
        private string v2;

        public PathDoesNotExistException()
        {
        }

        public PathDoesNotExistException(string message) : base(message)
        {
        }

        public PathDoesNotExistException(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public PathDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PathDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}