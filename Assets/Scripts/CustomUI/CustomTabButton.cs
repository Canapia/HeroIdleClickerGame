using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
public class CustomTabButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public CustomTabGroup m_TabGroup;
    public Image Img_BG;

    public UnityEvent onTabSelected;
    public UnityEvent onTabDeSelected;

    public bool m_IsUseable = true;

    // Start is called before the first frame update
    void Start()
    {
        Img_BG = GetComponent<Image>();
        m_TabGroup.Subscribe(this);
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    if (m_IsUseable == false)
    //        return;

    //    //throw new System.NotImplementedException();
    //    m_TabGroup.OnTabSelected(this);
    //}

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_IsUseable == false)
            return;

        //throw new System.NotImplementedException();
        m_TabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (m_IsUseable == false)
            return;

        //throw new System.NotImplementedException();
        m_TabGroup.OnTabExit(this);
    }

    public void Select()
    {
        if(onTabSelected != null)
        {
            onTabSelected.Invoke();

            if(SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundEffect();
            }
        }
    }

    public void DeSelect()
    {
        if(onTabDeSelected != null)
        {
            onTabDeSelected.Invoke();
        }
    }

    public void SetUseable(bool _IsUse)
    {
        if(_IsUse)
        {
            m_IsUseable = true;
        }
        else
        {
            m_IsUseable = false;
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        if (m_IsUseable == false)
            return;

        m_TabGroup.OnTabSelected(this);
    }
}
