using IspolnitelCherepashka.Enums;
using LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class LeftCommand : MoveCommand
    {
        public LeftCommand(LangLineCore langLine) : base(langLine) { }

        public override string CommandName { get; } = "LEFT";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.LEFT, MoveSteps);
        }
    }
}
