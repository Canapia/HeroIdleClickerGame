using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Linq;

using UnityEngine;

public static class SaveSystem
{
    public static void SaveUserData(User _user)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        /*
        // PC
        string path = Application.persistentDataPath + "/USer.fun";
        FileStream fs = new FileStream(path, FileMode.Create);

        UserData data = new UserData(_user);

        formatter.Serialize(fs, data);
        fs.Close();
        */

        //Android
        string path = Application.persistentDataPath + "/User.fun";
        Debug.Log(path);
        FileStream fs = new FileStream(path, FileMode.Create);

        UserData data = new UserData(_user);

        //List<object> EncryptData = new List<object>();

        //using (Aes aesAlg = Aes.Create())
        //{
        //    SaveAESKeyAndIV(aesAlg.Key, aesAlg.IV);
        //    EncryptData = UserDataEncrypt(data, aesAlg.Key, aesAlg.IV);
        //}

        formatter.Serialize(fs, data);
        fs.Close();
    }

    public static UserData LoadUserData()
    {
        string path = Application.persistentDataPath + "/User.fun";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            //List<object> data = formatter.Deserialize(fs) as List<object>;

            //UserData DecryptData = UserDataDecrypt(data, LoadAESKeyAndIV().Key, LoadAESKeyAndIV().IV);
            UserData data = formatter.Deserialize(fs) as UserData;
            fs.Close();

            return data;
        }
        else
        {
            //Debug.LogError("Save File not found in " + path);
            return null;
        }
    }

    public static void SaveAESKeyAndIV(byte[] Key, byte[] IV)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        //Android
        string path = Application.persistentDataPath + "/AESKeyAndIV.fun";
        Debug.Log(path);
        FileStream fs = new FileStream(path, FileMode.Create);

        AESKeyAndIV data = new AESKeyAndIV(Key, IV);

        formatter.Serialize(fs, data);
        fs.Close();
    }

    public static AESKeyAndIV LoadAESKeyAndIV()
    {
        string path = Application.persistentDataPath + "/AESKeyAndIV.fun";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            AESKeyAndIV data = formatter.Deserialize(fs) as AESKeyAndIV;
            fs.Close();

            return data;
        }
        else
        {
            //Debug.LogError("Save File not found in " + path);
            return null;
        }
    }

    /*
    private static List<object> UserDataEncrypt(UserData _user, byte[] Key, byte[] IV)
    {
        List<object> encryptData = new List<object>();

        encryptData.Add(AESSecurity.Encrypt_ToBytes_Aes<int>(_user.m_UserGold, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ToBytes_Aes<int>(_user.m_UserJam, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<bool>(_user.m_TutorialStoryClear, Key, IV));

        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<int>(_user.m_HeroAbilityLevel, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<int>(_user.m_HeroSKillLevel, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<int>(_user.m_BuildingLevel, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<bool>(_user.m_IsUnlockSkill, Key, IV));

        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<bool>(_user.m_IsStageOpen, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<bool>(_user.m_StageBossShow, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<bool>(_user.m_StageBossClear, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ArrayToBytes_Aes<string>(_user.m_StageBossTime, Key, IV));

        encryptData.Add(AESSecurity.Encrypt_ToBytes_Aes<float>(_user.m_BGMVolume, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(_user.m_BGMMute, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ToBytes_Aes<float>(_user.m_SFxVolume, Key, IV));
        encryptData.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(_user.m_SFxMute, Key, IV));

        return encryptData;
    }

    private static UserData UserDataDecrypt(List<object> _data, byte[] Key, byte[] IV)
    {
        UserData _user = new UserData(new User());

        _user.m_UserGold = AESSecurity.DecryptIntFromBytes_Aes((byte[])_data[0], Key, IV);
        _user.m_UserJam = AESSecurity.DecryptIntFromBytes_Aes((byte[])_data[1], Key, IV);
        _user.m_TutorialStoryClear = AESSecurity.DecryptBoolArrayFromBytes_Aes(
            ConvertListFromObject(_data[2]), Key, IV);

        _user.m_HeroAbilityLevel = AESSecurity.DecryptIntArrayFromBytes_Aes(
            ConvertListFromObject(_data[3]), Key, IV);
        _user.m_HeroSKillLevel = AESSecurity.DecryptIntArrayFromBytes_Aes(
            ConvertListFromObject(_data[4]), Key, IV);
        _user.m_BuildingLevel = AESSecurity.DecryptIntArrayFromBytes_Aes(
            ConvertListFromObject(_data[5]), Key, IV);
        _user.m_IsUnlockSkill = AESSecurity.DecryptBoolArrayFromBytes_Aes(
            ConvertListFromObject(_data[6]), Key, IV);

        _user.m_IsStageOpen = AESSecurity.DecryptBoolArrayFromBytes_Aes(
            ConvertListFromObject(_data[7]), Key, IV);
        _user.m_StageBossShow = AESSecurity.DecryptBoolArrayFromBytes_Aes(
            ConvertListFromObject(_data[8]), Key, IV);
        _user.m_StageBossClear = AESSecurity.DecryptBoolArrayFromBytes_Aes(
            ConvertListFromObject(_data[9]), Key, IV);
        _user.m_StageBossTime = AESSecurity.DecryptStringArrayFromBytes_Aes(
            ConvertListFromObject(_data[10]), Key, IV);

        _user.m_BGMVolume = AESSecurity.DecryptFloatFromBytes_Aes((byte[])_data[11], Key, IV);
        _user.m_BGMMute = AESSecurity.DecryptBoolFromBytes_Aes((byte[])_data[12], Key, IV);
        _user.m_SFxVolume = AESSecurity.DecryptFloatFromBytes_Aes((byte[])_data[13], Key, IV);
        _user.m_SFxMute = AESSecurity.DecryptBoolFromBytes_Aes((byte[])_data[14], Key, IV);

        return _user;
    }
    */

    /// <summary>
    /// Object 형식을 Byte[]로 변환
    /// </summary>
    /// <param name="_obj">Object 형식인 인수</param>
    /// <returns>Byte[]</returns>
    private static byte[] ConvertByteArrayFromObject(object _obj)
    {
        if (_obj == null)
        {
            return null;
        }

        BinaryFormatter bf = new BinaryFormatter();
        MemoryStream ms = new MemoryStream();
        bf.Serialize(ms, _obj);
        return ms.ToArray();
    }

    private static List<byte[]> ConvertListFromObject(object _obj)
    {
        return (_obj as IEnumerable<object>).Cast<byte[]>().ToList();
    }
}