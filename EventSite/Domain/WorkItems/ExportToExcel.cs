using System.Collections.Generic;
using EventSite.Domain.Infrastructure;
using EventSite.Infrastructure.Data.Export;
using EventSite.Infrastructure.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace EventSite.Domain.WorkItems {

    public class ExportToExcel<T> : Work {

        private readonly ITargetFile targetFile;
        private readonly IEnumerable<T> dataSource;
        private readonly IExportColumnMappings<T> columns; 

        public ExportToExcel(ITargetFile targetFile, IEnumerable<T> dataSource, IExportColumnMappings<T> columns) {
            this.targetFile = targetFile;
            this.dataSource = dataSource;
            this.columns = columns;
        } 

        public override void Process() {
            using(var package = new ExcelPackage(targetFile.ToFileInfo())) {
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                ws.View.ShowGridLines = true;

                var rowCounter = 1;
                var columnCounter = 1;
                foreach(var column in columns.Columns) {
                    ws.Column(columnCounter).Width = 25;
                    ws.Cells[1, columnCounter].Value = column.Key;
                    ws.Cells[1, columnCounter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[1, columnCounter].Style.Font.Bold = true;
                    columnCounter++;
                }

                foreach(var row in dataSource) {
                    rowCounter++;
                    columnCounter = 1;
                    foreach(var column in columns.Columns) {
                        ws.Cells[rowCounter, columnCounter].Value = column.Value(row);
                        columnCounter++;
                    }
                }

                package.Save();
            }
        }
    }
}