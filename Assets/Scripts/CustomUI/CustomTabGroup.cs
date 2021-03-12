using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CustomTabGroup : MonoBehaviour
{
    public List<CustomTabButton> m_TabButtons;
    public Sprite Spr_TabIdle;
    public Sprite Spr_TabHober;
    public Sprite Spr_TabActive;
    public Sprite Spr_Disable;
    public CustomTabButton m_SelectedTab;
    public List<CustomContentList> Obj_ToSwap;

    void Start()
    {
        for (int i = 0; i < Obj_ToSwap.Count; i++)
        {
            Obj_ToSwap[i].RenewalUI();
            if (Obj_ToSwap[i].m_ContentList.Count == 0)
            {
                SetUseable(i, false);
            }
        }

        OnTabSelected(m_TabButtons[0]);
    }

    public void Subscribe(CustomTabButton _Button)
    {
        if (m_TabButtons == null)
        {
            m_TabButtons = new List<CustomTabButton>();
        }

        m_TabButtons.Add(_Button);
    }

    public void OnTabEnter(CustomTabButton _Button)
    {
        ResetTabs();
        if (m_SelectedTab == null || _Button != m_SelectedTab)
        {
            _Button.Img_BG.sprite = Spr_TabHober;
        }
    }

    public void OnTabExit(CustomTabButton _Button)
    {
        ResetTabs();
    }

    public void OnTabSelected(CustomTabButton _Button)
    {
        if(m_SelectedTab != null)
        {
            m_SelectedTab.DeSelect();
            m_SelectedTab.Img_BG.sprite = Spr_TabIdle;
        }

        m_SelectedTab = _Button;

        m_SelectedTab.Select();

        ResetTabs();
        _Button.Img_BG.sprite = Spr_TabActive;
        int index = _Button.transform.GetSiblingIndex();
        for(int i = 0;i<Obj_ToSwap.Count;i++)
        {
            if(i==index)
            {
                Obj_ToSwap[i].gameObject.SetActive(true);
            }
            else
            {
                Obj_ToSwap[i].gameObject.SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(CustomTabButton button in m_TabButtons)
        {
            if(m_SelectedTab != null && button == m_SelectedTab)
            {
                return;
            }
            button.Img_BG.sprite = Spr_TabIdle;
        }
    }
    
    public void SetUseable(int _num, bool _IsUse)
    {
        for (int i = 0; i < m_TabButtons.Count; i++)
        {
            if(i == _num)
            {
                m_TabButtons[i].SetUseable(_IsUse);
                if (_IsUse == false)
                    m_TabButtons[i].Img_BG.sprite = Spr_Disable;
                else
                    m_TabButtons[i].Img_BG.sprite = Spr_TabIdle;
            }
        }
    }
}
