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
            HashAlgorithm algorithm = new SHA256Managed();
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
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);
            return buff;
        }
    }
}
