using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System;
using System.Runtime.InteropServices;

public class UIPanelNN : UIWindow
{
    UIPanel_ExitRoom UIPanel_ExitRoom;
    PlayerItemScript InitplayerItemScript;
    public Text RoomNum;
    public Text RoomCost;
    public Text RoomJuShu;
    public GameObject inviteFriend;
    public List<PlayerItemScript> playerItems;
    public List<PlayerItemScript> mPlayerItems;
    List<PlayerGameRoomInfo> playerList;
    public GameObject faPai;
    public GameObject pai;
    private Color32 _color32 = new Color32(255, 255, 255, 255);
    private int myChairId = -1;
    public GameObject readyBtn;
    public GameObject butCallBanker;
    public GameObject butNoCallBanker;
    public Button XiaZhu;
    public GameObject BeiShu;
    public GameObject qzNum;
    public GameObject cuoPaiBtn;
    public GameObject LiangPaiBtn;
    [HideInInspector]
    public Button butShowCard;
    public GameObject obj_FaPai;
    public Image img_Next;
    private int MyroomID;

    public Text timerText;
    public Text timerCuoPaitext;//搓牌倒计时text
    public Image stateTip;
    public Image Tip;
    private float timeBank = -1;//叫庄倒计时
    private float timeCuoPai = -1;//搓牌倒计时
    private float timeXiaZhu = -1;//下注倒计时
    private float timeShowCard = -1;//比牌倒计时
    public Image cuoPaiCard;
    public Image cuoPaiCardBg;
    public Image tipCuoPai;

    bool isCuoPai = false;

    void Awake()
    {
        AddListener();
    }
    public void AddListener()
    {

        SocketEventHandle.Instance.otherJoinRoomReply += OnOtherJoinRoomReply; //-
        SocketEventHandle.Instance.quitRoomReply += OnQuitRoomReply;
        SocketEventHandle.Instance.dissloveRoomReq += DissLoveRoomReq;
        SocketEventHandle.Instance.startGameReply += OnGameStart;
        SocketEventHandle.Instance.pickCardReply += SelectCardReply;
        SocketEventHandle.Instance.backBanker += CallBanker;
        SocketEventHandle.Instance.backXiaZhu += OnXiaZhu;
        SocketEventHandle.Instance.gameOverReply += SingleGameOver;
        SocketEventHandle.Instance.ALLgameOverReply += AllGameOver;
        SocketEventHandle.Instance.MPsendCard += mpSendCard;
        SocketEventHandle.Instance.userShowCard += userShowCard;
        SocketEventHandle.Instance.setRoomMark += SetRoomRemark;
        SocketEventHandle.Instance.backRoomReply += backRomm;//断线重连
        SocketEventHandle.Instance.messageBoxReply += OnMessageBoxReply;
    }

    private void RemoveListener()
    {
        SocketEventHandle.Instance.otherJoinRoomReply -= OnOtherJoinRoomReply;
        SocketEventHandle.Instance.quitRoomReply -= OnQuitRoomReply;
        SocketEventHandle.Instance.dissloveRoomReq -= DissLoveRoomReq;
        SocketEventHandle.Instance.startGameReply -= OnGameStart;
        SocketEventHandle.Instance.pickCardReply -= SelectCardReply;
        SocketEventHandle.Instance.backBanker -= CallBanker;
        SocketEventHandle.Instance.backXiaZhu -= OnXiaZhu;
        SocketEventHandle.Instance.gameOverReply -= SingleGameOver;
        SocketEventHandle.Instance.ALLgameOverReply -= AllGameOver;
        SocketEventHandle.Instance.MPsendCard -= mpSendCard;
        SocketEventHandle.Instance.userShowCard -= userShowCard;
        SocketEventHandle.Instance.setRoomMark -= SetRoomRemark;
        SocketEventHandle.Instance.backRoomReply -= backRomm;
        SocketEventHandle.Instance.messageBoxReply -= OnMessageBoxReply;
    }

