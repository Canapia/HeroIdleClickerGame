using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabDevelopers : MonoBehaviour
{
    public CustomButton Btn_OK;

    // Start is called before the first frame update
    void Start()
    {
        Btn_OK.OnClick.AddListener(OnClickButton_OK);

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
                OnClickButton_OK();
            }
        }
    }

    public void OnClickButton_OK()
    {
        if (MainController.Instance != null)
        {
            MainController.Instance.m_IsPopup = false;
        }

        Destroy(this.gameObject);
    }
}
