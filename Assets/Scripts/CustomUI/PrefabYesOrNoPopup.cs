using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum ePopupState
{
    YesOrNo,
    OnlyYes,
    Developer,

    END
}

public class PrefabYesOrNoPopup : MonoBehaviour
{
    #region Inspector
    [Header("Title")]
    public Text Txt_Tile;
    [Header("Contents")]
    public Text Txt_Contents;
    [Header("Button")]
    public CustomButton Btn_Yes;
    public Text Txt_Yes;
    public CustomButton Btn_No;
    public Text Txt_No;
    #endregion

    void Start()
    {
        // 상호작용 후에 팝업은 무조건 닫혀야 하므로
        Btn_Yes.OnClick.AddListener(PopupDestroy);
        Btn_No.OnClick.AddListener(PopupDestroy);

        if(MainController.Instance != null)
        {
            MainController.Instance.m_IsPopup = true;
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (CustomSceneManager.Instance != null)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    PopupDestroy();
                }
            }
        }
    }

    public void Initialize(ePopupState _state, string _title, string _content, UnityAction _YesEvent, UnityAction _NoEvent)
    {
        Txt_Tile.text = _title;
        Txt_Contents.text = _content;
        Btn_Yes.OnClick.AddListener(_YesEvent);
        Btn_No.OnClick.AddListener(_NoEvent);

        switch(_state)
        {
            case ePopupState.YesOrNo:
                Btn_Yes.gameObject.SetActive(true);
                Btn_No.gameObject.SetActive(true);
                break;

            case ePopupState.OnlyYes:
                Btn_Yes.gameObject.SetActive(true);
                Btn_Yes.gameObject.transform.localPosition = new Vector3(0, Btn_Yes.gameObject.transform.localPosition.y, 0);
                Btn_No.gameObject.SetActive(false);
                break;
        }
    }

    public void Initialize(ePopupState _state, string _title, string _content, string _YesText, UnityAction _YesEvent, string _NoText, UnityAction _NoEvent)
    {
        Txt_Tile.text = _title;
        Txt_Contents.text = _content;
        Txt_Yes.text = _YesText;
        Btn_Yes.OnClick.AddListener(_YesEvent);
        // Btn_Yes 위치, 크기 조절
        //Btn_Yes.GetComponent<RectTransform>().sizeDelta = new Vector2(
        //    Txt_Yes.GetComponent<RectTransform>().rect.width, Btn_Yes.GetComponent<RectTransform>().rect.height);
        //Btn_Yes.transform.position = new Vector2(
        //    Btn_Yes.GetComponent<RectTransform>().rect.width / 2, Btn_Yes.transform.position.y);
        Txt_No.text = _NoText;
        Btn_No.OnClick.AddListener(_NoEvent);
        //Btn_No 위치, 크기 조절
        //Btn_No.GetComponent<RectTransform>().sizeDelta = new Vector2(
        //    Txt_No.GetComponent<RectTransform>().rect.width, Btn_No.GetComponent<RectTransform>().rect.height);
        //Btn_No.transform.position = new Vector2(
        //    -Btn_No.GetComponent<RectTransform>().rect.width / 2, Btn_No.transform.position.y);

        switch (_state)
        {
            case ePopupState.YesOrNo:
                Btn_Yes.gameObject.SetActive(true);
                Btn_No.gameObject.SetActive(true);
                break;

            case ePopupState.OnlyYes:
                Btn_Yes.gameObject.SetActive(true);
                Btn_Yes.gameObject.transform.localPosition = new Vector3(0, Btn_Yes.gameObject.transform.localPosition.y, 0);
                Btn_No.gameObject.SetActive(false);
                break;
        }
    }

    public void Initialize(ePopupState _state, string _title, string _content, UnityAction _YesEvent)
    {
        Txt_Tile.text = _title;
        Txt_Contents.text = _content;
        if(_YesEvent != null)
            Btn_Yes.OnClick.AddListener(_YesEvent);

        switch (_state)
        {
            case ePopupState.YesOrNo:
                Btn_Yes.gameObject.SetActive(true);
                Btn_No.gameObject.SetActive(true);
                break;

            case ePopupState.OnlyYes:
                Btn_Yes.gameObject.SetActive(true);
                Btn_Yes.gameObject.transform.localPosition = new Vector3(0, Btn_Yes.gameObject.transform.localPosition.y, 0);
                Btn_No.gameObject.SetActive(false);
                break;
        }
    }

    private void PopupDestroy()
    {
        Destroy(this.gameObject);

        if (MainController.Instance != null)
        {
            MainController.Instance.m_IsPopup = false;
        }
    }
}
