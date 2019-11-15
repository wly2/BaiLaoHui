using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System;
using LitJson;
using System.Runtime.InteropServices;

public class UIPanelSSS : UIWindow
{
    //=========单列===============//          
    //=========房间信息============//
    public Text RoomNum;
    public Text RoomCost;
    public Text PeopelNum;
    public Text RoomJuShu;
    //=======房间内图片信息======//
    public Image Readly;
    //===========设置按钮弹出框===============//
    public GameObject PutDowm;
    public GameObject inviteFriend;
    public GameObject center;
    public List<PlayerItemScript> playerItems;
    public List<PlayerItemScript> mPlayerItems;
    List<PlayerGameRoomInfo> playerList;
    public List<GameObject> shouPai;
    public GameObject faPai;
    public GameObject pai;
    public GameObject button;
    public GameObject ShowPuke;
    public GameObject ShareanReady;//
    private Color32 _color32 = new Color32(255, 255, 255, 255);
    private int myChairId = -1;
    public GameObject cardItem;
    public Button weidaoback;
    public Button zhongdaoback;//;
    public Button toudaoback;//;头道返回
    public Button allsure;//确定出牌
    public Button allback;//全部取消
    private float sendTypeTimeCount;
    private bool IsSendType;//是否发送请求
    public GameObject zhezhao;//遮罩
    public GameObject startEffect;//开始特效
    public GameObject quanleidaEffect;//全垒打特效
    public Button onesShowPuke;
    public Button specialPuke;
    private bool isquanleida;
    private bool isdaqiang;
    private bool isjiesuan=true;
    public List<PukeCardItem> ShouPaiCardList;
    public List<PukeCardItem> WeiDaoCardList;
    public List<PukeCardItem> ZhongDaoCardList;//
    public List<PukeCardItem> TouDaoCardList;//
    public GameObject readyBtn;//准备
    private Vector3 initPos = new Vector3(0, -50, 0);
    public List<Image> TypeButtonlist;
    private SUB_S_COMMONCARD selected_type = SUB_S_COMMONCARD.HT_SINGLE;//定义牌的类型
    public Image maPai;//马牌
    private int oneceNum = 0;
    void Awake()
    {
        AddListener();
    }
    public void AddListener()
    {

        SocketEventHandle.Instance.otherJoinRoomReply += OnOtherJoinRoomReply;
        SocketEventHandle.Instance.quitRoomReply += OnQuitRoomReply;
        SocketEventHandle.Instance.dissloveRoomReq += DissLoveRoomReq;
        SocketEventHandle.Instance.startGameReply += OnGameStart;
        SocketEventHandle.Instance.actionBtnReply += OnShowTypeStart;
        SocketEventHandle.Instance.pickCardReply += SelectCardReply;
        SocketEventHandle.Instance.otherPutOutCardReply += ShowCardReply;
        SocketEventHandle.Instance.huReply += CompreCardReply;
        SocketEventHandle.Instance.gameOverReply += SingleGameOver;
        SocketEventHandle.Instance.backRoomReply += backRomm;
        SocketEventHandle.Instance.setRoomMark += SetRoomRemark;
        SocketEventHandle.Instance.messageBoxReply += OnMessageBoxReply;
    }

    public void backRomm(ClientResponse response)
    {

        if ((playStatue)SocketSSSEvent.instance.myStatue < playStatue.US_PLAYING)
        {
            SocketSSSEvent.instance.isDisConnect = false;
            return;
        }
        //if((playStatue)SocketSSSEvent.instance.myStatue < playStatue.US_READY)
        //{
        //    ShowPuke.SetActive(false);
        //    SocketSSSEvent.instance.isDisConnect = false;
        //    return;
        //}
        if (!SocketSSSEvent.instance.isDisConnect)
            return;
        readyBtn.gameObject.SetActive(false);
        SocketSSSEvent.instance.isDisConnect = false;

        MyDebug.Log("断线从连--------------------------------------------吗--------------" + SocketSSSEvent.instance.myStatue);
        //nGameStart(null);
        //IsFinishSegment= NetUtil.PuCardChange((PU_KE)LogicSSS.Instance.gameStatusInfo.bFinishSegment[0]);
        center.SetActive(false);

        for (int i = 0; i < mPlayerItems.Count; i++)
        {
          
            mPlayerItems[i].SetScore((int)GameDataSSS.Instance.gameStatusInfo.lGameScore[mPlayerItems[i].myselfInfo.chairId]);
            mPlayerItems[i].SetPlayStatus();
            if (mPlayerItems[i].myselfInfo.chairId == GlobalDataScript.Instance.myGameRoomInfo.chairId)
            {

                if (mPlayerItems[i].gameStatus == 0)
                {

                    for (int j = 0; j < GameDataSSS.Instance.gameStatusInfo.cbHandCardData.Length; j++)
                    {
                        GameDataSSS.Instance.lastCard.Add(NetUtil.PuCardChange((PU_KE)GameDataSSS.Instance.gameStatusInfo.cbHandCardData[j]));
                        ShouPaiCardList[j].SetValue(NetUtil.PuCardChange((PU_KE)GameDataSSS.Instance.gameStatusInfo.cbHandCardData[j]));
                        ShouPaiCardList[j].gameObject.SetActive(true);
                    }
                    ShowPuke.SetActive(true);
                    ShareanReady.SetActive(false);
                    //OnShowTypeStart(null);//换成请求
                    SendSelectCard(SUB_S_COMMONCARD.HT_INVALID);
                }
            }
            else
            {
                for (int j = 0; j < 13; j++)
                {
                    mPlayerItems[i].InitCard(j);
                }
            }

        }


    }

