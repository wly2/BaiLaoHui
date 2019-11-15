using UnityEngine;

public class NcRandomTimerTool : NcTimerTool
{
    protected float m_fRandomTime;
    protected float m_fUpdateTime;
    protected float m_fMinIntervalTime;
    protected int m_nRepeatCount;
    protected int m_nCallCount;
    protected object m_ArgObject;

    public bool UpdateRandomTimer(bool bReset)
    {
        if (UpdateRandomTimer())
        {
            ResetUpdateTime();
            return true;
        }

        return false;
    }

    public bool UpdateRandomTimer()
    {
        if (!m_bEnable)
            return false;
        bool bNext = (m_fUpdateTime <= GetTime());
        if (bNext)
        {
            m_fUpdateTime += m_fMinIntervalTime + (0 < m_fRandomTime ? Random.value % m_fRandomTime : 0);
            m_nCallCount++;
            if (m_nRepeatCount != 0 && m_nRepeatCount <= m_nCallCount)
                m_bEnable = false;
        }

        return bNext;
    }

    public void ResetUpdateTime()
    {
        m_fUpdateTime = GetTime() + m_fMinIntervalTime + (0 < m_fRandomTime ? Random.value % m_fRandomTime : 0);
    }

    public int GetCallCount()
    {
        return m_nCallCount;
    }

    public object GetArgObject()
    {
        return m_ArgObject;
    }

    public float GetElapsedRate()
    {
        if (m_fUpdateTime == 0)
            return 1;
        return (GetTime() / m_fUpdateTime);
    }

    public void SetTimer(float fStartTime, float fRandomTime)
    {
        SetRelTimer(fStartTime - NcTimerTool.GetEngineTime(), fRandomTime);
    }

    public void SetTimer(float fStartTime, float fRandomTime, float fMinIntervalTime)
    {
        SetRelTimer(fStartTime - GetEngineTime(), fRandomTime, fMinIntervalTime);
    }

    public void SetTimer(float fStartTime, float fRandomTime, float fMinIntervalTime, int nRepeatCount)
    {
        SetRelTimer(fStartTime - GetEngineTime(), fRandomTime, fMinIntervalTime, nRepeatCount);
    }

    public void SetTimer(float fStartTime, float fRandomTime, object arg)
    {
        SetRelTimer(fStartTime - GetEngineTime(), fRandomTime, arg);
    }

    public void SetTimer(float fStartTime, float fRandomTime, float fMinIntervalTime, object arg)
    {
        SetRelTimer(fStartTime - GetEngineTime(), fRandomTime, fMinIntervalTime, arg);
    }

    public void SetTimer(float fStartTime, float fRandomTime, float fMinIntervalTime, int nRepeatCount, object arg)
    {
        SetRelTimer(fStartTime - GetEngineTime(), fRandomTime, fMinIntervalTime, nRepeatCount, arg);
    }

    public void SetRelTimer(float fStartRelTime, float fRandomTime)
    {
        Start();
        m_nCallCount = 0;
        m_fRandomTime = fRandomTime;
        m_fUpdateTime = (0 < m_fRandomTime ? Random.value % m_fRandomTime : 0);
        m_fMinIntervalTime = 0;
        m_nRepeatCount = 0;
    }

    public void SetRelTimer(float fStartRelTime, float fRandomTime, float fMinIntervalTime)
    {
        Start();
        m_nCallCount = 0;
        m_fRandomTime = fRandomTime;
        m_fUpdateTime = (0 < m_fRandomTime ? Random.value % m_fRandomTime : 0);
        m_fMinIntervalTime = fMinIntervalTime;
        m_nRepeatCount = 0;
    }

    public void SetRelTimer(float fStartRelTime, float fRandomTime, float fMinIntervalTime, int nRepeatCount)
    {
        Start();
        m_nCallCount = 0;
        m_fRandomTime = fRandomTime;
        m_fUpdateTime = (0 < m_fRandomTime ? Random.value % m_fRandomTime : 0);
        m_fMinIntervalTime = fMinIntervalTime;
        m_nRepeatCount = nRepeatCount;
    }

    public void SetRelTimer(float fStartRelTime, float fRandomTime, object arg)
    {
        Start();
        m_nCallCount = 0;
        m_fRandomTime = fRandomTime;
        m_fUpdateTime = (0 < m_fRandomTime ? Random.value % m_fRandomTime : 0);
        m_fMinIntervalTime = 0;
        m_nRepeatCount = 0;
        m_ArgObject = arg;
    }

    public void SetRelTimer(float fStartRelTime, float fRandomTime, float fMinIntervalTime, object arg)
    {
        Start();
        m_nCallCount = 0;
        m_fRandomTime = fRandomTime;
        m_fUpdateTime = (0 < m_fRandomTime ? Random.value % m_fRandomTime : 0);
        m_fMinIntervalTime = fMinIntervalTime;
        m_nRepeatCount = 0;
        m_ArgObject = arg;
    }

    public void SetRelTimer(float fStartRelTime, float fRandomTime, float fMinIntervalTime, int nRepeatCount,
        object arg)
    {
        Start();
        m_nCallCount = 0;
        m_fRandomTime = fRandomTime;
        m_fUpdateTime = (0 < m_fRandomTime ? Random.value % m_fRandomTime : 0);
        m_fMinIntervalTime = fMinIntervalTime;
        m_nRepeatCount = nRepeatCount;
        m_ArgObject = arg;
    }
}