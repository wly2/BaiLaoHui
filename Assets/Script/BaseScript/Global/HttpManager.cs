using AssemblyCSharp;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class HttpManager : MonoBehaviour
{
    public delegate void MessageHandler(WWW message);

    public static HttpManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject
                {
                    name = "HttpManager"
                };
                _instance = go.AddComponent<HttpManager>();
            }

            return _instance;
        }
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

    private static HttpManager _instance;

    public void SentHttpRequre(HTTP_TYPE type, MessageHandler action)
    {
        StartCoroutine(SendGet(Url.GetUrl(type), action));
    }

    private IEnumerator SendGet(string _url, MessageHandler action)
    {
        MyDebug.Log("Http URL:" + _url);
        WWW getData = new WWW(_url);
        yield return getData;
        if (getData.error != null)
        {
            MyDebug.Log(getData.error);
        }
        else
        {
            MyDebug.Log(getData.text);
            if (action != null)
                action(getData);

        }
       
       
    }

    public void HttpMessage(string _url, MessageHandler action)
    {
        StartCoroutine(SendGet(_url, action));
    }

    public void GetWXPay(int goodID, MessageHandler action)
    {
        StartCoroutine(SendGet(Url.SendWxPayUrl(goodID), action));
    }
    public void GetGameStartpic(MessageHandler action)
    {
        string mes =Url.HOST  +"Game/getGameStartpic?time=" + GetTimeStamp() + "&hash=a6669b7709f1bb88445c5696390c47f5&uid="+ GetUid ()+ "&type=2";
        StartCoroutine(SendGet(mes, action));
    }
    public void GetUserReconnection(MessageHandler action)
    {
        string mes = Url.HOST + "Game/getUserReconnection?time=" + GetTimeStamp() + "&hash=4b50512c9c732419a0d992ab9cd202bc&uid=" + GetUid();
        StartCoroutine(SendGet(mes, action));
    }
    public void GetOneSystemNews(MessageHandler action)
    {
        string mes = Url.HOST + "Game/getOneSystemNews?time=" + GetTimeStamp() + "&hash=4b50512c9c732419a0d992ab9cd202bc&uid=" + GetUid();
        StartCoroutine(SendGet(mes, action));
    }
    public void GetSystemsNewsList(MessageHandler action)
    {
        string mes = Url.HOST + "Game/getSystemsNewsList?time=" + GetTimeStamp() + "&hash=4b50512c9c732419a0d992ab9cd202bc&uid=" + GetUid();
        StartCoroutine(SendGet(mes, action));
    }
    public void SetSystemsNewsStatus(int id,string status = "read")
    {
        string mes = Url.HOST + "Game/actSystemsNews?time=" + GetTimeStamp() + "&hash=a678f4dadf0000243f13752f31576ed4&uid=" + GetUid()+ "&act="+status+ "&id="+id;
        StartCoroutine(SendGet(mes, null));
    }
    public void GetGameGold(MessageHandler action)
    {
        string mes = Url.HOST + "Game/getUserScoreData?time=" + GetTimeStamp() + "&hash=4b50512c9c732419a0d992ab9cd202bc&uid=" + GetUid();
        StartCoroutine(SendGet(mes, action));
    }
    private static int GetTimeStamp()
    {
        return DateTime.Now.Second;
    }



    private static int GetUid()
    {
        return (int)GlobalDataScript.userData.dwUserID;
    }
}