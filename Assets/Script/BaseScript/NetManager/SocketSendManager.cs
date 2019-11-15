using AssemblyCSharp;
using System;
using System.Runtime.InteropServices;

public class SocketSendManager
{
    private static SocketSendManager _instance;

    public static SocketSendManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SocketSendManager();
            }

            return _instance;
        }
    }

    byte[] sendData = new byte[20480]; //发送的数据，必须为字节

    // 发送心跳包
    public void SendHeadData()
    {
        byte[] pDataBuffer = new byte[NetUtil.SOCKET_TCP_PACKET + Marshal.SizeOf(typeof(TCP_Head))];
        pDataBuffer = NetUtil.StringToBytes("523he1215515135451d");
        System.Random rr = new System.Random();
        int sizel = rr.Next(5, 17);
        SocketEngine.Instance.SendScoketData(NetUtil.MDM_KN_COMMAND, NetUtil.SUB_KN_DETECT_SOCKET, pDataBuffer, sizel);
    }

    //聊天消息
    public void ChewTheRag(string _Chat)
    {
        CMD_GF_C_UserVoice userVoice = new CMD_GF_C_UserVoice();  
        byte[] buffer = NetUtil.StringToBytes(_Chat);
        userVoice.dwTargetUserID = 0;
        userVoice.dwVoiceLength =(uint)buffer.Length;
        byte[] buffer1 = NetUtil.StructToBytes(userVoice);   
        sendData = new byte[8 + buffer.Length];
        Array.Copy(buffer1, sendData, buffer1.Length);
        Array.Copy(buffer, 0, sendData, buffer1.Length, buffer.Length);       
        SocketEngine.Instance.SendScoketData((int) GameServer.MDM_GF_FRAME, (int)SUB_GF_GAME_STATUS.SUB_GF_USER_VOICE, sendData,
           sendData.Length);         
    }
    //语音聊天消息
    public void ChewTheRag(byte[] buffer)
    {
        CMD_GF_C_UserVoice userVoice = new CMD_GF_C_UserVoice();
        userVoice.dwTargetUserID = 0;
        userVoice.dwVoiceLength = (uint)buffer.Length;
        byte[] buffer1 = NetUtil.StructToBytes(userVoice);
        MyDebug.TestLog("buffer.Length==" + buffer.Length);
        sendData = new byte[8 + buffer.Length];
        Array.Copy(buffer1, sendData, buffer1.Length);
        Array.Copy(buffer, 0, sendData, buffer1.Length, buffer.Length);
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_FRAME, (int)SUB_GF_GAME_STATUS.SUB_GF_USER_VOICE, sendData,
           sendData.Length);
    }

    //  账号登陆
    CMD_GP_LogonAccounts vxLoginAccount;

    public void LoginAccount(CMD_GP_LogonAccounts LoginAccount)
    {

        byte[] ret = NetUtil.StructToBytes(LoginAccount);
        //发送
        SocketEngine.Instance.SendScoketData((int)MainCmd.MDM_MB_LOGON,
            (int)MDM_GR_LOGON.SUB_MB_LOGON_OTHERPLATFORM, ret, Marshal.SizeOf(LoginAccount));

    }


    //账号注册
    public void RegisterServer(CMD_GP_RegisterAccounts RegisterAccount)
    {
        //清空发送缓存
        sendData = new byte[20480];
        //数据类型转换
        sendData = NetUtil.StructToBytes(RegisterAccount);
        //发送
        SocketEngine.Instance.SendScoketData((int)MainCmd.MDM_MB_LOGON,
            (int)MDM_GR_LOGON.SUB_MB_REGISTER_ACCOUNTS, sendData, Marshal.SizeOf(RegisterAccount));
    }

    //注册信息
    public void RegisterAccount()
    {
        if (LoginData.wxUserInfo.openid == "")
        {
            return;
        }

        CMD_GP_RegisterAccounts kRegister = new CMD_GP_RegisterAccounts
        {
            dwPlazaVersion = LoginData.PlazaVersion,
            cbValidateFlags = LoginData.MB_VALIDATE_FLAGS | LoginData.LOW_VER_VALIDATE_FLAGS,
            cbGender = (byte)LoginData.wxUserInfo.sex,
            wFaceID = 0,

        };
        //kRegister.szAccounts = vxLoginAccount.;
        //kRegister.szLogonPass = vxLoginAccount.szPassword;
        kRegister.szNickName = new byte[32];

        byte[] bt = NetUtil.StringToBytes(LoginData.wxUserInfo.nickname);
        Array.Copy(bt, kRegister.szNickName, bt.Length);
        //  kRegister.szHeadHttp = vxLoginAccount.szHeadHttp;
        RegisterServer(kRegister);
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    /// <param name="rule"> </param>
    /// <param name="payFor"> </param>
    /// <param name="playCount"> </param>
    public void CreateRoom(int rule, int payFor, int playCount)
    {
        GlobalDataScript.type = ModeType.Create;
        CMD_GR_Create_Private kSendNet = new CMD_GR_Create_Private
        {
            //cbGameType = (byte) RoomType.Type_Private, //房间规则   私人   公共大厅
            bGameRuleIdex = (uint)rule,
            w_player_count = 0,
            cb_pay_type = (byte)payFor,
            bGameTypeIdex = GlobalDataScript.GAME_TYPE_SUSONG,
            bPlayCoutIdex = (byte)playCount
        };
        m_SendNet = kSendNet;
    }

    private CMD_GR_Create_Private m_SendNet;
    private uint jointRoomNum;

    /// <summary>
    /// 创建与服务器链接的ID
    /// </summary>
    public void ConnectPrivateByKindID()
    {
        TagGameServer tagServer = CServerListData.Instance.GetTagServerByKindID(GlobalDataScript.SSS_KIND_ID);
        string ip = NetUtil.BytesToString(tagServer.szServerAddr);
        SocketInGameEvent.Instance.ISocketEngineSink(ip, tagServer.wServerPort);
    }

    public void SendLogonPacket()
    {
        CMD_GR_LogonUserID LogonUserID = new CMD_GR_LogonUserID
        {
            wKindID = (ushort)GlobalDataScript.SSS_KIND_ID,
            ////游戏版本
            dwProcessVersion = LoginData.DwProcessVersion,
            dwPlazeVersion = LoginData.PlazaVersion,
            ////登录信息
            dwUserID = GlobalDataScript.userData.dwUserID,
            szPassword = GlobalDataScript.tagUserData.szPassword,
            szHeadHttp = GlobalDataScript.tagUserData.szHeadHttp
        };
        byte[] buffer = NetUtil.StructToBytes(LogonUserID);
        //发送数据
        //  SocketEngine.Instance.SendScoketData((int) GameServer.MDM_GR_LOGON, (int) MDM_GR_LOGON.SUB_GR_LOGON_USERID,buffer, Marshal.SizeOf(LogonUserID));
    }

    //加入房间
    public void JointRoom(int num)
    {
        GlobalDataScript.type = ModeType.Join;
        int iServerId = num / 10000 - 10;
        jointRoomNum = (uint)num;
        ConnectGameServerByServerID(iServerId);
    }

    public void ConnectGameServerByServerID(int serverId)
    {
        TagGameServer tagServer = CServerListData.Instance.GetTagServerByServerID(serverId);
        if (tagServer.wKindID == 0)
        {
            UIManager.instance.Show(UIType.UITipsDialog,
                (go) => { go.GetComponent<UIPanel_TipsDialog>().SetMes("房间不存在"); });
            return;
        }

        string ip = NetUtil.BytesToString(tagServer.szServerAddr);
        SocketInGameEvent.Instance.ISocketEngineSink(ip, tagServer.wServerPort);
    }

    public void SendData(int main, int sub, byte[] temp, int size)
    {
        SocketEngine.Instance.SendScoketData(main, sub, temp, size);
    }

    public void OnSocketSubPrivateInfo()
    {
        if (GlobalDataScript.type == ModeType.Create)
        {
            byte[] temp = NetUtil.StructToBytes(m_SendNet);
            //SendData((int) GameServer.MDM_GR_PRIVATE, (int) MDM_GR_PRIVATE.SUB_GR_CREATE_PRIVATE, temp,
            //    Marshal.SizeOf(m_SendNet));
        }

        if (GlobalDataScript.type == ModeType.Join)
        {
            CMD_GR_Join_Private kSendNet;
            kSendNet.dwRoomNum = jointRoomNum;
            byte[] temp = NetUtil.StructToBytes(kSendNet);
            //SendData((int) GameServer.MDM_GR_PRIVATE, (int) MDM_GR_PRIVATE.SUB_GR_JOIN_PRIVATE, temp,
            //    Marshal.SizeOf(kSendNet));
        }
    }

    public void GetGameOption()
    {
        CMD_GF_GameOption GameOption = new CMD_GF_GameOption();
        ;
        //构造数据
        GameOption.dwFrameVersion = 9; //LoginData.DwProcessVersion;
        GameOption.cbAllowLookon = 0;
        GameOption.dwClientVersion = 9; // LoginData.DwProcessVersion;
        byte[] temp = NetUtil.StructToBytes(GameOption);
        //发送数据
        //Instance.SendData((int) GameServer.MDM_GF_FRAME, (int) MDM_GF_FRAME.SUB_GF_GAME_OPTION, temp,
        //    Marshal.SizeOf(GameOption));
    }

    //麻将
    public void SendStandUpPacket(int wTableID, int wChairID, byte cbForceLeave = 0)
    {
        CMD_GR_UserStandUp UserStandUp = new CMD_GR_UserStandUp
        {
            //构造数据
            wTableID = (ushort)wTableID,
            wChairID = (ushort)wChairID,
            cbForceLeave = cbForceLeave
        };
        //发送数据
        SendData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_STANDUP,
            NetUtil.StructToBytes(UserStandUp), Marshal.SizeOf(UserStandUp));
    }

    //十三水
    public void SendCancelRoomPacket(int wTableID, int wChairID, int dwUserID)
    {
        CMD_GR_CancelRequest UserStandUp = new CMD_GR_CancelRequest
        {
            //构造数据
            dwTableID = (uint)wTableID,
            dwChairID = (uint)wChairID,
            dwUserID = (uint)dwUserID,
        };
        //发送数据
        SendData((int)GameServer.MDM_GR_USER, (int)MDM_GR_PRIVATE.SUB_GR_CANCEL_REQUEST,
            NetUtil.StructToBytes(UserStandUp), Marshal.SizeOf(UserStandUp));
    }
    //用户准备
    public void SendPlayerReady()
    {
        byte[] bytes = new byte[10];
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_FRAME, (int)SUB_GF_GAME_STATUS.SUB_GF_USER_READY, bytes, 10);
    }
    public void StandUp()
    {
        CMD_GR_UserStandUp standUp = new CMD_GR_UserStandUp();
        standUp.wTableID = (ushort)GlobalDataScript.Instance.myGameRoomInfo.tableId;
        standUp.wChairID = (ushort)GlobalDataScript.Instance.myGameRoomInfo.chairId;
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_STANDUP, NetUtil.StructToBytes(standUp), Marshal.SizeOf(standUp));
        MySceneManager.instance.BackToMain();
        GlobalDataScript.Instance.ClearGameInfo();    
    }

    //牛牛叫庄
    public void sendCallBaker(int dex)
    {
        CMD_C_CallBanker callBanker = new CMD_C_CallBanker();
        callBanker.bBanker = (byte)dex;
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_GAME, (int)MDM_GP_CLIENT.SUB_C_CALL_BANKER, NetUtil.StructToBytes(callBanker), Marshal.SizeOf(callBanker));
    }

    //牛牛下注
    public void sendXZbeishu(int dex)
    {
        CMD_C_AddScore addScore = new CMD_C_AddScore();
        addScore.lScore = dex;
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_GAME, (int)MDM_GP_CLIENT.SUB_C_ADD_SCORE, NetUtil.StructToBytes(addScore), Marshal.SizeOf(addScore));

    }

    //牛牛亮牌
    public void sendShowCard()
    {
        CMD_C_OxCard bOX = new CMD_C_OxCard();
        bOX.bOX = 0;
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_GAME, (int)MDM_GP_CLIENT.SUB_C_OPEN_CARD, NetUtil.StructToBytes(bOX), Marshal.SizeOf(bOX));
        MyDebug.Log("------------------用户不叫庄------------------------");
    }

}