using UnityEngine;

public class ShowSpeakingScript : MonoBehaviour
{
    public GameObject speakBtn;
    private bool isSpeak;

    public void Clickspeakbtn()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        if (GameObject.Find("UIPanel_Talk(Clone)") == null)
        {
            UIManager.instance.Show(UIType.UITalk);
        }
    }
}