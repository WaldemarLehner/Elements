using System;
using System.Runtime.Serialization;

namespace ComputergrafikSpiel.Model.EntitySettings.Texture
{
    [Serializable]
    internal class SpaceBarIdentifierException : Exception
    {
        private string v1;
        private string v2;

        public SpaceBarIdentifierException()
        {
        }

        public SpaceBarIdentifierException(string message) : base(message)
        {
        }

        public SpaceBarIdentifierException(string v1, string v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }

        public SpaceBarIdentifierException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SpaceBarIdentifierException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}