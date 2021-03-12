using System.Collections;
using System.Collections.Generic;

[System.Serializable]

public class AESKeyAndIV
{

    private byte[] m_Key;
    public byte[] Key
    {
        get { return m_Key; }
        set { m_Key = value; }
    }
    private byte[] m_IV;
    public byte[] IV
    {
        get { return m_IV; }
        set { m_IV = value; }
    }

    public AESKeyAndIV(byte[] Key, byte[] IV)
    {
        m_Key = Key;
        m_IV = IV;
    }
}
