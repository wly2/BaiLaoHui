using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using LitJson;

namespace AssemblyCSharp
{
    /// <summary>
    /// 消息分发类
    /// </summary>
    public class SocketEventHandle : MonoBehaviour
    {
        private static SocketEventHandle _instance;

        public delegate void ServerCallBackEvent(ClientResponse response);

        public delegate void ServerDisconnectCallBackEvent();

        public ServerCallBackEvent loginReply; //登录回调
        public ServerCallBackEvent createRoomReply; //创建房间回调
        public ServerCallBackEvent readyReply; //准备游戏通知
        public ServerCallBackEvent joinRoomReply; //加入房间回调
        public ServerCallBackEvent otherJoinRoomReply; //其他玩家加入通知
        public ServerCallBackEvent startGameReply; //开始游戏通知
        public ServerCallBackEvent pickCardReply; //自己摸牌通知
        public ServerCallBackEvent otherPickCardReply; //别人摸牌通知
        public ServerCallBackEvent otherPutOutCardReply; //出牌通知
        public ServerCallBackEvent backBanker; //返回叫庄
        public ServerCallBackEvent backXiaZhu; //下注回调
        public ServerCallBackEvent MPsendCard; //明牌发牌回调
        public ServerCallBackEvent otherPentReply; //碰牌回调
        public ServerCallBackEvent otherChiReply; //吃牌回调
        public ServerCallBackEvent gangReply; //杠牌回调
        public ServerCallBackEvent huReply; //胡牌回调
        public ServerCallBackEvent gameOverReply; //结束回调
        public ServerCallBackEvent ALLgameOverReply; //全局结束回调
        public ServerCallBackEvent otherGangReply; //其他人杠
        public ServerCallBackEvent actionBtnReply; //胡碰杠吃过行为通知
        public ServerCallBackEvent quitRoomReply; //退出房间回调
        public ServerCallBackEvent dissloveRoomReq;//
        public ServerCallBackEvent dissolveRoomReply; //申请解散队伍
        public ServerCallBackEvent userShowCard; //用户摊牌回调
        public ServerCallBackEvent micInputReply; //语音聊天通知
        public ServerCallBackEvent messageBoxReply; //系统常用聊天语句
        public ServerCallBackEvent serviceErrorReply; //错误信息返回
        public ServerCallBackEvent reLoginReply; //玩家断线重连
        public ServerCallBackEvent backRoomReply; //掉线后返回房间
        public ServerCallBackEvent cardChangeReply; //房卡数据变化
        public ServerCallBackEvent offlineReply; //离线通知

        public ServerCallBackEvent setRoomMark;
        public ServerCallBackEvent onlineReply; //上线通知

        //public ServerCallBackEvent rewardRequestCallBack;//投资请求返回
        public ServerCallBackEvent giftReply; //奖品回调
        public ServerCallBackEvent returnGameReply;
        public ServerCallBackEvent followBankerReply; //跟庄
        public ServerCallBackEvent broadcastNoticeReply; //游戏公告
        public ServerDisconnectCallBackEvent disConnetReply; //断线
        public ServerCallBackEvent contactInfoReply; //联系方式回调
        public ServerCallBackEvent lotteryReply; //抽奖信息变化
        public ServerCallBackEvent recordReply; //房间战绩返回数据
        public ServerCallBackEvent recordDetailReply; //房间战绩返回数据
        public ServerCallBackEvent gameBattleReply; //回放返回数据
        public ServerCallBackEvent otherDeviceLoginReply; //其他设备登陆账户
        public ServerCallBackEvent playerStateReply; //登录回调
        public ServerCallBackEvent roomOptionReply; //登录回调
        public bool iscloseLoading;

        public ServerDisconnectCallBackEvent closeSceneLoading; //断线

        //private List<ClientResponse> callBackResponseList;
        private List<ClientResponse> callBackResponseList;
        private bool isDisconnet;
        float timeCount;

        public SocketEventHandle()
        {
            callBackResponseList = new List<ClientResponse>();
        }

        void Start()
        {
            DontDestroyOnLoad(this);
        }

        public static SocketEventHandle Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject temp = new GameObject
                    {
                        name = "SocketEventHandle"
                    };
                    _instance = temp.AddComponent<SocketEventHandle>();
                }

