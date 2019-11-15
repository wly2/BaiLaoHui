using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 这是声音控制面板，控制声音的大小，如果要显示或者隐藏该面板，请调用 showOrHidePanel（）方法
/// </summary>
public class AudioCtrlPanelScript : MonoBehaviour
{
    AudioSource myAudioClip;
    public Text volumeText;
    public Slider audioSlider;

    void Start()
    {
        myAudioClip = GameObject.Find("MyAudio").GetComponent<AudioSource>();
        audioSlider.value = myAudioClip.volume;
        SetText();
    }

    public void ShowOrHidePanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SilderChange()
    {
        myAudioClip.volume = audioSlider.value;
        SetText();
    }

    void SetText()
    {
        volumeText.text = (int) (100 * audioSlider.value) + "";
    }
}