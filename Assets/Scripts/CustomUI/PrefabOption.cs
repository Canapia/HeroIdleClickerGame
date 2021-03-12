using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrefabOption : MonoBehaviour
{
    #region Inspector
    public Slider Sld_BGM;
    public Slider Sld_SFx;
    public Image Img_Icon_BGM;
    public Image Img_Icon_SFx;

    public Sprite Spr_Icon_BGM_On;
    public Sprite Spr_Icon_BGM_Off;
    public Sprite Spr_Icon_SFx_On;
    public Sprite Spr_Icon_SFx_Off;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        this.transform.SetParent(GetComponentInParent<Transform>().transform, true);

        if (SoundManager.Instance == null)
        {
            Debug.LogError("SoundManager 인스턴스 생성되지 않음! SoundManager 처리 불가");
        }
        else if (SoundManager.Instance != null)
        {
            Sld_BGM.value = SoundManager.Instance.BGMVolume;
            Sld_SFx.value = SoundManager.Instance.SFxVolume;

            Sld_BGM.onValueChanged.AddListener(delegate { OnValueChanged_BGM(); });
            Sld_SFx.onValueChanged.AddListener(delegate { OnValueChanged_SFx(); });

            // 유저에게 저장되어 있는 배경음 / 효과음 MUTE에 관한 UI 처리
            if(SoundManager.Instance.BGMMute == true)
            {
                Img_Icon_BGM.sprite = Spr_Icon_BGM_Off;
            }

            if(SoundManager.Instance.SFxMute == true)
            {
                Img_Icon_SFx.sprite = Spr_Icon_SFx_Off;
            }
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
                if (CustomSceneManager.Instance != null)
                {
                    OnClickButton_Close();
                }
            }
        }
    }

    public void OnClickButton_Close()
    {
        Destroy(this.gameObject);

        if (MainController.Instance != null)
        {
            MainController.Instance.m_IsPopup = false;
        }
    }

    public void OnValueChanged_BGM()
    {
        if (SoundManager.Instance.BGMMute == true &&
                SoundManager.Instance.BGMVolume != Sld_BGM.value)
        {
            SoundManager.Instance.BGMMute = false;
            Img_Icon_BGM.sprite = Spr_Icon_BGM_On;
        }

        SoundManager.Instance.BGMVolume = Sld_BGM.value;

        if(Sld_BGM.value == 0)
        {
            SoundManager.Instance.BGMMute = true;
            //Img_Icon_BGM.sprite = ResourceManager.LoadIconImage("Background_music_off_UI");
            Img_Icon_BGM.sprite = Spr_Icon_BGM_Off;
        }
        else if(Sld_BGM.value > 0)
        {
            SoundManager.Instance.BGMMute = false;
            //Img_Icon_BGM.sprite = ResourceManager.LoadIconImage("Background_music_UI");
            Img_Icon_BGM.sprite = Spr_Icon_BGM_On;
        }
    }

    public void OnValueChanged_SFx()
    {
        if (SoundManager.Instance.SFxMute == true &&
                SoundManager.Instance.SFxVolume != Sld_SFx.value)
        {
            SoundManager.Instance.SFxMute = false;
            Img_Icon_SFx.sprite = Spr_Icon_SFx_On;
        }

        SoundManager.Instance.SFxVolume = Sld_SFx.value;

        if(Sld_SFx.value == 0)
        {
            SoundManager.Instance.SFxMute = true;
            //Img_Icon_SFx.sprite = ResourceManager.LoadIconImage("Effect_music_off_UI");
            Img_Icon_SFx.sprite = Spr_Icon_SFx_Off;
        }
        else if(Sld_SFx.value > 0)
        {
            SoundManager.Instance.SFxMute = false;
            //Img_Icon_SFx.sprite = ResourceManager.LoadIconImage("Effect_music_UI");
            Img_Icon_SFx.sprite = Spr_Icon_SFx_On;
        }
    }

    public void OnClickButton_Icon_BGM()
    {
        // 버튼을 통한 On/Off 시에는 이전 음량은 그대로 갖고 유지 한다.
        if(SoundManager.Instance.BGMMute == true &&
            SoundManager.Instance.BGMVolume > 0)
        {
            SoundManager.Instance.BGMMute = false;
            Img_Icon_BGM.sprite = Spr_Icon_BGM_On;

            SoundManager.Instance.BGMMuteSetting();
        }
        else if(SoundManager.Instance.BGMMute == false)
        {
            SoundManager.Instance.BGMMute = true;
            Img_Icon_BGM.sprite = Spr_Icon_BGM_Off;

            SoundManager.Instance.BGMMuteSetting();
        }
    }

    public void OnClickButton_Icon_SFx()
    {
        if(SoundManager.Instance.SFxMute == true &&
            SoundManager.Instance.SFxVolume > 0)
        {
            SoundManager.Instance.SFxMute = false;
            Img_Icon_SFx.sprite = Spr_Icon_SFx_On;

        }
        else if(SoundManager.Instance.SFxMute == false)
        {
            SoundManager.Instance.SFxMute = true;
            Img_Icon_SFx.sprite = Spr_Icon_SFx_Off;
        }
    }

    public void OnClickButton_AccountIntial()
    {
        OnClickButton_Close();

        if(GoodsSceneInstance.Instance != null)
        {
            GoodsSceneInstance.Instance.ShowPopup_UserInfo_ReStart();
        }

        //GameObject obj = ResourceManager.GetOBJCreatePrefab("PrefabYesOrNoPopup", gameObject.transform);
        //PrefabYesOrNoPopup popup = obj.GetComponent<PrefabYesOrNoPopup>();
        //// 추후 DB 또는 관리를 하나 만들어서 출력할 것
        //popup.Initialize(ePopupState.YesOrNo, "알림!", "계정을 초기화하면 현재까지 진행한 모든 내용이 사라집니다. 진행하시겠습니까?",
        //    PopupYesDoing);
    }

    public void PopupYesDoing()
    {
        User _newInfo = new User();
        MainController.Instance.UserInfo = _newInfo;
        MainController.Instance.UserInfo.SaveUser();

        // 임시로 재시작 하는 것 처럼 보임
        CustomSceneManager.Instance.ChangeScene(eSceneState.Main, eSceneState.Title);
    }

    public void OnClickButton_Developers()
    {
        OnClickButton_Close();

        if(GoodsSceneInstance.Instance != null)
        {
            GoodsSceneInstance.Instance.ShowPopup("PrefabDevelopers");
        }

        //GameObject obj = ResourceManager.GetOBJCreatePrefab("PrefabDevelopers", gameObject.transform);
        //PrefabDevelopers popup = obj.GetComponent<PrefabDevelopers>();
        // 추후 DB 또는 관리를 하나 만들어서 출력할 것
        //popup.Initialize();
    }
}
