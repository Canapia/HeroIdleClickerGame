using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using GoogleMobileAds.Api;

public class GoodsSceneUI : MonoBehaviour
{
    #region Inspector
    [Header("- UI")]
    public Text Txt_Gold;
    public Text Txt_Jam;
    public CustomButton Btn_Option;
    public CustomButton Btn_Back;
    public CustomButton Btn_Pause;
    #endregion

    private BannerView m_Banner;

    private void Awake()
    {
        Screen.SetResolution(Screen.width, (Screen.width / 9) * 16, true);

        //SetupCamera();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GoodsSceneInstance.Instance != null)
            GoodsSceneInstance.Instance.Obj_Position = gameObject;

        RenewalUI_Gold();
        RenewalUI_Jam();

        ChangeButtonState();
    }

    private void RenewalUI_Gold()
    {
        if(GoodsSceneInstance.Instance != null)
        {
            GoodsSceneInstance.Instance.RenewalUI_Gold(Txt_Gold);
        }
    }

    private void RenewalUI_Jam()
    {
        if(GoodsSceneInstance.Instance != null)
        {
            GoodsSceneInstance.Instance.RenewalUI_Jam(Txt_Jam);
        }
    }

    public void OnClickButton_Option()
    {
        ResourceManager.CreatePrefab("Prefab_Option", gameObject.transform);
    }

    public void OnClickButton_Back()
    {
        CustomSceneManager.Instance.ChangeScene(eSceneState.AdventureInMap, eSceneState.Main);
    }

    public void OnClickButton_Pause()
    {
        if(AdventureModeInHuntSceneUI.Instance != null)
        {
            // 카운트 다운 일 경우 무시
            if(AdventureModeInHuntSceneUI.Instance.HuntState == eHuntSceneState.CountDown)
            {
                return;
            }
            AdventureModeInHuntSceneUI.Instance.PauseInGamePlay();
        }

        ResourceManager.CreatePrefab("Prefab_Pause", gameObject.transform);

    }

    private void ChangeButtonState()
    {
        if (GoodsSceneInstance.Instance != null)
        {
            GoodsSceneInstance.Instance.ChangeButtonState(Btn_Option, Btn_Back, Btn_Pause);
        }
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
