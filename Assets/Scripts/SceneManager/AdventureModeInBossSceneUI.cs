using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eBossStageState
{
    Ready = 0,
    Appear,     // 보스 등장
    CountDown,
    Battle,
    Pause,
    HeroDead,
    Clear,

    END
}

public class AdventureModeInBossSceneUI : MonoBehaviour
{
    #region Inspector
    public CharacterHero m_HeroInfo;
    public GameObject Obj_Damage_Position;
    [Header("- Background")]
    public Image Img_Background;
    public Sprite[] m_Background;
    [Header("- Boss Info")]
    public GameObject Position_Boss;
    public UIHPBar Position_Boss_HPBar;
    public GameObject Position_Boss_Damage;
    //public MonsterAnimationController BossAnimcontroller;
    private CharacterBoss m_BossInfo;
    [Header("- Skill")]
    public UISkillButtonInfo[] Btn_Skills;
    #endregion

    private static AdventureModeInBossSceneUI m_Instance;
    public static AdventureModeInBossSceneUI Instance
    {
        get { return m_Instance; }
        set { m_Instance = value; }
    }

    private eBossStageState m_StageState = eBossStageState.Ready;
    public eBossStageState StageState
    {
        get { return m_StageState; }
        set { m_StageState = value; }
    }

    private void Awake()
    {
        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);

