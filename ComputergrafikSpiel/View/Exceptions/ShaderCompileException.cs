using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
