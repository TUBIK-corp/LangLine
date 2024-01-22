using IspolnitelCherepashka.Interfaces;
using LangLine;
using System;

namespace IspolnitelCherepashka.Commands
{
    public class MoveCommand : IIExecuteCommand
    {
        public virtual string CommandName { get; } = "MOVE";
        public int MoveSteps = 0;

        public virtual void Execute() { }

        public LangLineCore Context { get; set; }

        public MoveCommand(LangLineCore langLine)
        {
            Context = langLine;
        }

        public void ConfigureArguments(string str_arguments)
        {
            try
            {
                var arg = Context.InterpreteArgument(str_arguments);
                MoveSteps = Convert.ToInt32(arg);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to process unknown argument: {ex.Message}");
            }
        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            Execute();
        }
    }
}
