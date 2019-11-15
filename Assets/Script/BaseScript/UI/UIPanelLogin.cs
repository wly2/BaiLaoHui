using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using LitJson;
using System.Collections;

public class UIPanelLogin : MonoBehaviour
{
    public Toggle agreeProtocol;
    public static int TestId;

    private void Awake()
    {

#if UNITY_STANDALONE_WIN
        Screen.SetResolution(854, 480, false);
#endif
    }

    IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        SoundManager.Instance.Init();
        MySceneManager.instance.Init();
        yield return new WaitForEndOfFrame();
        SoundManager.Instance.PlayBGM("backMusic");
        SoundManager.Instance.SetMusicV(PlayerPrefs.GetFloat("MusicVolume", 1));
        GlobalDataScript.isonLoginPage = true;
        SocketEventHandle.Instance.loginReply += OnLoginReply;        
    }

 
    /// <summary>
    /// 微信登录
    /// </summary>
    public void Login()
    {
        //UnityPhoneManager.Instance.SetVXCode("021cwl8O1FHg8216Zz7O1jgz8O1cwl86");
        //return;
        SoundManager.Instance.PlaySoundBGM("clickbutton");

        if (agreeProtocol.isOn)
        {
            DoLogin();
            MyDebug.Log("微信登录");
        }
        else
        {
            MyDebug.Log("请先同意用户使用协议");
            TipsManagerScript.getInstance.setTips(LocalizationManager.GetInstance.GetValue("KEY.20012"));

        }
    }

    public void DoLogin()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        UnityPhoneManager.Instance.TestLogin("2");//+ Random.Range(100, 999));// 
#else
        UnityPhoneManager.Instance.WxDengLu();    ///微信登录  
#endif
    }

    public void OnLoginReply(ClientResponse response)
    {
        MyDebug.Log("OnLoginReply !!!");        
        if (LoginData.wxUserInfo.headimgurl != null && LoginData.wxUserInfo.headimgurl.Length > 5)
        {
            string url = Url.HOST + "Game/saveAccountsFace?time=" + System.DateTime.Now.DayOfYear + "&hash=deca153b2c7b5cbb7c6fb8586b271f6b&uid=" + GlobalDataScript.userData.dwUserID + "&faceurl=" + LoginData.wxUserInfo.headimgurl;
            HttpManager.instance.HttpMessage(url, null);
        }
        SocketEventHandle.Instance.ReEnterRoom();                   
    }       
    private void OnDestroy()
    {
        RemoveListener();
    }
    public void RemoveListener()
    {
        SocketEventHandle.Instance.loginReply -= OnLoginReply;        
    }         
    public void ShowXieYi()
    {
        ResourcesLoader.instance.testC();
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        UIManager.instance.Show(UIType.UIProtocol);
    }

    private string filePath;

    IEnumerator LoadAnouncementText()
    {
        var wwwObject = new WWW(filePath); //利用www类加载
        MyDebug.Log(wwwObject.url);
        yield return wwwObject;
        var mainBundle = wwwObject.assetBundle; //获得AssetBundle
        var abr = mainBundle.LoadAssetAsync("UIPanel_Protocol", typeof(GameObject)); //异步加载GameObject类型
        yield return abr;
        var go = Instantiate(abr.asset) as GameObject;
        yield return null;
        mainBundle.Unload(false); //卸载所有包含在bundle中的对象。已经加载的才会卸载
        wwwObject.Dispose(); //中断www
    }


    /// <summary>
    /// 游客登录
    /// </summary>
    public void RegisterBtn()
    {
        SocketLoginEvent.instance.LogonVisitor();
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
    }
}