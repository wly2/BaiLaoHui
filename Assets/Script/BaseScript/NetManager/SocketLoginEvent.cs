using System.Runtime.InteropServices;
using AssemblyCSharp;
using LitJson;
using UnityEngine;
using System;
using System.Text;

public class SocketLoginEvent : SocketBaseEvent<SocketLoginEvent>
{
    public int currentKindId;
    public int main;
    public int sub;
    MDM_GR_LOGON loginMode;
    // 处理有效数据
    public override void OnEventTCPSocketRead(int main, int sub, byte[] tmpBuf, int size)
    {
        MyDebug.Log("Main:" + (MainCmd)main);
        switch ((MainCmd)main)
        {
            case MainCmd.MDM_GP_LOGON:                //1---
                OnSocketSubUpdateNotify(sub, tmpBuf, size);
                break;
            case MainCmd.MDM_MB_LOGON: //登陆信息    //100---
                OnSocketMainLogon(sub, tmpBuf, size);
                break;
            case MainCmd.MDM_MB_SERVER_LIST: //列表信息 //101---
                OnSocketMainServerList(sub, tmpBuf, size);
                break;
            case MainCmd.MDM_MB_PERSONAL_SERVICE:        //200---
                OnSocketPersonal_Service(sub, tmpBuf, size);
                break;
        }
    }

    private void OnSocketPersonal_Service(int sub, byte[] tmpBuf, int size)
    {
        switch ((MDM_MB_PERSONAL_SERVICE)sub)
        {
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_GAME_SERVER:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_GAME_SERVER_RESULT:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_SEARCH_SERVER_TABLE:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_SEARCH_RESULT:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_GET_PERSONAL_PARAMETER:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_PERSONAL_PARAMETER:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_PERSONAL_ROOM_LIST:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_PERSONAL_ROOM_LIST_RESULT:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_PERSONAL_FEE_PARAMETER:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_DISSUME_SEARCH_SERVER_TABLE:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_DISSUME_SEARCH_RESULT:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_USER_ROOM_INFO:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_GR_USER_QUERY_ROOM_SCORE:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_GR_USER_QUERY_ROOM_SCORE_RESULT:   //100---217
                UerQueryRoomScoreResult(tmpBuf, size);
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_GR_USER_QUERY_ROOM_SCORE_RESULT_FINSIH:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_PERSONAL_ROOM_USER_INFO:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_PERSONAL_ROOM_USER_INFO_RESULT:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_ROOM_CARD_EXCHANGE_TO_SCORE:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_GP_EXCHANGE_ROOM_CARD_RESULT:
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_GET_SINGLE_PERSONAL_SCORE:
                UerRoomRoundScore(tmpBuf);
                break;
            default:
                break;
        }
    }
      //收到战绩
    private void UerQueryRoomScoreResult(byte[] tmpBuf, int size)
    {

        MyDebug.Log("-------------：" + tmpBuf.Length);
        int itemSize = Marshal.SizeOf(typeof(tagQueryPersonalRoomUserScore));
        if (size % itemSize != 0)
            return;
        int iItemCount = size / itemSize;
        for (int i = 0; i < iItemCount; i++)
        {
            tagQueryPersonalRoomUserScore item =
                (tagQueryPersonalRoomUserScore)NetUtil.BytesToStruct(tmpBuf, typeof(tagQueryPersonalRoomUserScore), itemSize, i * itemSize);
            MainNewsDataManager.instance.AddQuerys(item, currentKindId);
        }
        SocketEngine.Instance.SocketQuit();
        SocketEventHandle.Instance.SetClientResponse(APIS.ZHANJI_REPORTER_REPONSE, null);

    }
    //收到小战绩
    private void UerRoomRoundScore(byte[] tmpBuf)
    {

        CMD_MB_ROUND_LIST roundList = NetUtil.BytesToStruct<CMD_MB_ROUND_LIST>(tmpBuf);
        MainNewsDataManager.instance.SetMyQueryList(roundList);
        SocketEngine.Instance.SocketQuit();
     
    }

