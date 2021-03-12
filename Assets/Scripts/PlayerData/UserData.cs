using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eUserDataInfo
{
    UserGold = 0,
    UserJam,
    TutorialStoryClear,

    HeroAbilityLevel,
    HeroSkillLevel,
    BuildingLevel,
    IsUnlockSkill,

    IsStageOpen,
    StageBossShow,
    StageBossClear,
    StageBossTime,

    BGMVolume,
    BGMMute,
    SFxVolume,
    SFxMute,

    END
}

[System.Serializable]
public class UserData
{
    #region User Info
    //public int m_UserGold;
    //public int m_UserJam;
    //public bool[] m_TutorialStoryClear;
    public byte[] m_UserGold;
    public byte[] m_UserJam;
    public List<byte[]> m_TutorialStoryClear;
    #endregion

    // Hero Info
    //public int[] m_HeroAbilityLevel;
    //public int[] m_HeroSKillLevel;
    //public int[] m_BuildingLevel;
    //public bool[] m_IsUnlockSkill;
    public List<byte[]> m_HeroAbilityLevel;
    public List<byte[]> m_HeroSKillLevel;
    public List<byte[]> m_BuildingLevel;
    public List<byte[]> m_IsUnlockSkill;

    // Stage Info
    //public bool[] m_IsStageOpen;
    //public bool[] m_StageBossShow;
    //public bool[] m_StageBossClear;
    //public string[] m_StageBossTime; 
    public List<byte[]> m_IsStageOpen;
    public List<byte[]> m_StageBossShow;
    public List<byte[]> m_StageBossClear;
    public List<byte[]> m_StageBossTime;

    #region Option Info
    public float m_BGMVolume;
    public bool m_BGMMute;
    public float m_SFxVolume;
    public bool m_SFxMute;
    #endregion

    public UserData(User _user)
    {
        // User Info
        m_UserGold = _user.UserGold;
        m_UserJam = _user.UserJam;
        m_TutorialStoryClear = _user.TutorialStoryClear;

        // Hero Info
        m_HeroAbilityLevel = _user.UserAbilityLevel;
        m_HeroSKillLevel = _user.UserSkillLevel;
        m_BuildingLevel = _user.UserBuildingLevel;
        m_IsUnlockSkill = _user.UserIsUnlockSkill;

        // Stage Info
        m_IsStageOpen = _user.UserIsStageOpen;
        m_StageBossShow = _user.UserStageBossShow;
        m_StageBossClear = _user.UserStageBossClear;
        m_StageBossTime = _user.UserStageBossTime;

        // Option Info
        m_BGMVolume = _user.BGMVolume;
        m_BGMMute = _user.BGMMute;
        m_SFxVolume = _user.SFxVolume;
        m_SFxMute = _user.SFxMute;
    }
}
