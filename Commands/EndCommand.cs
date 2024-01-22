using IspolnitelCherepashka.Interfaces;
using LangLine;
using System;

namespace IspolnitelCherepashka.Commands
{
    public class EndCommand : IICommand
    {
        public virtual string CommandName { get; }

        public LangLineCore Context { get; set; }

        public EndCommand(LangLineCore langLine)
        {
            Context = langLine;
        }

        public void StartCommand(string args)
        {
            throw new Exception($"Extra closing command {CommandName} is not expected");
        }
    }
}
