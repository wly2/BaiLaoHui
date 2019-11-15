using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using AssemblyCSharp;              
using System.Collections.Generic;  
/// <summary>
/// 获取MD5值_测试
/// </summary>
public class MD5_test : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ReadBundleFromFile(Application.persistentDataPath + "/ui/prefab/uipanel_applyexit.assets"));
        MyDebug.Log(GetMD5HashFromFile(Application.persistentDataPath + "/ui/prefab/uipanel_applyexit.assets"));
    }

    /// <summary>
    /// 获取 MD5 值
    /// </summary>
    /// <returns> 返回MD5值.</returns>
    /// <param name="fileName">文件地址 </param>
    private static string GetMD5HashFromFile(string fileName)
    {
        try
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(file);
            file.Close();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
        catch (Exception ex)
        {
            throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
        }
    }

    AssetBundleCreateRequest asset; //定义一个资源包创建请求

    private IEnumerator ReadBundleFromFile(string fileName)
    {
        FileStream file = new FileStream(fileName, FileMode.Open);
        byte[] assetbytes = new byte[file.Length];
        file.Read(assetbytes, 0, (int) file.Length);
        file.Close();

        asset = AssetBundle.LoadFromMemoryAsync(assetbytes); //从内存中创建资源
        yield return asset;

        AssetBundle LoadAsset = asset.assetBundle; //这样就能得到我们需要的资源包了
        AssetBundleRequest
            abr = LoadAsset.LoadAssetAsync(LoadAsset.GetAllAssetNames()[0], typeof(GameObject)); //异步加载GameObject类型
        yield return abr;
        GameObject go = Instantiate(abr.asset) as GameObject;
        UIWindow win = go.GetComponent<UIWindow>();

        LoadAsset.Unload(false);
        yield return null;
    }
}