using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MicroPhoneInput : MonoBehaviour
{
    //==============单例==============//       
    public static MicroPhoneInput instance;
    public float sensitivity = 100;
    public float loudness;
    private string[] micArray;
    const int HEADER_SIZE = 44;
    const int RECORD_TIME = 10;
    private AudioClip redioclip;

    // Use this for initialization
    void Start()
    {
        //  SocketEventHandle.Instance.micInputReply += MicInputNotice;              
        if (SoundManager.Instance.GamePlayAudio.clip == null)
        {
            SoundManager.Instance.GamePlayAudio.clip = AudioClip.Create("playRecordClip", 7500, 1, 600, false);// AudioClip.Create("playRecordClip", 160000, 1, 8000,false);
        }
    }
    private void OnEnable()
    {
        instance = this;
        micArray = Microphone.devices;
        if (micArray.Length == 0)
        {
            MyDebug.LogError("Microphone.devices is null");
        }

        for (int i = 0; i < Microphone.devices.Length; ++i)
        {
            MyDebug.Log("device name = " + Microphone.devices[i]);
        }

        if (micArray.Length == 0)
        {
            MyDebug.LogError("no mic device");
        }
        SocketEventHandle.Instance.micInputReply += MicInputNotice;
    }

    private void OnDisable()
    {
        instance = null;
        SocketEventHandle.Instance.micInputReply -= MicInputNotice;
    }
    public void StartRecord()
    {
        SoundManager.Instance.GamePlayAudio.Stop();
        if (micArray.Length == 0)
        {
            MyDebug.TestLog("No Record Device!");
            return;
        }
        redioclip = Microphone.Start("inputMicro", false, RECORD_TIME, 600); //22050
        while (!(Microphone.GetPosition(null) > 0))
        {
        }
        MyDebug.TestLog("StartRecord");
    }

    public void StopRecord()
    {
        MyDebug.TestLog("StopRecord");
        if (micArray.Length == 0)
        {
            MyDebug.TestLog("No Record Device!");
            return;
        }
        if (!Microphone.IsRecording(null))
        {
            MyDebug.TestLog("StopRecord 1111111111");
            return;
        }

        MyDebug.TestLog("StopRecord 2222222222222");
        Microphone.End(null);
        MyDebug.TestLog("StopRecord 3333333333333");
        SoundManager.Instance.GamePlayAudio.clip = redioclip;
        MyDebug.TestLog("StopRecord 444444444444444444");
       // Test(GetClipData());
          SocketSendManager.Instance.ChewTheRag(GetClipData());          
    }

    public Byte[] GetClipData()
    {
        if (SoundManager.Instance.GamePlayAudio.clip == null)
        {
            MyDebug.TestLog("GetClipData audio.clip is null");
            return null;
        }
        MyDebug.TestLog("GetClipData audio.clip is 1111111111");
        var samples = new float[SoundManager.Instance.GamePlayAudio.clip.samples];
        MyDebug.TestLog("samples.Length = " + samples.Length);
        SoundManager.Instance.GamePlayAudio.clip.GetData(samples, 0);
        var outData = new byte[samples.Length * 2];
        MyDebug.TestLog("GetClipData audio.clip is 2222022222222");
        //Int16[] intData = new Int16[samples.Length];
        //converting in 2 float[] steps to Int16[], //then Int16[] to Byte[]
        const int rescaleFactor = 32767; //to convert float to Int16
        for (int i = 0; i < samples.Length; i++)
        {
            var temshort = (short)(samples[i] * rescaleFactor);
            var temdata = BitConverter.GetBytes(temshort);
            outData[i * 2] = temdata[0];
            outData[i * 2 + 1] = temdata[1];
        }
        MyDebug.TestLog("GetClipData audio.clip is 3333333333333");
        if (outData == null || outData.Length <= 0)
        {
            MyDebug.TestLog("GetClipData intData is null");
            return null;
        }
        MyDebug.TestLog("GetClipData audio.clip is 41444444444444");
        //return intData;
        return outData;
    }


    public void PlayClipData(Int16[] intArr)
    {
        if (intArr.Length == 0)
        {
            MyDebug.Log("get intarr clipdata is null");
            return;
        }

        MyDebug.Log("PlayClipData");
        //从Int16[]到float[]
        var samples = new float[intArr.Length];
        const int rescaleFactor = 32767;
        for (int i = 0; i < intArr.Length; i++)
        {
            samples[i] = (float)intArr[i] / rescaleFactor;
        }

        SoundManager.Instance.GamePlayAudio.clip.SetData(samples, 0);
        SoundManager.Instance.GamePlayAudio.mute = false;
        SoundManager.Instance.GamePlayAudio.Play();
    }

    private void PlayRecord()
    {
        if (SoundManager.Instance.GamePlayAudio.clip == null)
        {
            MyDebug.Log("audio.clip=null");
            return;
        }

        SoundManager.Instance.GamePlayAudio.mute = false;
        SoundManager.Instance.GamePlayAudio.loop = false;
        SoundManager.Instance.GamePlayAudio.Play();
    }

    private IEnumerator TimeDown()
    {
        var time = 0;
        while (time < RECORD_TIME)
        {
            if (!Microphone.IsRecording(null))
            {
                //如果没有录制
                MyDebug.Log("IsRecording false");
                yield break;
            }

            MyDebug.Log("yield return new WaitForSeconds " + time);
            yield return new WaitForSeconds(1);
            time++;
        }

        if (time >= RECORD_TIME)
        {
            MyDebug.Log("RECORD_TIME is out! stop record!");
            StopRecord();
        }

        yield return 0;
    }

    public void MicInputNotice(ClientResponse response)
    {
        MyDebug.Log("micInputNotice");
        if (GlobalDataScript.soundToggle)
        {
            byte[] data = GlobalDataScript.Instance.messageInfo.chatText;
            int i = 0;
            List<short> result = new List<short>();
            while (data.Length - i >= 2)
            {
                result.Add(BitConverter.ToInt16(data, i));
                i += 2;
            }

            var arr = result.ToArray(); //这就是你要的
            PlayClipData(arr);
        }
    }
    public void Test(byte[] data)
    {
        MyDebug.Log("micInputNotice");
        int i = 0;
        List<short> result = new List<short>();
        while (data.Length - i >= 2)
        {
            result.Add(BitConverter.ToInt16(data, i));
            i += 2;
        }

        var arr = result.ToArray(); //这就是你要的
        PlayClipData(arr);
    }
}