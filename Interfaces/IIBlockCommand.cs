using LangLine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Interfaces
{
    public interface IIBlockCommand : IICommand
    {

        List<InterpreterLine> Block { get; set; }
        string BlockEndName { get; }
        void ConfigureArguments(string str_arguments);
        int InitializeBlock(int index);
        void ExecuteBlock();
    }
}
