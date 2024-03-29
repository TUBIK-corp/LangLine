﻿using LangLine.Enums;
using LangLine;

namespace LangLine.Commands
{
    public class DownCommand : MoveCommand
    {
        public DownCommand(LangLineCore langLine, int index) : base(langLine, index) { }

        public override string CommandName { get; } = "DOWN";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.DOWN, MoveSteps);
        }
    }
}
