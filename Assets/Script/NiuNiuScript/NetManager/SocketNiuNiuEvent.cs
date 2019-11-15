using AssemblyCSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SocketNiuNiuEvent : SocketBaseEvent<SocketNiuNiuEvent>
{


    //不同kindid连接不同游戏服务器
    public override void ConnectGameServer()
    {
        ISocketEngineSink(GlobalDataScript.NN_KIND_ID);
    }
    public override void ConnectGameServerByServerID(int id)
    {
        ISocketEngineSinkByServerId(id);
    }

    //处理服务器返回消息
    public override void OnEventTCPSocketRead(int main, int sub, byte[] tmpBuf, int size)
    {
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

    //=======================================返回消息处理===========================================//
    //游戏框架命令子命令 100---
    private void Mdm_gf_Frame(int sub, byte[] tmpBuf)
    {
        switch ((SUB_GF_GAME_STATUS)sub)
        {
            case SUB_GF_GAME_STATUS.SUB_GF_GAME_OPTION:  //100---1
                Cmd_Gf_GameOption(tmpBuf);
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_USER_READY: //100---2
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_LOOKON_CONFIG: //100---3
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_USER_CHAT: //100---10
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_USER_EXPRESSION: //100---11
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_USER_VOICE: //100---12
                UserMessage(tmpBuf);
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_GAME_STATUS: //100---100
                GfGameStatus(tmpBuf);
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_GAME_SCENE: //100---101
                //断线重连
                GameScene(tmpBuf);
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_LOOKON_STATUS: //100---102
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_SYSTEM_MESSAGE: //100---200
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_ACTION_MESSAGE: //100---201
                break;
            case SUB_GF_GAME_STATUS.SUB_GF_PERSONAL_MESSAGE: //100---202
                break;
            default:
                break;
        }
    }

    private void GfGameStatus(byte[] tmpBuf)
    {
        CMD_GF_GameStatus _status = NetUtil.BytesToStruct<CMD_GF_GameStatus>(tmpBuf);
        nnstatus = _status.cbGameStatus;
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
    //断线重连
    private void GameScene(byte[] tmpBuf)
    {
        switch (nnstatus)
        {
            case 0:
                GameDataNN.Instance.statusFreeNN = NetUtil.BytesToStruct<CMD_S_StatusFreeNN>(tmpBuf);
                break;
            case 100://抢庄状态
                GameDataNN.Instance.statusCallNN = NetUtil.BytesToStruct<CMD_S_StatusCallNN>(tmpBuf);
                break;
            case 101://下注
                GameDataNN.Instance.statusScoreNN = NetUtil.BytesToStruct<CMD_S_StatusScoreNN>(tmpBuf);
                break;
            case 102://翻牌
                GameDataNN.Instance.statusPlayNN = NetUtil.BytesToStruct<CMD_S_StatusPlayNN>(tmpBuf);
                break;
        }
        SocketEventHandle.Instance.SetClientResponse(APIS.BACK_LOGIN_RESPONSE, null);
    }

    //连接服务器成功并发送数据
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

    //服务端游戏命令返回消息 200---
    private void OnEventGameMessage(int sub, byte[] tmpBuf, int size)
    {
        switch ((MDM_GP_NNGAMESERVER)sub)
        {
            case MDM_GP_NNGAMESERVER.SUB_S_GAME_START:
                OnEventGameStart(tmpBuf, size);
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_ADD_SCORE:
                OnEventXiaZhu(tmpBuf);
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_PLAYER_EXIT:
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_SEND_CARD:
                OnEventGetCard(tmpBuf, size);
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_GAME_END:
                OnEventNNGameOver(tmpBuf, size);
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_OPEN_CARD:
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_CALL_BANKER: //200---106
                OneventCallBanker(tmpBuf, size);
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_ALL_CARD:
                aaaOnEventGetCard(tmpBuf, size);
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_ANDROID_BANKOPERATOR:
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_ADMIN_STORAGE_INFO:
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_RESULT_ADD_USERROSTER:
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_RESULT_DELETE_USERROSTER:
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_UPDATE_USERROSTER:
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_REMOVEKEY_USERROSTER:
                break;
            case MDM_GP_NNGAMESERVER.SUB_S_DUPLICATE_USERROSTER:
                break;
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




    //=============================读取结构方法========================//
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
        //if (GlobalDataScript.Instance.roomInfo != null && GlobalDataScript.Instance.roomInfo.PlayGameCount > 0&&!isDisConnect)
        //   return;
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
        info.sex = tagUserInfo.cbGender;
        info.wFaceID = tagUserInfo.wFaceID;
        try
        {
            info.name = NetUtil.ChatsToString(tagUserInfo.szNickName);
        }
        catch
        {
            MyDebug.LogError("---------------------------------------------------------------------+++++++++++++++++++++++++++");
        }


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

    //返回叫庄
    public void OneventCallBanker(byte[] tmpBuf, int size)
    {
        GameDataNN.Instance.callBanker = NetUtil.BytesToStruct<CMD_S_CallBanker>(tmpBuf);
        MyDebug.Log("$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$ 叫庄倍数 $$$$$$$$$$$$$$$$$$$$$$$$$$" + GameDataNN.Instance.callBanker.bBanker);
        SocketEventHandle.Instance.SetClientResponse(APIS.BACK_BANKER, null);
    }

    //返回下注
    public void OnEventXiaZhu(byte[] tmpBuf)
    {
        GameDataNN.Instance.xiazhuInofoNN = NetUtil.BytesToStruct<CMD_S_AddScore>(tmpBuf);
        SocketEventHandle.Instance.SetClientResponse(APIS.BACK_XIAZHU, null);
        MyDebug.Log("==============================用户下注信息================================");
    }

    //开始游戏
    public void OnEventGameStart(byte[] tmpBuf, int size)
    {
        GameDataSSS.Instance.isSss = false;
        GameDataNN.Instance.gameStartInfo = NetUtil.BytesToStruct<CMD_S_NN_GameStart>(tmpBuf);
        SocketEventHandle.Instance.SetClientResponse(APIS.STARTGAME_RESPONSE_NOTICE, null);
    }

    //结束游戏
    public void OnEventGameOver(byte[] tmpBuf, int size)
    {
        CMD_GR_PersonalTableTip tipMessage = NetUtil.BytesToStruct<CMD_GR_PersonalTableTip>(tmpBuf);
        MyDebug.Log("" + NetUtil.BytesToStruct<CMD_GR_PersonalTableTip>(tmpBuf));
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
        MySceneManager.instance.SceneToNiuNiu();

    }

    //游戏配置
    private void Cmd_Gf_GameOption(byte[] tmpBuf)
    {
        CMD_GF_GameOption gameOption = NetUtil.BytesToStruct<CMD_GF_GameOption>(tmpBuf);
        if (GlobalDataScript.Instance.roomInfo == null)
            GlobalDataScript.Instance.roomInfo = new GameRoomInfo();

        GlobalDataScript.Instance.roomInfo.playerNum = gameOption.cb_game_rule[2];
        GlobalDataScript.Instance.roomInfo.payType = (int)gameOption.cb_game_rule[4];
        GlobalDataScript.Instance.roomInfo.gameMode = (int)gameOption.cb_game_rule[1];
        // GlobalDataScript.Instance.roomInfo.maPaiId = NetUtil.PuCardChange((PU_KE)gameOption.cb_game_rule[5]);

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
        isDisslove = true;
        CMD_GR_DissumeTable dissumeTable = NetUtil.BytesToStruct<CMD_GR_DissumeTable>(buffer);

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

    //返回发牌（自由抢庄）
    public void OnEventGetCard(byte[] buffer, int size)
    {
        GameDataNN.Instance.HandCard = NetUtil.BytesToStruct<CMD_NN_S_SendCard>(buffer);
        MyDebug.Log("-----------------------------自由抢庄手牌数据---------------------------");
        SocketEventHandle.Instance.SetClientResponse(APIS.PICKCARD_RESPONSE, null);

    }

    //明牌发牌
    public void aaaOnEventGetCard(byte[] buffer, int size)
    {
        GameDataNN.Instance.MPhandCard = NetUtil.BytesToStruct<CMD_S_AllCard>(buffer);
        SocketEventHandle.Instance.SetClientResponse(APIS.MPPICKCARD_RESPONSE, null);

    }



    //返回摊牌
    public void OnEventShowCard(byte[] buffer)
    {
        CMD_S_Open_Card showCard = NetUtil.BytesToStruct<CMD_S_Open_Card>(buffer);
        SocketEventHandle.Instance.SetClientResponse(APIS.USER_SHOW_CARD, null);

    }

    //返回单局游戏结束
    public void OnEventNNGameOver(byte[] buffer, int size)
    {
        GameDataNN.Instance.gameEnd = NetUtil.BytesToStruct<CMD_NN_S_GameEnd>(buffer);

        SocketEventHandle.Instance.SetClientResponse(APIS.HUPAIALL_RESPONSE, null);
    }

    //大结算返回消息
    public void OnEventAllGameOver(byte[] buffer)
    {
        GameDataNN.Instance.AllgameEnd = NetUtil.BytesToStruct<CMD_GR_PersonalTableEnd>(buffer);
        GlobalDataScript.Instance.isExitGame = true;
        if (GlobalDataScript.Instance.isSigGameOver)
        {
            GlobalDataScript.Instance.showAllGameEnd = true;
        }
    }

    public void RequestFailure(byte[] buffer)
    {
        CMD_GR_RequestFailure requestFailure = NetUtil.BytesToStruct<CMD_GR_RequestFailure>(buffer);
        SocketEngine.Instance.SocketQuit();
        MyDebug.Log("=============================请求失败====================" + NetUtil.BytesToString(requestFailure.szDescribeString));
        SocketEventHandle.Instance.SetTips(NetUtil.BytesToString(requestFailure.szDescribeString));
    }

    #endregion



    //==================================客户端发送请求==================================//
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
            return;
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
        myStatue = userStatus.UserStatus.cbUserStatus;
    }

    //发送用户选牌请求
    private void sendUserChooseCard(byte[] buffer, int size)
    {
        CMD_C_SelectCard sendCard = NetUtil.BytesToStruct<CMD_C_SelectCard>(buffer);

        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_GAME, (int)SUB_C_SSS.SUB_C_SELECT_CARD, NetUtil.StructToBytes(sendCard), Marshal.SizeOf(sendCard));
    }

    //服务端返回用户选牌
    public void OnEventChooseCard(byte[] buffer, int size)
    {
        CMD_S_SelectCard sendCard = NetUtil.BytesToStruct<CMD_S_SelectCard>(buffer);
        MyDebug.Log("========================用户选择的牌================================" + sendCard.w_selected_type);
        MyDebug.Log("========================选好后剩余的牌的可选牌型================================" + sendCard.w_left_card_type);

    }

    //创建房间成功
    public void CreateSuccess(byte[] tmpBuf, int size)
    {
        CMD_GR_CreateSuccess createSuccess = NetUtil.BytesToStruct<CMD_GR_CreateSuccess>(tmpBuf);
        MyDebug.Log("房间号" + NetUtil.BytesToString(createSuccess.szServerID));
        tableId = (ushort)createSuccess.dwTableId;
        CMD_GR_UserSitDown sitDown = new CMD_GR_UserSitDown();
        sitDown.wTableID = (ushort)tableId;
        sitDown.wChairID = 0;
        sitDown.szPassword = new byte[66];
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_SITDOWN, NetUtil.StructToBytes(sitDown), Marshal.SizeOf(sitDown));
    }

    //登录完成
    public void CreateLogonFinish(byte[] tmpBuf, int size)
    {
        if (GlobalDataScript.Instance.playerInfos == null)
            GlobalDataScript.Instance.playerInfos = new List<PlayerGameRoomInfo>();//登录完成初玩家始化信息

        if (GlobalDataScript.type == ModeType.Create)
            nnCreateRoom();
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

    //发送创建房间消息
    public void nnCreateRoom()
    {
        if (send_buffer != null && send_buffer.Length > 4)
            SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GP_Cretate, (int)MDM_GR_PRIVATE.SUB_GR_CREATE_TABLE, send_buffer, send_buffer.Length);
    }
}
