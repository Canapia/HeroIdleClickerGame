using System;
using System.Security.Cryptography;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eHeroInfoKindF
{
    // Ability
    HeroHPLevel = 0,
    HeroAttackLevel,
    HeroSpeedLevel,
    HeroDEFLevel,
    HeroPotionLevel,
    HeroGoldLevel,
    HeroCriLevel = 6,

    // Skill
    Skill_Smash = 7,
    Skill_Shield,
    Skill_Capture,
    Skill_Accel,

    END
}

public class User
{
    // User Info
    public byte[] UserGold { get; set; }
    public byte[] UserJam { get; set; }
    public List<byte[]> TutorialStoryClear { get; set; }

    // Hero Info
    public List<byte[]> UserAbilityLevel { get; set; }
    public List<byte[]> UserSkillLevel { get; set; }
    public List<byte[]> UserBuildingLevel { get; set; }
    public List<byte[]> UserIsUnlockSkill { get; set; }     // 스킬을 언락했는가?

    // Stage Info
    public List<byte[]> UserIsStageOpen { get; set; }     // 지역이 열렸는가?
    public List<byte[]> UserStageBossShow { get; set; }
    public List<byte[]> UserStageBossClear { get; set; }
    public List<byte[]> UserStageBossTime { get; set; }

    // Option Info
    public float BGMVolume { get; set; }
    public bool BGMMute { get; set; }
    public float SFxVolume { get; set; }
    public bool SFxMute { get; set; }

    private AESKeyAndIV keyIV;

