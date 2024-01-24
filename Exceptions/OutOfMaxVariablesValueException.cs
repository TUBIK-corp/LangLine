using System;

namespace LangLine.Exceptions
{
    public class OutOfMaxVariablesValueException : Exception
    {
        public OutOfMaxVariablesValueException(int value) : base($"Аргумент превысил максимальное возможное значение: {value}") { }
    }
}
