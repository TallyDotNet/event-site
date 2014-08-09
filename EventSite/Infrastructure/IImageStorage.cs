using System.Configuration;
using System.IO;
using System.Web;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace EventSite.Infrastructure {
    public interface IImageStorage {
        string GetUrl(string source);
        void Remove(string source);
        string Store(string name, Stream stream);
    }

    //For Development Only
    public class LocalImageStorage : IImageStorage {
        const string Location = "/Content/localImageStorage/";

        public string GetUrl(string source) {
            return Location + source;
        }

        public void Remove(string source) {
            if (string.IsNullOrEmpty(source)) {
                return;
            }

            var deletePath = HttpContext.Current.Server.MapPath("~" + Location + source);
            File.Delete(deletePath);
        }

        public string Store(string name, Stream stream) {
            var savePath = HttpContext.Current.Server.MapPath("~" + Location + name);

            createFolderIfNeeded(savePath);

            using(var file = File.OpenWrite(savePath)) {
                stream.CopyTo(file);
            }

            return name;
        }

        static void createFolderIfNeeded(string filename) {
            var folder = Path.GetDirectoryName(filename);
            if(!Directory.Exists(folder)) {
                Directory.CreateDirectory(folder);
            }
        }
    }

    //For Production
    public class CloudinaryImageStorage : IImageStorage {
        readonly Cloudinary cloudinary;

        public CloudinaryImageStorage() {
            cloudinary = new Cloudinary(ConfigurationManager.AppSettings.Get("CLOUDINARY_URL"));
        }

        public string Store(string name, Stream stream) {
            var upload = new ImageUploadParams {
                File = new FileDescription(name, stream)
            };

            var result = cloudinary.Upload(upload);

            return string.Format("{0}.{1}", result.PublicId, result.Format);
        }

        public string GetUrl(string source) {
            return cloudinary.Api.UrlImgUp.BuildUrl(source);
        }

        public void Remove(string source) {
            var result = cloudinary.Destroy(new DeletionParams(Path.GetFileNameWithoutExtension(source)));
        }
    }
}