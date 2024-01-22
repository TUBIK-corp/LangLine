using IspolnitelCherepashka.Enums;
using IspolnitelCherepashka.Interfaces;
using IspolnitelCherepashka.Models;
using LangLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IspolnitelCherepashka.Commands
{
    public class IfBlockCommand : IIBlockCommand
    {
        public string CommandName { get; } = "IFBLOCK";
        public string BlockEndName { get; } = "ENDIF";

        public List<InterpreterLine> Block = new List<InterpreterLine>();

        public bool Accessed = false;

        public LangLineCore Context { get; set; }

        public IfBlockCommand(LangLineCore langLine)
        {
            Context = langLine;
        }

        public void ConfigureArguments(string str_arguments)
        {
            var arg = Context.InterpreteArgument(str_arguments);
            try
            {
                string dir = (string)arg;
                Direction direction = DirectionParser.ParseDirection(dir);
                Accessed = CheckForConditional(direction);
            } catch (Exception ex)
            {
                throw new ArgumentException($"Failed to process unknown argument: {ex.Message}");
            }

        }

        public bool CheckForConditional(Direction direction)
        {
            switch (direction)
            {
                case Direction.RIGHT:
                    return Context.MainField.IsWallRight();
                case Direction.LEFT:
                    return Context.MainField.IsWallLeft();
                case Direction.UP:
                    return Context.MainField.IsWallUp();
                case Direction.DOWN:
                    return Context.MainField.IsWallDown();
                default:
                    return false;
            }
        }

        public void StartCommand(string args)
        {
            ConfigureArguments(args);
            if (Accessed)
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
                    throw new Exception($"\"IfBlock\" block line {Block[i].Index+1}: " + ex.Message);
                }
            }
        }

        public int InitializeBlock(int index)
        {
            Block = Context.InterpreterModule.TakeFrom(index);
            int skipEndsCount = 0;

            if(Block.Count == 0)
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
            //if (skipEndsCount > 0)
            //{
            //    throw new Exception("No ENDIF");
            //}
            return Block.Count+1;
        }

    }
}
