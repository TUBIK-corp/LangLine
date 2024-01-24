using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Exceptions
{
    public class NoEndCommandException : Exception
    {
        public NoEndCommandException(string command) : base($"Отсутсвует закрывающая команда {command}") { }
    }
}
