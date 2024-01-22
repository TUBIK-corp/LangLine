using LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class EndIfCommand : EndCommand
    {
        public EndIfCommand(LangLineCore langLine) : base(langLine) { }

        public override string CommandName { get; } = "ENDIF";
    }
}
