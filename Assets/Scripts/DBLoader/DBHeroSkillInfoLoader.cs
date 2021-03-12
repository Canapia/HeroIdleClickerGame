using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public struct HeroSkillInfo
{
    public int ID { get; set; }
    public eHeroSkillKind SkillKind { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }
    public string State { get; set; }
    public string IconName { get; set; }
    /// <summary>
    /// 스킬을 어느 스테이지를 클리어 하고 배우는 가?
    /// 0 : 튜토리얼
    /// </summary>
    public int StageClear { get; set; }
}

public static class DBHeroSkillInfoLoader
{
    private static string m_FilePath = @"DB\DBHeroSkillInfo";

    public static List<HeroSkillInfo> DBLoad()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop) // PC
        {
            using (StreamReader sr = new StreamReader(
                new FileStream(String.Format(@"Assets\Resources\{0}.txt", m_FilePath), FileMode.Open)))
            {
                sr.ReadLine();

                List<HeroSkillInfo> infoList = new List<HeroSkillInfo>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    HeroSkillInfo info = new HeroSkillInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.SkillKind = (eHeroSkillKind)Enum.Parse(typeof(eHeroSkillKind), arr[1]);
                    info.Name = arr[2];
                    info.Info = arr[3];
                    info.State = arr[4];
                    info.IconName = arr[5];
                    info.StageClear = Convert.ToInt32(arr[6]);

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
                List<HeroSkillInfo> infoList = new List<HeroSkillInfo>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    HeroSkillInfo info = new HeroSkillInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.SkillKind = (eHeroSkillKind)Enum.Parse(typeof(eHeroSkillKind), arr[1]);
                    info.Name = arr[2];
                    info.Info = arr[3];
                    info.State = arr[4];
                    info.IconName = arr[5];
                    info.StageClear = Convert.ToInt32(arr[6]);

                    infoList.Add(info);
                }

                return infoList;
            }
        }

        return null;
    }
}
