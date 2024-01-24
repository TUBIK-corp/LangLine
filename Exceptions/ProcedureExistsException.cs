using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Exceptions
{
    public class ProcedureExistsException : Exception
    {
        public ProcedureExistsException() : base("Ошибка при создании процедуры: процедура уже существует.") { }
    }
}
