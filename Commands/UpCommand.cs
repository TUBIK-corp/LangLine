using IspolnitelCherepashka.Enums;
using LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class UpCommand : MoveCommand
    {
        public UpCommand(LangLineCore langLine) : base(langLine) { }

        public override string CommandName { get; } = "UP";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.UP, MoveSteps);
        }
    }
}