    //断线重连
    private void backRomm(ClientResponse response)
    {
        if (SocketNiuNiuEvent.instance.nnstatus < 100)
        {
            for (int i = 0; i < mPlayerItems.Count; i++)
            {
                //  mPlayerItems[i].SetScore((int)GameDataNN.Instance.statusFreeNN.lTurnScore[mPlayerItems[i].myselfInfo.chairId]);

            }
            SocketNiuNiuEvent.instance.isDisConnect = false;
            return;
        }
        readyBtn.SetActive(false);
        if ((playStatue)SocketNiuNiuEvent.instance.myStatue < playStatue.US_PLAYING)
        {
            SocketNiuNiuEvent.instance.isDisConnect = false;
            return;
        }
        if (!SocketNiuNiuEvent.instance.isDisConnect)
            return;
        SocketNiuNiuEvent.instance.isDisConnect = false;

        switch (SocketNiuNiuEvent.instance.nnstatus)
        {
            case 100:///抢庄
                if (GameDataNN.Instance.statusCallNN.cbPlayStatus[GlobalDataScript.Instance.myGameRoomInfo.chairId] == 1)
                    ShowButCallBanker();
                inviteFriend.SetActive(false);
                MyDebug.Log("GameMode:" + GameDataNN.Instance.statusCallNN.cbGameMode);
                if (GameDataNN.Instance.statusCallNN.cbGameMode == 3)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < mPlayerItems.Count; j++)
                        {
                            if (i == 4)
                            {
                                ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                                {
                                    mPlayerItems[j].shouPai[i].sprite = sprite;
                                });
                            }
                            else
                            {
                                int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.statusCallNN.cbHandCardData[mPlayerItems[j].myselfInfo.chairId].HandCard[i]);
                                if (cardNum <= 0)
                                    cardNum = 1;
                                ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                                {
                                    mPlayerItems[j].shouPai[i].sprite = sprite;
                                });
                            }
                            mPlayerItems[j].shouPai[i].color = _color32;
                            mPlayerItems[j].shouPai[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                    }
                }
                #region 旧代码
                //else
                //{
                //    for (int i = 0; i < 5; i++)
                //    {
                //        for (int j = 0; j < mPlayerItems.Count; j++)
                //        {
                //            if (GlobalDataScript.Instance.myGameRoomInfo.chairId == j)
                //            {
                //                if (i == 4)
                //                {
                //                    ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                //                    {
                //                        mPlayerItems[j].shouPai[i].sprite = sprite;
                //                    });
                //                }
                //                else
                //                {
                //                    int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.statusCallNN.cbHandCardData[mPlayerItems[j].myselfInfo.chairId].HandCard[i]);
                //                    if (cardNum <= 0)
                //                        cardNum = 1;
                //                    ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                //                    {
                //                        mPlayerItems[j].shouPai[i].sprite = sprite;
                //                    });
                //                }
                //            }
                //            else
                //            {
                //                ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                //                {
                //                    mPlayerItems[j].shouPai[i].sprite = sprite;
                //                });
                //            }
                //            mPlayerItems[j].shouPai[i].color = _color32;
                //            mPlayerItems[j].shouPai[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                //        }

                //    }
                //}
                #endregion

                //积分
                for (int i = 0; i < mPlayerItems.Count; i++)
                {
                    mPlayerItems[i].SetScore((int)GameDataNN.Instance.statusCallNN.lCollectScore[mPlayerItems[i].myselfInfo.chairId]);

                }
                break;
            case 101:///下注
                inviteFriend.SetActive(false);
                if (GameDataNN.Instance.statusScoreNN.lTableScore[GlobalDataScript.Instance.myGameRoomInfo.chairId] <= 0 && GameDataNN.Instance.statusScoreNN.wBankerUser != myChairId)
                {
                    butXiaZhu();
                }

                if (GameDataNN.Instance.statusScoreNN.cbGameMode == 3)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        #region 旧代码
                        //for (int j = 0; j < mPlayerItems.Count; j++)
                        //{
                        //    if (i == 4)
                        //    {
                        //        ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                        //        {
                        //            mPlayerItems[j].shouPai[i].sprite = sprite;
                        //        });
                        //    }
                        //    else
                        //    {
                        //        int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.statusScoreNN.cbHandCardData[mPlayerItems[j].myselfInfo.chairId].HandCard[i]);
                        //        if (cardNum <= 0)
                        //            cardNum = 1;
                        //        ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                        //        {
                        //            mPlayerItems[j].shouPai[i].sprite = sprite;
                        //        });
                        //    }
                        //    mPlayerItems[j].shouPai[i].color = _color32;
                        //    mPlayerItems[j].shouPai[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                        //}

                        //if (i == 4)
                        //{
                        //    ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                        //    {
                        //        mPlayerItems[j].shouPai[i].sprite = sprite;
                        //    });
                        //}
                        //else
                        //{
                        //    int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.statusScoreNN.cbHandCardData[mPlayerItems[0].myselfInfo.chairId].HandCard[i]);
                        //    if (cardNum <= 0)
                        //        cardNum = 1;
                        //    ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                        //    {
                        //        mPlayerItems[0].shouPai[i].sprite = sprite;
                        //    });
                        //}
                        //mPlayerItems[0].shouPai[i].color = _color32;
                        //mPlayerItems[0].shouPai[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                        #endregion

                    }
                }
                for (int i = 0; i < mPlayerItems.Count; i++)
                {
                    //不叫庄
                    if (mPlayerItems[i].myselfInfo.chairId == GameDataNN.Instance.statusScoreNN.wBankerUser)
                    {
                        mPlayerItems[i].ShowBanker((int)GameDataNN.Instance.statusScoreNN.lTurnMaxScore);
                    }
                    else
                    {
                        mPlayerItems[i].HideBanker();
                    }

                    //房主
                    if (mPlayerItems[i].myselfInfo.chairId == GlobalDataScript.Instance.roomInfo.tableOwnerUserID)

                        mPlayerItems[i].ShowRoomer();
                    else
                        mPlayerItems[i].HideRoomer();

                    //积分
                    mPlayerItems[i].SetScore((int)GameDataNN.Instance.statusScoreNN.lCollectScore[mPlayerItems[i].myselfInfo.chairId]);
                    //  mPlayerItems[i].Init();

                    //下注倍数
                    mPlayerItems[i].XiaZhu((int)GameDataNN.Instance.statusScoreNN.lTableScore[mPlayerItems[i].myselfInfo.chairId]);
                }
                for (int i = 0; i < mPlayerItems.Count; i++)
                {
                    mPlayerItems[i].SetScore((int)GameDataNN.Instance.statusScoreNN.lCollectScore[mPlayerItems[i].myselfInfo.chairId]);

                }
                ////牛牛上庄初始化上一局数据
                //if (GlobalDataScript.Instance.roomInfo.gameMode != 3)
                //{
                //    InitPlayers();
                //}
                // XiaZhu.gameObject.SetActive(GameDataNN.Instance.gameStartInfo.wBankerUser != GlobalDataScript.Instance.myGameRoomInfo.chairId);

                break;


            case 102:///翻牌
                inviteFriend.SetActive(false);
                if (GameDataNN.Instance.statusPlayNN.cbGameMode == 3)
                {
                    InitPlayers();
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < mPlayerItems.Count; j++)
                        {
                            if (i == 4)
                            {
                                ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                                {
                                    mPlayerItems[j].shouPai[i].sprite = sprite;
                                });
                            }
                            else
                            {
                                int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.statusPlayNN.cbHandCardData[mPlayerItems[j].myselfInfo.chairId].CardData[i]);
                                if (cardNum <= 0)
                                    cardNum = 1;
                                ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                                {
                                    mPlayerItems[j].shouPai[i].sprite = sprite;
                                });
                            }
                            mPlayerItems[j].shouPai[i].color = _color32;
                            mPlayerItems[j].shouPai[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < mPlayerItems.Count; j++)
                        {
                            if (GlobalDataScript.Instance.myGameRoomInfo.chairId == mPlayerItems[j].myselfInfo.chairId)
                            {
                                if (i == 4 && GameDataNN.Instance.statusPlayNN.cbPlayStatus[GlobalDataScript.Instance.myGameRoomInfo.chairId] == 1)
                                {
                                    ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                                    {
                                        mPlayerItems[j].shouPai[i].sprite = sprite;
                                    });
                                }
                                else
                                {
                                    int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.statusPlayNN.cbHandCardData[mPlayerItems[j].myselfInfo.chairId].CardData[i]);
                                    if (cardNum <= 0)
                                        cardNum = 1;
                                    ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                                    {
                                        mPlayerItems[j].shouPai[i].sprite = sprite;
                                    });
                                }
                            }
                            else
                            {
                                ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                                {
                                    mPlayerItems[j].shouPai[i].sprite = sprite;
                                });
                            }
                            mPlayerItems[j].shouPai[i].color = _color32;
                            mPlayerItems[j].shouPai[i].transform.localEulerAngles = new Vector3(0, 0, 0);
                        }

                    }
                }
                if (GameDataNN.Instance.statusPlayNN.cbPlayStatus[GlobalDataScript.Instance.myGameRoomInfo.chairId] == 1)
                {

                    //cuoPaiBtn.gameObject.SetActive(true);
                    //LiangPaiBtn.gameObject.SetActive(true);
                    timeShowCard = GameConfig.GAME_SHOWCARD_TIME;
                    butShowCard.gameObject.SetActive(true);
                    stateTip.gameObject.SetActive(true);
                    ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/showcard", (sprite) =>
                    {
                        Tip.sprite = sprite;
                        Tip.SetNativeSize();
                    });
                    for (int i = 0; i < mPlayerItems.Count; i++)
                    {
                        //庄家显示
                        if (mPlayerItems[i].myselfInfo.chairId == GameDataNN.Instance.statusPlayNN.wBankerUser)
                        {
                            mPlayerItems[i].ShowBanker((int)GameDataNN.Instance.statusPlayNN.lTurnMaxScore);
                        }
                        else
                        {
                            mPlayerItems[i].HideBanker();
                        }
                        //房主
                        if (mPlayerItems[i].myselfInfo.chairId == GlobalDataScript.Instance.roomInfo.tableOwnerUserID)

                            mPlayerItems[i].ShowRoomer();
                        else
                            mPlayerItems[i].HideRoomer();

                        //下注倍数
                        mPlayerItems[i].XiaZhu((int)GameDataNN.Instance.statusPlayNN.lTableScore[mPlayerItems[i].myselfInfo.chairId]);
                        //积分
                        mPlayerItems[i].SetScore((int)GameDataNN.Instance.statusPlayNN.lCollectScore[mPlayerItems[i].myselfInfo.chairId]);
                    }
                }

                //else
                //{
                //timeShowCard = GameConfig.GAME_SHOWCARD_TIME;
                //butShowCard.gameObject.SetActive(true);
                //stateTip.gameObject.SetActive(true);
                //    ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/showcard", (sprite) =>
                //    {
                //        Tip.sprite = sprite;
                //        Tip.SetNativeSize();
                //    });
                //}

                GameDataNN.Instance.HandCard.cbCardData = GameDataNN.Instance.statusPlayNN.cbHandCardData;

                break;
        }
    }

    //下注回调
    private void OnXiaZhu(ClientResponse response)
    {
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (mPlayerItems[i].myselfInfo.chairId == GameDataNN.Instance.xiazhuInofoNN.chairId)
            {
                mPlayerItems[i].XiaZhu((int)GameDataNN.Instance.xiazhuInofoNN.lAddScoreCount);
            }
        }

        //牛牛上庄准备完不显示邀请
        if (GlobalDataScript.Instance.roomInfo.gameMode != 4)
        {
            inviteFriend.SetActive(false);
        }
        //牛牛上庄准备完初始数据
        //else
        //    InitPlayers();

    }

    //用户摊牌回调
    private void userShowCard(ClientResponse response)
    {
        MyDebug.Log("--------------------------摊牌回调-------------------------");
    }

    //明牌叫庄返回发牌4
    private void mpSendCard(ClientResponse response)
    {

        GlobalDataScript.Instance.isSigGameOver = false;
        if (GlobalDataScript.Instance.roomInfo.gameMode != 3)
            return;
        InitPlayers();
        StartCoroutine(BeginMP());
        MyDebug.Log("-----------------------------，明牌抢庄手牌数据---------------------------");
    }

    //发牌初始化上一局数据
    private void InitPlayers()
    {
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].InitNN();
            img_Next.gameObject.SetActive(false);
        }
    }

    IEnumerator Start()
    {
        SetRoomRemark();
        InitChair();
        SetPlayersInfo();
        SoundManager.Instance.PlayBGM("BackAudio1");
        yield return new WaitForSeconds(0.1f);
    }

    public void OnMessageBoxReply(ClientResponse response)
    {
        int userId = (int)GlobalDataScript.Instance.messageInfo.userid;
        string chatText = NetUtil.BytesToString(GlobalDataScript.Instance.messageInfo.chatText);

        mPlayerItems[PlayerIndex(userId)].ShowChat(chatText);
        MyDebug.Log("4454545454545454");
    }

    public int PlayerIndex(int userid)
    {
        int dex = 0;
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (mPlayerItems[i].myselfInfo.userID == userid)
                dex = i;
        }
        return dex;
    }

    enum slideVector { nullVector, left, right };

    private Vector2 lastPos;//上一个位置  

    private Vector2 currentPos;//下一个位置  

    private slideVector currentVector = slideVector.nullVector;//当前滑动方向  

    private float timer;//时间计数器  

    public float offsetTime = 0.01f;//判断的时间间隔


    void OnGUI()
    {
        if (!isCuoPai)
            return;
        if (Event.current.type == EventType.MouseDown)
        {//滑动开始  
            lastPos = Event.current.mousePosition;
            currentPos = Event.current.mousePosition;
            timer = 0;

            //TODO click event  
            MyDebug.Log("Click begin && Drag begin");
        }

        if (Event.current.type == EventType.MouseDrag)
        {//滑动过程  
            currentPos = Event.current.mousePosition;
            timer += Time.deltaTime;

            if (timer > offsetTime)
            {
                if ((lastPos.x - currentPos.x) < 0 && (lastPos.x - currentPos.x) > -120)
                    cuoPaiCardBg.transform.localEulerAngles = new Vector3(0, 0, -90 + (lastPos.x - currentPos.x) * 0.4f);
                //if (currentPos.x < lastPos.x)
                //{
                //    if (currentVector == slideVector.left)
                //    {

                //        return;
                //    }
                //    //TODO trun Left event  

                //    currentVector = slideVector.left;
                //    Debug.Log("Turn left");
                //}
                //if (currentPos.x > lastPos.x)
                //{
                //    if (currentVector == slideVector.right)
                //    {
                //        return;
                //    }
                //    //TODO trun right event  

                //    currentVector = slideVector.right;
                //    Debug.Log("Turn right");
                //}

                // lastPos = currentPos;
                timer = 0;
            }
        }

        if (Event.current.type == EventType.MouseUp)
        {//滑动结束  
            MyDebug.Log("sfsdf----------------------s:" + Math.Abs(lastPos.x - currentPos.x));
            if ((lastPos.x - currentPos.x) < -20)
            {
                isCuoPai = false;
                cuoPaiCardBg.gameObject.SetActive(false);

                int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.HandCard.cbCardData[GlobalDataScript.Instance.myGameRoomInfo.chairId].CardData[4]);
                if (cardNum <= 0)
                    cardNum = 1;
                ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                {
                    mPlayerItems[0].shouPai[4].sprite = sprite;
                });
                Invoke("SetcuoPaiDisable", 1);
            }

            currentVector = slideVector.nullVector;
            MyDebug.Log("Click over && Drag over");
        }
    }
    private void SetcuoPaiDisable()
    {
        cuoPaiCard.gameObject.SetActive(false);
        tipCuoPai.gameObject.SetActive(false);
        InitCuoPai();
        LiangPaiClick();
    }
    private void Update()
    {
        if (GlobalDataScript.Instance.roomInfo.gameMode != 4)
        {
            //5秒后自动叫庄
            if (timeBank >= 0)
            {
                timeBank -= Time.deltaTime;
                if (timeBank <= 0)
                {
                    NoCallBanker();
                    return;
                }
                timerText.text = Math.Floor(timeBank) + "";
            }

            //5秒后自动搓牌
            if (timeCuoPai >= 0)
            {
                timeCuoPai -= Time.deltaTime;
                if (timeCuoPai <= 0)
                {
                    SetcuoPaiDisable();
                    return;
                }
                timerCuoPaitext.text = Math.Floor(timeCuoPai) + "";
            }

            //6秒后自动下注
            if (timeXiaZhu >= 0)
            {
                timeXiaZhu -= Time.deltaTime;
                if (timeXiaZhu <= 0)
                {
                    StartCoroutine(DalyXiazhu());
                    return;
                }
                timerText.text = Math.Floor(timeXiaZhu) + "";
            }

            //7秒后自动翻牌
            if (timeShowCard >= 0)
            {
                timeShowCard -= Time.deltaTime;
                if (timeShowCard <= 0)
                {

                    ShowCard();
                    return;
                }
                timerText.text = Math.Floor(timeShowCard) + "";
            }
        }

        else
            stateTip.gameObject.SetActive(false);

        if (GlobalDataScript.Instance.showAllGameEnd)
        {
            SocketNiuNiuEvent.instance.isDisslove = true;
            GlobalDataScript.Instance.showAllGameEnd = false;
            StartCoroutine(ShowAllJieSuan());
        }
        if (!SocketNiuNiuEvent.instance.isDisConnect && !SocketNiuNiuEvent.instance.isDisslove)
        {
            if (!SocketEngine.Instance.isTcpConnect)
            {
                SocketEngine.Instance.SocketQuit();
                SocketNiuNiuEvent.instance.isDisConnect = true;
                SocketEventHandle.Instance.ShowReEnterTipDialog("网络已断开，检查网络后重试！");
            }
        }
    }


    //全局游戏结束
    private void AllGameOver(ClientResponse response)
    {
        MyDebug.Log("---------------------------全局游戏结束---------------------");
        //tCoroutine(ShowAllJieSuan());
    }

    //单局游戏结束
    private void SingleGameOver(ClientResponse response)
    {
        MyDebug.Log("---------------------------单局游戏结束---------------------");
        if (GlobalDataScript.Instance.roomInfo.gameMode != 3)
        {
            InitPlayers();
        }
        FanPai();
        StartCoroutine(ShowSingleJIeSuan());

        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].SetNormalScore((int)GameDataNN.Instance.gameEnd.lGameScore[mPlayerItems[i].myselfInfo.chairId]);
        }
    }

    public void FanPai()
    {
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].ShowNN();
            img_Next.gameObject.SetActive(true);
        }

        //StartCoroutine(FanPaiAniamtor());
    }

    IEnumerator FanPaiAniamtor()

    {
        for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)

        {
            mPlayerItems[i].shouPai[4].rectTransform.DORotate(new Vector3(0, 0, 0), 1)
                .SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.4f);
        }

    }


    //发牌回调
    private void SelectCardReply(ClientResponse response)
    {
        StartCoroutine(Begin());
        MyDebug.Log("-----------------------------，手牌数据---------------------------");

    }



    //叫庄回调
    private void CallBanker(ClientResponse response)
    {
        if (GameDataNN.Instance.callBanker.bFirst == 1)
        {
            if (GlobalDataScript.Instance.roomInfo.gameMode != 3)
                ShowButCallBanker();
        }

        GlobalDataScript.Instance.isSigGameOver = false;
        inviteFriend.SetActive(false);
        img_Next.gameObject.SetActive(false);

        for (int i = 0; i < mPlayerItems.Count; i++)
        {

            if (mPlayerItems[i].myselfInfo.chairId == GameDataNN.Instance.callBanker.wCallBanker)
            {
                //叫庄倍数
                mPlayerItems[i].XiaZhu((int)GameDataNN.Instance.callBanker.bBanker);
            }
            InitPlayers();
            mPlayerItems[i].Init();
            mPlayerItems[i].InitNN();
        }
    }


    private void DissLoveRoomReq(ClientResponse response)
    {
        UIManager.instance.Show(UIType.UIExitRoom, (go) => { go.GetComponent<UIPanel_DissloveRoom>().Init(int.Parse(response.message)); });
    }

    //叫庄完游戏开始
    public void OnGameStart(ClientResponse response)
    {
        //if (GlobalDataScript.Instance.playerInfos.Count < GlobalDataScript.Instance.roomInfo.playerNum)
        //    SocketEngine.Instance.SocketQuit();
        timeBank = -1;
        butNoCallBanker.SetActive(false);
        butCallBanker.SetActive(false);
        qzNum.SetActive(false);
        stateTip.gameObject.SetActive(false);

        GlobalDataScript.Instance.isSigGameOver = false;
        inviteFriend.SetActive(false);
        SetRoomRemark();
        List<int> maxBankList = new List<int>();

        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (GameDataNN.Instance.gameStartInfo.cbCallStatus[GameDataNN.Instance.gameStartInfo.wBankerUser] == GameDataNN.Instance.gameStartInfo.cbCallStatus[i])
            {
                maxBankList.Add(i);
            }
            mPlayerItems[i].HideBanker();
            mPlayerItems[i].Init();
        }

        int _dalyTime = 0;
        if (maxBankList.Count > 1)
        {
            _dalyTime = ShowAniQZ(maxBankList);
            //随机叫庄  播动画
            if (GameDataNN.Instance.gameStartInfo.cbCallStatus[GameDataNN.Instance.gameStartInfo.wBankerUser] == 0)
            {
                //显示无人叫庄，随机选庄
                ShowBankTip();
            }
            else
            {
                //多人叫相同的庄，随机选庄
                stateTip.gameObject.SetActive(false);
            }
        }
        else
        {
            //正常 一个人叫最大的庄

        }

        if (GameDataNN.Instance.gameStartInfo.wBankerUser != GlobalDataScript.Instance.myGameRoomInfo.chairId
            && GlobalDataScript.Instance.roomInfo.gameMode != 4)
        {
            Invoke("butXiaZhu", _dalyTime);
        }
        else//庄家
        {

            Invoke("WaitXiaZhuTip", _dalyTime);
        }

        //牛牛上庄
        if (GlobalDataScript.Instance.roomInfo.gameMode == 4)
        {
            InitPlayers();
            if (GameDataNN.Instance.gameStartInfo.wBankerUser != GlobalDataScript.Instance.myGameRoomInfo.chairId)
            {
                butXiaZhu();
                if (GameDataNN.Instance.callBanker.cbPlayerStatus[GlobalDataScript.Instance.myGameRoomInfo.chairId] == 0)
                {
                    stateTip.gameObject.SetActive(false);
                }
            }

            else
            {
                //stateTip.gameObject.SetActive(tuer);
                //ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/xzTip", (sprite) =>
                //{
                //    Tip.sprite = sprite;
                //    Tip.SetNativeSize();
                //});
            }
        }

        MyDebug.Log("sssssssssssssssssssssssssssss  ssssa游戏开始ssssssssssssssssssss" + GameDataNN.Instance.gameStartInfo.cbCardData[1]);
        // ShowAniQZ();
    }

    //庄家动画
    public int ShowAniQZ(List<int> chairList)
    {
        int _count = 0;
        for (int i = 0; i < chairList.Count; i++)
        {
            if (chairList[i] != GameDataNN.Instance.gameStartInfo.wBankerUser)
            {
                for (int j = 0; j < mPlayerItems.Count; j++)
                {
                    mPlayerItems[j].PlayBankAnima(chairList[i], _count);
                }
                _count++;
            }
        }
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (_count > 0)
                mPlayerItems[i].PlayBankAnima(GameDataNN.Instance.gameStartInfo.wBankerUser, _count);
        }
        return _count;
    }

    /// <summary>
    /// 无人抢庄，随机抢庄
    /// </summary>
    public void ShowBankTip()
    {
        stateTip.gameObject.SetActive(true);
        BeiShu.SetActive(false);
        timerText.text = "";
        ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/bankTip", (sprite) =>
        {
            Tip.sprite = sprite;
            Tip.SetNativeSize();
        });
    }

    /// <summary>
    /// 等待玩家下注
    /// </summary>
    public void WaitXiaZhuTip()
    {
        timeBank = GameConfig.GAME_BANK_TIME;
        stateTip.gameObject.SetActive(true);
        ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/xzTip", (sprite) =>
        {
            Tip.sprite = sprite;
            Tip.SetNativeSize();
        });
        //  timeXiaZhu = GameConfig.GAME_XIAZHU_TIME;
        Invoke("CloseTip", 5f);
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (GameDataNN.Instance.gameStartInfo.wBankerUser == mPlayerItems[i].myselfInfo.chairId)
                mPlayerItems[i].ShowBanker(GameDataNN.Instance.gameStartInfo.cbCallStatus[GameDataNN.Instance.gameStartInfo.wBankerUser]);
            else
                mPlayerItems[i].InitNN();
        }

    }



    /// <summary>
    /// 5秒后关闭提示框
    /// </summary>
    public void CloseTip()
    {
        stateTip.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ExitOrDissoliveRoom();
        RemoveListener();
    }

    private void InitChair()
    {
        GlobalDataScript.type = ModeType.None;
        mPlayerItems = new List<PlayerItemScript>();
        myChairId = -1;
        if (GlobalDataScript.Instance.roomInfo.playerNum == 3)
        {
            mPlayerItems.Add(playerItems[0]);
            mPlayerItems.Add(playerItems[2]);
            mPlayerItems.Add(playerItems[4]);
        }
        else if (GlobalDataScript.Instance.roomInfo.playerNum == 4)
        {
            mPlayerItems.Add(playerItems[0]);
            mPlayerItems.Add(playerItems[2]);
            mPlayerItems.Add(playerItems[3]);
            mPlayerItems.Add(playerItems[4]);
        }
        else if (GlobalDataScript.Instance.roomInfo.playerNum == 5)
        {
            mPlayerItems.Add(playerItems[0]);
            mPlayerItems.Add(playerItems[2]);
            mPlayerItems.Add(playerItems[3]);
            mPlayerItems.Add(playerItems[4]);
            mPlayerItems.Add(playerItems[5]);
        }
        else
        {
            mPlayerItems = playerItems;
        }
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].gameObject.SetActive(true);
            mPlayerItems[i].SetExit();
        }
    }
    public void SetPlayersInfo()
    {
        playerList = GlobalDataScript.Instance.playerInfos;
        for (int i = 0; i < playerList.Count; i++)
        {
            SetSeat(playerList[i]);
        }
    }

    public void OnOtherJoinRoomReply(ClientResponse response)
    {
        PlayerGameRoomInfo avatar = NetUtil.JsonToObj<PlayerGameRoomInfo>(response.message);
        SetSeat(avatar);
    }

    private void OnQuitRoomReply(ClientResponse response)
    {
        PlayerGameRoomInfo avatar = NetUtil.JsonToObj<PlayerGameRoomInfo>(response.message);
        if (avatar.userID == GlobalDataScript.userData.dwUserID)
        {

            if (SocketNiuNiuEvent.instance.isDisslove)
            {
                ExitOrDissoliveRoom();
                return;
            }

            UIManager.instance.Show(UIType.UITipsDialog, (obj) =>
            {
                SocketSSSEvent.instance.isDisslove = true;
                obj.GetComponent<UIPanel_TipsDialog>().SetMes("房间5分钟未开局，将自动解散，点击确认按钮返回大厅", ExitOrDissoliveRoom);
            });
            return;
        }

        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].SetExit(avatar.userID);
        }

        for (int i = 0; i < playerList.Count; i++)
        {
            if (avatar.userID == playerList[i].userID)
            {
                playerList.RemoveAt(i);
            }
        }
    }
    public void ExitOrDissoliveRoom()
    {
        TalkDataManager.Instance.ClearTalkData();
        if (GlobalDataScript.Instance.playerInfos != null)
        {
            GlobalDataScript.Instance.ClearGameInfo();

        }

        RemoveListener();
        TalkDataManager.Instance.ClearTalkData();
        SocketEngine.Instance.SocketQuit();
        MySceneManager.instance.BackToMain();


    }
    /// <summary>
    /// 设置当前角色的座位
    /// </summary>
    /// <param name="avatar">Avatar.</param>
    private void SetSeat(PlayerGameRoomInfo avatar)
    {

        MyDebug.Log("avatar.account.uuid:" + avatar.userID + "============== avatar.chairID:" + avatar.chairId);
        if (avatar.userID == GlobalDataScript.userData.dwUserID)
        {
            // readyBtn.SetActive(SocketNiuNiuEvent.instance.nnstatus <= 0);
            mPlayerItems[0].SetPlayerInfo(avatar);
        }
        else
        {
            myChairId = GetMyChairId();
            int seatIndex = myChairId - avatar.chairId;
            if (seatIndex < 0)
            {
                seatIndex = GlobalDataScript.Instance.roomInfo.playerNum + seatIndex;
            }

            mPlayerItems[seatIndex].SetPlayerInfo(avatar);
        }
    }
    private int GetMyChairId()
    {
        if (myChairId >= 0)
            return myChairId;
        for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)
        {
            if (GlobalDataScript.Instance.playerInfos[i].userID == GlobalDataScript.userData.dwUserID)
            {
                return GlobalDataScript.Instance.playerInfos[i].chairId;
            }
        }
        MyDebug.LogError("Do not found my Chaid id!");
        return -1;
    }
    private int GetDexByChairId(int chairId)
    {

        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (mPlayerItems[i].myselfInfo.chairId == chairId)
                return i;
        }
        return 0;
    }
    IEnumerator Begin()
    {
        pai.SetActive(true);
        Image image = faPai.GetComponent<Image>();
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < mPlayerItems.Count; i++)
            {
                ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                {
                    mPlayerItems[i].shouPai[j].sprite = sprite;
                });
                image.rectTransform.DOMove(mPlayerItems[i].shouPai[j].rectTransform.position, 0.2f)
                    .SetEase(Ease.Linear);
                image.rectTransform.DOSizeDelta(mPlayerItems[1].shouPai[j].rectTransform.sizeDelta,
                   0.2f);
                yield return new WaitForSeconds(0.2f);
                mPlayerItems[i].shouPai[j].color = _color32;
                image.rectTransform.localPosition = Vector3.zero;
                image.rectTransform.sizeDelta = new Vector2(117, 160);
                if (i == 0)
                {
                    if (j < 4)
                    {
                        mPlayerItems[i].shouPai[j].rectTransform.DORotate(new Vector3(0, 0, 0), 1)
                       .SetEase(Ease.Linear);
                        //yield return new WaitForSeconds(0.4f);
                        int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.HandCard.cbCardData[GlobalDataScript.Instance.myGameRoomInfo.chairId].CardData[j]);
                        ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                        {
                            mPlayerItems[i].shouPai[j].sprite = sprite;
                        });
                    }
                    else
                    {
                        mPlayerItems[i].shouPai[j].rectTransform.DORotate(new Vector3(0, 0, 0), 1)
                       .SetEase(Ease.Linear);
                        ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                         {
                             mPlayerItems[i].shouPai[j].sprite = sprite;
                         });
                    }

                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        pai.SetActive(false);
        cuoPaiBtn.gameObject.SetActive(true);
        LiangPaiBtn.gameObject.SetActive(true);

        //  butShowCard.gameObject.SetActive(true);

        ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/showcard", (sprite) =>
        {
            Tip.sprite = sprite;
            Tip.SetNativeSize();
        });
    }
    IEnumerator BeginMP()
    {
        pai.SetActive(true);
        Image image = faPai.GetComponent<Image>();
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < mPlayerItems.Count; i++)
            {
                image.rectTransform.DOMove(mPlayerItems[i].shouPai[j].rectTransform.position, 0.2f)
                    .SetEase(Ease.Linear);

                image.rectTransform.DOSizeDelta(mPlayerItems[1].shouPai[j].rectTransform.sizeDelta,
                   0.2f);
                yield return new WaitForSeconds(0.2f);
                mPlayerItems[i].shouPai[j].color = _color32;
                image.rectTransform.localPosition = Vector3.zero;
                image.rectTransform.sizeDelta = new Vector2(117, 160);
                if (j < 4)
                {
                    mPlayerItems[i].shouPai[j].rectTransform.DORotate(new Vector3(0, 0, 0), 1)
                        .SetEase(Ease.Linear);



                    int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.MPhandCard.cbCardData[mPlayerItems[i].myselfInfo.chairId].CardData[j]);



                    ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
                    {
                        mPlayerItems[i].shouPai[j].sprite = sprite;
                    });
                }
                else
                {
                    ResourcesLoader.Load<Sprite>("PuKe/card2", (sprite) =>
                    {
                        mPlayerItems[i].shouPai[j].sprite = sprite;
                    });
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        pai.SetActive(false);
        ShowButCallBanker();
    }
    //设置房间信息
    public void SetRoomRemark(ClientResponse response = null)
    {
        GameDataSSS.Instance.isSss = false;
        var roominfo = GlobalDataScript.Instance.roomInfo;
        RoomNum.text = roominfo.roomId;
        RoomJuShu.text = roominfo.PlayGameCount + "" + "/" + roominfo.limtNumber;
        switch (roominfo.gameMode)
        {
            case 1:
                RoomCost.text = "自由抢庄";
                break;
            case 2:
                RoomCost.text = "通比";
                break;
            case 3:
                RoomCost.text = "明牌抢庄";
                break;
            case 4:
                RoomCost.text = "牛牛上庄";
                break;

        }

    }

    //单局结算弹出框
    IEnumerator ShowSingleJIeSuan()
    {
        if (SocketNiuNiuEvent.instance.isDisslove)
            yield return new WaitForSeconds(0);
        else
            yield return new WaitForSeconds(3f);
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            RoomUserInfo info = new RoomUserInfo();
            info.name = mPlayerItems[i].nameText.text;
            info.headIcon = mPlayerItems[i].headerIcon.sprite;
            GlobalDataScript.Instance.playersNameList[mPlayerItems[i].myselfInfo.chairId] = info; ;
        }
        UIManager.instance.Show(UIType.UIRoomOver);

    }

    //全局结算弹出框
    IEnumerator ShowAllJieSuan()
    {
        yield return new WaitForSeconds(0f);
        UIManager.instance.Show(UIType.UIGameOver);

    }

    public void ShowSetting()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIGameRoomSetting);
    }
    public void SendReadly()
    {
        SocketSendManager.Instance.SendPlayerReady();
        readyBtn.gameObject.SetActive(false);
        SoundManager.Instance.PlaySoundByAction("ready", GlobalDataScript.Instance.myGameRoomInfo.sex);
    }

    //叫庄
    public void CallBanker()
    {
        timeBank = -1;
        butCallBanker.SetActive(false);
        butNoCallBanker.SetActive(false);
        stateTip.gameObject.SetActive(false);
        SocketSendManager.Instance.sendCallBaker(0);
        SoundManager.Instance.PlaySoundByAction("qiangzhuang", GlobalDataScript.Instance.myGameRoomInfo.sex);
    }

    //叫庄数
    public void BankNum(int dex)
    {
        timeBank = -1;
        butCallBanker.SetActive(false);
        butNoCallBanker.SetActive(false);
        stateTip.gameObject.SetActive(false);
        qzNum.SetActive(false);
        SocketSendManager.Instance.sendCallBaker(dex);
        SoundManager.Instance.PlaySoundByAction("qiangzhuang", GlobalDataScript.Instance.myGameRoomInfo.sex);
    }

    //不叫庄
    public void NoCallBanker()
    {
        timeBank = -1;
        butNoCallBanker.SetActive(false);
        butCallBanker.SetActive(false);
        qzNum.SetActive(false);
        stateTip.gameObject.SetActive(false);
        SocketSendManager.Instance.sendCallBaker(0);
        SoundManager.Instance.PlaySoundByAction("buqiang", GlobalDataScript.Instance.myGameRoomInfo.sex);
    }

    //亮牌
    public void ShowCard()
    {
        timeShowCard = -1;
        stateTip.gameObject.SetActive(false);
        butShowCard.gameObject.SetActive(false);
        cuoPaiBtn.gameObject.SetActive(false);
        LiangPaiBtn.gameObject.SetActive(false);
        SocketSendManager.Instance.sendShowCard();
    }

    //搓牌
    public void CuoPaiclick()
    {
        timeCuoPai = GameConfig.GAME_BANK_TIME;
        tipCuoPai.gameObject.SetActive(true);
        Tip.SetNativeSize();
        isCuoPai = true;
        cuoPaiBtn.gameObject.SetActive(false);
        LiangPaiBtn.gameObject.SetActive(false);
        cuoPaiCard.gameObject.SetActive(true);
        cuoPaiCardBg.gameObject.SetActive(true);


        int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.HandCard.cbCardData[GlobalDataScript.Instance.myGameRoomInfo.chairId].CardData[4]);
        if (cardNum <= 0)
            cardNum = 1;
        ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
        {
            cuoPaiCard.sprite = sprite;
        });


    }
    public void LiangPaiClick()
    {
        cuoPaiBtn.gameObject.SetActive(false);
        LiangPaiBtn.gameObject.SetActive(false);
        butShowCard.gameObject.SetActive(true);
        timeShowCard = GameConfig.GAME_SHOWCARD_TIME;
        stateTip.gameObject.SetActive(true);
        int cardNum = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.HandCard.cbCardData[GlobalDataScript.Instance.myGameRoomInfo.chairId].CardData[4]);
        if (cardNum <= 0)
            cardNum = 1;
        ResourcesLoader.Load<Sprite>("PuKe/card_" + cardNum, (sprite) =>
        {
            mPlayerItems[0].shouPai[4].sprite = sprite;
        });
    }

    //弹出下注按钮
    public void butXiaZhu()
    {
        InitPlayers();
        XiaZhu.gameObject.SetActive(true);
        stateTip.gameObject.SetActive(true);
        ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/beishu", (sprite) =>
        {
            Tip.sprite = sprite;
            Tip.SetNativeSize();
        });

        timeXiaZhu = GameConfig.GAME_XIAZHU_TIME;
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (GameDataNN.Instance.gameStartInfo.wBankerUser == mPlayerItems[i].myselfInfo.chairId)
                mPlayerItems[i].ShowBanker(GameDataNN.Instance.gameStartInfo.cbCallStatus[GameDataNN.Instance.gameStartInfo.wBankerUser]);
            else
                mPlayerItems[i].InitNN();
        }
    }


    //下注倍数
    public void PopBeiShu()
    {
        BeiShu.SetActive(true);
        XiaZhu.gameObject.SetActive(false);

    }

    #region 旧代码
    //IEnumerator delayBeiShu()
    //{
    //    for (int i = 0; i < mPlayerItems.Count; i++)
    //    {
    //        if (mPlayerItems[i].myselfInfo.chairId == GameDataNN.Instance.gameStartInfo.wBankerUser)
    //        {
    //            yield return new WaitForSeconds(5f);

    //            XiaZhu.gameObject.SetActive(false);
    //            BeiShu.SetActive(true);
    //        }
    //    }
    //}
    #endregion


    IEnumerator DalyXiazhu()
    {
        float _time = UnityEngine.Random.Range(1, 10) * 0.1f;
        MyDebug.Log(_time);
        yield return new WaitForSeconds(_time);
        butBeiShu(1);
    }
    public void butBeiShu(int dex)
    {
        timeXiaZhu = -1;
        XiaZhu.gameObject.SetActive(false);
        BeiShu.SetActive(false);
        stateTip.gameObject.SetActive(false);
        SocketSendManager.Instance.sendXZbeishu(dex);
    }
    public void ShowExit()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIExitGame);
    }

    public void ShowTalk()
    {
        UIManager.instance.Show(UIType.UITalk);
    }
    public void ShareRoomID()
    {
        // UnityPhoneManager.Instance.ShareSessionText("房间号：" + GlobalDataScript.Instance.roomInfo.roomId);
        UnityPhoneManager.Instance.ShareSessionText("房间号：" + GlobalDataScript.Instance.roomInfo.roomId + ",人数：" + GlobalDataScript.Instance.roomInfo.playerNum + "人场,"
          + GlobalDataScript.Instance.roomInfo.PlayGameCount + "局 " + RoomCost.text);
    }

    //显示叫庄按钮
    public void ShowButCallBanker()
    {

        timeBank = GameConfig.GAME_BANK_TIME;
        stateTip.gameObject.SetActive(true);
        ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Game/qz", (sprite) =>
        {
            Tip.sprite = sprite;
            Tip.SetNativeSize();
        });
        if (GlobalDataScript.Instance.roomInfo.gameMode == 3)
        {
            qzNum.SetActive(true);
            butNoCallBanker.SetActive(false);
            butCallBanker.SetActive(false);
            return;
        }
        qzNum.SetActive(false);
        butNoCallBanker.SetActive(true);
        butCallBanker.SetActive(true);
    }

    //搓牌
    private void InitCuoPai()
    {
        cuoPaiCardBg.gameObject.SetActive(false);
        cuoPaiCard.gameObject.SetActive(false);
        cuoPaiCardBg.transform.localEulerAngles = new Vector3(0, 0, -90);
    }

}