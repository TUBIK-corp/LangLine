using LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class EndIfCommand : EndCommand
    {
        public EndIfCommand(LangLineCore langLine, int index) : base(langLine, index) { }

        public override string CommandName { get; } = "ENDIF";
    }
}
