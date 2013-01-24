namespace Castra.Web.Controllers.Scaffold
{
	using System.Linq;
	using System.Web;
	using System.Web.Mvc;
	using BlueSpire.Kernel;
	using BlueSpire.Web.Mvc.Membership;
	using BlueSpire.Web.Mvc.Scaffold;
	using Models;
	using NHibernate;

	public class AdminBlobController : ScaffoldController<Blob>
	{
		public AdminBlobController(ISession session)
			: base(session)
		{
		}

		[HttpPost]
		[IsAdmin]
		public ActionResult Upload()
		{
			var file = GetFileById("Data");
			var len = file.ContentLength;
			var data = new byte[len];
			file.InputStream.Read(data, 0, len);

			var blob = new Blob
			           	{
			           		Data = data,
			           		ContentType = Enumeration.FromDisplayName(file.ContentType, ContentType.Unaccepted)
			           	};

			return Create(blob);
		}

		private HttpPostedFileBase GetFileById(string fileId)
		{
			var file = from key in Request.Files.AllKeys
			           where key == fileId
			           select Request.Files[key];

			return file.FirstOrDefault();
		}
	}
}