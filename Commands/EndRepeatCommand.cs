using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IspolnitelCherepashka.Commands
{
    public class EndRepeatCommand : EndCommand
    {
        public EndRepeatCommand(LangLine.LangLine langLine) : base(langLine) { }
        public override string CommandName { get; } = "ENDREPEAT";
    }
}
