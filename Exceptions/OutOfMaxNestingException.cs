using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Exceptions
{
    public class OutOfMaxNestingException : Exception {
        public OutOfMaxNestingException(string message) : base(message) { }
    }
}
