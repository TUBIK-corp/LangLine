using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IspolnitelCherepashka.Commands
{
    public class EndProcedureCommand : EndCommand
    {
        public EndProcedureCommand(LangLine.LangLine langLine) : base(langLine) { }
        public override string CommandName { get; } = "ENDPROC";
    }

}
