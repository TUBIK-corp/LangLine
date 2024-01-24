using System;

namespace LangLine.Exceptions
{
    public class WallException : Exception
    {
        public WallException() : base("Исполнитель упёрся в стенку.") { }
    }
}
