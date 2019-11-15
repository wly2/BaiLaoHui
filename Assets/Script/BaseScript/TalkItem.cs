using UnityEngine;
using AssemblyCSharp;
using UnityEngine.UI;
using System.Collections;
using System;                     

public class TalkItem : MonoBehaviour
{

    public delegate void GameEvent();
    public GameEvent BtnOnClick;
    public int id;
    public Text mesText;
    public Image iconImage;
    public Text localText;
    public GameObject talkInfo;
    public TalkItemData mData;
    public Image faceIcon;
    public GameObject textBg;
    public GameObject normalText;

    public void Init(TalkItemData data, GameEvent ev)
    {
        talkInfo.SetActive(false);
        normalText.SetActive(true);
        BtnOnClick = ev;
        id = data.id;
        mesText.text = data.message;
    }

    public void BtnClick(int index)
    {
        index = this.id;
        int sex = GlobalDataScript.Instance.myGameRoomInfo.sex;
        SoundManager.Instance.PlayMessageBoxSound(index,sex);
        if (BtnOnClick == null)
        {
            if (clip == null)
                return;
            MyDebug.Log("btnClick   Play Clip");
            SoundManager.Instance.GamePlayAudio.clip = clip;
            SoundManager.Instance.GamePlayAudio.mute = false;
            SoundManager.Instance.GamePlayAudio.Play();
            return;
        }

        MyDebug.Log("btnClick   SendMessage");
        string mes = "1|" + id;
        SocketSendManager.Instance.ChewTheRag(mes);
        BtnOnClick();
    }

    public void SenFace()
    {
        BtnOnClick();
    }

    Vector3 vRight = new Vector3(0, 185, 0);
    AudioClip clip;

    public void InitLocal(TalkItemData data)
    {
        BtnOnClick = null;
        talkInfo.SetActive(true);
        normalText.SetActive(false);
        clip = data.clip;
        localText.GetComponent<ContentSizeFitter>().enabled = true;
        textBg.SetActive(true);
        faceIcon.gameObject.SetActive(false);
        if (data.message != null)
        {
            localText.text = data.message;
        }
        else if (clip != null && clip.length > 0)
        {

            localText.text = "         '" + clip.length + "'";
        }
        else
        {
            textBg.SetActive(false);
            faceIcon.gameObject.SetActive(true);
            faceIcon.overrideSprite = data.faceSprite;
            faceIcon.SetNativeSize();
        }

        if (data.userId == GlobalDataScript.loginResponseData.account.uuid)
        {
            talkInfo.transform.localEulerAngles = vRight;
            localText.transform.localEulerAngles = vRight;
            iconImage.transform.localEulerAngles = vRight;
            faceIcon.transform.localEulerAngles = vRight;
        }
        else
        {
            talkInfo.transform.localEulerAngles = Vector3.zero;
            localText.transform.localEulerAngles = Vector3.zero;
            iconImage.transform.localEulerAngles = Vector3.zero;
            faceIcon.transform.localEulerAngles = Vector3.zero;
        }

        iconImage.sprite = data.icon;
        StartCoroutine(RefreshTextSize());
    }

    IEnumerator RefreshTextSize()
    {
        yield return new WaitForEndOfFrame();
        localText.GetComponent<ContentSizeFitter>().enabled = false;
    }
}