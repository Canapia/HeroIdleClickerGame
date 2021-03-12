using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Spine.Unity;

public enum eCharacterAnimState
{
    // 용사 / 몬스터 / 보스 공용
    Standing = 0,
    Run,
    Attack,
    Dead,

    // 용사 / 보스
    Smash,      // 강타
    Shield,     // 보호막
    Capture,    // 포획
    DSpeed,     // 2배속
    Heal,       // 회복 - 이상한 슬라임 전용

    // 보스
    Appear,     // 등장

    END
}

public class SpineAnimationManager : MonoBehaviour
{
    protected SkeletonGraphic m_UIAnimation;

    private void Awake()
    {
        m_UIAnimation = GetComponent<SkeletonGraphic>();
    }

    /// <summary>
    /// 애니메이션 재생
    /// </summary>
    /// <param name="_state">재생할 애니메이션 이름</param>
    /// <param name="_loop">반복 재생 여부</param>
    /// <param name="_timeScale">재생 속도</param>
    public void PlayAnimation(eCharacterAnimState _state, bool _loop, float _timeScale = 1.0f)
    {
        m_UIAnimation.AnimationState.SetAnimation(0, _state.ToString(), _loop).TimeScale = _timeScale;
    }

    /// <summary>
    /// 여러개의 애니메이션을 순차적으로 반복 재생
    /// </summary>
    /// <param name="_state">재생할 애니메이션 이름을 순차적으로 기재</param>
    /// <param name="_timeScale">재생 속도</param>
    public void LoopMultipleAnimations(eCharacterAnimState[] _state, float _timeScale = 1.0f)
    {
        m_UIAnimation.AnimationState.ClearTracks();

        for (int i = 0; i < _state.Length; i++)
        {
            m_UIAnimation.AnimationState.AddAnimation(0, _state[i].ToString(), true, 0f).TimeScale = _timeScale;
        }
    }

    /// <summary>
    /// 애니메이션 정지
    /// </summary>
    public void StopAnimation()
    {
        m_UIAnimation.AnimationState.GetCurrent(0).TimeScale = 0;
    }

    /// <summary>
    /// 애니메이션 크기
    /// </summary>
    /// <returns></returns>
    public Vector2 GetAnimationSize()
    {
        if (m_UIAnimation == null)
            return Vector2.zero;

        float x, y;
        float width, height;
        float[] vertexbeffer = new float[255];

        m_UIAnimation.Skeleton.GetBounds(out x, out y, out width, out height, ref vertexbeffer);

        Vector2 size = new Vector2(width, height);

        return size;
    }
}
