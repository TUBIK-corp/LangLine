using IspolnitelCherepashka.Commands;
using IspolnitelCherepashka.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using static System.Net.Mime.MediaTypeNames;
using LangContext = LangLine.LangLine;

namespace IspolnitelCherepashka.Models
{
    public class InterpreterLine
    {
        public int Index { get; set; }
        public string Line { get; set; }

        public InterpreterLine(int index, string line)
        {
            Index = index;
            Line = line;
        }
    }
    public class Interpreter
    {
        public delegate void ExceptionCatch(int index, string exMessage);
        public event ExceptionCatch ExceptionCatched;

        public List<InterpreterLine> CommandList;

        public Dictionary<string, Type> CommandTypeBinds;
        public LangContext Context;

        public Interpreter(LangContext langLine)
        {
            Context = langLine;
            CommandList = new List<InterpreterLine>();
            CommandTypeBinds = new Dictionary<string, Type>
            {
                { "RIGHT", typeof(RightCommand) },
                { "UP", typeof(UpCommand) },
                { "LEFT", typeof(LeftCommand) },
                { "DOWN", typeof(DownCommand) },
                { "IFBLOCK", typeof(IfBlockCommand) },
                { "ENDIF", typeof(EndIfCommand) },
                { "REPEAT", typeof(RepeatCommand) },
                { "ENDREPEAT", typeof(EndRepeatCommand) },
                { "PROCEDURE", typeof(ProcedureCommand) },
                { "ENDPROC", typeof(EndProcedureCommand) },
                { "SET", typeof(SetCommand) },
                { "CALL", typeof(CallCommand) }
            };
        }

        public void SetCommandList(List<InterpreterLine> commandList)
        {
            CommandList = commandList;
        }

        public void SetCommandList(List<string> stringList)
        {
            CommandList.Clear();
            for (int i = 0; i < stringList.Count; i++)
            {
                if (String.IsNullOrEmpty(CommandList[i].Line))
                    continue;
                CommandList.Add(new InterpreterLine(i, stringList[i].TrimEnd('\n').TrimEnd('\r').Replace('\\', '/')));
            }
        }
        public void SetCommandList(FlowDocument flowDocument)
        {
            CommandList.Clear();
            for (int i = 0; i < flowDocument.Blocks.Count; i++)
            {
                var paragraph = flowDocument.Blocks.ToList()[i] as Paragraph;
                var text = new TextRange(paragraph.ContentStart, paragraph.ContentEnd).Text;
                CommandList.Add(new InterpreterLine(i, text.TrimEnd('\n').TrimEnd('\r').Replace('\\', '/')));

            }
        }

        public List<InterpreterLine> TakeFrom(int index) =>
             CommandList.Where(line => line.Index > index).ToList();

        public Type TypifyCommand(string code)
        {
            var command = code.ToUpper();
            if (!CommandTypeBinds.ContainsKey(command))
            {
                throw new Exception("Command is not defined");
            }

            return CommandTypeBinds[command];
        }

        /// <summary>
        /// Запускает код и возвращает количество строчек, которые надо пропустить
        /// </summary>
        /// <param name="iline"></param>
        /// <returns></returns>
        public int StartLine(InterpreterLine iline)
        {
            var line = iline.Line;
            int skipCount = 0;
            if (string.IsNullOrWhiteSpace(line))
                return 0;
            line = line.TrimStart(' ');
            var parts = line.Split(' ');
            var command_name = parts[0];
            var arguments_line = "";
            if (parts.Length > 0)
            {
                arguments_line = String.Join(" ", parts.Skip(1));
            }

            var command_type = TypifyCommand(command_name);
            var command = (IICommand)Activator.CreateInstance(command_type, args: Context);

            if (command is IIBlockCommand)
                skipCount = ((IIBlockCommand)command).InitializeBlock(iline.Index);

            command.StartCommand(arguments_line);

            return skipCount;
        }

        public void ProcessException(int index, Exception ex)
        {
            ExceptionCatched?.Invoke(index-1, ex.Message);
        }

        public void StartProgram()
        {
            if (CommandList is null)
            {
                throw new Exception("CommandList is empty");
            }

            for (int i = 0; i < CommandList.Count; i++)
            {
                try
                {
                    var skip = StartLine(CommandList[i]);
                    i += skip;
                }
                catch (Exception ex)
                {
                    ProcessException(CommandList[i].Index+1, ex);
                    break;
                }
            }
        }
    }
}
