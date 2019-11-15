using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp;
using UnityEngine.UI;
using LitJson;
using System.Runtime.InteropServices;
using System.Collections;

public class UIMaJiangPanel : MonoBehaviour
{
    #region 变量
    //============单例===============//
    public event EventHandler ReSetPoisiton;
    public static UIMaJiangPanel instance;
    public static BottomScript bottomScript;
   
    public Text roomNum;//房间号
    public double lastTime;
    public Text Number;
    public Text firCount;
    //==========碰杠胡等效果==========//
    public GameObject pengEffectGame;
    public GameObject gangEffectGame;
    public GameObject huEffectGame;
    public GameObject liujuEffectGame;
    public GameObject chiEffectGame;
    //==========设置按钮、弹出框=============//
    public GameObject putDown;
    public GameObject packUp;
    public GameObject btnGroup;
    public GameObject rule;//玩法弹出框
    //==========碰杠牌等花色============//
    public int otherChiCard;
    public int otherPengCard;
    public int otherGangCard;
    public ButtonActionScript btnActionScript;//单例脚本ButtonActionScript
    public List<PlayerItemScript> playerItems;//游戏玩家
    public Text LeavedCastNumText; //牌堆剩余数
    public List<AvatarVO> avatarList;
    public Button inviteFriendButton;//邀请按钮
    public Button BeginButton;//准备按钮
    //==============选择吃牌按钮================//
    public Button left;
    public Button right;
    public Button center;
    public Image live1;//剩余牌数弹出框
    public GameObject genZhuang;//跟庄效果
    //private int uuid;
    private float timer;//计时变量
    private int LeavedCardsNum;//剩余牌数变量
    private int MoPaiCardPoint;//弃牌摸牌花色
    private string effectType;//吃杠牌类型等参数
    //private int gangKind;
    private bool isRuleShow;//玩法弹出框布尔值
    private int otherGangType;//杠牌类型
    //private GameObject cardOnTable;
    private int useForGangOrPengOrChi;//吃杠牌等牌id
    private int selfGangCardPoint;//自己杠的牌id
    //=========剩余电量=========//
    public Slider battery;
    public Image battery1;

    /// <summary>
    /// 庄家的索引
    /// </summary>
    private int bankerId;

    private int listIndex;

    /// <summary>
    /// 打出来的牌
    /// </summary>
    private GameObject putOutCard;

    private GameObject Pointertemp;
    private int putOutCardPoint = -1; //打出的牌
    private int putOutCardPointAvarIndex = -1; //最后一个打出牌的人的index
    private string outDir;//获取出牌的方位
    private int SelfAndOtherPutoutCard = -1;//

    /// <summary>
    /// 当前摸的牌
    /// </summary>
    private GameObject pickCardItem;

    private GameObject otherPickCardItem;

    /// <summary>
    /// 当前的方向字符串
    /// </summary>
    private string curDirString = "B";

    /// <summary>
    /// 普通胡牌算法
    /// </summary>
    private NormalHuScript norHu;

    /// <summary>
    /// 赖子胡牌算法
    /// </summary>
    private NaiziHuScript naiziHu;

    private GameToolScript gameTool;

    /*抓码动态面板*/
    private GameObject zhuamaPanel;

    /*游戏单局结束动态面板*/
    public bool chairAll;
    private int showTimeNumber;
    private int showNoticeNumber;

    private bool timeFlag;

    /*后台传过来的杠牌*/
    private string[] gangPaiList;

    /*所有的抓码数据字符串*/
    private string allMas;

    private bool isFirstOpen = true;

    /*是否为抢胡 游戏结束时需置为false*/
    private bool isQiangHu;

    /*更否申请退出房间申请*/
    private bool canClickButtonFlag;

    private string passType = "";

    //GameToolScript 里面一起改 spaceW
    private readonly float spaceW = 1f;
    private readonly float spaceH = 1.6f;
    private readonly float mjSize = 6;
    private readonly string mjPath = "MaJiangPerfab/mj";
    public List<Image> leftImage;
    public List<Image> centerImage;
    public List<Image> rightImage;
    public GameObject HeadFx;
    public Image netSign;
    public List<Sprite> wife;
    public List<Sprite> gsm;
    public Text msText;//网络延迟
    public bool isStartGame;
    public int startCardPoint;
#endregion

    private void Awake()
    {
        instance = this;
        AddListener();
        UnityPhoneManager.Instance.GetBattery();
    }
  

    IEnumerator Start()
    {
        BeginButton.transform.gameObject.SetActive(false);
        switch (GlobalDataScript.type)
        {
            case ModeType.Create:
                CreateRoomAddAvatarVO(GlobalDataScript.loginResponseData);
                break;
            case ModeType.Join:
                JoinToRoom();
                break;
        }

        GlobalDataScript.type = ModeType.None;
        GlobalDataScript.gamePlayPanel = gameObject;
        RandShowTime();
        timeFlag = true;
        norHu = new NormalHuScript();
        naiziHu = new NaiziHuScript();
        gameTool = new GameToolScript();
        btnActionScript = gameObject.GetComponent<ButtonActionScript>();
        InitPanel();
        GlobalDataScript.isonLoginPage = false;
        if (GlobalDataScript.reEnterRoomData != null)
        {
            GlobalDataScript.loginResponseData.roomId = GlobalDataScript.reEnterRoomData.roomId;
            ReEnterRoom();
        }

        GlobalDataScript.reEnterRoomData = null;
        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.PlayBGM("mjBGM");
        SoundManager.Instance.SetMusicV(PlayerPrefs.GetFloat("MusicVolume", 1));
    }

    void RandShowTime()
    {
        showTimeNumber = UnityEngine.Random.Range(5000, 10000);
    }

    void InitPanel()
    {
        Clean();
        btnActionScript.CleanBtnShow();
    }

