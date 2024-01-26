using LangLine.Exceptions;
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
        /// <summary>
        /// Команда инициализации блочной команды.
        /// </summary>
        /// <typeparam name="T">Тип команды.</typeparam>
        /// <param name="Command">Расширение передаёт команду.</param>
        /// <param name="index">Берёт с определённого индекса. </param>
        /// <returns></returns>
        public static int InitBlock<T>(this T Command, int index) where T : IIBlockCommand
        {
            Command.Block = Command.Context.InterpreterModule.TakeFrom(index);
            int skipEndsCount = 0;

            if (Command.Block.Count == 0)
            {
                var log = new ExceptionLog(Command.Context.GetCurrentIndex(), new NoEndCommandException(Command.BlockEndName));
                Command.Context.LogException(log);
            }
            for (int i = 0; i < Command.Block.Count; i++)
            {
                var line = Command.Block[i];
                if (line.Line.ToLower().StartsWith("procedure"))
                {
                    var log = new ExceptionLog(line.Index, new ProcedureInBlockException());
                    Command.Context.LogException(log);
                }
                if (line.Line.ToLower().StartsWith(Command.CommandName.ToLower()))
                    skipEndsCount++;
                else if (i == Command.Block.Count - 1 &&
                    !line.Line.ToLower().Equals(Command.BlockEndName.ToLower()))
                {
                    var log = new ExceptionLog(Command.Context.GetCurrentIndex(), new NoEndCommandException(Command.BlockEndName));
                    Command.Context.LogException(log);
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
            if(skipEndsCount>0)
            {
                var log = new ExceptionLog(Command.Context.GetCurrentIndex(), new NoEndCommandException(Command.BlockEndName));
                Command.Context.LogException(log);
            }
            return Command.Block.Count + 1;
        }

    }
}
