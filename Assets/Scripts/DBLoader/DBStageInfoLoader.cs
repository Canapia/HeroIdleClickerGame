using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum eStageKind
{
    STAGE1 = 0,
    STAGE2,
    STAGE3,
    STAGE4,
    STAGE5,

    END
}

public struct StageInfo
{
    public int ID { get; set; }
    public string Name { get; set; }
    public eStageKind StageKind { get; set; }
    public int BossID { get; set; }
    public int[] MonsterID { get; set; }
}

public static class DBStageInfoLoader
{
    private static string m_FilePath = @"DB\DBStageInfo";

    public static List<StageInfo> DBLoad()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop) // PC
        {
            using (StreamReader sr = new StreamReader(
                new FileStream(String.Format(@"Assets\Resources\{0}.txt", m_FilePath), FileMode.Open)))
            {
                sr.ReadLine();

                List<StageInfo> infoList = new List<StageInfo>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    StageInfo info = new StageInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.Name = arr[1];
                    info.StageKind = (eStageKind)Enum.Parse(typeof(eStageKind), arr[2]);
                    info.BossID = Convert.ToInt32(arr[3]);

                    string[] arrID = arr[4].Split(new char[] { '/' }, StringSplitOptions.None);
                    info.MonsterID = new int[arrID.Length];
                    for (int i = 0; i < arrID.Length; i++)
                    {
                        info.MonsterID[i] = Convert.ToInt32(arrID[i]);
                    }

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
                List<StageInfo> infoList = new List<StageInfo>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    StageInfo info = new StageInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.Name = arr[1];
                    info.StageKind = (eStageKind)Enum.Parse(typeof(eStageKind), arr[2]);
                    info.BossID = Convert.ToInt32(arr[3]);

                    string[] arrID = arr[4].Split(new char[] { '/' }, StringSplitOptions.None);
                    info.MonsterID = new int[arrID.Length];
                    for (int j = 0; j < arrID.Length; j++)
                    {
                        info.MonsterID[j] = Convert.ToInt32(arrID[j]);
                    }

                    infoList.Add(info);
                }

                return infoList;
            }
        }

        return null;
    }
}
