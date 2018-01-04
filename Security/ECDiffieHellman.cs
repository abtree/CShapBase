using System;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace ECDiffieHellman
{
    class ECDiffieHellman
    {
        static CngKey aliceKey;
        static CngKey bobKey;
        static byte[] alicePubKey;
        static byte[] bobPubKey;
        
        static void Main()
        {
            Run();
            Console.ReadKey();
        }

        private async static void Run()
        {
            try
            {
                CreateKeys();

                byte[] encrytpedData = await AliceSendsData("secret message");
                BobReceivesData(encrytpedData);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void CreateKeys()
        {
            //这里生成非对称的私钥 并导出公钥
            aliceKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256);
            bobKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP256);
            alicePubKey = aliceKey.Export(CngKeyBlobFormat.EccPublicBlob);
            bobPubKey = bobKey.Export(CngKeyBlobFormat.EccPublicBlob);
        }

        private async static Task<byte[]> AliceSendsData(string message)
        {
            Console.WriteLine("Alice sends message: {0}", message);
            byte[] rawData = Encoding.UTF8.GetBytes(message);
            byte[] encryptedData = null;

            using(var aliceAlgorithm = new ECDiffieHellmanCng(aliceKey))
            {
                using(CngKey bobPKey = CngKey.Import(bobPubKey, CngKeyBlobFormat.EccPublicBlob))
                {
                    //这里混合生成对称的AES密钥 和下面生成的相同
                    byte[] symkey = aliceAlgorithm.DeriveKeyMaterial(bobPKey);
                    Console.WriteLine("Alice creates this symmetric key with Bobs public key information: {0}", Convert.ToBase64String(symkey));

                    using(var aes = new AesCryptoServiceProvider())
                    {
                        aes.Key = symkey;
                        aes.GenerateIV();  //随机一个IV矢量
                        using(ICryptoTransform encryptor = aes.CreateEncryptor())
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                //create cryptostream and encrypt data to send
                                var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                                //write initialization vector not encrypted
                                await ms.WriteAsync(aes.IV, 0, aes.IV.Length);  //非加密的IV
                                cs.Write(rawData, 0, rawData.Length);  //加密的数据
                                cs.Close();
                                encryptedData = ms.ToArray();
                            }
                        }
                        aes.Clear();
                    }
                }
            }
            Console.WriteLine("Alice: message is encrypted: {0}", Convert.ToBase64String(encryptedData));
            Console.WriteLine();
            return encryptedData;
        }

        private static void BobReceivesData(byte[] encryptedData)
        {
            Console.WriteLine("Bob receives encrypted data");
            byte[] rawData = null;

            var aes = new AesCryptoServiceProvider();

            int nBytes = aes.BlockSize / 8;   //获取IV长度
            byte[] iv = new byte[nBytes];
            for(int i = 0; i < iv.Length; ++i)
            {
                iv[i] = encryptedData[i];  //读出非加密的IV
            }

            using(var bobAlgorithm = new ECDiffieHellmanCng(bobKey))
            {
                using(CngKey alicePKey = CngKey.Import(alicePubKey, CngKeyBlobFormat.EccPublicBlob))
                {
                    //混合生成AES密钥 和上面生成的相同
                    byte[] symkey = bobAlgorithm.DeriveKeyMaterial(alicePKey);
                    Console.WriteLine("Bob creates this symmetric key with Alices public key information: {0}", Convert.ToBase64String(symkey));
                    aes.Key = symkey;
                    aes.IV = iv;

                    using(ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        using(MemoryStream ms = new MemoryStream())
                        {
                            var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write);
                            cs.Write(encryptedData, nBytes, encryptedData.Length - nBytes);  //解密数据
                            cs.Close();

                            rawData = ms.ToArray();

                            Console.WriteLine("Bob decrypts message to: {0}", Encoding.UTF8.GetString(rawData)); //转换为字符串并打印
                        }
                    }
                    aes.Clear();
                }
            }
        }
    }
}