    public void AddListener()
    {
        SocketEventHandle.Instance.startGameReply += OnStartGameReply; //----------------
        SocketEventHandle.Instance.pickCardReply += OnPickCardReply; //----------------
        SocketEventHandle.Instance.otherPickCardReply += OnOtherPickCardReply; //----------------
        SocketEventHandle.Instance.otherPutOutCardReply += OnOtherPutOutCardReply; //----------------
        SocketEventHandle.Instance.otherJoinRoomReply += OnOtherJoinRoomReply; //----------------
        SocketEventHandle.Instance.otherPentReply += OnOtherPentReply; //-------
        SocketEventHandle.Instance.otherChiReply += OnOtherChiReply; //-------
        SocketEventHandle.Instance.gangReply += OnGangReply; //----------
        SocketEventHandle.Instance.otherGangReply += OnotherGangReply; //------------
        SocketEventHandle.Instance.actionBtnReply += OnActionBtnReply;
        SocketEventHandle.Instance.huReply += OnhuReply;
        SocketEventHandle.Instance.quitRoomReply += OnQuitRoomReply;
        SocketEventHandle.Instance.readyReply += OnReadyReply;
        SocketEventHandle.Instance.offlineReply += OfflineReply;
        SocketEventHandle.Instance.messageBoxReply += OnMessageBoxReply;
        SocketEventHandle.Instance.returnGameReply += ReturnGameReply;
        SocketEventHandle.Instance.onlineReply += OnlineReply;
        CommonEvent.GetInstance.readyGameReply += MarkSelfReadyGameReply;
        CommonEvent.GetInstance.closeGamePanelReply += ExitOrDissoliveRoom;
        SocketEventHandle.Instance.micInputReply += MicInputReply;
        SocketEventHandle.Instance.followBankerReply += FollowBankerReply;
        SocketEventHandle.Instance.playerStateReply += PlayerStateReply;
        SocketEventHandle.Instance.dissolveRoomReply += OnDissolveRoomReply;
        GameMessageManager.SetBattery += SetBattery;
    }

    private void PlayerStateReply(ClientResponse response)
    {
        var state = NetUtil.JsonToObj<PlayerStateVO>(response.message);
        if (state.userId == GlobalDataScript.loginResponseData.account.uuid && state.chairState == Define.US_FREE)
        {
            ExitOrDissoliveRoom();
            SocketEngine.Instance.SocketQuit();
            MySceneManager.instance.BackToMain();
        }
    }

