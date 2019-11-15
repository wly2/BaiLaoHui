using AssemblyCSharp;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelSetting : UIWindow
{
    public Slider soundSlider;
    public Slider musicSlider;

    public Toggle musicToggle;
    public Toggle soundToggle;

    void Start()
    {
        //soundSlider.value = SoundManager.Instance.GetSoundV();
        //musicSlider.value = SoundManager.Instance.GetMusicV();
        MyDebug.Log(SoundManager.Instance.GetMusicV());
        MyDebug.Log(SoundManager.Instance.GetSoundV());
        musicToggle.isOn = SoundManager.Instance.GetMusicV() == 1 ? true : false;
        soundToggle.isOn = SoundManager.Instance.GetSoundV() == 1 ? true : false;
    }

    public void OnMusicChange(float v)
    {
        SoundManager.Instance.SetMusicV(v);
    }

    public void OnSoundChange(float v)
    {
        SoundManager.Instance.SetSoundV(v);
    }

    //音乐开关
    public void OnMusicEvnent()
    {
        if (musicToggle.isOn == false)
            SoundManager.Instance.SetMusicV(0);
        else if (musicToggle.isOn == true)
            SoundManager.Instance.SetMusicV(1);
    }

    //音效开关
    public void OnSoundEvnent()
    {
        if (soundToggle.isOn == false)
            SoundManager.Instance.SetSoundV(0);
        else if (soundToggle.isOn == true)
            SoundManager.Instance.SetSoundV(1);
    }

    public void ExitButton()
    {
        UnityPhoneManager.Instance.ClearWxInfo();
        CloseUI();
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        MySceneManager.instance.SceneToLogIn();
    }
}