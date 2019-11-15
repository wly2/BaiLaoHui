using UnityEngine;

public class NcRotation : NcEffectBehaviour
{
    public bool m_bWorldSpace;
    public Vector3 m_vRotationValue = new Vector3(0, 360, 0);
#if UNITY_EDITOR
    public override string CheckProperty()
    {
        if (GetComponent<NcBillboard>() != null)
            return "SCRIPT_CLASH_ROTATEBILL";
        return ""; // no error
    }
#endif
    void Update()
    {
        transform.Rotate(GetEngineDeltaTime() * m_vRotationValue.x, GetEngineDeltaTime() * m_vRotationValue.y,
            GetEngineDeltaTime() * m_vRotationValue.z, (m_bWorldSpace ? Space.World : Space.Self));
    }

    public override void OnUpdateEffectSpeed(float fSpeedRate, bool bRuntime)
    {
        m_vRotationValue *= fSpeedRate;
    }
}