using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IspolnitelCherepashka.Commands
{
    public class EndIfCommand : EndCommand
    {
        public EndIfCommand(LangLine.LangLineCore langLine) : base(langLine) { }

        public override string CommandName { get; } = "ENDIF";
    }
}
