using LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class EndProcedureCommand : EndCommand
    {
        public EndProcedureCommand(LangLineCore langLine) : base(langLine) { }
        public override string CommandName { get; } = "ENDPROC";
    }

}
