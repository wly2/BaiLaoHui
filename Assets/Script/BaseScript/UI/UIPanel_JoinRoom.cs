using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;
using System;
using LitJson;

public class UIPanel_JoinRoom : UIWindow
{
    public Button button_sure, button_delete, button_clear; //确认删除按钮
    private List<String> inputChars; //输入的字符
    public List<Text> inputTexts;
    public List<GameObject> btnList;

    void Start()
    {
        SocketEventHandle.Instance.joinRoomReply += OnJoinRoomReply;
        //button_clear.gameObject.SetActive(true);
        button_sure.gameObject.SetActive(false);
        inputChars = new List<String>();
        for (int i = 0; i < btnList.Count; i++)
        {
            int index;
            index = i != btnList.Count - 1 ? i + 1 : 0;
            btnList[i].GetComponent<Button>().onClick.AddListener(delegate () { OnClickHandle(index); });
        }

    }

    public void OnClickHandle(int index)
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        ClickNumber(index.ToString());
    }

    private void ClickNumber(string number)
    {
        MyDebug.Log(number.ToString());
        if (inputChars.Count >= 6)
        {
            return;
        }

        inputChars.Add(number);
        int index = inputChars.Count;
        inputTexts[index - 1].text = number.ToString();
        if (index == inputTexts.Count)
        {
            SureRoomNumber();
            //button_sure.gameObject.SetActive(true);
            //button_clear.gameObject.SetActive(true);
        }
    }

    public void DeleteNumber()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));

        if (inputChars != null && inputChars.Count > 0)
        {

            inputChars.RemoveAt(inputChars.Count - 1);
            inputTexts[inputChars.Count].text = "";
        }
    }

    public void ClearNumber()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        for (int i = 0; i < inputChars.Count; i++)
        {
            inputTexts[i].text = "";
        }

        inputChars.Clear();
        //button_clear.gameObject.SetActive(true);
        button_sure.gameObject.SetActive(false);
    }

    private void RemoveListener()
    {
        SocketEventHandle.Instance.joinRoomReply -= OnJoinRoomReply;
    }

    //确认加入房间
    public void SureRoomNumber()
    {
        UIManager.instance.Show(UIType.UILoading);
        GlobalDataScript.type = ModeType.Join;
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));

        String roomNumber = inputChars[0] + inputChars[1] + inputChars[2] + inputChars[3] + inputChars[4] +
                            inputChars[5];
        MyDebug.Log("##############################roomNumber" + roomNumber);


        CMD_MB_SearchServerTable table = new CMD_MB_SearchServerTable();


        table.szServerID = new byte[14];
        byte[] bt = NetUtil.StringToBytes(roomNumber);
        Array.Copy(bt, table.szServerID,bt.Length);

        ScoketJRevent.instance.Init();
        ScoketJRevent.instance.send_buffer = NetUtil.StructToBytes(table);
        ScoketJRevent.instance.ConnectGameServer();//连接游戏服

        #region 旧代码
        //RoomJoinVo roomJoinVo = new RoomJoinVo
        //{
        //    roomId = int.Parse(roomNumber)
        //};
        //int iServerId = roomJoinVo.roomId / 10000 - 10;
        //SocketSendManager.Instance.JointRoom(roomJoinVo.roomId);
        #endregion
    }



    public void OnJoinRoomReply(ClientResponse response)
    {
        if (response.status == 1)
        {
            GlobalDataScript.roomJoinResponseData = JsonMapper.ToObject<RoomJoinResponseVo>(response.message);
            GlobalDataScript.roomVo.addWordCard = GlobalDataScript.roomJoinResponseData.addWordCard;
            GlobalDataScript.roomVo.hong = GlobalDataScript.roomJoinResponseData.hong;
            GlobalDataScript.roomVo.ma = GlobalDataScript.roomJoinResponseData.ma;
            GlobalDataScript.roomVo.name = GlobalDataScript.roomJoinResponseData.name;
            GlobalDataScript.roomVo.roomId = GlobalDataScript.roomJoinResponseData.roomId;
            GlobalDataScript.roomVo.roomType = GlobalDataScript.roomJoinResponseData.roomType;
            GlobalDataScript.roomVo.roundNumber = GlobalDataScript.roomJoinResponseData.roundNumber;
            GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.roomJoinResponseData.sevenDouble;
            GlobalDataScript.roomVo.xiaYu = GlobalDataScript.roomJoinResponseData.xiaYu;
            GlobalDataScript.roomVo.ziMo = GlobalDataScript.roomJoinResponseData.ziMo;
            GlobalDataScript.roomVo.magnification = GlobalDataScript.roomJoinResponseData.magnification;
            GlobalDataScript.surplusTimes = GlobalDataScript.roomJoinResponseData.roundNumber;
            GlobalDataScript.loginResponseData.roomId = GlobalDataScript.roomJoinResponseData.roomId;
            GlobalDataScript.type = ModeType.Join;
            GlobalDataScript.homePanel.SetActive(false);

        }
        else
        {
            TipsManagerScript.getInstance.setTips(response.message);
        }
    }
}