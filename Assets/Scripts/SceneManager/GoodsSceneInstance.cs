using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GoodsSceneInstance
{
    #region Instance
    private static GoodsSceneInstance m_Instance;
    public static GoodsSceneInstance Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new GoodsSceneInstance();
            }
            return m_Instance;
        }
        set { m_Instance = value; }
    }
    #endregion

    #region UI
    private Text Txt_Gold;
    private Text Txt_Jam;
    private CustomButton Btn_Option;
    private CustomButton Btn_Back;
    private CustomButton Btn_Pause;
    #endregion

    public GameObject Obj_Position;

    #region Renewal UI Fuction
    public void RenewalUI_Gold(Text _Obj)
    {
        if(MainController.Instance != null)
        {
            Txt_Gold = _Obj;
            RenewalUI_Gold();
        }
    }

    public void RenewalUI_Gold()
    {
        if(Txt_Gold != null)
        {
            Txt_Gold.text = MainController.Instance.UserInfo.GetUserGold().ToString();
        }
    }

    public void RenewalUI_Jam(Text _Obj)
    {
        if(MainController.Instance != null)
        {
            Txt_Jam = _Obj;
            RenewalUI_Jam();
        }
    }

    public void RenewalUI_Jam()
    {
        if(Txt_Jam != null)
        {
            Txt_Jam.text = MainController.Instance.UserInfo.UserJam.ToString();
        }
    }

    public void ChangeButtonState(CustomButton _option, CustomButton _Back, CustomButton _Pause)
    {
        Btn_Option = _option;
        Btn_Back = _Back;
        Btn_Pause = _Pause;

        ChangeButtonState();
    }

    public void ChangeButtonState()
    {
        if (CustomSceneManager.Instance != null 
            && Btn_Option != null && Btn_Back != null && Btn_Pause != null)
        {
            switch (CustomSceneManager.Instance.m_Scenestate)
            {
                case eSceneState.Main:
                    Btn_Option.gameObject.SetActive(true);
                    Btn_Back.gameObject.SetActive(false);
                    Btn_Pause.gameObject.SetActive(false);
                    break;
                case eSceneState.AdventureInMap:
                    Btn_Option.gameObject.SetActive(false);
                    Btn_Back.gameObject.SetActive(true);
                    Btn_Pause.gameObject.SetActive(false);
                    break;
                case eSceneState.AdventureInHunt:
                    Btn_Option.gameObject.SetActive(false);
                    Btn_Back.gameObject.SetActive(false);
                    Btn_Pause.gameObject.SetActive(true);
                    break;
                default:
                    Btn_Option.gameObject.SetActive(false);
                    Btn_Back.gameObject.SetActive(false);
                    Btn_Pause.gameObject.SetActive(false);
                    break;
            }
        }
    }
    #endregion


    public void ClearBossPopup(ePopupState _state, string _title, string _content, UnityAction _func)
    {
        GameObject popupObj = ResourceManager.GetOBJCreatePrefab("PrefabYesOrNoPopup", Obj_Position.transform);
        PrefabYesOrNoPopup popup = popupObj.GetComponent<PrefabYesOrNoPopup>();
        // 추후 DB 또는 관리를 하나 만들어서 출력할 것
        popup.Initialize(_state, _title, _content, _func);
    }

    public void ShowPopup_UserInfo_ReStart()
    {
        GameObject obj = ResourceManager.GetOBJCreatePrefab("PrefabYesOrNoPopup", Obj_Position.transform);
        PrefabYesOrNoPopup popup = obj.GetComponent<PrefabYesOrNoPopup>();
        // 추후 DB 또는 관리를 하나 만들어서 출력할 것
        popup.Initialize(ePopupState.YesOrNo, "알림!", "계정을 초기화하면 현재까지 진행한 모든 내용이 사라집니다. 진행하시겠습니까?",
            PopupYesDoing);
    }

    public void ShowPopup(string _popupName)
    {
        GameObject popupObj = ResourceManager.GetOBJCreatePrefab(_popupName, Obj_Position.transform);
        PrefabYesOrNoPopup popup = popupObj.GetComponent<PrefabYesOrNoPopup>();
    }

    public void ShowPopup_BossOrField(ePopupState _state, string _title, string _content, string _YesText, UnityAction _YesFunc, string _NoText,UnityAction _NoFunc)
    {
        GameObject popupObj = ResourceManager.GetOBJCreatePrefab("PrefabYesOrNoPopup", Obj_Position.transform);
        PrefabYesOrNoPopup popup = popupObj.GetComponent<PrefabYesOrNoPopup>();
        // 추후 DB 또는 관리를 하나 만들어서 출력할 것
        popup.Initialize(_state, _title, _content, _YesText, _YesFunc, _NoText, _NoFunc);
    }

    public void PopupYesDoing()
    {
        User _newInfo = new User();
        MainController.Instance.UserInfo = _newInfo;
        MainController.Instance.UserInfo.SaveUser();

        // 임시로 재시작 하는 것 처럼 보임
        CustomSceneManager.Instance.ChangeScene(eSceneState.Main, eSceneState.Title);
    }
}
