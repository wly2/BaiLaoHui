using AssemblyCSharp;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XLua;

public class UIPanel_SceneLoading : MonoBehaviour
{
    public static UIPanel_SceneLoading instance;
    private int mCurProgress;
    private AsyncOperation mAsyncOperation;
    public Slider slider;
    public static float percentV;
    public Text percentText;
    public Text tips;
    public string luatextName;
    LuaEnv luaenv = new LuaEnv();
    public Text tipsTxt;

    private void Awake()
    {
#if UNITY_STANDALONE_WIN
        Screen.SetResolution(642, 360, false);                  
#endif                                      
#if UNITY_EDITOR                                 
        WriteFileByLine(Application.dataPath, "config.txt");
#endif
        GlobalDataScript.szMachineID = ""+ Random.Range(100000, 9999999);
    }
    public void WriteFileByLine(string file_path, string file_name)
    {
        StreamWriter sw;
        if (!File.Exists(file_path + "//" + file_name))
        {
            sw = File.CreateText(file_path + "//" + file_name);//创建一个用于写入 UTF-8 编码的文本  
            MyDebug.Log("文件创建成功！");
            string mes = "";
            sw.WriteLine(mes);//以行为单位写入字符串  
            sw.Close();
            sw.Dispose();//文件流释放  
        }
        else
        {
            string mes = File.ReadAllText(file_path + "//" + file_name);
            GlobalDataScript.szMachineID = mes;
        }
    }



    void Start()
    {
        instance = this;
        InitEnvironment();
        if (AssetBundleManager.instance.LoadMode == AssetBundleLoadMode.LoadFromWWW)
        {
            BundleUpdate.instance.CompareUpdate(() =>
            {
                ResourcesLoader.Load<TextAsset>("BaseAssets/Lua/ooo.lua", (luaScript) =>
                {
                    luaenv.DoString(@luaScript.text);

                    StartCoroutine(LoadScene());
                });
            });
        }
        else
        {
            //TextAsset luaScript = Resources.Load<TextAsset>("ooo.lua");
            //luaenv.DoString(@luaScript.text);
            //ResourcesLoader.instance.testC();
            StartCoroutine(LoadScene());
        }


        IPAddress[] address = Dns.GetHostAddresses("www.baidu.com");
        APIS.tcpFamily = address[0].AddressFamily;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 30;
        percentV = 0;

    }

    private void InitEnvironment()
    {
        AssetBundleManager.instance.LoadMode = AssetBundleLoadMode.LoadFromFileAsync;
#if UNITY_IOS || UNITY_ANDROID  ||UNITY_STANDALONE_WIN  
        AssetBundleManager.instance.LoadMode = AssetBundleLoadMode.LoadFromWWW;
#endif        
#if UNITY_EDITOR
        ///加载资源弹出框
        if (UnityEditor.EditorUtility.DisplayDialog("Resources Loader", "Do you want load assets from original prefab?", "Yes", "No"))
        {
            AssetBundleManager.instance.LoadMode = AssetBundleLoadMode.LoadFromFile;
            ResourcesLoader.instance.LoadOriginalAssets = true;
        }
        else
        {
            if (UnityEditor.EditorUtility.DisplayDialog("Resources Loader", "Please select load mode", "Load From File", "Load From WWW"))
            {
                AssetBundleManager.instance.LoadMode = AssetBundleLoadMode.LoadFromFileAsync;
            }
            else
            {
                AssetBundleManager.instance.LoadMode = AssetBundleLoadMode.LoadFromWWW;
            }
        }
#endif
    }
    private void OnDisable()
    {
        instance = null;
        MyDebug.Log("CloseUILoading");
    }

    private IEnumerator LoadScene()
    {
        ////
        mAsyncOperation = SceneManager.LoadSceneAsync("LogIn_Scene");/*testLogin*/
        // 不允许加载完毕自动切换场景，因为有时候加载太快了就看不到加载进度条UI效果了
        mAsyncOperation.allowSceneActivation = false;
        // mAsyncOperation.progress测试只有0和0.9(其实只有固定的0.89...)
        // 所以大概大于0.8就当是加载完成了
        while (!mAsyncOperation.isDone && mAsyncOperation.progress < 0.8f)
        {
            yield return mAsyncOperation;
        }
    }

    void Update()
    {       
        slider.value = percentV * 0.01f;
        if (percentV <= 100)
        {
            percentText.text = percentV.ToString("f2") + "%";
        }

        if (mAsyncOperation == null)
            return;
        // 以下都是为实现加载进度条的
        // var progressBar = 0;
        percentV = mAsyncOperation.progress < 0.8 ? (int)(mAsyncOperation.progress * 100) : 100;
        if (percentV >= 100) mAsyncOperation.allowSceneActivation = true;
    }
}