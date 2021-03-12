using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public struct BuildingLevel
{
    public int Level { get; set; }
    public int NextPrice { get; set; }
}

public static class DBBuildingLevelLoader
{
    private static string m_FilePath = @"DB\DBBuildingLevel\{0}";

    public static Dictionary<eBuildingKind, List<BuildingLevel>> DBLoad()
    {
        Dictionary<eBuildingKind, List<BuildingLevel>> InfoDic = new Dictionary<eBuildingKind, List<BuildingLevel>>();

        InfoDic.Add(eBuildingKind.WEAPON, DBDetail("DBBuilding_Weapon"));
        InfoDic.Add(eBuildingKind.ARMOR, DBDetail("DBBuilding_Armor"));
        InfoDic.Add(eBuildingKind.POTION, DBDetail("DBBuilding_Potion"));
        InfoDic.Add(eBuildingKind.TRAINING, DBDetail("DBBuilding_Training"));
        InfoDic.Add(eBuildingKind.BANK, DBDetail("DBBuilding_Bank"));
        InfoDic.Add(eBuildingKind.MARKET, DBDetail("DBBuilding_Market"));

        return InfoDic;
    }
    
    private static List<BuildingLevel> DBDetail(string _DBName)
    {
        if(SystemInfo.deviceType == DeviceType.Desktop) // PC
        {
            string fullFileName = String.Format(@"Assets\Resources\" + m_FilePath + ".txt", _DBName);

            using (StreamReader sr = new StreamReader(new FileStream(fullFileName, FileMode.Open)))
            {
                sr.ReadLine();

                List<BuildingLevel> infoList = new List<BuildingLevel>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    BuildingLevel info = new BuildingLevel();
                    info.Level = Convert.ToInt32(arr[0]);
                    info.NextPrice = Convert.ToInt32(arr[1]);

                    infoList.Add(info);
                }

                sr.Close();

                return infoList;
            }
        }
        else if(SystemInfo.deviceType == DeviceType.Handheld)   // SmartPhone, Tablet
        {
            TextAsset asset = Resources.Load(String.Format(m_FilePath, _DBName)) as TextAsset;

            if(asset != null)
            {
                string assetContent = asset.text;
                List<BuildingLevel> infoList = new List<BuildingLevel>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    BuildingLevel info = new BuildingLevel();
                    info.Level = Convert.ToInt32(arr[0]);
                    info.NextPrice = Convert.ToInt32(arr[1]);

                    infoList.Add(info);
                }

                return infoList;
            }
        }

        return null;
    }
}
