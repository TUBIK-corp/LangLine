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
    public class RepeatCommand : IIBlockCommand
    {
        public string CommandName { get; } = "REPEAT";
        public string BlockEndName { get; } = "ENDREPEAT";

        public List<InterpreterLine> Block { get; set; } = new List<InterpreterLine>();

        public int Repeats = 0;

        public LangLineCore Context { get; set; }

        private int _index = -1;

        public RepeatCommand(LangLineCore langLine, int index)
        {
            Context = langLine;
            _index = index;
        }

        public void ConfigureArguments(string str_arguments)
        {
            try
            {
                var arg = Context.InterpreteArgument(str_arguments);
                Repeats = Convert.ToInt32(arg);
            }
            catch 
            {
                var log = new ExceptionLog(_index, new InvalidArgumentsException());
                Context.LogException(log);
            }
            if (Repeats > Context.MaxValueOfVariables)
            {
                var log = new ExceptionLog(_index, new OutOfMaxVariablesValueException(Context.MaxValueOfVariables), $"Значение аргумента не должно превышать {Context.MaxValueOfVariables}.");
                Context.LogException(log);
            }
            if (Repeats <= 0)
            {
                var log = new ExceptionLog(_index, new InfinityValueException());
                Context.LogException(log);
            }
        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            for(int i = 0; i<Repeats; i++)
                ExecuteBlock();
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
                    var log = new ExceptionLog(_index, new Exception($"Внутри {CommandName} произошла ошибка (в строке {_index+1})"));
                    Context.LogException(log);
                }
            }
        }

        public int InitializeBlock(int index)
        {
            return this.InitBlock(index);
            //Block = Context.InterpreterModule.TakeFrom(index);
            //int skipEndsCount = 0;
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
