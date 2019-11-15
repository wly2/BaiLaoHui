// ----------------------------------------------------------------------------------
//
// FXMaker
// Created by ismoon - 2012 - ismoonto@gmail.com
//
// ----------------------------------------------------------------------------------

using UnityEngine;
using System.Collections.Generic;

public class NsEffectManager : MonoBehaviour
{
    // Attribute ------------------------------------------------------------------------
    // Property -------------------------------------------------------------------------
    // Control Function -----------------------------------------------------------------
    //
    public static Texture[] PreloadResource(GameObject tarObj)
    {
        if (tarObj == null)
            return new Texture[0];
        List<GameObject> parentPrefabList = new List<GameObject>
        {
            tarObj
        };
        return PreloadResource(tarObj, parentPrefabList);
    }

    public static Component GetComponentInChildren(GameObject tarObj, System.Type findType)
    {
        if (tarObj == null)
            return null;
        List<GameObject> parentPrefabList = new List<GameObject>
        {
            tarObj
        };
        return GetComponentInChildren(tarObj, findType, parentPrefabList);
    }

    // Replay Function
    public static GameObject CreateReplayEffect(GameObject tarPrefab)
    {
        if (tarPrefab == null)
            return null;
        if (!NcEffectBehaviour.IsSafe())
            return null;
        GameObject instanceObj = Instantiate(tarPrefab);
        SetReplayEffect(instanceObj);
        return instanceObj;
    }

    public static void SetReplayEffect(GameObject instanceObj)
    {
        PreloadResource(instanceObj);
        SetActiveRecursively(instanceObj, false);

        NcEffectBehaviour[] ncComs = instanceObj.GetComponentsInChildren<NcEffectBehaviour>(true);
        for (int i = 0; i < ncComs.Length; ++i)
        {
            ncComs[i].OnSetReplayState();
        }
    }

    public static void RunReplayEffect(GameObject instanceObj, bool bClearOldParticle)
    {
        SetActiveRecursively(instanceObj, true);

        NcEffectBehaviour[] ncComs = instanceObj.GetComponentsInChildren<NcEffectBehaviour>(true);
        for (int i = 0; i < ncComs.Length; ++i)
        {
            ncComs[i].OnResetReplayStage(bClearOldParticle);
        }
    }

    // ----------------------------------------------------------------------------------
#if UNITY_EDITOR
    public static void AdjustSpeedEditor(GameObject target, float fSpeedRate) // support shuriken, legancy, mesh
    {
        NcEffectBehaviour[] coms = target.GetComponentsInChildren<NcEffectBehaviour>(true);
        for (int i = 0; i < coms.Length; ++i)
        {
            coms[i].OnUpdateEffectSpeed(fSpeedRate, false);
        }
    }
#endif
    public static void
        AdjustSpeedRuntime(GameObject target, float fSpeedRate) // support legancy/mesh , not support shuriken
    {
        NcEffectBehaviour[] coms = target.GetComponentsInChildren<NcEffectBehaviour>(true);
        for (int i = 0; i < coms.Length; ++i)
        {
            coms[i].OnUpdateEffectSpeed(fSpeedRate, true);
        }
    }

    // ----------------------------------------------------------------------------------
    public static void SetActiveRecursively(GameObject target, bool bActive)
    {
#if (!UNITY_3_5)
        for (int n = target.transform.childCount - 1; 0 <= n; n--)
            if (n < target.transform.childCount)
                SetActiveRecursively(target.transform.GetChild(n).gameObject, bActive);
        target.SetActive(bActive);
#else
		target.SetActiveRecursively(bActive);
#endif
        // 		SetActiveRecursivelyEffect(target, bActive);
    }

    public static bool IsActive(GameObject target)
    {
#if (!UNITY_3_5)
        return (target.activeInHierarchy && target.activeSelf);
#else
		return target.active;
#endif
    }

    protected static void SetActiveRecursivelyEffect(GameObject target, bool bActive)
    {
        NcEffectBehaviour[] coms = target.GetComponentsInChildren<NcEffectBehaviour>(true);
        for (int i = 0; i < coms.Length; ++i)
        {
            coms[i].OnSetActiveRecursively(bActive);
        }
    }

