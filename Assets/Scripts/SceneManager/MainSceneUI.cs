using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MainSceneUI : MonoBehaviour
{
    public UIIconBuildinSetting[] Btn_Buildings;

    private int m_ClickCount = 0;

    private void Awake()
    {
        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(eBGMState.Main);
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.WindowsEditor)
        {
            //Debug.Log(string.Format(
            //    "튜토리얼 중인가? : {0}", TutorialStorySystem.Instance.m_IsTutorialing == true ? "true" : "false"));

            if (TutorialStorySystem.Instance.m_IsTutorialing == false &&
                MainController.Instance.m_IsPopup == false &&
                CustomSceneManager.Instance.m_SceneChanging == false)
            {
                if (m_ClickCount == 2)
                {
                    CancelInvoke("DoubleClick");
                    Application.Quit();
                }
                else if (Input.GetKey(KeyCode.Escape))
                {
                    m_ClickCount++;

                    if (GoodsSceneInstance.Instance != null && MainController.Instance.ToastMessage == null)
                    {
                        GameObject obj = ResourceManager.GetOBJCreatePrefab(
                            "PrefabToastMessage", GoodsSceneInstance.Instance.Obj_Position.transform);
                        MainController.Instance.ToastMessage = obj.GetComponent<prefabToastMessage>();
                    }
                    else if (GoodsSceneInstance.Instance != null && MainController.Instance.ToastMessage != null)
                    {
                        // 기존 토스트를 삭제하고 재 생성
                        MainController.Instance.ToastMessage.ToastDestroy();

                        GameObject obj = ResourceManager.GetOBJCreatePrefab(
                            "PrefabToastMessage", GoodsSceneInstance.Instance.Obj_Position.transform);
                        MainController.Instance.ToastMessage = obj.GetComponent<prefabToastMessage>();
                    }

                    if (!IsInvoking("DoubleClick"))
                    {
                        Invoke("DoubleClick", 1.0f);
                    }
                   
                }
            }
        }
    }

    void DoubleClick()
    {
        m_ClickCount = 0;
    }

    public void OnClickButton_AdventureMode()
    {
        CustomSceneManager.Instance.ChangeScene(eSceneState.Main, eSceneState.AdventureInMap);
    }

    public void OtherBuildingUpdate()
    {
        foreach(UIIconBuildinSetting build in Btn_Buildings)
        {
            build.RenewalUI_NextPrice();
        }
    }
}
