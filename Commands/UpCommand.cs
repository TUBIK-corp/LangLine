using IspolnitelCherepashka.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IspolnitelCherepashka.Commands
{
    public class UpCommand : MoveCommand
    {
        public UpCommand(LangLine.LangLineCore langLine) : base(langLine) { }

        public override string CommandName { get; } = "UP";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.UP, MoveSteps);
        }
    }
}