    private void RemoveListener()
    {
        SocketEventHandle.Instance.startGameReply -= OnStartGameReply; //------
        SocketEventHandle.Instance.pickCardReply -= OnPickCardReply;
        SocketEventHandle.Instance.otherPickCardReply -= OnOtherPickCardReply;
        SocketEventHandle.Instance.otherPutOutCardReply -= OnOtherPutOutCardReply;
        SocketEventHandle.Instance.otherJoinRoomReply -= OnOtherJoinRoomReply;
        SocketEventHandle.Instance.otherPentReply -= OnOtherPentReply;
        SocketEventHandle.Instance.otherChiReply -= OnOtherChiReply;
        SocketEventHandle.Instance.gangReply -= OnGangReply;
        SocketEventHandle.Instance.otherGangReply -= OnotherGangReply;
        SocketEventHandle.Instance.actionBtnReply -= OnActionBtnReply;
        SocketEventHandle.Instance.huReply -= OnhuReply;
        SocketEventHandle.Instance.quitRoomReply -= OnQuitRoomReply;
        SocketEventHandle.Instance.readyReply -= OnReadyReply;
        SocketEventHandle.Instance.offlineReply -= OfflineReply;
        SocketEventHandle.Instance.onlineReply -= OnlineReply;
        SocketEventHandle.Instance.messageBoxReply -= OnMessageBoxReply;
        SocketEventHandle.Instance.returnGameReply -= ReturnGameReply;
        CommonEvent.GetInstance.readyGameReply -= MarkSelfReadyGameReply;
        CommonEvent.GetInstance.closeGamePanelReply -= ExitOrDissoliveRoom;
        SocketEventHandle.Instance.micInputReply -= MicInputReply;
        SocketEventHandle.Instance.followBankerReply -= FollowBankerReply;
        SocketEventHandle.Instance.playerStateReply -= PlayerStateReply;
        SocketEventHandle.Instance.dissolveRoomReply -= OnDissolveRoomReply;
        GameMessageManager.SetBattery -= SetBattery;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    /// <param name="response">Response.</param>
    public void OnStartGameReply(ClientResponse response)
    {
        GlobalDataScript.putOutCount = 100;
        GlobalDataScript.roomAvatarVoList = avatarList;
        var sgvo = JsonMapper.ToObject<StartGameVO>(response.message);
        bankerId = sgvo.bankerId;
        CleanGameplayUI();
        ShowLeavedCardsNumForInit(136);
        // MyDebug.SocketLog("==============");
        //if (bankerId == GlobalDataScript.loginResponseData.chairID)
        //{
        //    MyDebug.SocketLog("==============");
        //    sgvo.paiArray[startCardPoint]++;
        //}
        //开始游戏后不显示
        MyDebug.Log("startGame");
        GlobalDataScript.surplusTimes--;
        if (!isFirstOpen)
        {
            btnActionScript = gameObject.GetComponent<ButtonActionScript>();
            InitPanel();
        }

        GlobalDataScript.finalGameEndVo = null;
        MyDebug.Log("bankerId:" + bankerId);
        GlobalDataScript.mainUuid = avatarList[bankerId].account.uuid;
        isFirstOpen = false;
        GlobalDataScript.isOverByPlayer = false;
        SetCurrentDir(bankerId);
        playerItems[listIndex].SetbankImgEnable(true);
        GameMessageManager.SetDirCount(listIndex);
        SetAllPlayerReadImgVisbleToFalse();
        var cr = new ClientResponse
        {
            message = NetUtil.ObjToJson(sgvo)
        };
        GameMessageManager.SceneStartReply(cr);
        StartCoroutine(SetGameReady());
    }

    IEnumerator SetGameReady()
    {

        yield return new WaitForSeconds(7);
        UpateTimeReStart();
        if (curDirString == DirectionEnum.Bottom)
        {
            GlobalDataScript.isDrag = true;
        }
        else
        {
            GlobalDataScript.isDrag = false;
        }

        GlobalDataScript.isGameReadly = true;

    }

    private void CleanGameplayUI()
    {
        canClickButtonFlag = true;
        inviteFriendButton.transform.gameObject.SetActive(false);
        for (int i = 0; i < 4; i++)
            playerItems[i].SetbankImgEnable(false);
        live1.transform.gameObject.SetActive(true);
        liujuEffectGame.SetActive(false);
    }

    public void ShowLeavedCardsNumForInit(int _num)
    {
        LeavedCardsNum = _num - 53;
        LeavedCastNumText.text = (LeavedCardsNum) + "";
    }

    public void CardsNumChange()
    {
        LeavedCardsNum--;
        if (LeavedCardsNum < 0)
        {
            LeavedCardsNum = 0;
        }

        LeavedCastNumText.text = LeavedCardsNum + "";
    }

    /// <summary>
    /// 别人摸牌通知
    /// </summary>
    /// <param name="response">Response.</param>
    public void OnOtherPickCardReply(ClientResponse response)
    {
        MyDebug.Log("OnOtherPickCard!!!!!!!!!!!" + isStartGame);
        UpateTimeReStart();
        var cvo = JsonMapper.ToObject<CardVO>(response.message);
        if (!isStartGame)
        {
            isStartGame = true;
            startCardPoint = cvo.cardPoint;
            return;
        }

        var avatarIndex = cvo.avatarIndex;
        MyDebug.Log("otherPickCard avatarIndex = " + avatarIndex);
        SetCurrentDir(avatarIndex);
        GameMessageManager.OtherMoPai();
        bottomScript.ResetShouPaiPosition();
        CardsNumChange();
    }

    private void SetCurrentDir(int avatarIndex)
    {
        curDirString = GetDirection(avatarIndex);
        GameMessageManager.SetListIndex(listIndex);
        GameMessageManager.setPlayersDir(curDirString);
        SetPlayerHeadFx();
    }

    /// <summary>
    /// 自己摸牌
    /// </summary>
    /// <param name="response">Response.</param>
    public void OnPickCardReply(ClientResponse response)
    {
        UpateTimeReStart();
        var cardvo = JsonMapper.ToObject<CardVO>(response.message);
        MyDebug.SocketLog("OnPickCardReply!!!!!!" + cardvo.cardPoint);
        MyDebug.SocketLog("isStartGame:" + isStartGame);
        if (!isStartGame)
        {
            isStartGame = true;
            startCardPoint = cardvo.cardPoint;
            GameMessageManager.SetStartCardPoint(cardvo.cardPoint);
            return;
        }

        MoPaiCardPoint = cardvo.cardPoint;
        MyDebug.Log("摸牌" + MoPaiCardPoint);
        SelfAndOtherPutoutCard = MoPaiCardPoint;
        useForGangOrPengOrChi = cardvo.cardPoint;
        curDirString = DirectionEnum.Bottom;
        listIndex = 0;
        SetPlayerHeadFx();
        GameMessageManager.SetListIndex(0);
        GameMessageManager.setPlayersDir(curDirString);
        GameMessageManager.PickCardReply(response);
        CardsNumChange();
    }

    /// <summary>
    /// 胡，杠，碰，吃，pass按钮显示.
    /// </summary>
    /// <param name="response">Response.</param>
    MahjongMotion actionTips;

    public void OnActionBtnReply(ClientResponse response)
    {
        if (GlobalDataScript.putOutCount < 3)
        {
            MyPassBtnClick();
            return;
        }

        GlobalDataScript.isDrag = false;
        passType = curDirString == DirectionEnum.Bottom ? "selfPickCard" : "otherPickCard";
        actionTips = JsonMapper.ToObject<MahjongMotion>(response.message);
        if (actionTips.isChiHuMotion)
        {
            btnActionScript.ShowBtn(1);
        }

        if (actionTips.isPengMotion)
        {
            btnActionScript.ShowBtn(3);
        }

        if (actionTips.isCenterMotion || actionTips.isLeftMotion || actionTips.isRightMotion)
        {
            btnActionScript.ShowBtn(4);
        }

        if (actionTips.isGangMotion)
        {
            selfGangCardPoint = actionTips.cardID;
            btnActionScript.ShowBtn(2);
        }
    }

    private int dirCount;

    private void SetAllPlayerReadImgVisbleToFalse()
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].readyImg.enabled = false;
        }
    }

    private void SetAllPlayerHuImgVisbleToFalse()
    {
        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].SetHuFlagHidde();
        }
    }

    private void SetPlayerHeadFx()
    {
        HeadFx.transform.position = playerItems[listIndex].transform.position;
    }

    private int GetIndexByDir(string dir)
    {
        var result = 0;
        switch (dir)
        {
            case DirectionEnum.Top: //上
                result = 2;
                break;
            case DirectionEnum.Left: //左
                result = 3;
                break;
            case DirectionEnum.Right: //右
                result = 1;
                break;
            case DirectionEnum.Bottom: //下
                result = 0;
                break;
        }

        return result;
    }

    /// <summary>
    /// 接收到其它人的出牌通知
    /// </summary>
    /// <param name="response">Response.</param>
    public void OnOtherPutOutCardReply(ClientResponse response)
    {
        MyDebug.Log("Put Out Card Reply");
        isStartGame = true;
        var sgvo = JsonMapper.ToObject<OutCardVO>(response.message);
        var cardPoint = sgvo.cardID;
        var curAvatarIndex = sgvo.chairId;
        SoundManager.Instance.PlaySound(cardPoint, 0);
        putOutCardPointAvarIndex = GetIndexByDir(GetDirection(curAvatarIndex));
        MyDebug.Log("otherPickCard avatarIndex = " + curAvatarIndex);
        useForGangOrPengOrChi = cardPoint;
        //  SetCurrentDir(sgvo.chairId);
        GameMessageManager.OtherPutOutReply(response);
    }

    /// <summary>
    /// 根据一个人在数组里的索引，得到这个人所在的方位，L-左，T-上,R-右，B-下（自己的方位永远都是在下方）
    /// </summary>
    /// <returns>The direction.</returns>
    /// <param name="chairId">Avatar index.</param>
    private String GetDirection(int chairId)
    {
        String result = DirectionEnum.Bottom;
        int myChairId = GlobalDataScript.loginResponseData.chairID;
        if (myChairId == chairId)
        {
            MyDebug.Log("getDirection == B");
            listIndex = 0;
            return result;
        }

        listIndex = myChairId - chairId;
        if (listIndex < 0)
            listIndex += 4;
        switch (listIndex)
        {
            case 1:
                return DirectionEnum.Right;
            case 2:
                return DirectionEnum.Top;
            case 3:
                return DirectionEnum.Left;
        }

        MyDebug.Log("getDirection == B");
        listIndex = 0;
        return DirectionEnum.Bottom;
    }

    public void OnOtherPentReply(ClientResponse response) //其他人碰牌
    {
        UpateTimeReStart();
        var cardVo = JsonMapper.ToObject<OtherPengGangBackVO>(response.message);
        otherPengCard = cardVo.cardPoint;
        SetCurrentDir(cardVo.chairId);
        GameMessageManager.OnGamePengReply(response);
        print("Current Diretion==========>" + curDirString);
        effectType = "peng";
        PengGangHuEffectCtrl();
        SoundManager.Instance.PlaySoundByAction("peng", avatarList[cardVo.avatarId].account.sex);
        btnActionScript.CleanBtnShow();
    }

    OtherChiBackVO chiVO;

    public void OnOtherChiReply(ClientResponse response) //吃牌
    {
        UpateTimeReStart();
        chiVO = JsonMapper.ToObject<OtherChiBackVO>(response.message);
        otherChiCard = chiVO.cardPoint;
        SetCurrentDir(chiVO.chairId);
        GameMessageManager.OnGameChiReply(response);
        print("Current Diretion==========>" + curDirString);
        effectType = "chi";
        PengGangHuEffectCtrl();
        SoundManager.Instance.PlaySoundByAction("chi", avatarList[chiVO.avatarId].account.sex);
    }

    private void PlaceWithImage(GameObject image)
    {
        switch (curDirString)
        {
            case DirectionEnum.Bottom:
            {
                image.transform.localPosition = new Vector3(16, -180, 0);
                break;
            }
            case DirectionEnum.Right:
            {
                image.transform.localPosition = new Vector3(520, 145, 0);
                break;
            }
            case DirectionEnum.Top:
            {
                image.transform.localPosition = new Vector3(16, 380, 0);
                break;
            }
            case DirectionEnum.Left:
            {
                image.transform.localPosition = new Vector3(-520, 145, 0);
                break;
            }
        }
    }

    private void PengGangHuEffectCtrl()
    {
        switch (effectType)
        {
            case "peng":
                PlaceWithImage(pengEffectGame);
                pengEffectGame.SetActive(true);
                break;
            case "gang":
                PlaceWithImage(gangEffectGame);
                gangEffectGame.SetActive(true);
                break;
            case "hu":
                PlaceWithImage(huEffectGame);
                huEffectGame.SetActive(true);
                break;
            case "liuju":
                liujuEffectGame.SetActive(true);
                break;
            case "chi":
                PlaceWithImage(chiEffectGame);
                chiEffectGame.SetActive(true);
                break;
        }

        InvokeHidePengGangHuEff();
    }

    private void InvokeHidePengGangHuEff()
    {
        Invoke("HidePengGangHuEff", 1f);
    }

    private void HidePengGangHuEff()
    {
        pengEffectGame.SetActive(false);
        gangEffectGame.SetActive(false);
        huEffectGame.SetActive(false);
        chiEffectGame.SetActive(false);
        liujuEffectGame.SetActive(false);
    }

    private void OnotherGangReply(ClientResponse response) //其他人杠牌
    {
        var gangNotice = JsonMapper.ToObject<GangNoticeVO>(response.message);
        otherGangCard = gangNotice.cardPoint;
        otherGangType = gangNotice.type;
        var tempvector3 = new Vector3(0, 0, 0);
        var tempRotation = Vector3.zero;
        SetCurrentDir(gangNotice.chairId);
        effectType = "gang";
        PengGangHuEffectCtrl();
        GameMessageManager.OnOtherGangReply(response);
        SoundManager.Instance.PlaySoundByAction("gang", avatarList[gangNotice.avatarId].account.sex);
        btnActionScript.CleanBtnShow();
    }
    void Update()
    {
        SetNetState();
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 0;
        }

        Number.text = Math.Floor(timer) + "";
        if (timeFlag)
        {
            showTimeNumber--;
            if (showTimeNumber < 0)
            {
                timeFlag = false;
                showTimeNumber = 0;
            }
        }
    }

    void MoveCompleted()
    {
        showNoticeNumber++;
        if (showNoticeNumber == GlobalDataScript.noticeMegs.Count)
        {
            showNoticeNumber = 0;
        }

        RandShowTime();
        timeFlag = true;
    }

    /// <summary>
    /// 重新开始计时
    /// </summary>
    void UpateTimeReStart()
    {
        timer = 16;
    }

    #region Button Action

    /// <summary>
    /// 点击放弃按钮
    /// </summary>
    public void MyPassBtnClick()
    {
        btnActionScript.CleanBtnShow();
        left.gameObject.SetActive(false);
        center.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        if (passType == "selfPickCard" && GlobalDataScript.putOutCount > 4)
        {
            GlobalDataScript.isDrag = true;
        }

        passType = "";
        MyDebug.Log("过过过过过过过过过过过过：WIK_CHI_HU");
        SendAction((byte) WIK.WIK_NULL);
    }

    /*
    * 胡牌请求
    */
    public void MyHuBtnClick()
    {
        btnActionScript.CleanBtnShow();
        MyDebug.Log("胡胡胡胡胡胡胡胡胡：WIK_CHI_HU");
        SendAction((byte) WIK.WIK_CHI_HU);
        //模拟胡牌操作
    }

    public void MyPengBtnClick()
    {
        btnActionScript.CleanBtnShow();
        Debug.Log(GlobalDataScript.isDrag);
        UpateTimeReStart();
        MyDebug.Log("碰碰碰碰碰碰碰：WIK_CHI_HU");
        SendAction((byte) WIK.WIK_PENG);
    }

    public void LeftButtonClick()
    {
        MyDebug.Log("吃吃吃吃吃吃吃吃：WIK_LEFT");
        SendAction((byte) WIK.WIK_LEFT);
        left.gameObject.SetActive(false);
        center.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        btnActionScript.passBtn.SetActive(false);
    }

    public void CenterButtonClick()
    {
        MyDebug.Log("吃吃吃吃吃吃吃吃：WIK_CENTER");
        SendAction((byte) WIK.WIK_CENTER);
        left.gameObject.SetActive(false);
        center.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        btnActionScript.passBtn.SetActive(false);
    }

    public void RightButtonClick()
    {
        MyDebug.Log("吃吃吃吃吃吃吃吃：WIK_RIGHT");
        SendAction((byte) WIK.WIK_RIGHT);
        left.gameObject.SetActive(false);
        center.gameObject.SetActive(false);
        right.gameObject.SetActive(false);
        btnActionScript.passBtn.SetActive(false);
    }

    public void MyChiBtnClick()
    {
        btnActionScript.CleanBtnShow();
        Debug.Log(GlobalDataScript.isDrag);
        UpateTimeReStart();
        if (actionTips.chiCount == 1)
        {
            MyDebug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!1Chi Type:" + actionTips.type);
            SendAction((byte) actionTips.type);
        }
        else if (actionTips.chiCount > 1)
        {
            btnActionScript.passBtn.SetActive(true);
            if (actionTips.isLeftMotion)
            {
                /*
                 *  图片资源有变动，待更改...
                 *
                 **/
               ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + actionTips.cardID, (sprite)=>
                {
                    leftImage[0].overrideSprite = sprite;
                });                                                                           
                ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + (actionTips.cardID + 1), (sprite) =>
                {
                    leftImage[1].overrideSprite = sprite;
                });
                ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + (actionTips.cardID + 2), (sprite) =>
                {
                    leftImage[2].overrideSprite = sprite;
                });                                              
                left.gameObject.SetActive(true);
                left.onClick.AddListener(LeftButtonClick);
            }

            if (actionTips.isCenterMotion)
            {
                /*
                *  图片资源有变动，待更改...
                *
                **/
                ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + (actionTips.cardID-1), (sprite) =>
                {
                    centerImage[0].overrideSprite = sprite;
                });
                ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + actionTips.cardID , (sprite) =>
                {
                    centerImage[1].overrideSprite = sprite;
                });
                ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + (actionTips.cardID + 1), (sprite) =>
                {
                    centerImage[2].overrideSprite = sprite;
                });
                //centerImage[0].sprite = Resources.Load("Image/2/" + (actionTips.cardID - 1), typeof(Sprite)) as Sprite;
                //centerImage[1].sprite = Resources.Load("Image/2/" + actionTips.cardID, typeof(Sprite)) as Sprite;
                //centerImage[2].sprite = Resources.Load("Image/2/" + (actionTips.cardID + 1), typeof(Sprite)) as Sprite;
                center.gameObject.SetActive(true);
                center.onClick.AddListener(CenterButtonClick);
            }

            if (actionTips.isRightMotion)
            {
                /*
                *  图片资源有变动，待更改...
                *
                **/
                ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + (actionTips.cardID -2), (sprite) =>
                {
                    rightImage[0].overrideSprite = sprite;
                });
                ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + (actionTips.cardID-1), (sprite) =>
                {
                    rightImage[1].overrideSprite = sprite;
                });
                ResourcesLoader.Load<Sprite>("MajiangAssets/Texture/UI/MaJiang/" + actionTips.cardID , (sprite) =>
                {
                    rightImage[2].overrideSprite = sprite;
                });
                //rightImage[0].sprite = Resources.Load("Image/2/" + (actionTips.cardID - 2), typeof(Sprite)) as Sprite;
                //rightImage[1].sprite = Resources.Load("Image/2/" + (actionTips.cardID - 1), typeof(Sprite)) as Sprite;
                //rightImage[2].sprite = Resources.Load("Image/2/" + actionTips.cardID, typeof(Sprite)) as Sprite;
                right.gameObject.SetActive(true);
                right.onClick.AddListener(RightButtonClick);
            }
        }
    }

    public void MyGangBtnClick()
    {
        MyDebug.Log("杠杠杠杠杠杠：WIK_CHI_HU");
        Debug.Log(GlobalDataScript.isDrag);
        SendAction((byte) WIK.WIK_GANG, selfGangCardPoint);
        btnActionScript.CleanBtnShow();
        effectType = "gang";
        PengGangHuEffectCtrl();
        return;
    }

    private void SendAction(byte code, int cardId = -1)
    {
        CMD_C_OperateCard operateCard;
        operateCard.cbOperateCode = code;
        operateCard.cbOperateCard = (byte)MaJiangHelper.MaJiangCardToChange(cardId);
        //SocketSendManager.Instance.SendData((int) GameServer.MDM_GF_GAME, (int) SUB_C.SUB_C_OPERATE_CARD,
        //    NetUtil.StructToBytes(operateCard), Marshal.SizeOf(operateCard));
    }

    #endregion

    public void OnGangReply(ClientResponse response)
    {
        UpateTimeReStart();
        curDirString = DirectionEnum.Bottom;
        listIndex = 0;
        SetPlayerHeadFx();
        GameMessageManager.SetListIndex(0);
        GameMessageManager.setPlayersDir(curDirString);
        GameMessageManager.OnGangReply(response);
    }

    /// <summary>
    /// 清理桌面
    /// </summary>
    public void Clean()
    {
        if (putOutCard != null)
        {
            Destroy(putOutCard);
        }

        if (pickCardItem != null)
        {
            Destroy(pickCardItem);
        }

        if (otherPickCardItem != null)
        {
            Destroy(otherPickCardItem);
        }
    }

    private void CleanArrayList(List<List<GameObject>> list)
    {
        if (list != null)
        {
            while (list.Count > 0)
            {
                var tempList = list[0];
                list.RemoveAt(0);
                CleanList(tempList);
            }
        }
    }

    private void CleanList(List<GameObject> tempList)
    {
        if (tempList != null)
        {
            while (tempList.Count > 0)
            {
                var temp = tempList[0];
                tempList.RemoveAt(0);
                Destroy(temp);
            }
        }
    }

    public void SetRoomRemark()
    {
        var roomvo = GlobalDataScript.roomVo;
        GlobalDataScript.totalTimes = roomvo.roundNumber;
        GlobalDataScript.surplusTimes = roomvo.roundNumber;
        var str = LocalizationManager.GetInstance.GetValue("KEY.10001") + "\n" + roomvo.roomId + "\n";
        str += LocalizationManager.GetInstance.GetValue("KEY.10020") + roomvo.roundNumber + "\n";
        roomNum.text = "房间号：\n"+ roomvo.roomId.ToString();
        firCount.text = roomvo.xiaYu + "";
        if (roomvo.roomType == 3)
        {
            str += LocalizationManager.GetInstance.GetValue("KEY.10004") + "\n";
        }
        else
        {
            if (roomvo.hong)
            {
                str += LocalizationManager.GetInstance.GetValue("KEY.10005") + "\n";
            }
            else
            {
                if (roomvo.roomType == 1)
                {
                    str += LocalizationManager.GetInstance.GetValue("KEY.10002") + "\n";
                }
                else if (roomvo.roomType == 2)
                {
                    str += LocalizationManager.GetInstance.GetValue("KEY.10003") + "\n";
                }
                else if (roomvo.roomType == 3)
                {
                    str += LocalizationManager.GetInstance.GetValue("KEY.10004") + "\n";
                }
            }

            if (roomvo.ziMo == 1)
            {
                str += LocalizationManager.GetInstance.GetValue("KEY.11016") + "\n";
            }
            else
            {
                str += LocalizationManager.GetInstance.GetValue("KEY.11017") + "\n";
            }

            if (roomvo.sevenDouble && roomvo.roomType != GameConfig.GAME_TYPE_HUASHUI)
            {
                str += LocalizationManager.GetInstance.GetValue("KEY.11018") + "\n";
            }

            if (roomvo.addWordCard)
            {
                str += LocalizationManager.GetInstance.GetValue("KEY.11019") + "\n";
            }

            if (roomvo.xiaYu > 0)
            {
                str += LocalizationManager.GetInstance.GetValue("KEY.11020") + roomvo.xiaYu + "";
            }

            if (roomvo.ma > 0)
            {
                str += LocalizationManager.GetInstance.GetValue("KEY.11021") + roomvo.ma + "";
            }
        }

        if (roomvo.magnification > 0)
        {
            str += LocalizationManager.GetInstance.GetValue("KEY.11022") + roomvo.magnification + "";
        }
    }

    private void AddAvatarVOToList(AvatarVO avatar)
    {
        if (avatarList == null)
        {
            avatarList = new List<AvatarVO>();
        }

        SetSeat(avatar);
    }

    public void CreateRoomAddAvatarVO(AvatarVO avatar)
    {
        avatar.scores = 1000;
        AddAvatarVOToList(avatar);
        avatarList = GlobalDataScript.roomAvatarVoList;
        SetRoomRemark();
        ReadyGame();
        MarkSelfReadyGameReply();
    }

    public void JoinToRoom()
    {
        avatarList = GlobalDataScript.roomAvatarVoList;
        for (int i = 0; i < avatarList.Count; i++)
        {
            SetSeat(avatarList[i]);
        }

        SetRoomRemark();
        ReadyGame();
        MarkSelfReadyGameReply();
    }

    /// <summary>
    /// 设置当前角色的座位
    /// </summary>
    /// <param name="avatar">Avatar.</param>
    private void SetSeat(AvatarVO avatar)
    {                                      
        var seatIndex = 0;
        MyDebug.Log("avatar.account.uuid:" + avatar.account.uuid + "============== avatar.chairID:" + avatar.chairID);
        MyDebug.Log(" GlobalDataScript.loginResponseData.account.uuid:" +
                    GlobalDataScript.loginResponseData.account.uuid);
        if (avatar.account.uuid == GlobalDataScript.loginResponseData.account.uuid)
        {
            playerItems[0].SetAvatarVo(avatar);
            playerItems[0].headerIcon.sprite = GlobalDataScript.Instance.weChatInformation.headIcon;
            playerItems[0].nameText.text = GlobalDataScript.Instance.weChatInformation.weChatName;
            GameMessageManager.SetDeskDir(avatar.chairID);
        }
        else
        {
            var myIndex = GlobalDataScript.loginResponseData.chairID;
            var curAvaIndex = avatar.chairID;
            seatIndex = myIndex - curAvaIndex;
            if (seatIndex < 0)
            {
                seatIndex = 4 + seatIndex;
            }

            playerItems[seatIndex].SetAvatarVo(avatar);
        }

        if (avatar.main)
        {
        }
    }

    private int GetIndex(int uuid)
    {
        if (avatarList != null)
        {
            for (int i = 0; i < avatarList.Count; i++)
            {
                if (avatarList[i].account != null)
                {
                    if (avatarList[i].account.uuid == uuid)
                    {
                        return i;
                    }
                }
            }
        }

        return 0;
    }

    public void OnOtherJoinRoomReply(ClientResponse response)
    {
        var avatar = JsonMapper.ToObject<AvatarVO>(response.message);
        AddAvatarVOToList(avatar);
    }

    public void BeginGame()
    {
        BeginButton.transform.gameObject.SetActive(false);
        //SocketSendManager.Instance.SendData((int) GameServer.MDM_GF_FRAME, (int) MDM_GF_FRAME.SUB_GF_USER_READY, null,
        //    0);
    }

    /*
	 * 胡牌请求回调
	 */
    private void OnhuReply(ClientResponse response)
    {
        GameMessageManager.OnGameHuReply(response);
        MyDebug.Log("HU Message:" + response.message);
        isStartGame = false;
        GlobalDataScript.isGameReadly = false;
        GlobalDataScript.isBeginGame = false;
        var hrv = JsonMapper.ToObject<HupaiResponseVo>(response.message);
        if (hrv.endType != 0)
        {
            SoundManager.Instance.PlaySoundByAction("hu", GlobalDataScript.loginResponseData.account.sex);
        }

        btnActionScript.CleanBtnShow();
        effectType = hrv.endType != 0 ? "hu" : "liuju";
        StartCoroutine(ShowGameOver());
        GlobalDataScript.hupaiResponseVo = hrv;
        return;
    }

    public IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(9);
        UIManager.instance.Show(UIType.UIGameOver);
    }

    /*
     * 检测某人是否胡牌
     */
    public int CheckAvarHupai()
    {
        return 0;
    }

    private void HupaiCoinChange(string scores)
    {
        var scoreList = scores.Split(new char[1] {','});
        if (scoreList != null && scoreList.Length > 0)
        {
            for (int i = 0; i < scoreList.Length - 1; i++)
            {
                var itemstr = scoreList[i];
                var uuid = int.Parse(itemstr.Split(new char[1] {':'})[0]);
                var score = int.Parse(itemstr.Split(new char[1] {':'})[1]) + 1000;
                playerItems[GetIndexByDir(GetDirection(GetIndex(uuid)))].scoreText.text = score + "";
                avatarList[GetIndex(uuid)].scores = score;
            }
        }
    }

    #region       退出房间

    public void ShowExitConfirmation()
    {
        UIManager.instance.Show(UIType.UIExitRoom);
        SoundManager.Instance.PlaySoundBGM("clickbutton");
    }

    public void OnQuitRoomReply(ClientResponse response)
    {
        MyDebug.Log("aaaaaaaaaaaaaaaaaaaaaa");
        var responseMsg = JsonMapper.ToObject<PlayerStateVO>(response.message);
        if (responseMsg.userId == GlobalDataScript.loginResponseData.account.uuid)
        {
            ExitOrDissoliveRoom();
            return;
        }

        for (int i = 0; i < playerItems.Count; i++)
        {
            playerItems[i].SetExit(responseMsg.userId);
        }

        for (int i = 0; i < avatarList.Count; i++)
        {
            if (responseMsg.userId == avatarList[i].account.uuid)
            {
                avatarList.RemoveAt(i);
            }
        }
    }

    public void OnDissolveRoomReply(ClientResponse response)
    {
        if (avatarList.Count <= 3)
            return;
        var outRoom = JsonMapper.ToObject<OutRoomResponseVo>(response.message);
        MyDebug.Log("outRoom.dwDissUserCout:" + outRoom.dwDissUserCout);
        MyDebug.Log("outRoom.dwNotAgreeUserCout :" + outRoom.dwNotAgreeUserCout);
        if (outRoom.dwDissUserCout + outRoom.dwNotAgreeUserCout > 1 || outRoom.dwDissUserCout <= 0)
            return;
        UIManager.instance.Show(UIType.UIDissSloveRoom,
            (go) =>
            {
                //go.GetComponent<UIPanel_DissloveRoom>().ExitUI(outRoom.dwDissChairID.ToList(),
                //    outRoom.dwNotAgreeChairID.ToList(), avatarList);
            });
    }

    #endregion

    public void ExitOrDissoliveRoom()
    {
        TalkDataManager.Instance.ClearTalkData();
        GlobalDataScript.loginResponseData.ResetData(); //复位房间数据
        if (GlobalDataScript.roomJoinResponseData != null)
            GlobalDataScript.roomJoinResponseData.playerList.Clear();
        if (GlobalDataScript.roomAvatarVoList != null)
            GlobalDataScript.roomAvatarVoList.Clear();
        GlobalDataScript.roomVo.roomId = 0;
        RemoveListener();
        TalkDataManager.Instance.ClearTalkData();
        UnityPhoneManager.Instance.CloseBattery();
        instance = null;
        SocketEngine.Instance.SocketQuit();
        MySceneManager.instance.BackToMain();
    }

    public void OnReadyReply(ClientResponse response)
    {
    }

    private void FollowBankerReply(ClientResponse response)
    {
        genZhuang.SetActive(true);
        Invoke("HideGenzhuang", 2f);
    }

    private void HideGenzhuang()
    {
        genZhuang.SetActive(false);
    }

    /*************************断线重连*********************************/
    private void ReEnterRoom()
    {
        if (GlobalDataScript.reEnterRoomData != null)
        {
            //显示房间基本信息
            GlobalDataScript.roomVo.addWordCard = GlobalDataScript.reEnterRoomData.addWordCard;
            GlobalDataScript.roomVo.hong = GlobalDataScript.reEnterRoomData.hong;
            GlobalDataScript.roomVo.name = GlobalDataScript.reEnterRoomData.name;
            GlobalDataScript.roomVo.roomId = GlobalDataScript.reEnterRoomData.roomId;
            GlobalDataScript.roomVo.roomType = GlobalDataScript.reEnterRoomData.roomType;
            GlobalDataScript.roomVo.roundNumber = GlobalDataScript.reEnterRoomData.roundNumber;
            GlobalDataScript.roomVo.sevenDouble = GlobalDataScript.reEnterRoomData.sevenDouble;
            GlobalDataScript.roomVo.xiaYu = GlobalDataScript.reEnterRoomData.xiaYu;
            GlobalDataScript.roomVo.ziMo = GlobalDataScript.reEnterRoomData.ziMo;
            GlobalDataScript.roomVo.magnification = GlobalDataScript.reEnterRoomData.magnification;
            GlobalDataScript.roomVo.ma = GlobalDataScript.reEnterRoomData.ma;
            SetRoomRemark();
            //设置座位
            avatarList = GlobalDataScript.reEnterRoomData.playerList;
            GlobalDataScript.roomAvatarVoList = GlobalDataScript.reEnterRoomData.playerList;
            for (int i = 0; i < avatarList.Count; i++)
            {
                SetSeat(avatarList[i]);
            }

            RecoverOtherGlobalData();
            var selfPaiArray = GlobalDataScript.reEnterRoomData.playerList[0].paiArray;
            if (selfPaiArray == null || selfPaiArray.Length == 0)
            {
                //游戏还没有开始
            }
            else
            {
                //牌局已开始
                SetAllPlayerReadImgVisbleToFalse();
                CleanGameplayUI();
                //显示打牌数据
                DisplayTableCards();
                //显示碰牌
                DisplayallGangCard(); //显示杠牌
                DisplayPengCard(); //显示碰牌
                DispalySelfhanderCard(); //显示自己的手牌
                CustomSocket.Instance.SendMsg(new CurrentStatusRequest());
            }
        }
    }

    //恢复其他全局数据
    private void RecoverOtherGlobalData()
    {
        GlobalDataScript.loginResponseData.account.roomcard =
            GlobalDataScript.reEnterRoomData.playerList[0].account.roomcard; //恢复房卡数据，此时主界面还没有load所以无需操作界面显示
    }

    private void DispalySelfhanderCard()
    {
    }

    private List<List<int>> ToList(int[][] param)
    {
        var returnData = new List<List<int>>();
        for (int i = 0; i < param.Length; i++)
        {
            var temp = new List<int>();
            for (int j = 0; j < param[i].Length; j++)
            {
                temp.Add(param[i][j]);
            }

            returnData.Add(temp);
        }

        return returnData;
    }

    public void MyselfSoundActionPlay()
    {
        playerItems[0].ShowChatAction();
    }

    /*显示打牌数据在桌面*/
    private void DisplayTableCards()
    {
        for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++)
        {
            var chupai = GlobalDataScript.reEnterRoomData.playerList[i].chupais;
            outDir = GetDirection(GetIndex(GlobalDataScript.reEnterRoomData.playerList[i].account.uuid));
            if (chupai != null && chupai.Length > 0)
            {
                for (int j = 0; j < chupai.Length; j++)
                {
                }
            }
        }
    }

    /*显示杠牌*/
    private void DisplayallGangCard()
    {
    }

    private void DisplayPengCard()
    {
        for (int i = 0; i < GlobalDataScript.reEnterRoomData.playerList.Count; i++)
        {
            var paiArrayType = GlobalDataScript.reEnterRoomData.playerList[i].paiArray[1];
            var dirstr = GetDirection(GetIndex(GlobalDataScript.reEnterRoomData.playerList[i].account.uuid));
            if (paiArrayType.Contains(1))
            {
                for (int j = 0; j < paiArrayType.Length; j++)
                {
                    if (paiArrayType[j] == 1 && GlobalDataScript.reEnterRoomData.playerList[i].paiArray[0][j] > 0)
                    {
                        GlobalDataScript.reEnterRoomData.playerList[i].paiArray[0][j] -= 3;
                        DoDisplayPengGangCard(dirstr, j, 3, 2);
                    }
                }
            }
        }
    }

    /*
	 * 显示杠碰牌
	 * cloneCount 代表clone的次数  若为3则表示碰   若为4则表示杠
	 */
    private void DoDisplayPengGangCard(string dirstr, int point, int cloneCount, int flag)
    {
        List<GameObject> gangTempList;
        switch (dirstr)
        {
            case DirectionEnum.Bottom:
                gangTempList = new List<GameObject>();
                for (int i = 0; i < cloneCount; i++)
                {
                    if (i < 3)
                    {
                        if (flag != 1)
                        {
                        }
                        else
                        {
                        }
                    }
                }

                break;
            case DirectionEnum.Top:
                gangTempList = new List<GameObject>();
                for (int i = 0; i < cloneCount; i++)
                {
                    if (i < 3)
                    {
                    }
                    else
                    {
                    }
                }

                break;
            case DirectionEnum.Left:
                break;
            case DirectionEnum.Right:
                break;
        }
    }

    public void InviteFriend()
    {
        UnityPhoneManager.Instance.ShareSessionText(roomNum.text);
    }

    /*用户离线回调*/
    public void OfflineReply(ClientResponse response)
    {
        var uuid = int.Parse(response.message);
        var index = GetIndex(uuid);
        var dirstr = GetDirection(index);
        switch (dirstr)
        {
            case DirectionEnum.Bottom:
                playerItems[0].GetComponent<PlayerItemScript>().SetPlayerOffline();
                break;
            case DirectionEnum.Right:
                playerItems[1].GetComponent<PlayerItemScript>().SetPlayerOffline();
                break;
            case DirectionEnum.Top:
                playerItems[2].GetComponent<PlayerItemScript>().SetPlayerOffline();
                break;
            case DirectionEnum.Left:
                playerItems[3].GetComponent<PlayerItemScript>().SetPlayerOffline();
                break;
        }

        //申请解散房间过程中，有人掉线，直接不能解散房间
        if (GlobalDataScript.isonApplayExitRoomstatus)
        {
            TipsManagerScript.getInstance.setTips(string.Format(LocalizationManager.GetInstance.GetValue("20008")));
        }
    }

    /*用户上线提醒*/
    public void OnlineReply(ClientResponse response)
    {
        var uuid = int.Parse(response.message);
        var index = GetIndex(uuid);
        var dirstr = GetDirection(index);
        switch (dirstr)
        {
            case DirectionEnum.Bottom:
                playerItems[0].GetComponent<PlayerItemScript>().SetPlayerOnline();
                break;
            case DirectionEnum.Right:
                playerItems[1].GetComponent<PlayerItemScript>().SetPlayerOnline();
                break;
            case DirectionEnum.Top:
                playerItems[2].GetComponent<PlayerItemScript>().SetPlayerOnline();
                break;
            case DirectionEnum.Left:
                playerItems[3].GetComponent<PlayerItemScript>().SetPlayerOnline();
                break;
        }
    }

    public void OnMessageBoxReply(ClientResponse response)
    {
        var chitchat = JsonMapper.ToObject<Chitchat>(response.message);
        var chaidId = chitchat.userid;
        var chatText = NetUtil.BytesToString(chitchat.chatText);
        var chair = GetIndexByDir(GetDirection(chaidId));
        playerItems[chair].ShowChat(chatText);
    }

    /*显示自己准备*/
    private void MarkSelfReadyGameReply()
    {
        playerItems[0].readyImg.transform.gameObject.SetActive(true);
    }

    /*
    *准备游戏
	*/
    public void ReadyGame()
    {
    }

    public void MicInputReply(ClientResponse response)
    {
        var sendUUid = int.Parse(response.message);
        if (sendUUid > 0)
        {
            for (int i = 0; i < playerItems.Count; i++)
            {
                if (playerItems[i].GetUuid() != -1)
                {
                    if (sendUUid == playerItems[i].GetUuid())
                    {
                        playerItems[i].ShowChatAction();
                    }
                }
            }
        }
    }

    public void ReturnGameReply(ClientResponse response)
    {
        var returnstr = response.message;
        //JsonData returnJsonData = new JsonData (returnstr);
        //1.显示剩余牌的张数和圈数
        var returnJsonData = JsonMapper.ToObject(response.message);
        var surplusCards = returnJsonData["surplusCards"].ToString();
        LeavedCastNumText.text = surplusCards;
        LeavedCardsNum = int.Parse(surplusCards);
        var gameRound = int.Parse(returnJsonData["gameRound"].ToString());
        GlobalDataScript.surplusTimes = gameRound;
        var curAvatarIndexTemp = -1; //当前出牌人的索引
        var pickAvatarIndexTemp = -1; //当前摸牌人的索引
        var putOffCardPointTemp = -1; //当前打得点数
        var currentCardPointTemp = -1; //当前摸的点数
        //不是自己摸牌
        try
        {
            curAvatarIndexTemp = int.Parse(returnJsonData["curAvatarIndex"].ToString()); //当前打牌人的索引
            putOffCardPointTemp = int.Parse(returnJsonData["putOffCardPoint"].ToString()); //当前打得点数
            putOutCardPointAvarIndex = GetIndexByDir(GetDirection(curAvatarIndexTemp));
            putOutCardPoint = putOffCardPointTemp; //碰
            SelfAndOtherPutoutCard = putOutCardPoint;
            pickAvatarIndexTemp = int.Parse(returnJsonData["pickAvatarIndex"].ToString()); //当前摸牌牌人的索引
            /*这句代码有可能引发catch  所以后面的 SelfAndOtherPutoutCard = currentCardPointTemp; 可能不执行*/
            currentCardPointTemp = int.Parse(returnJsonData["currentCardPoint"].ToString()); //当前摸得的点数
            SelfAndOtherPutoutCard = currentCardPointTemp;
        }
        catch (Exception)
        {
            throw;
        }

        if (pickAvatarIndexTemp == 0)
        {
            //自己摸牌
            if (currentCardPointTemp == -2)
            {
            }
            else
            {
            }
        }
        else
        {
            //别人摸牌
            curDirString = GetDirection(pickAvatarIndexTemp);
        }
    }

    int netLv;

    private void SetNetState()
    {
        Color32 color;
        msText.text = SocketInGameEvent.Instance.netMs.ToString();
        if (SocketInGameEvent.Instance.netMs > 0 && SocketInGameEvent.Instance.netMs <= 100)
        {
            color = new Color32(0, 255, 55, 255);
            msText.color = color;
            netSign.color = color;
        }
        else if (SocketInGameEvent.Instance.netMs < 256)
        {
            color = new Color32(209, 255, 0, 255);
            msText.color = color;
            netSign.color = color;
        }
        else
        {
            color = new Color32(255, 0, 90, 255);
            msText.color = color;
            netSign.color = color;
        }

        if (SocketInGameEvent.Instance.netMs < 0)
            msText.text = 1.ToString();

        netLv = 0;
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            netSign.sprite = wife[netLv];
            return;
        } //当用户使用移动网络时

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            netSign.sprite = gsm[netLv];
        }
    }

    public void PutDownBtn()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        putDown.SetActive(false);
        btnGroup.SetActive(true);
        packUp.SetActive(true);
    }

    public void PackUpBtn()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        packUp.SetActive(false);
        btnGroup.SetActive(false);
        putDown.SetActive(true);
    }

    public void RuleButton()
    {
        if (isRuleShow)
        {
            rule.SetActive(false);
            isRuleShow = false;
        }
        else
        {
            rule.SetActive(true);
            isRuleShow = true;
        }
    }

    public void SetBattery(int num)
    {
        Color32 color;
        battery.value = num * 0.01f;
        if (battery.value > 0.65f)
        {
            color = new Color32(0, 154, 20, 255);
            battery.GetComponent<Image>().color = color;
            battery1.GetComponent<Image>().color = color;
        }
        else if (battery.value > 0.25f)
        {
            color = new Color32(255, 100, 0, 255);
            battery.GetComponent<Image>().color = color;
            battery1.GetComponent<Image>().color = color;
        }
        else
        {
            color = new Color32(137, 0, 0, 255);
            battery.GetComponent<Image>().color = color;
            battery1.GetComponent<Image>().color = color;
        }
    }
}