namespace Castra.Web.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;
	using System.Web.Mvc;

	public class MainMenu
	{
		private readonly Dictionary<string, Tuple<string, string>> items = new Dictionary<string, Tuple<string, string>>();

		public MainMenu()
		{
			Add("Speakers", (SpeakersController x) => x.Index(1));
			Add("Sessions", (SessionController x) => x.Index(1,"level"));
			Add("Sponsors", (SponsorsController x) => x.Index());
			Add("Location", (HomeController x) => x.Location());
			Add("Attend", (RegistrationController x) => x.Attend(null));
			Add("Speak", (AccountController x) => x.New(null));
		}

		public IEnumerable<MainMenuEntry> Items
		{
			get
			{
				foreach (var key in items.Keys)
				{
					yield return new MainMenuEntry
					             	{
					             		Label = key,
					             		Controller = items[key].Item1,
					             		Action = items[key].Item2
					             	};
				}
			}
		}

		private void Add<T>(string label, Expression<Func<T, ActionResult>> expression)
		{
			var m = (MethodCallExpression) expression.Body;

			var controller = m.Object.Type.Name.Replace("Controller", "");
			var action = m.Method.Name;

			items.Add(label, new Tuple<string, string>(controller, action));
		}
	}

	public struct MainMenuEntry
	{
		public string Label { get; set; }
		public string Controller { get; set; }
		public string Action { get; set; }
	}
}