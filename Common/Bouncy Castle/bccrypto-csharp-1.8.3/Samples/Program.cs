using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
           
            string text = "This is a test message!_________________";
            string keyStr = "12345678901234567890123456789012";

            string str = Encrypt(keyStr, text);
            string s = Decrypt(keyStr, str);
            Console.WriteLine(s);

        }


       static IBlockCipher engine = new ThreefishEngine( ThreefishEngine.BLOCKSIZE_256);



        public static string Encrypt(String keys, string clearText)
        {
            byte[] key = Encoding.ASCII.GetBytes(keys);
            byte[] ptBytes = Encoding.ASCII.GetBytes(clearText);
            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(engine));
            cipher.Init(true, new KeyParameter(key));
            byte[] rv = new byte[cipher.GetOutputSize(ptBytes.Length)];
            int tam = cipher.ProcessBytes(ptBytes, 0, ptBytes.Length, rv, 0);
            try
            {
                cipher.DoFinal(rv, tam);
            }
            catch (Exception ce)
            {
                //ce.printStackTrace();
            }
            return Convert.ToBase64String(rv);
        }

        public static string Decrypt(String key2, string cryptedText)
        {
            byte[] key = Encoding.ASCII.GetBytes(key2);
            byte[] cipherText = Convert.FromBase64String(cryptedText);
            BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(engine));
            
            cipher.Init(false, new KeyParameter(key));
            byte[] rv = new byte[cipher.GetOutputSize(cipherText.Length)];
            int tam = cipher.ProcessBytes(cipherText, 0, cipherText.Length, rv, 0);
            try
            {
                cipher.DoFinal(rv, tam);
            }
            catch (Exception ce)
            {
                //ce.printStackTrace();
            }
            return Encoding.ASCII.GetString(rv).Trim();
        }

    }
}
