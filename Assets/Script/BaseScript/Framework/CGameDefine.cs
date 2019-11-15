using UnityEngine;

public class CGameMsg
{
    public CGameMsg()
    {
    }

    public Vector3 m_vecPos;
    public Vector3 m_vecDestPos;
    public Vector3 m_vecBallPos;
    public Vector3 m_vecBallVelocity;
    public int m_nDir;
    public float m_fRotate;
    public int m_nBehaviorID;
}

public class CBallStateEvent
{
    public CBallStateEvent()
    {
        m_ulCharacterID = 0;
        m_fDeltaTime = 0.0f;
        m_fPassTime = 0.0f;
    }

    public ulong m_ulCharacterID;
    public float m_fPassTime;
    public float m_fDeltaTime;
}

public class CCharacterStateEvent
{
    public CCharacterStateEvent()
    {
    }
}