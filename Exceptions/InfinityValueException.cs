using System;

namespace LangLine.Exceptions
{
    public class InfinityValueException : Exception
    {
        public InfinityValueException() : base("Указанное значение приводит к бесконечности.") { }
    }

}
