using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum eStoryState
{
    FirstStart = 0,     // 게임 첫 시작
    Stage1EndStory,     // 보스1 끝나고 나오는 스토리
    Stage2EndStory,
    Stage3EndStory,
    Stage4EndStroy,
    Stage5BossStageStory,
    FinalStory,

    END
}

public struct ReStoreSpeach
{
    public int StorySpeachNumber { get; set; }
    public eSpeachCharacter SpeachCharacter { get; set; }
    public string Speach { get; set; }
}

public class TutorialStorySystem : MonoBehaviour, IPointerClickHandler
{
    #region
    public Image Img_BlackBG;
    public GameObject Obj_SpeachBox;
    public GameObject Position_SpeachBox_Up;
    public GameObject Position_SpeachBox_Down;
    public Text Txt_Speach;
    public CustomButton Btn_Skip;
    public Image Img_NPC;
    [Header("- Tutorial_01_GameoGuid")]
    public GameObject Group_Buildings;
    public GameObject Group_BottomUI;
    public CustomButton Btn_Adventure;
    public GameObject Btn_Shop;
    [Header("- Tutorial_02_Stage1Guid")]
    public GameObject Group_Maps;
    public CustomButton Btn_Stage1;
    #endregion

    private static TutorialStorySystem m_Instance;
    public static TutorialStorySystem Instance
    {
        get { return m_Instance; }
        set { m_Instance = value; }
    }

    private Dictionary<eStoryState, List<ReStoreSpeach>> m_ResotreSpeach;
    private int m_PresentSpeachNumber = 0;

    private eStoryState m_StoryState = eStoryState.FirstStart;
    public eStoryState StoryState
    {
        get { return m_StoryState; }
        set { m_StoryState = value; }
    }

    private WaitForSeconds WaitSecond = new WaitForSeconds(0f);
    private bool m_IsTalked = false;    // 말 하고 있음
    public bool m_IsTutorialing { get; set; }   // 튜토리얼 중...

