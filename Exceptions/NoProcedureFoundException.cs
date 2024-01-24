using System;

namespace LangLine.Exceptions
{
    public class NoProcedureFoundException : Exception
    {
        public NoProcedureFoundException() : base("Искомая процедура не найдена.") { }
    }
}
