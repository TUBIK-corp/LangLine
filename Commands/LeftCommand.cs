using LangLine.Enums;
using LangLine;

namespace LangLine.Commands
{
    public class LeftCommand : MoveCommand
    {
        public LeftCommand(LangLineCore langLine, int index) : base(langLine, index) { }

        public override string CommandName { get; } = "LEFT";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.LEFT, MoveSteps);
        }
    }
}
