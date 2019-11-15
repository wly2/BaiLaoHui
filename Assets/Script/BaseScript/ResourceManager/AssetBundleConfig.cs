using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AssetBundleConfig
{
    //AssetBundle打包路径
    public static string ASSETBUNDLE_PATH_LOCSTREAM = Application.dataPath + "/StreamingAssets/";
    public static string ASSETBUNDLE_PATH_D = "D:/TestResource/";
    public static string ASSETBUNDLE_PATH_E = "E:/Game/3D/Assets";//加载游戏ui资源路劲

    public static string ASSETBUNDLE_PATH_WEB = "http://103.239.245.151:8080/game_update/"
#if UNITY_IOS
        +  "BaiLaoHuiIos/"  ;
#elif UNITY_ANDROID   ||  UNITY_STANDALONE_WIN || UNITY_EDITOR
           + "BaiLaoHuiAndroid/";
#endif
    //public static string ASSETBUNDLE_PATH_WEB = "D:/TestResource/";
    //public static string ASSETBUNDLE_PATH = "C:/Users/Administrator/AppData/LocalLow/fdsfds/TestXLua" + "/StreamingAssets/";
    public static string ASSETBUNDLE_PATH_PERSIS = Application.persistentDataPath + "/StreamingAssets/";
    //AssetBundle存放的文件夹名
    public static string ASSETBUNDLE_FILENAM = "StreamingAssets";

    //AssetBundle打包的后缀名
    public static string SUFFIX = ".unity3d";
}