    public override void OnEventTCPSocketLink()
    {
        SocketEngine.Instance.SendScoketData(main,
           sub, send_buffer, send_buffer.Length);
    }
    #region  列表信息处理

    // 列表信息处理
    private void OnSocketMainServerList(int sub, byte[] tmpBuf, int size)
    {
        switch ((SUB_MB_LIST)sub)
        {
            case SUB_MB_LIST.SUB_MB_LIST_KIND://101---100
                break;
            case SUB_MB_LIST.SUB_MB_LIST_SERVER://101---101
                OnSocketListServer(tmpBuf, size);
                break;
            case SUB_MB_LIST.SUB_MB_LIST_MATCH://101---102
                break;
            case SUB_MB_LIST.SUB_MB_LIST_FINISH://101---200
                OnSocketListFinish(tmpBuf, size);
                break;
            case SUB_MB_LIST.SUB_MB_GET_ONLINE://101---300
                break;
            case SUB_MB_LIST.SUB_MB_KINE_ONLINE://101---301
                break;
            case SUB_MB_LIST.SUB_MB_SERVER_ONLINE:// 101-- - 302
                break;
            case SUB_MB_LIST.SUB_MB_AGENT_KIND://101---400
                break;
            default:
                break;
        }
    }

    // 种类列表
    bool OnSocketListKind(byte[] data, int size)
    {
        ////更新数据
        int itemSize = Marshal.SizeOf(typeof(TagGameServer));
        if (size % itemSize != 0)
            return false;
        int iItemCount = size / itemSize;
        for (int i = 0; i < iItemCount; i++)
        {
            TagGameServer pGameKind =
                NetUtil.BytesToStruct<TagGameServer>(data);
            CServerListData.Instance.InsertGameServer(pGameKind);
        }

        return true;
    }

    // 房间列表
    bool OnSocketListServer(byte[] data, int size)
    {
        ////更新数据
        int itemSize = Marshal.SizeOf(typeof(TagGameServer));
        if (size % itemSize != 0)
            return false;
        int iItemCount = size / itemSize;
        for (int i = 0; i < iItemCount; i++)
        {
            TagGameServer pGameServer =
                (TagGameServer)NetUtil.BytesToStruct(data, typeof(TagGameServer), itemSize, i * itemSize);
            CServerListData.Instance.InsertGameServer(pGameServer);
        }

        return true;
    }

    // 列表完成
    bool OnSocketListFinish(byte[] data, int size)
    {
        //登陆完成通知
        ClientResponse response = new ClientResponse
        {
            headCode = APIS.LOGIN_RESPONSE,
            status = 1
        };
        SocketEventHandle.Instance.AddResponse(response);

        return true;
    }

    #endregion

    #region 登陆信息

    // 登陆信息处理
    void OnSocketMainLogon(int sub, byte[] tmpBuf, int size)
    {
        MyDebug.Log("登陆信息 Sub:" + (SUB_GP_LOGON_STATE)sub);
        switch ((SUB_GP_LOGON_STATE)sub)
        {
            case SUB_GP_LOGON_STATE.SUB_MB_LOGON_SUCCESS://100---100
                OnSocketSubLogonSuccess(tmpBuf, size);
                break;
            case SUB_GP_LOGON_STATE.SUB_MB_LOGON_FAILURE://100---101
                OnSocketSubLogonFailure(tmpBuf, size);
                break;
            case SUB_GP_LOGON_STATE.SUB_MB_MATCH_SIGNUPINFO://100---102
                OnSocketSubLogonFinish(tmpBuf, size);
                break;
            case SUB_GP_LOGON_STATE.SUB_MB_PERSONAL_TABLE_CONFIG://100---103
                OnSocketSubLogonValidateMBCard(tmpBuf, size);
                break;
            default:
                break;
        }
    }



    private void OnSocketSubMacthSignupInfo(byte[] tmpBuf, int size)
    {
    }

    // 登陆完成
    private void OnSocketSubLogonFinish(byte[] tmpBuf, int size)
    {
        SocketPIndividualEveng.Instance.ISocketEngineSink();
        //链接游戏服
    }

