using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using LitJson;
using AssemblyCSharp;
using System.Runtime.InteropServices;
using System;

public class UpdateFrame : MonoBehaviour
{
    public delegate void MessageHandler(string message);
    public Image ima;
    public List<GameObject> list;
    public GameObject obj;
    public bool isnull;
    public float dex;
    public UIType type;
    string LOCAL_RES_PATH;
    public Text t1;
    public Text t2;
    public Image mm;

    void Start()
    {
        UIManager.instance.Init();
        TalkDataManager.Instance.InitTalkData();
        //UIManager.instance.Show(UIType.UITalk);
        //AssetBundleManager.instance.LoadMode = AssetBundleLoadMode.LoadFromFileAsync;
        //ResourcesLoader.instance.LoadOriginalAssets = false;
        //ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/admitDefeat", (sprite)=> {
        //    ima.overrideSprite = sprite;
        //});
       
        ////NetUtil.ShotSceneTexture();
        //string message =
        //    "{'code':'0','msg':'ok','time':'2018-02-06 15:45:51','apiurl':'ApiPayrecharge_Add.html','ApiHash':'b97319f083246f02aa99addd2de5036f','data':{'appid':'wxcb036be901577d6a','noncestr':'gkOXFIfdCIuojUfy','package':'Sign=WXpay','partnerid':'1493430492','prepayid':'wx2018020615455247e86069c80918778651','timestamp':1517903151,'sign':'325FEE903EB8EF10C240241F1875DE17'}}";
        //WebPayItem item = NetUtil.JsonToObj<WebPayItem>(message);
        //Debug.Log(0);
        //Debug.Log(DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"));
        //mm.sprite = Resources.Load("Image/JinDuTiao/disagree", typeof(Sprite)) as Sprite;
        //CMD_GF_UserReady cg = new CMD_GF_UserReady();
        //Debug.Log(Marshal.SizeOf(typeof(CMD_GF_UserReady)));

        //string testmes = PlayerPrefs.GetString("Testzpy");
        //WxUserInfo wxPersonalInfo = JsonMapper.ToObject<WxUserInfo>(testmes);
        //Debug.Log(testmes);
        //HttpManager.Instance.GetWXReaure("https://api.weixin.qq.com/sns/userinfo?access_token=6_4jmvr8oUIcFVhTMcy7TDjPcp3ZHJwFiTe5-kb0tdOywz_37LXwhX0fR7xG2o-E9jNmyhzVd2iT8Z99SkED-ozA&openid=oDZhz05zhXdQZr1zWoOZEpbf0k_0", Test);   

        //StartCoroutine(SendGet(Test));


        //string message = "{'openid':'OPENID','nickname':'NICKNAME','sex':1,'province':'PROVINCE','city':'CITY','country':'COUNTRY','headimgurl': 'http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0','privilege':['PRIVILEGE1','PRIVILEGE2'],'unionid': ' o6_bmasdasdsad6_2sgVt7hMZOPfL'}";
        //WxUserInfo ifn = JsonMapper.ToObject<WxUserInfo>(message);
        //Debug.Log(message);

        //testlong tl = new testlong();
        //tl.l1 = -1;
        //tl.l2 = -2;
        //string lm = NetUtil.ObjToJson(tl);
        //testlong tll = JsonMapper.ToObject<testlong>(lm);
        //string mess = "{'endType':0,'avatarList':[{'chairId':0,'win':false,'roundTotalScore':-16,'pengArray':null,'chiArray':null,'gangInfos':null,'paiArray':[23,24,25,0,1,3,4,8,8,13],'huInfo':null},{'chairId':1,'win':true,'roundTotalScore':48,'pengArray':[1],'chiArray':null,'gangInfos':null,'paiArray':[20,21,22,2,33,4,10,11,12,33],'huInfo':{'way':null,'type':null,'specialScene':null,'card':24,'huUid':0}},{'chairId':2,'win':false,'roundTotalScore':-16,'pengArray':[16],'chiArray':null,'gangInfos':null,'paiArray':[21,22,23,2,6,7,8],'huInfo':null},{'chairId':3,'win':false,'roundTotalScore':-16,'pengArray':null,'chiArray':null,'gangInfos':null,'paiArray':[33,22,2,3,3,5,5,6,7,8,14,14,15],'huInfo':null}]}";
        //HupaiResponseVo hrv = JsonMapper.ToObject<HupaiResponseVo>(mess);
        //Debug.Log(00);
        // HttpManager.Instance.SentHttpRequre(HTTP_TYPE.paomadeng, Test);
        //try
        //{
        //    Directory.CreateDirectory(Application.persistentDataPath + "/ui/prefab");
        //    t1.text = Application.persistentDataPath;
        //}
        //catch
        //{
        //}
        //try
        //{
        //    Directory.CreateDirectory("file:///" + Application.persistentDataPath + "/yyui/prefab");
        //    t2.text = "file:///" + Application.persistentDataPath;
        //}
        //catch
        //{
        //}
        //string prjPath = Application.dataPath + "/eefff";
        //LOCAL_RES_PATH = "file:///" + Application.persistentDataPath + "/ui/ff/ee.txt";
        //LOCAL_RES_PATH = LOCAL_RES_PATH.Replace('/', '\\');
        //Debug.Log(LOCAL_RES_PATH);
        //  System.IO.File.Create(prjPath);
        // Directory.CreateDirectory(LOCAL_RES_PATH);
        //FileStream stream = new FileStream(LOCAL_RES_PATH, FileMode.Create);
        //UIManager.instance.Init();
        // StartCoroutine(LoadFrompersister());
        //BuildPipeline.BuildAssetBundles("Assets/AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
    }

