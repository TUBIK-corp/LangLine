using IspolnitelCherepashka.Enums;
using IspolnitelCherepashka.Interfaces;
using IspolnitelCherepashka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IspolnitelCherepashka.Commands
{
    public class RightCommand : MoveCommand
    {
        public RightCommand(LangLine.LangLineCore langLine) : base(langLine) { }

        public override string CommandName { get; } = "RIGHT";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.RIGHT, MoveSteps);
        }
    }
}
