using AssemblyCSharp;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class UIPanel_ExitRoom : UIWindow
{
    public byte[] send_buffer;
    List<PlayerGameRoomInfo> playerList;
    public Text Tip;

    private void Start()
    {
        otherTip();
    }

    public void otherTip()
    {
        if (Tip == null)
            return;
        //判断如果只有房主，强制解散
        if (GlobalDataScript.Instance.roomInfo.tableOwnerUserID == GlobalDataScript.Instance.myGameRoomInfo.userID)
        {
            if (GlobalDataScript.Instance.roomInfo.PlayGameCount == 0)
            {
                Tip.text = "是否解散房间？";
            }
            else
            {
                Tip.text = "是否解散房间？";
            }
        }
        else
        {
            if (GlobalDataScript.Instance.roomInfo.PlayGameCount > 0)
            {

                Tip.text = "是否解散房间？";
            }
            else
            {
                //自己退出--发送站立，退场景
                Tip.text = "是否退出房间？";

            }
        }

    }

    public void ExitTheRoom()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        //判断如果只有房主，强制解散
        if (GlobalDataScript.Instance.roomInfo.tableOwnerUserID == GlobalDataScript.Instance.myGameRoomInfo.userID)
        {
            if (GlobalDataScript.Instance.roomInfo.PlayGameCount == 0)
            {
                //强制
                MyDebug.Log("=========================房主====================" + GlobalDataScript.Instance.roomInfo.tableOwnerUserID);
                CMD_GR_HostDissumeGame HostJ = new CMD_GR_HostDissumeGame();
                HostJ.dwUserID = (uint)GlobalDataScript.Instance.myGameRoomInfo.userID;
                HostJ.dwTableID = (uint)GlobalDataScript.Instance.myGameRoomInfo.tableId;
                MyDebug.Log("=====================桌子 I D====================" + HostJ.dwTableID);
                MyDebug.Log("=====================桌子 I D====================" + HostJ.dwUserID);

                SocketSendManager.Instance.SendData((int)GameServer.MDM_GP_Cretate,
                       (int)MDM_GR_PRIVATE.SUB_GR_HOSTL_DISSUME_TABLE, NetUtil.StructToBytes(HostJ), Marshal.SizeOf(HostJ));
                MyDebug.Log("===============强制解散================" + HostJ);
            }
            else
            {
                //申请解散
                CMD_GR_CancelRequest CancelRequest = NetUtil.BytesToStruct<CMD_GR_CancelRequest>(send_buffer);
                CancelRequest.dwUserID = GlobalDataScript.userData.dwUserID;
                CancelRequest.dwTableID = (uint)GlobalDataScript.Instance.myGameRoomInfo.tableId;
                CancelRequest.dwChairID = (uint)GlobalDataScript.Instance.myGameRoomInfo.chairId;
                SocketSendManager.Instance.SendData((int)GameServer.MDM_GP_Cretate,
                       (int)MDM_GR_PRIVATE.SUB_GR_CANCEL_REQUEST, NetUtil.StructToBytes(CancelRequest), Marshal.SizeOf(CancelRequest));
                SetMySelfDisslove();
            }
        }
        else
        {
            if (GlobalDataScript.Instance.roomInfo.PlayGameCount > 0)
            {

                CMD_GR_CancelRequest CancelRequest = NetUtil.BytesToStruct<CMD_GR_CancelRequest>(send_buffer);
                CancelRequest.dwUserID = GlobalDataScript.userData.dwUserID;
                CancelRequest.dwTableID = (uint)GlobalDataScript.Instance.myGameRoomInfo.tableId;
                CancelRequest.dwChairID = (uint)GlobalDataScript.Instance.myGameRoomInfo.chairId;
                SocketSendManager.Instance.SendData((int)GameServer.MDM_GP_Cretate,
                       (int)MDM_GR_PRIVATE.SUB_GR_CANCEL_REQUEST, NetUtil.StructToBytes(CancelRequest), Marshal.SizeOf(CancelRequest));
                SetMySelfDisslove();
            }
            else
            {
                //自己退出--发送站立，退场景
                SocketSSSEvent.instance.isDisslove = true;
                SocketNiuNiuEvent.instance.isDisslove = true;
                SocketSendManager.Instance.StandUp();

            }
        }

        CloseUI();
    }

    public void SetMySelfDisslove()
    {
        UIManager.instance.Show(UIType.UIExitRoom, (go) => { go.GetComponent<UIPanel_DissloveRoom>().Init(GlobalDataScript.Instance.myGameRoomInfo.userID); });
    }

    public void CancelButton()
    {
        CloseUI();
    }
}