    // 升级提示
    private void OnSocketSubUpdateNotify(int sub, byte[] tmpBuf, int size)
    {
        if (sub == (int)SUB_GP_LOGON_STATE.SUB_MB_UPDATE_NOTIFY)
        {
            CMD_MB_UpdateNotify update = NetUtil.BytesToStruct<CMD_MB_UpdateNotify>(tmpBuf);
            // OnGPLoginFailure(updateTip.cbAdviceUpdate, NetUtil.BytesToString(logonError.szDescribeString));
        }
    }

    // 登陆失败
    private void OnSocketSubLogonValidateMBCard(byte[] tmpBuf, int size)
    {
    }

    // 登陆失败
    private void OnSocketSubLogonFailure(byte[] tmpBuf, int size)
    {
        CMD_MB_LogonFailure logonError =
            (CMD_MB_LogonFailure)NetUtil.BytesToStruct(tmpBuf, typeof(CMD_MB_LogonFailure), size);
        OnGPLoginFailure(logonError.lResultCode, NetUtil.BytesToString(logonError.szDescribeString));
    }

    //登陆成功
    private void OnSocketSubLogonSuccess(byte[] tmpBuf, int size)
    {
        GlobalDataScript.userData = NetUtil.BytesToStruct<CMD_MB_LogonSuccess>(tmpBuf);
        // string name = NetUtil.BytesToString(GlobalDataScript.userData.szNickName);      
        #region 
        //MyDebug.Log("登陆成功  222");
        //AvatarVO avatar = new AvatarVO
        //{
        //    account = new Account()
        //};
        //avatar.account.nickname = LoginData.wxUserInfo.nickname;
        //avatar.account.sex = LoginData.wxUserInfo.sex;
        //avatar.account.uuid = (int)logSucess.dwUserID;
        //avatar.account.headicon = LoginData.wxUserInfo.headimgurl;
        //avatar.account.roomcard = (int)logSucess.lUserScore;
        //if (GlobalDataScript.weChatInformation == null)
        //    GlobalDataScript.weChatInformation = new WeChatInformation();
        //GlobalDataScript.weChatInformation.weChatName = LoginData.wxUserInfo.nickname;
        //GlobalDataScript.weChatInformation.sex = LoginData.wxUserInfo.sex;
        //MyDebug.Log("登陆成功  333");
        //avatar.IP = "0"; // GlobalDataScript.getInstance().getIpAddress();
        //response.message = JsonMapper.ToJson(avatar);
        //GlobalDataScript.userData = logSucess;
        //MyDebug.Log("登陆成功   444");
        //SocketEventHandle.Instance.AddResponse(response);
        //MyDebug.Log("登陆成功    555");
        #endregion
    }



    //登陆失败
    void OnGPLoginFailure(uint iErrorCode, string szDescription)
    {
        MyDebug.Log("ErrorCode:" + iErrorCode);

        if (iErrorCode == 1 || iErrorCode == 3) //注册
        {
            SocketSendManager.Instance.RegisterAccount();
        }
        else
        {
            MyDebug.Log("Setsocketquit");
            SocketEngine.Instance.SocketQuit();
            SocketEventHandle.Instance.SetTips(szDescription);
        }
    }

    #endregion

