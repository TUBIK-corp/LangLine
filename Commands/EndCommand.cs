using IspolnitelCherepashka.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LangContext = LangLine.LangLineCore;

namespace IspolnitelCherepashka.Commands
{
    public class EndCommand : IICommand
    {
        public virtual string CommandName { get; }

        public LangContext Context { get; set; }

        public EndCommand(LangContext langLine)
        {
            Context = langLine;
        }

        public void StartCommand(string args)
        {
            throw new Exception($"Extra closing command {CommandName} is not expected");
        }
    }
}
