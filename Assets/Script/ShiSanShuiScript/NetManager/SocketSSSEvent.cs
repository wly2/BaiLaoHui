using AssemblyCSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SocketSSSEvent : SocketBaseEvent<SocketSSSEvent>
{

    //通过不同的kindID连接不同游戏的服务器
    public override void ConnectGameServer()
    {
        ISocketEngineSink(GlobalDataScript.SSS_KIND_ID);
    }
    public override void ConnectGameServerByServerID(int id)
    {
        ISocketEngineSinkByServerId(id);
    }


    //服务器连接成功并发送数据
    public override void OnEventTCPSocketLink()
    {

        CMD_GR_LogonMobile mobile = new CMD_GR_LogonMobile();
        mobile.wGameID = serverInfo.wServerID;
        mobile.dwProcessVersion = LoginData.PlazaVersion;
        mobile.cbDeviceType = 16;
        mobile.wBehaviorFlags = 17;
        mobile.wPageTableCount = 255;
        mobile.dwUserID = GlobalDataScript.userData.dwUserID;
        mobile.szPassword = GlobalDataScript.userData.szGroupName;
        mobile.szServerPasswd = new byte[66];
        mobile.szMachineID = new byte[66];
        byte[] bt = NetUtil.StringToBytes(GlobalDataScript.szMachineID);
        Array.Copy(bt, mobile.szMachineID, bt.Length);

        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_LOGON, (int)CreateLogon.SUB_GR_LOGON_MOBILE, NetUtil.StructToBytes(mobile), Marshal.SizeOf(mobile));
        return;
        #region 旧代码
        //if (GlobalDataScript.type == ModeType.Create)
        //{
        //    CMD_GR_LogonMobile mobile = new CMD_GR_LogonMobile();
        //    mobile.wGameID = serverInfo.wServerID;
        //    mobile.dwProcessVersion = LoginData.PlazaVersion;
        //    mobile.cbDeviceType = 16;
        //    mobile.wBehaviorFlags = 17;
        //    mobile.wPageTableCount = 255;
        //    mobile.dwUserID = GlobalDataScript.userData.dwUserID;
        //    mobile.szPassword = GlobalDataScript.userData.szGroupName;
        //    mobile.szServerPasswd = new byte[66];
        //    mobile.szMachineID = new byte[66];
        //    byte[] bt = NetUtil.StringToBytes("A501164B366ECFC9E549163873094D50");
        //    Array.Copy(bt, mobile.szMachineID, bt.Length);

        //    SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_LOGON, (int)CreateLogon.SUB_GR_LOGON_MOBILE, NetUtil.StructToBytes(mobile), Marshal.SizeOf(mobile));

        //}
        //else
        //{
        //    CMD_GR_UserSitDown sitDown = new CMD_GR_UserSitDown();
        //    sitDown.wTableID = (ushort)tableId;
        //    sitDown.wChairID = 0;
        //    sitDown.szPassword = new byte[66];
        //    SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_SITDOWN, NetUtil.StructToBytes(sitDown), Marshal.SizeOf(sitDown));
        //}
        #endregion
    }

    //=====================处理有效数据==============================//
    public override void OnEventTCPSocketRead(int main, int sub, byte[] tmpBuf, int size)

    {
        MyDebug.SocketLog(main + "GameServer:" + (GameServer)sub);
        switch ((GameServer)main)
        {
            case GameServer.MDM_GR_LOGON: //1---
                LogInCommon(sub, tmpBuf, size);
                break;
            case GameServer.MDM_GR_CONFIG://2---
                ConfigCommon(sub, tmpBuf, size);
                break;
            case GameServer.MDM_GR_USER://3---
                UserCommon(sub, tmpBuf, size);
                break;
            case GameServer.MDM_GR_STATUS://4---
                StateCommon(sub, tmpBuf, size);
                break;
            case GameServer.MDM_GR_INSURE://5---
                break;
            case GameServer.MDM_GR_TASK://6---
                break;
            case GameServer.MDM_GR_EXCHANGE://7---
                break;
            case GameServer.MDM_GR_PROPERTY://8---
                break;
            case GameServer.MDM_GF_FRAME://100---
                Mdm_gf_Frame(sub, tmpBuf);
                break;
            case GameServer.MDM_GF_GAME: //200---
                OnEventGameMessage(sub, tmpBuf, size);
                break;
            case GameServer.MDM_GP_Cretate://210---
                CreateRoomReslt(sub, tmpBuf, size);
                break;
        }
    }



    //=================================返回消息处理============================//

    //发送创建房间消息
    public void sssCreateRoom()
    {
        if (send_buffer != null && send_buffer.Length > 4)
            SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GP_Cretate, (int)MDM_GR_PRIVATE.SUB_GR_CREATE_TABLE, send_buffer, send_buffer.Length);
    }

    //创建房间登录结果返回消息 1---
    public void LogInCommon(int sub, byte[] buffer, int size)
    {
        switch ((ResCretaeLogon)sub)
        {
            case ResCretaeLogon.SUB_GR_LOGON_SUCCESS: //1---100
                LogInSucess(buffer, size);
                break;
            case ResCretaeLogon.SUB_GR_LOGON_FAILURE://1---101
                LogInFail(buffer, size);
                break;
            case ResCretaeLogon.SUB_GR_LOGON_FINISH://1---102
                CreateLogonFinish(buffer, size);
                break;
            case ResCretaeLogon.SUB_GR_UPDATE_NOTIFY://1---200
                break;
            default:
                break;
        }
    }

    //配置命令返回消息 2---
    public void ConfigCommon(int sub, byte[] buffer, int size)
    {
        switch ((MDM_GR_CONFIG)sub)
        {
            case MDM_GR_CONFIG.SUB_GR_CONFIG_COLUMN: //2---100
                break;
            case MDM_GR_CONFIG.SUB_GR_CONFIG_SERVER: //2---101
                ConFigReolve(buffer, size);
                break;
            case MDM_GR_CONFIG.SUB_GR_CONFIG_PROPERTY: //2---102
                break;
            case MDM_GR_CONFIG.SUB_GR_CONFIG_FINISH://2---103

                break;
            case MDM_GR_CONFIG.SUB_GR_CONFIG_USER_RIGHT://2---104
                break;
            default:
                break;
        }
    }

    //用户命令返回消息 3---
    public void UserCommon(int sub, byte[] buffer, int size)
    {
        switch ((MDM_GR_USER)sub)
        {
            //用户动作
            case MDM_GR_USER.SUB_GR_USER_RULE: //3---1
                break;
            case MDM_GR_USER.SUB_GR_USER_LOOKON://3---2
                break;
            case MDM_GR_USER.SUB_GR_USER_SITDOWN://3---3
                break;
            case MDM_GR_USER.SUB_GR_USER_STANDUP://3---4
                break;
            case MDM_GR_USER.SUB_GR_USER_INVITE://3---5
                break;
            case MDM_GR_USER.SUB_GR_USER_INVITE_REQ://3---6
                break;
            case MDM_GR_USER.SUB_GR_USER_REPULSE_SIT://3---7
                break;
            case MDM_GR_USER.SUB_GR_USER_KICK_USER://3---8
                break;
            case MDM_GR_USER.SUB_GR_USER_INFO_REQ://3---9
                break;
            case MDM_GR_USER.SUB_GR_USER_CHAIR_REQ://3---10
                break;
            case MDM_GR_USER.SUB_GR_USER_CHAIR_INFO_REQ://3---11
                break;
            case MDM_GR_USER.SUB_GR_USER_WAIT_DISTRIBUTE://3---12
                break;

            //用户状态
            case MDM_GR_USER.SUB_GR_USER_ENTER://3---100
                UserInfoHead(buffer, size);
                break;
            case MDM_GR_USER.SUB_GR_USER_SCORE://3---101
                break;
            case MDM_GR_USER.SUB_GR_USER_STATUS://3---102
                SuerStatus(buffer, size);
                break;
            case MDM_GR_USER.SUB_GR_SIT_FAILED://3---103
                RequestFailure(buffer);
                break;
            case MDM_GR_USER.SUB_GR_USER_GAME_DATA://3---104
                break;

            //聊天命令
            case MDM_GR_USER.SUB_GR_USER_CHAT://3---201
                break;
            case MDM_GR_USER.SUB_GR_USER_EXPRESSION://3---202
                break;
            case MDM_GR_USER.SUB_GR_WISPER_CHAT://3---203
                break;
            case MDM_GR_USER.SUB_GR_WISPER_EXPRESSION://3---204
                break;
            case MDM_GR_USER.SUB_GR_COLLOQUY_CHAT://3---205
                break;
            case MDM_GR_USER.SUB_GR_COLLOQUY_EXPRESSION://3---206
                break;

            //等级服务
            case MDM_GR_USER.SUB_GR_GROWLEVEL_QUERY://3---410
                break;
            case MDM_GR_USER.SUB_GR_GROWLEVEL_PARAMETER://3---411
                break;
            case MDM_GR_USER.SUB_GR_GROWLEVEL_UPGRADE://3---412
                break;
            default:
                break;
        }
    }

    //状态命令返回消息 4---
    public void StateCommon(int sub, byte[] buffer, int size)
    {
        switch ((MDM_GR_STATUS)sub)
        {
            case MDM_GR_STATUS.SUB_GR_TABLE_INFO: //4---100
                TableMessage(buffer, size);
                break;
            case MDM_GR_STATUS.SUB_GR_TABLE_STATUS://4---101
                break;
            default:
                break;
        }
    }

    //银行命令返回消息 5---
    public void BankCommon(int sub, byte[] buffer, int size)
    {
        switch ((MDM_GR_INSURE)sub)
        {
            //银行命令
            case MDM_GR_INSURE.SUB_GR_ENABLE_INSURE_REQUEST: //5---1
                break;
            case MDM_GR_INSURE.SUB_GR_QUERY_INSURE_INFO://5---2
                break;
            case MDM_GR_INSURE.SUB_GR_SAVE_SCORE_REQUEST://5---3
                break;
            case MDM_GR_INSURE.SUB_GR_TAKE_SCORE_REQUEST://5---4
                break;
            case MDM_GR_INSURE.SUB_GR_TRANSFER_SCORE_REQUEST://5---5
                break;
            case MDM_GR_INSURE.SUB_GR_QUERY_USER_INFO_REQUEST://5---6
                break;

            //开通银行
            case MDM_GR_INSURE.SUB_GR_USER_INSURE_INFO://5---100
                break;
            case MDM_GR_INSURE.SUB_GR_USER_INSURE_SUCCESS://5---101
                break;
            case MDM_GR_INSURE.SUB_GR_USER_INSURE_FAILURE://5---102
                break;
            case MDM_GR_INSURE.SUB_GR_USER_TRANSFER_USER_INFO://5---103
                break;
            case MDM_GR_INSURE.SUB_GR_USER_INSURE_ENABLE_RESULT://5---104
                break;
            default:
                break;
        }
    }

    //任务命令返回消息 6---
    public void TaskCommon(int sub, byte[] buffer, int size)
    {
        switch ((MDM_GR_TASK)sub)
        {
            case MDM_GR_TASK.SUB_GR_TASK_LOAD_INFO: //6---1
                break;
            case MDM_GR_TASK.SUB_GR_TASK_TAKE://6---2
                break;
            case MDM_GR_TASK.SUB_GR_TASK_REWARD://6---3
                break;
            case MDM_GR_TASK.SUB_GR_TASK_GIVEUP://6---4\
                break;


            case MDM_GR_TASK.SUB_GR_TASK_INFO://6---11
                break;
            case MDM_GR_TASK.SUB_GR_TASK_FINISH://6---12
                break;
            case MDM_GR_TASK.SUB_GR_TASK_LIST://6---13
                break;
            case MDM_GR_TASK.SUB_GR_TASK_RESULT://6---14
                break;
            case MDM_GR_TASK.SUB_GR_TASK_GIVEUP_RESULT://6---15
                break;
            default:
                break;
        }
    }

    //兑换命令返回消息 7---
    public void ExchanceCommon(int sub, byte[] buffer, int size)
    {
        switch ((MDM_GR_EXCHANGE)sub)
        {
            case MDM_GR_EXCHANGE.SUB_GR_EXCHANGE_LOAD_INFO: //7---400
                break;
            case MDM_GR_EXCHANGE.SUB_GR_EXCHANGE_PARAM_INFO://7---401
                break;
            case MDM_GR_EXCHANGE.SUB_GR_PURCHASE_MEMBER://7---402
                break;
            case MDM_GR_EXCHANGE.SUB_GR_PURCHASE_RESULT://7---403
                break;
            case MDM_GR_EXCHANGE.SUB_GR_EXCHANGE_SCORE_BYINGOT://7---404
                break;
            case MDM_GR_EXCHANGE.SUB_GR_EXCHANGE_SCORE_BYBEANS://7---405
                break;
            case MDM_GR_EXCHANGE.SUB_GR_EXCHANGE_RESULT://7---406
                break;
            default:
                break;
        }
    }

    //兑换命令返回消息 8---
    public void ToolCommon(int sub, byte[] buffer, int size)
    {
        switch ((MDM_GR_PROPERTY)sub)
        {
            case MDM_GR_PROPERTY.SUB_GR_QUERY_PROPERTY: //8---1
                break;
            case MDM_GR_PROPERTY.SUB_GR_GAME_PROPERTY_BUY://8---2
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_BACKPACK://8---3
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_USE://8---4
                break;
            case MDM_GR_PROPERTY.SUB_GR_QUERY_SEND_PRESENT://8---5
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_PRESENT://8---6
                break;
            case MDM_GR_PROPERTY.SUB_GR_GET_SEND_PRESENT://8---7
                break;


            case MDM_GR_PROPERTY.SUB_GR_QUERY_PROPERTY_RESULT://8---101
                break;
            case MDM_GR_PROPERTY.SUB_GR_GAME_PROPERTY_BUY_RESULT://8---102
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_BACKPACK_RESULT://8---103
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_USE_RESULT://8---104
                break;
            case MDM_GR_PROPERTY.SUB_GR_QUERY_SEND_PRESENT_RESULT://8---105
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_PRESENT_RESULT://8---106
                break;
            case MDM_GR_PROPERTY.SUB_GR_GET_SEND_PRESENT_RESULT://8---107
                break;
            case MDM_GR_PROPERTY.SUB_GR_QUERY_PROPERTY_RESULT_FINISH://8---111
                break;


            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_SUCCESS://8---201
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_FAILURE://8---202
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_MESSAGE://8---203
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_EFFECT://8---204
                break;
            case MDM_GR_PROPERTY.SUB_GR_PROPERTY_TRUMPET://8---205
                break;
            case MDM_GR_PROPERTY.SUB_GR_USER_PROP_BUFF://8---206
                break;
            case MDM_GR_PROPERTY.SUB_GR_USER_TRUMPET://8---207
                break;
            case MDM_GR_PROPERTY.SUB_GR_GAME_PROPERTY_FAILURE://8---404
                break;
            default:
                break;
        }
    }

    //框架命令子命令 100---
    private void Mdm_gf_Frame(int sub, byte[] tmpBuf)
    {
        switch ((SUB_GF_GAME_STATUS)sub)
        {
            case SUB_GF_GAME_STATUS.SUB_GF_GAME_OPTION:  //100---1
                Cmd_Gf_GameOption(tmpBuf);
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_USER_READY:
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_LOOKON_CONFIG:
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_USER_CHAT:
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_USER_EXPRESSION:
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_USER_VOICE:
                UserMessage(tmpBuf);
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_GAME_STATUS:
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_GAME_SCENE:
                //十三水断线重连
                GameScene(tmpBuf);
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_LOOKON_STATUS:
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_SYSTEM_MESSAGE:
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_ACTION_MESSAGE:
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_PERSONAL_MESSAGE:
                break;
            default:
                break;
        }
    }

    private void UserMessage(byte[] tmpBuf)
    {
        CMD_GF_S_UserVoice voice = NetUtil.BytesToStruct<CMD_GF_S_UserVoice>(tmpBuf);
        GlobalDataScript.Instance.messageInfo = new Chitchat();
        GlobalDataScript.Instance.messageInfo.userid = (int)voice.dwSendUserID;
        byte[] buffer = new byte[voice.dwVoiceLength];
        Array.Copy(tmpBuf, 12, buffer, 0, buffer.Length);
        GlobalDataScript.Instance.messageInfo.chatText = buffer;
        SocketEventHandle.Instance.SetClientResponse(APIS.MessageBox_Notice, null);

    }

    private void GameScene(byte[] tmpBuf)
    {
        GameDataSSS.Instance.gameStatusInfo = NetUtil.BytesToStruct<CMD_S_StatusPlaySSS>(tmpBuf);
        MyDebug.Log("-------------------------------------------------------------------------------st1111111111atau:" + myStatue);
        SocketEventHandle.Instance.SetClientResponse(APIS.BACK_LOGIN_RESPONSE, null);

    }

    //服务端游戏命令返回消息 200---
    private void OnEventGameMessage(int sub, byte[] tmpBuf, int size)
    {
        switch ((SUB_S_SSS)sub)
        {
            case SUB_S_SSS.SUB_S_SHOW_CARD: //200---203
                ShowPlayerCard(tmpBuf);
                break;
            case SUB_S_SSS.SUB_S_GAME_START://200---206
                OnEventGameStart(tmpBuf, size);
                break;
            case SUB_S_SSS.SUB_S_SEND_CARD://200---215
                OnEventSendCard(tmpBuf, size);
                break;
            case SUB_S_SSS.SUN_S_SELECT_CARD://200---216
                OnEventChooseCard(tmpBuf, size);
                break;
            case SUB_S_SSS.SUB_S_GAME_END://200---204
                OnEventGameOver(tmpBuf, size);
                break;
            case SUB_S_SSS.SUB_S_COMPARE_CARD://200---205
                CompareCard(tmpBuf);
                break;
            case SUB_S_SSS.SUB_S_PLAYER_EXIT://200---210
                break;
            case SUB_S_SSS.SUB_S_ANDROID_BANKOPERATOR://200---109
                break;
            case SUB_S_SSS.SUB_S_ADMIN_STORAGE_INFO://200---110
                break;
            case SUB_S_SSS.SUB_S_RESULT_ADD_USERROSTER://200---111
                break;
            case SUB_S_SSS.SUB_S_RESULT_DELETE_USERROSTER://200---112
                break;
            case SUB_S_SSS.SUB_S_UPDATE_USERROSTER://200---113
                break;
            case SUB_S_SSS.SUB_S_REMOVEKEY_USERROSTER://200---114
                break;
            case SUB_S_SSS.SUB_S_DUPLICATE_USERROSTER://200---115
                break;
            default:
                break;
        }
    }

    private void CompareCard(byte[] tmpBuf)
    {
        GameDataSSS.Instance.sCompare = NetUtil.BytesToStruct<CMD_S_Compare>(tmpBuf);
        SocketEventHandle.Instance.SetClientResponse(APIS.HUPAI_RESPONSE, null);
    }

    private void ShowPlayerCard(byte[] tmpBuf)
    {
        CMD_S_ShowCard showCard = NetUtil.BytesToStruct<CMD_S_ShowCard>(tmpBuf);
        GameDataSSS.Instance.playersCard = showCard;
        if (GlobalDataScript.Instance.myGameRoomInfo.chairId == showCard.wChairID)
        {

            if (showCard.m_b_dao_shui == 1)
            {
                SocketEventHandle.Instance.SetTips("请按照：第一墩<第二墩<第三墩 牌型排列！");
                SocketEventHandle.Instance.SetClientResponse(APIS.CHUPAI_RESPONSE, "0");
            }
            else
            {
                SocketEventHandle.Instance.SetClientResponse(APIS.CHUPAI_RESPONSE, "1");
            }
        }
        else
        {
            if (showCard.m_b_dao_shui == 1)
                return;

            SocketEventHandle.Instance.SetClientResponse(APIS.CHUPAI_RESPONSE, null);
        }
    }

    //游戏服私人房间状态 210---
    private void CreateRoomReslt(int sub, byte[] buffer, int size)
    {
        switch ((MDM_GR_PRIVATE)sub)
        {
            case MDM_GR_PRIVATE.SUB_GR_CREATE_TABLE: //210---1
                break;
            case MDM_GR_PRIVATE.SUB_GR_CREATE_SUCCESS://210---2
                CreateSuccess(buffer, size);
                break;
            case MDM_GR_PRIVATE.SUB_GR_CREATE_FAILURE://210---3
                CreateFail(buffer, size);
                break;
            case MDM_GR_PRIVATE.SUB_GR_CANCEL_TABLE://210---4
                SocketEventHandle.Instance.SetClientResponse(APIS.DISSOLIVE_ROOM_RESPONSE, null);
                break;
            case MDM_GR_PRIVATE.SUB_GR_CANCEL_REQUEST://210---5
                OnEventCancelRequest(buffer);

                break;
            case MDM_GR_PRIVATE.SUB_GR_REQUEST_REPLY://210---6
                OnEventRequestReply(buffer, size);
                break;
            case MDM_GR_PRIVATE.SUB_GR_REQUEST_RESULT://210---7
                OnEventCancelResult(buffer);
                break;
            case MDM_GR_PRIVATE.SUB_GR_WAIT_OVER_TIME://210---8
                break;
            case MDM_GR_PRIVATE.SUB_GR_PERSONAL_TABLE_TIP://210---9
                OnEventTipMessage(buffer, size);
                break;
            case MDM_GR_PRIVATE.SUB_GR_PERSONAL_TABLE_END://210---10
                OnEventAllGameOver(buffer);
                break;
            case MDM_GR_PRIVATE.SUB_GR_HOSTL_DISSUME_TABLE://210---11
                break;
            case MDM_GR_PRIVATE.SUB_GR_HOST_DISSUME_TABLE_RESULT://210---13
                break;
            case MDM_GR_PRIVATE.SUB_GR_CURRECE_ROOMCARD_AND_BEAN://210---16
                OnEnventDissumeTable(buffer, size);
                break;
            case MDM_GR_PRIVATE.SUB_GF_PERSONAL_MESSAGE:   //210---202
                SocketEngine.Instance.SocketQuit();
                SocketEventHandle.Instance.SetTips("银币不足，无法进入房间!");
                break;
            default:
                break;
        }
    }

    private void OnEventAllGameOver(byte[] buffer)
    {
        GameDataSSS.Instance.AllgameEnd = NetUtil.BytesToStruct<CMD_GR_PersonalTableEnd>(buffer);
        GlobalDataScript.Instance.isExitGame = true;
        if (GlobalDataScript.Instance.isSigGameOver)
        {
            GlobalDataScript.Instance.showAllGameEnd = true;
        }
    }




    //=============================服务器返回结构转换========================//
    #region 读取
    //创建房间时登录失败
    public void LogInFail(byte[] tmpBuf, int size)
    {
        CMD_GR_CreateLogonFailure createLoginFail = NetUtil.BytesToStruct<CMD_GR_CreateLogonFailure>(tmpBuf);
        SocketEventHandle.Instance.SetTips(NetUtil.BytesToString(createLoginFail.szDescribeString));
        SocketEngine.Instance.SocketQuit();
        MyDebug.Log("###############################" + NetUtil.BytesToString(createLoginFail.szDescribeString));
        MyDebug.Log("###############################" + createLoginFail.lErrorCode);
    }

    //创建房间登录成功
    public void LogInSucess(byte[] tmpBuf, int size)
    {
        CMD_GR_CreateLogonSuccess createLogonSucess = NetUtil.BytesToStruct<CMD_GR_CreateLogonSuccess>(tmpBuf);
    }

    //房间配置
    public void ConFigReolve(byte[] tmpBuf, int size)
    {
        CMD_GR_ConfigServer config = NetUtil.BytesToStruct<CMD_GR_ConfigServer>(tmpBuf);
        MyDebug.Log("###################################" + config);
    }

    //桌子信息
    public void TableMessage(byte[] tmpBuf, int size)
    {
        CMD_GR_TableInfo tableInfo = NetUtil.BytesToStruct<CMD_GR_TableInfo>(tmpBuf);

    }

    //用户进入返回用户信息
    public void UserInfoHead(byte[] tmpBuf, int size)
    {

        if (GlobalDataScript.Instance.playerInfos == null)
            GlobalDataScript.Instance.playerInfos = new List<PlayerGameRoomInfo>();//登录完成初玩家始化信息         
        MyDebug.Log("USerID:" + GlobalDataScript.userData.dwUserID);
        tagMobileUserInfoHead tagUserInfo = NetUtil.BytesToStruct<tagMobileUserInfoHead>(tmpBuf);
        if (tagUserInfo.wTableID != tableId)
            return;
        MyDebug.Log(tagUserInfo.dwUserID + "=======================" + size + "===table ID=========================" + tagUserInfo.wTableID);
        PlayerGameRoomInfo info = new PlayerGameRoomInfo();
        info.chairId = tagUserInfo.wChairID;
        info.tableId = tagUserInfo.wTableID;
        info.userID = (int)tagUserInfo.dwUserID;
        info.wFaceID = tagUserInfo.wFaceID;
        MyDebug.Log(tagUserInfo.szNickName.Length);
        try
        {
            info.name = NetUtil.ChatsToString(tagUserInfo.szNickName);
        }
        catch
        {
            MyDebug.LogError("---------------------------------------------------------------------+++++++++++++++++++++++++++");
        }
        MyDebug.Log(info.name);

        if (tagUserInfo.cbUserStatus == 2)
        {
            if (GlobalDataScript.type == ModeType.None)
                SocketEventHandle.Instance.SetClientResponse(APIS.JOIN_ROOM_NOICE, NetUtil.ObjToJson(info));
        }
        if (GlobalDataScript.userData.dwUserID == info.userID)
        {
            GlobalDataScript.Instance.myGameRoomInfo = info;
        }

        for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)
        {
            if (GlobalDataScript.Instance.playerInfos[i].userID == info.userID)
                return;
        }
        if (GlobalDataScript.Instance.roomInfo != null)
            MyDebug.Log(GlobalDataScript.Instance.roomInfo.PlayGameCount);
        MyDebug.Log(isDisConnect);
        GlobalDataScript.Instance.playerInfos.Add(info);

    }


    //创建房间失败
    public void CreateFail(byte[] tmpBuf, int size)
    {
        CMD_GR_CreateFailure createFail = NetUtil.BytesToStruct<CMD_GR_CreateFailure>(tmpBuf);
        SocketEventHandle.Instance.SetTips(NetUtil.BytesToString(createFail.szDescribeString));
        SocketEngine.Instance.SocketQuit();
        MyDebug.Log("创建房间失败" + NetUtil.BytesToString(createFail.szDescribeString));
    }

    //开始游戏
    public void OnEventGameStart(byte[] tmpBuf, int size)
    {
        CMD_S_GameStart gameStart = NetUtil.BytesToStruct<CMD_S_GameStart>(tmpBuf);
        MyDebug.Log("房间号" + (long)(gameStart.lUserScore));
        MyDebug.Log("房间号" + (ushort)(gameStart.wSpecialType));
        MyDebug.Log("房间号" + NetUtil.BytesToStruct<CMD_S_GameStart>(tmpBuf));
    }

    //结束游戏 单局---
    public void OnEventGameOver(byte[] tmpBuf, int size)
    {

        GameDataSSS.Instance.gameEnd = NetUtil.BytesToStruct<CMD_S_GameEnd>(tmpBuf);
        SocketEventHandle.Instance.SetClientResponse(APIS.HUPAIALL_RESPONSE, null);
    }

    //框架命令提示消息
    private void OnEventTipMessage(byte[] tmpBuf, int size)
    {
        CMD_GR_PersonalTableTip tipMessage = NetUtil.BytesToStruct<CMD_GR_PersonalTableTip>(tmpBuf);
        MyDebug.Log("" + NetUtil.BytesToStruct<CMD_GF_GameOptionNew>(tmpBuf));
        if (GlobalDataScript.Instance.roomInfo == null)
            GlobalDataScript.Instance.roomInfo = new GameRoomInfo();
        GlobalDataScript.Instance.roomInfo.roomId = NetUtil.BytesToString(tipMessage.szServerID);
        GlobalDataScript.Instance.roomInfo.limtNumber = tipMessage.dwDrawCountLimit.ToString();
        GlobalDataScript.Instance.roomInfo.PlayGameCount = (int)tipMessage.dwPlayCount;
        GlobalDataScript.Instance.roomInfo.tableOwnerUserID = tipMessage.dwTableOwnerUserID;//房主
        GlobalDataScript.Instance.roomInfo.InitScore = tipMessage.lCellScore;
        GlobalDataScript.Instance.roomInfo.InitBeishu = tipMessage.lIniScore;
        SocketEventHandle.Instance.SetClientResponse(APIS.SetRoomMark, null);
        MyDebug.Log("====================房主=====================" + GlobalDataScript.Instance.roomInfo.tableOwnerUserID);
        MySceneManager.instance.SceneToShiSanShui();

    }

    //发送扑克
    private void OnEventSendCard(byte[] tmpBuf, int size)
    {
        CMD_SSS_SendCard sendCard = NetUtil.BytesToStruct<CMD_SSS_SendCard>(tmpBuf);
        GlobalDataScript.Instance.cardInfo = new PlayerCardInfo();
        GlobalDataScript.Instance.cardInfo.cardlist = new int[sendCard.cbHandCardData.Length];
        GlobalDataScript.Instance.iswSpecialType = sendCard.wSpecialType;
        MyDebug.Log("-------------------------------+++++++++++++++++++++++++++++++++++++" + GlobalDataScript.Instance.iswSpecialType);
        for (int i = 0; i < sendCard.cbHandCardData.Length; i++)
        {
            MyDebug.Log("发送扑克发送扑克发送扑克发送扑克发送扑克:" + sendCard.cbHandCardData[i]);
            GlobalDataScript.Instance.cardInfo.cardlist[i] = NetUtil.PuCardChange((PU_KE)sendCard.cbHandCardData[i]);
        }
        GlobalDataScript.Instance.cardInfo.sortedList = new List<List<int>>();
        for (int i = 0; i < sendCard.cb_sorted_card.Length; i++)
        {
            List<int> list = new List<int>();
            for (int j = 0; j < sendCard.cb_sorted_card[i].sorted_card.Length; j++)
            {
                list.Add(NetUtil.PuCardChange((PU_KE)sendCard.cb_sorted_card[i].sorted_card[j]));
            }
            GlobalDataScript.Instance.cardInfo.sortedList.Add(list);
        }
        SetPuKType(sendCard.w_select_card_type);
        SocketEventHandle.Instance.SetClientResponse(APIS.STARTGAME_RESPONSE_NOTICE, null);

    }
    public void SetPuKType(ushort type)
    {
        if (GlobalDataScript.Instance.cardInfo == null)
            GlobalDataScript.Instance.cardInfo = new PlayerCardInfo();
        GlobalDataScript.Instance.cardInfo.selectcardList = new int[9];

        if ((type & (ushort)SUB_S_COMMONCARD.HT_ONE_DOUBLE) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[0] = 1;//只有一对
        }
        if ((type & (ushort)SUB_S_COMMONCARD.HT_TWO_DOUBLE) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[1] = 1;
            //  GlobalDataScript.Instance.cardInfo.selectcardList[0] = 1;
        }
        if ((type & (ushort)SUB_S_COMMONCARD.HT_THREE) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[2] = 1;
        }
        if ((type & (ushort)SUB_S_COMMONCARD.HT_LINE) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[3] = 1;
        }
        if ((type & (ushort)SUB_S_COMMONCARD.HT_COLOR) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[4] = 1;
        }
        if ((type & (ushort)SUB_S_COMMONCARD.HT_THREE_DEOUBLE) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[5] = 1;
        }
        if ((type & (ushort)SUB_S_COMMONCARD.HT_FOUR_BOOM) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[6] = 1;
        }
        if ((type & (ushort)SUB_S_COMMONCARD.HT_LINE_COLOR) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[7] = 1;
        }
        if ((type & (ushort)SUB_S_COMMONCARD.HT_FIVE) > 0)
        {
            GlobalDataScript.Instance.cardInfo.selectcardList[8] = 1;
        }
    }

    //游戏配置
    private void Cmd_Gf_GameOption(byte[] tmpBuf)
    {
        CMD_GF_GameOption gameOption = NetUtil.BytesToStruct<CMD_GF_GameOption>(tmpBuf);
        byte rule = gameOption.cb_game_rule[1];
        if (GlobalDataScript.Instance.roomInfo == null)
            GlobalDataScript.Instance.roomInfo = new GameRoomInfo();
        GlobalDataScript.Instance.roomInfo.gameMode = rule;
        GlobalDataScript.Instance.roomInfo.playerNum = gameOption.cb_game_rule[2];
        GlobalDataScript.Instance.roomInfo.payType = (int)gameOption.cb_game_rule[4];
        GlobalDataScript.Instance.roomInfo.maPaiId = NetUtil.PuCardChange((PU_KE)gameOption.cb_game_rule[5]);

    }

    //服务端返回用户选牌
    public void OnEventChooseCard(byte[] buffer, int size)
    {
        GameDataSSS.Instance.choiceCard.Clear();
        CMD_S_SelectCard sendCard = NetUtil.BytesToStruct<CMD_S_SelectCard>(buffer);
        if (sendCard.cb_select_card[4] > 0)
        {
            GameDataSSS.Instance.choiceCard.Clear();
            for (int i = 0; i < sendCard.cb_select_card.Length; i++)
            {
                int cardPoint = NetUtil.PuCardChange((PU_KE)sendCard.cb_select_card[i]);
                GameDataSSS.Instance.choiceCard.Add(cardPoint);
                GameDataSSS.Instance.lastCard.Remove(cardPoint);
            }
            MyDebug.Log("LastCard:" + GameDataSSS.Instance.lastCard.Count);
            MyDebug.Log("ChoicedCard:" + GameDataSSS.Instance.choiceCard.Count);
            SocketEventHandle.Instance.SetClientResponse(APIS.PICKCARD_RESPONSE, null);
            //用户选牌，返回选的牌
        }
        else
        {
            //用户放牌，剩余可选类型
            SetPuKType(sendCard.w_left_card_type);
            SocketEventHandle.Instance.SetClientResponse(APIS.RETURN_INFO_RESPONSE, null);
            MyDebug.Log("LastCard:" + GameDataSSS.Instance.lastCard.Count);
            MyDebug.Log("ChoicedCard:" + GameDataSSS.Instance.choiceCard.Count);
        }
        MyDebug.Log("========================用户选择的牌================================" + sendCard.w_selected_type);
        MyDebug.Log("========================选好后剩余的牌的可选牌型================================" + sendCard.w_left_card_type);

    }

    //申请解散房间者信息
    public void OnEventCancelRequest(byte[] buffer)
    {
        CMD_GR_CancelRequest cancelRequest = NetUtil.BytesToStruct<CMD_GR_CancelRequest>(buffer);
        SocketEventHandle.Instance.SetClientResponse(APIS.DISSOLIVE_ROOM_REQUEST, cancelRequest.dwUserID.ToString());
    }

    //申请解散房间返回用户选择信息
    public void OnEventRequestReply(byte[] buffer, int size)
    {
        CMD_GR_RequestReply requestReply = NetUtil.BytesToStruct<CMD_GR_RequestReply>(buffer);
        MyDebug.Log(requestReply.cbAgree + "================请求答复===================" + requestReply.dwUserID);
        DissloveRoomResponseVo vo = new DissloveRoomResponseVo();
        vo.result = requestReply.cbAgree;
        vo.userId = (int)requestReply.dwUserID;
        SocketEventHandle.Instance.SetClientResponse(APIS.DISSOLIVE_ROOM_RESPONSE, NetUtil.ObjToJson(vo));
    }

    //房主强制结算返回信息
    public void OnEnventDissumeTable(byte[] buffer, int size)
    {
        CMD_GR_DissumeTable dissumeTable = NetUtil.BytesToStruct<CMD_GR_DissumeTable>(buffer);
        isDisslove = true;
        SocketSendManager.Instance.StandUp();
    }

    //返回最终请求解散结果  
    public void OnEventCancelResult(byte[] buffer)
    {
        CMD_GR_RequestResult requestResult = NetUtil.BytesToStruct<CMD_GR_RequestResult>(buffer);
        MyDebug.Log("===============================" + requestResult.cbResult);
        DissloveRoomResponseVo vo = new DissloveRoomResponseVo();
        vo.result = requestResult.cbResult;
        vo.tableId = (int)requestResult.dwTableID;
        SocketEventHandle.Instance.SetClientResponse(APIS.DISSOLIVE_ROOM_RESPONSE, NetUtil.ObjToJson(vo));
        if (requestResult.cbResult == 1)
        {
            isDisslove = true;
        }

    }

    public void RequestFailure(byte[] buffer)
    {
        CMD_GR_RequestFailure requestFailure = NetUtil.BytesToStruct<CMD_GR_RequestFailure>(buffer);
        // SocketEventHandle.Instance.SetTips(NetUtil.BytesToString(requestFailure.szDescribeString));
        SocketEngine.Instance.SocketQuit();
        MyDebug.Log("=============================请求失败====================" + NetUtil.BytesToString(requestFailure.szDescribeString));
        SocketEventHandle.Instance.SetTips(NetUtil.BytesToString(requestFailure.szDescribeString));

    }



    //==================================客户端发送请求==================================//

    //发送用户选牌请求
    private void sendUserChooseCard(byte[] buffer, int size)
    {
        CMD_C_SelectCard sendCard = NetUtil.BytesToStruct<CMD_C_SelectCard>(buffer);

        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_GAME, (int)SUB_C_SSS.SUB_C_SELECT_CARD, NetUtil.StructToBytes(sendCard), Marshal.SizeOf(sendCard));
    }

    //用户状态
    private void SuerStatus(byte[] buffer, int size)
    {
        MyDebug.Log("Set User Statue!!!!!!");
        CMD_GR_UserStatus userStatus = NetUtil.BytesToStruct<CMD_GR_UserStatus>(buffer);
        if (GlobalDataScript.Instance.isExitGame)
            return;
        if (userStatus.UserStatus.cbUserStatus == 1 && myStatue != userStatus.UserStatus.cbUserStatus)    //站立
        {
            PlayerGameRoomInfo info = new PlayerGameRoomInfo();
            info.userID = (int)userStatus.dwUserID;
            SocketEventHandle.Instance.SetClientResponse(APIS.OUT_ROOM_RESPONSE, NetUtil.ObjToJson(info));
            for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)
            {
                if (GlobalDataScript.Instance.playerInfos[i].userID == (int)userStatus.dwUserID)
                {
                    GlobalDataScript.Instance.playerInfos.RemoveAt(i);
                }
            }
        }
        if (userStatus.dwUserID != GlobalDataScript.userData.dwUserID)
            return;
        tableId = userStatus.UserStatus.wTableID;
        chairId = userStatus.UserStatus.wChairID;

        CMD_GR_ChairUserInfoReq chairInfo = new CMD_GR_ChairUserInfoReq();
        chairInfo.wChairID = (byte)chairId;
        chairInfo.wTableID = (byte)tableId;
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_CHAIR_INFO_REQ, NetUtil.StructToBytes(chairInfo), Marshal.SizeOf(chairInfo));


        byte[] bytes = new byte[10];
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_FRAME, (int)SUB_GF_GAME_STATUS.SUB_GF_GAME_OPTION, bytes, 10);
        MyDebug.Log("st*****************************************************************************************atau:" + myStatue);
        MyDebug.Log("userStatus.UserStatus.cbUserStatus:" + userStatus.UserStatus.cbUserStatus);
        myStatue = userStatus.UserStatus.cbUserStatus;
        MyDebug.Log("statau:" + myStatue);
    }

    //创建房间成功
    public void CreateSuccess(byte[] tmpBuf, int size)
    {
        CMD_GR_CreateSuccess createSuccess = NetUtil.BytesToStruct<CMD_GR_CreateSuccess>(tmpBuf);
        MyDebug.Log("房间号" + NetUtil.BytesToString(createSuccess.szServerID));
        tableId = (int)createSuccess.dwTableId;
        //CMD_GR_UserSitDown sitDown = new CMD_GR_UserSitDown();
        //sitDown.wTableID = (ushort)tableId;
        //sitDown.wChairID = 0xff;
        //sitDown.szPassword = new byte[66];
        //SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_SITDOWN, NetUtil.StructToBytes(sitDown), Marshal.SizeOf(sitDown));


    }

    //登录完成
    public void CreateLogonFinish(byte[] tmpBuf, int size)
    {


        if (GlobalDataScript.Instance.playerInfos == null)
            GlobalDataScript.Instance.playerInfos = new List<PlayerGameRoomInfo>();//登录完成初玩家始化信息

        if (GlobalDataScript.type == ModeType.Create)
            sssCreateRoom();
        else
        {

            if ((playStatue)myStatue < playStatue.US_SIT)
            {
                CMD_GR_UserSitDown sitDown = new CMD_GR_UserSitDown();
                sitDown.wTableID = (ushort)tableId;
                sitDown.wChairID = 0xff;
                //if (chairId == -1)
                //    sitDown.wChairID = 0xff;
                //else if ((playStatue)myStatue >= playStatue.US_SIT)
                //    sitDown.wChairID = (byte)chairId;
                sitDown.szPassword = new byte[66];
                SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_SITDOWN, NetUtil.StructToBytes(sitDown), Marshal.SizeOf(sitDown));
                return;
            }

            CMD_GR_ChairUserInfoReq chairInfo = new CMD_GR_ChairUserInfoReq();
            chairInfo.wChairID = (byte)chairId;
            chairInfo.wTableID = (byte)tableId;

            SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_CHAIR_INFO_REQ, NetUtil.StructToBytes(chairInfo), Marshal.SizeOf(chairInfo));
        }
    }
}


#endregion