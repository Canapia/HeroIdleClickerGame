using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;

using UnityEngine;

public static class AESSecurity
{

    /*
    public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
        {
            throw new ArgumentNullException("plainText");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        byte[] encrypted;

        // Create an Aes Object
        // with the specified key and IV.
        // 지정된 Key와 IV로 Aes 오브젝트 생성
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            // 스트림 변환을 수행 할 암호화기 작성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            // 암호화에 사용되는 스트림 작성
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write all data to the stream.
                        // 모든 데이터를 스트림에 작성
                        swEncrypt.WriteLine(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        // 메모리 스트림에서 암호화된 바이트를 반환
        return encrypted;
    }
    */

    public static byte[] Encrypt_ToBytes_Aes<T>(T plainArgu, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainArgu == null || plainArgu.ToString().Length <= 0)
        {
            throw new ArgumentNullException("plainArgu");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");

        }
        byte[] encrypted;

        // Create an Aes Object
        // with the specified key and IV.
        // 지정된 Key와 IV로 Aes 오브젝트 생성
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            // 스트림 변환을 수행 할 암호화기 작성
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            // 암호화에 사용되는 스트림 작성
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        // Write all data to the stream.
                        // 모든 데이터를 스트림에 작성
                        swEncrypt.WriteLine(plainArgu);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        // 메모리 스트림에서 암호화된 바이트를 반환
        return encrypted;
    }

    public static List<byte[]> Encrypt_ArrayToBytes_Aes<T>(T[] plainArray, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainArray == null || plainArray.Length <= 0)
        {
            throw new ArgumentNullException("plainArray");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        List<byte[]> encrypted = new List<byte[]>();

        for (int i = 0; i < plainArray.Length; i++)
        {
            encrypted.Add(Encrypt_ToBytes_Aes<T>(plainArray[i], Key, IV));
        }

        // Return the encrypted bytes from the memory stream.
        // 메모리 스트림에서 암호화된 바이트를 반환
        return encrypted;
    }

    #region FromBytes
    public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
        {
            throw new ArgumentNullException("cipherText");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        // Declare the string used to hold
        // the decrypted text.
        // 해독 된 텍스트를 보관하는 데 사용되는 문자열 선언
        string plaintext = null;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            // 스트림 변환을 수행 할 암호 해독기 생성
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            // 암호 해독에 사용되는 스트림 생성
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        // 해독 스트림에서 해독 된 바이트를 읽고 문자열에 배치
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        return plaintext;
    }

    public static int DecryptIntFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if(cipherText == null || cipherText.Length <= 0)
        {
            throw new ArgumentNullException("cipherText");
        }
        if(Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if(IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        // 해독 된 텍스트를 보관하는 정수 선언
        int plainInt;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            // 스트림 변환을 수행 할 암호 해독기 생성
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            // 암호 해독에 사용되는 스트림 생성
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // 해독 스트림에서 해독 된 바이트를 읽고 정수에 배치
                        plainInt = Convert.ToInt32(srDecrypt.ReadToEnd());
                    }
                }
            }
        }

        return plainInt;
    }

    public static bool DecryptBoolFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
        {
            throw new ArgumentNullException("cipherText");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        // 해독 된 텍스트를 보관하는 Boolean 형식 선언
        bool plainBool;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            // 스트림 변환을 수행 할 암호 해독기 생성
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            // 암호 해독에 사용되는 스트림 생성
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // 해독 스트림에서 해독 된 바이트를 읽고 Boolean에 배치
                        plainBool = Convert.ToBoolean(srDecrypt.ReadToEnd());
                    }
                }
            }
        }

        return plainBool;
    }

    public static float DecryptFloatFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
        {
            throw new ArgumentNullException("cipherText");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        // 해독 된 텍스트를 보관하는 부동 소수점 형식 선언
        float plainFloat;

        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            // 스트림 변환을 수행 할 암호 해독기 생성
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            // 암호 해독에 사용되는 스트림 생성
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        // 해독 스트림에서 해독 된 바이트를 읽고 부동 소수점에 배치
                        plainFloat = float.Parse(srDecrypt.ReadToEnd());
                    }
                }
            }
        }

        return plainFloat;
    }

    // Array
    public static string[] DecryptStringArrayFromBytes_Aes(List<byte[]> cipherArray, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherArray == null || cipherArray.Count <= 0)
        {
            throw new ArgumentNullException("cipherArray");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        // Declare the string used to hold
        // the decrypted text.
        // 해독 된 텍스트를 보관하는 데 사용되는 문자열 선언
        string[] plaintext = new string[cipherArray.Count];

        for (int i = 0; i < cipherArray.Count; i++)
        {
            plaintext[i] = DecryptStringFromBytes_Aes(cipherArray[i], Key, IV);
        }

        return plaintext;
    }

    public static int[] DecryptIntArrayFromBytes_Aes(List<byte[]> cipherArray, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherArray == null || cipherArray.Count <= 0)
        {
            throw new ArgumentNullException("cipherArray");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        // Declare the string used to hold
        // the decrypted text.
        // 해독 된 텍스트를 보관하는 데 사용되는 문자열 선언
        int[] plainInt = new int[cipherArray.Count];

        for (int i = 0; i < cipherArray.Count; i++)
        {
            plainInt[i] = DecryptIntFromBytes_Aes(cipherArray[i], Key, IV);
        }

        return plainInt;
    }

    public static bool[] DecryptBoolArrayFromBytes_Aes(List<byte[]> cipherArray, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (cipherArray == null || cipherArray.Count <= 0)
        {
            throw new ArgumentNullException("cipherArray");
        }
        if (Key == null || Key.Length <= 0)
        {
            throw new ArgumentNullException("Key");
        }
        if (IV == null || IV.Length <= 0)
        {
            throw new ArgumentNullException("IV");
        }

        // Declare the string used to hold
        // the decrypted text.
        // 해독 된 텍스트를 보관하는 데 사용되는 문자열 선언
        bool[] plainBool = new bool[cipherArray.Count];

        for (int i = 0; i < cipherArray.Count; i++)
        {
            plainBool[i] = DecryptBoolFromBytes_Aes(cipherArray[i], Key, IV);
        }

        return plainBool;
    }
    #endregion
}
