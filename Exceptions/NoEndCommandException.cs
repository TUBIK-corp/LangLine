using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Exceptions
{
    public class NoEndCommandException : Exception
    {
        public NoEndCommandException(string command) : base($"Отсутствует закрывающая команда {command}") { }
    }
}
