using System;
using System.Collections.Generic;
using System.Linq;
using EventSite.Domain.Model;

namespace EventSite.ViewModels.Speakers {
    public class IndexOutput {
        const float ColumnCount = 6;

        public List<List<Speaker>> Speakers { get; private set; }
        public string ActiveSpeakerSlug { get; private set; }

        public IndexOutput(IEnumerable<Speaker> speakers, string speakerSlug) {
            var list = speakers.ToList();
            var rows = new List<List<Speaker>>();

            for(var i = 0; i < list.Count; i++) {
                var row = (int) Math.Floor(i/ColumnCount);
                var column = i - (ColumnCount*row);

                if(column == 0) {
                    rows.Add(new List<Speaker>());
                }

                rows[row].Add(list[i]);
            }

            Speakers = rows;
            ActiveSpeakerSlug = speakerSlug;
        }
    }
}