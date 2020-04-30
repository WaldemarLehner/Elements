using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComputergrafikSpiel.View.Exceptions
{
    public class ArgumentNotPositiveIntegerGreaterZeroException : ArgumentOutOfRangeException
    {
        public ArgumentNotPositiveIntegerGreaterZeroException()
            : base()
        {
        }

        public ArgumentNotPositiveIntegerGreaterZeroException(string parameter)
            : base(parameter, $"{parameter} needs to be a positive integer that is not zero.")
        {
        }
    }
}
