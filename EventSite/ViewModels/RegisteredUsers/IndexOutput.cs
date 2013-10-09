using System;
using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Infrastructure;

namespace EventSite.ViewModels.RegisteredUsers {
    public class IndexOutput<T> {
        const float ColumnCount = 6;

        public Page<T> Page { get; private set; }
        public string EventSlug { get; private set; }
        public List<List<T>> UserRows { get; private set; }
        public string ActiveUserSlug { get; private set; }

        public IndexOutput(Page<T> users, string userSlug, string eventSlug) {
            var list = users.Items.ToList();
            var rows = new List<List<T>>();

            for(var i = 0; i < list.Count; i++) {
                var row = (int) Math.Floor(i/ColumnCount);
                var column = i - (ColumnCount*row);

                if(column == 0) {
                    rows.Add(new List<T>());
                }

                rows[row].Add(list[i]);
            }

            Page = users;
            EventSlug = eventSlug;
            UserRows = rows;
            ActiveUserSlug = userSlug;
        }
    }
}