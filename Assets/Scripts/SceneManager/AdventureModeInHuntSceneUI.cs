using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public enum eHuntSceneState
{
    Ready = 0,  // 준비
    CountDown,  // 카운트다운
    Hunting,    // 전투 중 
    Pause,      // 일시정지
    ReStart,    // 재 시작
    HeroDead,   // 용사 사망

    END
}

public class AdventureModeInHuntSceneUI : MonoBehaviour
{
    #region Inspector
    [Header("Background Object")]
    public Image Img_RearBG1;
    public Image Img_RearBG2;
    public Image Img_Town;
    public Image Img_Front1;
    public Image Img_Front2;
    public ScrollingBackground[] m_ScrollingBG;
    public CharacterHero m_HeroInfo;
    public GameObject Position_Monster;
    public GameObject Position_Damage;
    [Header("Background Img")]
    public Sprite[] m_RearBG;
    public Sprite[] m_FrontBG;
    #endregion

    #region Instance
    private static AdventureModeInHuntSceneUI m_Instance;
    public static AdventureModeInHuntSceneUI Instance
    {
        get { return m_Instance; }
        set { m_Instance = value; }
    }
    #endregion

    // 몬스터 소환 관련
    private int[] m_MonsterID;
    private CharacterMonster[] m_MonsterInfo;
    private int m_NowMonsterNum = 0;

    // 전투 실행 관련
    private eHuntSceneState m_HuntState = eHuntSceneState.Ready;
    public eHuntSceneState HuntState
    {
        get { return m_HuntState; }
        set { m_HuntState = value; }
    }

    private void Awake()
    {
        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);

