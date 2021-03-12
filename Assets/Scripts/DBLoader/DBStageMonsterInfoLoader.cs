using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public struct StageMonsterInfo
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int Attack { get; set; }
    public int DropGold { get; set; }
    public string AnimName { get; set; }
    public string SkinName { get; set; }
}

public static class DBStageMonsterInfoLoader
{
    private static string m_FilePath = @"DB\DBStageMonsterInfo";

    public static List<StageMonsterInfo> DBLoad()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop) // PC
        {
            using (StreamReader sr = new StreamReader(
                new FileStream(String.Format(@"Assets\Resources\{0}.txt", m_FilePath), FileMode.Open)))
            {
                sr.ReadLine();

                List<StageMonsterInfo> infoList = new List<StageMonsterInfo>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    StageMonsterInfo info = new StageMonsterInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.Name = arr[1];
                    info.HP = Convert.ToInt32(arr[2]);
                    info.Attack = Convert.ToInt32(arr[3]);
                    info.DropGold = Convert.ToInt32(arr[4]);
                    info.AnimName = arr[5];
                    info.SkinName = arr[6];

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
                List<StageMonsterInfo> infoList = new List<StageMonsterInfo>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    StageMonsterInfo info = new StageMonsterInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.Name = arr[1];
                    info.HP = Convert.ToInt32(arr[2]);
                    info.Attack = Convert.ToInt32(arr[3]);
                    info.DropGold = Convert.ToInt32(arr[4]);
                    info.AnimName = arr[5];
                    info.SkinName = arr[6];

                    infoList.Add(info);
                }

                return infoList;
            }
        }

        return null;
    }
}
