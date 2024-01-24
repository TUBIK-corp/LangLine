using IspolnitelCherepashka.Interfaces;
using LangLine;
using LangLine.Exceptions;
using LangLine.Models;
using System;

namespace IspolnitelCherepashka.Commands
{
    public class EndCommand : IICommand
    {
        public virtual string CommandName { get; }

        public LangLineCore Context { get; set; }

        private int _index = -1;
        public EndCommand(LangLineCore langLine, int index)
        {
            Context = langLine;
            _index = index;
        }

        public void StartCommand(string args)
        {
            var log = new ExceptionLog(_index, new UnknownEndException());
            Context.LogException(log);
        }
    }
}
