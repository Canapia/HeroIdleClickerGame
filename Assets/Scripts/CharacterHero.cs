using System;
using System.Collections;

using UnityEngine;

public enum eHeroState
{
    Standing = 0,
    Attack,

    // Skill
    Smash,
    Shield,
    Capture,
    DSpeed,

    Dead,
    Win,

    END
}

public class CharacterHero : Character
{
    public HeroAnimationController m_CharacterAnimation;

    public eHeroState m_State = eHeroState.Standing;

    public bool m_UseShield = false;

    public float m_SpeedValue = 0f;

    public bool m_UseSkill = false;

    private bool m_HealReady = false;

    public override void Initialize()
    {
        base.Initialize();

        if(MainController.Instance != null)
        {
            SetHP();
            SetMaxHP();

            m_IsMove = true;

            int speedLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED);
            m_SpeedValue = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, speedLevel).Effect;

            m_HPBar.Initialize(m_HP, m_MaxHP);

            StartCoroutine(AutoHeal());
        }
    }

    public override void AttackedByEnemies(int _damage, bool _isCritical = false)
    {
        if (m_UseShield == true)
            return;

        // 방어력에 따른 데미지 감소 계산
        int defLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.DEF);
        float def = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.DEF, defLevel).Effect;
        // 소수점 이하 버림
        double defDamage = Math.Truncate(_damage - (_damage * (def / 100f)));

        if (m_IsDead == false)
        {
            if (m_HP <= (int)defDamage)
            {
                m_HP = 0;
                m_IsDead = true;

                PlayAnimation(eCharacterAnimState.Dead, false);

                if(SoundManager.Instance != null)
                {
                    SoundManager.Instance.OneShotPlayBGM(eBGMState.GameOver);
                }

                if (AdventureModeInHuntSceneUI.Instance != null)
                {
                    AdventureModeInHuntSceneUI.Instance.PauseInGamePlay();
                    CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInHunt, eSceneState.Main);
                }
                else if(AdventureModeInBossSceneUI.Instance != null)
                {
                    CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInBoss, eSceneState.Main);
                }
            }
            else if(m_HP > defDamage)
            {
                m_HP = m_HP - (int)defDamage;
            }
            m_HPBar.SetStatePoint(m_HP);

            GameObject obj = ResourceManager.GetOBJCreatePrefab("Prefab_Damage", Position_Damage.transform);
            UIDamage damage = obj.GetComponent<UIDamage>();

            damage.Initailize(eDamageState.Damage, (int)defDamage);
        }
    }

    public override void AttackTheEnemy(Character _character)
    {
        if (IsPause == true)
            return;

        

        int attackLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.ATTACK);

        // 크리티컬 확률 적용
        // 더 찾아보길 바람
        int criticalLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.CRITICAL);
        float critical = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.CRITICAL, criticalLevel).Effect;

        int CriticalDamage = (int)MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.ATTACK, attackLevel).Effect;

        if (UnityEngine.Random.Range(0f, 100f) < critical)
        {
            CriticalDamage = CriticalDamage * 2;

            _character.GetComponent<Character>().AttackedByEnemies(CriticalDamage, true);
        }
        else
        {
            _character.GetComponent<Character>().AttackedByEnemies(CriticalDamage);
        }

        Debug.Log(string.Format("용사가 상대에게 데미지 {0} 추가!", CriticalDamage));
    }

    public IEnumerator Attack(Character _character)
    {
        m_IsAttack = true;

        int speedLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED);
        m_SpeedValue = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, speedLevel).Effect;

        PlayAnimation(eCharacterAnimState.Attack, false,
            MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, speedLevel).Effect);

        yield return new WaitForSeconds((m_SpeedValue * 59) / 60);

        if (m_UseSkill == false)
        {
            AttackTheEnemy(_character);

            yield return new WaitForSeconds(m_SpeedValue / 60);
        }

        m_IsAttack = false;
    }

    public void SetHP()
    {
        int hpLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.HP);
        m_HP = (int)MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.HP, hpLevel).Effect;
    }

    public void SetMaxHP()
    {
        int hpLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.HP);
        m_MaxHP = (int)MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.HP, hpLevel).Effect;
    }

    public void SetUpgradeUIHPBar()
    {
        SetMaxHP();
        m_HPBar.SetMaxPoint(m_MaxHP);
        // 이 전 레벨과의 생명력 격차만큼 현재 생명력을 올려준다.
        int hpLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.HP);
        float preHP = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.HP, hpLevel - 1).Effect;
        float nowHP = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.HP, hpLevel).Effect;
        m_HP = m_HP + (int)(nowHP - preHP);
        m_HPBar.SetStatePoint(m_HP);
    }

    #region Animation
    public void PlayAnimation(eCharacterAnimState _state, bool _loop, float _timeScale = 1f)
    {
        //int speedLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED);

        //m_CharacterAnimation.PlayAnimation(_state, _loop,
        //    MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, speedLevel).Effect);
        m_CharacterAnimation.PlayAnimation(_state, _loop, _timeScale);
    }

    public void StopAnimation()
    {
        m_CharacterAnimation.StopAnimation();
    }

    public void SetSkin(int _armorLevel, int _swordLevel)
    {
        m_CharacterAnimation.SetSkinCombine(_armorLevel, _swordLevel);
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            m_IsMove = false;
            PlayAnimation(eCharacterAnimState.Standing, true);

            if(AdventureModeInHuntSceneUI.Instance != null)
            {
                AdventureModeInHuntSceneUI.Instance.StopBackground();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (m_IsPause == true)
            return;

        CharacterMonster monster = collision.GetComponent<CharacterMonster>();

        if (collision.tag == "Monster")
        {
            if (m_IsAttack == false && m_IsMove == false && m_IsDead == false && m_IsPause == false &&
                monster.IsDead == false)
            {
                StartCoroutine(Attack(monster));
            }
            else if(m_IsAttack == false && m_IsMove == false && m_IsDead == false && m_IsPause == false &&
                monster.IsDead == true)
            {
                int speedLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED);
                m_SpeedValue = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, speedLevel).Effect;

                m_IsMove = true;
                PlayAnimation(eCharacterAnimState.Run, true, m_SpeedValue);

                if(AdventureModeInHuntSceneUI.Instance != null)
                {
                    AdventureModeInHuntSceneUI.Instance.StartBackground();
                }
            }
        }
    }

    private IEnumerator AutoHeal()
    {
        while (!m_IsDead)
        {
            if (AdventureModeInHuntSceneUI.Instance != null)
            {
                if (AdventureModeInHuntSceneUI.Instance.HuntState == eHuntSceneState.CountDown ||
               AdventureModeInHuntSceneUI.Instance.HuntState == eHuntSceneState.Pause ||
               AdventureModeInHuntSceneUI.Instance.HuntState == eHuntSceneState.Ready ||
               AdventureModeInHuntSceneUI.Instance.HuntState == eHuntSceneState.ReStart ||
               m_HealReady == true)
                {
                    yield return null;
                }
                else
                {
                    m_HealReady = true;
                    yield return new WaitForSeconds(7.0f);

                    if (m_IsDead == false && AdventureModeInHuntSceneUI.Instance.HuntState != eHuntSceneState.Pause)
                    {
                        int healLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.POTION);
                        float HealValue = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.POTION, healLevel).Effect;

                        GameObject obj = ResourceManager.GetOBJCreatePrefab("Prefab_Damage", Position_Damage.transform);
                        UIDamage damage = obj.GetComponent<UIDamage>();

                        damage.Initailize(eDamageState.Heal, (int)HealValue);

                        m_HP = m_HP + (int)HealValue;
                        if (m_HP >= m_MaxHP)
                        {
                            m_HP = m_MaxHP;
                        }
                        m_HPBar.SetStatePoint(m_HP);
                        m_HealReady = false;
                    }
                }

            }

            if (AdventureModeInBossSceneUI.Instance != null)
            {
                if (AdventureModeInBossSceneUI.Instance.StageState == eBossStageState.CountDown ||
               AdventureModeInBossSceneUI.Instance.StageState == eBossStageState.Pause ||
               AdventureModeInBossSceneUI.Instance.StageState == eBossStageState.Ready ||
               m_HealReady == true)
                {
                    yield return null;
                }
                else
                {
                    m_HealReady = true;
                    yield return new WaitForSeconds(7.0f);

                    if (m_IsDead == false)
                    {

                        int healLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.POTION);
                        float HealValue = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.POTION, healLevel).Effect;

                        GameObject obj = ResourceManager.GetOBJCreatePrefab("Prefab_Damage", Position_Damage.transform);
                        UIDamage damage = obj.GetComponent<UIDamage>();

                        damage.Initailize(eDamageState.Heal, (int)HealValue);

                        m_HP = m_HP + (int)HealValue;
                        if (m_HP >= m_MaxHP)
                        {
                            m_HP = m_MaxHP;
                        }
                        m_HPBar.SetStatePoint(m_HP);
                        m_HealReady = false;
                    }
                }
            }
        }
    }
}
