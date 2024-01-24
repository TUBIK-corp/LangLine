using LangLine.Enums;
using LangLine;

namespace LangLine.Commands
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
