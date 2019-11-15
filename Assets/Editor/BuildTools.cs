using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildTools : EditorWindow
{
    [MenuItem("BuildAsset/Build Tools 5.x")]
    static void ShowBuildTool()
    {
        //创建窗口
        Rect rc = new Rect(0, 0, 500, 500);
        BuildTools window = (BuildTools) GetWindow(typeof(BuildTools), true, "Build Tool 5.x");
        window.Show();
    }

    private int selectionGridInt;
    string[] selectionStrings;
    BuildCheckBox box = new BuildCheckBox();
    Vector2 scrollPosition;

    void OnGUI()
    {
        //box.buildAll = !EditorGUILayout.BeginToggleGroup(box.buildAll ? "Build All" : "Build Selected", !box.buildAll);
        //box.bAssetBundles = EditorGUILayout.Toggle("Build AssetBundle", box.bAssetBundles);
        //box.bData = EditorGUILayout.Toggle("Build Data", box.bData);
        //box.bLanguage = EditorGUILayout.Toggle("Build Language", box.bLanguage);
        ////box.bUI = EditorGUILayout.Toggle("Build UI", box.bUI);
        ////box.bScene = EditorGUILayout.Toggle("Build Scene", box.bScene);
        //GUILayout.Space(20f);
        //box.bStory = EditorGUILayout.Toggle("Build Story Res", box.bStory);
        //box.bStoryData = EditorGUILayout.Toggle("Build Story Data", box.bStoryData);
        //EditorGUILayout.EndToggleGroup();
        //GUILayout.Space(20f);
        //box.enableCache = EditorGUILayout.Toggle("Use Cache", box.enableCache);
        //GUILayout.Space(20f);
        //box.bPlayer = EditorGUILayout.Toggle("Build Player", box.bPlayer);
        //GUILayout.Space(20f);
        //box.bCompressed = EditorGUILayout.Toggle("Compress AssetBundle", box.bCompressed);
        if (GUILayout.Button("Process Bundle Name", GUILayout.Width(200)))
        {
            ProcessBundleName();
        }
        if (GUILayout.Button("Clear Bundle Name", GUILayout.Width(200)))
        {                      
            string[] files = System.IO.Directory.GetFiles("Assets/", "*.*", SearchOption.AllDirectories);
            if ((files != null) && (files.Length > 0))
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    string fname = files[i].Replace("\\", "/");
                    if (fname.Contains(".meta") || fname.Contains("VersionMD5") 
                        || fname.Contains(".xml") || fname.Contains(".unity3d") 
                        || fname.Contains(".zip") || fname.Contains(".cs")
                        || fname.Contains(".js"))
                        continue;
                    AssetImporter importer = AssetImporter.GetAtPath(fname);
                    Debug.Log( fname);
                    if (importer != null)
                    {
                        Debug.Log("*******************************************"+ fname);
                        importer.assetBundleName = string.Empty;
                    }
                }
            }
            AssetDatabase.RemoveUnusedAssetBundleNames();
        }
        if (GUILayout.Button("Build IOS", GUILayout.Width(200)))
        {
            BuildTarget target = BuildTarget.iOS;
            Build(box, target, box.bCompressed);
        }

        if (GUILayout.Button("Build Android", GUILayout.Width(200)))
        {
            BuildTarget target = BuildTarget.Android;
            Build(box, target, box.bCompressed, true);
        }

        //if (GUILayout.Button("Build Windows", GUILayout.Width(200)))
        //{
        //    BuildTarget target = BuildTarget.StandaloneWindows;
        //    Build(box, target, box.bCompressed, true);
        //}

       

        //if (GUILayout.Button("Build Current Targets", GUILayout.Width(200)))
        //{
        //    BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        //    //  Build(box, target, box.bCompressed, false);
        //}
        if (GUILayout.Button("CreateMD5", GUILayout.Width(200)))
        {
            AssetbundleHelper.CreatMD5XML();
            //  Build(box, target, box.bCompressed, false);

        }
        if (GUILayout.Button("CompressUnity3D", GUILayout.Width(200)))
        {
            AssetbundleHelper.Compressbundle();
            //CreatMD5.CreatMD5XML();
            //  Build(box, target, box.bCompressed, false);
        }
        //if (GUILayout.Button("UnCompressUnity3D", GUILayout.Width(200)))
        //{
        //    AssetbundleHelper.DecompressFileLZMA(Application.persistentDataPath + "/Zip/StreamingAssets/baseassets/texture/ui/game/genzhuang.unity3d.zip", Application.persistentDataPath+ "/StreamingAssets/baseassets/texture/ui/game/genzhuang.unity3d");
        //    //CreatMD5.CreatMD5XML();
        //    //  Build(box, target, box.bCompressed, false);
        //}
        if (GUILayout.Button("Modify Particle Material", GUILayout.Width(200)))
        {
            string[] files = Directory.GetFiles("Assets/FX/Materials");
            for (int i = 0; i < files.Length; ++i)
            {
                Material mat = (Material) AssetDatabase.LoadMainAssetAtPath(files[i]);
                if (mat != null)
                {
                    switch (mat.shader.name)
                    {
                        case "Particles/Additive":
                            mat.shader = Shader.Find("Mobile/Particles/Additive");
                            break;
                        case "Particles/Alpha Blended":
                            mat.shader = Shader.Find("Mobile/Particles/Alpha Blended");
                            break;
                        case "Particles/Multiply":
                            mat.shader = Shader.Find("Mobile/Particles/Multiply");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        if (selectionStrings != null)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUIStyle.none);
            selectionGridInt = GUILayout.SelectionGrid(selectionGridInt, selectionStrings, 1);
            if (selectionGridInt > 0)
                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(selectionStrings[selectionGridInt]);
            GUILayout.EndScrollView();
        }
    }

    public static void ProcessBundleName()
    {
        Caching.ClearCache();
        Dictionary<string, int> refcountmap = new Dictionary<string, int>();
        Dictionary<string, string> pe = new Dictionary<string, string>();
        //List<string> prefabs = new List<string>(Directory.GetFiles("Assets/BuildOnlyAssets/Units/", "*.prefab", SearchOption.AllDirectories));
        //foreach (string keyx in prefabs)
        //{
        //    AssetImporter importer = AssetImporter.GetAtPath(keyx);
        //    string name = Path.GetFileNameWithoutExtension(keyx);
        //    if (name.IndexOf('_') > 0)
        //        name = name.Substring(0, name.IndexOf('_'));
        //    importer.SetAssetBundleNameAndVariant("units/" + name + ".assets", "");
        //}
         //SetAssetBundleSingleName("Assets/FX/Materials/", "*.mat", "fx/fx_materials.assets", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BaseAssets/Lua/", "*.txt", SearchOption.AllDirectories);
        // SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BaseAssets/UI_Online/Game/UIPanel_ExitGame/", "*.png", SearchOption.AllDirectories);
        // SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BaseAssets/UI_Online/Game/UIPanel_ResultsTheDetails/", "*.png", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BaseAssets/UI_Online/Game/UIPanel_Setting/", "*.png", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BaseAssets/UI_Online/Game/UIPanel_Share/", "*.png", SearchOption.AllDirectories); 
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/PuKe/", "*.png", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BaseAssets/", "*.png", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BaseAssets/Prefab/UI/", "*.prefab", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/NiuNiuAssets/", "*.png", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/NiuNiuAssets/Prefab/UI/", "*.prefab", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/ShiSanShuiAssets/", "*.png", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/ShiSanShuiAssets/Prefab/UI/", "*.prefab", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/ShareAssets/", "*.png", SearchOption.AllDirectories);
        SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/ShareAssets/Prefabs/", "*.prefab", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BaseAssets/UI_Online/Hall/UIPanel_CreateRoom/", "*.png", SearchOption.AllDirectories);





        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/MajiangAssets/Materal/", "*.mat", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/MajiangAssets/Texture/", "*.png", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/MajiangAssets/Prefab/", "*.prefab", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/MajiangAssets/Shader/", "*.shader", SearchOption.AllDirectories);



        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/Sound/BGM/", "*.wav", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/Sound/Chinese_VO/", "*.wav", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/Sound/combat/", "*.wav", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/Sound/English_VO/", "*.wav", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/Sound/UI/", "*.wav", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/UI/Common/", "*.png", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/UI/Icons/", "*.png", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/UI/LangIcons/", "*.png", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/UI/LevelBg/", "*.png", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/UI/Affiche/", "*.png", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/BattlePoint/", "*.prefab", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/Camera/", "*.prefab", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/CameraPath/", "*.prefab", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/Guide/", "*.prefab", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/Talk/", "*.prefab", SearchOption.AllDirectories);
        //SetAssetBundleNameAtPath("Assets/BuildOnlyAssets/TerrainElement/", "*.prefab", SearchOption.AllDirectories);
        EditorUtility.DisplayDialog("", "AssetBundle Process Name Done", "OK");
    }

    public static void SetAssetBundleNameAtPath(string pathname, string searchPattern, SearchOption searchOpt)
    {
        string targetPath = null;
        List<string> files = new List<string>(Directory.GetFiles(pathname, searchPattern, SearchOption.AllDirectories));
        for (int i = 0; i < files.Count; ++i)
        {
            string fname = files[i].Replace("\\", "/");
            AssetImporter importer = AssetImporter.GetAtPath(fname);
            string name = Path.GetFileNameWithoutExtension(fname);
            targetPath = fname.Replace("Assets/BuildOnlyAssets/", "");
            targetPath = targetPath.Substring(0, targetPath.LastIndexOf(name));
            if (!targetPath.EndsWith("/"))
            {
                targetPath += "/";
            }

            importer.SetAssetBundleNameAndVariant(targetPath + name + ".unity3d", "");
        }
    }

    private static void SetAssetBundleSingleName(string pathname, string searchPattern, string targetName,
        SearchOption searchOpt)
    {
        List<string> assets =
            new List<string>(Directory.GetFiles(pathname, searchPattern, SearchOption.AllDirectories));
        for (int i = 0; i < assets.Count; ++i)
        {
            AssetImporter importer = AssetImporter.GetAtPath(assets[i]);

            importer.SetAssetBundleNameAndVariant(targetName, "");
        }
    }

    public static void Build(BuildCheckBox box, BuildTarget target = BuildTarget.iOS, bool compressed = false,
        bool external = true, bool isHD = false)
    {
        Debug.Log("************Build for " + target.ToString());

        BuildAssetBundleOptions buildOp = BuildAssetBundleOptions.UncompressedAssetBundle;
        //if (!box.enableCache)
        //    buildOp |= BuildAssetBundleOptions.ForceRebuildAssetBundle;
        //if (!compressed)
        //    buildOp |= BuildAssetBundleOptions.UncompressedAssetBundle;
        Debug.Log(Application.persistentDataPath);
        string StreamingAssetsPath = "Assets/StreamingAssets/";
        //if (!external)
        //{
        //    StreamingAssetsPath = "Assets/StreamingAssets/";
        //}
        //else
        //{
        //    if (target == BuildTarget.Android)
        //    {

        //        StreamingAssetsPath = isHD ? "StreamingAssetsAndroidHD/" : "Assets/StreamingAssets/";
        //    }
        //    else if (target == BuildTarget.iOS)
        //    {
        //        StreamingAssetsPath = isHD ? "StreamingAssetsIosHD/" : "Assets/StreamingAssets/";
        //    }
        //    else if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64)
        //    {
        //        StreamingAssetsPath = "StreamingAssetsWin/";
        //    }
        //}

        //AssetBuilder5 builder = new AssetBuilder5(StreamingAssetsPath, buildOp, target, box.enableCache);
        //        AssetBuilder builder = ((target == BuildTarget.Android) && external) ? new AssetBuilder("StreamingAssetsAndroid/", buildOp, target) : new AssetBuilder("Assets/StreamingAssets/", buildOp, target);
        //Caching.CleanCache();
        //BuildPipeline.PushAssetDependencies();
        //Object sharedAsset = AssetDatabase.LoadMainAssetAtPath("Assets/Units/C001.prefab");
        //BuildPipeline.BuildAssetBundle(sharedAsset, null, SavePath + sharedAsset.name + ".assetbundle", buildOp, BuildTarget.StandaloneWindows);
        //BuildPipeline.PopAssetDependencies();
        //if (box.buildAll || box.bAssetBundles)
        //{
        //    BuildPipeline.BuildAssetBundles(StreamingAssetsPath, buildOp, target);
        //}
        BuildPipeline.BuildAssetBundles(StreamingAssetsPath, buildOp, target);
        ////builder.Finish();
        //AssetBuilder builder = new AssetBuilder(StreamingAssetsPath, buildOp, target, box.enableCache);
        //if (box.buildAll || box.bStoryData)
        //{
        //    builder.BuildStoryFileAtPath("Assets/BuildOnlyAssets/LevelSettings/", "*.txt", "LevelSettings/", "assets", DownloadPriority.High);
        //}
        //if (box.buildAll || box.bData)
        //{
        //    builder.BuildJSONFileAtPath("Assets/BuildOnlyAssets/Data/", "*.txt", "Data/", "assets", DownloadPriority.High);
        //}
        //if (box.buildAll || box.bLanguage)
        //{
        //    builder.BuildLanguageFileAtPath("Assets/BuildOnlyAssets/Language/", "*.txt", "Language/", "assets",
        //        DownloadPriority.High);
        //}
        //builder.CopyFileContent("Assets/BuildOnlyAssets/optional_list.txt", "optional_list.txt");
        EditorUtility.DisplayDialog("", "AssetBundle build completed", "OK");
        AssetDatabase.Refresh();
        //        builder.SaveAssetList("Assets/StreamingAssets/Assets.txt");
    }
}

public class BuildCheckBox
{
    public bool buildAll;
    public bool bAssetBundles;
    public bool bData;
    public bool bLanguage;
    public bool bAvatar;
    public bool bBattleText;
    public bool bCard;
    public bool bItem;
    public bool bSkill;
    public bool bRune;
    public bool bCountry;
    public bool bStages;
    public bool bSound;
    public bool bIcons;
    public bool bLegend;
    public bool bLevelBg;
    public bool bUnits;
    public bool bTags;
    public bool bFX;
    public bool bUI;
    public bool bScene;
    public bool bPlayer;
    public bool bStory;
    public bool bStoryData;
    public bool bCompressed = true;
    public bool enableCache = true;
}