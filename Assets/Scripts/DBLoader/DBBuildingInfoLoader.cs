using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum eBuildingKind
{
    WEAPON = 0,
    ARMOR,
    POTION,
    TRAINING,
    BANK,
    MARKET,
    TEMPLE,

    END
}

public struct BuildingInfo
{
    public int ID { get; set; }
    public string Name { get; set; }
    public eBuildingKind BuildingKind { get; set; }
    public string Infomation { get; set; }
}

public static class DBBuildingInfoLoader
{
    private static string m_FilePath = @"DB\DBBuildingInfo";

    public static List<BuildingInfo> DBLoad()
    {
        if(SystemInfo.deviceType == DeviceType.Desktop) // PC
        {
            using (StreamReader sr = new StreamReader(
                new FileStream(String.Format(@"Assets\Resources\{0}.txt", m_FilePath), FileMode.Open)))
            {
                sr.ReadLine();

                List<BuildingInfo> infoList = new List<BuildingInfo>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    BuildingInfo info = new BuildingInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.Name = arr[1];
                    info.BuildingKind = (eBuildingKind)Enum.Parse(typeof(eBuildingKind), arr[2]);
                    info.Infomation = arr[3];

                    infoList.Add(info);
                }

                sr.Close();

                return infoList;
            }
        }
        else if(SystemInfo.deviceType == DeviceType.Handheld)   // SmartPhone, Tablet
        {
            TextAsset asset = Resources.Load(m_FilePath) as TextAsset;

            if (asset != null)
            {
                string assetContent = asset.text;
                List<BuildingInfo> infoList = new List<BuildingInfo>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    BuildingInfo info = new BuildingInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.Name = arr[1];
                    info.BuildingKind = (eBuildingKind)Enum.Parse(typeof(eBuildingKind), arr[2]);
                    info.Infomation = arr[3];

                    infoList.Add(info);
                }

                return infoList;
            }
        }

        return null;
    }
}
