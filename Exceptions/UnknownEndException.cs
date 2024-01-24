using System;

namespace LangLine.Exceptions
{
    public class UnknownEndException : Exception
    {
        public UnknownEndException() : base("Лишняя команда закрытия блока.") { }
    }

}
