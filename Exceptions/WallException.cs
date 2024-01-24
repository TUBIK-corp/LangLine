using System;

namespace LangLine.Exceptions
{
    public class WallException : Exception
    {
        public WallException(string message) : base(message) { }
    }
}
