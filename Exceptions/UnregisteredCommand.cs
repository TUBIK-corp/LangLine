using System;

namespace LangLine.Exceptions
{
    public class UnregisteredCommand : Exception
    {
        public UnregisteredCommand(string name) : base($"Команда {name} не зарегистрирована") { }
    }
}
