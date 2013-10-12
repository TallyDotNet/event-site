using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using EventSite.Domain.Infrastructure;
using OfficeOpenXml;

namespace EventSite.Domain.Commands
{
    public abstract class ExportToExcel<T> : Work {

        protected abstract IEnumerable<T> DataSource { get; }

        protected abstract IDictionary<string, Func<string>> Columns { get; }

        protected abstract FileInfo TargetFile { get; }

        public override void Process() {
            using (var package = new ExcelPackage(TargetFile))
            {
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                ws.View.ShowGridLines = true;

                var columnCounter = 1;
                foreach (var column in Columns)
                {
                    ws.Cells[1, columnCounter].Value = column.Key;
                    columnCounter++;
                }

                package.Save();
            }   
        }
    }
}