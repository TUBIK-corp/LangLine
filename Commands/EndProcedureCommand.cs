﻿using LangLine;

namespace LangLine.Commands
{
    public class EndProcedureCommand : EndCommand
    {
        public EndProcedureCommand(LangLineCore langLine, int index) : base(langLine, index) { }
        public override string CommandName { get; } = "ENDPROC";
    }

}
