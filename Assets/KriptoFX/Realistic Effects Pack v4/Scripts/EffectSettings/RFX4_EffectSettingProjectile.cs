using UnityEngine;

public class RFX4_EffectSettingProjectile : MonoBehaviour
{
    public float FlyDistanceForProjectiles = 30;
    public float SpeedMultiplier = 1;
    public LayerMask CollidesWith = ~0;

    float startSpeed;
    const string particlesAdditionalName = "Distance";

    void Awake()
    {
        var transformMotion = GetComponentInChildren<RFX4_TransformMotion>(true);
        if (transformMotion != null)
        {
            startSpeed = transformMotion.Speed;
        }
    }

    void OnEnable()
    {
        var transformMotion = GetComponentInChildren<RFX4_TransformMotion>(true);
        if (transformMotion != null)
        {
            transformMotion.Distance = FlyDistanceForProjectiles;
            transformMotion.CollidesWith = CollidesWith;
            transformMotion.Speed = startSpeed * SpeedMultiplier;
        }

        var rayCastCollision = GetComponentInChildren<RFX4_RaycastCollision>(true);
        if (rayCastCollision != null) rayCastCollision.RaycastDistance = FlyDistanceForProjectiles;
        var particlesystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particlesystems.Length; ++i)
        {

            if (particlesystems[i].name.Contains(particlesAdditionalName))
#if !UNITY_5_5_OR_NEWER
                ps.GetComponent<ParticleSystemRenderer>().lengthScale = FlyDistanceForProjectiles / ps.startSize;
#else
                particlesystems[i].GetComponent<ParticleSystemRenderer>().lengthScale =
                    FlyDistanceForProjectiles / particlesystems[i].main.startSize.constantMax;
#endif
        }
    }
}