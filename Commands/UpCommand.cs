using LangLine.Enums;
using LangLine;

namespace LangLine.Commands
{
    public class UpCommand : MoveCommand
    {
        public UpCommand(LangLineCore langLine, int index) : base(langLine, index) { }

        public override string CommandName { get; } = "UP";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.UP, MoveSteps);
        }
    }
}
