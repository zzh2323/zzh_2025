using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Dal
{
    public class AesEncryption
    {
        RandomNum randomNum = new RandomNum();
        public (byte[] key, byte[] iv) GenerateKeyAndIV()
        {
            byte[] key = randomNum.Generate(256); // 256 位密钥
            byte[] iv = randomNum.Generate(128); // 128 位初始化向量
            return (key, iv);
        }
        // 调用加密或解密方法
        public string UseAes(string Tmp, bool isEncrypt, byte[] key, byte[] iv)
        {
            
            if (isEncrypt)
            {
                byte[] encryptedBytes = Encrypt(Tmp, key, iv);
                //加密结果
                string encryptedBase64 = Convert.ToBase64String(encryptedBytes);
                return encryptedBase64;
            }
            else 
            {
                byte[] decryptedBytes = Convert.FromBase64String(Tmp);
                //解密结果
                return Decrypt(decryptedBytes, key, iv);
            }
            
        }

        // 加密
        public  byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }

        // 解密
        public string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
