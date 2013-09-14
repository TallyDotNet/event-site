using System;
using System.IO;
using System.Web.Security;

namespace EventSite.Domain.Infrastructure {
    public interface ISecurityEncoder {
        string SerializeOAuthProviderUserId(string providerName, string providerUserId);
        bool TryDeserializeOAuthProviderUserId(string protectedData, out string providerName, out string providerUserId);
    }

    public class DefaultSecurityEncoder : ISecurityEncoder  {
        static readonly byte[] Padding = {0x85, 0xC5, 0x65, 0x72};

        public string SerializeOAuthProviderUserId(string providerName, string providerUserId) {
            using(var ms = new MemoryStream())
            using(var bw = new BinaryWriter(ms)) {
                bw.Write(providerName);
                bw.Write(providerUserId);
                bw.Flush();
                var serializedWithPadding = new byte[ms.Length + Padding.Length];
                Buffer.BlockCopy(Padding, 0, serializedWithPadding, 0, Padding.Length);
                Buffer.BlockCopy(ms.GetBuffer(), 0, serializedWithPadding, Padding.Length, (int) ms.Length);
                return MachineKey.Encode(serializedWithPadding, MachineKeyProtection.All);
            }
        }

        public bool TryDeserializeOAuthProviderUserId(string protectedData, out string providerName, out string providerUserId) {
            providerName = null;
            providerUserId = null;

            if(string.IsNullOrEmpty(protectedData)) {
                return false;
            }

            var decodedWithPadding = MachineKey.Decode(protectedData, MachineKeyProtection.All);
            if(decodedWithPadding == null || decodedWithPadding.Length < Padding.Length) {
                return false;
            }

            // timing attacks aren't really applicable to this, so we just do the simple check.
            for(var i = 0; i < Padding.Length; i++) {
                if(Padding[i] != decodedWithPadding[i]) {
                    return false;
                }
            }

            using(var ms = new MemoryStream(decodedWithPadding, Padding.Length, decodedWithPadding.Length - Padding.Length))
            using(var br = new BinaryReader(ms)) {
                try {
                    // use temp variable to keep both out parameters consistent and only set them when the input stream is read completely
                    var name = br.ReadString();
                    var userId = br.ReadString();
                    // make sure that we consume the entire input stream
                    if(ms.ReadByte() == -1) {
                        providerName = name;
                        providerUserId = userId;
                        return true;
                    }
                } catch {
                    // Any exceptions will result in this method returning false.
                }
            }

            return false;
        }
    }
}