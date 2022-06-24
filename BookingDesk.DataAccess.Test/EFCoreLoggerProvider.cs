using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BookingDesk.DataAccess
{
    public class EFCoreLoggerProvider : ILoggerProvider
    {
        private readonly LogLevel _logLevel;
        private readonly Action<string> _efCoreAction;

        internal EFCoreLoggerProvider(Action<string> efCoreAction, LogLevel loglevel = LogLevel.Information)
        {
            _logLevel = loglevel;
            _efCoreAction = efCoreAction;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new EFCoreLogger(_efCoreAction, _logLevel);
        }

        public void Dispose()
        {
            //nothing to dispose.
        }
    }
}
