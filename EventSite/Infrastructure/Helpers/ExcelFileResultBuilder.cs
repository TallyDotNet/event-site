﻿using System;
using System.Collections.Generic;
using System.IO;
using EventSite.Domain.Infrastructure;
using EventSite.Infrastructure.Data.Export;

namespace EventSite.Infrastructure.Helpers
{
    public interface ITargetFile {
        bool Exists { get; }
        string FullPath { get; }
        FileInfo ToFileInfo();
    }

    public class FileInfoTargetFile : ITargetFile {
        private readonly FileInfo fileInfo;

        public FileInfoTargetFile(string filePath) {
            this.fileInfo = new FileInfo(filePath);
        }

        public FileInfoTargetFile(FileInfo file) {
            this.fileInfo = file;
        }

        public bool Exists {
            get { return fileInfo.Exists; }
        }

        public string FullPath {
            get { return fileInfo.FullName; }
        }

        public FileInfo ToFileInfo() {
            return fileInfo;
        }
    }

    public class ExcelFileBuilder {
        private readonly IApplicationBus bus;

        public ExcelFileBuilder(IApplicationBus bus) {
            this.bus = bus;
        } 

        public void Build<T>(Func<int,Query<Page<T>>> queryBuilder, IExcelExporter<T> exportCommand) {
            var data = ExecuteQuery(queryBuilder);
            exportCommand.Export(data);
        }

        public void Build<T>(Query<IEnumerable<T>> query, IExcelExporter<T> exportCommand)
        {
            var data = ExecuteQuery(query);
            exportCommand.Export(data);
        }

        private IEnumerable<T> ExecuteQuery<T>(Func<int, Query<Page<T>>> queryBuilder) {
            var pageNumber = 1;
            var result = new List<T>();
            Page<T> currentPage;

            do {
                var queryForNextPage = queryBuilder(pageNumber);
                currentPage = bus.Query(queryForNextPage);
                result.AddRange(currentPage.Items);
                pageNumber++;
            } while (currentPage.HasNextPage);

            return result;
        }       

        private IEnumerable<T> ExecuteQuery<T>(Query<IEnumerable<T>> query)
        {
            return bus.Query(query);
        }
    }
}