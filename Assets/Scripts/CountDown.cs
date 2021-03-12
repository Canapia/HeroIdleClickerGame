using System.Timers;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    #region Inspector
    public Text Txt_CountNum;
    #endregion

    private static Timer m_Timer;
    public int m_StartTime = 3;
    private int m_TimeLeft;
    public bool m_IsDone = false;

    // Start is called before the first frame update
    void Start()
    {
        m_TimeLeft = m_StartTime;
        Txt_CountNum.text = m_TimeLeft.ToString();

        SetCountDown();
    }

    private void Update()
    {
        if (m_TimeLeft > 0)
        {
            Txt_CountNum.text = m_TimeLeft.ToString();
        }
        else if(m_TimeLeft == 0)
        {
            Txt_CountNum.text = "Start!";
        }
        else if(m_TimeLeft == -1)
        {
            m_Timer.Stop();
            Destroy(this.gameObject);
        }
    }

    private void SetCountDown()
    {
        m_Timer = new Timer(1000);
        m_Timer.Elapsed += RenewalUI;
        m_Timer.AutoReset = true;
        m_Timer.Enabled = true;
    }

    private void RenewalUI(object source, ElapsedEventArgs e)
    {
        // 이 곳에서 Text가 갱신되지 않음....
        if (m_TimeLeft > 0)
        {
            m_TimeLeft = m_TimeLeft - 1;
            //Txt_CountNum.text = m_TimeLeft.ToString();
        }
        else if(m_TimeLeft == 0)
        {
            m_IsDone = true;
            m_TimeLeft--;
            //Txt_CountNum.text = "Start!";
        }
    }
}
