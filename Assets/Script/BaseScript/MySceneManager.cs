using AssemblyCSharp;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    private AsyncOperation mAsyncOperation;
    private int mCurProgress;
    private bool isLoadResource = true;
    SCENE_STATE Change_scene_state = SCENE_STATE.NONE;
    SCENE_STATE Current_scene_state = SCENE_STATE.NONE;
    SCENE_STATE Ll_scene_state = SCENE_STATE.NONE;

    enum SCENE_STATE
    {
        NONE,
        LOGIN,
        MAIN,
        MaJiang,
        NiuNiu,
        ShiSanShui,
        Loading
    }

    private static MySceneManager _instance;

    public static MySceneManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject temp = new GameObject();
                temp.name = "MySceneManager";
                _instance = temp.AddComponent<MySceneManager>();
            }

            return _instance;

        }
    }

    public void Init()
    {
    }

    void OnEnable()
    {
        DontDestroyOnLoad(this);
    }

    public void SceneToLogIn()
    {
        if (Current_scene_state == SCENE_STATE.LOGIN)
            return;
        MyDebug.Log("Scene to LogIn");
        Change_scene_state = SCENE_STATE.LOGIN;
        Current_scene_state = SCENE_STATE.LOGIN;
  

        ////UIPanel_Loading.instance.SetLoadPercentShow("");
    }

    public void ScentToMain()
    {   
        if (Current_scene_state == SCENE_STATE.MAIN)
            return;
        MyDebug.Log("Scene to Main");
        Change_scene_state = SCENE_STATE.MAIN;
        Current_scene_state = SCENE_STATE.MAIN;                  
    }

    public void SceneToMaJiang()
    {
        if (Current_scene_state == SCENE_STATE.MaJiang)
            return;
        MyDebug.Log("Scene to MaJiang");
        Change_scene_state = SCENE_STATE.MaJiang;
        Current_scene_state = SCENE_STATE.MaJiang;
       

       //// UIPanel_Loading.instance.SetLoadPercentShow("");
    }
    public void SceneToShiSanShui()
    {
        if (Current_scene_state == SCENE_STATE.ShiSanShui)
            return;
        MyDebug.Log("Scene to ShiSanShui");
        Change_scene_state = SCENE_STATE.ShiSanShui;
        Current_scene_state = SCENE_STATE.ShiSanShui;
  

        ////UIPanel_Loading.instance.SetLoadPercentShow("");
    }
    public void SceneToNiuNiu()
    {
        if (Current_scene_state == SCENE_STATE.NiuNiu)
            return;
        MyDebug.Log("Scene to MaJiang");
        Change_scene_state = SCENE_STATE.NiuNiu;
        Current_scene_state = SCENE_STATE.NiuNiu;


      ////  UIPanel_Loading.instance.SetLoadPercentShow("");
    }

    public void BackToMain()
    {                        
        MyDebug.Log("Back to Main");
        ScentToMain();
        SocketEngine.Instance.SocketQuit();
    }

    private void SceneToLoding(SCENE_STATE type)
    {
        isLoadResource = false;
        Ll_scene_state = type;
        Change_scene_state = SCENE_STATE.Loading;
        if (UIPanel_Loading.instance == null)
        {
            UIManager.instance.Show(UIType.UILoading);
        }

      ////  UIPanel_Loading.instance.SetLoadPercentShow("");
    }  
    public void ResLoadinOver()
    {
    }  
    public void SceneState()
    {     
        switch (Change_scene_state)
        {
            case SCENE_STATE.LOGIN:
                //if (UIPanel_Loading.instance == null)
                //{
                //    UIManager.instance.Show(UIType.UILoading);
                //}
                MyDebug.Log("Scene to LOGIN   1");
                UIManager.instance.ClearUI();
                StartCoroutine(LoadScene(1));
                Change_scene_state = SCENE_STATE.NONE;
                break;
            case SCENE_STATE.MAIN:
                //if (UIPanel_Loading.instance == null)
                //{
                //    UIManager.instance.Show(UIType.UILoading);
                //}
                MyDebug.Log("Scene to MAIN   1");
                UIManager.instance.ClearUI();
                GlobalDataScript.isGameReadly = false;
                GlobalDataScript.isBeginGame = false;
                StartCoroutine(LoadScene(2));
                Change_scene_state = SCENE_STATE.NONE;
                break;
            case SCENE_STATE.MaJiang:
                //if (UIPanel_Loading.instance == null)
                //{
                //    UIManager.instance.Show(UIType.UILoading);
                //}
                MyDebug.Log("Scene to Game   1");
                UIManager.instance.ClearUI();
                SoundManager.Instance.StopBGM();
                MyDebug.Log("Scene to Game   2");
                StartCoroutine(LoadScene(3));
                Change_scene_state = SCENE_STATE.NONE;
                break;
            case SCENE_STATE.NiuNiu:
                //if (UIPanel_Loading.instance == null)
                //{
                //    UIManager.instance.Show(UIType.UILoading);
                //}
                MyDebug.Log("Scene to Game   1");
                UIManager.instance.ClearUI();
                SoundManager.Instance.StopBGM();
                MyDebug.Log("Scene to Game   2");
                StartCoroutine(LoadScene(4));
                Change_scene_state = SCENE_STATE.NONE;
                break;
            case SCENE_STATE.ShiSanShui:
                //if (UIPanel_Loading.instance == null)
                //{
                //    UIManager.instance.Show(UIType.UILoading);
                //}
                MyDebug.Log("Scene to Game   1");
                UIManager.instance.ClearUI();
                SoundManager.Instance.StopBGM();
                MyDebug.Log("Scene to Game   2");
                StartCoroutine(LoadScene(5));
                Change_scene_state = SCENE_STATE.NONE;
                break;
            default:
                break;
        }
    }        
    private IEnumerator LoadScene(int sceneName)
    {                                                                  
        mAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
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
        SceneState();
        if (mAsyncOperation == null)
            return;
        // 以下都是为实现加载进度条的
        int progressBar = 0;
        progressBar = mAsyncOperation.progress < 0.8 ? (int) (mAsyncOperation.progress * 100) : 100;
        MyDebug.Log(progressBar);
        if (UIPanel_Loading.instance != null)
            UIPanel_Loading.instance.percentV = progressBar - 1;
        if (progressBar >= 100) mAsyncOperation.allowSceneActivation = true;
        if (mAsyncOperation.isDone)
            mAsyncOperation = null;
        return;
        if (mCurProgress <= progressBar)
        {
            mCurProgress++;
            if (UIPanel_Loading.instance != null)
                UIPanel_Loading.instance.percentV = mCurProgress - 1;
            // 进度条ui显示（本文不讨论）
            //  ((Win_Loading)UIWindowCtrl.GetInstance().GetCurrentWindow()).loadingView.SetLoadSceneInfo(mCurProgress * 0.01f);
        }
        else
        {
            // 必须等进度条跑到100%才允许切换到下一场景
            if (progressBar == 100)
            {
                mAsyncOperation.allowSceneActivation = true;
                UIManager.instance.ClearUI();
            }
        }

        if (mAsyncOperation.isDone)
            mAsyncOperation = null;
    }
}