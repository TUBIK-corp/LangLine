using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Exceptions
{
    public class NotStatedVariableException : Exception
    {
        public NotStatedVariableException(string name) : base($"Переменная {name} не найдена.") { }
    }
}
