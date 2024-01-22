using LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class EndRepeatCommand : EndCommand
    {
        public EndRepeatCommand(LangLineCore langLine) : base(langLine) { }
        public override string CommandName { get; } = "ENDREPEAT";
    }
}
