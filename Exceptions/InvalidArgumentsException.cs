using System;

namespace LangLine.Exceptions
{
    public class InvalidArgumentsException : Exception
    {
        public InvalidArgumentsException() : base("Неправильно заданы аргументы.") { }

        public InvalidArgumentsException(string message) : base(message) { }
    }
}
