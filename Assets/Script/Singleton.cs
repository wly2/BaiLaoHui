using System;
using UnityEngine;
using System.Collections;

public class Singleton<T> : IDisposable where T : new()
{
    private static T m_tInstance;

    private static readonly object m_oSync = new object();

    protected Singleton()
    {
    }

    public static T Instance
    {
        get
        {
            if (m_tInstance == null)
            {
                lock (m_oSync)
                {
                    if (m_tInstance == null)
                    {
                        m_tInstance = new T();
                    }
                }
            }
            return m_tInstance;
        }
    }

    public virtual void Init()
    {

    }

    public virtual void OnDestroy()
    {

    }

    public virtual void Dispose()
    {
        OnDestroy();
        m_tInstance = default(T);
        GC.Collect();
    }
}
