using System;

namespace LangLine.Exceptions
{
    public class ProcedureInBlockException : Exception
    {
        public ProcedureInBlockException(int openBracketsCount, int endBracketsCount) : base($"Процедура не может быть указана в блоке. ({openBracketsCount}vs{endBracketsCount})") { }
        public ProcedureInBlockException() : base($"Процедура не может быть указана в блоке.") { }

    }


}
