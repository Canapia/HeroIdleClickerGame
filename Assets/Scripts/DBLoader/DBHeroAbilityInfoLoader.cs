using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public struct HeroAbilityInfo
{
    public int ID { get; set; }

    public eHeroAbilityKind AbilityKind { get; set; }
    public string Name { get; set; }
    public string Info { get; set; }
    public string State { get; set; }
    public string IconName { get; set; }
}

public class DBHeroAbilityInfoLoader
{
    private static string m_FilePath = @"DB\DBHeroAbilityInfo";

    public static List<HeroAbilityInfo> DBLoad()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop) // PC
        {
            using (StreamReader sr = new StreamReader(
                new FileStream(String.Format(@"Assets\Resources\{0}.txt", m_FilePath), FileMode.Open)))
            {
                sr.ReadLine();

                List<HeroAbilityInfo> infoList = new List<HeroAbilityInfo>();

                while (sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    HeroAbilityInfo info = new HeroAbilityInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.AbilityKind = (eHeroAbilityKind)Enum.Parse(typeof(eHeroAbilityKind), arr[1]);
                    info.Name = arr[2];
                    info.Info = arr[3];
                    info.State = arr[4];
                    info.IconName = arr[5];

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
                List<HeroAbilityInfo> infoList = new List<HeroAbilityInfo>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 1; i < contentArr.Length; i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    HeroAbilityInfo info = new HeroAbilityInfo();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.AbilityKind = (eHeroAbilityKind)Enum.Parse(typeof(eHeroAbilityKind), arr[1]);
                    info.Name = arr[2];
                    info.Info = arr[3];
                    info.State = arr[4];
                    info.IconName = arr[5];

                    infoList.Add(info);
                }

                return infoList;
            }
        }
        return null;
    }
}
