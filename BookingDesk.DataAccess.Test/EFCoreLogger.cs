using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BookingDesk.DataAccess
{
    internal class EFCoreLogger : ILogger
    {
        private Action<string> _efcoreAction;
        private LogLevel _logLevel;

        public EFCoreLogger(Action<string> efcoreAction, LogLevel logLevel)
        {
            _efcoreAction = efcoreAction;
            _logLevel = logLevel;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return default!;
        }
       

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _logLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _efcoreAction($"Log level: {logLevel}, {state}");
        }
    }
}
