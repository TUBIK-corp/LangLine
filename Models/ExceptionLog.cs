using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LangLine.Models
{
    public class ExceptionLog
    {
        public int Line { get; set; }
        public string ExceptionMessage { get; set; }
        public Exception Exception { get; set; }

        public ExceptionLog(int line, Exception exception)
        {
            Line = line+1;
            Exception = exception;
            ExceptionMessage = exception.Message;
        }

        public ExceptionLog(int line, Exception exception, string exceptionMessage)
        {
            Line = line+1;
            ExceptionMessage = exceptionMessage;
            Exception = exception;
        }

    }
}
