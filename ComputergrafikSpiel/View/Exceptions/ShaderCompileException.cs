using System;

namespace ComputergrafikSpiel.View.Exceptions
{
    public class ShaderCompileException : Exception
    {
        public ShaderCompileException(string message)
            : base(message)
        {
        }
    }
}
