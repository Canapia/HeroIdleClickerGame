using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum eHeroAbilityKind
{
    HP = 0,
    ATTACK,
    SPEED,
    DEF,
    POTION,
    GOLDUP,
    CRITICAL,

    END
}

public struct HeroAbilityLevel
{
    public int Level { get; set; }
    public int NextPrice { get; set; }
    public float Effect { get; set; }
}

public static class DBHeroAbilityLevelLoader
{
    private static string m_FilePath = @"DB\DBHeroAbilityLevel\{0}";

    public static Dictionary<eHeroAbilityKind, List<HeroAbilityLevel>> DBLoad()
    {
        Dictionary<eHeroAbilityKind, List<HeroAbilityLevel>> infoDic = new Dictionary<eHeroAbilityKind, List<HeroAbilityLevel>>();

        infoDic.Add(eHeroAbilityKind.HP, DBDetail("DBHeroAbility_HP"));
        infoDic.Add(eHeroAbilityKind.ATTACK, DBDetail("DBHeroAbility_Attack"));
        infoDic.Add(eHeroAbilityKind.SPEED, DBDetail("DBHeroAbility_Speed"));
        infoDic.Add(eHeroAbilityKind.DEF, DBDetail("DBHeroAbility_DEF"));
        infoDic.Add(eHeroAbilityKind.POTION, DBDetail("DBHeroAbility_Potion"));
        infoDic.Add(eHeroAbilityKind.GOLDUP, DBDetail("DBHeroAbility_GoldUp"));
        infoDic.Add(eHeroAbilityKind.CRITICAL, DBDetail("DBHeroAbility_Critical"));

        return infoDic;
    }

    private static List<HeroAbilityLevel> DBDetail(string _DBName)
    {
        if (SystemInfo.deviceType == DeviceType.Desktop) // PC
        {
            string fullFilePath = String.Format(@"Assets\Resources\" + m_FilePath + ".txt", _DBName);

            using (StreamReader sr = new StreamReader(new FileStream(fullFilePath, FileMode.Open)))
            {
                sr.ReadLine();

                List<HeroAbilityLevel> infoList = new List<HeroAbilityLevel>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    HeroAbilityLevel info = new HeroAbilityLevel();
                    info.Level = Convert.ToInt32(arr[0]);
                    info.NextPrice = Convert.ToInt32(arr[1]);
                    info.Effect = float.Parse(arr[2]);

                    infoList.Add(info);
                }

                sr.Close();

                return infoList;
            }
        }
        else if (SystemInfo.deviceType == DeviceType.Handheld)   // SmartPhone, Tablet
        {
            TextAsset asset = Resources.Load(String.Format(m_FilePath, _DBName)) as TextAsset;

            if (asset != null)
            {
                string assetContent = asset.text;
                List<HeroAbilityLevel> infoList = new List<HeroAbilityLevel>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    HeroAbilityLevel info = new HeroAbilityLevel();
                    info.Level = Convert.ToInt32(arr[0]);
                    info.NextPrice = Convert.ToInt32(arr[1]);
                    info.Effect = float.Parse(arr[2]);

                    infoList.Add(info);
                }

                return infoList;
            }
        }

        return null;
    }
}
