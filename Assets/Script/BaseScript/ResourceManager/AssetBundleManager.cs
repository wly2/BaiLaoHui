using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleManager : MonoSingleton<AssetBundleManager>
{
    public AssetBundleLoadMode LoadMode;



}
public enum AssetBundleLoadMode
{
    LoadFromFile,
    LoadFromFileAsync,
    LoadFromWWW
}
