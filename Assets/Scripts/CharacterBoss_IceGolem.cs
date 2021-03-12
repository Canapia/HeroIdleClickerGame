using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eIceGolemState
{
    Standing = 0,
    Attack,
    Shield,
    Smash,
    Dead,
}

public class CharacterBoss_IceGolem : CharacterBoss
{
    private eIceGolemState m_State = eIceGolemState.Standing;

    private void Start()
    {
        skillStack = new int[] { 0, 1, 0, 1, 0, 2 };

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
            m_State = (eIceGolemState)ReturnActionNum();

            yield return new WaitForSeconds(1.0f);

            if (m_IsDead == true)
                break;

            switch (m_State)
            {
                case eIceGolemState.Standing:
                    PlayAnimation(eCharacterAnimState.Standing, true);
                    break;

                case eIceGolemState.Attack:
                    StartCoroutine(Attack());
                    break;

                case eIceGolemState.Shield:
                    StartCoroutine(Skill_Shield());
                    break;

                case eIceGolemState.Smash:
                    StartCoroutine(Skill_Smash());
                    break;

                case eIceGolemState.Dead:
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
            if (MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.FinalStory) == false)
            {
                TutorialStorySystem.Instance.StartSpeach(eStoryState.FinalStory);
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
