using UnityEngine;

public class UIPanel_Share : UIWindow
{
    public void WeChatShare()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        UnityPhoneManager.Instance.ShareSessionText("百老汇棋牌");
    }

    public void PengYouQuanShare()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        UnityPhoneManager.Instance.ShareTimelineText("百老汇棋牌");
    }
}