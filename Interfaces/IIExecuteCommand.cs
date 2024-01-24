using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Interfaces
{
    public interface IIExecuteCommand : IICommand
    {
        void ConfigureArguments(string str_arguments);
        void Execute();
    }
}
