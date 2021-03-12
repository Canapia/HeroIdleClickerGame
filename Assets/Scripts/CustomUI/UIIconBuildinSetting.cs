using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIIconBuildinSetting : MonoBehaviour
{
    #region Inspector
    [Header("- UI")]
    public Text Txt_BuildingName;
    public Text Txt_BuildingLevel;
    public Text Txt_NextPrice;
    public GameObject Obj_Gold;
    public CustomButton Btn_Info;
    [Header("- Building KIND")]
    public eBuildingKind m_BuildingKind = eBuildingKind.END;
    #endregion

    void Start()
    {
        // Renewal UI
        Txt_BuildingName.text = MainController.Instance.GetBuildingInfo(m_BuildingKind).Name;
        RenewalUI_Level();
        RenewalUI_NextPrice();

        if(Btn_Info != null)
        {
            Btn_Info.OnClick.AddListener(OnClickButton_ShowInfomationPopup);
        }
    }

    private void RenewalUI_Level()
    {
        if (MainController.Instance != null)
        {
            if(MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind) ==
                MainController.Instance.GetAllBuildingLevel(m_BuildingKind).Count)
            {
                Txt_BuildingLevel.text = "MAX";
                Txt_BuildingLevel.color = new Color(1f, 0.556f, 0.27f);
                return;
            }

            Txt_BuildingLevel.text = MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind).ToString();
        }
    }

    public void RenewalUI_NextPrice()
    {
        if (MainController.Instance != null)
        {
            if(MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind) ==
                MainController.Instance.GetAllBuildingLevel(m_BuildingKind).Count)
            {
                Obj_Gold.SetActive(false);
                return;
            }

            if(MainController.Instance.UserInfo.GetUserGold() < 
                NextPrice(MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind)))
            {
                Txt_NextPrice.color = new Color(0.823f, 0.333f, 0.313f);
            }

            if(MainController.Instance.UserInfo.GetUserGold() >=
                NextPrice(MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind)))
            {
                Txt_NextPrice.color = Color.white;
            }

            Txt_NextPrice.text = NextPrice(MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind)).ToString();
        }
    }

    private int NextPrice(int _Level)
    {
        return MainController.Instance.GetBuildingLevel(m_BuildingKind,
            MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind)).NextPrice;
    }

    public void OnClickButton_UpgradeBuilding()
    {
        if (MainController.Instance != null)
        {
            if(MainController.Instance.UserInfo.GetUserGold() >=
                NextPrice(MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind)) &&
                MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind) <
                MainController.Instance.GetAllBuildingLevel(m_BuildingKind).Count)
            {
                // User Info 변경
                MainController.Instance.UserInfo.ChangeUserGold((-1) *
                    NextPrice(MainController.Instance.UserInfo.GetUserBuildingLevel(m_BuildingKind)));
                MainController.Instance.UserInfo.UserBuildingLevel_LevelUp(m_BuildingKind);
                MainController.Instance.UserInfo.SaveUser();
                
                // Goods UI 변경
                if(GoodsSceneInstance.Instance != null)
                {
                    GoodsSceneInstance.Instance.RenewalUI_Gold();
                }

                // Building UI 변경
                RenewalUI_Level();
                RenewalUI_NextPrice();
            }
        }
    }

    public void OnClickButton_ShowInfomationPopup()
    {
        if(GoodsSceneInstance.Instance != null)
        {
            GoodsSceneInstance.Instance.ClearBossPopup(ePopupState.OnlyYes, "안 내",
                MainController.Instance.GetBuildingInfo(m_BuildingKind).Infomation, null);
        }
    }
}
