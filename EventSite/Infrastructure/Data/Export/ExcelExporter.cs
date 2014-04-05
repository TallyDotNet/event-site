using System.Collections.Generic;
using EventSite.Domain.Infrastructure;
using EventSite.Domain.WorkItems;
using EventSite.Infrastructure.Helpers;

namespace EventSite.Infrastructure.Data.Export {

    public interface IExcelExporter<T> {
        void Export(IEnumerable<T> items);
    }

    public class ExcelExporter<T> : IExcelExporter<T> {

        private readonly IExportColumnMappings<T> columns;
        private readonly ITargetFile targetFile;
        private readonly IApplicationBus bus;

        public ExcelExporter(IExportColumnMappings<T> columns, ITargetFile targetFile, IApplicationBus bus) {
            this.columns = columns;
            this.targetFile = targetFile;
            this.bus = bus;
        }

        public void Export(IEnumerable<T> items) {
            var exportCommand = new ExportToExcel<T>(targetFile, items, columns);
            bus.Do(exportCommand);
        }
    }
}