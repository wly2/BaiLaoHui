using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_MicPhoneScript : MonoBehaviour
{
    public float WholeTime = 10f;
    private Boolean btnDown;
    public Image icon;
    private List<Sprite> list = new List<Sprite>();
    private float timeCout;
    private int spriteCount;
    public void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/YuYin/V_"+i, (sprite) => {
                list.Add(sprite);
            });
        }
    }
    void FixedUpdate()
    {
        if (btnDown)
        {
            timeCout += Time.deltaTime;
            if (timeCout > 0.3f)
            {
                timeCout = 0;
                icon.overrideSprite = list[spriteCount % 5];
                spriteCount++;
            }          
            WholeTime -= Time.deltaTime;
            if (WholeTime <= 0)
            {
                OnPointerUp();
            }
            
        }
    }

    public void OnPointerDown()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        if (GlobalDataScript.Instance.playerInfos != null && GlobalDataScript.Instance.playerInfos.Count > 0)
        {
            btnDown = true;
            icon.enabled = true;
            timeCout = 0;
            spriteCount = 0;
         //   MicroPhoneInput.instance.StartRecord();

        }
        else
        {
            TipsManagerScript.getInstance.setTips("房间里只有你一个人，不能发送语音");
        }
    }

    public void OnPointerUp()
    {
        if (btnDown)
        {
            btnDown = false;
            icon.enabled = false;
            WholeTime = 10;
            if (GlobalDataScript.Instance.playerInfos != null && GlobalDataScript.Instance.playerInfos.Count > 0)
            {
              //  MicroPhoneInput.instance.StopRecord();
            }
        }
    }
}