using AssemblyCSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_Screenshot : UIWindow
{
    public Image shareTexture;
    private string path;
    public  float ff1 = 0.1f;
    public float ff2 = 2;

    void Start()
    {
        //Init("C:/Users/Administrator/AppData/LocalLow/xm/hangzhoumj/2018-02-05-02-51-01.png");
        //Init("http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Init(string _path)
    {
        path = _path;
        Invoke("LoadImage", ff1);
        Invoke("ShareSession", ff2);   
    }

    /// <summary>
    /// 分享朋友圈
    /// </summary>
    public void shareTimeLine()
    {
        CloseUI();
        UnityPhoneManager.Instance.ShareTimelineImage(Application.persistentDataPath + "/" + path);
    }

    /// <summary>
    /// 分享好友
    /// </summary>
    public void ShareSession()
    {
        CloseUI();
        UnityPhoneManager.Instance.ShareSessionImage(Application.persistentDataPath + "/" + path);
    }

    public void SaveLocalDocment()
    {
        UnityPhoneManager.Instance.SaveLocalPhoto(Application.persistentDataPath + "/" + path);
    }

    Texture2D texture2D;

    private IEnumerator LoadImg()
    {
        MyDebug.Log("LoadImg:" + Application.persistentDataPath + "/" + path);
        var www = new WWW(Application.persistentDataPath + "/" + path);
        yield return www;
        //下载完成，保存图片到路径filePath
        try
        {
            texture2D = www.texture;
            //将图片赋给场景上的Sprite
            shareTexture.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height),
                new Vector2(0, 0));

        }
        catch (Exception e)
        {
            MyDebug.Log("LoadImg" + e.Message);
        }
    }

    private void LoadImage()
    {
        FileStream fs = null;
        byte[] buffer = null;
        try
        {
            if (File.Exists(Application.persistentDataPath + "//" + path)) //图片文件的全路径字符串
            {
                fs = new FileStream(Application.persistentDataPath + "//" + path, FileMode.Open);
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, int.Parse(fs.Length.ToString()));

                Texture2D tex = new Texture2D(80, 80);
                try
                {
                    tex.LoadImage(buffer);
                }
                catch (Exception ex)
                {
                }
                shareTexture.enabled = true;
                shareTexture.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));

                fs.Close();
                fs.Dispose();
            }
        }
        catch
        {
            fs.Close();
            fs.Dispose();
        }
       
    }
}
