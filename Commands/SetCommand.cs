using LangLine.Interfaces;
using LangLine.Models;
using LangLine;
using LangLine.Exceptions;
using LangLine.Models;
using System;
using System.Reflection;

namespace LangLine.Commands
{
    public class SetCommand : IIExecuteCommand
    {
        public string CommandName { get; set; } = "SET";
        public string Name { get; set; }
        public object Value { get; set; }

        public LangLineCore Context { get; set; }

        private int _index = -1;

        public SetCommand(LangLineCore langLine, int index)
        {
            Context = langLine;
            _index = index;
        }

        public void ConfigureArguments(string str_arguments)
        {
            try
            {
                var between = str_arguments.Split('=');
                Name = between[0].Trim();
                Value = Context.InterpreteArgument(between[1].Trim());
            }
            catch
            {
                var log = new ExceptionLog(_index, new InvalidArgumentsException());
                Context.LogException(log);
            }
            if (int.TryParse(Name[0].ToString(), out int res))
            {
                var log = new ExceptionLog(_index, new InvalidNameOfVariableException(), $"Переменная \"{Name}\" не может начинаться с цифры \"{res}\"");
                Context.LogException(log);
            }
            if((int)Value > Context.MaxValueOfVariables)
            {
                var log = new ExceptionLog(_index, new OutOfMaxVariablesValueException(Context.MaxValueOfVariables), $"Переменная \"{Name}\" слишком большая.");
                Context.LogException(log);
            }
        }

        public void Execute()
        {
            if (Context.ContainsVariable(Name))
            {
                Context.InterpreterVariables[Name] = new InterpreterVariable(Value);
            }
            else
            {
                Context.CreateNewVariable(Name, new InterpreterVariable(Value));
            }
        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            Execute();
        }
    }
}
