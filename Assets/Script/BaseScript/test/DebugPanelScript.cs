using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using AssemblyCSharp;
using LitJson;

public class DebugPanelScript : MonoBehaviour
{
    public Text roomInputText;
    public Text remarkText;
    public Text loginText;
    public Text cardPointInput;
    public Text CardPointInput2;
    private List<SocketForDebugPanel> socketList;
    private List<AvatarVO> avatarList;
    public List<int> userList;
    private Hashtable ava_socket;

    void Start()
    {
        socketList = new List<SocketForDebugPanel>();
        avatarList = new List<AvatarVO>();
        ava_socket = new Hashtable();
    }

    public void LoginNewUse()
    {
        if (socketList.Count >= 4)
        {
            remarkText.text = LocalizationManager.GetInstance.GetValue("KEY.20016");
        }
        else
        {
            SocketForDebugPanel tempsocket = new SocketForDebugPanel();
            tempsocket.LoginCallBack_debug += LoginCallBack;
            socketList.Add(tempsocket);
            tempsocket.sendMsg(new LoginRequest(null));
        }
    }

    public void JointRoom()
    {
        int RoomId = int.Parse(roomInputText.text);
        if (RoomId != 0)
        {
            RoomJoinVo roomJoinVo = new RoomJoinVo();
            roomJoinVo.roomId = RoomId;
            for (int i = 0; i < socketList.Count; i++)
            {
                socketList[i].sendMsg(new JoinRoomRequest(JsonMapper.ToJson(roomJoinVo)));
            }
        }
        else
        {
            remarkText.text = LocalizationManager.GetInstance.GetValue("KEY.20017");
            roomInputText.text = "";
        }
    }

    public void LoginCallBack(ClientResponse response)
    {
        AvatarVO temp = JsonMapper.ToObject<AvatarVO>(response.message);
        avatarList.Add(temp);
        string str = "";
        for (int i = 0; i < avatarList.Count; i++)
        {
            str += string.Format(LocalizationManager.GetInstance.GetValue("KEY.11025"),
                avatarList[i].account.nickname + "\n");
        }

        loginText.text = str;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void CheckOnClick()
    {
        int cardPoint = Int32.Parse(cardPointInput.text);
    }

    public void CheckGangClick2()
    {
        int cardPoint = Int32.Parse(CardPointInput2.text);
    }

    public void StartRecord()
    {
        MicroPhoneInput.instance.StartRecord();
    }

    public void StopRecord()
    {
        MicroPhoneInput.instance.StopRecord();
    }
}