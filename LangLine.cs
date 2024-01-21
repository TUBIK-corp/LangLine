using IspolnitelCherepashka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace LangLine
{
    public class LangLine
    {
        public Dictionary<string, InterpreterVariable> InterpreterVariables;
        public Interpreter InterpreterModule;
        public FieldModel MainField;

        public LangLine()
        {
            InterpreterVariables = new Dictionary<string, InterpreterVariable>();
            InterpreterModule = new Interpreter(this);
            MainField = new FieldModel(21, 21);
        }

        public LangLine(int width, int height)
        {
            InterpreterVariables = new Dictionary<string, InterpreterVariable>();
            InterpreterModule = new Interpreter(this);
            MainField = new FieldModel(width, height);
        }

        public void StartProgram()
        {
            MainField.ClearPositions();
            ClearVariables();
            InterpreterModule.StartProgram();
        }

        public void SetCommandsFromFlowDocument(FlowDocument document) => InterpreterModule.SetCommandList(document);
        public void SetCommandsFromStringList(List<string> list) => InterpreterModule.SetCommandList(list);
        public void SetCommands(List<InterpreterLine> lines) => InterpreterModule.SetCommandList(lines);

        public object GetValue(string name) =>
            InterpreterVariables[name];

        public void NewVariable(string name, InterpreterVariable value)
        {
            if (!InterpreterVariables.ContainsKey(name))
                InterpreterVariables.Add(name, value);
        }
        public bool ContainsVariable(string arg) =>
            InterpreterVariables.ContainsKey(arg);

        public void ClearVariables() =>
            InterpreterVariables.Clear();

        public object InterpreteArgument(string argstr)
        {
            if (ContainsVariable(argstr))
                return InterpreterVariables[argstr].Value;
            return argstr;
        }
    }
}
