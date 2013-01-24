namespace Castra.Web.Commands
{
	using BlueSpire.Kernel;
	using BlueSpire.Kernel.Bus;
	using BlueSpire.Kernel.Data;
	using BlueSpire.Web.Mvc.Infrastructure;
	using BlueSpire.Web.Mvc.Membership;
	using Caliburn.Core.Validation;
	using Models;

	public class EditProfileHandler : CommandHandler<EditProfile>
	{
		private readonly IContextProvider context;
		private readonly IRepository repository;

		public EditProfileHandler(IContextProvider context, IRepository repository, IValidator validator)
			: base(validator)
		{
			this.context = context;
			this.repository = repository;
		}

		protected override Result Handle(EditProfile command)
		{
			var profile = repository.Get(new QueryById<Profile> {Id = context.User.Id});
			if (profile.NotFound())
			{
				profile = new Profile
				          	{
				          		Account = repository.Reference<User>(context.User.Id)
				          	};
				CopyProperties(profile, command);

				repository.Add(profile);
			}
			else
			{
				CopyProperties(profile, command);
			}

			return Success();
		}

		private static void CopyProperties(Profile profile, EditProfile form)
		{
			profile.Bio = form.Bio;
			profile.BlogUrl = form.BlogUrl;
			profile.CompanyName = form.CompanyName;
			profile.CompanyUrl = form.CompanyUrl;
			profile.IsMVP = form.IsMVP;
			profile.Location = form.Location;
			profile.Name = form.Name;
			profile.Phone = form.Phone;
			profile.ShirtSize = form.ShirtSize;
			profile.Title = form.Title;
			profile.Twitter = form.Twitter;
			profile.IsPublic = form.IsPublic;
			profile.OptOut = form.OptOut;

			profile.LookupName = profile.Name.DeriveLookupName();
		}
	}
}