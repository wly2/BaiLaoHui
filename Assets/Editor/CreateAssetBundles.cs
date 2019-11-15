using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundles", BuildAssetBundleOptions.None,
            BuildTarget.Android);
        //BuildPipeline.BuildAssetBundles("Assets/AssetBundles");
    }

    public static void ReName()
    {
        Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(objs[i]);
            FileInfo dir = new FileInfo(path);
            string parent = dir.Directory.Name;
            AssetImporter.GetAtPath(path).assetBundleName = parent + "/" + objs[i].name;
            if (i % 10 == 0)
            {
                bool isCancel = EditorUtility.DisplayCancelableProgressBar("修改中", path, (float) i / objs.Length);
                if (isCancel)
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }
            }
        }

        EditorUtility.ClearProgressBar();
    }
}