    public User()
    {
        using (Aes aesAlg = Aes.Create())
        {
            keyIV = SaveSystem.LoadAESKeyAndIV();

            if(SaveSystem.LoadAESKeyAndIV() != null)
            {
                aesAlg.Key = keyIV.Key;
                aesAlg.IV = keyIV.IV;
            }
            else if(SaveSystem.LoadAESKeyAndIV() == null)
            {
                keyIV = new AESKeyAndIV(aesAlg.Key, aesAlg.IV);
                SaveSystem.SaveAESKeyAndIV(aesAlg.Key, aesAlg.IV);
            }

            // User Info
            //UserGold = 1000;
            //UserJam = 10;
            //TutorialStoyClear = new bool[(int)eStoryState.END];
            //for (int i = 0; i < (int)eStoryState.END; i++)
            //{
            //    TutorialStoyClear[i] = false;
            //}
            UserGold = AESSecurity.Encrypt_ToBytes_Aes<int>(1000, aesAlg.Key, aesAlg.IV);
            UserJam = AESSecurity.Encrypt_ToBytes_Aes<int>(10, aesAlg.Key, aesAlg.IV);
            TutorialStoryClear = new List<byte[]>();
            for (int i = 0; i < (int)eStoryState.END; i++)
            {
                TutorialStoryClear.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(false, aesAlg.Key, aesAlg.IV));
            }

            // Hero Info
            /*
            UserAbilityLevel = new int[(int)eHeroAbilityKind.END];
            for (int i = 0; i < (int)eHeroAbilityKind.END; i++)
            {
                UserAbilityLevel[i] = 1;
            }

            UserSkillLevel = new int[(int)eHeroAbilityKind.END];
            for (int i = 0; i < (int)eHeroSkillKind.END; i++)
            {
                UserSkillLevel[i] = 1;
            }

            UserBuildingLevel = new int[(int)eHeroAbilityKind.END];
            for (int i = 0; i < (int)eBuildingKind.END; i++)
            {
                UserBuildingLevel[i] = 1;
            }

            UserIsUnlockSkill = new bool[(int)eHeroSkillKind.END];
            for (int i = 0; i < (int)eHeroSkillKind.END; i++)
            {
                // 임시로 스매시 스킬 언락
                if (i == 0)
                {
                    UserIsUnlockSkill[i] = true;
                }
                else
                {
                    UserIsUnlockSkill[i] = false;
                }
            }
            */
            UserAbilityLevel = new List<byte[]>();
            for (int i = 0; i < (int)eHeroAbilityKind.END; i++)
            {
                UserAbilityLevel.Add(AESSecurity.Encrypt_ToBytes_Aes<int>(1, aesAlg.Key, aesAlg.IV));
            }
            UserSkillLevel = new List<byte[]>();
            for (int i = 0; i < (int)eHeroSkillKind.END; i++)
            {
                UserSkillLevel.Add(AESSecurity.Encrypt_ToBytes_Aes<int>(1, aesAlg.Key, aesAlg.IV));
            }
            UserBuildingLevel = new List<byte[]>();
            for (int i = 0; i < (int)eBuildingKind.END; i++)
            {
                UserBuildingLevel.Add(AESSecurity.Encrypt_ToBytes_Aes<int>(1, aesAlg.Key, aesAlg.IV));
            }
            UserIsUnlockSkill = new List<byte[]>();
            for (int i = 0; i < (int)eHeroSkillKind.END; i++)
            {
                if (i==0)
                {
                    UserIsUnlockSkill.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(true, aesAlg.Key, aesAlg.IV));
                }
                UserIsUnlockSkill.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(false, aesAlg.Key, aesAlg.IV));
            }

            // Stage Info
            /*
            UserIsStageOpen = new bool[(int)eStageKind.END];
            {
                for (int i = 0; i < (int)eStageKind.END; i++)
                {
                    if (i == 0)
                    {
                        UserIsStageOpen[i] = true;  // Stage1은 처음부터 열려있다.
                    }
                    else if (i == 1)
                    {
                        UserIsStageOpen[i] = false;  // 임시로 열어둠
                    }
                    else
                    {
                        UserIsStageOpen[i] = false;
                    }
                }
            }

            UserStageBossShow = new bool[(int)eStageKind.END];
            for (int i = 0; i < (int)eStageKind.END; i++)
            {
                //UserStageBossShow[i] = false;
                if (i == 0)
                {
                    UserStageBossShow[i] = false;    // 임의로 열어놓음
                }
                else
                {
                    UserStageBossShow[i] = false;
                }
            }

            UserStageBossClear = new bool[(int)eStageKind.END];
            for (int i = 0; i < (int)eStageKind.END; i++)
            {
                UserStageBossClear[i] = false;
            }

            UserStageBossTime = new string[(int)eStageKind.END];
            for (int i = 0; i < (int)eStageKind.END; i++)
            {
                // 현재 시간 - 1Day 으로 저장
                UserStageBossTime[i] = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }
            */

            UserIsStageOpen = new List<byte[]>();
            for (int i = 0; i < (int)eStageKind.END; i++)
            {
                if (i == 0)
                {
                    UserIsStageOpen.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(true, aesAlg.Key, aesAlg.IV));
                }
                else
                {
                    UserIsStageOpen.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(false, aesAlg.Key, aesAlg.IV));
                }
            }
            UserStageBossShow = new List<byte[]>();
            for (int i = 0; i < (int)eStageKind.END; i++)
            {
                UserStageBossShow.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(false, aesAlg.Key, aesAlg.IV));
            }
            UserStageBossClear = new List<byte[]>();
            for (int i = 0; i < (int)eStageKind.END; i++)
            {
                UserStageBossClear.Add(AESSecurity.Encrypt_ToBytes_Aes<bool>(false, aesAlg.Key, aesAlg.IV));
            }
            UserStageBossTime = new List<byte[]>();
            for (int i = 0; i < (int)eStageKind.END; i++)
            {
                UserStageBossTime.Add(AESSecurity.Encrypt_ToBytes_Aes<string>(
                    DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss"), aesAlg.Key, aesAlg.IV));
            }

            // Option Info
            BGMVolume = 1.0f;
            BGMMute = false;
            SFxVolume = 1.0f;
            SFxMute = false;
        }
    }

    public int GetUserGold()
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptIntFromBytes_Aes(UserGold, keyIV.Key, keyIV.IV);
    }

    public void ChangeUserGold(int _inputGold)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        int currentGold = GetUserGold();
        UserGold = AESSecurity.Encrypt_ToBytes_Aes<int>(currentGold + _inputGold, keyIV.Key, keyIV.IV);
        GoodsSceneInstance.Instance.RenewalUI_Gold();
    }

    public int GetUserJam()
    {
        //AESKeyAndIV keyIv = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptIntFromBytes_Aes(UserJam, keyIV.Key, keyIV.IV);
    }

    public bool GetTutorialStoryClear(eStoryState _state)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptBoolFromBytes_Aes(TutorialStoryClear[(int)_state], keyIV.Key, keyIV.IV);
    }

    public void ChangeTutorialstoryClear(eStoryState _state)
    {
        //AESKeyAndIV keyIv = SaveSystem.LoadAESKeyAndIV();

        TutorialStoryClear[(int)_state] =
            AESSecurity.Encrypt_ToBytes_Aes<bool>(true, keyIV.Key, keyIV.IV);
    }

    public int GetUserAbilityLevel(eHeroAbilityKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptIntFromBytes_Aes(UserAbilityLevel[(int)_Kind], keyIV.Key, keyIV.IV);
    }

    public void UserAbilityLevel_LevelUp(eHeroAbilityKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        int currentLevel = AESSecurity.DecryptIntFromBytes_Aes(UserAbilityLevel[(int)_Kind], keyIV.Key, keyIV.IV);
        UserAbilityLevel[(int)_Kind] = AESSecurity.Encrypt_ToBytes_Aes<int>(currentLevel + 1, keyIV.Key, keyIV.IV);
    }

    public int GetUserSkillLevel(eHeroSkillKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptIntFromBytes_Aes(UserSkillLevel[(int)_Kind], keyIV.Key, keyIV.IV);
    }

    public void UserSkillLevel_LevelUp(eHeroSkillKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        int currentLevel = AESSecurity.DecryptIntFromBytes_Aes(UserSkillLevel[(int)_Kind], keyIV.Key, keyIV.IV);
        UserSkillLevel[(int)_Kind] = AESSecurity.Encrypt_ToBytes_Aes<int>(currentLevel + 1, keyIV.Key, keyIV.IV);
    }

    public int GetUserBuildingLevel(eBuildingKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptIntFromBytes_Aes(UserBuildingLevel[(int)_Kind], keyIV.Key, keyIV.IV);
    }

    public void UserBuildingLevel_LevelUp(eBuildingKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        int currentLevel = AESSecurity.DecryptIntFromBytes_Aes(UserBuildingLevel[(int)_Kind], keyIV.Key, keyIV.IV);
        UserBuildingLevel[(int)_Kind] = AESSecurity.Encrypt_ToBytes_Aes<int>(currentLevel + 1, keyIV.Key, keyIV.IV);
    }

    public bool GetUserIsUnlockSkill(eHeroSkillKind _kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptBoolFromBytes_Aes(UserIsUnlockSkill[(int)_kind], keyIV.Key, keyIV.IV);
    }

    public void ChangeUserIsUnlockSkill(eHeroSkillKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        UserIsUnlockSkill[(int)_Kind] = AESSecurity.Encrypt_ToBytes_Aes<bool>(true, keyIV.Key, keyIV.IV);
    }

    public bool GetUserIsStageOpen(eStageKind _kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptBoolFromBytes_Aes(UserIsStageOpen[(int)_kind], keyIV.Key, keyIV.IV);
    }

    public void ChangeUserIsStageOpen(eStageKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        UserIsStageOpen[(int)_Kind] = AESSecurity.Encrypt_ToBytes_Aes<bool>(true, keyIV.Key, keyIV.IV);
    }

    public bool GetUserStageBossShow(eStageKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptBoolFromBytes_Aes(UserStageBossShow[(int)_Kind], keyIV.Key, keyIV.IV);
    }

    public void ChangeUserStageBossShow(eStageKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        UserStageBossShow[(int)_Kind] = AESSecurity.Encrypt_ToBytes_Aes<bool>(true, keyIV.Key, keyIV.IV);
    }

    public bool GetUserStageBossClear(eStageKind _Kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        return AESSecurity.DecryptBoolFromBytes_Aes(UserStageBossClear[(int)_Kind], keyIV.Key, keyIV.IV);
    }

    public void ChangeUserStageBossClear(eStageKind _kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        UserStageBossClear[(int)_kind] = AESSecurity.Encrypt_ToBytes_Aes<bool>(true, keyIV.Key, keyIV.IV);
    }

    public DateTime GetStageBossTime(eStageKind _kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();
        string bossTime = AESSecurity.DecryptStringFromBytes_Aes(UserStageBossTime[(int)_kind], keyIV.Key, keyIV.IV);

        return Convert.ToDateTime(bossTime);
    }

    public void ChangeStageBossTime(eStageKind _kind)
    {
        //AESKeyAndIV keyIV = SaveSystem.LoadAESKeyAndIV();

        UserStageBossTime[(int)_kind] = AESSecurity.Encrypt_ToBytes_Aes<string>(
            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), keyIV.Key, keyIV.IV);
    }

    public void SaveUser()
    {
        SaveSystem.SaveUserData(this);
    }

    public void LoadUser()
    {
        UserData data = SaveSystem.LoadUserData();

        if (data != null)
        {
            // User Info
            UserGold = data.m_UserGold;
            UserJam = data.m_UserJam;
            if(data.m_TutorialStoryClear != null)
            {
                TutorialStoryClear = data.m_TutorialStoryClear;
            }

            if(data.m_HeroAbilityLevel != null)
            {
                UserAbilityLevel = data.m_HeroAbilityLevel;
            }
            if(data.m_HeroSKillLevel != null)
            {
                UserSkillLevel = data.m_HeroSKillLevel;
            }
            if(data.m_BuildingLevel != null)
            {
                UserBuildingLevel = data.m_BuildingLevel;
            }
            if (data.m_IsUnlockSkill != null)
            {
                UserIsUnlockSkill = data.m_IsUnlockSkill;
            }

            // Stage Info
            if(data.m_IsStageOpen != null)
            {
                UserIsStageOpen = data.m_IsStageOpen;
            }
            if(data.m_StageBossShow != null)
            {
                UserStageBossShow = data.m_StageBossShow;
            }
            if(data.m_StageBossClear != null)
            {
                UserStageBossClear = data.m_StageBossClear;
            }
            if(data.m_StageBossTime != null)
            {
                UserStageBossTime = data.m_StageBossTime;
            }

            // Option Info
            BGMVolume = data.m_BGMVolume;
            BGMMute = data.m_BGMMute;
            SFxVolume = data.m_SFxVolume;
            SFxMute = data.m_SFxMute;
        }
    }
}
