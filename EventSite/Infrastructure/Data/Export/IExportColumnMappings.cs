using System;
using System.Collections.Generic;

namespace EventSite.Infrastructure.Data.Export {

    public interface IExportColumnMappings<T> {
        IDictionary<string, Func<T, object>> Columns { get; } 
    }
}