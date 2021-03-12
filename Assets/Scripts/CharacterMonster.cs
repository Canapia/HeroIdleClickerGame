using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMonster : Character
{
    public MonsterAnimationController m_CharacterAnimation;

    private StageMonsterInfo m_MonsterInfo;

    public override void Initialize()
    {
        base.Initialize();

        if (MainController.Instance != null)
        {
            SetHP();
            SetMaxHP();

            SetMonsterSkeletonGraphic(m_MonsterInfo.AnimName);
            SetSkin(m_MonsterInfo.SkinName);
            ReSizeBoxCollider();

            m_IsMove = true;

            m_HPBar.Initialize(m_HP, m_MaxHP);
        }
    }

    public void Initialize(int _ID)
    {
        if (MainController.Instance != null)
        {
            m_MonsterInfo = MainController.Instance.GetStageMonsterInfo(_ID);

            SetHP();
            SetMaxHP();

            // 애니메이션 관련
            SetMonsterSkeletonGraphic(m_MonsterInfo.AnimName);
            SetSkin(m_MonsterInfo.SkinName);
            ReSizeBoxCollider();

            m_IsMove = true;

            m_HPBar.Initialize(m_HP, m_MaxHP);
        }
    }

    public override void AttackedByEnemies(int _damage, bool _IsCritical = false)
    {
        if (m_IsDead == false)
        {
            if(m_HP <= _damage)
            {
                m_HP = 0;
                StopAnimation();
                StartMoveMonster();
                m_IsDead = true;
                m_IsMove = true;

                m_CharacterAnimation.PlayAnimation(eCharacterAnimState.Dead, false);

                // 재화 증가 능력치 적용
                int goldUpLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.GOLDUP);
                float goldUp = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.GOLDUP, goldUpLevel).Effect;
                // 반올림
                double GoldUpGold = Math.Round(m_MonsterInfo.DropGold * goldUp);

                // 유저 정보 수정
                MainController.Instance.UserInfo.ChangeUserGold((int)GoldUpGold);
                MainController.Instance.UserInfo.SaveUser();

                GameObject obj1 = ResourceManager.GetOBJCreatePrefab("Prefab_Damage", Position_Damage.transform);
                UIDamage gold = obj1.GetComponent<UIDamage>();

                gold.Initailize(eDamageState.Gold, (int)GoldUpGold);

                if(AdventureSceneManager.Instance != null)
                {
                    AdventureSceneManager.Instance.RenewalTabUI();
                }
            }
            else if(m_HP > _damage)
            {
                m_HP = m_HP - _damage;
            }
            m_HPBar.SetStatePoint(m_HP);

            GameObject obj = ResourceManager.GetOBJCreatePrefab("Prefab_Damage", Position_Damage.transform);
            UIDamage damage = obj.GetComponent<UIDamage>();

            if(_IsCritical == false)
            {
                damage.Initailize(eDamageState.Damage, _damage);
            }
            else
            {
                damage.Initailize(eDamageState.Critical, _damage);
            }
        }
    }

    public override void AttackTheEnemy(Character _character)
    {
        // 데미지 만큼 공격
        AdventureModeInHuntSceneUI.Instance.m_HeroInfo.AttackedByEnemies(m_MonsterInfo.Attack);

        Debug.Log(string.Format("몬스터가 영웅에게 {0} 만큼의 데미지!", m_MonsterInfo.Attack));
    }

    IEnumerator Attack(Character _character)
    {
        m_IsAttack = true;

        PlayAnimation(eCharacterAnimState.Attack, false);

        yield return new WaitForSeconds(0.75f);

        AttackTheEnemy(_character);

        yield return new WaitForSeconds(0.25f);

        m_IsAttack = false;
    }

    public void SetHP()
    {
        m_HP = m_MonsterInfo.HP;
    }

    public void SetMaxHP()
    {
        m_MaxHP = m_MonsterInfo.HP;
    }

    #region Animation
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

    public void SetMonsterSkeletonGraphic(string _name)
    {
        m_CharacterAnimation.SetSekeltonUIAnimation(
            ResourceManager.CreateSkeletonGraphic(_name, gameObject.transform));
    }

    private void ReSizeBoxCollider()
    {
        Vector2 size = m_CharacterAnimation.GetAnimationSize();
        Vector2 ReSize = new Vector2(size.x * 100, size.y * 100);

        m_Coll2D.size = ReSize;
        m_Coll2D.offset = new Vector2(0, ReSize.y / 2);
    }
    #endregion

    public void StartMoveMonster()
    {
        int speedLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED);

        m_RB2D.velocity = new Vector2(
            -(MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, speedLevel).Effect) * 300f,
            0);
    }

    public void StopMoveMonster()
    {
        m_RB2D.velocity = Vector2.zero;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Hero")
        {
            m_IsMove = false;
            StopMoveMonster();
        }

        if(collision.tag == "DeadZone")
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (m_IsPause == true)
            return;

        CharacterHero hero = collision.GetComponent<CharacterHero>();

        if (collision.tag == "Hero")
        {
            if(m_IsAttack == false && m_IsMove == false && m_IsDead == false && m_IsPause == false &&
                hero.IsDead == false)
            {
                StartCoroutine(Attack(hero));
            }
            else if(m_IsAttack == false && m_IsMove == true && m_IsDead == true && m_IsPause == false &&
                hero.IsDead == false)
            {
                StartMoveMonster();
            }
        }
    }
}
