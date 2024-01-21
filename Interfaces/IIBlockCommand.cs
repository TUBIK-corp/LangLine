using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IspolnitelCherepashka.Interfaces
{
    public interface IIBlockCommand : IICommand
    {
        void ConfigureArguments(string str_arguments);
        int InitializeBlock(int index);
        void ExecuteBlock();
    }
}
