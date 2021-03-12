using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eBackScrollKind
{
    Rear,       // 맨뒤
    Middle,     // 중간
    Front,      // 맨앞

    END
}

public class ScrollingBackground : MonoBehaviour
{
    public eBackScrollKind m_Kind;

    private Rigidbody2D RG2D;

    // 임시 추후 수정!!!
    private float scrollSpeed = -1.5f;

    private void Awake()
    {
        RG2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(MainController.Instance != null)
        {
            int SpeedLevel = MainController.Instance.UserInfo.GetUserAbilityLevel(eHeroAbilityKind.SPEED);
            float Speed = MainController.Instance.GetHeroAbilityLevel(eHeroAbilityKind.SPEED, SpeedLevel).Effect;
            switch (m_Kind)
            {
                case eBackScrollKind.Rear:
                    scrollSpeed = Speed;
                    break;
                case eBackScrollKind.Middle:
                    scrollSpeed = Speed * 1.5f;
                    break;
                case eBackScrollKind.Front:
                    scrollSpeed = Speed * 3f;
                    break;
            }
            
        }

        startScrolling();
    }

    public void startScrolling()
    {
        RG2D.velocity = new Vector2(-(scrollSpeed * 100f), 0);
    }

    public void StopScrolling()
    {
        StartCoroutine(WaitNullForUI());
    }

    // SceneManager에서 부르기 전에 UI가 그려지지 않아서 처리
    IEnumerator WaitNullForUI()
    {
        yield return new WaitUntil(() => RG2D != null);

        RG2D.velocity = Vector2.zero;
    }
}