    // ----------------------------------------------------------------------------------
    protected static Texture[] PreloadResource(GameObject tarObj, List<GameObject> parentPrefabList)
    {
        if (!NcEffectBehaviour.IsSafe())
            return null;
        // texture
        Renderer[] rens = tarObj.GetComponentsInChildren<Renderer>(true);
        List<Texture> texList = new List<Texture>();
        for (int i = 0; i < rens.Length; ++i)
        {
            if (rens[i].sharedMaterials == null || rens[i].sharedMaterials.Length <= 0)
                continue;
            for (int j = 0; j < rens[i].sharedMaterials.Length; ++j)
            {
                if (rens[i].sharedMaterials[j] != null && rens[i].sharedMaterials[j].mainTexture != null)
                    texList.Add(rens[i].sharedMaterials[j].mainTexture);
            }
        }

        // prefab
        NcAttachPrefab[] prefabs = tarObj.GetComponentsInChildren<NcAttachPrefab>(true);
        for (int i = 0; i < prefabs.Length; ++i)
        {
            if (prefabs[i].m_AttachPrefab != null)
            {
                Texture[] ret = PreloadPrefab(prefabs[i].m_AttachPrefab, parentPrefabList, true);
                if (ret == null)
                    prefabs[i].m_AttachPrefab = null; // clear
                else texList.AddRange(ret);
            }
        }

        NcParticleSystem[] pss = tarObj.GetComponentsInChildren<NcParticleSystem>(true);
        for (int i = 0; i < pss.Length; ++i)
        {
            if (pss[i].m_AttachPrefab != null)
            {
                Texture[] ret = PreloadPrefab(pss[i].m_AttachPrefab, parentPrefabList, true);
                if (ret == null)
                    pss[i].m_AttachPrefab = null; // clear
                else texList.AddRange(ret);
            }
        }

        NcSpriteTexture[] sts = tarObj.GetComponentsInChildren<NcSpriteTexture>(true);
        for (int i = 0; i < sts.Length; ++i)
        {
            if (sts[i].m_NcSpriteFactoryPrefab != null)
            {
                Texture[] ret = PreloadPrefab(sts[i].m_NcSpriteFactoryPrefab, parentPrefabList, false);
                if (ret != null)
                    texList.AddRange(ret);
            }
        }

        NcParticleSpiral[] sps = tarObj.GetComponentsInChildren<NcParticleSpiral>(true);
        for (int i = 0; i < sps.Length; ++i)
        {
            if (sps[i].m_ParticlePrefab != null)
            {
                Texture[] ret = PreloadPrefab(sps[i].m_ParticlePrefab, parentPrefabList, false);
                if (ret != null)
                    texList.AddRange(ret);
            }
        }

        NcParticleEmit[] ses = tarObj.GetComponentsInChildren<NcParticleEmit>(true);
        for (int i = 0; i < ses.Length; ++i)
        {
            if (ses[i].m_ParticlePrefab != null)
            {
                Texture[] ret = PreloadPrefab(ses[i].m_ParticlePrefab, parentPrefabList, false);
                if (ret != null)
                    texList.AddRange(ret);
            }
        }

        NcAttachSound[] ass = tarObj.GetComponentsInChildren<NcAttachSound>(true);
        for (int i = 0; i < ass.Length; ++i)
        {
            if (ass[i].m_AudioClip != null)
                continue;
        }

        NcSpriteFactory[] sprites = tarObj.GetComponentsInChildren<NcSpriteFactory>(true);
        for (int i = 0; i < sprites.Length; ++i)
        {
            if (sprites[i].m_SpriteList != null)
                for (int n = 0; n < sprites[i].m_SpriteList.Count; n++)
                    if (sprites[i].m_SpriteList[n].m_EffectPrefab != null)
                    {
                        Texture[] ret = PreloadPrefab(sprites[i].m_SpriteList[n].m_EffectPrefab, parentPrefabList,
                            true);
                        if (ret == null)
                            sprites[i].m_SpriteList[n].m_EffectPrefab = null; // clear
                        else texList.AddRange(ret);
                        if (sprites[i].m_SpriteList[n].m_AudioClip != null)
                            continue;
                    }
        }

        return texList.ToArray();
    }

