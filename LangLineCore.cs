using LangLine.Interfaces;
using LangLine.Models;
using LangLine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace LangLine
{
    public class LangLineCore
    {
        /// <summary>
        /// Переменные программы.
        /// </summary>
        public Dictionary<string, InterpreterVariable> InterpreterVariables { get; private set; }
        /// <summary>
        /// Модуль интерпретатора.
        /// </summary>
        public Interpreter InterpreterModule { get; private set; }
        /// <summary>
        /// Физическое поле для передвижения.
        /// </summary>
        public FieldModel MainField { get; private set; }
        /// <summary>
        /// Максимально возможная вложенность (По умолчанию 1000).
        /// </summary>
        public int MaxNesting { get; private set; } = 1000;

        /// <summary>
        /// Максимально возможное значение переменной (По умолчанию 10000000). 
        /// </summary>
        public int MaxValueOfVariables { get; private set; } = 10000000;
        
        /// <summary>
        /// Точка запуска исполнителя.
        /// </summary>
        public Point SpawnPoint { get; private set; } = new Point(0, 0);

        /// <summary>
        /// Основной модуль запуска програмного кода на языке LangLine.
        /// </summary>
        public LangLineCore()
        {
            InterpreterVariables = new Dictionary<string, InterpreterVariable>();
            InterpreterModule = new Interpreter(this);
            MainField = new FieldModel(this, 21, 21);
            StackTrace = new List<ExceptionLog>();
        }

        /// <summary>
        /// Основной модуль запуска програмного кода на языке LangLine. Задаётся собственная высота и ширина.
        /// </summary>
        /// <param name="width">Ширина</param>
        /// <param name="height">Высота</param>
        public LangLineCore(int width, int height)
        {
            InterpreterVariables = new Dictionary<string, InterpreterVariable>();
            InterpreterModule = new Interpreter(this);
            MainField = new FieldModel(this, width, height);
            StackTrace = new List<ExceptionLog>();
        }

        /// <summary>
        /// Устанавливает точку запуска исполнителя.
        /// </summary>
        /// <param name="newPoint"></param>
        public void SetSpawnPoint(Point newPoint)
        {
            SpawnPoint = newPoint;
        }

        /// <summary>
        /// Запуск программы с заданным програмным кодом. В случае отсутсвия програмного кода, вызывается Exception. Обработать Exception можно используя event - OnException
        /// </summary>                                                                       
        public void StartProgram()                                                           
        {                                                                                    
            StackTrace.Clear();                                                              
            MainField.ClearPositions();                                                      
            ClearVariables();                                                                
            var success = InterpreterModule.StartProgram();                                  
            if(!success)                                                                     
            {                                                                                
                if(!InterpreterModule.IsExceptionsHandling())                                
                {                                                                            
                    throw StackTrace.Last().Exception;                                       
                }                                                                            
            }                                                                                
        }                                                                                    
                                    
        /// <summary>
        /// Получить индекс команды, на которой сейчас находится программа.
        /// </summary>
        /// <returns>Индекс.</returns>
        public int GetCurrentIndex()
        {
            return InterpreterModule.GetCurrentIndex();
        }                                                                           
                                      
        /// <summary>
        /// StackTrace для логирования ошибок. 
        /// </summary>
        public List<ExceptionLog> StackTrace { get; set; }                           
                             
        /// <summary>
        /// Логирование возникающих ошибок в StackTrace, навигация через throw.
        /// </summary>
        /// <param name="exceptionLog">Лог ошибки.</param>
        public void LogException(ExceptionLog exceptionLog)                                  
        {
            StackTrace.Add(exceptionLog);
            throw exceptionLog.Exception;
        }                                                                                    
                                      
        /// <summary>
        /// Ограничить максимальную вложенность.
        /// </summary>
        /// <param name="maxNesting">Число.</param>
        public void LimitNesting(int maxNesting)                                             
        {                                                                                    
            MaxNesting = maxNesting;
        }

        /// <summary>
        /// Ограничить максимальное значение переменных.
        /// </summary>
        /// <param name="maxVariable">Число.</param>
        public void LimitVariable(int maxVariable)
        {
            MaxValueOfVariables = maxVariable;
        }



        /// <summary>
        /// Зарегистрировать собственную команду.
        /// </summary>
        /// <param name="name">Название команды для кода.</param>
        /// <param name="type">Тип команды для инициализации.</param>
        public void RegisterCommand(string name, Type type)
        {
            InterpreterModule.AddCommand(name, type);
        }
        
        /// <summary>
        /// Отменить регистрацию существующей команды.
        /// </summary>
        /// <param name="name">Название команды для кода.</param>
        /// <param name="type">Тип команды для инициализации.</param>
        public void UnregisterCommand(string name, Type type)
        {
            InterpreterModule.AddCommand(name, type);
        }

        /// <summary>
        /// Установить команды из FlowDocument.
        /// </summary>
        /// <param name="document">Получить FlowDocument можно из RichTextBox из WPF.</param>
        public void SetCommandsFromFlowDocument(FlowDocument document) => InterpreterModule.SetCommandList(document);

        /// <summary>
        /// Установить команды списком текста.
        /// </summary>
        /// <param name="list">Список с текстовыми командами</param>
        public void SetCommandsFromStringList(List<string> list) => InterpreterModule.SetCommandList(list);

        /// <summary>
        /// Установить команды вручную.
        /// </summary>
        /// <param name="lines">Список команд.</param>
        public void SetCommands(List<InterpreterLine> lines) => InterpreterModule.SetCommandList(lines);

        /// <summary>
        /// Получить значение из существующей переменной.
        /// </summary>
        /// <param name="name">Название переменной.</param>
        /// <returns>Значение переменной.</returns>
        public object GetVariableValue(string name) =>
            InterpreterVariables[name];

        /// <summary>
        /// Создание новой переменной в программе.
        /// </summary>
        /// <param name="name">Название переменной.</param>
        /// <param name="value">Значение переменной.</param>
        public void CreateNewVariable(string name, InterpreterVariable value)
        {
            if (!InterpreterVariables.ContainsKey(name))
                InterpreterVariables.Add(name, value);
        }
        /// <summary>
        /// Проверяет, содержит ли программа переменную с указанным названием.
        /// </summary>
        /// <param name="arg">Название.</param>
        /// <returns>Boolean</returns>
        public bool ContainsVariable(string arg) =>
            InterpreterVariables.ContainsKey(arg);

        /// <summary>
        /// Очищает переменные из программы.
        /// </summary>
        public void ClearVariables() =>
            InterpreterVariables.Clear();

        /// <summary>
        /// Обрабатывает текстовый аргумент, полученный из команды.
        /// </summary>
        /// <param name="argstr">Аргумент.</param>
        /// <returns>Возвращает значение переменной, если переменная существует, иначе возвращает эту же строку.</returns>
        public object InterpreteArgument(string argstr)
        {
            if (ContainsVariable(argstr))
                return InterpreterVariables[argstr].Value;
            return argstr;
        }
    }
}
