using System;
using NLog;

namespace EventSite.Infrastructure.Logging {
    public class NLogModule : LogModule<Logger> {
        protected override Logger CreateLoggerFor(Type type) {
            return LogManager.GetLogger(type.FullName);
        }
    }
}