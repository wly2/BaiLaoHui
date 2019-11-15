using System;
using UnityEngine;

public static class RFX4_ColorHelper
{
    const float TOLERANCE = 0.0001f;

    static readonly string[] colorProperties =
    {
        "_TintColor", "_Color", "_EmissionColor", "_BorderColor", "_ReflectColor", "_RimColor", "_MainColor",
        "_CoreColor"
    };

    public struct HSBColor
    {
        public float H;
        public float S;
        public float B;
        public float A;

        public HSBColor(float h, float s, float b, float a)
        {
            H = h;
            S = s;
            B = b;
            A = a;
        }
    }

    public static HSBColor ColorToHSV(Color color)
    {
        HSBColor ret = new HSBColor(0f, 0f, 0f, color.a);

        float r = color.r;
        float g = color.g;
        float b = color.b;

        float max = Mathf.Max(r, Mathf.Max(g, b));

        if (max <= 0)
            return ret;

        float min = Mathf.Min(r, Mathf.Min(g, b));
        float dif = max - min;

        if (max > min)
        {
            if (Math.Abs(g - max) < TOLERANCE)
                ret.H = (b - r) / dif * 60f + 120f;
            else if (Math.Abs(b - max) < TOLERANCE)
                ret.H = (r - g) / dif * 60f + 240f;
            else if (b > g)
                ret.H = (g - b) / dif * 60f + 360f;
            else
                ret.H = (g - b) / dif * 60f;
            if (ret.H < 0)
                ret.H = ret.H + 360f;
        }
        else
            ret.H = 0;

        ret.H *= 1f / 360f;
        ret.S = (dif / max) * 1f;
        ret.B = max;

        return ret;
    }

    public static Color HSVToColor(HSBColor hsbColor)
    {
        float r = hsbColor.B;
        float g = hsbColor.B;
        float b = hsbColor.B;
        if (Math.Abs(hsbColor.S) > TOLERANCE)
        {
            float max = hsbColor.B;
            float dif = hsbColor.B * hsbColor.S;
            float min = hsbColor.B - dif;

            float h = hsbColor.H * 360f;

            if (h < 60f)
            {
                r = max;
                g = h * dif / 60f + min;
                b = min;
            }
            else if (h < 120f)
            {
                r = -(h - 120f) * dif / 60f + min;
                g = max;
                b = min;
            }
            else if (h < 180f)
            {
                r = min;
                g = max;
                b = (h - 120f) * dif / 60f + min;
            }
            else if (h < 240f)
            {
                r = min;
                g = -(h - 240f) * dif / 60f + min;
                b = max;
            }
            else if (h < 300f)
            {
                r = (h - 240f) * dif / 60f + min;
                g = min;
                b = max;
            }
            else if (h <= 360f)
            {
                r = max;
                g = min;
                b = -(h - 360f) * dif / 60 + min;
            }
            else
            {
                r = 0;
                g = 0;
                b = 0;
            }
        }

        return new Color(Mathf.Clamp01(r), Mathf.Clamp01(g), Mathf.Clamp01(b), hsbColor.A);
    }

    public static Color ConvertRGBColorByHUE(Color rgbColor, float hue)
    {
        var brightness = ColorToHSV(rgbColor).B;
        if (brightness < TOLERANCE)
            brightness = TOLERANCE;
        var hsv = ColorToHSV(rgbColor / brightness);
        hsv.H = hue;
        var color = HSVToColor(hsv) * brightness;
        color.a = rgbColor.a;
        return color;
    }

    public static void ChangeObjectColorByHUE(GameObject go, float hue)
    {
        var renderers = go.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; ++i)
        {
            var mat = renderers[i].material;
            if (mat == null)
                continue;
            for (int j = 0; j < colorProperties.Length; ++j)
            {
                if (mat.HasProperty(colorProperties[j]))
                {
                    SetMatHUEColor(mat, colorProperties[j], hue);
                }
            }
        }

        var skinRenderers = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        for (int i = 0; i < skinRenderers.Length; ++i)
        {
            var mat = skinRenderers[i].material;
            if (mat == null)
                continue;
            for (int j = 0; j < colorProperties.Length; ++j)
            {
                if (mat.HasProperty(colorProperties[j]))
                {
                    SetMatHUEColor(mat, colorProperties[j], hue);
                }
            }
        }

        var projectors = go.GetComponentsInChildren<Projector>(true);
        for (int i = 0; i < projectors.Length; ++i)
        {
            if (!projectors[i].material.name.EndsWith("(Instance)"))
                projectors[i].material =
                    new Material(projectors[i].material) {name = projectors[i].material.name + " (Instance)"};
            var mat = projectors[i].material;
            if (mat == null)
                continue;
            for (int j = 0; j < colorProperties.Length; ++j)
            {
                if (mat.HasProperty(colorProperties[j]))
                {
                    projectors[i].material = SetMatHUEColor(mat, colorProperties[j], hue);
                }
            }
        }

        var lights = go.GetComponentsInChildren<Light>(true);
        for (int i = 0; i < lights.Length; ++i)
        {
            var hsv = ColorToHSV(lights[i].color);
            hsv.H = hue;
            lights[i].color = HSVToColor(hsv);
        }

        var particles = go.GetComponentsInChildren<ParticleSystem>(true);
        for (int i = 0; i < particles.Length; ++i)
        {
#if !UNITY_5_5_OR_NEWER
            var hsv = ColorToHSV(ps.startColor);
            hsv.H = hue;
            ps.startColor = HSVToColor(hsv);
#else
            var main = particles[i].main;
            var hsv = ColorToHSV(particles[i].main.startColor.color);
            hsv.H = hue;
            main.startColor = HSVToColor(hsv);
#endif
        }

        var rfx4_trails = go.GetComponentsInChildren<RFX4_ParticleTrail>(true);
        for (int i = 0; i < rfx4_trails.Length; ++i)
        {
            var mat = rfx4_trails[i].TrailMaterial;
            if (mat == null)
                continue;
            mat = new Material(rfx4_trails[i].TrailMaterial);
            rfx4_trails[i].TrailMaterial = mat;

            for (int j = 0; j < colorProperties.Length; ++j)
            {
                if (mat.HasProperty(colorProperties[j]))
                {
                    SetMatHUEColor(mat, colorProperties[j], hue);
                }
            }
        }

        var rfx4_shaderColorGradients = go.GetComponentsInChildren<RFX4_ShaderColorGradient>(true);

        for (int i = 0; i < rfx4_shaderColorGradients.Length; ++i)
        {
            rfx4_shaderColorGradients[i].HUE = hue;
        }


    }

    static Material SetMatHUEColor(Material mat, String name, float hueColor)
    {
        var oldColor = mat.GetColor(name);
        var color = ConvertRGBColorByHUE(oldColor, hueColor);
        mat.SetColor(name, color);
        return mat;
    }

    static Material SetMatAlphaColor(Material mat, String name, float alpha)
    {
        var oldColor = mat.GetColor(name);
        oldColor.a = alpha;
        mat.SetColor(name, oldColor);
        return mat;
    }
}