using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class SoundManager
{

    private readonly Hashtable soudHash = new Hashtable();
    private static SoundManager _instance;
    public AudioSource audioS;
    public AudioSource audioM;
    public AudioSource GamePlayAudio;

    public static SoundManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SoundManager();
            }

            return _instance;
        }
    }

    public void Init()
    {
        MyDebug.Log("Init soundManager");
    }

    private SoundManager()
    {
        var temp = new GameObject
        {
            name = "SoundSource"
        };
        temp.AddComponent<MyAudio>();
        temp.AddComponent<AudioListener>();
        audioS = temp.AddComponent<AudioSource>();
        audioS.volume = PlayerPrefs.GetFloat("SoundVolume", 1f);
        audioS.spatialize = true;
        audioS.spatialBlend = 1;
        audioS.rolloffMode = AudioRolloffMode.Custom;
        audioS.spread = 360;
        var temp1 = new GameObject();
        temp1.AddComponent<MyAudio>();
        temp1.name = "MusicSource";
        audioM = temp1.AddComponent<AudioSource>();
        audioM.volume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        audioM.spatialize = true;
        audioM.spatialBlend = 1;
        audioM.rolloffMode = AudioRolloffMode.Custom;
        audioM.spread = 360;

        var temp2 = new GameObject();
        temp2.AddComponent<MyAudio>();
        temp2.name = "GamePlayAudio";
        GamePlayAudio = temp2.AddComponent<AudioSource>();
        GamePlayAudio.volume = PlayerPrefs.GetFloat("SoundVolume", 1f);
    }

    public void PlaySound(int cardPoint, int sex)
    {
        if (GlobalDataScript.soundToggle)
        {
            var path = "Sounds/";
            if (sex == 0)
            {
                path += "boy/" + (cardPoint + 1);
            }
            else
            {
                path += "girl/" + (cardPoint + 1);
            }

            var temp = (AudioClip) soudHash[path];
            if (temp == null)
            {
                MyDebug.Log(path);
                temp = Object.Instantiate(Resources.Load(path)) as AudioClip;

                soudHash.Add(path, temp);
            }

            audioS.PlayOneShot(temp);
        }
    }

    public void PlayMessageBoxSound(int codeIndex,int sex)
    {
        if (GlobalDataScript.soundToggle)
        {
            var path = "Sounds/";
         
            if (sex == 0)
            {
                path += "boy/sssMessage/" + (codeIndex);
            }
            else
            {
                path += "girl/sssMessage" + (codeIndex);
            }

            var temp = (AudioClip) soudHash[path];
            if (temp == null)
            {
                temp = Object.Instantiate(Resources.Load(path)) as AudioClip;
                soudHash.Add(path, temp);
            }

            audioS.PlayOneShot(temp);
        }
    }

    public void PlayBGM(string _name)
    {
        var path = "Sounds/" + _name;
        var temp = (AudioClip) soudHash[path];
        if (temp == null)
        {
            temp = Object.Instantiate(Resources.Load(path)) as AudioClip;
            soudHash.Add(path, temp);
        }

        audioM.clip = temp;
        audioM.loop = true;
        audioM.Play();
    }

    public void StopBGM()
    {
        audioM.loop = false;
        audioM.Stop();
    }

    public void PlaySoundByAction(string str, int sex)
    {
        var path = "Sounds/";
        if (sex == 0)
        {
            path += "boy/gameNN/" + str;
        }
        else
        {
            path += "girl/gameNN/" + str;
        }

        var temp = (AudioClip) soudHash[path];
        if (temp == null)
        {
            MyDebug.Log(path);
            temp = Object.Instantiate(Resources.Load(path)) as AudioClip;

            soudHash.Add(path, temp);
        }

        audioS.PlayOneShot(temp);
    }

    public void PlayFx(string name)
    {
        var path = "FxSound/" + name;
        var temp = (AudioClip) soudHash[path];
        if (temp == null)
        {
            temp = Object.Instantiate(Resources.Load(path)) as AudioClip;
            soudHash.Add(path, temp);
        }

        audioS.PlayOneShot(temp);
    }   

    public void PlaySoundBGM(string name)
    {
        var path = "Sounds/other/" + name;
        var temp = (AudioClip) soudHash[path];
        if (temp == null)
        {
            temp = Object.Instantiate(Resources.Load(path)) as AudioClip;
            soudHash.Add(path, temp);
        }

        audioS.PlayOneShot(temp);
    }

    public void SetSoundV(float v)
    {
        audioS.volume = v;
        PlayerPrefs.SetFloat("SoundVolume", v);
    }    
    public void SetMusicV(float v)
    {
        audioM.volume = v;
        PlayerPrefs.SetFloat("MusicVolume", v);
    }    

    public float GetSoundV()
    {
        return audioS.volume;
    }

    public float GetMusicV()
    {
        return audioM.volume;
    }
}