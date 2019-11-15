using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_Loading : UIWindow
{
    public static UIPanel_Loading instance;
    public GameObject go;
    public Slider slider;
    public int percentV;
    public Text percentText;
    public bool isLoadResource;
    public Text tipText;
    private bool showpercent;
    private string tipMes;

    public GameObject bg;
    public bool isLoading = false;
    public float loadCount;

    void OnEnable()
    {
        playCloseSound = false;
        tipText.text = "";
        showpercent = false;
        instance = this;
        MyDebug.Log("ShowUILoading");
        GameMessageManager.CloseLoading += CloseUI;
        percentV = 0;
        slider.gameObject.SetActive(false);
        bg.SetActive(false);
        isLoading = true;
        loadCount = 0;
    }

    void Update()
    {
        if (showpercent)
        {
            slider.gameObject.SetActive(true);
            bg.SetActive(true);
            tipText.text = tipMes;
            showpercent = false;
            loadCount = 0;
            isLoading = false;
        }

        go.transform.Rotate(new Vector3(0, 0, -90 * Time.deltaTime * 5));
        slider.value = percentV * 0.01f;
        percentText.text = percentV + "%";
        if(isLoading)
        {
            loadCount += Time.deltaTime;
            if(loadCount>20)
            {
                SocketEngine.Instance.SocketQuit();
                CloseUI();
                SocketEventHandle.Instance.SetTips("网络请求超时超时，请重试！！");
            }
        }
    }

    private void OnDisable()
    {
        instance = null;
        MyDebug.Log("CloseUILoading");
        GameMessageManager.CloseLoading -= CloseUI;
        slider.gameObject.SetActive(false);
        bg.SetActive(false);                   
    } 
    public void SetLoadPercentShow(string mes)
    {
        tipMes = mes;
        showpercent = true;
    }
}