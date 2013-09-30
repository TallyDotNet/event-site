using System.Configuration;
using System.IO;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace EventSite.Infrastructure {
    public interface IImageStorage {
        string GetUrl(string source);
        void Remove(string source);
        string Store(string name, Stream stream);
    }

    public class DefaultImageStorage : IImageStorage {
        readonly Cloudinary cloudinary;

        public DefaultImageStorage() {
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