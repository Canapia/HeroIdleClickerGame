using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum eSceneState
{
    Title = 0,
    Main,
    AdventureInMap,
    AdventureInHunt,
    AdventureInBoss,

    END
}

// 인스턴스 제거 하도록 변경 할 것!!!!!

public class CustomSceneManager : MonoBehaviour
{
    // Fade
    public eSceneState m_Scenestate = eSceneState.Title;
    public float m_FadeTime = 1f;
    public Image Img_FadeBlack;
    public Image Img_FadeBlack_Adventure;
    private float m_Start;
    private float m_End;
    private float m_Time = 0f;
    public bool m_IsPlaying = false;
    public bool m_SceneChanging = false;

    private static CustomSceneManager m_Instance;
    public static CustomSceneManager Instance
    {
        get 
        {
            return m_Instance; 
        }
        set { m_Instance = value; }
    }

    void Awake()
    {
        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);

        if (m_Instance == null)
        {
            m_Instance = GetComponent<CustomSceneManager>();
        }
        else
        {
            Destroy(m_Instance);
        }
    }

    void Start()
    {
        //Img_FadeBlack.gameObject.SetActive(false);
        //Img_FadeBlack_Adventure.gameObject.SetActive(false);
    }

    public void OutStartFadeAnim(Image _img)
    {
        // 중복 재생 방지
        if(m_IsPlaying == true)
        {
            return;
        }

        m_End = 0f;

        StartCoroutine(FadeOutPlay(_img));
    }

    public void InStartFadeAnim(Image _img)
    {
        if(m_IsPlaying == true)
        {
            return;
        }

        StartCoroutine(FadeInPlay(_img));
    }

    IEnumerator FadeOutPlay(Image _img)
    {
        m_IsPlaying = true;
        _img.gameObject.SetActive(true);

        Color fadeColor = Img_FadeBlack.color;
        m_Time = 0f;
        m_Start = 1f;
        m_End = 0f;
        fadeColor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while(fadeColor.a > 0f)
        {
            yield return null;
            m_Time += Time.deltaTime / m_FadeTime;
            fadeColor.a = Mathf.Lerp(m_Start, m_End, m_Time);
            _img.color = fadeColor;
        }

        m_IsPlaying = false;
        _img.gameObject.SetActive(false);
    }

    IEnumerator FadeInPlay(Image _img)
    {
        m_IsPlaying = true;
        _img.gameObject.SetActive(true);

        Color fadeColor = _img.color;
        m_Time = 0f;
        m_Start = 0f;
        m_End = 1f;
        fadeColor.a = Mathf.Lerp(m_Start, m_End, m_Time);

        while(fadeColor.a < 1f)
        {
            yield return null;
            m_Time += Time.deltaTime / m_FadeTime;
            fadeColor.a = Mathf.Lerp(m_Start, m_End, m_Time);
            _img.color = fadeColor;
        }

        m_IsPlaying = false;
        //_img.gameObject.SetActive(false);
    }

    public void ChangeScene(eSceneState _before, eSceneState _after)
    {

        StartCoroutine(WaitChangeScene(_before, _after));
        
    }

    public void ChangeScene_Prepare()
    {
        SceneManager.LoadScene("TitleScene");
    }

    IEnumerator WaitChangeScene(eSceneState _before, eSceneState _after)
    {
        m_SceneChanging = true;

        if (_before == eSceneState.AdventureInMap && _after == eSceneState.AdventureInHunt)
        {
            InStartFadeAnim(Img_FadeBlack_Adventure);
        }
        else
        {
            InStartFadeAnim(Img_FadeBlack);
        }
        

        yield return new WaitForSeconds(2f);

        if (_before == eSceneState.Title && _after == eSceneState.Main)
        {
            SceneManager.UnloadSceneAsync("TitleScene");

            SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
            SceneManager.LoadScene("GoodsScene", LoadSceneMode.Additive);

            // 튜토리얼 체크
            if(TutorialStorySystem.Instance != null && MainController.Instance != null)
            {
                if(MainController.Instance.UserInfo.GetTutorialStoryClear(eStoryState.FirstStart) == false)
                {
                    TutorialStorySystem.Instance.StartSpeach(eStoryState.FirstStart);
                }
            }
        }

        if (_before == eSceneState.Main)
        {
            if (_after == eSceneState.AdventureInMap)
            {
                SceneManager.UnloadSceneAsync("MainScene");

                SceneManager.LoadScene("AdventureScene", LoadSceneMode.Additive);
                SceneManager.LoadScene("AdventureModeInMapScene", LoadSceneMode.Additive);

            }
            else if (_after == eSceneState.Title)
            {
                // 임시로 재시작 하는 것 처럼 보임
                //SceneManager.UnloadSceneAsync("GoodsScene");
                //SceneManager.UnloadSceneAsync("MainScene");

                SceneManager.LoadScene("TitleScene");
            }
        }

        if(_before == eSceneState.AdventureInMap)
        {
            if (_after == eSceneState.AdventureInHunt)
            {
                SceneManager.UnloadSceneAsync("AdventureModeInMapScene");

                SceneManager.LoadScene("AdventureModeInHuntScene", LoadSceneMode.Additive);
            }
            else if(_after == eSceneState.AdventureInBoss)
            {
                SceneManager.UnloadSceneAsync("AdventureModeInMapScene");
                SceneManager.UnloadSceneAsync("AdventureScene");

                SceneManager.LoadScene("AdventureModeInBossScene", LoadSceneMode.Additive);
            }
            else if(_after == eSceneState.Main)
            {
                SceneManager.UnloadSceneAsync("AdventureModeInMapScene");
                SceneManager.UnloadSceneAsync("AdventureScene");

                SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
            }
        }

        if(_before == eSceneState.AdventureInHunt && _after == eSceneState.Main)
        {
            SceneManager.UnloadSceneAsync("AdventureScene");
            SceneManager.UnloadSceneAsync("AdventureModeInHuntScene");

            SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        }

        if (_before == eSceneState.AdventureInBoss && _after == eSceneState.Main)
        {
            SceneManager.UnloadSceneAsync("AdventureModeInBossScene");

            SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
        }

        yield return new WaitForSeconds(1f);

        m_Scenestate = _after;

        if (GoodsSceneInstance.Instance != null)
        {
            GoodsSceneInstance.Instance.ChangeButtonState();
        }

        
        if (_before == eSceneState.AdventureInMap && _after == eSceneState.AdventureInHunt)
        {
            OutStartFadeAnim(Img_FadeBlack_Adventure);
        }
        else
        {
            OutStartFadeAnim(Img_FadeBlack);
        }
        yield return new WaitForSeconds(1f);

        m_SceneChanging = false;
    }
}
