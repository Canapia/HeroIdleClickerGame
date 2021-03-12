using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class UIHPBar : MonoBehaviour
{
    #region Inspector
    public Image Img_Bar;
    public Text Txt_Point;
    #endregion

    private float m_MaxPoint = 0;
    private float m_StatePoint = 0;

    // Start is called before the first frame update
    public void Initialize(int _state, int _max)
    {
        m_MaxPoint = _max;
        m_StatePoint = _state;

        RenewalUI();
    }

    private void Update()
    {
        RenewalUI();
    }

    public void SetMaxPoint(float _point)
    {
        m_MaxPoint = _point;
    }

    public void SetStatePoint(float _point)
    {
        m_StatePoint = _point;
    }

    public void RenewalUI()
    {
        Txt_Point.text = string.Format("{0} / {1}", m_StatePoint, m_MaxPoint);
        Img_Bar.fillAmount = m_StatePoint / m_MaxPoint;
    }
}
