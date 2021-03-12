using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour, IPointerDownHandler
{
    [Header("- Background 관련")]
    public Image Img_BG;
    public Sprite txt_Default;
    public Sprite txt_Special;

    [Header("- Info 관련")]
    public Image Img_ProgressBar;
    public Text Txt_Info;

    [Header("- Controller 관련")]
    public MainController m_MainCtr;
    public SoundManager m_SoundCtr;
    public CustomSceneManager m_SceneCtr;

    private void Awake()
    {
        // 중요 컨트롤러 박제
        //DontDestroyOnLoad(m_MainCtr);
        //DontDestroyOnLoad(m_SoundCtr);
        //DontDestroyOnLoad(m_SceneCtr);

        //SetupCamera();

        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        Debug.Log("씬 구성 중 ...");
        var thisScene = SceneManager.GetActiveScene();

        for(int i =0;i<SceneManager.sceneCountInBuildSettings;i++)
        {
            if (thisScene.buildIndex == i) continue;

            // 이미 씬이 로딩되어 있다면 스킵
            if (SceneManager.GetSceneByBuildIndex(i).IsValid()) continue;

            if(i == 1)
            {

            }
        }
        */

        SceneManager.LoadSceneAsync("ImportantInstance", LoadSceneMode.Additive);

        Img_BG.sprite = txt_Default;
        Txt_Info.enabled = false;

        StartCoroutine(LoadData());

        StartCoroutine(StartBGM());
    }

    private void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                // 한번 더 누르면 종료됩니다 로그 출력
                Application.Quit();
            }
        }
    }

    IEnumerator StartBGM()
    {
        yield return new WaitUntil(()=>SoundManager.Instance != null);

        SoundManager.Instance.PlayBGM(eBGMState.Title);
    }

    IEnumerator LoadData()
    {
        float timer = 0.0f;
        Img_ProgressBar.fillAmount = 0f;
        while (Img_ProgressBar.fillAmount < 1f)
        {
            timer += Time.deltaTime;

            Img_ProgressBar.fillAmount = Mathf.Lerp(Img_ProgressBar.fillAmount, 1f, timer);

            if (Img_ProgressBar.fillAmount >= 0.9f)
            {
                Img_ProgressBar.fillAmount = Mathf.Lerp(Img_ProgressBar.fillAmount, 1f, timer);

                if (Img_ProgressBar.fillAmount == 1f)
                {
                    Txt_Info.enabled = true;
                    Img_ProgressBar.enabled = false;
                    yield return null;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CustomSceneManager.Instance.ChangeScene(eSceneState.Title, eSceneState.Main);
    }

    private void SetupCamera()
    {
        float targetWidthAspect = 9f;
        float targetHeightAspect = 16f;

        Camera mainCamera = this.GetComponentInParent<Camera>();

        if (mainCamera != null)
        {

            mainCamera.aspect = targetWidthAspect / targetHeightAspect;

            float widthRatio = (float)Screen.width / targetWidthAspect;
            float heightRatio = (float)Screen.height / targetHeightAspect;

            float heightAdd = ((widthRatio / (heightRatio / 100)) - 100) / 200;
            float widthAdd = ((heightRatio / (widthRatio / 100)) - 100) / 200;

            if (widthRatio > heightRatio)
                heightAdd = 0f;
            else
                widthAdd = 0.0f;

            mainCamera.rect = new Rect(
                mainCamera.rect.x + Math.Abs(widthAdd),
                mainCamera.rect.y + Math.Abs(heightAdd),
                mainCamera.rect.width + (widthAdd * 2),
                mainCamera.rect.height + (heightAdd * 2));
        }
    }
}