    protected static Texture[] PreloadPrefab(GameObject tarObj, List<GameObject> parentPrefabList, bool bCheckDup)
    {
        if (parentPrefabList.Contains(tarObj))
        {
            if (bCheckDup)
            {
                string str = "";
                for (int n = 0; n < parentPrefabList.Count; n++)
                    str += parentPrefabList[n].name + "/";
                Debug.LogWarning("LoadError : Recursive Prefab - " + str + tarObj.name);
                return null; // error
            }
            else return null;
        }

        parentPrefabList.Add(tarObj);
        Texture[] ret = PreloadResource(tarObj, parentPrefabList);
        parentPrefabList.Remove(tarObj);
        return ret;
    }

    protected static Component GetComponentInChildren(GameObject tarObj, System.Type findType,
        List<GameObject> parentPrefabList)
    {
        Component[] coms = tarObj.GetComponentsInChildren(findType, true);
        for (int i = 0; i < coms.Length; ++i)
        {
            if (coms[i].GetComponent<NcDontActive>() == null)
                return coms[i];
        }

        Component findCom;
        NcAttachPrefab[] prefabs = tarObj.GetComponentsInChildren<NcAttachPrefab>(true);
        for (int i = 0; i < prefabs.Length; ++i)
        {
            if (prefabs[i].m_AttachPrefab != null)
            {
                findCom = GetValidComponentInChildren(prefabs[i].m_AttachPrefab, findType, parentPrefabList, true);
                if (findCom != null)
                    return findCom;
            }
        }

        NcParticleSystem[] pss = tarObj.GetComponentsInChildren<NcParticleSystem>(true);
        for (int i = 0; i < pss.Length; ++i)
        {
            if (pss[i].m_AttachPrefab != null)
            {
                findCom = GetValidComponentInChildren(pss[i].m_AttachPrefab, findType, parentPrefabList, true);
                if (findCom != null)
                    return findCom;
            }
        }

        NcSpriteTexture[] sts = tarObj.GetComponentsInChildren<NcSpriteTexture>(true);
        for (int i = 0; i < sts.Length; ++i)
        {
            if (sts[i].m_NcSpriteFactoryPrefab != null)
            {
                findCom = GetValidComponentInChildren(sts[i].m_NcSpriteFactoryPrefab, findType, parentPrefabList,
                    false);
                if (findCom != null)
                    return findCom;
            }
        }

        NcParticleSpiral[] sps = tarObj.GetComponentsInChildren<NcParticleSpiral>(true);
        for (int i = 0; i < sps.Length; ++i)
        {
            if (sps[i].m_ParticlePrefab != null)
            {
                findCom = GetValidComponentInChildren(sps[i].m_ParticlePrefab, findType, parentPrefabList, false);
                if (findCom != null)
                    return findCom;
            }
        }

        NcParticleEmit[] ses = tarObj.GetComponentsInChildren<NcParticleEmit>(true);
        for (int i = 0; i < ses.Length; ++i)
        {
            if (ses[i].m_ParticlePrefab != null)
            {
                findCom = GetValidComponentInChildren(ses[i].m_ParticlePrefab, findType, parentPrefabList, false);
                if (findCom != null)
                    return findCom;
            }
        }

        NcSpriteFactory[] sprites = tarObj.GetComponentsInChildren<NcSpriteFactory>(true);
        for (int i = 0; i < sprites.Length; ++i)
        {
            if (sprites[i].m_SpriteList != null)
                for (int n = 0; n < sprites[i].m_SpriteList.Count; n++)
                    if (sprites[i].m_SpriteList[n].m_EffectPrefab != null)
                    {
                        findCom = GetValidComponentInChildren(sprites[i].m_SpriteList[n].m_EffectPrefab, findType,
                            parentPrefabList, true);
                        if (findCom != null)
                            return findCom;
                    }
        }

        return null;
    }

    protected static Component GetValidComponentInChildren(GameObject tarObj, System.Type findType,
        List<GameObject> parentPrefabList, bool bCheckDup)
    {
        if (parentPrefabList.Contains(tarObj))
        {
            if (bCheckDup)
            {
                string str = "";
                for (int n = 0; n < parentPrefabList.Count; n++)
                    str += parentPrefabList[n].name + "/";
                Debug.LogWarning("LoadError : Recursive Prefab - " + str + tarObj.name);
                return null; // error
            }
            else return null;
        }

        parentPrefabList.Add(tarObj);
        Component com = GetComponentInChildren(tarObj, findType, parentPrefabList);
        parentPrefabList.Remove(tarObj);
        return com;
    }
}