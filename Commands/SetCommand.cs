using IspolnitelCherepashka.Interfaces;
using IspolnitelCherepashka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using LangContext = LangLine.LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class SetCommand : IIExecuteCommand
    {
        public string CommandName { get; set; } = "SET";
        public string Name { get; set; }
        public object Value { get; set; }

        public LangContext Context { get; set; }

        public SetCommand(LangContext langLine)
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
                Context.NewVariable(Name, new InterpreterVariable(Value));
            }
        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            Execute();
        }
    }
}
