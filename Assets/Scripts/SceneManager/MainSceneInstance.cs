using System.Collections;
using System.Collections.Generic;

public class MainSceneInstance
{
    private static MainSceneInstance m_Instance;
    public static MainSceneInstance Instance
    {
        get
        {
            if(m_Instance != null)
            {
                m_Instance = new MainSceneInstance();
            }
            return m_Instance;
        }
        set { m_Instance = value; }
    }
}