        if (m_Instance == null)
        {
            m_Instance = GetComponent<AdventureModeInBossSceneUI>();
        }
    }

    private void Start()
    {
        if(MainController.Instance != null)
        {
            int BossID = MainController.Instance.GetStageInfo_BossID(MainController.Instance.SelectStageKind);

            GameObject obj = ResourceManager.GetOBJCreatePrefab(@"Prefab_Monster\" + 
                MainController.Instance.GetStageBossInfo(BossID).AnimName, Position_Boss.transform);

            SoundManager.Instance.PlayBGM(eBGMState.Boss_Stage);

            // 배경 및 BGM 변경
            switch(MainController.Instance.SelectStageKind)
            {
                case eStageKind.STAGE1:
                    Img_Background.sprite = m_Background[0];
                    
                    CharacterBoss_KingSlime kingslime = obj.GetComponent<CharacterBoss_KingSlime>();
                    m_BossInfo = kingslime;
                    m_BossInfo.m_HPBar = Position_Boss_HPBar;
                    m_BossInfo.Position_Damage = Position_Boss_Damage;
                    //m_BossInfo.m_CharacterAnimation = BossAnimcontroller;
                    break;

                case eStageKind.STAGE2:
                    Img_Background.sprite = m_Background[1];
                    
                    CharacterBoss_GiantTree giantTree = obj.GetComponent<CharacterBoss_GiantTree>();
                    m_BossInfo = giantTree;
                    m_BossInfo.m_HPBar = Position_Boss_HPBar;
                    m_BossInfo.Position_Damage = Position_Boss_Damage;
                    break;

                case eStageKind.STAGE3:
                    Img_Background.sprite = m_Background[2];
                    
                    CharacterBoss_WeirdSlime weridSlime = obj.GetComponent<CharacterBoss_WeirdSlime>();
                    m_BossInfo = weridSlime;
                    m_BossInfo.m_HPBar = Position_Boss_HPBar;
                    m_BossInfo.Position_Damage = Position_Boss_Damage;
                    break;

                case eStageKind.STAGE4:
                    Img_Background.sprite = m_Background[3];
                    
                    CharacterBoss_AntLion antLion = obj.GetComponent<CharacterBoss_AntLion>();
                    m_BossInfo = antLion;
                    m_BossInfo.m_HPBar = Position_Boss_HPBar;
                    m_BossInfo.Position_Damage = Position_Boss_Damage;
                    break;

                case eStageKind.STAGE5:
                    Img_Background.sprite = m_Background[4];
                    
                    CharacterBoss_IceGolem iceGolem = obj.GetComponent<CharacterBoss_IceGolem>();
                    m_BossInfo = iceGolem;
                    m_BossInfo.m_HPBar = Position_Boss_HPBar;
                    m_BossInfo.Position_Damage = Position_Boss_Damage;
                    break;
            }

            // 용사 초기화
            m_HeroInfo.Initialize();
            m_HeroInfo.Position_Damage = Obj_Damage_Position;
            m_HeroInfo.SetSkin(SetHeroAnimArmorSkinLevel(), SetHeroAnimSwordSkinLevel());
            m_HeroInfo.PlayAnimation(eCharacterAnimState.Standing, true);

            // 보스 초기화
            m_BossInfo.Initialize(BossID);

            switch (MainController.Instance.SelectStageKind)
            {
                case eStageKind.STAGE1:
                case eStageKind.STAGE2:
                case eStageKind.STAGE3:
                case eStageKind.STAGE4:
                    StartCoroutine(StartCountDown());
                    break;

                case eStageKind.STAGE5:
                    // 스토리 진행 때문에 별도로 진행
                    if(MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.Stage5BossStageStory) == false)
                    {
                        TutorialStorySystem.Instance.StartSpeach(eStoryState.Stage5BossStageStory);
                    }
                    else
                    {
                        StartCoroutine(StartCountDown());
                    }
                    break;
            }
                
        }
    }


    #region Hero Initialize
    public int SetHeroAnimArmorSkinLevel()
    {
        if (MainController.Instance == null)
        {
            Debug.LogError("MainController 실종");
            return 0;
        }

        if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) <
            MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count / 5)
        {
            return 1;
        }
        else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) >=
            MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count / 5 &&
            MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) <
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count * 2) / 5)
        {
            return 2;
        }
        else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) >=
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count * 2) / 5 &&
            MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) <
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count * 3) / 5)
        {
            return 3;
        }
        else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) >=
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count * 3) / 5 &&
            MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) <
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count * 4) / 5)
        {
            return 4;
        }
        else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) >=
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count * 4) / 5 &&
            MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) <=
            MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count)
        {
            return 5;
        }

        return 0;
    }

    public int SetHeroAnimSwordSkinLevel()
    {
        if (MainController.Instance == null)
        {
            Debug.LogError("Maincontroller 실종");
            return 0;
        }

        if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) <
            MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count / 5)
        {
            return 1;
        }
        else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
            MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count / 5 &&
            MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) <
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 2) / 5)
        {
            return 2;
        }
        else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 2) / 5 &&
            MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) <
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 3) / 5)
        {
            return 3;
        }
        else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
           (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 3) / 5 &&
           MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) <
           (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 4) / 5)
        {
            return 4;
        }
        else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
           (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 4) / 5 &&
            MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) <=
            MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count)
        {
            return 5;
        }

        return 0;
    }
    #endregion

    public void StartCountDownForStage5()
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown()
    {
        m_StageState = eBossStageState.CountDown;

        yield return new WaitUntil(() => CustomSceneManager.Instance.m_SceneChanging == false);

        m_BossInfo.PlayAnimation(eCharacterAnimState.Appear, false);

        yield return new WaitForSeconds(1.0f);

        m_BossInfo.PlayAnimation(eCharacterAnimState.Standing, true);

        GameObject obj = ResourceManager.GetOBJSetPositionPrefab("PrefabCountDown",
            gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z,
            gameObject.transform);
        CountDown count = obj.GetComponent<CountDown>();

        yield return new WaitUntil(() => count.m_IsDone == true);

        m_BossInfo.Appear = false;
        m_StageState = eBossStageState.Battle;

        StartCoroutine(HeroAttack());
    }

    IEnumerator Smash()
    {
        m_HeroInfo.StopAnimation();
        m_HeroInfo.PlayAnimation(eCharacterAnimState.Smash, false);

        yield return new WaitForSeconds(1.0f);

        int AttackLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK);
        float Attack = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.ATTACK, AttackLevel).Effect;

        int SmashLevel = MainController.Instance.UserInfo.GetUserSkillLevel(eHeroSkillKind.SMASH);
        float smashEffect = MainController.Instance.GetHeroSkillLevel(eHeroSkillKind.SMASH, SmashLevel).Effect;

        double FinalDamage = Math.Truncate(Attack * (smashEffect / 100));

        m_BossInfo.AttackedByEnemies((int)FinalDamage, true);

        m_HeroInfo.m_State = eHeroState.Attack;
    }

    IEnumerator Shield()
    {
        m_HeroInfo.StopAnimation();
        m_HeroInfo.PlayAnimation(eCharacterAnimState.Shield, false);

        m_HeroInfo.m_UseShield = true;

        yield return new WaitForSeconds(1.0f);

        m_HeroInfo.m_UseShield = false;

        m_HeroInfo.m_State = eHeroState.Attack;
    }

    IEnumerator Capture(float _time)
    {
        m_HeroInfo.StopAnimation();
        m_HeroInfo.PlayAnimation(eCharacterAnimState.Capture, false);

        yield return new WaitForSeconds(_time);

        // 보스 경직 = 애니메이션 정지

        m_HeroInfo.m_State = eHeroState.Attack;
    }

    IEnumerator DSpeed(float _time)
    {
        m_HeroInfo.StopAnimation();
        m_HeroInfo.PlayAnimation(eCharacterAnimState.DSpeed, false);

        yield return new WaitForSeconds(1.0f);

        m_HeroInfo.m_State = eHeroState.Attack;

        AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_SpeedValue =
            AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_SpeedValue * 2;

        int skillLevel = MainController.Instance.UserInfo.GetUserSkillLevel(eHeroSkillKind.DSPEED);

        yield return new WaitForSeconds(MainController.Instance.GetHeroSkillLevel(eHeroSkillKind.DSPEED, skillLevel).Effect);

        int speedLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED);
        AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_SpeedValue =
            MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, speedLevel).Effect;
    }

    IEnumerator HeroAttack()
    {
        int speedLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED);
        float speed = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, speedLevel).Effect;

        m_HeroInfo.m_State = eHeroState.Attack;

        while (!m_HeroInfo.IsDead)
        {
            switch (m_HeroInfo.m_State)
            {
                case eHeroState.Standing:
                    m_HeroInfo.PlayAnimation(eCharacterAnimState.Standing, true);
                    break;
                case eHeroState.Attack:
                    
                    StartCoroutine(m_HeroInfo.Attack(m_BossInfo));
                    yield return new WaitForSeconds(speed);
                    break;

                case eHeroState.Smash:
                    
                    StartCoroutine(Smash());
                    yield return new WaitForSeconds(1.0f);
                    break;
                case eHeroState.Shield:
                    
                    StartCoroutine(Shield());
                    yield return new WaitForSeconds(1.0f);
                    break;
                case eHeroState.Capture:
                    int CaptureLevel = MainController.Instance.UserInfo.GetUserSkillLevel(eHeroSkillKind.CAPTURE);
                    float CaptureEffect = MainController.Instance.GetHeroSkillLevel(eHeroSkillKind.CAPTURE, CaptureLevel).Effect;
                    
                    StartCoroutine(Capture(CaptureEffect));
                    yield return new WaitForSeconds(CaptureEffect);
                    break;
                case eHeroState.DSpeed:
                    int DSpeedLevel = MainController.Instance.UserInfo.GetUserSkillLevel(eHeroSkillKind.DSPEED);
                    float DSpeedEffect = MainController.Instance.GetHeroSkillLevel(eHeroSkillKind.DSPEED, DSpeedLevel).Effect;
                    
                    StartCoroutine(DSpeed(DSpeedEffect));
                    yield return new WaitForSeconds(DSpeedEffect);
                    break;

                case eHeroState.Win:
                    
                    m_HeroInfo.PlayAnimation(eCharacterAnimState.Standing, true);
                    yield return new WaitForSeconds(1.0f);
                    break;

                case eHeroState.Dead:
                    break;
            }
        }
    }
}
