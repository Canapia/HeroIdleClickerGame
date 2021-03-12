using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PrefabContentListForm_Skill : MonoBehaviour
{
    #region Inspector
    public Image Img_Icon;
    public Text Txt_Level;
    public Text Txt_Name;
    public Text Txt_Info;
    public Text Txt_State;
    public Text Txt_NextPrice;
    public Image Img_GoldIcon;
    public CustomButton Btn_Gold;
    #endregion

    private eHeroSkillKind m_Kind;
    //private UnityEvent MasterEvent;

    public void Initialize(eHeroSkillKind _Kind)
    {
        m_Kind = _Kind;

        Renewal_Icon();
        Renewal_Level();
        Renewal_Name();
        Renewal_Info();
        Renewal_State();
        Renewal_NextPrice();
        Renewal_Button_Upgrade();

        //MasterEvent = _event;
        //_event.AddListener(Renewal_Button_Upgrade);
    }

    private void Update()
    {
        //Renewal_Button_Upgrade();
    }

    private void Renewal_Icon()
    {
        if(MainController.Instance != null)
        {
            Img_Icon.sprite = ResourceManager.LoadIconImage(
                MainController.Instance.GetHeroSkillInfo(m_Kind).IconName);
        }
    }

    private void Renewal_Level()
    {
        if(MainController.Instance != null)
        {
            Txt_Level.text = MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind).ToString();
        }
    }

    private void Renewal_Name()
    {
        if(MainController.Instance != null)
        {
            Txt_Name.text = MainController.Instance.GetHeroSkillInfo(m_Kind).Name;
        }
    }

    private void Renewal_Info()
    {
        if(MainController.Instance != null)
        {
            Txt_Info.text = MainController.Instance.GetHeroSkillInfo(m_Kind).Info;
        }
    }

    private void Renewal_State()
    {
        if(MainController.Instance != null)
        {
            Txt_State.text = string.Format(MainController.Instance.GetHeroSkillInfo(m_Kind).State
                , MainController.Instance.GetHeroSkillLevel(m_Kind,
                MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind)).Effect);
        }
    }

    private void Renewal_NextPrice()
    {
        if(MainController.Instance != null)
        {
            Txt_NextPrice.text = MainController.Instance.GetHeroSkillLevel(m_Kind,
                MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind)).NextPrice.ToString();
        }
    }

    public void Renewal_Button_Upgrade()
    {
        if(MainController.Instance != null)
        {
            if(MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind) ==
                MainController.Instance.GetAllHeroSkillLevel(m_Kind).Count)
            {
                Img_GoldIcon.gameObject.SetActive(false);
                Txt_NextPrice.text = "MAX!";
                Txt_NextPrice.color = new Color(1f, 0.556f, 0.27f);
                Btn_Gold.DisableButton();

                return;
            }

            if(MainController.Instance.UserInfo.GetUserGold() <
                MainController.Instance.GetHeroSkillLevel(m_Kind,
            MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind)).NextPrice)
            {
                Txt_NextPrice.color = new Color(0.823f, 0.333f, 0.313f);
                Btn_Gold.DisableButton();
            }

            if (MainController.Instance.UserInfo.GetUserGold() >=
                MainController.Instance.GetHeroSkillLevel(m_Kind,
            MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind)).NextPrice)
            {
                Txt_NextPrice.color = Color.white;
                Btn_Gold.UseableButton();
            }
        }
    }

    public void OnClickButton_Upgrade()
    {
        if(MainController.Instance == null)
        {
            Debug.LogError("MainController가 없습니다. 수행할 수 없습니다.");
            return;
        }

        if (MainController.Instance.UserInfo.GetUserGold() >=
            MainController.Instance.GetHeroSkillLevel(m_Kind,
            MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind)).NextPrice &&
            MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind) <
            MainController.Instance.GetAllHeroSkillLevel(m_Kind).Count)
        {
            // 돈 차감
            MainController.Instance.UserInfo.ChangeUserGold((-1) *
                MainController.Instance.GetHeroSkillLevel(m_Kind,
                MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind)).NextPrice);

            // 해당 스킬 레벨 업
            MainController.Instance.UserInfo.UserSkillLevel_LevelUp(m_Kind);

            // 변경 사항 저장
            MainController.Instance.UserInfo.SaveUser();

            // UI 변경
            Renewal_Level();
            Renewal_State();
            Renewal_NextPrice();
            Renewal_Button_Upgrade();

            //MasterEvent.Invoke();
        }
    }
}
