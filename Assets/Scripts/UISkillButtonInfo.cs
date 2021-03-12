using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UISkillButtonInfo : MonoBehaviour
{
    #region Inspector
    public eHeroSkillKind m_Kind = eHeroSkillKind.END;
    public CustomButton Obj_Button;
    public Image Img_Filled;
    #endregion

    private bool m_CanUsed = false;
    private bool m_First = true;
    private HeroSkillInfo m_SkillInfo;

    private void Start()
    {
        if(MainController.Instance != null)
        {
            // 스킬 언락 여부에 따른 아이콘 노출
            if (MainController.Instance.UserInfo.GetUserIsUnlockSkill(m_Kind) == true)
            {
                Obj_Button.UseableButton();
            }
            else
            {
                Obj_Button.DisableButton();
            }
        }

        // 첫 쿨은 1초
        StartCoroutine(FirstSkillCoolTime());
    }

    private void Update()
    {
        if(m_CanUsed == false)
        {
            if(m_First == true)
            {
                //Img_Filled.fillAmount = Mathf.Lerp(1.0f, 0, 1.0f / 1.0f * Time.deltaTime);
                Img_Filled.fillAmount -= 1.0f / 5f * Time.deltaTime;
            }
            else
            {
                int skillLevel = MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind);
                //Img_Filled.fillAmount -= 1.0f / MainController.Instance.GetHeroSkillLevel(m_Kind, skillLevel).CoolTime * Time.deltaTime;
                Img_Filled.fillAmount -= 1.0f / 5.0f * Time.deltaTime;
                //Img_Filled.fillAmount = Mathf.Lerp(1.0f, 0, 1.0f / MainController.Instance.GetHeroSkillLevel(m_Kind, skillLevel).CoolTime * Time.deltaTime);
            }
        }
    }

    public void OnClickButton_Skill()
    {
        if (m_CanUsed == false)
            return;

        //m_CanUsed = false;
        if (AdventureModeInBossSceneUI.Instance != null)
        {
            switch(m_Kind)
            {
                case eHeroSkillKind.SMASH:
                    AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_State = eHeroState.Smash;
                    //StartCoroutine(SkillCoolTime());
                    break;

                case eHeroSkillKind.SHIELD:
                    AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_State = eHeroState.Shield;
                    //StartCoroutine(SkillCoolTime());
                    break;

                case eHeroSkillKind.DSPEED:
                    AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_State = eHeroState.DSpeed;
                    //StartCoroutine(SkillCoolTime());
                    break;

                case eHeroSkillKind.CAPTURE:
                    AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_State = eHeroState.Capture;
                    //StartCoroutine(SkillCoolTime());
                    break;
            }

            foreach(UISkillButtonInfo skill in AdventureModeInBossSceneUI.Instance.Btn_Skills)
            {
                skill.PublicSkillCollTime();
            }
        }
    }

    IEnumerator FirstSkillCoolTime()
    {
        //Img_Filled.fillAmount = Mathf.Lerp(1f, 0f, 1f);

        yield return new WaitForSeconds(5f);

        m_CanUsed = true;
        m_First = false;
    }

    public void PublicSkillCollTime()
    {
        StartCoroutine(SkillCoolTime());
    }

    IEnumerator SkillCoolTime()
    {
        m_CanUsed = false;
        AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_UseSkill = true;

        Img_Filled.fillAmount = 1f;

        int skillLevel = MainController.Instance.UserInfo.GetUserSkillLevel(m_Kind);

        //yield return new WaitForSeconds(
        //    MainController.Instance.GetHeroSkillLevel(m_Kind, skillLevel).CoolTime);

        // 쿨 타임 통일
        yield return new WaitForSeconds(5.0f);

        m_CanUsed = true;
        AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_UseSkill = false;
    }
}
