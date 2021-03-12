using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PrepareSceneManager : MonoBehaviour
{
    #region Inpector
    public MainController m_MainCtr;
    public SoundManager m_SoundCtr;
    public CustomSceneManager m_SceneCtr;
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(m_MainCtr);
        DontDestroyOnLoad(m_SoundCtr);
        DontDestroyOnLoad(m_SceneCtr);
    }

    // Start is called before the first frame update
    void Start()
    {
        

        //CustomSceneManager.Instance.ChangeScene_Prepare();
    }
}