        // Instance 할당
        if (m_Instance == null)
        {
            m_Instance = GetComponent<AdventureModeInHuntSceneUI>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 화면 꺼짐 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        if (MainController.Instance != null)
        {
            SoundManager.Instance.PlayBGM(eBGMState.Stage);

            #region Background Image Change : 배경 이미지 변경
            Img_RearBG1.sprite = m_RearBG[(int)MainController.Instance.SelectStageKind];
            Img_RearBG2.sprite = m_RearBG[(int)MainController.Instance.SelectStageKind];
            Img_Front1.sprite = m_FrontBG[(int)MainController.Instance.SelectStageKind];
            Img_Front2.sprite = m_FrontBG[(int)MainController.Instance.SelectStageKind];

            switch (MainController.Instance.SelectStageKind)
            {
                case eStageKind.STAGE1:
                    Img_Front1.rectTransform.sizeDelta = new Vector2(2048, 342);
                    Img_Front1.transform.localPosition = new Vector3(
                        Img_Front1.transform.localPosition.x, -330, 0);
                    Img_Front2.rectTransform.sizeDelta = new Vector2(2048, 342);
                    Img_Front2.transform.localPosition = new Vector3(
                        Img_Front2.transform.localPosition.x, -330, 0);
                    break;

                case eStageKind.STAGE2:
                    Img_Town.rectTransform.sizeDelta = new Vector2(1649, 833.5f);
                    break;

                case eStageKind.STAGE3:
                    Img_Town.enabled = false;
                    break;

                case eStageKind.STAGE4:
                    Img_Front1.rectTransform.sizeDelta = new Vector2(2048, 474);
                    Img_Front1.transform.localPosition = new Vector3(
                        Img_Front1.transform.localPosition.x, -270, 0);
                    Img_Front2.rectTransform.sizeDelta = new Vector2(2048, 474);
                    Img_Front2.transform.localPosition = new Vector3(
                        Img_Front2.transform.localPosition.x, -270, 0);

                    Img_Town.rectTransform.sizeDelta = new Vector2(484.8f, 255f);
                    Img_Town.transform.localPosition = new Vector3(
                        Img_Town.transform.localPosition.x, -190, 0);
                    break;

                case eStageKind.STAGE5:
                    Img_Town.enabled = false;
                    break;
            }
            #endregion

            // 몬스터 정보 저장
            m_MonsterID = new int[
                MainController.Instance.GetStageInfo(
                    MainController.Instance.SelectStageKind).MonsterID.Length];
        }

        // 능력 / 스킬 탭 설정
        if (AdventureSceneManager.Instance != null)
        {
            //AdventureSceneManager.Instance.Tab_Group.SetUseable(1, false);
        }

        // 몬스터 랜덤 소환
        for (int i = 0; i < m_MonsterID.Length; i++)
        {
            m_MonsterID[i] = MainController.Instance.GetStageInfo(MainController.Instance.SelectStageKind).MonsterID[i];
        }
        m_MonsterInfo = new CharacterMonster[2];    // 0 : 진행, 1: 대기
        FirstMonsterSummons();
        StartCoroutine(CheckNowMonster());

        // 용사 설정
        m_HeroInfo.Initialize();
        m_HeroInfo.Position_Damage = Position_Damage;
        m_HeroInfo.SetSkin(SetHeroAnimArmorSkinLevel(), SetHeroAnimSwordSkinLevel());

        // 인 게임 일시정지
        PauseInGamePlay();
        // 카운트 다운 시작
        StartCoroutine(StartCountDown());
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if(CustomSceneManager.Instance != null)
            {
                if(CustomSceneManager.Instance.m_SceneChanging == false)
                {
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        if (AdventureModeInHuntSceneUI.Instance != null &&
                            GoodsSceneInstance.Instance != null)
                        {
                            // 카운트 다운 일 경우 무시
                            if (AdventureModeInHuntSceneUI.Instance.HuntState == eHuntSceneState.CountDown)
                            {
                                return;
                            }
                            AdventureModeInHuntSceneUI.Instance.PauseInGamePlay();
                        }

                        if(MainController.Instance != null)
                        {
                            if(MainController.Instance.m_IsPopup == false)
                            {
                                ResourceManager.CreatePrefab("Prefab_Pause", //gameObject.transform);
                            GoodsSceneInstance.Instance.Obj_Position.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    #region Monster : 몬스터 관련 함수
    private void FirstMonsterSummons()
    {
        for (int i = 0; i < m_MonsterInfo.Length; i++)
        {
            m_MonsterInfo[i] = CreateMonster(RandomMonsterID(m_MonsterID));
            m_MonsterInfo[i].PlayAnimation(eCharacterAnimState.Run, true);
            m_MonsterInfo[i].StopMoveMonster();
        }
    }

    IEnumerator CheckNowMonster()
    {
        yield return new WaitUntil(() => m_MonsterInfo[m_NowMonsterNum].IsDead == true);

        if (m_NowMonsterNum == 0)
        {
            StartCoroutine(CheckDestroyBeforeMonster(0));
            m_NowMonsterNum = 1;
        }
        else if (m_NowMonsterNum == 1)
        {
            StartCoroutine(CheckDestroyBeforeMonster(1));
            m_NowMonsterNum = 0;
        }

        if (m_HeroInfo.IsDead == false)
        {
            m_MonsterInfo[m_NowMonsterNum].StartMoveMonster();
            //m_MonsterInfo[m_NowMonsterNum].IsMove = true;

            StartCoroutine(CheckNowMonster());
        }
    }
    IEnumerator CheckDestroyBeforeMonster(int _num)
    {
        yield return new WaitUntil(() => m_MonsterInfo[_num] == null);

        m_MonsterInfo[_num] = CreateMonster(RandomMonsterID(m_MonsterID));
        m_MonsterInfo[_num].PlayAnimation(eCharacterAnimState.Run, true);
    }
    #endregion

    IEnumerator StartCountDown()
    {
        m_HuntState = eHuntSceneState.CountDown;
        yield return new WaitUntil(() => CustomSceneManager.Instance.m_SceneChanging == false);

        GameObject obj = ResourceManager.GetOBJSetPositionPrefab("PrefabCountDown",
            gameObject.transform.position.x, gameObject.transform.position.y + 390, gameObject.transform.position.z,
            gameObject.transform);
        CountDown count = obj.GetComponent<CountDown>();

        yield return new WaitUntil(() => count.m_IsDone == true);

        StartInGamePlay();
    }

    public void PublicStartCountDown()
    {
        StartCoroutine(StartCountDown());
    }

    public void StartInGamePlay()
    {
        if (m_HeroInfo.IsDead == true)
            return;

        if(m_HuntState == eHuntSceneState.CountDown)
        {
            m_HuntState = eHuntSceneState.Hunting;

            //StartBackground();

            m_HeroInfo.PlayAnimation(eCharacterAnimState.Run, true);

            if (m_MonsterInfo != null)
            {
                foreach (CharacterMonster monster in m_MonsterInfo)
                {
                    //m_MonsterInfo[m_NowMonsterNum].IsPause = false;
                    monster.IsPause = false;
                }
                if (m_MonsterInfo[m_NowMonsterNum].IsDead == false && m_MonsterInfo[m_NowMonsterNum].IsMove == true)
                {
                    m_MonsterInfo[m_NowMonsterNum].PlayAnimation(eCharacterAnimState.Run, true);
                    m_MonsterInfo[m_NowMonsterNum].StartMoveMonster();
                }
                else if (m_MonsterInfo[m_NowMonsterNum].IsDead == false && m_MonsterInfo[m_NowMonsterNum].IsMove == false)
                {
                    m_MonsterInfo[m_NowMonsterNum].StopMoveMonster();
                    m_MonsterInfo[m_NowMonsterNum].PlayAnimation(eCharacterAnimState.Run, true);
                }
            }

            if(m_HeroInfo != null && m_HeroInfo.IsDead == false)
            {
                m_HeroInfo.IsPause = false;
                //m_HeroInfo.IsMove = true;
            }

            if(m_HeroInfo.IsMove == false)
            {
                StopBackground();
            }
            else if(m_HeroInfo.IsMove == true)
            {
                StartBackground();
            }
        }
    }

    public void PauseInGamePlay()
    {
        if(m_HuntState == eHuntSceneState.Ready)
        {
            StopBackground();

            m_HeroInfo.PlayAnimation(eCharacterAnimState.Standing, true);
        }
        else if(m_HuntState == eHuntSceneState.Hunting && m_HeroInfo.IsDead == false)
        {
            m_HuntState = eHuntSceneState.Pause;

            StopBackground();

            if(m_MonsterInfo != null)
            {
                foreach (CharacterMonster monster in m_MonsterInfo)
                {
                    monster.IsPause = true;
                    monster.StopAnimation();
                    monster.StopMoveMonster();
                }
            }

            if(m_HeroInfo != null && m_HeroInfo.IsDead == false)
            {
                m_HeroInfo.IsPause = true;
                m_HeroInfo.StopAnimation();
            }
        }
        else if(m_HeroInfo.IsDead == true)
        {
            m_HuntState = eHuntSceneState.HeroDead;

            StopBackground();

            m_HeroInfo.PlayAnimation(eCharacterAnimState.Dead, false);
        }
    }

    public void StartBackground()
    {
        foreach (ScrollingBackground scroll in m_ScrollingBG)
        {
            scroll.startScrolling();
        }
    }

    public void StopBackground()
    {
        foreach (ScrollingBackground scroll in m_ScrollingBG)
        {
            scroll.StopScrolling();
        }
    }

    public void CoroutineStartConutDown()
    {
        StartCoroutine(StartCountDown());
    }



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
            (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.DEF).Count * 2 ) / 5 &&
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
        if(MainController.Instance == null)
        {
            Debug.LogError("Maincontroller 실종");
            return 0;
        }

        if(MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) <
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

    private int RandomMonsterID(int[] _IDs)
    {
        int random = Random.Range(0, _IDs.Length);

        return _IDs[random];
    }

    private CharacterMonster CreateMonster(int _ID)
    {
        GameObject obj = ResourceManager.GetOBJCreatePrefab("Prefab_Monster", Position_Monster.transform);
        CharacterMonster monster = obj.GetComponent<CharacterMonster>();

        monster.Initialize(_ID);

        return monster;
    }
}
