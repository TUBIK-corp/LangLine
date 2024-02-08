using System;

namespace LangLine.Exceptions
{
    public class ProcedureCallInsideException : Exception
    {
        public ProcedureCallInsideException() : base($"Процедура не может вызывать сама себя.") { }
    } 
}
