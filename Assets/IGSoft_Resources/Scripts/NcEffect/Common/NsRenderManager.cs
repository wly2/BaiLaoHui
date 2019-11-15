// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

public class NsRenderManager : MonoBehaviour
{
    // Attribute ------------------------------------------------------------------------
    public List<Component> m_RenderEventCalls;

    void OnPreRender()
    {
        if (m_RenderEventCalls != null)
        {
            for (int n = m_RenderEventCalls.Count - 1; 0 <= n; n--)
            {
                if (m_RenderEventCalls[n] == null)
                {
                    m_RenderEventCalls.RemoveAt(n);
                    continue;
                }

                m_RenderEventCalls[n].SendMessage("OnPreRender", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnPostRender()
    {
        if (m_RenderEventCalls != null)
            for (int i = 0; i < m_RenderEventCalls.Count; ++i)
            {
                if (m_RenderEventCalls[i] != null)
                    m_RenderEventCalls[i].SendMessage("OnPostRender", SendMessageOptions.DontRequireReceiver);
            }
    }

    // Control Function -----------------------------------------------------------------
    public void AddRenderEventCall(Component tarCom)
    {
        if (m_RenderEventCalls == null)
            m_RenderEventCalls = new List<Component>();
        if (!m_RenderEventCalls.Contains(tarCom))
            m_RenderEventCalls.Add(tarCom);
    }

    public void RemoveRenderEventCall(Component tarCom)
    {
        if (m_RenderEventCalls == null)
            m_RenderEventCalls = new List<Component>();
        if (m_RenderEventCalls.Contains(tarCom))
            m_RenderEventCalls.Remove(tarCom);
    }

    // Event Function -------------------------------------------------------------------
}