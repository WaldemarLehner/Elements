using System;
using System.Runtime.Serialization;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    [Serializable]
    internal class EmptyIdentifierException : Exception
    {
        private string v1;
        private string v2;

        public EmptyIdentifierException()
        {
        }

        public EmptyIdentifierException(string message) : base(message)
        {
        }

        public EmptyIdentifierException(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public EmptyIdentifierException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EmptyIdentifierException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}