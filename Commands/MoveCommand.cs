using LangLine.Interfaces;
using LangLine;
using LangLine.Exceptions;
using LangLine.Models;
using System;

namespace LangLine.Commands
{
    public class MoveCommand : IIExecuteCommand
    {
        public virtual string CommandName { get; } = "MOVE";
        public int MoveSteps = 0;

        public virtual void Execute() { }

        public LangLineCore Context { get; set; }

        private int _index = -1;

        public MoveCommand(LangLineCore langLine, int index)
        {
            Context = langLine;
            _index = index;
        }

        public void ConfigureArguments(string str_arguments)
        {
            try
            {
                var arg = Context.InterpreteArgument(str_arguments);
                MoveSteps = Convert.ToInt32(arg);
            }
            catch 
            {
                var log = new ExceptionLog(_index, new InvalidArgumentsException());
                Context.LogException(log);
            }
            if (MoveSteps < 0)
            {
                var log = new ExceptionLog(_index, new NegativeValueException());
                Context.LogException(log);
            }

        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            try
            {
                Execute();
            } catch
            {
                var log = new ExceptionLog(_index, new WallException());
                Context.LogException(log);
            }
        }
    }
}