                return _instance;
            }
        }

        void FixedUpdate()
        {
          //  Debug.Log(SocketEngine.Instance.isTcpConnect);
            while (callBackResponseList.Count > 0)
            {
                MyDebug.Log("callBackResponseList.Count:" + callBackResponseList.Count);
                ClientResponse response = callBackResponseList[0];
                callBackResponseList.RemoveAt(0);
                DispatchHandle(response);
            }

            if (isDisconnet)
            {
                isDisconnet = false;
                var handler = disConnetReply;
                if (handler != null)
                    handler();
            }

            if (GameMessageManager.CloseLoading != null && iscloseLoading)
            {
                GameMessageManager.CloseLoading();
                iscloseLoading = false;
            }
            else if (iscloseLoading)
                iscloseLoading = false;

            if (ShowTips)
            {
                ShowTips = false;
                ShowTipDialog();
            }

            //MySceneManager.instance.SceneState();
            timeCount += Time.deltaTime;
            if (timeCount < 5f)
            {
                return;
            }

            if (SocketEngine.Instance.isConnected)
                SocketSendManager.Instance.SendHeadData();
            timeCount = 0;
            if (SocketInGameEvent.Instance.socketip != null)
                StartCoroutine(SendPing());
        }

        private void DispatchHandle(ClientResponse response)
        {
            MyDebug.Log(response.headCode + "===========================");
            switch (response.headCode)
            {
                case APIS.CLOSE_RESPONSE:
                    TipsManagerScript.getInstance.setTips(LocalizationManager.GetInstance.GetValue("KEY.20013"));
                    CustomSocket.Instance.CloseSocket();
                    break;
                case APIS.LOGIN_RESPONSE:
                    if (loginReply != null)
                    {
                        loginReply(response);
                    }

                    break;
                case APIS.CREATEROOM_RESPONSE:
                    if (createRoomReply != null)
                    {
                        createRoomReply(response);
                    }

                    break;
                case APIS.JOIN_ROOM_RESPONSE:
                    if (joinRoomReply != null)
                    {
                        joinRoomReply(response);
                    }

                    break;
                case APIS.STARTGAME_RESPONSE_NOTICE:
                    if (startGameReply != null)
                    {
                        startGameReply(response);
                    }
                    //startResply = response;
                    //Invoke("StartReplyInvoke", 0.1f);
                    break;
                case APIS.BACK_BANKER:
                    if (backBanker != null)
                    {
                        backBanker(response);
                    }
                    break;

                case APIS.BACK_XIAZHU:
                    if (backXiaZhu != null)
                    {
                        backXiaZhu(response);
                    }
                    break;

                case APIS.MPPICKCARD_RESPONSE:
                    if (MPsendCard != null)
                    {
                        MPsendCard(response);
                    }
                    break;


                case APIS.PICKCARD_RESPONSE:
                    if (pickCardReply != null)
                    {
                        pickCardReply(response);
                    }
                    //pickCardResply = response;
                    //StartCoroutine(PickReplyInvoke());
                    break;
                case APIS.OTHER_PICKCARD_RESPONSE_NOTICE:
                    if (otherPickCardReply != null)
                    {
                        otherPickCardReply(response);
                    }

                    break;
                case APIS.CHUPAI_RESPONSE:
                    if (otherPutOutCardReply != null)
                    {
                        otherPutOutCardReply(response);
                    }

                    break;
                case APIS.JOIN_ROOM_NOICE:
                    if (otherJoinRoomReply != null)
                    {
                        otherJoinRoomReply(response);
                    }

                    break;
                case APIS.CHIPAI_RESPONSE:
                    if (otherChiReply != null)
                    {
                        otherChiReply(response);
                    }

                    break;
                case APIS.PENGPAI_RESPONSE:
                    if (otherPentReply != null)
                    {
                        otherPentReply(response);
                    }
                    break;
                case APIS.GANGPAI_RESPONSE:
                    if (gangReply != null)
                    {
                        gangReply(response);
                    }

                    break;
                case APIS.OTHER_GANGPAI_NOICE:
                    if (otherGangReply != null)
                    {
                        otherGangReply(response);
                    }

                    break;
                case APIS.RETURN_INFO_RESPONSE:
                    actionBtnReply(response);
                    //actioonResply = response;
                    //StartCoroutine(ActionInvoke());
                    break;
                case APIS.HUPAI_RESPONSE:
                    if (huReply != null)
                    {
                        huReply(response);
                    }

                    break;
                case APIS.HUPAIALL_RESPONSE:
                    if (gameOverReply != null)
                    {
                        gameOverReply(response);
                    }
                    break;
                case APIS.GAMEOVERALL_RESPONSE:
                    if (ALLgameOverReply != null)
                    {
                        ALLgameOverReply(response);
                    }
                    break;
                case APIS.OUT_ROOM_RESPONSE:
                    if (quitRoomReply != null)
                    {
                        quitRoomReply(response);
                    }

                    break;
                case APIS.headRESPONSE:
                    break;
                case APIS.USER_SHOW_CARD:
                    if (userShowCard != null)
                    {
                        userShowCard(response);
                    }

                    break;

                case APIS.DISSOLIVE_ROOM_RESPONSE:
                    if (response.message == null)
                        ShowExit();
                    if (dissolveRoomReply != null)
                    {
                        dissolveRoomReply(response);
                    }

                    break;
                case APIS.DISSOLIVE_ROOM_REQUEST:
                    if (dissloveRoomReq != null)
                        dissloveRoomReq(response);
                    break;
                case APIS.PrepareGame_MSG_RESPONSE:
                    if (readyReply != null)
                    {
                        readyReply(response);
                    }

                    break;
                case APIS.MicInput_Response:
                    if (micInputReply != null)
                    {
                        micInputReply(response);
                    }

                    break;
                case APIS.MessageBox_Notice:
                    if (messageBoxReply != null)
                    {
                        messageBoxReply(response);
                    }

                    break;
                case APIS.ERROR_RESPONSE:
                    if (serviceErrorReply != null)
                    {
                        serviceErrorReply(response);
                    }

                    break;
                case APIS.BACK_LOGIN_RESPONSE:
                    backResply = response;
                    BackReplyInvoke();

                    break;
                case APIS.CARD_CHANGE:
                    if (cardChangeReply != null)
                    {
                        cardChangeReply(response);
                    }

                    break;
                case APIS.OFFLINE_NOTICE:
                    if (offlineReply != null)
                    {
                        offlineReply(response);
                    }

                    break;
                case APIS.RETURN_ONLINE_RESPONSE:
                    if (returnGameReply != null)
                    {
                        returnGameReply(response);
                    }

                    break;
                case APIS.PRIZE_RESPONSE:
                    if (giftReply != null)
                    {
                        giftReply(response);
                    }

                    break;
                case APIS.Game_FollowBander_Notice:
                    if (followBankerReply != null)
                    {
                        followBankerReply(response);
                    }

                    break;
                case APIS.ONLINE_NOTICE:
                    if (onlineReply != null)
                    {
                        onlineReply(response);
                    }

                    break;
                case APIS.GAME_BROADCAST:
                    if (broadcastNoticeReply != null)
                    {
                        broadcastNoticeReply(response);
                    }

                    break;
                case APIS.CONTACT_INFO_RESPONSE:
                    if (contactInfoReply != null)
                    {
                        contactInfoReply(response);
                    }

                    break;
                case APIS.HOST_UPDATEDRAW_RESPONSE:
                    if (lotteryReply != null)
                    {
                        lotteryReply(response);
                    }

                    break;
                case APIS.ZHANJI_REPORTER_REPONSE:
                    if (recordReply != null)
                    {
                        recordReply(response);
                    }

                    break;
                case APIS.ZHANJI_DETAIL_REPORTER_REPONSE:
                    if (recordDetailReply != null)
                    {
                        recordDetailReply(response);
                    }

                    break;
                case APIS.GAME_BACK_PLAY_RESPONSE:
                    if (gameBattleReply != null)
                    {
                        gameBattleReply(response);
                    }

                    break;
                case APIS.TIP_MESSAGE:
                    TipsManagerScript.getInstance.setTips(response.message);
                    break;
                case APIS.OTHER_TELE_LOGIN:
                    if (otherDeviceLoginReply != null)
                    {
                        otherDeviceLoginReply(response);
                    }

                    break;
                case APIS.REFRESH_PLAYER_STATE:
                    if (playerStateReply != null)
                    {
                        playerStateReply(response);
                    }

                    break;
                case APIS.ROOM_OPTION:
                    if (roomOptionReply != null)
                    {
                        roomOptionReply(response);
                    }
                    break;
                case APIS.SetRoomMark:
                    if (setRoomMark != null)
                        setRoomMark(response);
                    break;
            }
        }

        ClientResponse startResply;

        void StartReplyInvoke()
        {
            if (startGameReply != null)
            {
                startGameReply(startResply);
            }
            else
            {
                MyDebug.Log("StartReplyInvoke  To Game");
                MySceneManager.instance.SceneToMaJiang();
                Invoke("StartReplyInvoke", 0.1f);
            }
        }
        ClientResponse backResply;
        void BackReplyInvoke()
        {
            if (backRoomReply != null)
            {
                backRoomReply(backResply);
            }
            else
            {
                
                Invoke("BackReplyInvoke", 0.1f);
            }
        }

        ClientResponse pickCardResply;

        IEnumerator PickReplyInvoke()
        {
            if (pickCardReply != null)
            {
                pickCardReply(pickCardResply);
            }
            else
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(PickReplyInvoke());
            }
        }

        ClientResponse actioonResply;

        IEnumerator ActionInvoke()
        {
            if (actionBtnReply != null && GlobalDataScript.isGameReadly)
            {
                if (!GlobalDataScript.isBeginGame)
                    yield return new WaitForSeconds(2);
                var handler = actionBtnReply;
                if (handler != null)
                    handler(actioonResply);
            }
            else
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(ActionInvoke());
            }
        }

        public void AddResponse(ClientResponse response)
        {
            MyDebug.Log(response.headCode);
            callBackResponseList.Add(response);
            MyDebug.Log("----" + response.headCode);
        }

        public void NoticeDisConect()
        {
            isDisconnet = true;
        }

        Ping ping;

        IEnumerator SendPing()
        {
            if (SocketInGameEvent.Instance.socketip == null)
                StopCoroutine(SendPing());
            ping = new Ping(SocketInGameEvent.Instance.socketip);
            while (!ping.isDone)
                yield return null;
            yield return new WaitForSeconds(2);
            SocketInGameEvent.Instance.netMs = ping.time;
            StartCoroutine(SendPing());
        }

        private string tipMes;
        private bool ShowTips;

        public void SetTips(string mes)
        {
            ShowTips = true;
            tipMes = mes;
        }

        private void ShowTipDialog()
        {

            UIManager.instance.Show(UIType.UITipsDialog,
                (go) => { go.GetComponent<UIPanel_TipsDialog>().SetMes(tipMes); });
        }
        public void SetClientResponse(int code, string mes)
        {
            MyDebug.Log("SetClientResponse:" + code);
            var cr = new ClientResponse
            {
                headCode = code,
                message = mes
            };
            AddResponse(cr);
        }
        public void ShowReEnterTipDialog(string mes)
        {
            GlobalDataScript.Instance.ClearGameInfo();
            UIManager.instance.Show(UIType.UITipsDialog,
               (go) => { go.GetComponent<UIPanel_TipsDialog>().SetMes(mes, ReEnterRoom); });

        }
        public void ReEnterRoom()
        {

            MySceneManager.instance.SceneToLogIn();
            UIManager.instance.Show(UIType.UILoading);
            HttpManager.instance.GetUserReconnection(SetGameStatue);
        }
        public void SetGameStatue(WWW mes)
        {
            if(!string.IsNullOrEmpty(mes.error))
            {
                SocketEventHandle.Instance.iscloseLoading = true;
                ShowReEnterTipDialog("网络连接失败，检查网络后重试！");
                return;
            }
            JsonData json = JsonMapper.ToObject<JsonData>(mes.text);
            if (json["code"].Equals("0"))
            {
                SocketEngine.Instance.SocketQuit();
                string jm = json["data"].ToJson();
                MyDebug.Log(jm);
                JsonData jd = JsonMapper.ToObject<JsonData>(jm);
                int kindId = int.Parse(jd["kindid"].ToString());
                int serverId = int.Parse(jd["serverid"].ToString());
                if (kindId == GlobalDataScript.NN_KIND_ID)
                {
                    SocketNiuNiuEvent.instance.Init();
                    SocketNiuNiuEvent.instance.ConnectGameServerByServerID(serverId);
                    SocketNiuNiuEvent.instance.isDisConnect = true;
                }
                else
                {
                    SocketSSSEvent.instance.Init();
                    SocketSSSEvent.instance.isDisConnect = true;
                    SocketSSSEvent.instance.ConnectGameServerByServerID(serverId);
                }
            }
            else
            {
                MySceneManager.instance.ScentToMain();
                SocketEngine.Instance.SocketQuit();
            }


        }
        public void ShowExit()
        {
            UIManager.instance.Show(UIType.UITipsDialog,
        (go) => { go.GetComponent<UIPanel_TipsDialog>().SetMes("房主强制解散房间，点击确认退出房间", StandUp); });
        }
        public void StandUp()
        {
            SocketSendManager.Instance.StandUp();
        }
    }
}