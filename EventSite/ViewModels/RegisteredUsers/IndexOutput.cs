using System;
using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Model;

namespace EventSite.ViewModels.RegisteredUsers {
    public class IndexOutput<T> {
        const float ColumnCount = 6;

        public List<List<T>> Users { get; private set; }
        public string ActiveUserSlug { get; private set; }

        public IndexOutput(IEnumerable<T> speakers, string userSlug) {
            var list = speakers.ToList();
            var rows = new List<List<T>>();

            for(var i = 0; i < list.Count; i++) {
                var row = (int) Math.Floor(i/ColumnCount);
                var column = i - (ColumnCount*row);

                if(column == 0) {
                    rows.Add(new List<T>());
                }

                rows[row].Add(list[i]);
            }

            Users = rows;
            ActiveUserSlug = userSlug;
        }
    }
}