using System;
using System.Text;
using System.Security.Cryptography;

namespace ECDsa
{
    /// <summary>
    /// ECDsa 非对称加密算法
    /// </summary>
    class ECDsa
    {
        internal static CngKey aliceKeySignature;
        internal static byte[] alicePubKeyBlob;
        internal static byte[] aliceSecKeyBlob;

        static void CreateKeys()
        {
            aliceKeySignature = CngKey.Create(CngAlgorithm.ECDsaP256, "Alice", new CngKeyCreationParameters() { ExportPolicy = CngExportPolicies.AllowPlaintextArchiving});
            alicePubKeyBlob = aliceKeySignature.Export(CngKeyBlobFormat.GenericPublicBlob);
            aliceSecKeyBlob = aliceKeySignature.Export(CngKeyBlobFormat.GenericPrivateBlob);
        }

        static byte[] CreateSignature(byte[] data, CngKey key)
        {
            byte[] signature;
            //这里直接用 CngKey 加密
            //using(var signingAlg = new ECDsaCng(key))
            //{
            //    signature = signingAlg.SignData(data);
            //    signingAlg.Clear();
            //}
            //这里用私钥重新生成CngKey对象
            using (CngKey pKey = CngKey.Import(aliceSecKeyBlob, CngKeyBlobFormat.GenericPrivateBlob))
            {
                using(var signingAlg = new ECDsaCng(pKey))
                {
                    signature = signingAlg.SignData(data);
                    signingAlg.Clear();
                }
            }
            return signature;
        }

        static bool VerifySignature(byte[] data, byte[] signature, byte[] pubKey)
        {
            bool retValue = false;
            using(CngKey key = CngKey.Import(pubKey, CngKeyBlobFormat.GenericPublicBlob))
            {
                using(var signingAlg = new ECDsaCng(key))
                {
                    retValue = signingAlg.VerifyData(data, signature);
                    signingAlg.Clear();
                }
            }
            return retValue;
        }

        static void Main()
        {
            CreateKeys();

            byte[] aliceData = Encoding.UTF8.GetBytes("Alice");
            byte[] aliceSignature = CreateSignature(aliceData, aliceKeySignature);
            Console.WriteLine("Alice created signature: {0}", Convert.ToBase64String(aliceSignature));

            if(VerifySignature(aliceData, aliceSignature, alicePubKeyBlob))
            {
                Console.WriteLine("Alice signature verified successful");
            }
        }
    }
}
