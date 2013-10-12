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

        protected abstract string TargetFileName { get; }

        public override void Process() {
            var excelFile = new FileInfo(TargetFileName);

            using (var package = new ExcelPackage(excelFile))
            {
                var ws = package.Workbook.Worksheets.Add("Attendees");
                ws.View.ShowGridLines = true;

                var columnCounter = 0;
                foreach (var column in Columns)
                {
                    ws.Cells[columnCounter, 1].Value = column.Key;
                    columnCounter++;
                }

                package.Save();
            }   
        }
    }
}