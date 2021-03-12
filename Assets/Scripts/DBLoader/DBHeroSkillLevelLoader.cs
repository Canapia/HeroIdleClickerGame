using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum eHeroSkillKind
{
    SMASH = 0,
    SHIELD,
    CAPTURE,
    DSPEED,

    END
}

public struct HeroSkillLevel
{
    public int Level { get; set; }
    public int NextPrice { get; set; }
    public float Effect { get; set; }
    public float CoolTime { get; set; }
}

public static class DBHeroSkillLevelLoader
{
    private static string m_FilePath = @"DB\DBHeroSkillLevel\{0}";

    public static Dictionary<eHeroSkillKind, List<HeroSkillLevel>> DBLoad()
    {
        Dictionary<eHeroSkillKind, List<HeroSkillLevel>> infoDic = new Dictionary<eHeroSkillKind, List<HeroSkillLevel>>();

        infoDic.Add(eHeroSkillKind.SMASH, DBDetail("DBHeroSkill_Smash"));
        infoDic.Add(eHeroSkillKind.SHIELD, DBDetail("DBHeroSkill_Shield"));
        infoDic.Add(eHeroSkillKind.CAPTURE, DBDetail("DBHeroSkill_Capture"));
        infoDic.Add(eHeroSkillKind.DSPEED, DBDetail("DBHeroSkill_DSpeed"));

        return infoDic;
    }

    private static List<HeroSkillLevel> DBDetail(string _DBName)
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            string fullFilePath = String.Format(@"Assets\Resources\" + m_FilePath + ".txt", _DBName);

            using (StreamReader sr = new StreamReader(new FileStream(fullFilePath, FileMode.Open)))
            {
                sr.ReadLine();

                List<HeroSkillLevel> infoList = new List<HeroSkillLevel>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    HeroSkillLevel info = new HeroSkillLevel();
                    info.Level = Convert.ToInt32(arr[0]);
                    info.NextPrice = Convert.ToInt32(arr[1]);
                    info.Effect = float.Parse(arr[2]);
                    info.CoolTime = float.Parse(arr[3]);

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
                List<HeroSkillLevel> infoList = new List<HeroSkillLevel>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    HeroSkillLevel info = new HeroSkillLevel();
                    info.Level = Convert.ToInt32(arr[0]);
                    info.NextPrice = Convert.ToInt32(arr[1]);
                    info.Effect = float.Parse(arr[2]);
                    info.CoolTime = float.Parse(arr[3]);

                    infoList.Add(info);
                }

                return infoList;
            }
        }
        return null;
    }
}
