using LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class EndRepeatCommand : EndCommand
    {
        public EndRepeatCommand(LangLineCore langLine, int index) : base(langLine, index) { }
        public override string CommandName { get; } = "ENDREPEAT";
    }
}
