using EventSite.Domain.Infrastructure;
using EventSite.Domain.Model;

namespace EventSite.Domain.Commands {
    public class CreateOrUpdateSponsor : Command<Result<Sponsor>>.AdminOnly {
        public Sponsor Sponsor { get; set; }
        public ISlugConverter SlugConverter { get; set; }

        public CreateOrUpdateSponsor() {
            Sponsor = new Sponsor();
        }

        protected override Result<Sponsor> Execute() {
            var toSave = Sponsor;
            var isNew = string.IsNullOrEmpty(toSave.Id);

            if (isNew) {
                if(string.IsNullOrEmpty(Sponsor.Name)) {
                    return PropertyError("Name", "Name is required.");
                }

                var id = Sponsor.IdFrom(
                    Event.SlugFromId(State.CurrentEvent.Id),
                    SlugConverter.ToSlug(Sponsor.Name)
                    );

                var existing = DocSession.Load<Sponsor>(id);

                if(existing != null) {
                    return Error("The requested sponsor name is not available.");
                }

                toSave.Id = id;
                DocSession.Store(toSave);
            }
            else {
                toSave = DocSession.Load<Sponsor>(toSave.Id);
                toSave.Name = Sponsor.Name;
            }

            return SuccessFormat("\"{0}\" was successfully {1}.", toSave.Name, isNew ? "created" : "updated")
                .WithSubject(toSave);
        }
    }
}