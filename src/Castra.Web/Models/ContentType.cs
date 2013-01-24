namespace Castra.Web.Models
{
	using System;
	using BlueSpire.Kernel;
	using System.Linq;

	[Serializable]
	public class ContentType : Enumeration
    {
        public static ContentType Unaccepted = new ContentType(0, "Unaccepted", string.Empty);

        public static ContentType Gif = new ContentType(1, "image/gif", ".gif");
        public static ContentType Jpeg = new ContentType(2, "image/jpeg", ".jpg");
        public static ContentType Png = new ContentType(3, "image/png", ".png");
        public static ContentType ProgressiveJpeg = new ContentType(4, "image/pjpeg", ".jpg");
        public static ContentType XPng = new ContentType(5, "image/x-png", ".png");

        public static ContentType Xap = new ContentType(6, "application/x-silverlight-app", ".xap");
        public static ContentType Manifest = new ContentType(7, "application/manifest", ".manifest");
        public static ContentType Xaml = new ContentType(8, "application/xaml+xml", ".xaml");
        public static ContentType Dll = new ContentType(9, "application/x-msdownload", ".dll");
        public static ContentType Application = new ContentType(10, "application/x-ms-application", ".application");
        public static ContentType Xbap = new ContentType(11, "application/x-ms-xbap", ".xbap");
        public static ContentType Deploy = new ContentType(12, "application/octet-stream", ".xap");
        public static ContentType Zip = new ContentType(13, "application/x-zip-compressed", ".xap");

        public static ContentType Xml = new ContentType(14, "text/xml", ".xml");

        public static ContentType Mp3 = new ContentType(15,"audio/mpeg", ".mp3");
        public static ContentType Wma = new ContentType(16, "audio/x-ms-wma", ".wma");
        public static ContentType Wmv = new ContentType(17, "video/x-ms-wmv", ".wmv");

        private readonly string _fileExtension;

        public ContentType(int value, string displayName, string fileExtension)
            : base(value, displayName)
        {
            _fileExtension = fileExtension;
        }

        public string FileExtension
        {
            get { return _fileExtension; }
        }

        public bool IsImage
        {
            get { return this == Gif || this == Jpeg || this == Png || this == ProgressiveJpeg || this == XPng; }
        }

        public bool IsSilverlightApplication
        {
            get { return this == Xap || this == Deploy || this == Zip; }
        }

        public static ContentType FromExtension(string extension)
        {
            return All<ContentType>()
                .Where(x => x.FileExtension == extension)
                .FirstOrDefault();
        }
    }
}