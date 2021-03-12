using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public struct StageBossInfo
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string AnimName { get; set; }
    public string IconName { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int DropGold { get; set; }
    public int OpenPay { get; set; }
    public int RemainTime { get; set; }
}

public static class DBStageBossInfoLoader
{
    private static string m_FilePath = @"DB\DBStageBossInfo";

    public static List<StageBossInfo> DBLoad()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop) // PC
        {
            using (StreamReader sr = new StreamReader(
                new FileStream(String.Format(@"Assets\Resources\{0}.txt", m_FilePath), FileMode.Open)))
            {
                sr.ReadLine();

                List<StageBossInfo> infoList = new List<StageBossInfo>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    StageBossInfo info = new StageBossInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.Name = arr[1];
                    info.AnimName = arr[2];
                    info.IconName = arr[3];
                    info.HP = Convert.ToInt32(arr[4]);
                    info.Attack = Convert.ToInt32(arr[5]);
                    info.DropGold = Convert.ToInt32(arr[6]);
                    info.OpenPay = Convert.ToInt32(arr[7]);
                    info.RemainTime = Convert.ToInt32(arr[8]);

                    infoList.Add(info);
                }

                sr.Close();

                return infoList;
            }
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)   // SmartPhone, Tablet
        {
            TextAsset asset = Resources.Load(m_FilePath) as TextAsset;

            if (asset != null)
            {
                string assetContent = asset.text;
                List<StageBossInfo> infoList = new List<StageBossInfo>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    StageBossInfo info = new StageBossInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.Name = arr[1];
                    info.AnimName = arr[2];
                    info.IconName = arr[3];
                    info.HP = Convert.ToInt32(arr[4]);
                    info.Attack = Convert.ToInt32(arr[5]);
                    info.DropGold = Convert.ToInt32(arr[6]);
                    info.OpenPay = Convert.ToInt32(arr[7]);
                    info.RemainTime = Convert.ToInt32(arr[8]);

                    infoList.Add(info);
                }

                return infoList;
            }
        }

        return null;
    }
}
