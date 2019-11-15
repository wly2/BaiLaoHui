using UnityEngine;

public class NcDetachObject : NcEffectBehaviour
{
    public GameObject m_LinkGameObject;

    public static NcDetachObject Create(GameObject parentObj, GameObject linkObject)
    {
        NcDetachObject deObj = parentObj.AddComponent<NcDetachObject>();
        deObj.m_LinkGameObject = linkObject;
        return deObj;
    }

    public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
    {
        if (bRuntime)
            NsEffectManager.AdjustSpeedRuntime(m_LinkGameObject, fSpeedRate);
    }

    public override void OnSetActiveRecursively(bool bActive)
    {
        if (m_LinkGameObject != null)
            NsEffectManager.SetActiveRecursively(m_LinkGameObject, bActive);
    }
}