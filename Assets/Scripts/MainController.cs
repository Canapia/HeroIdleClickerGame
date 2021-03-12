using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    private static MainController m_Instance;
    public static MainController Instance
    {
        get { return m_Instance; }
        set { m_Instance = value; }
    }

    private User m_User;
    public User UserInfo
    {
        get
        {
            /*
            if (m_User == null)
            {
                m_User = new User();
                m_User.LoadUser();
                m_User.SaveUser();
            }
            */
            return m_User;
        }
        set
        {
            m_User = value;
        }
    }

    public eStageKind SelectStageKind;

    public bool m_IsPopup { get; set; }

    #region DB
    private List<BuildingInfo> m_BuildingInfo;
    private Dictionary<eBuildingKind, List<BuildingLevel>> m_BuildingLevel;
    private List<HeroAbilityInfo> m_HeroAbilityInfo;
    private Dictionary<eHeroAbilityKind, List<HeroAbilityLevel>> m_HeroAbilityLevel;
    private List<HeroSkillInfo> m_HeroSkillInfo;
    private Dictionary<eHeroSkillKind, List<HeroSkillLevel>> m_HeroSkillLevel;
    private List<StageBossInfo> m_StageBossInfo;
    private List<StageInfo> m_StageInfo;
    private List<StageMonsterInfo> m_StageMonsterInfo;
    #endregion

    private prefabToastMessage m_ToastMessage = null;
    public prefabToastMessage ToastMessage
    {
        get { return m_ToastMessage; }
        set { m_ToastMessage = value; }
    }

    void Start()
    {    

        if(m_Instance == null)
        {
            m_Instance = GetComponent<MainController>();
            Debug.Log("MainController 생성됨");
        }
        else
        {
            Destroy(m_Instance);
            Debug.Log("MainController 제거됨");
        }

        if(m_User == null)
        {
            m_User = new User();
            m_User.LoadUser();
        }

        // Sound Manager가 생성되기 전에 MainController가 생성되어야 하기 때문에
        // MainController에서 SoundManager 생성
        /*
        if(SoundManager.Instance == null)
        {
            SoundManager.Instance = GetComponent<SoundManager>();
        }
        */

        m_IsPopup = false;

        #region DB Load
        m_BuildingInfo = DBBuildingInfoLoader.DBLoad();
        m_BuildingLevel = DBBuildingLevelLoader.DBLoad();
        m_HeroAbilityInfo = DBHeroAbilityInfoLoader.DBLoad();
        m_HeroAbilityLevel = DBHeroAbilityLevelLoader.DBLoad();
        m_HeroSkillInfo = DBHeroSkillInfoLoader.DBLoad();
        m_HeroSkillLevel = DBHeroSkillLevelLoader.DBLoad();
        m_StageBossInfo = DBStageBossInfoLoader.DBLoad();
        m_StageInfo = DBStageInfoLoader.DBLoad();
        m_StageMonsterInfo = DBStageMonsterInfoLoader.DBLoad();
        #endregion
    }

    public void UserAbilityLevelUp(eHeroAbilityKind _kind)
    {
        if (m_User != null)
        {
            m_User.UserAbilityLevel_LevelUp(_kind);

            if(AdventureModeInHuntSceneUI.Instance != null &&
                _kind == eHeroAbilityKind.HP)
            {
                AdventureModeInHuntSceneUI.Instance.m_HeroInfo.SetUpgradeUIHPBar();
            }
        }
    }

    #region BuildingInfo Function
    public BuildingInfo GetBuildingInfo(int _ID)
    {
        return m_BuildingInfo.Find(x => x.ID == _ID);
    }

    public BuildingInfo GetBuildingInfo(eBuildingKind _Kind)
    {
        return m_BuildingInfo.Find(x => x.BuildingKind == _Kind);
    }
    #endregion

    #region BuildingLevel Function
    public List<BuildingLevel> GetAllBuildingLevel(eBuildingKind _Kind)
    {
        List<BuildingLevel> list = new List<BuildingLevel>();
        m_BuildingLevel.TryGetValue(_Kind, out list);
        return list;
    }

    public BuildingLevel GetBuildingLevel(eBuildingKind _Kind, int _Level)
    {
        return GetAllBuildingLevel(_Kind).Find(x => x.Level == _Level);
    }
    #endregion

    #region HeroAbilityInfo Function
    public HeroAbilityInfo GetHeroAbilityInfo(int _ID)
    {
        return m_HeroAbilityInfo.Find(x => x.ID == _ID);
    }

    public HeroAbilityInfo GetHeroAbilityInfo(eHeroAbilityKind _Kind)
    {
        return m_HeroAbilityInfo.Find(x => x.AbilityKind == _Kind);
    }
    #endregion

    #region HeroAbilityLevel Function
    public List<HeroAbilityLevel> GetAllHeroAbilityLevel(eHeroAbilityKind _Kind)
    {
        List<HeroAbilityLevel> list = new List<HeroAbilityLevel>();
        m_HeroAbilityLevel.TryGetValue(_Kind, out list);
        return list;
    }

    public HeroAbilityLevel GetHeroAbilityLevel(eHeroAbilityKind _Kind, int _Level)
    {
        return GetAllHeroAbilityLevel(_Kind).Find(x => x.Level == _Level);
    }
    #endregion

    #region HeroSkillInfo Function
    public HeroSkillInfo GetHeroSkillInfo(int _ID)
    {
        return m_HeroSkillInfo.Find(x => x.ID == _ID);
    }

    public HeroSkillInfo GetHeroSkillInfo(eHeroSkillKind _Kind)
    {
        return m_HeroSkillInfo.Find(x => x.SkillKind == _Kind);
    }
    #endregion

    #region HeroSKillLevel Function
    public List<HeroSkillLevel> GetAllHeroSkillLevel(eHeroSkillKind _Kind)
    {
        List<HeroSkillLevel> list = new List<HeroSkillLevel>();
        m_HeroSkillLevel.TryGetValue(_Kind, out list);
        return list;
    }

    public HeroSkillLevel GetHeroSkillLevel(eHeroSkillKind _Kind, int _Level)
    {
        return GetAllHeroSkillLevel(_Kind).Find(x => x.Level == _Level);
    }
    #endregion

    #region StageBossInfo Function
    public StageBossInfo GetStageBossInfo(int _ID)
    {
        return m_StageBossInfo.Find(x => x.ID == _ID);
    }
    #endregion

    #region StageInfo Function
    public StageInfo GetStageInfo(int _ID)
    {
        return m_StageInfo.Find(x => x.ID == _ID);
    }

    public StageInfo GetStageInfo(eStageKind _Kind)
    {
        return m_StageInfo.Find(x => x.StageKind == _Kind);
    }

    public int GetStageInfo_BossID(eStageKind _Kind)
    {
        return GetStageInfo(_Kind).BossID;
    }

    public eStageKind GetStageInfo_StageKind(int _BossID)
    {
        return m_StageInfo.Find(x => x.BossID == _BossID).StageKind;
    }
    #endregion

    #region StageMonsterInfo Function
    public StageMonsterInfo GetStageMonsterInfo(int _ID)
    {
        return m_StageMonsterInfo.Find(x => x.ID == _ID);
    }
    #endregion

    public void DestroyOnController()
    {
        Destroy(this);
    }
}
