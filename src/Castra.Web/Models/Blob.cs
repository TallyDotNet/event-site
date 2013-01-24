namespace Castra.Web.Models
{
	using System;
	using BlueSpire.Kernel.Data;
	using BlueSpire.NHibernate;

	public class Blob : Entity<Guid>
	{
		public virtual byte[] Data { get; set; }
		public virtual ContentType ContentType { get; set; }
	}

	public class BlobMap : EntityMap<Blob>
	{
		public BlobMap()
		{
			Not.LazyLoad();

			Map(x => x.ContentType);

			Map(x => x.Data)
				.CustomType("BinaryBlob")
				.CustomSqlType("Image");
		}
	}
}