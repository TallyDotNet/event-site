using System;
using System.Collections.Generic;
using System.IO;
using EventSite.Domain.Infrastructure;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace EventSite.Domain.Commands
{
    public abstract class ExportToExcel<T> : Work {

        protected abstract IEnumerable<T> DataSource { get; }

        protected abstract IDictionary<string, Func<T, object>> Columns { get; }

        protected abstract FileInfo TargetFile { get; }

        public override void Process() {
            using (var package = new ExcelPackage(TargetFile))
            {
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                ws.View.ShowGridLines = true;

                var rowCounter = 1;
                var columnCounter = 1;
                foreach (var column in Columns)
                {
                    ws.Column(columnCounter).Width = 25;
                    ws.Cells[1, columnCounter].Value = column.Key;
                    ws.Cells[1, columnCounter].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells[1, columnCounter].Style.Font.Bold = true;
                    columnCounter++;
                }

                foreach (var row in DataSource)
                {
                    rowCounter++;
                    columnCounter = 1;
                    foreach (var column in Columns)
                    {
                        ws.Cells[rowCounter, columnCounter].Value = column.Value(row);
                        columnCounter++;
                    }
                }

                package.Save();
            }   
        }
    }
}