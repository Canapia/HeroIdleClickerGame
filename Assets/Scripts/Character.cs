using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Character : MonoBehaviour
{
    #region Inspector
    //public SpineAnimationManager m_CharacterAnimation;
    public UIHPBar m_HPBar;
    public Rigidbody2D m_RB2D;
    public BoxCollider2D m_Coll2D;
    public GameObject Position_Damage;
    #endregion

    // 기본 스펙 사항
    protected int m_HP;
    protected int m_MaxHP;
    protected float m_Attack;

    // 애니메이션 관련 사항
    // 공격하고 있는가?
    protected bool m_IsAttack = false;
    public bool IsAttack
    {
        get { return m_IsAttack; }
        set { m_IsAttack = value; }
    }
    // 움직이고 있는가?
    protected bool m_IsMove = false;
    public bool IsMove
    {
        get { return m_IsMove; }
        set { m_IsMove = value; }
    }
    // 죽었는가?
    protected bool m_IsDead = false;
    public bool IsDead
    {
        get { return m_IsDead; }
        set { m_IsDead = value; }
    }
    protected bool m_IsPause = false;
    public bool IsPause
    {
        get { return m_IsPause; }
        set { m_IsPause = value; }
    }

    /// <summary>
    /// 캐릭터 초기화
    /// </summary>
    public virtual void Initialize() { }
    /// <summary>
    /// 적에게 공격 받음
    /// </summary>
    /// <param name="_damage">받은 데미지</param>
    public virtual void AttackedByEnemies(int _damage, bool _IsCritical = false) { }
    /// <summary>
    /// 적에게 공격
    /// </summary>
    public virtual void AttackTheEnemy(Character _character) { }



    // 스킬 관련
    protected bool m_SkillShield = false;
}
