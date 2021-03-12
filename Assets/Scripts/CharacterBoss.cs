using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.Events;
using UnityEngine;

public class CharacterBoss : Character
{
    public MonsterAnimationController m_CharacterAnimation;

    protected StageBossInfo m_BossInfo;

    protected bool m_Appear = true; // 등장 하고 있는가?
    public bool Appear
    {
        get { return m_Appear; }
        set { m_Appear = value; }
    }


    protected int[] skillStack;
    protected int StateStack = -1;

    public virtual void AfterDead() { }

    

    public override void Initialize()
    {
        //base.Initialize();
    }

    public void Initialize(int _ID)
    {
        if (MainController.Instance != null)
        {
            m_BossInfo = MainController.Instance.GetStageBossInfo(_ID);

            m_MaxHP = m_BossInfo.HP;
            m_HP = m_BossInfo.HP;
            m_Attack = m_BossInfo.Attack;

            m_HPBar.Initialize(m_HP, m_MaxHP);

            //SetMonsterSkeletonGraphic(m_BossInfo.AnimName);
        } 
    }

    public void PlayAnimation(eCharacterAnimState _state, bool _loop)
    {
        m_CharacterAnimation.PlayAnimation(_state, _loop);
    }

    public void StopAnimation()
    {
        m_CharacterAnimation.StopAnimation();
    }

    public void SetSkin(string _name)
    {
        m_CharacterAnimation.SetSkin(_name);
    }

    private void SetMonsterSkeletonGraphic(string _name)
    {
        m_CharacterAnimation.SetSekeltonUIAnimation(
            ResourceManager.CreateSkeletonGraphic(_name, gameObject.transform));
    }

    public override void AttackedByEnemies(int _damage, bool _IsCritical = false)
    {
        if (m_IsDead == true || m_SkillShield == true)
            return;

        //base.AttackedByEnemies(_damage, _IsCritical);
        if(m_HP <= _damage)
        {
            m_HP = 0;
            m_IsDead = true;

            PlayAnimation(eCharacterAnimState.Dead, false);

            if(AdventureModeInBossSceneUI.Instance.m_HeroInfo != null)
            {
                AdventureModeInBossSceneUI.Instance.m_HeroInfo.m_State = eHeroState.Win;
            }

            AfterDead();
        }
        else
        {
            m_HP = m_HP - _damage;
        }

        m_HPBar.SetStatePoint(m_HP);

        GameObject obj = ResourceManager.GetOBJCreatePrefab("Prefab_Damage", Position_Damage.transform);
        UIDamage damage = obj.GetComponent<UIDamage>();

        if (_IsCritical == false)
        {
            damage.Initailize(eDamageState.Damage, _damage);
        }
        else
        {
            damage.Initailize(eDamageState.Critical, _damage);
        }
    }

    public override void AttackTheEnemy(Character _character)
    {
        //base.AttackTheEnemy(_character);
        AdventureModeInBossSceneUI.Instance.m_HeroInfo.AttackedByEnemies(m_BossInfo.Attack);
    }

    public IEnumerator Attack()
    {
        m_IsAttack = true;

        PlayAnimation(eCharacterAnimState.Attack, false);

        yield return new WaitForSeconds(0.75f);

        AttackTheEnemy(AdventureModeInBossSceneUI.Instance.m_HeroInfo);

        yield return new WaitForSeconds(0.25f);

        m_IsAttack = false;
    }

    protected void GetReward()
    {
        // 재화 증가 능력치 적용
        int goldUpLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP);
        float goldUp = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.GOLDUP, goldUpLevel).Effect;
        // 반올림
        double GoldUpGold = Math.Round(m_BossInfo.DropGold * goldUp);

        // 유저 정보 수정
        MainController.Instance.UserInfo.ChangeUserGold((int)GoldUpGold);

        eStageKind kind = MainController.Instance.GetStageInfo_StageKind(m_BossInfo.ID);

        MainController.Instance.UserInfo.ChangeStageBossTime(kind);
        MainController.Instance.UserInfo.SaveUser();

        if(CustomSceneManager.Instance != null)
        {
            CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInBoss, eSceneState.Main);
        }
    }
}
