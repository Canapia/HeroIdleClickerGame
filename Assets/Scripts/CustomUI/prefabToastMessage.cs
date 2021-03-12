using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefabToastMessage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ToastRemoving());
    }

    IEnumerator ToastRemoving()
    {
        yield return new WaitForSeconds(1.5f);

        ToastDestroy();
    }

    public void ToastDestroy()
    {
        Destroy(this.gameObject);

        if (MainController.Instance != null)
        {
            MainController.Instance.ToastMessage = null;
        }
    }
}
