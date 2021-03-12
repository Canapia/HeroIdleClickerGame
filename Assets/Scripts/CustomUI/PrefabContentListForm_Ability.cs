using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PrefabContentListForm_Ability : MonoBehaviour
{
    #region Inspector
    public Image Img_Icon;
    public Text Txt_Level;
    //public Text Txt_Name;
    public Text Txt_Info;
    public Text Txt_State;
    public Text Txt_NextPrice;
    public Image Img_GoldIcon;
    public CustomButton Btn_Gold;
    #endregion

    private eHeroAbilityKind m_Kind;
    //private UnityEvent MasterEvent;

    public void Initialize(eHeroAbilityKind _Kind)
    {
        m_Kind = _Kind;

        Renewal_Icon();
        Renewal_Level();
        Renewal_Info();
        Renewal_State();
        Renewal_NextPrice();
        Renewal_Button_Upgrade();

        //MasterEvent = _event;
        //_event.AddListener(Renewal_Button_Upgrade);
    }

    private void Update()
    {
        // 문제!!!
        //Renewal_Button_Upgrade();
    }

    private void Renewal_Icon()
    {
        if(MainController.Instance != null)
        {
            Img_Icon.sprite = ResourceManager.LoadIconImage(
                MainController.Instance.GetHeroAbilityInfo(m_Kind).IconName);
        }
    }

    private void Renewal_Level()
    {
        if(MainController.Instance != null)
        {
            Txt_Level.text = MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind).ToString();
        }
    }

    private void Renewal_Info()
    {
        if(MainController.Instance != null)
        {
            Txt_Info.text = MainController.Instance.GetHeroAbilityInfo(m_Kind).Info;
        }
    }

    private void Renewal_State()
    {
        if(MainController.Instance != null)
        {
            Txt_State.text = string.Format(MainController.Instance.GetHeroAbilityInfo(m_Kind).State,
                MainController.Instance.GetHeroAbilityLevel(m_Kind,
                MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind)).Effect);

            if(m_Kind == eHeroAbilityKind.SPEED)
            {
                float effect = 2.0f - MainController.Instance.GetHeroAbilityLevel(m_Kind,
                MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind)).Effect;

                Txt_State.text = String.Format(MainController.Instance.GetHeroAbilityInfo(m_Kind).State, effect);
            }
        }
    }

    private void Renewal_NextPrice()
    {
        if(MainController.Instance != null)
        {
            Txt_NextPrice.text = MainController.Instance.GetHeroAbilityLevel(m_Kind,
                MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind)).NextPrice.ToString();
        }
    }

    public void Renewal_Button_Upgrade()
    {
        if(MainController.Instance != null)
        {
            if(MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind) == 
                MainController.Instance.GetAllHeroAbilityLevel(m_Kind).Count)
            {
                Img_GoldIcon.gameObject.SetActive(false);
                Txt_NextPrice.text = "MAX!";
                Txt_NextPrice.color = new Color(1f, 0.556f, 0.27f);
                Btn_Gold.DisableButton();

                return;
            }

            if(MainController.Instance.UserInfo.GetUserGold() <
                MainController.Instance.GetHeroAbilityLevel(m_Kind,
                MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind)).NextPrice)
            {
                Txt_NextPrice.color = new Color(0.823f, 0.333f, 0.313f);
                Btn_Gold.DisableButton();
            }

            if(MainController.Instance.UserInfo.GetUserGold() >=
                 MainController.Instance.GetHeroAbilityLevel(m_Kind,
                MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind)).NextPrice)
            {
                Txt_NextPrice.color = Color.white;
                Btn_Gold.UseableButton();
            }

            // 건물 레벨에 따른 능력치 레벨 제한
            LimitAbilityOfBuildingLevel();
        }
    }

    public void OnClickButton_upgrade()
    {
        if(MainController.Instance == null || GoodsSceneInstance.Instance == null)
        {
            Debug.LogError("MainController 또는 GoodsSceneInstance 가 없습니다. 수행할 수 없습니다.");
            return;
        }

        if(MainController.Instance.UserInfo.GetUserGold() >=
            MainController.Instance.GetHeroAbilityLevel(m_Kind,
            MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind)).NextPrice &&
            MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind) <
            MainController.Instance.GetAllHeroAbilityLevel(m_Kind).Count)
        {
            // 돈 차감
            MainController.Instance.UserInfo.ChangeUserGold((-1) *
            MainController.Instance.GetHeroAbilityLevel(m_Kind,
            MainController.Instance.UserInfo.GetUserAbilityLevel(m_Kind)).NextPrice);

            // 해당 스킬 레벨 업
            MainController.Instance.UserAbilityLevelUp(m_Kind);

            // 변경 사항 저장
            MainController.Instance.UserInfo.SaveUser();

            // UI 변경
            Renewal_Level();
            Renewal_State();
            Renewal_NextPrice();
            Renewal_Button_Upgrade();

            //MasterEvent.Invoke();
        }

        if(AdventureSceneManager.Instance != null)
        {
            AdventureSceneManager.Instance.RenewalTabUI();
        }
    }

    private void LimitAbilityOfBuildingLevel()
    {
        switch(m_Kind)
        {
            case eHeroAbilityKind.ATTACK:
                if(MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) == 10)
                {
                    if(MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.WEAPON) < 10)
                    {
                        UIForLimitAbiliyty(eBuildingKind.WEAPON, 10);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) == 30)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.WEAPON) < 20)
                    {
                        UIForLimitAbiliyty(eBuildingKind.WEAPON, 20);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) == 50)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.WEAPON) < 30)
                    {
                        UIForLimitAbiliyty(eBuildingKind.WEAPON, 30);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) == 70)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.WEAPON) < 40)
                    {
                        UIForLimitAbiliyty(eBuildingKind.WEAPON, 40);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK) == 90)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.WEAPON) < 50)
                    {
                        UIForLimitAbiliyty(eBuildingKind.WEAPON, 50);
                    }
                }
                break;

            case eHeroAbilityKind.DEF:
                if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) == 10)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.ARMOR) < 10)
                    {
                        UIForLimitAbiliyty(eBuildingKind.ARMOR, 10);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) == 30)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.ARMOR) < 20)
                    {
                        UIForLimitAbiliyty(eBuildingKind.ARMOR, 20);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) == 50)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.ARMOR) < 30)
                    {
                        UIForLimitAbiliyty(eBuildingKind.ARMOR, 30);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) == 70)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.ARMOR) < 40)
                    {
                        UIForLimitAbiliyty(eBuildingKind.ARMOR, 40);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF) == 90)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.ARMOR) < 50)
                    {
                        UIForLimitAbiliyty(eBuildingKind.ARMOR, 50);
                    }
                }
                break;

            case eHeroAbilityKind.POTION:
                if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.POTION) == 10)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.POTION) < 10)
                    {
                        UIForLimitAbiliyty(eBuildingKind.POTION, 10);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.POTION) == 30)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.POTION) < 20)
                    {
                        UIForLimitAbiliyty(eBuildingKind.POTION, 20);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.POTION) == 50)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.POTION) < 30)
                    {
                        UIForLimitAbiliyty(eBuildingKind.POTION, 30);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.POTION) == 70)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.POTION) < 40)
                    {
                        UIForLimitAbiliyty(eBuildingKind.POTION, 40);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.POTION) == 90)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.POTION) < 50)
                    {
                        UIForLimitAbiliyty(eBuildingKind.POTION, 50);
                    }
                }
                break;

            case eHeroAbilityKind.HP:
                if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.HP) == 5)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 10)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 10);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.HP) == 25)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 20)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 20);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.HP) == 45)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 30)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 30);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.HP) == 65)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 40)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 40);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.HP) == 85)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 50)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 50);
                    }
                }
                break;

            case eHeroAbilityKind.SPEED:
                if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED) == 10)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 10)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 10);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED) == 30)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 20)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 20);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED) == 50)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 30)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 30);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED) == 70)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 40)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 40);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED) == 90)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 50)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 50);
                    }
                }
                break;

            case eHeroAbilityKind.CRITICAL:
                if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.CRITICAL) == 15)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 10)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 10);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.CRITICAL) == 35)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 20)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 20);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.CRITICAL) == 55)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 30)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 30);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.CRITICAL) == 75)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 40)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 40);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.CRITICAL) == 95)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.TRAINING) < 50)
                    {
                        UIForLimitAbiliyty(eBuildingKind.TRAINING, 50);
                    }
                }
                break;

            case eHeroAbilityKind.GOLDUP:
                if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP) == 10)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.BANK) < 10)
                    {
                        UIForLimitAbiliyty(eBuildingKind.BANK, 10);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP) == 30)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.BANK) < 20)
                    {
                        UIForLimitAbiliyty(eBuildingKind.BANK, 20);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP) == 50)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.BANK) < 30)
                    {
                        UIForLimitAbiliyty(eBuildingKind.BANK, 30);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP) == 70)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.BANK) < 40)
                    {
                        UIForLimitAbiliyty(eBuildingKind.BANK, 40);
                    }
                }
                else if (MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP) == 90)
                {
                    if (MainController.Instance.UserInfo.GetUserBuildingLevel(eBuildingKind.BANK) < 50)
                    {
                        UIForLimitAbiliyty(eBuildingKind.BANK, 50);
                    }
                }
                break;
        }
    }

    private void UIForLimitAbiliyty(eBuildingKind _building, int _level)
    {
        Img_GoldIcon.enabled = false;
        Txt_NextPrice.color = new Color(0.823f, 0.333f, 0.313f);
        Txt_NextPrice.text = String.Format("{0}\n레벨 {1} 필요", 
            MainController.Instance.GetBuildingInfo(_building).Name, _level);
        Txt_NextPrice.fontSize = 50;
        Btn_Gold.DisableButton();
    }
}
