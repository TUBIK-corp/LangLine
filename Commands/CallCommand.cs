using LangLine.Interfaces;
using LangLine;
using LangLine.Exceptions;
using LangLine.Models;
using System;
using System.Xml.Linq;
using static LangLine.Commands.ProcedureCommand;

namespace LangLine.Commands
{
    public class CallCommand : IIExecuteCommand
    {
        public string CommandName { get; set; } = "CALL";


        public ExecuteProcedure Executes;

        public LangLineCore Context { get; set; }

        private int _index = -1;
        public CallCommand(LangLineCore langLine, int index)
        {
            Context = langLine;
            _index = index;
        }

        public void ConfigureArguments(string str_arguments)
        {
            bool notFound = false;
            bool isBusy = false;
            try
            {
                if (Context.ContainsVariable(str_arguments))
                {
                    Executes = (ExecuteProcedure)(Context.InterpreterVariables[str_arguments].Value);
                    var target = (ProcedureCommand)Executes.Target;
                    if(target.IsBusy)
                    {
                        isBusy = true;
                    }
                }
                else
                {
                    notFound = true;
                }
            }
            catch
            {
                var log = new ExceptionLog(_index, new InvalidArgumentsException());
                Context.LogException(log);
            }
            if(isBusy)
            {
                var log = new ExceptionLog(_index, new ProcedureCallInsideException());
                Context.LogException(log);
            }
            if(notFound)
            {
                var log = new ExceptionLog(_index, new NoProcedureFoundException());
                Context.LogException(log);
            }

        }

        public void Execute()
        {
            try
            {
                Executes();
            } catch
            {
                var log = new ExceptionLog(_index, new Exception($"Внутри {CommandName} произошла ошибка (в строке {_index + 1})"));
                Context.LogException(log);
            }
        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            Execute();
        }
    }
}
