using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using XLua;

[Hotfix]
public class ResourcesLoader : MonoSingleton<ResourcesLoader>
{
    public bool LoadOriginalAssets = false;
    //表示所有已经加载的 bundles 的集合<AssetName,AssetBundle>
    Dictionary<string, AssetBundle> PreloadAssetBundles = new Dictionary<string, AssetBundle>();
    Dictionary<string, CachedObjectInfo> cached_objects = new Dictionary<string, CachedObjectInfo>();
    Dictionary<string, Object> preload_cached_set = new Dictionary<string, Object>();
    private static AssetBundleManifest manifest = null;
    private string bundlePath
    {
        get
        {
            if (AssetBundleManager.instance.LoadMode == AssetBundleLoadMode.LoadFromFileAsync)
                return AssetBundleConfig.ASSETBUNDLE_PATH_LOCSTREAM;
            return AssetBundleConfig.ASSETBUNDLE_PATH_PERSIS;
        }
    }

  //  private string loadPath = "E:/Game/3D/Assets";
    //{
    //    get
    //    {
    //        if (AssetBundleManager.instance.LoadMode == AssetBundleLoadMode.LoadFromFileAsync)
    //            return AssetBundleConfig.ASSETBUNDLE_PATH_E;
    //            return AssetBundleConfig.ASSETBUNDLE_PATH_PERSIS;
    //    }
    //}

    public static void Load<T>(string resource, UnityAction<T> onLoaded, bool localCache = true) where T : Object
    {
        if (string.IsNullOrEmpty(resource))
        {
            MyDebug.LogError("Passed NULL resource name in ResourcesLoader.Load()");
            return;
        }
     //   MyDebug.Log("Load");
        ResourcesLoader.instance.LoadAsset<T>(resource, onLoaded, localCache);
    }

    protected void LoadAsset<T>(string resource, UnityAction<T> onLoaded, bool localCache) where T : Object
    {
#if UNITY_EDITOR
        if (this.LoadOriginalAssets)
        {
            string path = "Assets/BuildOnlyAssets/" + resource + ".prefab";
            if (File.Exists(path))
            {
                T obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null)
                {
                    onLoaded(obj);
                    MyDebug.Log("LoadFromFile");
                    return;
                }
            }
               path = "Assets/BuildOnlyAssets/" + resource + ".png";
            if (File.Exists(path))
            {
                T obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(path);
                if (obj != null)
                {
                    onLoaded(obj);
                   // MyDebug.Log("LoadFromFile");
                    return;
                }
            }
        }
#endif
      //  MyDebug.Log("LoadFromBundle");
        UnityAction<AssetBundle> action = new UnityAction<AssetBundle>((bundle) =>
        {
            if (bundle != null)
            {
                // PreloadAssetBundles[resource] = bundle;
                string resname = ParseResourceName(resource);
                T obj = bundle.LoadAsset(resname, typeof(T)) as T;

                if (obj == null)
                {
                    MyDebug.LogError("Load Asset Failed from: " + resource + ", AssetName: " + resname);
                }
                onLoaded(obj);

                if (localCache)
                {
                    pushCacheObject(resource, obj);
                }
            }
        });


        Object cobj = getCachedObject(resource);
        if (cobj != null)
        {
            onLoaded(cobj as T);
           // MyDebug.Log("LoadFromCached");
            return;
        }

        AssetBundle preloaded = this.GetPreloadBundle(resource);
        if (preloaded != null)
        {
         //   MyDebug.Log("LoadFromPreload");
            action(preloaded);
        }
        else
        {
            LoadBundle(resource, action);
        }

    }

    private AssetBundle GetPreloadBundle(string resource)
    {
        if (string.IsNullOrEmpty(resource))
        {
            return null;
        }

        string bundleKey = resource.ToLower() + ".unity3d";
        if (this.PreloadAssetBundles.ContainsKey(bundleKey))
        {
            if (this.PreloadAssetBundles[bundleKey] == null)
            {
                this.PreloadAssetBundles.Remove(bundleKey);
            }
            else
            {
                return this.PreloadAssetBundles[bundleKey];
            }
        }
        return null;
    }
    private string ParseResourceName(string resName)
    {
        resName = resName.ToLower();
        int lastSlashIndex = resName.LastIndexOf("/");
        string res = resName.Substring(lastSlashIndex + 1);
        return res;
    }
    public bool isDone
    {
        get
        {
            return this.LoadingBundles.Count == 0;
        }
    }
    protected void LoadBundle(string abPath, UnityAction<AssetBundle> onloaded, bool unloadAfterUsed = true)
    {
        MyDebug.Log(abPath);
        string resPath = abPath + ".unity3d";
        onloaded(LoadAB(resPath.ToLower()));
    }
    protected AssetBundle LoadAB(string abPath)
    {

        MyDebug.Log(bundlePath + abPath);

        if (manifest == null)
        {
            AssetBundle manifestBundle = AssetBundle.LoadFromFile(bundlePath +
            AssetBundleConfig.ASSETBUNDLE_FILENAM);
            manifest = (AssetBundleManifest)manifestBundle.LoadAsset("AssetBundleManifest");
        }
        if (manifest != null)
        {
            // 2.获取依赖文件列表  
            string[] cubedepends = manifest.GetAllDependencies(abPath);

            for (int index = 0; index < cubedepends.Length; index++)
            {
                // 3.加载所有的依赖资源
                if (!PreloadAssetBundles.ContainsKey(cubedepends[index]))
                    LoadAB(cubedepends[index]);
            }
            // 4.加载资源
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath + abPath);//bundlePath
            PreloadAssetBundles[abPath] = bundle;
            return bundle;
        }
        return null;
    }
    List<string> LoadingBundles = new List<string>();
    private void pushCacheObject(string key, Object obj)
    {
        if (obj == null)
            return;

        if (isPreloadingCachedMode)
        {
            preload_cached_set[key] = obj;
            return;
        }

        if (cached_objects.ContainsKey(key))
        {
            cached_objects[key].cacheTime = Time.realtimeSinceStartup;
            cached_objects[key].cacheObj = obj;
        }
        else
        {
            CachedObjectInfo coi = new CachedObjectInfo();
            coi.cacheObj = obj;
            coi.cacheTime = Time.realtimeSinceStartup;
            cached_objects.Add(key, coi);
        }
    }
    class CachedObjectInfo
    {
        public Object cacheObj;
        public float cacheTime;
    }
    private bool isPreloadingCachedMode = false;

    public void setPreloadingCachedMode(bool v)
    {
        isPreloadingCachedMode = v;
    }
    private Object getCachedObject(string key)
    {
        if (string.IsNullOrEmpty(key))
        {
            return null;
        }
        if (preload_cached_set.ContainsKey(key))
        {
            return preload_cached_set[key];
        }

        if (cached_objects.ContainsKey(key))
        {
            if (cached_objects[key].cacheObj == null)
            {
                cached_objects.Remove(key);
            }
            else
            {
                cached_objects[key].cacheTime = Time.realtimeSinceStartup;
                return cached_objects[key].cacheObj;
            }
        }
        return null;
    }
    public void testC()
    {
        MyDebug.Log("C#");

    }
}
