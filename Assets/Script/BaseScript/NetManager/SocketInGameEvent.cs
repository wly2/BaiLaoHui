using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AssemblyCSharp;

public class SocketInGameEvent : ISocketEvent
{
    public int netMs;
    private static SocketInGameEvent _instance;

    public static SocketInGameEvent Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SocketInGameEvent();
            }

            return _instance;
        }
    }

    public string socketip;
    int socketPort;

    /// <summary>
    /// 连接不同kindId游戏服务器
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="port"></param>
    public void ISocketEngineSink(string ip, int port)
    {
        socketip = ip;
        socketPort = port;
        ISocketEngineSink();
    }

    public void ISocketEngineSink()
    {
        SocketEngine.Instance.SetSocketEvent(this);
        if (socketPort > 0 && socketip != null)
            SocketEngine.Instance.InitSocket(socketip, socketPort);
        UIManager.instance.Show(UIType.UILoading);
    }

    public bool OnEventTCPHeartTick()
    {
        return true; //TODO:
    }

    public void OnEventTCPSocketError(int errorCode)
    {
    }

    public void OnEventTCPSocketLink()
    {
        SocketSendManager.Instance.SendLogonPacket();
    }

    public void OnEventTCPSocketRead(int main, int sub, byte[] tmpBuf, int size)
    {
        MyDebug.Log("onEventTCPSocketRead的main:" + main);
        switch (main)
        {
            //登录信息
            case (int)(GameServer.MDM_GR_LOGON): //1
                {
                    OnSocketMainLogon(sub, tmpBuf, size);
                    break;
                } //配置信息
            case (int)(GameServer.MDM_GR_CONFIG): //2
                {
                    OnSocketMainConfig(sub, tmpBuf, size);
                    break;
                } //用户信息
            case (int)(GameServer.MDM_GR_USER): //3
                {
                    OnSocketMainUser(sub, tmpBuf, size);
                    break;
                } //状态信息
            case (int)(GameServer.MDM_GR_STATUS): //4
                {
                    OnSocketMainStatus(sub, tmpBuf, size);
                    break;
                } //系统消息
                  //     case (int)(GameServer.MDM_CM_SYSTEM): //1000
                {
                  //  OnSocketMainSystem(sub, tmpBuf, size);
                    break;
                } //游戏消息
            case (int)(GameServer.MDM_GF_GAME): //200
                {
                    OnEventGameMessage(sub, tmpBuf, size);
                    break;
                }
                //框架消息
                //   case (int)(GameServer.MDM_GF_FRAME): //100
                {
                    OnSocketMainGameFrame(sub, tmpBuf, size);
                    break;
                } //比赛消息
                  //    case (int)(GameServer.MDM_GR_MATCH): //9
                {
                    OnSocketMainMatch(sub, tmpBuf, size);
                    break;
                } //四人场消息
                  //   case (int)(GameServer.MDM_GR_PRIVATE): //10
                {
                    OnSocketMainPrivate(sub, tmpBuf, size);
                    break;
                }
        }
    }

    //登录信息
    private bool OnSocketMainLogon(int sub, byte[] data, int dataSize)
    {
        switch (sub)
        {
            //登录成功
            case (int)(MDM_GR_LOGON.SUB_MB_LOGON_GAMEID): //1---100
                {
                    return OnSocketSubLogonSuccess(data, dataSize);
                } //登录失败
            case (int)(MDM_GR_LOGON.SUB_MB_LOGON_ACCOUNTS): //1---101
                {
                    return OnSocketSubLogonFailure(data, dataSize);
                } //登录完成
            case (int)(MDM_GR_LOGON.SUB_MB_REGISTER_ACCOUNTS): //1---102
                {
                    return OnSocketSubLogonFinish(data, dataSize);
                } //更新提示
            case (int)(MDM_GR_LOGON.SUB_MB_LOGON_OTHERPLATFORM): //1---200
                {
                    return OnSocketSubUpdateNotify(data, dataSize);
                } //响应录像列表
            case (int)(MDM_GR_LOGON.SUB_MB_LOGON_VISITOR): //1---412
                {
                    return OnSocketSubReplayList(data, dataSize);
                } //响应录像数据
            case (int)(MDM_GR_LOGON.MDM_MB_LOGON): //1---413
                {
                    return OnSocketSubReplayData(data, dataSize);
                }
        }

        return true;
    }

    //配置信息
    private bool OnSocketMainConfig(int sub, byte[] data, int dataSize)
    {
        switch (sub)
        {
            //列表配置
            case (int)(MDM_GR_CONFIG.SUB_GR_CONFIG_COLUMN): //2---100
                {
                    return OnSocketSubConfigColumn(data, dataSize);
                } //房间配置
            case (int)(MDM_GR_CONFIG.SUB_GR_CONFIG_SERVER): //2---101
                {
                    return OnSocketSubConfigServer(data, dataSize);
                } //道具配置
            case (int)(MDM_GR_CONFIG.SUB_GR_CONFIG_PROPERTY): //2---102
                {
                    return OnSocketSubConfigOrder(data, dataSize);
                } //配置玩家权限
            case (int)(MDM_GR_CONFIG.SUB_GR_CONFIG_USER_RIGHT): //2---104
                {
                    return OnSocketSubConfigMmber(data, dataSize);
                } //配置完成
            case (int)(MDM_GR_CONFIG.SUB_GR_CONFIG_FINISH): //2---103
                {
                    return OnSocketSubConfigFinish(data, dataSize);
                }
        }

        return true;
    }

    //用户信息
    private bool OnSocketMainUser(int sub, byte[] data, int dataSize)
    {
        MyDebug.Log("3的sub:" + sub);
        switch (sub)
        {
            //请求坐下失败
            case (int)(MDM_GR_USER.SUB_GR_SIT_FAILED): //3---103
                {
                    return OnSocketSubRequestFailure(data, dataSize);
                } //用户进入
            case (int)(MDM_GR_USER.SUB_GR_USER_ENTER): //3---100
                {
                    OnSocketSubUserEnter(data, dataSize);
                    break;
                } //用户积分
            case (int)(MDM_GR_USER.SUB_GR_USER_SCORE): //3---101
                {
                    return OnSocketSubUserScore(data, dataSize);
                } //用户状态
            case (int)(MDM_GR_USER.SUB_GR_USER_STATUS): //3---102
                {
                    OnSocketSubUserStatus(data, dataSize);
                    break;
                }
            //用户聊天
            case (int)(MDM_GR_USER.SUB_GR_USER_CHAT): //3---201
                {
                    return OnSocketSubUserChat(data, dataSize);
                } //用户表情
            case (int)(MDM_GR_USER.SUB_GR_USER_EXPRESSION): //3---202
                {
                    return OnSocketSubExpression(data, dataSize);
                } //用户私聊
            case (int)(MDM_GR_USER.SUB_GR_WISPER_CHAT): //3---203
                {
                    return OnSocketSubWisperUserChat(data, dataSize);
                } //私聊表情
            case (int)(MDM_GR_USER.SUB_GR_WISPER_EXPRESSION): //3---204
                {
                    return OnSocketSubWisperExpression(data, dataSize);
                }
                //道具成功
                //case (int)(MDM_GR_USER.SUB_GR_PROPERTY_SUCCESS): //3---301
                {
                    return OnSocketSubPropertySuccess(data, dataSize);
                } //道具失败
                  //  case (int)(MDM_GR_USER.SUB_GR_PROPERTY_FAILURE): //3---302
                {
                    return OnSocketSubPropertyFailure(data, dataSize);
                } //道具效应
                  //case (int)(MDM_GR_USER.SUB_GR_PROPERTY_EFFECT): //3---304
                {
                    return OnSocketSubPropertyEffect(data, dataSize);
                } //礼物消息
                  //case (int)(MDM_GR_USER.SUB_GR_PROPERTY_MESSAGE): //3---303
                {
                    return OnSocketSubPropertyMessage(data, dataSize);
                } //喇叭消息
                  // case (int)(MDM_GR_USER.SUB_GR_PROPERTY_TRUMPET): //3---305
                {
                    return OnSocketSubPropertyTrumpet(data, dataSize);
                } //喜报消息
                  //case (int)(MDM_GR_USER.SUB_GR_GLAD_MESSAGE): //3---400
                {
                    return OnSocketSubGladMessage(data, dataSize);
                }
        }

        return true;
    }

    //状态信息
    private bool OnSocketMainStatus(int sub, byte[] data, int dataSize)
    {
        switch (sub)
        {
            //桌子信息
            case (int)(MDM_GR_STATUS.SUB_GR_TABLE_INFO): //4---100
                {
                    return OnSocketSubStatusTableInfo(data, dataSize);
                } //桌子状态
            case (int)(MDM_GR_STATUS.SUB_GR_TABLE_STATUS): //4---101
                {
                    return OnSocketSubStatusTableStatus(data, dataSize);
                }
        }

        return true;
    }

    //游戏命令
    private void OnEventGameMessage(int sub, byte[] tmpBuf, int size)
    {
        MyDebug.Log("OnEventGameMessage:" + sub);
        switch ((SUB_S)sub)
        {
            case SUB_S.SUB_S_GAME_START: //200---100  游戏开始
                OnSubGameStart(tmpBuf, size);
                break;
            case SUB_S.SUB_S_OUT_CARD: //200---101    出牌命令
                OnSubOutCard(tmpBuf, size);
                break;
            case SUB_S.SUB_S_SEND_CARD: //200-102     发送扑克
                OnSubSendCard(tmpBuf, size);
                break;
            case SUB_S.SUB_S_OPERATE_NOTIFY: //200---104  操作提示
                OnSubOperateNotify(tmpBuf, size);
                break;
            case SUB_S.SUB_S_OPERATE_RESULT: //200---105   操作命令
                OnSubOperateResult(tmpBuf, size);
                break;
            case SUB_S.SUB_S_GAME_END: //200---106    游戏结束
                OnSubGameEnd(tmpBuf, size);
                break;
            case SUB_S.SUB_S_TRUSTEE: //200---107    用户托管
                OnSubTrustee(tmpBuf, size);
                break;
            case SUB_S.SUB_S_USER_INFO: //200---110     用户信息
                break;
            case SUB_S.SUB_S_CHAT: //200---112   聊天命令
                OnSubUserChat(tmpBuf, size);
                break;
            case SUB_S.SUB_S_CHU_ZENG: //200---112 出弹消息
                OnSubChuZeng(tmpBuf, size);
                break;
            case SUB_S.SUB_S_CHU_ZENG_RESULT: //200---113  出弹消息回应
                OnSubChuZengResult(tmpBuf, size);
                break;
        }
    }

    private void OnSubUserChat(byte[] tmpBuf, int size)
    {
        var _Chat = NetUtil.BytesToStruct<CMD_S_Chat>(tmpBuf);
        var chitchat = new Chitchat
        {
            userid = _Chat.wChairID,
            chatText = _Chat.szTitle
        };
        SetClientResponse(APIS.MessageBox_Notice, NetUtil.ObjToJson(chitchat));
        return;
    }

    //框架消息
    private bool OnSocketMainGameFrame(int sub, byte[] data, int dataSize)
    {
        switch (sub)
        {
            case (int)(MDM_GF_FRAME.SUB_GF_USER_CHAT): //100---10      //用户聊天
                {
                    return OnSocketSubUserChat(data, dataSize);
                }
            case (int)(MDM_GF_FRAME.SUB_GR_TABLE_TALK): //100---12      //用户聊天
                {
                    return OnSocketSubUserTalk(data, dataSize);
                }
            case (int)(MDM_GF_FRAME.SUB_GF_USER_EXPRESSION): //100---11   //用户表情
                {
                    return OnSocketSubExpression(data, dataSize);
                }
            case (int)(MDM_GF_FRAME.SUB_GF_GAME_STATUS): //100---100    //游戏状态
                {
                    return OnSocketSubGameStatus(data, dataSize);
                }
            case (int)(MDM_GF_FRAME.SUB_GF_GAME_SCENE): //100---101     //游戏场景
                {
                    return OnSocketSubGameScene(data, dataSize);
                }
            case (int)(MDM_GF_FRAME.SUB_GF_LOOKON_STATUS): //100---102   //旁观状态
                {
                    return OnSocketSubLookonStatus(data, dataSize);
                }
            case (int)(MDM_GF_FRAME.SUB_GF_SYSTEM_MESSAGE): //100---200  //系统消息
                {
                    OnSocketSubSystemMessage(data, dataSize);
                    break;
                }
            case (int)(MDM_GF_FRAME.SUB_GF_ACTION_MESSAGE): //100---201   //动作消息
                {
                    return OnSocketSubActionMessage(data, dataSize);
                }
            case (int)(MDM_GF_FRAME.SUB_GF_USER_READY): //100---2      //用户准备
            case (int)(MDM_GF_FRAME.SUB_GR_MATCH_INFO): //100---403       //比赛信息
            case (int)(MDM_GF_FRAME.SUB_GR_MATCH_WAIT_TIP): //100---404  //等待提示
            case (int)(MDM_GF_FRAME.SUB_GR_MATCH_RESULT): //100---405     //比赛结果
                {
                    //设置参数
                    break;
                }
        }

        return false;
    }

    //比赛消息
    private bool OnSocketMainMatch(int sub, byte[] data, int dataSize)
    {
        switch (sub)
        {
            //费用查询
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_FEE): //9---400  报名费用
                {
                    return OnSocketSubMatchFee(data, dataSize);
                }
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_NUM): //9---401  等待人数
                {
                    return OnSocketSubMatchNum(data, dataSize);
                }
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_INFO): //9---403   比赛信息
                {
                    return OnSocketSubMatchInfo(data, dataSize);
                }
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_WAIT_TIP): //9---404   等待提示
                {
                    return OnSocketSubWaitTip(data, dataSize);
                }
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_RESULT): //9---405   比赛结果
                {
                    return OnSocketSubMatchResult(data, dataSize);
                }
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_STATUS): //9---406   比赛状态
                {
                    return OnSocketSubMatchStatus(data, dataSize);
                }
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_GOLDUPDATE): //9---409   金币更新
                {
                    return OnSocketSubMatchGoldUpdate(data, dataSize);
                }
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_ELIMINATE): //9---410   比赛淘汰
                {
                    return OnSocketSubMatchEliminate(data, dataSize);
                }
            case (int)(MDM_GR_MATCH.SUB_GR_MATCH_JOIN_RESOULT): //9---411   加入结果
                {
                    return OnSocketSubMatchJoinResoult(data, dataSize);
                }
        }

        return true;
    }

    //四人场信息
    private bool OnSocketMainPrivate(int sub, byte[] data, int dataSize)
    {
        //switch (sub)
        //{
        //    //费用查询
        //    case (int)(MDM_GR_PRIVATE.SUB_GR_PRIVATE_INFO): //10---401      私人场信息
        //        {
        //            return OnSocketSubPrivateInfo(data, dataSize);
        //        }
        //    case (int)(MDM_GR_PRIVATE.SUB_GR_CREATE_PRIVATE_SUCESS): //10---403   创建四人场成功
        //        {
        //            return OnSocketSubPrivateCreateSuceess(data, dataSize);
        //        }
        //    case (int)(MDM_GR_PRIVATE.SUB_GF_PRIVATE_ROOM_INFO): //10---405   四人场房间信息
        //        {
        //            return OnSocketSubPrivateRoomInfo(data, dataSize);
        //        }
        //    case (int)(MDM_GR_PRIVATE.SUB_GF_PRIVATE_END): //10---407   四人场结算
        //        {
        //            return OnSocketSubPrivateEnd(data, dataSize);
        //        }
        //    case (int)(MDM_GR_PRIVATE.SUB_GR_PRIVATE_DISMISS): //10---406   四人场请求解散
        //        {
        //            return OnSocketSubPrivateDismissInfo(data, dataSize);
        //        }
        //    case (int)(MDM_GR_PRIVATE.SUB_GR_JOIN_PRIVATE): //10---404   加入四人场
        //        {
        //            MyDebug.Log("SUB_GR_JOIN_PRIVATE  To Game");
        //            MySceneManager.instance.SceneToMaJiang();
        //            break;
        //        }
        //}

        return true;
    }

    private void OnSubChuZengResult(byte[] tmpBuf, int size)
    {
        var pChuZengResult = NetUtil.BytesToStruct<CMD_S_ChuZengResult>(tmpBuf);
    }

    private void OnSubChuZeng(byte[] tmpBuf, int size)
    {
        var pChuZeng = NetUtil.BytesToStruct<CMD_S_ChuZeng>(tmpBuf);
    }

    private void OnSubTrustee(byte[] tmpBuf, int size)
    {
        var pTrustee = NetUtil.BytesToStruct<CMD_S_Trustee>(tmpBuf);
    }

    private void OnSubGameEnd(byte[] tmpBuf, int size)
    {
        MyDebug.Log("OnSubGameEnd Size==" + size);
        MyDebug.Log(NetUtil.BytesToString(tmpBuf));
        var pGameEnd = NetUtil.BytesToStruct<CMD_S_GameEnd>(tmpBuf);
        // MyDebug.Log("OnSubGameEnd---:" + pGameEnd.bAllGameEnd + "????????????????????????????????????????????");
        var gnv = new HupaiResponseVo();
        //    for (int i = 0; i < pGameEnd.cbCardCount.Length; i++)
        //    {
        //        var hupaiResponseItem = new HupaiResponseItem
        //        {
        //            chairId = i
        //        };
        //        if (i == pGameEnd.wChiHuUser)
        //        {
        //            hupaiResponseItem.win = true;
        //            var card = MaJiangHelper.MaJiangCardChange((MJ_PAI)pGameEnd.cb_hu_card);
        //            var huInfo = new HuInfo
        //            {
        //                card = card
        //            };
        //            hupaiResponseItem.huInfo = huInfo;
        //        }
        //        else
        //        {
        //            hupaiResponseItem.win = false;
        //        }

        //        hupaiResponseItem.roundTotalScore = (int)pGameEnd.lGameScore[i];
        //        for (int j = 0; j < pGameEnd.cbWeaveCount[i]; j++)
        //        {
        //            if (pGameEnd.WeaveItemArray[i].WeaveItem[j].cbWeaveKind == 16)
        //            {
        //                var gangInfo = new GangInfo
        //                {
        //                    cardIndex = MaJiangHelper.MaJiangCardChange((MJ_PAI)pGameEnd.WeaveItemArray[i].WeaveItem[j].cbCenterCard)
        //                };
        //                hupaiResponseItem.gangInfos.Add(gangInfo);
        //            }
        //            else if (pGameEnd.WeaveItemArray[i].WeaveItem[j].cbWeaveKind == 8)
        //            {
        //                var card = MaJiangHelper.MaJiangCardChange((MJ_PAI)pGameEnd.WeaveItemArray[i].WeaveItem[j].cbCenterCard);
        //                hupaiResponseItem.pengArray.Add(card);
        //            }
        //            else if (pGameEnd.WeaveItemArray[i].WeaveItem[j].cbWeaveKind == 2)
        //            {
        //                var card = MaJiangHelper.MaJiangCardChange((MJ_PAI)pGameEnd.WeaveItemArray[i].WeaveItem[j].cbCenterCard);
        //                hupaiResponseItem.centerChiArray.Add(card);
        //            }
        //            else if (pGameEnd.WeaveItemArray[i].WeaveItem[j].cbWeaveKind == 4)
        //            {
        //                var card = MaJiangHelper.MaJiangCardChange((MJ_PAI)pGameEnd.WeaveItemArray[i].WeaveItem[j].cbCenterCard);
        //                hupaiResponseItem.rightChiArray.Add(card);
        //            }
        //            else if (pGameEnd.WeaveItemArray[i].WeaveItem[j].cbWeaveKind == 1)
        //            {
        //                var card = MaJiangHelper.MaJiangCardChange((MJ_PAI)pGameEnd.WeaveItemArray[i].WeaveItem[j].cbCenterCard);
        //                hupaiResponseItem.leftChiArray.Add(card);
        //            }
        //        }

        //        for (int k = 0; k < pGameEnd.cbCardCount[i]; k++)
        //        {
        //            hupaiResponseItem.paiArray.Add(MaJiangHelper.MaJiangCardChange((MJ_PAI)pGameEnd.cbCardData[i].cbCardData[k]));
        //        }

        //        gnv.avatarList.Add(hupaiResponseItem);
        //    }

        //    gnv.endType = pGameEnd.dwChiHuRight;
        //    gnv.bAllGameEnd = pGameEnd.bAllGameEnd == 1 ? true : false;
        //    SetClientResponse(APIS.HUPAI_RESPONSE, NetUtil.ObjToJson(gnv));
        //}
    }
    private void OnSubOperateResult(byte[] tmpBuf, int size)
    {
        var pOperateResult = NetUtil.BytesToStruct<CMD_S_OperateResult>(tmpBuf);
        var wOperateUser = pOperateResult.wOperateUser;
        var cbOperateCard = pOperateResult.cbOperateCard;
        if (pOperateResult.cbOperateCode == (byte)WIK.WIK_PENG)
        {
            var opgbv = new OtherPengGangBackVO
            {
                chairId = pOperateResult.wOperateUser,
                provideUser = pOperateResult.wProvideUser,
                cardPoint = MaJiangHelper.MaJiangCardChange((MJ_PAI)pOperateResult.cbOperateCard)
            };
            SetClientResponse(APIS.PENGPAI_RESPONSE, NetUtil.ObjToJson(opgbv));
        }

        if (pOperateResult.cbOperateCode == (byte)WIK.WIK_LEFT ||
            pOperateResult.cbOperateCode == (byte)WIK.WIK_CENTER ||
            pOperateResult.cbOperateCode == (byte)WIK.WIK_RIGHT)
        {
            var ocbv = new OtherChiBackVO
            {
                chairId = pOperateResult.wOperateUser,
                type = pOperateResult.cbOperateCode,
                cardPoint = MaJiangHelper.MaJiangCardChange((MJ_PAI)pOperateResult.cbOperateCard)
            };
            SetClientResponse(APIS.CHIPAI_RESPONSE, NetUtil.ObjToJson(ocbv));
        }

        if (pOperateResult.cbOperateCode == (byte)WIK.WIK_GANG &&
            pOperateResult.wOperateUser == GlobalDataScript.loginResponseData.chairID)
        {
            var gbv = new GangBackVO
            {
                wProvideUser = pOperateResult.wProvideUser
            };
            if (pOperateResult.wOperateUser == pOperateResult.wProvideUser)
            {
                gbv.type = 1;
            }
            else
            {
                gbv.type = 0;
            }

            gbv.cardPoint = MaJiangHelper.MaJiangCardChange((MJ_PAI)pOperateResult.cbOperateCard);
            SetClientResponse(APIS.GANGPAI_RESPONSE, NetUtil.ObjToJson(gbv));
        }

        if (pOperateResult.cbOperateCode == (byte)WIK.WIK_GANG &&
            pOperateResult.wOperateUser != GlobalDataScript.loginResponseData.chairID)
        {
            var gnv = new GangNoticeVO();
            if (pOperateResult.wOperateUser == pOperateResult.wProvideUser)
            {
                gnv.type = 1;
            }
            else
            {
                gnv.type = 0;
            }

            gnv.cardPoint = MaJiangHelper.MaJiangCardChange((MJ_PAI)pOperateResult.cbOperateCard);
            gnv.chairId = pOperateResult.wOperateUser;
            SetClientResponse(APIS.OTHER_GANGPAI_NOICE, NetUtil.ObjToJson(gnv));
        }

        if (pOperateResult.cbOperateCode == (byte)WIK.WIK_CHI_HU)
        {
            var gnv = new HupaiResponseVo();
            MyDebug.Log("操作用户座位ID：" + pOperateResult.wOperateUser);
            MyDebug.Log("供应用户座位ID：" + pOperateResult.wProvideUser);
            SetClientResponse(APIS.HUPAI_RESPONSE, NetUtil.ObjToJson(gnv));
        }
    }

    private void OnSubOperateNotify(byte[] tmpBuf, int size)
    {
        var mm = new MahjongMotion();
        var pOperateNotify = NetUtil.BytesToStruct<CMD_S_OperateNotify>(tmpBuf);
        MyDebug.Log("还原用户：" + pOperateNotify.wResumeUser);
        if (pOperateNotify.cbActionMask != (int)WIK.WIK_NULL)
        {
            MyDebug.Log(MaJiangHelper.MaJiangCardChange((MJ_PAI)pOperateNotify.cbActionCard) +
                        "操作命令llllllllllllllllllllllllllllllllllllllllllllllllllll：" + pOperateNotify.cbActionMask);
            mm.cardID = MaJiangHelper.MaJiangCardChange((MJ_PAI)pOperateNotify.cbActionCard);
            if (((pOperateNotify.cbActionMask & (byte)WIK.WIK_CHI_HU)) > 0)
            {
                //胡
                mm.isChiHuMotion = true;
            }

            if ((pOperateNotify.cbActionMask & (byte)WIK.WIK_GANG) > 0)
            {
                mm.isGangMotion = true;
                //杠
            }

            if ((pOperateNotify.cbActionMask & (byte)WIK.WIK_PENG) > 0)
            {
                mm.isPengMotion = true;
                //碰
            }

            if (((pOperateNotify.cbActionMask & (byte)WIK.WIK_LEFT)) > 0)
            {
                //吃
                MyDebug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1Chi Type:WIK_LEFT");
                mm.isLeftMotion = true;
                mm.chiCount++;
                mm.type = pOperateNotify.cbActionMask;
            }

            if (((pOperateNotify.cbActionMask & (byte)WIK.WIK_CENTER)) > 0)
            {
                MyDebug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1Chi Type:WIK_CENTER");
                mm.isCenterMotion = true;
                mm.chiCount++;
                mm.type = pOperateNotify.cbActionMask;
            }

            if (((pOperateNotify.cbActionMask & (byte)WIK.WIK_RIGHT)) > 0)
            {
                MyDebug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1Chi Type:WIK_RIGHT");
                mm.isRightMotion = true;
                mm.chiCount++;
                mm.type = pOperateNotify.cbActionMask;
            }
        }

        SetClientResponse(APIS.RETURN_INFO_RESPONSE, NetUtil.ObjToJson(mm));
    }

    private void OnSubSendCard(byte[] tmpBuf, int size)
    {
        MyDebug.Log("OnSubSendCard");
        var pSpendCard = NetUtil.BytesToStruct<CMD_S_SendCard>(tmpBuf); //发送扑克？
        if (pSpendCard.wCurrentUser == GlobalDataScript.loginResponseData.chairID) //自己摸牌
        {
            var cvo = new CardVO
            {
                cardPoint = MaJiangHelper.MaJiangCardChange((MJ_PAI)pSpendCard.cbCardData)
            };
            SetClientResponse(APIS.PICKCARD_RESPONSE, NetUtil.ObjToJson(cvo));
            var mahjongMotion = new MahjongMotion();
            MyDebug.Log(pSpendCard.cbActionMask);
            MyDebug.Log((byte)WIK.WIK_GANG);
            MyDebug.Log((byte)WIK.WIK_CHI_HU);
            if (pSpendCard.cbActionMask != (int)WIK.WIK_NULL)
            {
                mahjongMotion.cardID = cvo.cardPoint;
                if ((pSpendCard.cbActionMask & (byte)WIK.WIK_GANG) > 0)
                {
                    mahjongMotion.isGangMotion = true;

                }

                if ((pSpendCard.cbActionMask & (Byte)WIK.WIK_CHI_HU) > 0)
                {
                    mahjongMotion.isChiHuMotion = true;
                }

                SetClientResponse(APIS.RETURN_INFO_RESPONSE, NetUtil.ObjToJson(mahjongMotion));
            }
        }
        else //他人摸牌
        {
            var cvo = new CardVO
            {
                avatarIndex = pSpendCard.wCurrentUser
            };
            MyDebug.Log("当前摸牌用户用户：" + cvo.avatarIndex);
            SetClientResponse(APIS.OTHER_PICKCARD_RESPONSE_NOTICE, NetUtil.ObjToJson(cvo));
        }
    }

    private void OnSubOutCard(byte[] tmpBuf, int size)
    {
        var pOutCard = NetUtil.BytesToStruct<CMD_S_OutCard>(tmpBuf);
        var ocv = new OutCardVO
        {
            cardID = MaJiangHelper.MaJiangCardChange((MJ_PAI)pOutCard.cbOutCardData),
            chairId = pOutCard.wOutCardUser
        };
        SetClientResponse(APIS.CHUPAI_RESPONSE, NetUtil.ObjToJson(ocv));
    }

    private void OnSubGameStart(byte[] tmpBuf, int size)
    {
        var pGameStart = NetUtil.BytesToStruct<CMD_S_GameStart>(tmpBuf);
        var list = new int[34];
        var sgv = new StartGameVO
        {
            //bankerId = pGameStart.wCurrentUser
        };
        int cardID;
        for (int i = 0; i < 14; i++)
        {
           // cardID = MaJiangHelper.MaJiangCardChange((MJ_PAI)pGameStart.cbCardData[i]);
            //if (cardID < 0 || cardID > 34)
            //    continue;
            //list[cardID]++;
        }

        sgv.paiArray = new List<int>(list);
        SetClientResponse(APIS.STARTGAME_RESPONSE_NOTICE, NetUtil.ObjToJson(sgv));
    }

    private void SetClientResponse(int code, string message)
    {
        MyDebug.Log("SetClientResponse:" + code);
        var cr = new ClientResponse
        {
            headCode = code,
            message = message
        };
        SocketEventHandle.Instance.AddResponse(cr);
    }

    //解散房间
    private bool OnSocketSubPrivateDismissInfo(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GF_Private_Dismiss_Info))) return false;
        var pNetInfo = NetUtil.BytesToStruct<CMD_GF_Private_Dismiss_Info>(data);
        var orrv = new OutRoomResponseVo
        {
            dwDissUserCout = (int)pNetInfo.dwDissUserCout,
            dwNotAgreeUserCout = (int)pNetInfo.dwNotAgreeUserCout,
            dwDissChairID = new int[pNetInfo.dwDissUserCout],
            dwNotAgreeChairID = new int[pNetInfo.dwNotAgreeUserCout]
        };
        for (int i = 0; i < pNetInfo.dwDissUserCout; i++)
        {
            orrv.dwDissChairID[i] = (int)pNetInfo.dwDissChairID[i];
        }

        for (int i = 0; i < pNetInfo.dwNotAgreeUserCout; i++)
        {
            orrv.dwNotAgreeChairID[i] = (int)pNetInfo.dwNotAgreeChairID[i];
        }

        MyDebug.Log("OnSocketSubPrivateDismissInfo：解散房间");
        SetClientResponse(APIS.DISSOLIVE_ROOM_RESPONSE, NetUtil.ObjToJson(orrv));
        return true;
    }

    private bool OnSocketSubPrivateEnd(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubPrivateRoomInfo(byte[] data, int dataSize)
    {
        var info = NetUtil.BytesToStruct<CMD_GF_Private_Room_Info>(data);
        GlobalDataScript.roomVo.roomId = (int)info.dwRoomNum;
        GlobalDataScript.roomVo.dwPlayTotal = info.dwPlayTotal;
        GlobalDataScript.roomVo.dwPlayCout = info.dwPlayCout;
        MyDebug.Log("OnSocketSubPrivateRoomInfo  To Game roomID：" + info.dwRoomNum);
        MySceneManager.instance.SceneToMaJiang();
        return true; //TODO: 跳转场景
    }

    private bool OnSocketSubPrivateCreateSuceess(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GF_Create_Private_Sucess))) return false;
        var pNetInfo = NetUtil.BytesToStruct<CMD_GF_Create_Private_Sucess>(data);
        MyDebug.Log("房间号：" + pNetInfo.dwRoomNum);
        GlobalDataScript.roomVo.roomId = (int)pNetInfo.dwRoomNum;
        MyDebug.Log("OnSocketSubPrivateCreateSuceess  To Game");
        MySceneManager.instance.SceneToMaJiang();
        return true;
    }

    private bool OnSocketSubPrivateInfo(byte[] data, int dataSize)
    {
        var pNetInfo = NetUtil.BytesToStruct<CMD_GR_Private_Info>(data);
        SocketSendManager.Instance.OnSocketSubPrivateInfo();
        SocketSendManager.Instance.GetGameOption();
        return true;
    }

    private bool OnSocketSubMatchFee(byte[] data, int dataSize)
    {
        var pNetInfo = NetUtil.BytesToStruct<CMD_GR_Match_Fee>(data);
        return true;
    }

    private bool OnSocketSubMatchNum(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GR_Match_Num))) return false;
        var pNetInfo = NetUtil.BytesToStruct<CMD_GR_Match_Num>(data);
        return true;
    }

    private bool OnSocketSubMatchInfo(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GR_Match_Info))) return false;
        var pNetInfo = NetUtil.BytesToStruct<CMD_GR_Match_Info>(data);
        return true;
    }

    private bool OnSocketSubWaitTip(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GR_Match_Wait_Tip)) && dataSize != 0) return false;
        return true;
    }

    private bool OnSocketSubMatchResult(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GR_MatchResult))) return false;
        var pNetInfo = NetUtil.BytesToStruct<CMD_GR_MatchResult>(data);
        return true;
    }

    private bool OnSocketSubMatchStatus(byte[] data, int dataSize)
    {
        if (dataSize != sizeof(byte)) return false;
        return true;
    }

    private bool OnSocketSubMatchGoldUpdate(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GR_MatchGoldUpdate))) return false;
        var pNetInfo = NetUtil.BytesToStruct<CMD_GR_MatchGoldUpdate>(data);
        return true;
    }

    private bool OnSocketSubMatchEliminate(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubMatchJoinResoult(byte[] data, int dataSize)
    {
        MyDebug.Log("OnSocketSubMatchJoinResoult");
        if (dataSize != Marshal.SizeOf(typeof(CMD_GR_Match_JoinResoult))) return false;
        var pNetInfo = NetUtil.BytesToStruct<CMD_GR_Match_JoinResoult>(data);
        return true;
    }

    private bool OnSocketSubActionMessage(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubLookonStatus(byte[] data, int dataSize)
    {
        return true; //TODO:
    }
    //断线重连
    private bool OnSocketSubGameScene(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_S_StatusPlay))) return false;
        CMD_S_StatusPlay gsmeStatus = NetUtil.BytesToStruct<CMD_S_StatusPlay>(data);
        return true; //TODO:
    }

    private bool OnSocketSubGameStatus(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GF_GameStatus))) return false;
        var pGameStatus = NetUtil.BytesToStruct<CMD_GF_GameStatus>(data);
        return true;
    }

    private bool OnSocketSubUserTalk(byte[] data, int dataSize)
    {
        return true;
    }

    private bool OnSocketMainSystem(int sub, byte[] data, int dataSize)
    {
        OnSocketSubSystemMessage(data, dataSize);
        return true;
    }

    private void OnSocketSubSystemMessage(byte[] data, int dataSize)
    {
        var pSystemMessage = NetUtil.BytesToStruct<CMD_CM_SystemMessage>(data);
        var wType = pSystemMessage.wType;
        SocketEventHandle.Instance.iscloseLoading = true;
        SocketEventHandle.Instance.SetTips(NetUtil.GetServerLog(pSystemMessage.szString));
        //关闭处理
        if ((wType & (Define.SMT_CLOSE_ROOM | Define.SMT_CLOSE_LINK)) != 0)
        {
            MyDebug.Log("关闭处理");
        }

        //显示消息
        if ((wType & Define.SMT_CHAT) == 1)
        {
            MyDebug.Log("显示消息");
            //SocketEventHandle.Instance.iscloseLoading = true;
            //if (mIStringMessageSink)
            //{
            //    mIStringMessageSink->InsertSystemString(pSystemMessage->szString);
            //}
        }
        //弹出消息
        else if ((wType & Define.SMT_EJECT) == 1)
        {
            MyDebug.Log("弹出消息");
            //if (mIStringMessageSink)
            //{
            //    mIStringMessageSink->InsertPromptString(pSystemMessage->szString, 0);
            //}
        }

        //关闭游戏
        if ((wType & Define.SMT_CLOSE_GAME) == 1)
        {
            MyDebug.Log("关闭游戏s");
            //if (mIStringMessageSink)
            //{
            //    mIStringMessageSink->InsertSystemString(pSystemMessage->szString);
            //}
            //OnGFGameClose(0);
        }



        //关闭房间
        if ((wType & Define.SMT_CLOSE_ROOM) == 1)
        {
            MyDebug.Log("关闭房间");
            //if (mIStringMessageSink)
            //{
            //    mIStringMessageSink->InsertSystemString(pSystemMessage->szString);
            //}
            //OnGFGameClose(0);
        }
    }

    private bool OnSocketSubStatusTableStatus(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubStatusTableInfo(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubGladMessage(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubPropertyTrumpet(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubPropertyMessage(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubPropertyEffect(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubPropertyFailure(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubPropertySuccess(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubWisperExpression(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubWisperUserChat(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubExpression(byte[] data, int dataSize)
    {
        CMD_GR_S_UserExpression sUserFace = NetUtil.BytesToStruct<CMD_GR_S_UserExpression>(data);
        return true;
    }

    private bool OnSocketSubUserChat(byte[] data, int dataSize)
    {
        CMD_GF_S_UserChat sUserChat = NetUtil.BytesToStruct<CMD_GF_S_UserChat>(data);
        return true;
    }

    //状态
    private void OnSocketSubUserStatus(byte[] data, int dataSize)
    {
        if (dataSize < Marshal.SizeOf(typeof(CMD_GR_UserStatus))) return;
        CMD_GR_UserStatus pUserStatus = NetUtil.BytesToStruct<CMD_GR_UserStatus>(data);
        MyDebug.Log("=========" + pUserStatus.dwUserID + "==" + pUserStatus.UserStatus.wChairID + "==" +
                    pUserStatus.UserStatus.wTableID);
        PlayerStateVO state = new PlayerStateVO
        {
            userId = (int)pUserStatus.dwUserID,
            chairState = pUserStatus.UserStatus.cbUserStatus
        };
        MyDebug.Log("UserStatus.cbUserStatus:" + pUserStatus.UserStatus.cbUserStatus);
        if (pUserStatus.UserStatus.cbUserStatus == Define.US_NULL)
        {
            //删除用户
        }
        else if (pUserStatus.UserStatus.cbUserStatus == Define.US_FREE)
        {
            //退出房间
            SetClientResponse(APIS.OUT_ROOM_RESPONSE, NetUtil.ObjToJson(state));
        }
    }

    private bool OnSocketSubUserScore(byte[] data, int dataSize)
    {
        if (dataSize < Marshal.SizeOf(typeof(CMD_GR_UserScore)))
        {
            return false;
        }

        CMD_GR_UserScore pUserScore = NetUtil.BytesToStruct<CMD_GR_UserScore>(data);
        return true;
    }

    private void OnSocketSubUserEnter(byte[] data, int dataSize)
    {
        int len = 0;
        TagUserInfoHead pUserInfoHead = NetUtil.BytesToStruct<TagUserInfoHead>(data);
        MyDebug.Log("座位号：" + pUserInfoHead.wChairID + "---------------------" + pUserInfoHead.dwUserID);
        if (pUserInfoHead.wChairID > 3)
        {
            return;
        }

        if (GlobalDataScript.roomAvatarVoList == null)
            GlobalDataScript.roomAvatarVoList = new List<AvatarVO>();
        for (int i = 0; i < GlobalDataScript.roomAvatarVoList.Count; i++)
        {
            if (GlobalDataScript.roomAvatarVoList[i].chairID == pUserInfoHead.wChairID)
            {
                GlobalDataScript.roomAvatarVoList[i].account.uuid = (int)pUserInfoHead.dwUserID;
                GlobalDataScript.roomAvatarVoList[i].account.headicon = GetHeadHttp(pUserInfoHead.szHeadHttp);
                GlobalDataScript.roomAvatarVoList[i].account.sex = pUserInfoHead.cbGender;
                GlobalDataScript.roomAvatarVoList[i].chairID = pUserInfoHead.wChairID;
                GlobalDataScript.roomAvatarVoList[i].tableID = pUserInfoHead.wTableID;
                return;
            }
        }

        AvatarVO avo = new AvatarVO();
        Account acc = new Account();
        avo.account = acc;
        avo.account.uuid = (int)pUserInfoHead.dwUserID;
        avo.account.headicon = GetHeadHttp(pUserInfoHead.szHeadHttp);
        avo.account.sex = pUserInfoHead.cbGender;
        avo.chairID = pUserInfoHead.wChairID;
        avo.tableID = pUserInfoHead.wTableID;
        if (pUserInfoHead.dwUserID == GlobalDataScript.loginResponseData.account.uuid)
        {
            GlobalDataScript.loginResponseData.account.uuid = avo.account.uuid;
            GlobalDataScript.loginResponseData.account.headicon = avo.account.headicon;
            GlobalDataScript.loginResponseData.account.sex = avo.account.sex;
            GlobalDataScript.loginResponseData.chairID = avo.chairID;
            GlobalDataScript.loginResponseData.tableID = avo.tableID;
        }

        GlobalDataScript.roomAvatarVoList.Add(avo);
        if (GlobalDataScript.type != ModeType.None)
            return;
        SetClientResponse(APIS.JOIN_ROOM_NOICE, NetUtil.ObjToJson(avo));
    }

    private bool OnSocketSubRequestFailure(byte[] data, int dataSize)
    {
        CMD_GR_RequestFailure pRequestFailure = NetUtil.BytesToStruct<CMD_GR_RequestFailure>(data);
        if (dataSize <= Marshal.SizeOf(typeof(CMD_GR_RequestFailure)) - pRequestFailure.szDescribeString.Length
        ) return false;                                     
        SocketEngine.Instance.SocketQuit();
        return true;
    }

    private bool OnSocketSubConfigFinish(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubConfigMmber(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubConfigOrder(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubConfigServer(byte[] data, int dataSize)
    {
        if (dataSize < Marshal.SizeOf(typeof(CMD_GR_ConfigServer))) return false;
        CMD_GR_ConfigServer pConfigServer = NetUtil.BytesToStruct<CMD_GR_ConfigServer>(data);
        return true;
    }

    private bool OnSocketSubConfigColumn(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    public void OnEventTCPSocketShut()
    {
        //TODO:
    }

    private bool OnSocketSubReplayData(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubReplayList(byte[] data, int dataSize)
    {
        CMD_S_ResponseReplayList pNetInfo = NetUtil.BytesToStruct<CMD_S_ResponseReplayList>(data);
        return true;
    }

    private bool OnSocketSubUpdateNotify(byte[] data, int dataSize)
    {
        return true; //TODO:
    }

    private bool OnSocketSubLogonFinish(byte[] data, int dataSize)
    {
        byte[] szPassword = new byte[33];
        CMD_GR_UserSitDown UserSitReq = new CMD_GR_UserSitDown
        {
            wTableID = (ushort)Define.INVALID_TABLE,
            wChairID = (ushort)Define.INVALID_CHAIR,
            szPassword = szPassword
        };
        byte[] temp = NetUtil.StructToBytes(UserSitReq);
        //发送数据包
        SocketSendManager.Instance.SendData((int)GameServer.MDM_GR_USER, (int)MDM_GR_USER.SUB_GR_USER_SITDOWN, temp,
            Marshal.SizeOf(UserSitReq));
        return true; //TODO:
    }

    private bool OnSocketSubLogonFailure(byte[] data, int dataSize)
    {
        CMD_GR_LogonFailure pGameServer = NetUtil.BytesToStruct<CMD_GR_LogonFailure>(data);
        MyDebug.SocketLog("IErrorCode:" + pGameServer.lErrorCode + "--" + NetUtil.GetServerLog(pGameServer.szDescribeString));   
        SocketEventHandle.Instance.SetTips("IErrorCode:" + pGameServer.lErrorCode + "--" +
                                           NetUtil.GetServerLog(pGameServer.szDescribeString));
        SocketEngine.Instance.SocketQuit();
        return true;
    }

    private bool OnSocketSubLogonSuccess(byte[] data, int dataSize)
    {
        if (dataSize != Marshal.SizeOf(typeof(CMD_GR_LogonSuccess))) return false;
        //消息处理
        CMD_GR_LogonSuccess pLogonSuccess = NetUtil.BytesToStruct<CMD_GR_LogonSuccess>(data);
        return true;
    }

    int len;

    private string GetHeadHttp(byte[] bytes)
    {
        len = 0;
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] > 0)
                len++;
        }

        MyDebug.Log("########################Len:" + len);
        byte[] blist = new byte[len];
        Array.Copy(bytes, blist, len);
        return NetUtil.BytesToString(blist);
    }

    public void ISocketEngineSink(int kind_Id)
    {
        throw new NotImplementedException();
    }
}