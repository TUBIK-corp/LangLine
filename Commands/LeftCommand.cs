﻿using IspolnitelCherepashka.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LangContext = LangLine.LangLineCore;

namespace IspolnitelCherepashka.Commands
{
    public class LeftCommand : MoveCommand
    {
        public LeftCommand(LangContext langLine) : base(langLine) { }

        public override string CommandName { get; } = "LEFT";

        public override void Execute()
        {
            Context.MainField.MoveUser(Direction.LEFT, MoveSteps);
        }
    }
}
