using System.Linq;
using CodeCamp.Domain.Infrastructure;
using CodeCamp.Domain.Model;
using Raven.Client.Indexes;

namespace CodeCamp.Domain.Queries {
    public class CurrentEvent : Query<Event> {
        protected override Event Execute() {
            var currentEvent = DocSession.Query<Event, CurrentEventIndex>()
                .SingleOrDefault(x => x.IsCurrent);

            return currentEvent;
        }

        public class CurrentEventIndex : AbstractIndexCreationTask<Event> {
            public CurrentEventIndex() {
                Map = events =>
                    from ev in events
                    select new {ev.IsCurrent};
            }
        }
    }
}