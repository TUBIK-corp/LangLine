using IspolnitelCherepashka.Interfaces;
using LangLine;
using System;
using static IspolnitelCherepashka.Commands.ProcedureCommand;

namespace IspolnitelCherepashka.Commands
{
    public class CallCommand : IIExecuteCommand
    {
        public string CommandName { get; set; } = "CALL";


        public ExecuteProcedure Executes;

        public LangLineCore Context { get; set; }

        public CallCommand(LangLineCore langLine)
        {
            Context = langLine;
        }

        public void ConfigureArguments(string str_arguments)
        {
            try
            {
                if (Context.ContainsVariable(str_arguments))
                {
                    Executes = (ExecuteProcedure)(Context.InterpreterVariables[str_arguments].Value);
                }
                else
                {
                    throw new Exception($"There is no procedure with name {str_arguments}");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to process unknown argument: {ex.Message}");
            }
        }

        public void Execute()
        {
            Executes();
        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            Execute();
        }
    }
}
