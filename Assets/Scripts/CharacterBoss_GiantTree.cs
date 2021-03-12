using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eGiantTreeState
{
    Standing = 0,
    Attack,
    Shield,
    Smash,
    Dead,
}

public class CharacterBoss_GiantTree : CharacterBoss
{
    private eGiantTreeState m_State = eGiantTreeState.Standing;

    // Start is called before the first frame update
    void Start()
    {
        skillStack = new int[] { 0, 1, 0, 1, 0, 2, 0, 1, 0, 1, 0, 3 };

        StartCoroutine(Action());
    }

    IEnumerator Skill_Shield()
    {
        m_SkillShield = true;

        PlayAnimation(eCharacterAnimState.Shield, false);

        yield return new WaitForSeconds(1.0f);

        m_SkillShield = false;
    }

    IEnumerator Skill_Smash()
    {
        float saveAttack = m_Attack;

        m_Attack = m_Attack * 2;

        PlayAnimation(eCharacterAnimState.Smash, false);

        yield return new WaitForSeconds(1.0f);

        m_Attack = saveAttack;
    }


    IEnumerator Action()
    {
        yield return new WaitUntil(() => AdventureModeInBossSceneUI.Instance.StageState == eBossStageState.Battle);

        while (!m_IsDead)
        {
            m_State = (eGiantTreeState)ReturnActionNum();

            yield return new WaitForSeconds(1.0f);

            if (m_IsDead == true)
                break;

            switch (m_State)
            {
                case eGiantTreeState.Standing:
                    PlayAnimation(eCharacterAnimState.Standing, true);
                    break;

                case eGiantTreeState.Attack:
                    StartCoroutine(Attack());
                    break;

                case eGiantTreeState.Shield:
                    StartCoroutine(Skill_Shield());
                    break;

                case eGiantTreeState.Smash:
                    StartCoroutine(Skill_Smash());
                    break;

                case eGiantTreeState.Dead:
                    PlayAnimation(eCharacterAnimState.Dead, true);
                    break;
            }
        }
    }

    private int ReturnActionNum()
    {
        StateStack++;

        if (skillStack.Length <= StateStack)
        {
            StateStack = 0;
        }

        return skillStack[StateStack];
    }

    public override void AfterDead()
    {
        base.AfterDead();

        // 재화 증가 능력치 적용
        int goldUpLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP);
        float goldUp = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.GOLDUP, goldUpLevel).Effect;
        // 반올림
        double GoldUpGold = Math.Round(m_BossInfo.DropGold * goldUp);

        if (MainController.Instance != null)
        {
            if (MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.Stage2EndStory) == false)
            {
                TutorialStorySystem.Instance.StartSpeach(eStoryState.Stage2EndStory);
            }
            else
            {
                if (GoodsSceneInstance.Instance != null)
                {
                    GoodsSceneInstance.Instance.ClearBossPopup(ePopupState.OnlyYes, "완 료!", string.Format("{0} Gold\n획득!", GoldUpGold),
                        GetReward);
                }
            }
        }
    }
}