    //确定全部出牌后比牌过程
    private void CompreCardReply(ClientResponse response)
    {
        StartCoroutine(ShowCompareRseult());
    }
    IEnumerator ShowCompareRseult()
    {

        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            for (int j = 0; j < mPlayerItems[i].cardList.Count; j++)
            {
                mPlayerItems[i].cardList[j].enabled = false;
            }

        }
        //显示头道牌值
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].setTouDao();

        }
        yield return new WaitForSeconds(1.2f);
        ////显示中道牌值
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].setZhongDao();
        }
        yield return new WaitForSeconds(1.2f);
        //显示尾道牌值
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].setWeiDao();
           
        }

        //显示打枪动画

        StartCoroutine(GunShow());
       
       
       
    }
    public IEnumerator GunShow()
    {
            for (int i = 0; i < mPlayerItems.Count - 1; i++)
            {
                for (int j = i + 1; j < mPlayerItems.Count; j++)
                {
                if (mPlayerItems[i].gunscorelist[0] > mPlayerItems[j].gunscorelist[0]
               && mPlayerItems[i].gunscorelist[1] > mPlayerItems[j].gunscorelist[1]
               && mPlayerItems[i].gunscorelist[2] > mPlayerItems[j].gunscorelist[2])
                {
                    mPlayerItems[i].gun.SetActive(true);
                    mPlayerItems[i].gun.transform.LookAt(mPlayerItems[j].transform.position);
                    yield return new WaitForSeconds(1f);
                    mPlayerItems[j].dankong1.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    mPlayerItems[j].dankong2.gameObject.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    mPlayerItems[j].dankong1.gameObject.SetActive(false);
                    mPlayerItems[j].dankong2.gameObject.SetActive(false);
                    mPlayerItems[i].gun.SetActive(false);
                    isdaqiang = true;



                }
                else if (mPlayerItems[i].gunscorelist[0] < mPlayerItems[j].gunscorelist[0]
               && mPlayerItems[i].gunscorelist[1] < mPlayerItems[j].gunscorelist[1]
               && mPlayerItems[i].gunscorelist[2] < mPlayerItems[j].gunscorelist[2])
                {
                    mPlayerItems[j].gun.SetActive(true);
                    mPlayerItems[j].gun.transform.LookAt(mPlayerItems[i].transform.position);
                    yield return new WaitForSeconds(1f);
                    mPlayerItems[i].dankong1.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    mPlayerItems[i].dankong2.gameObject.SetActive(true);
                    yield return new WaitForSeconds(1f);
                    mPlayerItems[i].dankong1.gameObject.SetActive(false);
                    mPlayerItems[i].dankong2.gameObject.SetActive(false);
                    mPlayerItems[j].gun.SetActive(false);
                }
                }
            }
        yield return new WaitForSeconds(0.5f);
        isquanleida = true;
        bool isquanleida1 = false;
        bool isquanleida2 = false;
        int tMax =0;
        int zMax = 0;
        int wMax = 0;

        for(int i = 0;i<mPlayerItems.Count-1;i++)
        {
            if (mPlayerItems[tMax].gunscorelist[0]> mPlayerItems[i+1].gunscorelist[0])
            {
                tMax = tMax;
            }
            else 
            {
                tMax = i + 1;
            }
        }
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
          if(i!=tMax)
            {
                if (mPlayerItems[i].gunscorelist[0] == mPlayerItems[tMax].gunscorelist[0])
                    tMax = -1;
            }
        }


        for (int i = 0; i < mPlayerItems.Count - 1; i++)
        {
            if (mPlayerItems[zMax].gunscorelist[1] > mPlayerItems[i + 1].gunscorelist[1])
            {
                zMax = zMax;
            }
            else
            {
                zMax = i + 1;
            }
        }
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (i != zMax)
            {
                if (mPlayerItems[i].gunscorelist[1] == mPlayerItems[zMax].gunscorelist[1])
                    zMax = -1;
            }
        }



        for (int i = 0; i < mPlayerItems.Count - 1; i++)
        {
            if (mPlayerItems[wMax].gunscorelist[2] > mPlayerItems[i + 1].gunscorelist[2])
            {
                wMax = wMax;
            }
            else
            {
                wMax = i + 1;
            }
        }
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            if (i != wMax)
            {
                if (mPlayerItems[i].gunscorelist[2] == mPlayerItems[wMax].gunscorelist[2])
                    wMax = -1;
            }
        }

        Debug.Log(mPlayerItems[wMax].myselfInfo.chairId);
        Debug.Log(mPlayerItems[zMax].myselfInfo.chairId);
        Debug.Log(mPlayerItems[tMax].myselfInfo.chairId);
        if (wMax == -1 || tMax == -1||zMax == -1||tMax!=zMax||tMax!=wMax||zMax!=wMax)
        {

        }else
        {
            quanleidaEffect.gameObject.SetActive(true);
            yield return new WaitForSeconds(1f);
            quanleidaEffect.gameObject.SetActive(false);
        
        }
        yield return new WaitForSeconds(1f);
        MyDebug.Log("----------------------jiesuan");
        UIManager.instance.Show(UIType.UIRoomOver);       
    }

    private void DissLoveRoomReq(ClientResponse response)
    {
        UIManager.instance.Show(UIType.UIExitRoom, (go) => { go.GetComponent<UIPanel_DissloveRoom>().Init(int.Parse(response.message)); });
    }

    private void OnDestroy()
    {
        ExitOrDissoliveRoom();
        RemoveListener();

    }
    private void RemoveListener()
    {
        SocketEventHandle.Instance.otherJoinRoomReply -= OnOtherJoinRoomReply;
        SocketEventHandle.Instance.quitRoomReply -= OnQuitRoomReply;
        SocketEventHandle.Instance.dissloveRoomReq -= DissLoveRoomReq;
        SocketEventHandle.Instance.startGameReply -= OnGameStart;
        SocketEventHandle.Instance.actionBtnReply -= OnShowTypeStart;
        SocketEventHandle.Instance.pickCardReply -= SelectCardReply;
        SocketEventHandle.Instance.otherPutOutCardReply -= ShowCardReply;
        SocketEventHandle.Instance.huReply -= CompreCardReply;
        SocketEventHandle.Instance.gameOverReply -= SingleGameOver;
        SocketEventHandle.Instance.backRoomReply -= backRomm;
        SocketEventHandle.Instance.setRoomMark -= SetRoomRemark;
        SocketEventHandle.Instance.messageBoxReply -= OnMessageBoxReply;
    }

    private void ShowCardReply(ClientResponse response)
    {
        if (response.message == "1")
        {
            IsSendType = false;
        }
        else if (response.message == "0")
        {
            IsSendType = false;
            return;
        }

        if (GameDataSSS.Instance.playersCard.wChairID == GlobalDataScript.Instance.myGameRoomInfo.chairId)
        {
            ShowPuke.SetActive(false);
            for (int i = 0; i < 13; i++)
            {
                mPlayerItems[0].InitCard(i);
            }

        }
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].ShowBackCards(GameDataSSS.Instance.playersCard.wChairID);
        }

    }

    IEnumerator Start()
    {


        InitCard();
        SetRoomRemark();
        InitChair();
        SoundManager.Instance.PlayBGM("BackAudio1");
        SetPlayersInfo();
        yield return new WaitForSeconds(0.1f);

    }
    void Update()
    {

        if(GameDataSSS.Instance.tdCardId.Count<=0)
        {
            toudaoback.gameObject.SetActive(false);
        }
        if (GameDataSSS.Instance.zdCardId.Count <= 0)
        {
            zhongdaoback.gameObject.SetActive(false);
        }
        if (GameDataSSS.Instance.wdCardId.Count <= 0)
        {
            weidaoback.gameObject.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            for (int i = 0; i < ShouPaiCardList.Count; i++)
            {
                if (ShouPaiCardList[i].gameObject.activeSelf)
                    ShouPaiCardList[i].setPos();
            }
            GameDataSSS.Instance.choseCount = 0;
            GameDataSSS.Instance.isPutOn = false;
            GameDataSSS.Instance.isMouseDown = false;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameDataSSS.Instance.isMouseDown = true;
        }
        if (GameDataSSS.Instance.isRefreshShouPai)
        {
            if (GameDataSSS.Instance.lastCard.Count > 0)
            {
                allback.gameObject.SetActive(false);
                allsure.gameObject.SetActive(false);
            }
            RefreshShouPaiList();
            GameDataSSS.Instance.isRefreshShouPai = false;
        }

        if (GlobalDataScript.Instance.showAllGameEnd)
        {
            GlobalDataScript.Instance.showAllGameEnd = false;
            SocketSSSEvent.instance.isDisslove = true;
            StartCoroutine(ShowAllJieSuan());
        }
        if (IsSendType)
        {
            zhezhao.gameObject.SetActive(true);
            sendTypeTimeCount += Time.deltaTime;
            if (sendTypeTimeCount > 2)
                IsSendType = false;
        }
        else
        {
            zhezhao.gameObject.SetActive(false);
        }
        if (!SocketSSSEvent.instance.isDisConnect && !SocketSSSEvent.instance.isDisslove)
        {
            if (!SocketEngine.Instance.isTcpConnect)
            {                                             
                HindShowCardTabel();
                GameDataSSS.Instance.Init();
                SocketEngine.Instance.SocketQuit();
                SocketSSSEvent.instance.isDisConnect = true;
                SocketEventHandle.Instance.ShowReEnterTipDialog("网络已断开，检查网络后重试！");

            }
        }
    }
    //全局结算弹出框
    IEnumerator ShowAllJieSuan()
    {
        yield return new WaitForSeconds(0f);
       
        UIManager.instance.Show(UIType.UIGameOver);

    }

    private void InitCard()
    {
        GameDataSSS.Instance.tdCardId = new List<int>();
        GameDataSSS.Instance.zdCardId = new List<int>();
        GameDataSSS.Instance.wdCardId = new List<int>();
        GameDataSSS.Instance.choiceCard = new List<int>();
        GameDataSSS.Instance.lastCard = new List<int>();
        ShouPaiCardList = new List<PukeCardItem>();
        ShouPaiCardList.Add(cardItem.GetComponent<PukeCardItem>());
        for (int i = 0; i < 12; i++)
        {
            GameObject go = Instantiate(cardItem);
            go.transform.SetParent(cardItem.transform.parent);
            go.transform.localScale = Vector3.one;
            ShouPaiCardList.Add(go.GetComponent<PukeCardItem>());
        }


    }
    //刷新手牌
    private void RefreshShouPaiList()
    {
        SortList(GameDataSSS.Instance.lastCard);
         for (int i=0;i<13;i++)
        {
            if (i >= GameDataSSS.Instance.lastCard.Count)
            {
                ShouPaiCardList[i].gameObject.SetActive(false);
                ShouPaiCardList[i].SetValue(0);
            }
            else
            {
                ShouPaiCardList[i].SetValue(GameDataSSS.Instance.lastCard[i]);
                ShouPaiCardList[i].gameObject.SetActive(true);
            }
        }
        MyDebug.Log("LastCard:" + GameDataSSS.Instance.lastCard.Count);
        MyDebug.Log("ChoicedCard:" + GameDataSSS.Instance.choiceCard.Count);

    }
    //游戏开始获取手牌信息
    public void OnGameStart(ClientResponse response)
    {
        //隐藏比牌后中的马牌
        if (GlobalDataScript.Instance.isSigGameOver)
        {
            for (int i = 0; i < mPlayerItems.Count; i++)
            {
                for (int j = 0; j < mPlayerItems[i].tdMalist.Count; j++)
                {
                    mPlayerItems[i].tdMalist[j].mapai.gameObject.SetActive(false);
                }
                for (int j = 0; j < mPlayerItems[i].zdMalist.Count; j++)
                {
                    mPlayerItems[i].zdMalist[j].mapai.gameObject.SetActive(false);
                }
                for (int j = 0; j < mPlayerItems[i].wdMalist.Count; j++)
                {
                    mPlayerItems[i].wdMalist[j].mapai.gameObject.SetActive(false);
                }
            }

        }

        GlobalDataScript.Instance.isSigGameOver = false;
        isquanleida = false;
        readyBtn.gameObject.SetActive(false);
        inviteFriend.SetActive(false);
        SetRoomRemark();
        HindShowCardTabel();
        StartCoroutine(PlayCardAnima());
    }
    IEnumerator PlayCardAnima()
    {
        startEffect.SetActive(true);
        yield return new WaitForSeconds(1.05f);
        startEffect.SetActive(false);
        //手牌排序
        SortCard(GlobalDataScript.Instance.cardInfo.cardlist);

        for (int i = 0; i < 13; i++)
        {
            GameDataSSS.Instance.lastCard.Add(GlobalDataScript.Instance.cardInfo.cardlist[i]);
            ShouPaiCardList[i].SetValue(GlobalDataScript.Instance.cardInfo.cardlist[i]);
            ShouPaiCardList[i].gameObject.SetActive(true);
            for (int j = 1; j < mPlayerItems.Count; j++)
            {

                mPlayerItems[j].InitCard(i);
            }
            yield return new WaitForSeconds(0.1f);
        }
        ShowPuke.SetActive(true);
        if (GlobalDataScript.Instance.iswSpecialType!=0)
        {
            specialPuke.enabled = true;
            specialPuke.image.color = Color.white;
        }
        ShareanReady.SetActive(false);
        OnShowTypeStart(null);
    }

    //对应显示类型
    public void OnShowTypeStart(ClientResponse response = null)
    {
        for (int i = 0; i < 9; i++)
        {
            if (GlobalDataScript.Instance.cardInfo.selectcardList[i] == 0)
            {
                MyDebug.Log("ssssssssssss" + GlobalDataScript.Instance.cardInfo.selectcardList[i]);
                TypeButtonlist[i].color = Color.gray;
                TypeButtonlist[i].GetComponent<Button>().enabled = false;
            }
            else
            {
                TypeButtonlist[i].color = Color.white;
                TypeButtonlist[i].GetComponent<Button>().enabled = true;
            }

        }
    }
    public void ShowCardList()
    {
        allback.gameObject.SetActive(false);
        allsure.gameObject.SetActive(false);
    }
    public void SelectCardReply(ClientResponse response)
    {
        //如果服务器没返回数据的话则遮罩去除
        if (response == null)
        {
            IsSendType = false;
        }
        for (int i = 0; i < ShouPaiCardList.Count; i++)
        {
            if (GameDataSSS.Instance.choiceCard.Contains(ShouPaiCardList[i].cardPoint))
            {
                ShouPaiCardList[i].SetSelect(true);
            }
            else
            {
                ShouPaiCardList[i].SetSelect(false);
            }
        }
        IsSendType = false;
    }
    //单局游戏
    private void SingleGameOver(ClientResponse response)
    {
        GameDataSSS.Instance.isSss = true;
        MyDebug.Log("---------------------------单局游戏结束---------------------");
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].SetNormalScore((int)GameDataSSS.Instance.gameEnd.lGameScore[mPlayerItems[i].myselfInfo.chairId]);

            mPlayerItems[i].gunscorelist.Clear();
        }
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            RoomUserInfo info = new RoomUserInfo();
            info.name = mPlayerItems[i].nameText.text;
            info.headIcon = mPlayerItems[i].headerIcon.sprite;
            GlobalDataScript.Instance.playersNameList[mPlayerItems[i].myselfInfo.chairId] = info; ;
        }
        if (SocketSSSEvent.instance.isDisslove)
        {
           UIManager.instance.Show(UIType.UIRoomOver);
        }
             
    

    }
    //断线从连显示
    public void OnReShow()
    {
        if (GameDataSSS.Instance.gameStatusInfo.cbFrontCard[myChairId].arrayItem.Length > 0)
        {
            for (int i = 0; i < GameDataSSS.Instance.gameStatusInfo.cbFrontCard[myChairId].arrayItem.Length; i++)
            {
                GameDataSSS.Instance.tdCardId.Add(GameDataSSS.Instance.gameStatusInfo.cbFrontCard[myChairId].arrayItem[i]);
            }
            for (int i = 0; i < GameDataSSS.Instance.tdCardId.Count; i++)
            {
                TouDaoCardList[i].gameObject.SetActive(true);
                TouDaoCardList[i].SetValue(GameDataSSS.Instance.tdCardId[i], SHISANSHUISHOW.SHISANSHUITOUDAO);
            }
        }
        if (GameDataSSS.Instance.tdCardId.Count > 0)
        {
            toudaoback.gameObject.SetActive(true);
        }

        if (GameDataSSS.Instance.gameStatusInfo.cbMidCard[myChairId].arrayItem.Length > 0)
        {
            for (int i = 0; i < GameDataSSS.Instance.gameStatusInfo.cbMidCard[myChairId].arrayItem.Length; i++)
            {
                GameDataSSS.Instance.zdCardId.Add(GameDataSSS.Instance.gameStatusInfo.cbMidCard[myChairId].arrayItem[i]);
            }
            for (int i = 0; i < GameDataSSS.Instance.zdCardId.Count; i++)
            {

                ZhongDaoCardList[i].gameObject.SetActive(true);
                ZhongDaoCardList[i].SetValue(GameDataSSS.Instance.zdCardId[i], SHISANSHUISHOW.SHISANSHUIZHONGDAO);
            }
        }
        if (GameDataSSS.Instance.zdCardId.Count > 0)
        {
            zhongdaoback.gameObject.SetActive(true);
        }
        if (GameDataSSS.Instance.gameStatusInfo.cbBackCard[myChairId].arrayItem.Length > 0)
        {
            for (int i = 0; i < GameDataSSS.Instance.gameStatusInfo.cbBackCard[myChairId].arrayItem.Length; i++)
            {
                GameDataSSS.Instance.wdCardId.Add(GameDataSSS.Instance.gameStatusInfo.cbBackCard[myChairId].arrayItem[i]);
            }

            for (int i = 0; i < GameDataSSS.Instance.wdCardId.Count; i++)
            {
                WeiDaoCardList[i].gameObject.SetActive(true);
                WeiDaoCardList[i].SetValue(GameDataSSS.Instance.wdCardId[i], SHISANSHUISHOW.SHISANSHUIWEIDAO);
            }

        }
        if (GameDataSSS.Instance.wdCardId.Count > 0)
        {
            weidaoback.gameObject.SetActive(true);
        }

    }

    //一鍵摆牌
    public void OneShowPuke()
    {
        if(GlobalDataScript.Instance.cardInfo.sortedList==null)
        {
            GameDataSSS.Instance.tdCardId.Clear();
            GameDataSSS.Instance.zdCardId.Clear();
            GameDataSSS.Instance.wdCardId.Clear();
            for (int j = 0; j < ShouPaiCardList.Count; j++)
            {
                if (j < 3)
                {
                    GameDataSSS.Instance.tdCardId.Add(ShouPaiCardList[j].cardPoint);
                }
                else if (3 <= j && j < 8)
                {
                    GameDataSSS.Instance.zdCardId.Add(ShouPaiCardList[j].cardPoint);
                }
                else if (8 <= j && j < 13)
                {
                    GameDataSSS.Instance.wdCardId.Add(ShouPaiCardList[j].cardPoint);
                }
            }
        }
        else
        {

            if (GlobalDataScript.Instance.cardInfo.sortedList.Count < 0)
            {
                return;
            }
            if (GlobalDataScript.Instance.cardInfo.sortedList[oneceNum][1] == 0)
            {
                oneceNum = ++oneceNum % 4;
                OneShowPuke();
                return;
            }
            GameDataSSS.Instance.tdCardId.Clear();
            GameDataSSS.Instance.zdCardId.Clear();
            GameDataSSS.Instance.wdCardId.Clear();

            for (int j = 0; j < GlobalDataScript.Instance.cardInfo.sortedList[oneceNum].Count; j++)
            {
                if (j < 3)
                {
                    GameDataSSS.Instance.tdCardId.Add(GlobalDataScript.Instance.cardInfo.sortedList[oneceNum][j]);
                }
                else if (3 <= j && j < 8)
                {
                    GameDataSSS.Instance.zdCardId.Add(GlobalDataScript.Instance.cardInfo.sortedList[oneceNum][j]);
                }
                else if (8 <= j && j < 13)
                {
                    GameDataSSS.Instance.wdCardId.Add(GlobalDataScript.Instance.cardInfo.sortedList[oneceNum][j]);
                }
            }
            SortList(GameDataSSS.Instance.tdCardId);
            SortList(GameDataSSS.Instance.zdCardId);
            SortList(GameDataSSS.Instance.wdCardId);
        }
      
        for (int i = 0; i < GameDataSSS.Instance.tdCardId.Count; i++)
        {
            TouDaoCardList[i].gameObject.SetActive(true);
            TouDaoCardList[i].SetValue(GameDataSSS.Instance.tdCardId[i], SHISANSHUISHOW.SHISANSHUITOUDAO);
        }
        toudaoback.gameObject.SetActive(true);

        for (int i = 0; i < GameDataSSS.Instance.zdCardId.Count; i++)
        {

            ZhongDaoCardList[i].gameObject.SetActive(true);
            ZhongDaoCardList[i].SetValue(GameDataSSS.Instance.zdCardId[i], SHISANSHUISHOW.SHISANSHUIZHONGDAO);
        }
        zhongdaoback.gameObject.SetActive(true);

        for (int i = 0; i < GameDataSSS.Instance.wdCardId.Count; i++)
        {
            WeiDaoCardList[i].gameObject.SetActive(true);
            WeiDaoCardList[i].SetValue(GameDataSSS.Instance.wdCardId[i], SHISANSHUISHOW.SHISANSHUIWEIDAO);
        }
        weidaoback.gameObject.SetActive(true);
        allback.gameObject.SetActive(true);
        allsure.gameObject.SetActive(true);
        GameDataSSS.Instance.lastCard.Clear();

        RefreshShouPaiList();
        weidaoback.gameObject.SetActive(true);
        GlobalDataScript.Instance.cardInfo.selectcardList = new int[9];
        OnShowTypeStart(null);
        oneceNum = ++oneceNum % 4;

    }
    //显示摆牌
    public void OnShowDao(int S)
    {

        switch ((SHISANSHUISHOW)S)
        {
            case SHISANSHUISHOW.SHISANSHUITOUDAO:
                if (GameDataSSS.Instance.choiceCard.Count > 3 - GameDataSSS.Instance.tdCardId.Count || GameDataSSS.Instance.choiceCard.Count == 0 ||GameDataSSS.Instance.tdCardId.Count==3)
                    return;
                GameDataSSS.Instance.isPutOn = true;
                for (int i = 0; i < GameDataSSS.Instance.choiceCard.Count; i++)
                {
                    GameDataSSS.Instance.tdCardId.Add(GameDataSSS.Instance.choiceCard[i]);
                }
                SortList(GameDataSSS.Instance.tdCardId);

                for (int i = 0; i < GameDataSSS.Instance.tdCardId.Count; i++)
                {
                    TouDaoCardList[i].gameObject.SetActive(true);
                    TouDaoCardList[i].SetValue(GameDataSSS.Instance.tdCardId[i], SHISANSHUISHOW.SHISANSHUITOUDAO);
                }
                toudaoback.gameObject.SetActive(true);
                break;
            case SHISANSHUISHOW.SHISANSHUIZHONGDAO:
                if (GameDataSSS.Instance.choiceCard.Count > 5 - GameDataSSS.Instance.zdCardId.Count || GameDataSSS.Instance.choiceCard.Count == 0 || GameDataSSS.Instance.zdCardId.Count == 5)
                    return;
                GameDataSSS.Instance.isPutOn = true;
                for (int i = 0; i < GameDataSSS.Instance.choiceCard.Count; i++)
                {
                    GameDataSSS.Instance.zdCardId.Add(GameDataSSS.Instance.choiceCard[i]);
                }
                SortList(GameDataSSS.Instance.zdCardId);

                for (int i = 0; i < GameDataSSS.Instance.zdCardId.Count; i++)
                {

                    ZhongDaoCardList[i].gameObject.SetActive(true);
                    ZhongDaoCardList[i].SetValue(GameDataSSS.Instance.zdCardId[i], SHISANSHUISHOW.SHISANSHUIZHONGDAO);
                }
                zhongdaoback.gameObject.SetActive(true);
                break;
            case SHISANSHUISHOW.SHISANSHUIWEIDAO:
                if (GameDataSSS.Instance.choiceCard.Count > 5 - GameDataSSS.Instance.wdCardId.Count || GameDataSSS.Instance.choiceCard.Count == 0|GameDataSSS.Instance.wdCardId.Count==5)
                    return;
                GameDataSSS.Instance.isPutOn = true;
                for (int i = 0; i < GameDataSSS.Instance.choiceCard.Count; i++)
                {
                    GameDataSSS.Instance.wdCardId.Add(GameDataSSS.Instance.choiceCard[i]);
                }
                SortList(GameDataSSS.Instance.wdCardId);
                for (int i = 0; i < GameDataSSS.Instance.wdCardId.Count; i++)
                {
                    WeiDaoCardList[i].gameObject.SetActive(true);
                    WeiDaoCardList[i].SetValue(GameDataSSS.Instance.wdCardId[i], SHISANSHUISHOW.SHISANSHUIWEIDAO);
                }
                weidaoback.gameObject.SetActive(true);
                break;
        }
        if (getChoseCount >= 2)
        {

            if (GameDataSSS.Instance.tdCardId.Count < 3)
            {
                for (int i = 0; i < GameDataSSS.Instance.lastCard.Count; i++)
                {
                    GameDataSSS.Instance.tdCardId.Add(GameDataSSS.Instance.lastCard[i]);
                }
                SortList(GameDataSSS.Instance.tdCardId);
                for (int i = 0; i < GameDataSSS.Instance.tdCardId.Count; i++)
                {
                    TouDaoCardList[i].gameObject.SetActive(true);
                    TouDaoCardList[i].SetValue(GameDataSSS.Instance.tdCardId[i], SHISANSHUISHOW.SHISANSHUITOUDAO);
                }
                toudaoback.gameObject.SetActive(true);
            }
            if (GameDataSSS.Instance.zdCardId.Count < 5)
            {
                for (int i = 0; i < GameDataSSS.Instance.lastCard.Count; i++)
                {
                    GameDataSSS.Instance.zdCardId.Add(GameDataSSS.Instance.lastCard[i]);
                }
                SortList(GameDataSSS.Instance.zdCardId);

                for (int i = 0; i < GameDataSSS.Instance.zdCardId.Count; i++)
                {

                    ZhongDaoCardList[i].gameObject.SetActive(true);
                    ZhongDaoCardList[i].SetValue(GameDataSSS.Instance.zdCardId[i], SHISANSHUISHOW.SHISANSHUIZHONGDAO);
                }
                zhongdaoback.gameObject.SetActive(true);
            }
            if (GameDataSSS.Instance.wdCardId.Count < 5)
            {
                for (int i = 0; i < GameDataSSS.Instance.lastCard.Count; i++)
                {
                    GameDataSSS.Instance.wdCardId.Add(GameDataSSS.Instance.lastCard[i]);
                }
                SortList(GameDataSSS.Instance.wdCardId);
                for (int i = 0; i < GameDataSSS.Instance.wdCardId.Count; i++)
                {
                    WeiDaoCardList[i].gameObject.SetActive(true);
                    WeiDaoCardList[i].SetValue(GameDataSSS.Instance.wdCardId[i], SHISANSHUISHOW.SHISANSHUIWEIDAO);
                }
                weidaoback.gameObject.SetActive(true);
            }
            GameDataSSS.Instance.lastCard.Clear();
            allback.gameObject.SetActive(true);
            allsure.gameObject.SetActive(true);
            GlobalDataScript.Instance.cardInfo.selectcardList = new int[9];
            OnShowTypeStart(null);


        }
        GameDataSSS.Instance.choiceCard.Clear();
        SendSelectCard(SUB_S_COMMONCARD.HT_INVALID);
        RefreshShouPaiList();
    }
    public int getChoseCount
    {
        get
        {
            int count = 0;
            if (GameDataSSS.Instance.tdCardId.Count >= 3)
                count++;
            if (GameDataSSS.Instance.zdCardId.Count >= 5)
                count++;
            if (GameDataSSS.Instance.wdCardId.Count >= 5)
                count++;

            return count;
        }
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
            if (SocketSSSEvent.instance.isDisslove)
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
            GlobalDataScript.Instance.ClearGameInfo();
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
            GlobalDataScript.Instance.myGameRoomInfo = avatar;
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

            MyDebug.Log("seatIndex:" + seatIndex);
            mPlayerItems[seatIndex].SetPlayerInfo(avatar);
        }
        if ((playStatue)SocketSSSEvent.instance.myStatue == playStatue.US_PLAYING)
        {
            //nGameStart(null);


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

    IEnumerator Begin()
    {
        pai.SetActive(true);
        Image image = faPai.GetComponent<Image>();
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < shouPai.Count; i++)
            {
                image.rectTransform.DOMove(shouPai[i].GetComponentsInChildren<Image>()[j].rectTransform.position, 1)
                    .SetEase(Ease.Linear);
                image.rectTransform.DOSizeDelta(shouPai[1].GetComponentsInChildren<Image>()[j].rectTransform.sizeDelta,
                    1);
                yield return new WaitForSeconds(1);
                shouPai[i].GetComponentsInChildren<Image>()[j].color = _color32;
                image.rectTransform.localPosition = Vector3.zero;
                image.rectTransform.sizeDelta = new Vector2(117, 160);
                if (i == 0 && j < 4)
                {
                    shouPai[i].GetComponentsInChildren<Image>()[j].rectTransform.DORotate(new Vector3(0, 0, 0), 1)
                        .SetEase(Ease.Linear);
                    yield return new WaitForSeconds(0.4f);
                    int cardNum = UnityEngine.Random.Range(0, 13);
                    ResourcesLoader.Load<Sprite>("Puke/card_" + cardNum, (sprite) =>
                    {
                        shouPai[i].GetComponentsInChildren<Image>()[j].sprite = sprite;
                    });
                }
            }
        }

        yield return new WaitForSeconds(0.5f);
        pai.SetActive(false);
    }

    public void FanPai()
    {
        StartCoroutine(FanPaiAniamtor());
    }

    IEnumerator FanPaiAniamtor()
    {
        shouPai[0].GetComponentsInChildren<Image>()[4].rectTransform.eulerAngles = new Vector3(0, 180, 0);
        shouPai[0].GetComponentsInChildren<Image>()[4].rectTransform.DORotate(new Vector3(0, 0, 0), 1)
            .SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.4f);
        int cardNum = UnityEngine.Random.Range(0, 13);
        ResourcesLoader.Load<Sprite>("Puke/card_" + cardNum, (sprite) =>
        {
            shouPai[0].GetComponentsInChildren<Image>()[4].sprite = sprite;
        });
        button.SetActive(false);
    }
    //设置房间信息
    public void SetRoomRemark(ClientResponse response = null)
    {
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].comparereadly.gameObject.SetActive(false);
            mPlayerItems[i].complete.gameObject.SetActive(false);
        }
        GameDataSSS.Instance.isSss = true;
        RoomNum.text = GlobalDataScript.Instance.roomInfo.roomId;
        PeopelNum.text = GlobalDataScript.Instance.roomInfo.playerNum.ToString();
        RoomJuShu.text = GlobalDataScript.Instance.roomInfo.PlayGameCount + "/" + GlobalDataScript.Instance.roomInfo.limtNumber;
        if (GlobalDataScript.Instance.roomInfo.maPaiId > 0)
        {
            ResourcesLoader.Load<Sprite>("Puke/card_" + GlobalDataScript.Instance.roomInfo.maPaiId, (sprite) =>
              {
                  maPai.sprite = sprite;
              });
        }
        else
        {
            maPai.enabled = false;
        }
        if (GlobalDataScript.Instance.roomInfo.payType == 1)
        {
            RoomCost.text = "房主付费";
        }
        else
        {
            RoomCost.text = "AA支付";
        }
        specialPuke.enabled = false;
        specialPuke.image.color = Color.gray ;

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
    }
    public void ShowExit()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIExitGame);
    }

    //类型对应的牌型--连对
    public void LiangDui()
    {
      
        SendSelectCard(SUB_S_COMMONCARD.HT_TWO_DOUBLE);
    }
    //对子
    public void DuiZi()
    {
     
        SendSelectCard(SUB_S_COMMONCARD.HT_ONE_DOUBLE);
    }
    //三条
    public void SanTiao()
    {
       
        SendSelectCard(SUB_S_COMMONCARD.HT_THREE);
    }
    //顺子
    public void ShunZi()
    {
       
        SendSelectCard(SUB_S_COMMONCARD.HT_LINE);
    }
    //同花
    public void TongHua()
    {
      
        SendSelectCard(SUB_S_COMMONCARD.HT_COLOR);
    }
    //葫芦
    public void HuLu()
    {
        
        SendSelectCard(SUB_S_COMMONCARD.HT_THREE_DEOUBLE);
    }
    //铁支
    public void TieZhi()
    {
    
        SendSelectCard(SUB_S_COMMONCARD.HT_FOUR_BOOM);
    }
    //同花顺
    public void TongHuaShun()
    {
     
        SendSelectCard(SUB_S_COMMONCARD.HT_LINE_COLOR);
    }
    //同花顺
    public void WuTong()
    {
      
        SendSelectCard(SUB_S_COMMONCARD.HT_FIVE);
    }


    private SUB_S_COMMONCARD seletType = SUB_S_COMMONCARD.HT_INVALID;
    private void SendSelectCard(SUB_S_COMMONCARD type)
    {
        IsSendType = true;
        CMD_C_SelectCard selectCard = new CMD_C_SelectCard();
        selectCard.w_selected_type = (ushort)type;
        selectCard.cb_left_card = new byte[13];
        if (type == selected_type)
        {
            selectCard.cb_select_count = 2;
        }
        else
        {
            selectCard.cb_select_count = 1;
        }
        selected_type = type;
        for (int i = 0; i < GameDataSSS.Instance.choiceCard.Count; i++)
        {
            GameDataSSS.Instance.lastCard.Add(GameDataSSS.Instance.choiceCard[i]);
        }
        for (int i = 0; i < GameDataSSS.Instance.lastCard.Count; i++)
        {
            selectCard.cb_left_card[i] = NetUtil.PuCardChange(GameDataSSS.Instance.lastCard[i]);
        }
        if (GameDataSSS.Instance.lastCard.Count <= 0)
            return;
        GameDataSSS.Instance.choiceCard.Clear();
        MyDebug.Log("LastCard:" + GameDataSSS.Instance.lastCard.Count);
        MyDebug.Log("ChoicedCard:" + GameDataSSS.Instance.choiceCard.Count);
        selectCard.cb_left_card_count = (byte)GameDataSSS.Instance.lastCard.Count;
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_GAME, (int)SUB_C_SSS.SUB_C_SELECT_CARD, NetUtil.StructToBytes(selectCard), Marshal.SizeOf(selectCard));

    }
    public void CardBack(int type)
    {

        ReleasePuOnCar(type);
        RefreshHandCard();
    }
    public void AllSure()
    {

        CMD_C_ShowCard sendCard = new CMD_C_ShowCard();
        sendCard.cbFrontCard = new byte[3];
        sendCard.cbMidCard = new byte[5];
        sendCard.cbBackCard = new byte[5];
        sendCard.wSpecialType = (ushort)GlobalDataScript.Instance.iswSpecialType;
        for (int i = 0; i < GameDataSSS.Instance.tdCardId.Count; i++)
        {
            sendCard.cbFrontCard[i] = NetUtil.PuCardChange(GameDataSSS.Instance.tdCardId[i]);
        }
        for (int i = 0; i < GameDataSSS.Instance.zdCardId.Count; i++)
        {
            sendCard.cbMidCard[i] = NetUtil.PuCardChange(GameDataSSS.Instance.zdCardId[i]);
        }
        for (int i = 0; i < GameDataSSS.Instance.wdCardId.Count; i++)
        {
            sendCard.cbBackCard[i] = NetUtil.PuCardChange(GameDataSSS.Instance.wdCardId[i]);
        }
        mPlayerItems[GameDataSSS.Instance.playersCard.wChairID].comparereadly.gameObject.SetActive(true);
        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GF_GAME, (int)SUB_C_SSS.SUB_C_SHOWCARD, NetUtil.StructToBytes(sendCard), Marshal.SizeOf(sendCard));
    }
    public void AllBack()
    {
        ReleasePuOnCar(1);
        ReleasePuOnCar(2);
        ReleasePuOnCar(3);
        RefreshHandCard();
    }
    public void OnMessageBoxReply(ClientResponse response)
    {
        int userId = GlobalDataScript.Instance.messageInfo.userid;
        string chatText = NetUtil.BytesToString(GlobalDataScript.Instance.messageInfo.chatText);

        mPlayerItems[PlayerIndex(userId)].ShowChat(chatText);
    }
    public void MyselfSoundActionPlay()
    {
        mPlayerItems[0].ShowChatAction();
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
    public void ReleasePuOnCar(int type)
    {
        switch ((SHISANSHUISHOW)type)
        {
            case SHISANSHUISHOW.SHISANSHUITOUDAO:
                if (GameDataSSS.Instance.choiceCard.Count > 0)
                {
                    for (int i = 0; i < GameDataSSS.Instance.choiceCard.Count; i++)
                    {
                        GameDataSSS.Instance.lastCard.Add(GameDataSSS.Instance.choiceCard[i]);

                    }
                    GameDataSSS.Instance.choiceCard.Clear();
                }
                if (GameDataSSS.Instance.tdCardId.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < GameDataSSS.Instance.tdCardId.Count; i++)
                {
                    GameDataSSS.Instance.lastCard.Add(GameDataSSS.Instance.tdCardId[i]);
                }
                for(int i=0;i<3;i++)
                {
                    TouDaoCardList[i].gameObject.SetActive(false);
                    TouDaoCardList[i].mapai.gameObject.SetActive(false);
                }
                toudaoback.gameObject.SetActive(false);
                GameDataSSS.Instance.tdCardId.Clear();
                break;
            case SHISANSHUISHOW.SHISANSHUIZHONGDAO:
              
                if (GameDataSSS.Instance.choiceCard.Count > 0)
                {
                    for (int i = 0; i < GameDataSSS.Instance.choiceCard.Count; i++)
                    {
                        GameDataSSS.Instance.lastCard.Add(GameDataSSS.Instance.choiceCard[i]);

                    }
                    GameDataSSS.Instance.choiceCard.Clear();
                }
                if (GameDataSSS.Instance.zdCardId.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < GameDataSSS.Instance.zdCardId.Count; i++)
                {
                    GameDataSSS.Instance.lastCard.Add(GameDataSSS.Instance.zdCardId[i]);
                }
                for(int i=0;i<5;i++)
                {
                    ZhongDaoCardList[i].gameObject.SetActive(false);
                    ZhongDaoCardList[i].mapai.gameObject.SetActive(false);
                }
                zhongdaoback.gameObject.SetActive(false);
                GameDataSSS.Instance.zdCardId.Clear();
                break;
            case SHISANSHUISHOW.SHISANSHUIWEIDAO:
                
                if (GameDataSSS.Instance.choiceCard.Count>0)
                {
                    for (int i = 0; i < GameDataSSS.Instance.choiceCard.Count; i++)
                    {
                        GameDataSSS.Instance.lastCard.Add(GameDataSSS.Instance.choiceCard[i]);
                       
                    }
                    GameDataSSS.Instance.choiceCard.Clear();
                }
                if (GameDataSSS.Instance.wdCardId.Count <= 0)
                {
                    return;
                }
                for (int i = 0; i < GameDataSSS.Instance.wdCardId.Count; i++)
                {
                    GameDataSSS.Instance.lastCard.Add(GameDataSSS.Instance.wdCardId[i]);
                }
                for(int i=0;i<5;i++)
                {
                    WeiDaoCardList[i].gameObject.SetActive(false);
                    WeiDaoCardList[i].mapai.gameObject.SetActive(false);
                }
                weidaoback.gameObject.SetActive(false);
                GameDataSSS.Instance.wdCardId.Clear();
                break;
        }
        allback.gameObject.SetActive(false);
        allsure.gameObject.SetActive(false);
    }
    public void RefreshHandCard()
    {
        RefreshShouPaiList();
        SendSelectCard(SUB_S_COMMONCARD.HT_INVALID);
    }
    public void HindShowCardTabel()
    {
        if (GameDataSSS.Instance.isSss)
        {
            for (int i = 0; i < 3; i++)
            {
                TouDaoCardList[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < 5; i++)
            {
                ZhongDaoCardList[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < 5; i++)
            {
                WeiDaoCardList[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < mPlayerItems.Count; i++)
            {
                mPlayerItems[i].InitSSS();
            }
        }
    }
    ////单局结算弹出框
    //IEnumerator ShowSingleJIeSuan()
    //{
    //    if (SocketSSSEvent.instance.isDisslove)
    //        yield return new WaitForSeconds(0);
    //    else
    //        yield return new WaitForSeconds(5f);
    //    UIManager.instance.Show(UIType.UIRoomOver);
    //}
    public void ShowTalk()
    {
        UIManager.instance.Show(UIType.UITalk);
    }
    public void ShareRoomID()
    {
        string mes = "";
        byte rule =(byte)GlobalDataScript.Instance.roomInfo.gameMode;
        if((rule& (byte)GameMode.HORSE_CAED_MODE)>0)
        {
            mes += "马牌 ";
        }
        if((rule&(byte)GameMode.KING_MODE)>0)
        {
            mes += "百变 ";
        }
        if ((rule & (byte)GameMode.NULL_SUPPORT_SPECIAL_TYPE) > 0)
        {
            mes += "有特殊牌 ";
        }
        string sendMes = "房间号：" + GlobalDataScript.Instance.roomInfo.roomId + ",人数：" + GlobalDataScript.Instance.roomInfo.playerNum + "人场,"
+ GlobalDataScript.Instance.roomInfo.PlayGameCount + "局 " + mes;
        UnityPhoneManager.Instance.ShareSessionText(sendMes);
    }
    public void SortList(List<int> _list)
    {
        int[] arry = _list.ToArray();
        SortCard(arry);
        _list.Clear();
        for (int i = 0; i < arry.Length; i++)
        {
            _list.Add(arry[i]);
        }
    }
    public void SortCard(int[] cardlist)
    {
        int[] cardlength = new int[cardlist.Length];
        for (int i = 0; i < cardlist.Length; i++)
        {
            cardlength[i] = GetcardLength(cardlist[i]);
        }

        for (int i = 0; i < cardlength.Length; i++)
        {
            for (int j = 0; j < cardlength.Length - 1 - i; j++)
            {

                if (cardlength[j] < cardlength[j + 1])
                {
                    int temps = cardlength[j];
                    cardlength[j] = cardlength[j + 1];
                    cardlength[j + 1] = temps;
           
                    int temp = cardlist[j];
                    cardlist[j] = cardlist[j + 1];
                    cardlist[j + 1] = temp;

                }
            }
        }

    }
    public int GetcardLength(int card)
    {
        int cardvalue = card % 13;
        if (cardvalue == 0)
        {
            return 13;
        }
        if (cardvalue == 1)
        {
            return 14;
        }
        return cardvalue;
    }
    /// <summary>
    /// 特殊牌
    /// </summary>
    public void Speicail()
    {

        if(GlobalDataScript.Instance.iswSpecialType==0)
        { return; }
        AllBack();
        switch (GlobalDataScript.Instance.iswSpecialType)
        {
            case 1:
                Yitiaolong(GameDataSSS.Instance.lastCard);
                AllSure();
                break;
            case 2:
                ThreeColor();
                break;
            case 4:
                GetSortthreeShunzi(GameDataSSS.Instance.lastCard);
                AllSure();
                break;
            case 8:
                liangduiban(GameDataSSS.Instance.lastCard);
                AllSure();
                break;
            case 16:
                Yitiaolong(GameDataSSS.Instance.lastCard);
                AllSure();
                break;
        }
        RefreshShouPaiList();
    }
    //特殊牌型
    /// <summary>
    /// 全部特殊牌类型摆牌
    /// </summary>
    /// <param name="cardlist"></param>
    public void GetSortthreeShunzi(List<int> cardlist)
    {
        List<int> cardlength = new List<int>();

        for (int i = 0; i < cardlist.Count; i++)
        {
            cardlength.Add(GetcardLength(cardlist[i]));
        }
        int max = 0;


        for (int i = 0; i < cardlength.Count; i++)
        {
            if (cardlength[i] == cardlength[i + 1] + 1)
                if (i == 0)
                {
                    GameDataSSS.Instance.wdCardId.Add(cardlist[0]);
                }
            if (GameDataSSS.Instance.wdCardId.Count == 5)
            {
                continue;
            }
            if (cardlength[max] == cardlength[i] + 1)
            {
                GameDataSSS.Instance.wdCardId.Add(cardlist[i]);
                cardlist.Remove(cardlist[i]);
                if (GameDataSSS.Instance.wdCardId.Count == 5)
                {
                    continue;
                }
                else
                {
                    GameDataSSS.Instance.wdCardId.Add(cardlist[i + 1]);
                    cardlist.Remove(cardlist[i]);
                }
                max = i;
            }


        }

        for (int i = 0; i < GameDataSSS.Instance.wdCardId.Count; i++)
        {
            if (cardlength.Contains(GameDataSSS.Instance.wdCardId[i]))
            {
                cardlength.Remove(GameDataSSS.Instance.wdCardId[i]);
                cardlist.Remove(GameDataSSS.Instance.wdCardId[i]);
            }
        }
        int max1 = 0;

        for (int i = 0; i < cardlength.Count; i++)
        {
            if (i == 0)
            {
                GameDataSSS.Instance.zdCardId.Add(cardlist[0]);
            }
            if (GameDataSSS.Instance.zdCardId.Count == 5)
            {
                continue;
            }
            if (cardlength[max1] == cardlength[i] + 1)
            {
                GameDataSSS.Instance.zdCardId.Add(cardlist[i]);
                max1 = i;
            }
        }

        for (int i = 0; i < GameDataSSS.Instance.zdCardId.Count; i++)
        {
            if (cardlength.Contains(GameDataSSS.Instance.zdCardId[i]))
            {
                cardlength.Remove(GameDataSSS.Instance.zdCardId[i]);
                cardlist.Remove(GameDataSSS.Instance.zdCardId[i]);
            }
        }
        for (int i = 0; i < cardlength.Count; i++)
        {
            GameDataSSS.Instance.tdCardId.Add(cardlist[i]);
        }
    }
    /// <summary>
    /// 三同花
    /// </summary>
    public void ThreeColor()
    {
        MyDebug.Log("assadad");
    }
    /// <summary>
    ///两对半
    /// </summary>
    public void liangduiban(List<int> cardlist)
    {
        List<int> cardlength = new List<int>();

        for (int i = 0; i < cardlist.Count; i++)
        {
            cardlength.Add(GetcardLength(cardlist[i]));
        }

        for (int i = 0; i < cardlength.Count; i++)
        {
            i = 0;
            GameDataSSS.Instance.wdCardId.Add(cardlist[i]);
            cardlist.Remove(cardlist[i]);
            cardlength.Remove(cardlist[i]);
            if (GameDataSSS.Instance.wdCardId.Count == 5)
            { continue; }
        }
        for (int i = 0; i < cardlist.Count; i++)
        {
            i = 0;
            GameDataSSS.Instance.zdCardId.Add(cardlist[i]);
            cardlist.Remove(cardlist[i]);
            cardlength.Remove(cardlist[i]);
            if (GameDataSSS.Instance.zdCardId.Count == 5)
            { continue; }
        }
        for (int i = 0; i <=cardlist.Count; i++)
        {
            i = 0;
            GameDataSSS.Instance.tdCardId.Add(cardlist[i]);
            cardlength.Remove(cardlist[i]);
            cardlist.Remove(cardlist[i]);
        }
    }
    /// <summary>
    ///  同花一条龙or一条龙
    /// </summary>
    public void Yitiaolong(List<int> cardlist)
    {
        List<int> cardlength = new List<int>();

        for (int i = 0; i < cardlist.Count; i++)
        {
            cardlength.Add(GetcardLength(cardlist[i]));
        }
        for (int i = 0; i < cardlength.Count; i++)
        {
            if (GameDataSSS.Instance.wdCardId.Count == 5)
            {
                continue;
            }
            i = 0;
            if (cardlength[i] == cardlength[i + 1] + 1)
            {
                GameDataSSS.Instance.wdCardId.Add(cardlist[i]);
                cardlength.Remove(cardlength[i]);
                cardlist.Remove(cardlist[i]);
            }
        }
        for (int i = 0; i < cardlength.Count; i++)
        {
            if (GameDataSSS.Instance.zdCardId.Count == 5)
            {
                continue;
            }
            i = 0;
            if (cardlength[i] == cardlength[i + 1] + 1)
            {
                GameDataSSS.Instance.zdCardId.Add(cardlist[i]);
                cardlength.Remove(cardlength[i]);
                cardlist.Remove(cardlist[i]);
            
            }
        }
        for (int i = 0; i <=cardlength.Count; i++)
        {
            i = 0;
            GameDataSSS.Instance.tdCardId.Add(cardlist[i]);
            cardlength.Remove(cardlength[i]);
            cardlist.Remove(cardlist[i]);
        }
    }
}