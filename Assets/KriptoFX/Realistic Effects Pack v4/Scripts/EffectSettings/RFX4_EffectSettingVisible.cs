using UnityEngine;

public class RFX4_EffectSettingVisible : MonoBehaviour
{
    public bool IsActive = true;
    public float FadeOutTime = 3;

    private bool previousActiveStatus;
    const string rendererAdditionalName = "Loop";

    string[] colorProperties =
    {
        "_TintColor", "_Color", "_EmissionColor", "_BorderColor", "_ReflectColor", "_RimColor",
        "_MainColor", "_CoreColor"
    };

    float alpha;

    void Update()
    {
        if (IsActive) alpha += Time.deltaTime;
        else alpha -= Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        if (!IsActive)
        {
            var loopRenderers = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < loopRenderers.Length; ++i)
            {
                if (loopRenderers[i].GetComponent<ParticleSystem>() != null) continue;
                if (!loopRenderers[i].name.Contains(rendererAdditionalName)) continue;

                var mat = loopRenderers[i].material;
                var shaderColorGradient = loopRenderers[i].GetComponent<RFX4_ShaderColorGradient>();
                if (shaderColorGradient != null) shaderColorGradient.canUpdate = false;

                for (int j = 0; j < colorProperties.Length; ++j)
                {
                    if (mat.HasProperty(colorProperties[j]))
                    {
                        var color = mat.GetColor(colorProperties[j]);
                        color.a = alpha;
                        mat.SetColor(colorProperties[j], color);
                    }
                }
            }

            var loopProjectors = GetComponentsInChildren<Projector>();
            for (int i = 0; i < loopProjectors.Length; ++i)
            {

                if (!loopProjectors[i].name.Contains(rendererAdditionalName)) continue;

                if (!loopProjectors[i].material.name.EndsWith("(Instance)"))
                    loopProjectors[i].material =
                        new Material(loopProjectors[i].material)
                        {
                            name = loopProjectors[i].material.name + " (Instance)"
                        };
                var mat = loopProjectors[i].material;

                var shaderColorGradient = loopProjectors[i].GetComponent<RFX4_ShaderColorGradient>();
                if (shaderColorGradient != null) shaderColorGradient.canUpdate = false;

                for (int j = 0; j < colorProperties.Length; ++j)
                {
                    if (mat.HasProperty(colorProperties[j]))
                    {
                        var color = mat.GetColor(colorProperties[j]);
                        color.a = alpha;
                        mat.SetColor(colorProperties[j], color);
                    }
                }
            }

            var particleSystems = GetComponentsInChildren<ParticleSystem>(true);
            for (int i = 0; i < particleSystems.Length; ++i)
            {
                if (particleSystems[i] != null) particleSystems[i].Stop();
            }

            var lights = GetComponentsInChildren<Light>(true);
            for (int i = 0; i < lights.Length; i++)
            {
                if (lights[i].isActiveAndEnabled)
                {
                    var lightCurves = lights[i].GetComponent<RFX4_LightCurves>();
                    if (lightCurves != null)
                    {
                        lights[i].intensity = alpha * lightCurves.GraphIntensityMultiplier;
                        lightCurves.canUpdate = false;
                    }
                    else
                    {
                        lights[i].intensity = alpha;
                    }
                }
            }
        }

        if (IsActive && !previousActiveStatus)
        {
            var allGO = gameObject.GetComponentsInChildren<Transform>();

            for (int i = 0; i < allGO.Length; ++i)
            {
                allGO[i].gameObject.SetActive(false);
                allGO[i].gameObject.SetActive(true);
            }


        }

        previousActiveStatus = IsActive;
    }
}