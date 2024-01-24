using LangLine.Interfaces;
using LangLine.Models;
using LangLine;
using LangLine.Commands.Helpers;
using LangLine.Exceptions;
using LangLine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LangLine.Commands
{
    public class ProcedureCommand : IIBlockCommand
    {
        public delegate void ExecuteProcedure();
        public string CommandName { get; } = "PROCEDURE";
        public string BlockEndName { get; } = "ENDPROC";

        public List<InterpreterLine> Block { get; set; } = new List<InterpreterLine>();

        public string Name { get; set; } = "";
        public ExecuteProcedure Execute;

        public LangLineCore Context { get; set; }


        private int _index = -1;

        public ProcedureCommand(LangLineCore langLine, int index)
        {
            Context = langLine;
            _index = index;
        }

        public void ConfigureArguments(string str_arguments)
        {
            object name = null;
            if (str_arguments.Split(' ').Length > 1)
            {
                var log = new ExceptionLog(_index, new InvalidArgumentsException());
                Context.LogException(log);
            }
            name = Context.InterpreteArgument(str_arguments);

            if (!name.Equals(str_arguments))
            {
                var log = new ExceptionLog(_index, new InvalidArgumentsException("Невозможно назвать процедуру как переменную."));
                Context.LogException(log);
            }
            if (name is ExecuteProcedure)
            {
                var log = new ExceptionLog(_index, new ProcedureExistsException());
                Context.LogException(log);
            }
            Name = (string)name;
        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            Execute = ExecuteBlock;
            Context.CreateNewVariable(Name, new InterpreterVariable(Execute));
        }

        public void ExecuteBlock()
        {
            for (int i = 0; i < Block.Count; i++)
            {
                try
                {
                    var skip = Context.InterpreterModule.StartLine(Block[i]);
                    i += skip;
                }
                catch
                {
                    var log = new ExceptionLog(_index, new Exception($"Внутри процедуры {Name} произошла ошибка (в строке {_index+1})"));
                    Context.LogException(log);
                }
            }
        }

        public int InitializeBlock(int index)
        {
            return this.InitBlock(index);

            //Block = Context.InterpreterModule.TakeFrom(index);
            //int skipEndsCount = 0;
            //
            //if (Block.Count == 0)
            //    throw new Exception($"No {BlockEndName}");
            //for (int i = 0; i < Block.Count; i++)
            //{
            //    var line = Block[i];
            //    if (line.Line.ToLower().StartsWith(CommandName.ToLower()))
            //        skipEndsCount++;
            //    else if (i == Block.Count - 1 &&
            //        !line.Line.ToLower().Equals(BlockEndName.ToLower()))
            //    {
            //        throw new Exception($"No {BlockEndName}");
            //    }
            //    else if (line.Line.ToLower().Equals(BlockEndName.ToLower()))
            //    {
            //        if (skipEndsCount > 0)
            //        {
            //            skipEndsCount--;
            //        }
            //        else
            //        {
            //            Block = Block.Take(i).ToList();
            //            break;
            //        }
            //    }
            //}
            //if (skipEndsCount > 0)
            //{
            //    //there is no closed
            //    //throw exception
            //}
            //return Block.Count + 1;
        }
    }
}
