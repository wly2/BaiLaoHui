using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;

public class SetPanelScript : MonoBehaviour
{
    public GameObject openSoundBtn;
    public GameObject closeSoundBtn;
    public GameObject openMusicBtn;
    public GameObject closeMusicBtn;
    public Slider soundSlider;
    public Slider musicSlider;
    readonly UIPanelLogin lss = new UIPanelLogin();

    void Start()
    {

        soundSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        musicSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1);
    }

    void Update()
    {
        if (soundSlider.value > 0)
        {
            openSoundBtn.SetActive(false);
            closeSoundBtn.SetActive(true);
            GlobalDataScript.soundToggle = true;
            PlayerPrefs.SetInt("Music", 1);
        }
        else
        {
            closeSoundBtn.SetActive(false);
            openSoundBtn.SetActive(true);
            GlobalDataScript.soundToggle = false;
            PlayerPrefs.SetInt("Music", 0);
        }

        GlobalDataScript.soundVolume = soundSlider.value;
        SoundManager.Instance.SetMusicV(soundSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", soundSlider.value);

        if (musicSlider.value > 0)
        {
            openMusicBtn.SetActive(false);
            closeMusicBtn.SetActive(true);
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            closeMusicBtn.SetActive(false);
            openMusicBtn.SetActive(true);
            PlayerPrefs.SetInt("Sound", 0);
        }

        GlobalDataScript.musicVolume = musicSlider.value;
        SoundManager.Instance.SetSoundV(musicSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", musicSlider.value);
    }

    public void OpenClick()
    {
        SoundManager.Instance.SetMusicV(soundSlider.value);
        soundSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        openSoundBtn.SetActive(false);
        closeSoundBtn.SetActive(true);
        GlobalDataScript.soundToggle = true;
        PlayerPrefs.SetInt("Music", 1);
    }

    public void CloseClick()
    {
        SoundManager.Instance.SetMusicV(soundSlider.value);
        soundSlider.value = 0;
        openSoundBtn.SetActive(true);
        closeSoundBtn.SetActive(false);
        GlobalDataScript.soundToggle = false;
        PlayerPrefs.SetInt("Music", 0);
    }

    public void OpenMusicClick()
    {
        SoundManager.Instance.SetSoundV(musicSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1);
        openMusicBtn.SetActive(false);
        closeMusicBtn.SetActive(true);
        GlobalDataScript.soundToggle = true;
        PlayerPrefs.SetInt("Sound", 1);
    }

    public void CloseMusicClick()
    {
        SoundManager.Instance.SetSoundV(musicSlider.value);
        musicSlider.value = 0;
        openMusicBtn.SetActive(true);
        closeMusicBtn.SetActive(false);
        GlobalDataScript.soundToggle = false;
        PlayerPrefs.SetInt("Sound", 0);
    }

    public void CloseSetButton()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        Destroy(this);
        Destroy(gameObject);
    }

    public void ExitButton()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        //SoundCtrl.instance.stopBGM();

        CustomSocket.Instance.SendMsg(new LoginRequest());
        Destroy(transform.parent.parent.parent.gameObject);
        lss.RemoveListener();
        MySceneManager.instance.SceneToLogIn();
    }
}