using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIIconStageSetting : MonoBehaviour
{
    #region Inspector
    [Header("- Stage UI")]
    public Text Txt_Stage_Name;
    public CustomButton Btn_Stage;
    [Header("- Stage Boss UI")]
    public CustomButton Btn_BossOpen;
    public Text Txt_Stage_Boss_Name;
    public Text Txt_RemainTime;
    public CustomButton Obj_Stage_Boss;
    [Header("- STAGE KIND")]
    public eStageKind m_StageKind = eStageKind.END;
    #endregion

    private int m_BossRemainTime;
    private int hours, minutes, seconds;
    private TimeSpan m_BossRemain;

    // Start is called before the first frame update
    void Start()
    {
        m_BossRemainTime = MainController.Instance.GetStageBossInfo(
            MainController.Instance.GetStageInfo_BossID(m_StageKind)).RemainTime;
        hours = (m_BossRemainTime / 60) / 60;
        minutes = m_BossRemainTime / 60;
        seconds = m_BossRemainTime % 60;
        m_BossRemain = new TimeSpan(hours, minutes, seconds);

        RenewalUI_BossOpen();

        RenewalUI_StageButton();
        RenewalUI_StageName();
        RenewalUI_StageBossName();
    }

    private void Update()
    {
        TimeSpan remain = DateTime.Now - MainController.Instance.UserInfo.GetStageBossTime(m_StageKind);

        TimeSpan total = m_BossRemain - remain;

        if (total.TotalSeconds < 0)
        {
            Obj_Stage_Boss.UseableButton();
            Txt_RemainTime.text = "Battle!";
        }
        else
        {
            //Obj_Stage_Boss.FineButNotUsedButton();
            Obj_Stage_Boss.UseableButton();
            Txt_RemainTime.text = String.Format("{0}:{1}:{2}", total.Hours, total.Minutes, total.Seconds);
        }
    }

    private void RenewalUI_BossOpen()
    {
        Btn_BossOpen.gameObject.SetActive(false);

        switch (m_StageKind)
        {
            case eStageKind.STAGE1:
                if (MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.FirstStart) == true &&
                    MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
                    MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count / 10)
                {
                    Btn_BossOpen.gameObject.SetActive(true);
                }
                if(MainController.Instance.UserInfo.GetUserStageBossShow(eStageKind.STAGE1) == true)
                {
                    Btn_BossOpen.gameObject.SetActive(false);
                }
                break;

            case eStageKind.STAGE2:
                if(MainController.Instance.UserInfo.GetUserStageBossClear(eStageKind.STAGE1) == true &&
                    MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.Stage1EndStory) == true &&
                    MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
                    (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 3)/ 10)
                {
                    Btn_BossOpen.gameObject.SetActive(true);
                }
                if (MainController.Instance.UserInfo.GetUserStageBossShow(eStageKind.STAGE2) == true)
                {
                    Btn_BossOpen.gameObject.SetActive(false);
                }
                break;

            case eStageKind.STAGE3:
                if (MainController.Instance.UserInfo.GetUserStageBossClear(eStageKind.STAGE2) == true &&
                    MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.Stage2EndStory) == true &&
                    MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
                    (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 5) / 10)
                {
                    Btn_BossOpen.gameObject.SetActive(true);
                }
                if (MainController.Instance.UserInfo.GetUserStageBossShow(eStageKind.STAGE3) == true)
                {
                    Btn_BossOpen.gameObject.SetActive(false);
                }
                break;

            case eStageKind.STAGE4:
                if (MainController.Instance.UserInfo.GetUserStageBossClear(eStageKind.STAGE3) == true &&
                    MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.Stage3EndStory) == true &&
                    MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
                    (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 7) / 10)
                {
                    Btn_BossOpen.gameObject.SetActive(true);
                }
                if (MainController.Instance.UserInfo.GetUserStageBossShow(eStageKind.STAGE4) == true)
                {
                    Btn_BossOpen.gameObject.SetActive(false);
                }
                break;

            case eStageKind.STAGE5:
                if (MainController.Instance.UserInfo.GetUserStageBossClear(eStageKind.STAGE4) == true &&
                    MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.Stage4EndStroy) == true &&
                    MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) >=
                    (MainController.Instance.GetAllHeroAbilityLevel(eHeroAbilityKind.ATTACK).Count * 9) / 10)
                {
                    Btn_BossOpen.gameObject.SetActive(true);
                }
                if (MainController.Instance.UserInfo.GetUserStageBossShow(eStageKind.STAGE5) == true)
                {
                    Btn_BossOpen.gameObject.SetActive(false);
                }
                break;
        }
    }

    private void RenewalUI_StageName()
    {
        if(MainController.Instance != null)
        {
            if (MainController.Instance.UserInfo.GetUserStageBossShow(m_StageKind) == true)
            {
                Txt_Stage_Name.gameObject.SetActive(false);
            }

            Txt_Stage_Name.text = MainController.Instance.GetStageInfo(m_StageKind).Name;
        }
    }

    private void RenewalUI_StageBossName()
    {
        if(MainController.Instance != null)
        {
            if(MainController.Instance.UserInfo.GetUserStageBossShow(m_StageKind) == false)
            {
                Obj_Stage_Boss.gameObject.SetActive(false);
            }
            else if(MainController.Instance.UserInfo.GetUserStageBossShow(m_StageKind) == true)
            {
                Obj_Stage_Boss.gameObject.SetActive(true);
                int bossID = MainController.Instance.GetStageInfo_BossID(m_StageKind);
                Txt_Stage_Boss_Name.text = MainController.Instance.GetStageBossInfo(bossID).Name;
            }
        }
    }

    private void RenewalUI_StageButton()
    {
        if (MainController.Instance.UserInfo.GetUserIsStageOpen(m_StageKind) == true &&
            MainController.Instance.UserInfo.GetUserStageBossShow(m_StageKind) == true)
        {
            Btn_Stage.FineButNotUsedButton();
        }
        else if (MainController.Instance.UserInfo.GetUserIsStageOpen(m_StageKind) == true &&
             MainController.Instance.UserInfo.GetUserStageBossShow(m_StageKind) == false)
        {
            Btn_Stage.UseableButton();
        }
        else if(MainController.Instance.UserInfo.GetUserIsStageOpen(m_StageKind) == false &&
             MainController.Instance.UserInfo.GetUserStageBossShow(m_StageKind) == false)
        {
            Btn_Stage.DisableButton();
        }
    }

    public void OnClickButton_StageSelect()
    {
        if(MainController.Instance != null)
        {
            MainController.Instance.SelectStageKind = m_StageKind;
        }

        CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInMap, eSceneState.AdventureInHunt);
    }

    public void SelectStage_Boss()
    {
        if (MainController.Instance != null)
        {
            MainController.Instance.SelectStageKind = m_StageKind;
        }

        CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInMap, eSceneState.AdventureInBoss);
    }

    public void OnClickButton_BossSelect()
    {
        //if(MainController.Instance != null)
        //{
        //    MainController.Instance.SelectStageKind = m_StageKind;
        //}

        //CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInMap, eSceneState.AdventureInBoss);

        TimeSpan remain = DateTime.Now - MainController.Instance.UserInfo.GetStageBossTime(m_StageKind);

        TimeSpan total = m_BossRemain - remain;

        if(total.TotalSeconds < 0)
        {
            // 보스 / 필드 스테이지 선택 추가
            if (GoodsSceneInstance.Instance != null)
            {
                GoodsSceneInstance.Instance.ShowPopup_BossOrField(
                    ePopupState.YesOrNo,
                    "알 림",
                    "정말 보스 스테이지에 진입하겠습니까?",
                    "보스로",
                    SelectStage_Boss,
                    "필드로",
                    OnClickButton_StageSelect);
            }
        }
        else
        {
            if (GoodsSceneInstance.Instance != null)
            {
                GoodsSceneInstance.Instance.ClearBossPopup(
                    ePopupState.YesOrNo,
                    "알 림",
                    "보스 스테이지 시간이 경과되지 않아 일반 필드로 이동합니다.",
                    OnClickButton_StageSelect);
            }
        }
    }

    public void OnClickButton_BossOpen()
    {
        if (MainController.Instance.UserInfo.GetUserGold() >=
            MainController.Instance.GetStageBossInfo(MainController.Instance.GetStageInfo(m_StageKind).BossID).OpenPay)
        {
            if (MainController.Instance.UserInfo.GetUserStageBossShow(m_StageKind) == false)
            {
                GoodsSceneInstance.Instance.ClearBossPopup(ePopupState.YesOrNo, "안 내",
                    "보스 스테이지를 구매하시겠습니까?",
                    PurchaseBossStage);
            }
        }
        else
        {
            GoodsSceneInstance.Instance.ClearBossPopup(ePopupState.OnlyYes, "안 내",
                string.Format("보스 스테이지를 여는데 필요한 골드가 부족합니다!\n필요 골드:{0}",
                MainController.Instance.GetStageBossInfo(MainController.Instance.GetStageInfo(m_StageKind).BossID).OpenPay),
                null);
        }
    }

    public void PurchaseBossStage()
    {
        MainController.Instance.UserInfo.ChangeUserGold((-1) *
            MainController.Instance.GetStageBossInfo(MainController.Instance.GetStageInfo(m_StageKind).BossID).OpenPay);

        MainController.Instance.UserInfo.ChangeUserStageBossShow(m_StageKind);
        MainController.Instance.UserInfo.SaveUser();

        RenewalUI_BossOpen();
        RenewalUI_StageButton();
        RenewalUI_StageName();
        RenewalUI_StageBossName();
    }
}
