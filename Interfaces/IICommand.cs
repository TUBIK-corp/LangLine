
using LangContext = LangLine.LangLineCore;

namespace LangLine.Interfaces
{
    public interface IICommand
    {
        LangContext Context { get; set; }
        string CommandName { get; }
        void StartCommand(string args);
    }
}
