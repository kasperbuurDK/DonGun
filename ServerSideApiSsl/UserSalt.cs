using SharedClassLibrary;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ServerSideApiSsl
{
    public class UserSalt : User
    {
        public byte[] Salt { get; set; }
        public byte[] HashedPass { get; set; }

        public UserSalt()
        {
            Salt = new byte[32];
            HashedPass = new byte[32];
        }

        public UserSalt(byte[] salt, string password)
        {
            Salt = salt;
            HashedPass = SaltedHash(password);
        }

        public byte[] SaltedHash(string pass)
        {
#pragma warning disable SYSLIB0021 // Type or member is obsolete
            HashAlgorithm algorithm = new SHA256Managed();
#pragma warning restore SYSLIB0021 // Type or member is obsolete
            byte[] bytesPass = Encoding.ASCII.GetBytes(pass);

            byte[] passWithSalt = new byte[bytesPass.Length + Salt.Length];

            for (int i = 0; i < bytesPass.Length; i++)
            {
                passWithSalt[i] = bytesPass[i];
            }
            for (int i = 0; i < Salt.Length; i++)
            {
                passWithSalt[bytesPass.Length + i] = Salt[i];
            }

            return algorithm.ComputeHash(passWithSalt);
        }

        public bool CompareHashed(byte[] hash)
        {
            if (HashedPass is null)
                return false;
            if (HashedPass.Length != hash.Length)
            {
                return false;
            }

            for (int i = 0; i < hash.Length; i++)
            {
                if (HashedPass[i] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static byte[] CreateSalt()
        {
#pragma warning disable SYSLIB0023 // Type or member is obsolete
            using RNGCryptoServiceProvider rng = new();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);
            return buff;
#pragma warning restore SYSLIB0023 // Type or member is obsolete
        }
    }
}
