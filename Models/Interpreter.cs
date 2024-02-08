using LangLine.Commands;
using LangLine.Interfaces;
using LangLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using LangLine.Exceptions;
using System.Windows.Shapes;
using System.Data;

namespace LangLine.Models
{
    public class InterpreterLine
    {
        public int Index { get; private set; }
        public string Line { get; private set; }
        public bool Extracted { get; private set; } = false;

        public InterpreterLine(int index, string line)
        {
            Index = index;
            Line = line;
        }
        /// <summary>
        /// Позволяет установить флаг об игноре команды, так как она уже обработана.
        /// </summary>
        public void SetExtracted()
        {
            Extracted = true;
        }
    }
    public class ExceptionEventArgs
    {
        public int Index { get; set; }
        public string ExMessage { get; set; }

        public ExceptionEventArgs(int index, string exMessage)
        {
            Index = index;
            ExMessage = exMessage;
        }
    }
    
    public class CompletedEventArgs
    {
        public List<Point> Positions { get; set; }
        public CompletedEventArgs(List<Point> positions)
        {
            Positions = positions;
        }
    }

    public class CustomEventArgs
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public CustomEventArgs(string argName, object value = null)
        {
            Name = argName;
            Value = value;
        }
    }

    /// <summary>
    /// Класс интерпретатора.
    /// </summary>
    public class Interpreter
    {
        public delegate void InterpreterEventHandler<T>(T args);
        public event InterpreterEventHandler<ExceptionEventArgs> OnException;
        public event InterpreterEventHandler<CompletedEventArgs> OnCompleted;
        public event InterpreterEventHandler<CustomEventArgs> OnCustomEvent;

        /// <summary>
        /// Проверяет, обрабатываются ли где-нибудь ошибки.
        /// </summary>
        /// <returns></returns>
        public bool IsExceptionsHandling()
        {
            return OnException.GetInvocationList().Length != 0;
        }

        /// <summary>
        /// Список команд.
        /// </summary>
        public List<InterpreterLine> CommandList { get; private set; }


        public Dictionary<string, Type> CommandTypeBinds { get; private set; }
        public LangLineCore Context { get; private set; }

        public Interpreter(LangLineCore langLine)
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

        /// <summary>
        /// Вызвать собственный event.
        /// </summary>
        /// <param name="name">Навзание.</param>
        public void InvokeCustomEvent(string name)
        {
            OnCustomEvent?.Invoke(new CustomEventArgs(name));
        }
        /// <summary>
        /// Вызвать собственный event.
        /// </summary>
        /// <param name="name">навзание.</param>
        /// <param name="value">Передаваемый параметр.</param>
        public void InvokeCustomEvent(string name, object value)
        {
            OnCustomEvent?.Invoke(new CustomEventArgs(name, value));
        }

        /// <summary>
        /// Добавляет команду в словарь команд.
        /// </summary>
        /// <param name="name">Название, по которому будет вызываться команда.</param>
        /// <param name="type">Тип, который будет реализовываться при вызове команды.</param>
        public void AddCommand(string name, Type type)
        {
            CommandTypeBinds.Add(name, type);
            Console.WriteLine($"Команда {name} добавлена.");
        }

         /// <summary>
        /// Удаляет команду из словаря команд.
        /// </summary>
        /// <param name="name">Название, по которому вызывается команда.</param>
        public void RemoveCommand(string name)
        {
            if(!CommandTypeBinds.ContainsKey(name))
                throw new UnregisteredCommand(name);
            CommandTypeBinds.Remove(name);
            Console.WriteLine($"Команда {name} удалена.");
        }
        
        /// <summary>
        /// Возвращает, зарегистрирована ли команда.
        /// </summary>
        /// <param name="name">Название команды</param>
        /// <returns>True - зарегистрировна, False - незарегистрирована.</returns>
        public bool IsCommandRegistered(string name)
        {
            return CommandTypeBinds.ContainsKey(name);
        }


        /// <summary>
        /// Устанавливает список команд.
        /// </summary>
        /// <param name="commandList">Список с командами интерпретатора.</param>
        public void SetCommandList(List<InterpreterLine> commandList)
        {
            CommandList = commandList;
        }

        /// <summary>
        /// Устанавливает команды списком из текста.
        /// </summary>
        /// <param name="stringList">Список текста с построчными командами.</param>
        public void SetCommandList(List<string> stringList)
        {
            CommandList.Clear();
            for (int i = 0; i < stringList.Count; i++)
            {
                if (string.IsNullOrEmpty(stringList[i]))
                    continue;
                CommandList.Add(new InterpreterLine(i, stringList[i].TrimEnd('\n').TrimEnd('\r').Replace('\\', '/')));
            }
        }

        /// <summary>
        /// Устанавливает команды списком из FlowDocument.
        /// </summary>
        /// <param name="flowDocument">Получить FlowDocument можно из RichTextBox из WPF.</param>
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

        /// <summary>
        /// Получает часть списка команд с указанного индексв
        /// </summary>
        /// <param name="index">Индекс, с которого нужно взять часть списка команд</param>
        /// <returns>Обрезанный список</returns>
        public List<InterpreterLine> TakeFrom(int index) =>
             CommandList.Where(line => line.Index > index).ToList();

        /// <summary>
        /// Возвращает тип найденной по названию команде.
        /// </summary>
        /// <param name="code">Название команды.</param>
        /// <returns>Type команды.</returns>
        /// <exception cref="Exception">Команда не распознана.</exception>
        public Type TypifyCommand(string code)
        {
            var command = code.ToUpper();
            if (!CommandTypeBinds.ContainsKey(command))
            {
                var log = new ExceptionLog(GetCurrentIndex(), new CommandIsNotDefinedException());
                Context.LogException(log);
            }

            return CommandTypeBinds[command];
        }


        private int _currentIndex = -1;
        public int GetCurrentIndex() => _currentIndex;

        public InterpreterLine GetCommandLineByIndex(int index)
        {
            return CommandList.First(line => line.Index == index);
        }

        /// <summary>
        /// Запускает код и возвращает количество строчек, которые надо пропустить
        /// </summary>
        /// <param name="iline"></param>
        /// <param name="ignoreExtracted"></param>
        /// <returns></returns>
        public int StartLine(InterpreterLine iline, bool ignoreExtracted = true)
        {
            if (iline.Extracted && !ignoreExtracted)
                return 0;

            _currentIndex = iline.Index;
            var line = iline.Line;
            int skipCount = 0;



            if (string.IsNullOrWhiteSpace(line))
                return 0;

            line = line.Trim(' ');
            var parts = line.Split(' ');
            var command_name = parts[0];
            var arguments_line = "";

            if (parts.Length > 0)
            {
                arguments_line = string.Join(" ", parts.Skip(1));
            }

            var command_type = TypifyCommand(command_name);
            var command = (IICommand)Activator.CreateInstance(command_type, Context, iline.Index );

            if (command is IIBlockCommand)
            {
                skipCount = ((IIBlockCommand)command).InitializeBlock(iline.Index);
                IncrementCurrentNest();
            }

            command.StartCommand(arguments_line);
            if(command is IIBlockCommand) 
                DecrementCurrentNest();
            return skipCount;
        }

        /// <summary>
        /// Инициализирует все процедуры в любом месте программы, кроме .
        /// </summary>
        public bool ExtractAndInitializeProcedures()
        {
            var filteredLines = CommandList.Where(line => line.Line.Trim().ToLower().StartsWith("procedure")).ToList();

            foreach ( var line in filteredLines)
            {
                try
                {
                    int openBlockCommandCount = 0;
                    int closeBlockCommandCount = 0;
                    foreach (var before in CommandList.Where(command => command.Index < line.Index))
                    {
                        if (string.IsNullOrWhiteSpace(before.Line.Trim()))
                            continue;
                        var split = before.Line.Trim().Split(' ');

                        _currentIndex = before.Index;

                        var typed = TypifyCommand(split[0].Trim());
                        var command = (IICommand)Activator.CreateInstance(typed, Context, before.Index);
                        if (command is IIBlockCommand)
                        {
                            openBlockCommandCount++;
                        }
                        else if (command is EndCommand)
                        {
                            closeBlockCommandCount++;
                        }
                    }
                    if (closeBlockCommandCount >= openBlockCommandCount)
                    {
                        var skipLines = StartLine(line);

                        CommandList.ForEach(command => { 
                            if(command.Index >= line.Index && command.Index < line.Index + skipLines + 1)
                            {
                                command.SetExtracted();
                            }
                        });
                    }
                    else
                    {
                        var log = new ExceptionLog(line.Index, new ProcedureInBlockException(openBlockCommandCount, closeBlockCommandCount));
                        Context.LogException(log);
                    }
                } catch
                {
                    ProcessException(line.Index);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Вызывает привязанные делегаты к OnException
        /// </summary>
        /// <param name="index">Номер строки</param>
        /// <param name="ex">Сообщение ошибки</param>
        public void ProcessException(int index)
        {
            OnException?.Invoke(new ExceptionEventArgs(index-1, ""));
        }

        /// <summary>
        /// Вложенность в текущей функции.
        /// </summary>
        public int CurrentNest { get; private set; } = 0;

        /// <summary>
        /// Повышает значение вложенности в текущей функции.
        /// </summary>
        public void IncrementCurrentNest()
        {
            if(CurrentNest+1 > Context.MaxNesting)
            {
                var log = new ExceptionLog(GetCurrentIndex(), new OutOfMaxNestingException(Context.MaxNesting));
                Context.LogException(log);
            }
            CurrentNest+=1;
        }
        
        /// <summary>
        /// Понижает значение вложенности в текущей функции.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void DecrementCurrentNest()
        {
            if(CurrentNest-1 < 0)
            {
                throw new Exception("Нарушена логика изменения вложенности.");
            }
            CurrentNest-=1;
        }
        /// <summary>
        /// Запуск программы с загруженными командами
        /// </summary>
        /// <exception cref="Exception">Командный список пуст</exception>
        public bool StartProgram()
        {
            CurrentNest = 0;
            bool isSuccess = true;

            if (!ExtractAndInitializeProcedures())
                return false;

            for (int i = 0; i < CommandList.Count; i++)
            {
                try
                {
                    var skip = StartLine(CommandList[i], ignoreExtracted: false);
                    i += skip;
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                    ProcessException(CommandList[i].Index+1);
                    break;
                }
            }

            if (isSuccess)
                OnCompleted?.Invoke(new CompletedEventArgs(Context.MainField.GetPositions()));
            
            return isSuccess;
        }
    }
}
