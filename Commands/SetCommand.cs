using IspolnitelCherepashka.Interfaces;
using IspolnitelCherepashka.Models;
using LangLine;
using System;

namespace IspolnitelCherepashka.Commands
{
    public class SetCommand : IIExecuteCommand
    {
        public string CommandName { get; set; } = "SET";
        public string Name { get; set; }
        public object Value { get; set; }

        public LangLineCore Context { get; set; }

        public SetCommand(LangLineCore langLine)
        {
            Context = langLine;
        }

        public void ConfigureArguments(string str_arguments)
        {
            try
            {
                var between = str_arguments.Split('=');
                Name = between[0].Trim();
                Value = Context.InterpreteArgument(between[1].Trim());
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to process unknown argument: {ex.Message}");
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
