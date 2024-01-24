using System;

namespace LangLine.Exceptions
{
    public class OutOfMaxVariablesValueException : Exception
    {
        public OutOfMaxVariablesValueException(string message) : base(message) { }
    }
}
