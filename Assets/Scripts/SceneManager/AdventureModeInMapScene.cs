using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureModeInMapScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (CustomSceneManager.Instance != null)
            {
                if (CustomSceneManager.Instance.m_SceneChanging == false &&
                    MainController.Instance.m_IsPopup == false)
                {
                    if (Input.GetKey(KeyCode.Escape))
                    {
                        if (CustomSceneManager.Instance != null)
                        {
                            CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInMap, eSceneState.Main);
                        }
                    }
                }
            }
        }
    }
}
