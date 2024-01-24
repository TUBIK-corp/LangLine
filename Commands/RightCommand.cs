using IspolnitelCherepashka.Enums;
using LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class RightCommand : MoveCommand
    {
        public RightCommand(LangLineCore langLine, int index) : base(langLine, index) { }

        public override string CommandName { get; } = "RIGHT";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.RIGHT, MoveSteps);
        }
    }
}
