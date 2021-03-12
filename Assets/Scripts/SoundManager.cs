using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public enum eBGMState
{
    Title = 0,              // 시작 화면
    Main,           // 메인, 지도선택, 마을, 재화샵
    Stage,
    Boss_Stage,
    GameOver,           // 사망
    Story,              // 스토리 진행시 (더 추가 될 수 있음)

    END
}

public class SoundManager : MonoBehaviour
{
    #region Inspector ======================
    [Header("- BGM")]
    public AudioSource m_BGMAudioSource;
    public AudioClip[] m_MainBGMs;
    [Header("- SoundEffect")]
    public AudioSource m_SoundEffect;
    #endregion ==============================

    private bool m_IsFade = false;
    private float m_FadeTime = 1.0f;

    private static SoundManager m_Instance;
    public static SoundManager Instance
    {
        set { m_Instance = value; }
        get { return m_Instance; }
    }

    private bool m_BGMMute = false;
    public bool BGMMute
    {
        get { return m_BGMMute; }
        set 
        { 
            m_BGMMute = value;
            if (MainController.Instance != null)
            {
                MainController.Instance.UserInfo.BGMMute = m_BGMMute;
                MainController.Instance.UserInfo.SaveUser();
            }
        }
    }
    private float m_BGMVolume = 1.0f;
    public float BGMVolume
    {
        get { return m_BGMVolume; }
        set 
        { 
            m_BGMVolume = value;
            BGMVolumeSetting();
            if (MainController.Instance != null)
            {
                MainController.Instance.UserInfo.BGMVolume = m_BGMVolume;
                MainController.Instance.UserInfo.SaveUser();
            }
        }
    }
    private bool m_SFxMute = false;
    public bool SFxMute
    {
        get { return m_SFxMute; }
        set 
        { 
            m_SFxMute = value;
            if (MainController.Instance != null)
            {
                MainController.Instance.UserInfo.SFxMute = m_SFxMute;
                MainController.Instance.UserInfo.SaveUser();
            }
        }
    }
    private float m_SFxVolume = 1.0f;
    public float SFxVolume
    {
        get { return m_SFxVolume; }
        set 
        { 
            m_SFxVolume = value;
            if (MainController.Instance != null)
            {
                MainController.Instance.UserInfo.SFxVolume = m_SFxVolume;
                MainController.Instance.UserInfo.SaveUser();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_Instance == null)
        {
            m_Instance = GetComponent<SoundManager>();
            Debug.Log("SoundManager 생성됨");
        }
        else
        {
            Destroy(m_Instance);
            Debug.Log("SoundManager 제거됨");
        }

        StartCoroutine(WaitForMainController());
    }

    IEnumerator WaitForMainController()
    {
        yield return new WaitUntil(() => MainController.Instance != null);

        m_BGMVolume = MainController.Instance.UserInfo.BGMVolume;
        m_BGMMute = MainController.Instance.UserInfo.BGMMute;
        m_SFxVolume = MainController.Instance.UserInfo.SFxVolume;
        m_SFxMute = MainController.Instance.UserInfo.SFxMute;
    }

    public void PlayBGM(eBGMState _state)
    {
        if (m_BGMAudioSource.isPlaying == true)
        {
            m_BGMAudioSource.Stop();
        }
            

        m_BGMAudioSource.clip = m_MainBGMs[(int)_state];
        m_BGMAudioSource.loop = true;
        m_BGMAudioSource.volume = m_BGMVolume;
        m_BGMAudioSource.mute = m_BGMMute;
        m_BGMAudioSource.Play();
    }

    public void OneShotPlayBGM(eBGMState _state)
    {
        if(m_BGMAudioSource.isPlaying == true)
        {
            m_BGMAudioSource.Stop();
        }

        m_BGMAudioSource.clip = m_MainBGMs[(int)_state];
        m_BGMAudioSource.loop = false;
        m_BGMAudioSource.volume = m_BGMVolume;
        m_BGMAudioSource.mute = m_BGMMute;
        m_BGMAudioSource.Play();
    }

    public void BGMMuteSetting()
    {
        if(m_BGMAudioSource.isPlaying == true)
        {
            m_BGMAudioSource.loop = true;
            m_BGMAudioSource.volume = m_BGMVolume;
            m_BGMAudioSource.mute = m_BGMMute;
            m_BGMAudioSource.Play();
        }
    }

    public void BGMVolumeSetting()
    {
        if(m_BGMAudioSource.isPlaying == true)
        {
            m_BGMAudioSource.volume = m_BGMVolume;
        }
    }

    public void PlaySoundEffect()
    {
        m_SoundEffect.loop = false;
        m_SoundEffect.volume = m_SFxVolume;
        m_SoundEffect.mute = m_SFxMute;
        m_SoundEffect.Play();
    }
}
