using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum eSpeachCharacter
{
    Narration = 0,
    Village_Chief,

    END
}

public struct TutorialStorySpeach
{
    public int ID { get; set; }
    public eStoryState StoryState { get; set; }
    public int StorySpeachnumber { get; set; }
    public eSpeachCharacter SpeachCharacter { get; set; }
    public string Speach { get; set; }
}

public static class TutorialStorySpeachLoader
{
    private static string m_FilePath = @"DB\TutorialStorySpeach";

    public static List<TutorialStorySpeach> DBLoad()
    {
        if(SystemInfo.deviceType == DeviceType.Desktop)
        {
            using(StreamReader sr = new StreamReader(
                new FileStream(String.Format(@"Assets\Resources\{0}.txt", m_FilePath), FileMode.Open)))
            {
                sr.ReadLine();

                List<TutorialStorySpeach> infoList = new List<TutorialStorySpeach>();

                while(sr.EndOfStream == false)
                {
                    string[] arr = sr.ReadLine().Split(new char[] { '\t' }, StringSplitOptions.None);

                    TutorialStorySpeach info = new TutorialStorySpeach();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.StoryState = (eStoryState)Enum.Parse(typeof(eStoryState), arr[1]);
                    info.StorySpeachnumber = Convert.ToInt32(arr[2]);
                    info.SpeachCharacter = (eSpeachCharacter)Enum.Parse(typeof(eSpeachCharacter), arr[3]);
                    info.Speach = arr[4];

                    infoList.Add(info);
                }

                sr.Close();

                return infoList;
            }
        }
        else if(SystemInfo.deviceType == DeviceType.Handheld)
        {
            TextAsset asset = Resources.Load(m_FilePath) as TextAsset;

            if(asset != null)
            {
                string assetContent = asset.text;
                List<TutorialStorySpeach> infoList = new List<TutorialStorySpeach>();

                string[] contentArr = assetContent.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                for(int i =1;i<contentArr.Length;i++)
                {
                    string[] arr = contentArr[i].Split(new char[] { '\t' }, StringSplitOptions.None);

                    TutorialStorySpeach info = new TutorialStorySpeach();
                    info.ID = Convert.ToInt32(arr[0]);
                    info.StoryState = (eStoryState)Enum.Parse(typeof(eStoryState), arr[1]);
                    info.StorySpeachnumber = Convert.ToInt32(arr[2]);
                    info.SpeachCharacter = (eSpeachCharacter)Enum.Parse(typeof(eSpeachCharacter), arr[3]);
                    info.Speach = arr[4];

                    infoList.Add(info);
                }

                return infoList;
            }
        }

        return null;
    }
}
