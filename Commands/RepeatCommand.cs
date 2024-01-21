using IspolnitelCherepashka.Enums;
using IspolnitelCherepashka.Interfaces;
using IspolnitelCherepashka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LangContext = LangLine.LangLine;

namespace IspolnitelCherepashka.Commands
{
    public class RepeatCommand : IIBlockCommand
    {
        public string CommandName { get; } = "REPEAT";
        public string BlockEndName { get; } = "ENDREPEAT";

        public List<InterpreterLine> Block = new List<InterpreterLine>();

        public int Repeats = 0;

        public LangContext Context { get; set; }

        public RepeatCommand(LangContext langLine)
        {
            Context = langLine;
        }

        public void ConfigureArguments(string str_arguments)
        {
            try
            {
                var arg = Context.InterpreteArgument(str_arguments);
                Repeats = Convert.ToInt32(arg);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to process unknown argument: {ex.Message}");
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
                catch (Exception ex)
                {
                    throw new Exception($"\"Repeat\" block line {Block[i].Index+1}: " + ex.Message);
                }
            }
        }

        public int InitializeBlock(int index)
        {
            Block = Context.InterpreterModule.TakeFrom(index);
            int skipEndsCount = 0;
            if (Block.Count == 0)
                throw new Exception($"No {BlockEndName}");
            for (int i = 0; i < Block.Count; i++)
            {
                var line = Block[i];
                if (line.Line.ToLower().StartsWith(CommandName.ToLower()))
                    skipEndsCount++;
                else if (i == Block.Count - 1 &&
                    !line.Line.ToLower().Equals(BlockEndName.ToLower()))
                {
                    throw new Exception($"No {BlockEndName}");
                }
                else if (line.Line.ToLower().Equals(BlockEndName.ToLower()))
                {
                    if (skipEndsCount > 0)
                    {
                        skipEndsCount--;
                    }
                    else
                    {
                        Block = Block.Take(i).ToList();
                        break;
                    }
                }
            }
            if (skipEndsCount > 0)
            {
                //there is no closed
                //throw exception
            }
            return Block.Count + 1;
        }
    }
}
