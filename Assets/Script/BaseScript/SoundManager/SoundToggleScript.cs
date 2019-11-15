using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class SoundToggleScript : UIWindow
{
    public GameObject openSoundBtn;
    public GameObject closeSoundBtn;
    public GameObject openMusicBtn;
    public GameObject closeMusicBtn;
    public Slider soundSlider;

    public Slider musicSlider;

    // Use this for initialization
    void Start()
    {

        soundSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        musicSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1);
    }

    // Update is called once per frame
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

        SoundManager.Instance.SetMusicV(soundSlider.value);
        GlobalDataScript.soundVolume = soundSlider.value;
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

        SoundManager.Instance.SetSoundV(musicSlider.value);
        GlobalDataScript.musicVolume = musicSlider.value;
        PlayerPrefs.SetFloat("SoundVolume", musicSlider.value);
    }

    public void openClick()
    {
        soundSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1);
        openSoundBtn.SetActive(false);
        closeSoundBtn.SetActive(true);
        GlobalDataScript.soundToggle = true;
        PlayerPrefs.SetInt("Music", 1);
        SoundManager.Instance.SetMusicV(soundSlider.value);
    }

    public void OpenMusicClick()
    {
        musicSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1);
        openMusicBtn.SetActive(false);
        closeMusicBtn.SetActive(true);
        PlayerPrefs.SetInt("Sound", 1);
        SoundManager.Instance.SetSoundV(musicSlider.value);
    }

    public void closeClick()
    {
        soundSlider.value = 0;
        openSoundBtn.SetActive(true);
        closeSoundBtn.SetActive(false);
        GlobalDataScript.soundToggle = false;
        PlayerPrefs.SetInt("Music", 0);
        SoundManager.Instance.SetMusicV(soundSlider.value);
    }

    public void CloseMusicClick()
    {
        musicSlider.value = 0;
        openMusicBtn.SetActive(true);
        closeMusicBtn.SetActive(false);
        PlayerPrefs.SetInt("Sound", 0);
        SoundManager.Instance.SetSoundV(musicSlider.value);
    }
}