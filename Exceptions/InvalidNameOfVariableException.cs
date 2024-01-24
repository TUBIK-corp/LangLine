using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Exceptions
{
    public class InvalidNameOfVariableException : Exception
    {
        public InvalidNameOfVariableException() : base("Переменную нельзя так называть.") { }
    }
}
