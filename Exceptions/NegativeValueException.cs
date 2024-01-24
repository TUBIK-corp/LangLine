using System;

namespace LangLine.Exceptions
{
    public class NegativeValueException : Exception
    {
        public NegativeValueException() : base("Программа не поддерживает отрицательные значения") { }
    }

}