    //private IEnumerator SendGet(MessageHandler action)
    //{
    //    WWW getData =
    //        new WWW(
    //            "https://api.weixin.qq.com/sns/oauth2/access_token?appid=wx9e0ac6315e6937a7&secret=c93c89ef6cbabb005266b2d5470b9060&code=0712jOgc1QQWMu0qWcec1Tyzgc12jOgz&grant_type=authorization_code");
    //    yield return getData;
    //    if (getData.error != null)
    //    {
    //        Debug.Log(getData.error);
    //        action(getData.error);
    //    }
    //    else
    //    {
    //        Debug.Log(getData.text);
    //        action(getData.text);
    //    }

    //}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("--------------------------------");
            SocketSendManager.Instance.SendPlayerReady();
            //HttpManager.instance.SentHttpRequre(HTTP_TYPE.GetSigin, Test);
            // UIManager.instance.Show(type);
            // iTween.RotateTo(gameObject, iTween.Hash("rotation", new Vector3(-90,180,90), "easeType",iTween.EaseType.easeInOutQuart, "loopType", "none", "time", 6));
        }
    }

//    IEnumerator LoadFrompersister()
//    {
//        string des = Application.persistentDataPath + "/ui/prefab/uipanel_applyexit.assets";
//        Debug.Log(des);
//        string src =
//#if UNITY_EDITOR
//        "file:///" + Application.streamingAssetsPath + "/ui/prefab/uipanel_applyexit.assets";
//        des = des.Replace('/', '\\');
//        Debug.Log(des);
//#elif UNITY_ANDROID
//        "jar:file://" + Application.dataPath + "!/assets/2.Android.unity3d";
//#endif
//        Debug.Log("des:" + des);
//        Debug.Log("src:" + src);
//        using (WWW w = new WWW(src))
//        {
//            yield return w;
//            if (string.IsNullOrEmpty(w.error))
//            {
//                while (!w.isDone) yield return null;
//                FileStream stream = new FileStream(des, FileMode.Create);
//                stream.Write(w.bytes, 0, w.bytes.Length);
//                stream.Flush();
//                stream.Close();
//            }
//            else
//            {
//                Debug.LogError(w.error);
//            }
//        }
//        //string downpath = "file:///" + Application.persistentDataPath + "/uipanel_applyexit.assets";
//        //Debug.Log("down path:" + downpath);
//        //using (WWW www = WWW.LoadFromCacheOrDownload(downpath, 7))
//        //{
//        //    yield return www;
//        //    AssetBundle b = www.assetBundle;
//        //    AssetBundleRequest abr = b.LoadAssetAsync(b.GetAllAssetNames()[0], typeof(GameObject));    //异步加载GameObject类型
//        //    yield return abr;
//        //    GameObject go = Instantiate(abr.asset) as GameObject;
//        //    //Application.LoadLevel("2");
//        //    //lab.text = downpath;
//        //}
//    }
    public void Test(string mes)
    {
        //PaoMaDengManager.paoMaDeng = JsonMapper.ToObject<PaoMaDeng>(mes);
        MyDebug.Log(mes);
    }
}

public class testlong
{
    public long l1;
    public long l2;
}