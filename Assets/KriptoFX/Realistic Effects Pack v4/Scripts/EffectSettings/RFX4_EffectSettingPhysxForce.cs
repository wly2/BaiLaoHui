using UnityEngine;

public class RFX4_EffectSettingPhysxForce : MonoBehaviour
{

    public float ForceMultiplier = 1;

    // Update is called once per frame
    void Update()
    {
        //if (Math.Abs(previousForceMultiplier - ForceMultiplier) > 0.001f)
        {
            var transformMotion = GetComponentInChildren<RFX4_TransformMotion>(true);
            if (transformMotion != null)
            {
                var instances = transformMotion.CollidedInstances;
                for (int i = 0; i < instances.Count; ++i)
                {
                    var physxForceCurve = instances[i].GetComponent<RFX4_PhysicsForceCurves>();
                    if (physxForceCurve != null) physxForceCurve.forceAdditionalMultiplier = ForceMultiplier;
                }
            }

            var physxForceCurves = GetComponentsInChildren<RFX4_PhysicsForceCurves>();
            for (int i = 0; i < physxForceCurves.Length; ++i)
            {
                if (physxForceCurves[i] != null) physxForceCurves[i].forceAdditionalMultiplier = ForceMultiplier;
            }
        }
    }
}