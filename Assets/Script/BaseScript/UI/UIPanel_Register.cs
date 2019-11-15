using UnityEngine;
using UnityEngine.UI;

public class UIPanel_Register : UIWindow
{
    public InputField zhangHao;
    public InputField miMa;
    public InputField zhuCeZH;
    public InputField zhuCeMM;
    public InputField sureMiMa;
    public InputField gameName;
    public InputField phoneNum;
    public InputField yanZhengMa;
    public GameObject login;
    public GameObject register;

    void Awake()
    {
        register.SetActive(false);
        login.SetActive(true);
    }

    public void RegisterBtn()
    {
        register.SetActive(true);
        login.SetActive(false);
    }

    public void CancleBtn()
    {
        register.SetActive(false);
        login.SetActive(true);
    }
}