    private void Awake()
    {
        if(m_Instance == null)
        {
            m_Instance = GetComponent<TutorialStorySystem>();
        }
        else
        {
            Destroy(this.gameObject);
        }

        m_ResotreSpeach = RestoreSpeach(TutorialStorySpeachLoader.DBLoad());

        m_IsTutorialing = false;

        // UI Setting
        Obj_SpeachBox.SetActive(false);
        Img_BlackBG.gameObject.SetActive(false);
        Btn_Skip.gameObject.SetActive(false);
        Img_NPC.gameObject.SetActive(false);
        Group_Buildings.SetActive(false);
        Group_BottomUI.SetActive(false);
        Group_Maps.SetActive(false);
        Btn_Stage1.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if(m_IsTutorialing == true)
                {
                    OnClickButton_Skip();
                }
            }
        }
    }

    private List<ReStoreSpeach> StateSpeachCount()
    {
        List<ReStoreSpeach> Restore = new List<ReStoreSpeach>();
        m_ResotreSpeach.TryGetValue(m_StoryState, out Restore);

        return Restore;
    }

    public void StartSpeach(eStoryState _state)
    {
        m_StoryState = _state;

        m_IsTutorialing = true;

        StartCoroutine(WaitStartSpeach());
    }

    IEnumerator WaitStartSpeach()
    {
        yield return new WaitUntil(() => CustomSceneManager.Instance.m_SceneChanging == false);

        m_IsTalked = true;

        yield return WaitSecond;

        //List<ReStoreSpeach> list = new List<ReStoreSpeach>();

        //m_ResotreSpeach.TryGetValue(m_StoryState, out list);

        if(m_PresentSpeachNumber == StateSpeachCount().Count)
        {
            OnClickButton_Skip();
            yield return 0;
        }

        /*
        switch (m_StoryState)
        {
            case eStoryState.FirstStart:
                if (m_PresentSpeachNumber < list.Count)
                {
                    BetweenSpeachAndFunction(list.Find(x=>x.StorySpeachNumber == m_PresentSpeachNumber));
                    //Txt_Speach.text = list.Find(x => x.StorySpeachNumber == m_PresentSpeachNumber).Speach;
                    if(list.Find(x => x.StorySpeachNumber == m_PresentSpeachNumber).SpeachCharacter == eSpeachCharacter.Narration)
                    {
                        Img_NPC.gameObject.SetActive(false);
                    }
                    else if(list.Find(x=>x.StorySpeachNumber == m_PresentSpeachNumber).SpeachCharacter == eSpeachCharacter.Village_Chief)
                    {
                        Img_NPC.gameObject.SetActive(true);
                    }
                }
                break;
            case eStoryState.GameGuid:
                break;
        }
        */
        if (m_PresentSpeachNumber < StateSpeachCount().Count && MainController.Instance.UserInfo.GetTutorialStoryClear(m_StoryState) == false)
        {
            Obj_SpeachBox.SetActive(true);
            Img_BlackBG.gameObject.SetActive(true);
            Btn_Skip.gameObject.SetActive(true);

            //Txt_Speach.text = list.Find(x => x.StorySpeachNumber == m_PresentSpeachNumber).Speach;
            if (StateSpeachCount().Find(x => x.StorySpeachNumber == m_PresentSpeachNumber).SpeachCharacter == eSpeachCharacter.Narration)
            {
                Img_NPC.gameObject.SetActive(false);
            }
            else if (StateSpeachCount().Find(x => x.StorySpeachNumber == m_PresentSpeachNumber).SpeachCharacter == eSpeachCharacter.Village_Chief)
            {
                Img_NPC.gameObject.SetActive(true);
            }
            BetweenSpeachAndFunction(StateSpeachCount().Find(x => x.StorySpeachNumber == m_PresentSpeachNumber));
        }

        //WaitSecond = new WaitForSeconds(0f);
        //m_IsTalked = false;
    }

    private void NextStoryState()
    {
        //OnClickButton_Skip();

        MainController.Instance.UserInfo.ChangeTutorialstoryClear(m_StoryState);

        m_PresentSpeachNumber = 0;
        m_StoryState++;

        StartSpeach(m_StoryState);
    }

    /// <summary>
    /// DB 내용에서 대사인지 함수인지 구분하는 함수
    /// 문장의 첫 문자가 -이면 함수이다.
    /// </summary>
    /// <param name="_speach"></param>
    private void BetweenSpeachAndFunction(ReStoreSpeach _speach)
    {
        if(_speach.Speach.Contains("%") == true)
        {
            FunctionInvoke(_speach.Speach.Replace("%", ""));

            Obj_SpeachBox.SetActive(false);
            Img_NPC.gameObject.SetActive(false);
            //Img_NPC.enabled = false;

            m_PresentSpeachNumber++;
            StartCoroutine(WaitStartSpeach());
        }
        else if(_speach.Speach.Contains("^") == true)
        {
            FunctionInvoke(_speach.Speach.Replace("^", ""));

            Obj_SpeachBox.SetActive(false);
            Img_NPC.gameObject.SetActive(false);

            m_IsTalked = false;
        }
        else
        {
            Txt_Speach.text = _speach.Speach;
            WaitSecond = new WaitForSeconds(0f);

            m_IsTalked = false;
        }
    }

    private Dictionary<eStoryState, List<ReStoreSpeach>> RestoreSpeach(List<TutorialStorySpeach> _list)
    {
        Dictionary<eStoryState, List<ReStoreSpeach>> dic = new Dictionary<eStoryState, List<ReStoreSpeach>>();

        if(_list != null)
        {
            for (int i = 0; i < (int)eStoryState.END; i++)
            {
                List<ReStoreSpeach> listINDic = new List<ReStoreSpeach>();
                foreach (TutorialStorySpeach speach in _list)
                {
                    if (speach.StoryState == (eStoryState)i)
                    {
                        ReStoreSpeach restore = new ReStoreSpeach();
                        restore.StorySpeachNumber = speach.StorySpeachnumber;
                        restore.SpeachCharacter = speach.SpeachCharacter;
                        restore.Speach = speach.Speach;

                        listINDic.Add(restore);
                    }
                }

                dic.Add((eStoryState)i, listINDic);
            }
        }

        return dic;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_IsTalked == true)
            return;

        //throw new System.NotImplementedException();
        m_PresentSpeachNumber++;

        StartCoroutine(WaitStartSpeach());
    }

    public void OnClickButton_Skip()
    {
        Obj_SpeachBox.SetActive(false);
        Img_BlackBG.gameObject.SetActive(false);
        Btn_Skip.gameObject.SetActive(false);
        Img_NPC.gameObject.SetActive(false);
        Group_Buildings.SetActive(false);
        Group_BottomUI.SetActive(false);
        Group_Maps.SetActive(false);
        Btn_Stage1.enabled = false;

        MainController.Instance.UserInfo.ChangeTutorialstoryClear(m_StoryState);
        MainController.Instance.UserInfo.SaveUser();

        //m_StoryState++;

        if (StateSpeachCount().Count != m_PresentSpeachNumber)
        {
            if (m_StoryState == eStoryState.Stage1EndStory ||
                m_StoryState == eStoryState.Stage2EndStory ||
                m_StoryState == eStoryState.Stage3EndStory ||
                m_StoryState == eStoryState.Stage4EndStroy)
            {
                Tutorial_StageGuid_ChangeScene();
                RewardBossClearStoryDone();
            }
        }

        m_PresentSpeachNumber = 0;

        m_IsTutorialing = false;

        /*
        if(m_StoryState == eStoryState.FirstStart)
        {
            m_StoryState = eStoryState.GameGuid;
            StartCoroutine(WaitStartSpeach());
        }
        */
    }

    private void SettingPositionDownOfSpeachBox()
    {
        Obj_SpeachBox.transform.SetParent(Position_SpeachBox_Down.transform);
        Obj_SpeachBox.transform.position = Position_SpeachBox_Down.transform.position;

        m_IsTalked = false;
    }

    private void SettingPositionUpOfSpeachBox()
    {
        Obj_SpeachBox.transform.SetParent(Position_SpeachBox_Up.transform);
        Obj_SpeachBox.transform.position = Position_SpeachBox_Up.transform.position;

        m_IsTalked = false;
    }

    private void ChangeAlphaOfBlackBG_UP()
    {
        WaitSecond = new WaitForSeconds(1f);

        Color color = Img_BlackBG.color;

        color.a = 1f;

        Img_BlackBG.color = color;

        m_IsTalked = false;
    }

    private void ChangeAlphaOfBlackBG_DOWN()
    {
        WaitSecond = new WaitForSeconds(1f);

        Color color = Img_BlackBG.color;

        color.a = 0.5f;

        Img_BlackBG.color = color;

        m_IsTalked = false;
    }

    private void ChangeAlphaOfBlackBG_Zero()
    {
        WaitSecond = new WaitForSeconds(1f);

        Color color = Img_BlackBG.color;

        color.a = 0f;

        Img_BlackBG.color = color;
    }

    private void RewardBossClearStoryDone()
    {
        Btn_Skip.gameObject.SetActive(false);

        WaitSecond = new WaitForSeconds(0f);

        // 재화 증가 능력치 적용
        int goldUpLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP);
        float goldUp = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.GOLDUP, goldUpLevel).Effect;
        // 반올림
        double GoldUpGold = 0;

        switch (m_StoryState)
        {
            case eStoryState.Stage1EndStory:
                GoldUpGold = Math.Round(MainController.Instance.GetStageBossInfo(
                    MainController.Instance.GetStageInfo_BossID(eStageKind.STAGE1)).DropGold * goldUp);
                // 스킬 해제
                MainController.Instance.UserInfo.ChangeUserIsUnlockSkill(eHeroSkillKind.SHIELD);
                MainController.Instance.UserInfo.ChangeUserIsStageOpen(eStageKind.STAGE2);
                MainController.Instance.UserInfo.ChangeUserStageBossClear(eStageKind.STAGE1);
                MainController.Instance.UserInfo.ChangeStageBossTime(eStageKind.STAGE1);
                break;

            case eStoryState.Stage2EndStory:
                GoldUpGold = Math.Round(MainController.Instance.GetStageBossInfo(
                    MainController.Instance.GetStageInfo_BossID(eStageKind.STAGE2)).DropGold * goldUp);
                // 스킬 해제
                //MainController.Instance.UserInfo.ChangeUserIsUnlockSkill(eHeroSkillKind.CAPTURE);
                MainController.Instance.UserInfo.ChangeUserIsStageOpen(eStageKind.STAGE3);
                MainController.Instance.UserInfo.ChangeUserStageBossClear(eStageKind.STAGE2);
                MainController.Instance.UserInfo.ChangeStageBossTime(eStageKind.STAGE2);
                break;

            case eStoryState.Stage3EndStory:
                GoldUpGold = Math.Round(MainController.Instance.GetStageBossInfo(
                    MainController.Instance.GetStageInfo_BossID(eStageKind.STAGE3)).DropGold * goldUp);
                MainController.Instance.UserInfo.ChangeUserIsStageOpen(eStageKind.STAGE4);
                MainController.Instance.UserInfo.ChangeUserStageBossClear(eStageKind.STAGE3);
                MainController.Instance.UserInfo.ChangeStageBossTime(eStageKind.STAGE3);
                break;

            case eStoryState.Stage4EndStroy:
                GoldUpGold = Math.Round(MainController.Instance.GetStageBossInfo(
                    MainController.Instance.GetStageInfo_BossID(eStageKind.STAGE4)).DropGold * goldUp);
                // 스킬 해제
                MainController.Instance.UserInfo.ChangeUserIsUnlockSkill(eHeroSkillKind.DSPEED);
                MainController.Instance.UserInfo.ChangeUserIsStageOpen(eStageKind.STAGE5);
                MainController.Instance.UserInfo.ChangeUserStageBossClear(eStageKind.STAGE4);
                MainController.Instance.UserInfo.ChangeStageBossTime(eStageKind.STAGE4);
                break;

            case eStoryState.Stage5BossStageStory:
                GoldUpGold = Math.Round(MainController.Instance.GetStageBossInfo(
                    MainController.Instance.GetStageInfo_BossID(eStageKind.STAGE5)).DropGold * goldUp);
                MainController.Instance.UserInfo.ChangeUserStageBossClear(eStageKind.STAGE5);
                MainController.Instance.UserInfo.ChangeStageBossTime(eStageKind.STAGE5);
                break;

            default:
                GoldUpGold = 0;
                break;
        }

        if(GoldUpGold > 0)
        {
            GoodsSceneInstance.Instance.ClearBossPopup(ePopupState.OnlyYes, "완 료!", string.Format("{0} Gold\n획득!", GoldUpGold), null);

            // 유저 정보 수정
            MainController.Instance.UserInfo.ChangeUserGold((int)GoldUpGold);
            MainController.Instance.UserInfo.SaveUser();
        }

        MainController.Instance.UserInfo.SaveUser();

        m_IsTalked = false;
    }

    private void Tutorial_01_GameGuid_ShowBuilding()
    {
        WaitSecond = new WaitForSeconds(1f);

        //ChangeAlphaOfBlackBG_DOWN();

        Group_Buildings.SetActive(true);
    }

    private void Tutorial_01_GameGuid_HideBuilding()
    {
        WaitSecond = new WaitForSeconds(0f);

        //ChangeAlphaOfBlackBG_DOWN();

        Group_Buildings.SetActive(false);
    }

    private void Tutorial_01_GameGuid_ShowButtonGroup()
    {
        WaitSecond = new WaitForSeconds(1f);

        Group_BottomUI.SetActive(true);
        Btn_Adventure.enabled = false;
    }

    private void Tutorial_01_GameGuid_HideButtonGroup()
    {
        WaitSecond = new WaitForSeconds(0f);

        Group_BottomUI.SetActive(false);
    }

    private void Tutorial_01_GameGuid_HideShopButton()
    {
        WaitSecond = new WaitForSeconds(0f);

        Btn_Shop.SetActive(false);
        Btn_Adventure.enabled = true;
    }

    private void Tutorial_02_Stage1Guid_ShowStage1Button()
    {
        WaitSecond = new WaitForSeconds(0f);

        Group_Maps.SetActive(true);
        Btn_Stage1.enabled = true;
    }

    private void Tutorial_05_Stage05BossStageStory_StartBattle()
    {
        AdventureModeInBossSceneUI.Instance.StartCountDownForStage5();
    }

    private void Tutorial_StageGuid_ChangeScene()
    {
        Btn_Skip.gameObject.SetActive(false);

        WaitSecond = new WaitForSeconds(2f);

        Btn_Skip.gameObject.SetActive(false);

        if (CustomSceneManager.Instance != null)
        {
            CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInBoss, eSceneState.Main);
        }

        //m_IsTalked = false;
    }

    public void OnClickButton_Adventure()
    {
        if(Btn_Adventure.enabled == true)
        {
            m_IsTalked = true;

            Group_BottomUI.SetActive(false);
            Btn_Skip.gameObject.SetActive(false);

            CustomSceneManager.Instance.ChangeScene(eSceneState.Main, eSceneState.AdventureInMap);

            StartCoroutine(WaitForSceneChange());
        }
    }

    IEnumerator WaitForSceneChange()
    {
        yield return new WaitUntil(() => CustomSceneManager.Instance.m_SceneChanging == false);

        yield return new WaitUntil(() => CustomSceneManager.Instance.m_Scenestate == eSceneState.AdventureInMap);

        m_IsTalked = false;
        Btn_Skip.gameObject.SetActive(true);
        m_PresentSpeachNumber++;
        StartCoroutine(WaitStartSpeach());
    }

    public void OnClickButton_Stage1()
    {
        if(Btn_Stage1.enabled == true)
        {
            m_IsTalked = true;

            Group_Maps.SetActive(false);
            Btn_Skip.gameObject.SetActive(false);

            if (MainController.Instance != null)
            {
                MainController.Instance.SelectStageKind = eStageKind.STAGE1;
            }

            CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInMap, eSceneState.AdventureInHunt);

            StartCoroutine(WaitForSceneChange());
        }
    }

    private void FunctionInvoke(string _name)
    {
        //TutorialStorySystem tytorial = new TutorialStorySystem();

        Type type = m_Instance.GetType();

        MethodInfo method = type.GetMethod(_name, BindingFlags.Instance | BindingFlags.NonPublic);

        method.Invoke(m_Instance, null);
    }
}
