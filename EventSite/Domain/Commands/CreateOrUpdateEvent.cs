using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;

namespace EventSite.Domain.Commands
{
    public class CreateOrUpdateEvent : Command<Result<Event>>.AdminOnly
    {
        public Event Event { get; set; }
        public string Slug { get; set; }

        public ISlugConverter SlugConverter { get; set; }

        public CreateOrUpdateEvent()
        {
            Event = new Event
            {
                Start = Now(),
                End = Now()
            };
        }

        protected override Result<Event> Execute()
        {
            var toSave = Event;
            var isNew = string.IsNullOrEmpty(toSave.Id);

            if (isNew)
            {
                if (string.IsNullOrEmpty(Slug))
                {
                    return PropertyError("Slug", "Slug is required.");
                }

                var id = Event.IdFrom(SlugConverter.ToSlug(Slug));
                var existing = DocSession.Load<Event>(id);

                if (existing != null)
                {
                    return Error("The requested event slug is not available.");
                }

                toSave.Id = id;
                DocSession.Store(toSave);
            }
            else {
                toSave = DocSession.Load<Event>(toSave.Id);
                toSave.Name = Event.Name;
                toSave.Start = Event.Start;
                toSave.End = Event.End;
                toSave.Description = Event.Description;
                toSave.IsSessionSubmissionOpen = Event.IsSessionSubmissionOpen;
                toSave.Venue = Event.Venue;
                toSave.ScheduleSummary = Event.ScheduleSummary;
                toSave.FlyerUrl = Event.FlyerUrl;
                toSave.ScheduleUrl = Event.ScheduleUrl;
            }

            if (toSave.IsCurrent)
            {
                State.ChangeCurrentEvent(toSave);
            }

            return SuccessFormat("\"{0}\" was successfully {1}.", toSave.Name, isNew ? "created" : "updated")
                .WithSubject(toSave);
        }
    }
}