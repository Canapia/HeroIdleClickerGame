using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum eButtonTrigger
{
    Normal,
    Highlighted,
    Pressed,
}

public class CustomButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    #region Inspector =================
    private Image Img_Button;
    public Sprite Spr_Normal;
    public Sprite Spr_Disabled;
    //public Animation Anim_Button;
    public UnityEvent OnClick;
    #endregion ========================

    [SerializeField]
    private bool m_Use = true;

    private Animator m_BtnAnim;
    private bool m_IsClick = true;

    private void Awake()
    {
        Img_Button = GetComponent<Image>();
        m_BtnAnim = GetComponent<Animator>();
    }

    void Start()
    {
        if (m_Use)
        {
            if (Spr_Normal != null)
                Img_Button.sprite = Spr_Normal;
        }
        else
        {
            if(Spr_Disabled != null)
                Img_Button.sprite = Spr_Disabled;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_IsClick == false)
            return;

        m_BtnAnim.SetTrigger(eButtonTrigger.Highlighted.ToString());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (m_IsClick == false)
            return;

        OnClick.Invoke();

        //throw new System.NotImplementedException();
        if (m_Use)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundEffect();
            }
        }

        m_BtnAnim.SetTrigger(eButtonTrigger.Normal.ToString());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_IsClick == false)
            return;

        //throw new System.NotImplementedException();
        m_BtnAnim.SetTrigger(eButtonTrigger.Normal.ToString());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (m_IsClick == false)
            return;

        //throw new System.NotImplementedException();
        m_BtnAnim.SetTrigger(eButtonTrigger.Pressed.ToString());
    }

    public void DisableButton()
    {
        // StartCoroutine(WaitDisableButton());

        if (Spr_Disabled != null)
        {
            m_IsClick = false;
            m_Use = false;
            Img_Button.sprite = Spr_Disabled;
        }
    }

    // 이미지는 Normal 이나 기능은 사용하지 않음.
    public void FineButNotUsedButton()
    {
        if(Spr_Normal != null)
        {
            m_IsClick = false;
            m_Use = false;
            if(Img_Button != null)
                Img_Button.sprite = Spr_Normal;
        }
    }

    private IEnumerator WaitDisableButton()
    {
        yield return new WaitUntil(()=>Img_Button != null);

        if (Spr_Disabled != null)
        {
            m_IsClick = false;
            Img_Button.sprite = Spr_Disabled;
        }
    }

    public void UseableButton()
    {
        if(Spr_Normal != null)
        {
            m_IsClick = true;
            m_Use = true;
            if(Img_Button != null)
                Img_Button.sprite = Spr_Normal;
        }
    }
}
