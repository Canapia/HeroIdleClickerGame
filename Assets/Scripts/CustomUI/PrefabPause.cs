using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabPause : MonoBehaviour
{
    #region Inspector
    public CustomButton Btn_Restart;
    public CustomButton Btn_Exit;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(AdventureModeInHuntSceneUI.Instance != null)
        {
            AdventureModeInHuntSceneUI.Instance.HuntState = eHuntSceneState.Pause;
        }

        if (MainController.Instance != null)
        {
            MainController.Instance.m_IsPopup = true;
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                OnClickButton_Exit();
            }
        }
    }

    public void OnClickButton_Exit()
    {
        CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInHunt, eSceneState.Main);

        PopupDestroy();
    }

    public void OnClickButton_Restart()
    {
        if(AdventureModeInHuntSceneUI.Instance != null)
        {
            AdventureModeInHuntSceneUI.Instance.PublicStartCountDown();
        }

        PopupDestroy();
    }

    public void PopupDestroy()
    {
        Destroy(this.gameObject);

        if (MainController.Instance != null)
        {
            MainController.Instance.m_IsPopup = false;
        }
    }
}