    //获取战绩
    public void GetQueryPernalScore(int kingId)
    {
        currentKindId = kingId;
        CMD_GR_QUERY_USER_ROOM_SCORE userRoom = new CMD_GR_QUERY_USER_ROOM_SCORE();
        userRoom.dwUserID = GlobalDataScript.userData.dwUserID;
        userRoom.dwKindID = (uint)SocketLoginEvent.instance.currentKindId;
        SocketLoginEvent.instance.send_buffer = NetUtil.StructToBytes(userRoom);
        SocketLoginEvent.instance.ISocketEngineSink(APIS.socketUrl, APIS.socketPort);
    }
    //获取小战绩
    public void GetQueryRoundScore(byte[] roomid)
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UILoading);
        main = (int)MainCmd.MDM_MB_PERSONAL_SERVICE;
        sub = (int)MDM_MB_PERSONAL_SERVICE.SUB_MB_GET_SINGLE_PERSONAL_SCORE;
        CMD_MB_QueryPersonalScore userRoom = new CMD_MB_QueryPersonalScore();
        userRoom.szRoomID = roomid;
        send_buffer = NetUtil.StructToBytes(userRoom);
        ISocketEngineSink(APIS.socketUrl, APIS.socketPort);
    }    
    CMD_GP_LogonAccounts vxAccount;
    //游客登陆
    public void LogonVisitor()
    {
        LoginData.wxUserInfo = new WxUserInfo();

        main = (int)MainCmd.MDM_MB_LOGON;
        sub = (int)MDM_GR_LOGON.SUB_MB_LOGON_VISITOR;
        CMD_MB_LogonVisitor vist = new CMD_MB_LogonVisitor();
        vist.dwPlazaVersion = LoginData.PlazaVersion;
        vist.szMachine = new byte[66];
        vist.wModuleID = 65535;
        byte[] bt = NetUtil.StringToBytes(GlobalDataScript.szMachineID);
        Array.Copy(bt, vist.szMachine, bt.Length);

        send_buffer = NetUtil.StructToBytes(vist);

        ISocketEngineSink(APIS.socketUrl, APIS.socketPort);
        UIManager.instance.Show(UIType.UILoading);
        //发送

    }
    // 微信登陆成功
    public void OnWxLoginSucess(WxUserInfo kWxUserInfo)
    {
        main = (int)MainCmd.MDM_MB_LOGON;
        sub = (int)MDM_GR_LOGON.SUB_MB_LOGON_OTHERPLATFORM;
        LoginData.wxUserInfo = kWxUserInfo;
        vxAccount = new CMD_GP_LogonAccounts();
        vxAccount.dwPlazaVersion = LoginData.PlazaVersion;
        vxAccount.wModuleID = 65535;
        vxAccount.cbPlatformID = 5;

        //用户昵称                         
        vxAccount.szNickName = new byte[64];
        byte[] bt = NetUtil.StringToBytes(kWxUserInfo.nickname);
        Array.Copy(bt, vxAccount.szNickName, bt.Length);
        vxAccount.cbGender =(byte)kWxUserInfo.sex;
        //机器标识
        vxAccount.szMachineID = new byte[66];
        //bt = NetUtil.StringToBytes("A501164B366ECFC9E549163873094D50");
        bt = NetUtil.StringToBytes(kWxUserInfo.openid);
        Array.Copy(bt, vxAccount.szMachineID, bt.Length);
        //真实名字
        vxAccount.szCompellation = new byte[32];
        //电话号码
        vxAccount.szMobilePhone = new byte[24];
        //用户Uin
        vxAccount.szUserUin = new byte[66];
        bt = NetUtil.StringToBytes(kWxUserInfo.unionid);
        Array.Copy(bt, vxAccount.szUserUin, bt.Length);
        #region 
        //s bt = NetUtil.StringToBytes("WX" + LoginData.wxUserInfo.openid);
        //Array.Copy(bt, vxAccount.szAccounts, bt.Length);
        //bt = NetUtil.StringToBytes(LoginData.wxUserInfo.headimgurl);

        ////  Array.Copy(bt, vxAccount.szHeadHttp, bt.Length);
        //bt = NetUtil.StringToBytes("WeiXinPassword");
        //Array.Copy(bt, vxAccount.szPassword, bt.Length);
        //GlobalDataScript.tagUserData = new TagGlobalUserData
        //{
        //    szAccounts = vxAccount.szAccounts,
        //    //   szHeadHttp = vxAccount.szHeadHttp,
        //    szPassword = vxAccount.szPassword

        //};
        #endregion
        send_buffer = NetUtil.StructToBytes(vxAccount);
        ISocketEngineSink(APIS.socketUrl, APIS.socketPort);
        UIManager.instance.Show(UIType.UILoading);
        // HttpManager.instance.GetWXReaure(url, );     
    }


}