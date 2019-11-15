using UnityEngine;

public class UIWindow : MonoBehaviour
{
    public bool playCloseSound = true;
    public UIType myType;

    public virtual void CloseUI()
    {
        UIManager.instance.CloseUI();
        Destroy(gameObject);
        if (!playCloseSound)
            return;
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
    }

    public virtual void ShowUI()
    {
    }
}