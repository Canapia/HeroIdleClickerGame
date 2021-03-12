using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class RepeatingBackground : MonoBehaviour
{
    private Image Img_BG;
    private float groundHorizontalLength;

    public float m_Distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        Img_BG = GetComponent<Image>();
        groundHorizontalLength = Img_BG.rectTransform.sizeDelta.x;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Img_BG.transform.position.x.ToString());
        if(transform.position.x <= -groundHorizontalLength)
        //if(Img_BG.transform.position.x <= -(groundHorizontalLength/3)*2 - m_Distance)
        {
            RepositionBackground();
        }
    }

    private void RepositionBackground()
    {
        Debug.Log("이미지 위치 초기화!");
        Vector2 bgOffset = new Vector2(groundHorizontalLength * 2.0f + m_Distance, 0);
        //transform.position = (Vector2)transform.position + bgOffset;
        transform.localPosition = (Vector2)transform.localPosition + bgOffset;
    }
}
