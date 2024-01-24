using LangLine.Interfaces;
using LangLine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LangLine.Commands.Helpers
{
    public static class BlockCommand
    {
        public static int InitBlock<T>(this T Command, int index) where T : IIBlockCommand
        {
            Command.Block = Command.Context.InterpreterModule.TakeFrom(index);
            int skipEndsCount = 0;

            if (Command.Block.Count == 0)
                throw new Exception($"No {Command.BlockEndName}");
            for (int i = 0; i < Command.Block.Count; i++)
            {
                var line = Command.Block[i];
                if (line.Line.ToLower().StartsWith(Command.CommandName.ToLower()))
                    skipEndsCount++;
                else if (i == Command.Block.Count - 1 &&
                    !line.Line.ToLower().Equals(Command.BlockEndName.ToLower()))
                {
                    throw new Exception($"No {Command.BlockEndName}");
                }
                else if (line.Line.ToLower().Equals(Command.BlockEndName.ToLower()))
                {
                    if (skipEndsCount > 0)
                    {
                        skipEndsCount--;
                    }
                    else
                    {
                        Command.Block = Command.Block.Take(i).ToList();
                        break;
                    }
                }
            }
            return Command.Block.Count + 1;
        }

    }
}
