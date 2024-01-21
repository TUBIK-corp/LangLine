using IspolnitelCherepashka.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IspolnitelCherepashka.Commands
{
    public class DownCommand : MoveCommand
    {
        public DownCommand(LangLine.LangLine langLine) : base(langLine) { }

        public override string CommandName { get; } = "DOWN";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.DOWN, MoveSteps);
        }
    }
}
