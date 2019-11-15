using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.Runtime.InteropServices;
using System.IO;

public class MaJiangGameCtl : MonoBehaviour
{
    #region 定义变量
    [SerializeField] private Material glassM;
    //============投子器===============//
    [SerializeField] private Animation holeOne;
    [SerializeField] private Animation holeTwo;
    //============桌子方向材质球=========//
    [SerializeField] private Material lightDir;
    [SerializeField] private Material darkDir;
    public TextMesh LeavedRoundNumText; //剩余局数
    //============麻将摆放位置参数==============//
    private readonly float spaceW = 1f;
    private readonly float spaceH = 0.68f;
    private readonly float spaceL = 1.32f;
    /// <summary>
    /// 玩家位置计数，便与确认操作的玩家
    /// </summary>
    private int dirCount;
    //=============碰杠牌摆放父类=============//
    [SerializeField] private Transform pengGangParenTransformB;
    [SerializeField] private Transform pengGangParenTransformL;
    [SerializeField] private Transform pengGangParenTransformR;
    [SerializeField] private Transform pengGangParenTransformT;
    [SerializeField] private GameObject tableAnima;//桌子麻将槽
    //=============弃牌子类、父类====================//
    [SerializeField] private GameObject publicB;
    [SerializeField] private GameObject publicL;
    [SerializeField] private GameObject publicT;
    [SerializeField] private GameObject publicR;
    [SerializeField] private Transform publicCards;
    [SerializeField] private Transform dirPartent;//活跃用户父类
    private int selfGangCardPoint;//自己杠牌的花色
    private int otherGangCard;//其他玩家杠牌花色
    private int otherGangType;//杠牌类型
    private int MoPaiCardPoint;//摸牌id
    private int useForGangOrPengOrChi;//碰杠吃牌id
    private List<GameObject> publicCardList = new List<GameObject>();//牌堆集合
    private List<GameObject> lastCardList = new List<GameObject>();//最后一张牌集合
    private int holeNum1;

    // [SerializeField]
    private int holeNum2;
    public ThrowCtl PengGangList_B; //碰杠牌组
    public ThrowCtl PengGangList_L;
    public ThrowCtl PengGangList_T;
    public ThrowCtl PengGangList_R;
    public static MaJiangGameCtl instance;

    /// <summary>
    /// 当前的方向字符串
    /// </summary>
    private string curDirString = "B";

    private int curListIndex;
    private GameObject otherPickCardItem;
    private GameObject pickCardItem;
    [SerializeField] private List<GameObject> dirLight;
    private int bankerId;
    private List<int> mineList;
    [SerializeField] private List<Transform> parentList;
    [SerializeField] public List<Transform> outparentList;

    /// <summary>
    /// 手牌数组，0自己，1-右边。2-上边。3-左边
    /// </summary>
    [SerializeField] private List<List<GameObject>> handerCardList;

    private int SelfAndOtherPutoutCard = -1;

    private int putOutCardPoint = -1; //打出的牌

    //最后一次打出来的牌
    public GameObject cardOnTable;
    private int myCardsCount;
    [SerializeField] private GameObject Pointertemp;
    private int putOutCardPointAvarIndex = -1; //最后一个打出牌的人的index
    bool isGang;

    /// <summary>
    /// 打在桌子上的牌
    /// </summary>
    [SerializeField] public List<List<GameObject>> tableCardList;

    private int gangKind;
    private int gangCount;
    bool isShowGlass;
    float glassA = 1;
    [SerializeField] private GameObject lightForDir;
    [SerializeField] private List<HandAnima> rightHand;
    [SerializeField] private List<HandAnima> leftHand;
    public GameObject girlleft;
    public GameObject girlright;
    public GameObject boyleft;
    public GameObject boyright;

    #endregion

    private void Awake()
    {
        instance = this;
        AddListener();
        InitArrayList();
    }

    private void OnDestroy()
    {
        RemoveListener();
    }

    void Start()
    {
        SetLightAnima();
        InitTable();
    }

    void Update()
    {
        if (isShowGlass)
        {
            if (glassA < 1)
            {
                glassA += Time.deltaTime * 0.5f;
                glassM.color = new Color(1, 1, 1, glassA);
            }
        }
        else
        {
            if (glassA != 0)
            {
                glassA = 0;
                glassM.color = new Color(1, 1, 1, 0);
            }
        }
    }

    /// <summary>
    /// 添加监听事件
    /// </summary>
    private void AddListener()
    {
        GameMessageManager.SetDirCount += SetDirCount;
        GameMessageManager.SceneStartReply += OnStartGameReply;
        GameMessageManager.SetListIndex += SetListIndex;
        GameMessageManager.setPlayersDir += SetPlayersDir;
        GameMessageManager.PickCardReply += OnPickCardReply;
        GameMessageManager.OtherMoPai += OnOtherPickCardReply;
        GameMessageManager.OtherPutOutReply += OnOtherPutOutCardReply;
        GameMessageManager.OnGamePengReply += OnOtherPentReply;
        GameMessageManager.OnGameHuReply += OnHuReply;
        GameMessageManager.OnGameChiReply += OnChiReply;
        GameMessageManager.OnGangReply += OnGangReply;
        GameMessageManager.OnOtherGangReply += OnotherGangReply;
        GameMessageManager.SetMyGangPoint += SetMyGangPoint;
        GameMessageManager.SetDeskDir += SetDeskDir;
        GameMessageManager.SetStartCardPoint += SetStartCardPoint;
    }

    private int DeskDir;

    private void SetDeskDir(int _num)
    {
        MyDebug.Log("DeskDir:" + _num);
        DeskDir = _num;
        dirPartent.transform.localEulerAngles = new Vector3(0, (1 - DeskDir) * 90, 0);
        MyDebug.Log(" dirPartent.transform.localEulerAngles:" + dirPartent.transform.localEulerAngles);
    }

    private void SetLightAnima()
    {
        var argsLight = new Hashtable
        {
            {"easeType", iTween.EaseType.easeInOutCirc},
            {"speed", 20},
            {"loopType", "pingPong"},
            {"color", new Color(1, 1, 1)}
        };
        iTween.ColorTo(lightForDir, argsLight);
    }

    /// <summary>
    /// 移除监听事件
    /// </summary>
    private void RemoveListener()
    {
        GameMessageManager.SetDirCount -= SetDirCount;
        GameMessageManager.SceneStartReply -= OnStartGameReply;
        GameMessageManager.SetListIndex -= SetListIndex;
        GameMessageManager.setPlayersDir -= SetPlayersDir;
        GameMessageManager.PickCardReply -= OnPickCardReply;
        GameMessageManager.OtherMoPai -= OnOtherPickCardReply;
        GameMessageManager.OtherPutOutReply -= OnOtherPutOutCardReply;
        GameMessageManager.OnGamePengReply -= OnOtherPentReply;
        GameMessageManager.OnGameHuReply -= OnHuReply;
        GameMessageManager.OnGameChiReply -= OnChiReply;
        GameMessageManager.OnGangReply -= OnGangReply;
        GameMessageManager.OnOtherGangReply -= OnotherGangReply;
        GameMessageManager.SetMyGangPoint -= SetMyGangPoint;
        GameMessageManager.SetDeskDir -= SetDeskDir;
        GameMessageManager.SetStartCardPoint -= SetStartCardPoint;

    }

    private void SetStartCardPoint(int _num)
    {
        MoPaiCardPoint = _num;
    }

    public void CreateHand()
    {
        for (int i = 0; i < 4; i++)
        {
            var go = Instantiate(girlright) as GameObject;
            go.transform.eulerAngles = new Vector3(0, 90 * i, 0);
            rightHand.Add(go.GetComponent<HandAnima>());
        }

        rightHand[0].endPos = new Vector3(0, 4.95f, -15f);
        rightHand[1].endPos = new Vector3(-20, 2.46f, 0);
        rightHand[2].endPos = new Vector3(0, 2.46f, 20);
        rightHand[3].endPos = new Vector3(20, 2.16f, 0);
        rightHand[1].dir = 3;
        rightHand[2].dir = 2;
        rightHand[3].dir = 1;
        for (int i = 0; i < rightHand.Count; i++)
        {
            rightHand[i].transform.position = rightHand[i].endPos;
            rightHand[i].endRotation = new Vector3(0, i * 90, 0);
        }

        for (int i = 0; i < 3; i++)
        {
            var go = Instantiate(girlleft) as GameObject;
            go.transform.eulerAngles = new Vector3(0, 90 * (i + 1), 0);
            leftHand.Add(go.GetComponent<HandAnima>());
        }

        leftHand[0].startPos = new Vector3(13.32f, 2.46f, -0.84f);
        leftHand[1].startPos = new Vector3(-0.17f, 2.46f, -13.48f);
        leftHand[2].startPos = new Vector3(-13.4f, 2.16f, 0.2f);
        leftHand[0].endPos = new Vector3(-17, 2.46f, -0.84f);
        leftHand[1].endPos = new Vector3(-0.17f, 2.46f, 13f);
        leftHand[2].endPos = new Vector3(17f, 2.16f, 0.2f);
        for (int i = 0; i < leftHand.Count; i++)
        {
            leftHand[i].transform.position = leftHand[i].endPos;
            leftHand[i].endRotation = new Vector3(0, (i + 1) * 90, 0);
        }
    }

    /// <summary>
    /// 接收开始消息
    /// </summary>
    /// <param name="response"></param>
    private void OnStartGameReply(ClientResponse response)
    {
        NumberOfGame();
        //LeavedRoundNumText.text = "di"+GlobalDataScript.roomVo.dwPlayCout+"/" + GlobalDataScript.roomVo.dwPlayTotal + "ju";
        CreateHand();
        InitTable();
        var r = new System.Random();
        holeNum1 = r.Next(1, 7);
        holeNum2 = r.Next(1, 7);
        var sgvo = JsonMapper.ToObject<StartGameVO>(response.message);
        bankerId = sgvo.bankerId;
        InitPublicCardList(136);
        mineList = sgvo.paiArray;
        SetDirAction();
        StartCoroutine(PlayDeskAnima());

    }

    private void SetHandleLog()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        var mes = "";
        var builder = new System.Text.StringBuilder();
        builder.Append(mes);
        for (int i = 0; i < handerCardList[0].Count; i++)
        {
            var temp = handerCardList[0][i].GetComponent<BottomScript>().GetPoint();
            builder.Append((temp + "__"));
        }

        mes = builder.ToString();
        StreamWriter sw;
        if (!File.Exists(Application.dataPath + "//" + GlobalDataScript.roomVo.roomId + "_" +
                         GlobalDataScript.loginResponseData.chairID + ".txt"))
        {
            sw = File.CreateText(Application.dataPath + "//" + GlobalDataScript.roomVo.roomId + "_" +
                                 GlobalDataScript.loginResponseData.chairID + ".txt"); //创建一个用于写入 UTF-8 编码的文本
            Debug.Log("文件创建成功！");
        }
        else
        {
            sw = File.AppendText(Application.dataPath + "//" + GlobalDataScript.roomVo.roomId + "_" +
                                 GlobalDataScript.loginResponseData.chairID + ".txt"); //打开现有 UTF-8 编码文本文件以进行读取
        }

        sw.WriteLine(mes); //以行为单位写入字符串
        sw.Close();
        sw.Dispose(); //文件流释
#endif
    }

    /// <summary>
    /// 自己摸牌消息
    /// </summary>
    /// <param name="response"></param>
    private void OnPickCardReply(ClientResponse response)
    {
        var cardvo = JsonMapper.ToObject<CardVO>(response.message);
        MoPaiCardPoint = cardvo.cardPoint;
        MyDebug.Log("摸牌" + MoPaiCardPoint);
        useForGangOrPengOrChi = cardvo.cardPoint;
        if (isGang)
            Invoke("GangMoPai", 0.6f);
        else
            MoPai();
        SetDirAction();
        if (GlobalDataScript.putOutCount >= 3)
            GlobalDataScript.isDrag = true;
        Debug.Log(GlobalDataScript.putOutCount + "==" + GlobalDataScript.isDrag);
    }

    /// <summary>
    /// 杠的时候摸牌
    /// </summary>
    private void GangMoPai()
    {
        isGang = false;
        pickCardItem =
            GameResourceManager.Instance.CreateGameObjectAndReturn(MoPaiCardPoint, transform, Vector3.zero,
                Vector3.zero); //实例化当前摸的牌
        MyDebug.Log("摸牌 === >> " + MoPaiCardPoint);
        if (pickCardItem != null) //有可能没牌了
        {
            pickCardItem.name = "pickCardItem";
            pickCardItem.transform.SetParent(parentList[0]); //父节点
            pickCardItem.transform.localEulerAngles = new Vector3(-40, 270, 90);
            pickCardItem.transform.localPosition =
                new Vector3((myCardsCount / 2.0f - PengGangList_B.Count) * spaceW, 2, 0);
            pickCardItem.GetComponent<BottomScript>().OnSendMessage += CardChange; //发送消息
            pickCardItem.GetComponent<BottomScript>().ReSetPoisiton += CardSelect;
            pickCardItem.GetComponent<BottomScript>().InitMyPoint(MoPaiCardPoint); //得到索引
            PickCardAnima(pickCardItem, new Vector3((myCardsCount / 2.0f - PengGangList_B.Count) * spaceW + 550, 0, 0),
                new Vector3(-90, 270, 90));
            CheckGangRemove();
            gangCount++;
            mineList[MoPaiCardPoint]++;
            InsertCardIntoList(pickCardItem);
        }

        MyDebug.Log("moPai  goblist count === >> " + handerCardList[0].Count);
    }

    /// <summary>
    /// 其他人摸牌消息
    /// </summary>
    private void OnOtherPickCardReply()
    {
        SetDirAction();
        StartCoroutine(OtherPickCardAndCreate());
    }

    /// <summary>
    /// 吃牌消息
    /// </summary>
    /// <param name="response"></param>
    private void OnChiReply(ClientResponse response) //吃牌
    {
        var cardVo = JsonMapper.ToObject<OtherChiBackVO>(response.message);
        SetDirAction();
        //删除桌面上的麻将
        if (cardOnTable != null)
        {
            ReSetOutOnTabelCardPosition(cardOnTable);
            Destroy(cardOnTable);
        }

        if (curDirString == DirectionEnum.Bottom) //
        {
            var removeCount = 0;
            var removeCount2 = 0;
            var card1 = 0;
            var card2 = 0;
            if (cardVo.type == (int) WIK.WIK_CENTER)
            {
                card1 = putOutCardPoint - 1;
                card2 = putOutCardPoint + 1;
                BottomChi(card1, putOutCardPoint, card2);
            }
            else if (cardVo.type == (int) WIK.WIK_LEFT)
            {
                card1 = putOutCardPoint + 1;
                card2 = putOutCardPoint + 2;
                BottomChi(putOutCardPoint, card1, card2);
            }
            else if (cardVo.type == (int) WIK.WIK_RIGHT)
            {
                card1 = putOutCardPoint - 2;
                card2 = putOutCardPoint - 1;
                BottomChi(card1, card2, putOutCardPoint);
            }

            for (int i = 0; i < handerCardList[0].Count; i++)
            {
                var temp = handerCardList[0][i];
                var tempCardPoint = temp.GetComponent<BottomScript>().GetPoint();
                if (tempCardPoint == card1 && removeCount == 0)
                {
                    handerCardList[0].RemoveAt(i);
                    mineList[card1]--;
                    Destroy(temp);
                    i--;
                    removeCount++;
                }

                if (tempCardPoint == card2 && removeCount2 == 0)
                {
                    handerCardList[0].RemoveAt(i);
                    mineList[card2]--;
                    Destroy(temp);
                    i--;
                    removeCount2++;
                }
            }

            RefreshCardPos(true);
        }
        else //其他人吃牌
        {
            var tempCardList = handerCardList[curListIndex];
            if (tempCardList != null)
            {
                MyDebug.Log("tempCardList.count======前" + tempCardList.Count);
                //消除其他的人牌吃牌长度
                for (int i = 0; i < 2; i++)
                {
                    var temp = tempCardList[0];
                    Destroy(temp);
                    tempCardList.RemoveAt(0);
                }

                MyDebug.Log("tempCardList.count======前" + tempCardList.Count);
                //排序
                RefreshCardPos(true);
            }

            GameObject obj = null;
            var throwItem = new GameObject();
            var item = throwItem.AddComponent<ThrowItem>();
            var listObj = new List<GameObject>();
            var card1 = 0;
            var card2 = 0;
            var card3 = 0;
            if (cardVo.type == (int) WIK.WIK_CENTER)
            {
                card1 = cardVo.cardPoint - 1;
                card2 = cardVo.cardPoint;
                card3 = cardVo.cardPoint + 1;
            }
            else if (cardVo.type == (int) WIK.WIK_LEFT)
            {
                card1 = cardVo.cardPoint;
                card2 = cardVo.cardPoint + 1;
                card3 = cardVo.cardPoint + 2;
            }
            else if (cardVo.type == (int) WIK.WIK_RIGHT)
            {
                card1 = cardVo.cardPoint - 2;
                card2 = cardVo.cardPoint - 1;
                card3 = cardVo.cardPoint;
            }

            GameObject go;
            switch (curDirString)
            {
                case DirectionEnum.Right:
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card3, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card3);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card2, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card2);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card1, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card1);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    if (putOutCardPoint == card1)
                    {
                        item.TestPosition(listObj, 1, 0);
                    }
                    else if (putOutCardPoint == card2)
                    {
                        item.TestPosition(listObj, 1, 3);
                    }
                    else if (putOutCardPoint == card3)
                    {
                        item.TestPosition(listObj, 1, 2);
                    }

                    PengGangList_R.AddItem(item);
                    item.transform.position -= new Vector3(0, 0, 2);
                    rightHand[3].startPos = item.transform.position + new Vector3(-8.5f, -3.45f, -1.9f);
                    StartCoroutine(rightHand[3].SetMove());
                    iTween.MoveBy(item.gameObject,
                        iTween.Hash("z", 2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                            0.3f));
                    break;
                case DirectionEnum.Top:
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card3, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card3);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card2, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card2);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card1, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card1);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    if (putOutCardPoint == card1)
                    {
                        item.TestPosition(listObj, 2, 1);
                    }
                    else if (putOutCardPoint == card2)
                    {
                        item.TestPosition(listObj, 2, 0);
                    }
                    else if (putOutCardPoint == card3)
                    {
                        item.TestPosition(listObj, 2, 3);
                    }

                    PengGangList_T.AddItem(item);
                    item.transform.position += new Vector3(2, 0, 0);
                    rightHand[2].startPos = item.transform.position + new Vector3(1.04f, -3.72f, -7.99f);
                    StartCoroutine(rightHand[2].SetMove());
                    iTween.MoveBy(item.gameObject,
                        iTween.Hash("x", -2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                            0.3f));
                    break;
                case DirectionEnum.Left:
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card3, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card3);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card2, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card2);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    obj = GameResourceManager.Instance.CreateGameObjectAndReturn(card1, throwItem.transform,
                        Vector3.zero, Vector3.zero);
                    obj.GetComponent<BottomScript>().SetDeskPointInfo(card1);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj);
                    if (putOutCardPoint == card1)
                    {
                        item.TestPosition(listObj, 3, 2);
                    }
                    else if (putOutCardPoint == card2)
                    {
                        item.TestPosition(listObj, 3, 1);
                    }
                    else if (putOutCardPoint == card3)
                    {
                        item.TestPosition(listObj, 3, 0);
                    }

                    PengGangList_L.AddItem(item);
                    rightHand[1].startPos = item.transform.position + new Vector3(8.67f, -3.65f, 1.9f);
                    StartCoroutine(rightHand[1].SetMove());
                    item.transform.position += new Vector3(0, 0, 2);
                    iTween.MoveBy(item.gameObject,
                        iTween.Hash("z", -2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                            0.3f));
                    break;
            }
        }
    }

    /// <summary>
    /// 胡牌消息
    /// </summary>
    /// <param name="response"></param>
    private void OnHuReply(ClientResponse response)
    {
        StartCoroutine(Hu(response));
    }

    IEnumerator Hu(ClientResponse response)
    {
        for (int i = 0; i < leftHand.Count; i++)
        {
            leftHand[i].gameObject.SetActive(true);
        }

        GameObject go;
        var mesh = new Mesh();
        var hupaiResponseVo = JsonMapper.ToObject<HupaiResponseVo>(response.message);
        SetDirAction();
        var index = 0;
        int hu = 9;
        for (int i = 0; i < hupaiResponseVo.avatarList.Count; i++)
        {
            if (hupaiResponseVo.avatarList[i].win)
            {
                index = hupaiResponseVo.avatarList[i].huInfo.card;
                hu = i;
                break;
            }
        }

        if (hu != 9)
        {
            switch (GetDirection(hu))
            {
                case DirectionEnum.Bottom:
                    handerCardList[0].Remove(pickCardItem);
                    Destroy(pickCardItem);
                    var obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(index,
                        pengGangParenTransformB.transform, new Vector3(-0.3f, 0, 1.7f), Vector3.zero);
                    var gameObject = obj1.GetComponentInChildren<MeshRenderer>().gameObject;
                    gameObject.SetActive(false);
                    GameResourceManager.Instance.PlayHuFx(obj1.transform);
                    yield return new WaitForSeconds(1f);
                    gameObject.SetActive(true);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj1.transform;
                    go.transform.localPosition = new Vector3(0.01f, -0.27f, -0.196f);
                    obj1.GetComponent<BottomScript>().SetDeskPointInfo(index);
                    break;
                case DirectionEnum.Right:
                    handerCardList[1].Remove(otherPickCardItem);
                    Destroy(otherPickCardItem);
                    obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(index,
                        pengGangParenTransformR.transform, new Vector3(-1.47f, 0, -2.32f), new Vector3(0, -90, 0));
                    gameObject = obj1.GetComponentInChildren<MeshRenderer>().gameObject;
                    gameObject.SetActive(false);
                    GameResourceManager.Instance.PlayHuFx(obj1.transform);
                    yield return new WaitForSeconds(1f);
                    gameObject.SetActive(true);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj1.transform;
                    go.transform.localPosition = new Vector3(-0.217f, -0.288f, 0.02f);
                    go.transform.localEulerAngles = new Vector3(90, 90, -90);
                    obj1.GetComponent<BottomScript>().SetDeskPointInfo(index);
                    break;
                case DirectionEnum.Top:
                    handerCardList[2].Remove(otherPickCardItem);
                    Destroy(otherPickCardItem);
                    obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(index,
                        pengGangParenTransformT.transform, new Vector3(3.6f, 0, -1.4f), new Vector3(0, -180, 0));
                    gameObject = obj1.GetComponentInChildren<MeshRenderer>().gameObject;
                    gameObject.SetActive(false);
                    GameResourceManager.Instance.PlayHuFx(obj1.transform);
                    yield return new WaitForSeconds(1f);
                    gameObject.SetActive(true);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj1.transform;
                    go.transform.localPosition = new Vector3(0.03f, -0.28f, 0.15f);
                    obj1.GetComponent<BottomScript>().SetDeskPointInfo(index);
                    break;
                case DirectionEnum.Left:
                    handerCardList[3].Remove(otherPickCardItem);
                    Destroy(otherPickCardItem);
                    obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(index,
                        pengGangParenTransformL.transform, new Vector3(3.6f, 0, 0.3f), new Vector3(0, 90, 0));
                    gameObject = obj1.GetComponentInChildren<MeshRenderer>().gameObject;
                    gameObject.SetActive(false);
                    GameResourceManager.Instance.PlayHuFx(obj1.transform);
                    yield return new WaitForSeconds(1f);
                    gameObject.SetActive(true);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj1.transform;
                    go.transform.localPosition = new Vector3(0.152f, -0.28f, 0.01f);
                    go.transform.localEulerAngles = new Vector3(90, 90, -90);
                    obj1.GetComponent<BottomScript>().SetDeskPointInfo(index);
                    break;
            }
        }

        for (int i = 0; i < hupaiResponseVo.avatarList.Count; i++)
        {
            MyDebug.Log(i);
            switch (GetDirection(hupaiResponseVo.avatarList[i].chairId))
            {
                case DirectionEnum.Right:
                    Debug.Log(hupaiResponseVo.avatarList[i].paiArray.Count);
                    Debug.Log(handerCardList[1].Count);
                    for (int j = 0; j < hupaiResponseVo.avatarList[i].paiArray.Count; j++)
                    {
                        Debug.Log(hupaiResponseVo.avatarList[i].paiArray[j]);
                        var game = GameResourceManager.Instance.CreateGameObjectAndReturn(
                            hupaiResponseVo.avatarList[i].paiArray[j], parentList[1],
                            handerCardList[1][j].transform.localPosition,
                            handerCardList[1][j].transform.localEulerAngles);
                        Destroy(handerCardList[1][j]);
                        handerCardList[1].RemoveAt(j);
                        handerCardList[1].Insert(j, game);
                    }

                    MyDebug.Log(i + "R");
                    break;
                case DirectionEnum.Top:
                    for (int j = 0; j < hupaiResponseVo.avatarList[i].paiArray.Count; j++)
                    {
                        var game = GameResourceManager.Instance.CreateGameObjectAndReturn(
                            hupaiResponseVo.avatarList[i].paiArray[j], parentList[2],
                            handerCardList[2][j].transform.localPosition,
                            handerCardList[2][j].transform.localEulerAngles);
                        Destroy(handerCardList[2][j]);
                        handerCardList[2].RemoveAt(j);
                        handerCardList[2].Insert(j, game);
                    }

                    MyDebug.Log(i + "T");
                    break;
                case DirectionEnum.Left:
                    for (int j = 0; j < hupaiResponseVo.avatarList[i].paiArray.Count; j++)
                    {
                        var game = GameResourceManager.Instance.CreateGameObjectAndReturn(
                            hupaiResponseVo.avatarList[i].paiArray[j], parentList[3],
                            handerCardList[3][j].transform.localPosition,
                            handerCardList[3][j].transform.localEulerAngles);
                        Destroy(handerCardList[3][j]);
                        handerCardList[3].RemoveAt(j);
                        handerCardList[3].Insert(j, game);
                    }

                    MyDebug.Log(i + "L");
                    break;
            }
        }

        yield return new WaitForSeconds(3);
        for (int i = 0; i < handerCardList[2].Count; i++)
        {
            handerCardList[2][i].transform.position += new Vector3(0, 0, 0.63f);
            handerCardList[2][i].transform.localEulerAngles = new Vector3(-90, -180, 0);
        }

        for (int i = 0; i < handerCardList[3].Count; i++)
        {
            handerCardList[3][i].transform.localEulerAngles = new Vector3(-90, 90, 0);
        }

        for (int i = 0; i < handerCardList[1].Count; i++)
        {
            handerCardList[1][i].transform.localEulerAngles = new Vector3(-90, -90, 0);
        }

        rightHand[1].startPos = handerCardList[3][handerCardList[3].Count - 1].transform.position +
                                new Vector3(6.86f, -2.15f, -2.58f);
        rightHand[1].SetHu();
        leftHand[0].startPos = handerCardList[3][0].transform.position + new Vector3(13.94f, -2.19f, -11.86f);
        leftHand[0].SetHu();
        rightHand[2].startPos = handerCardList[2][handerCardList[2].Count - 1].transform.position +
                                new Vector3(-2.33f, -1.72f, -7.16f);
        rightHand[2].SetHu();
        leftHand[1].startPos = handerCardList[2][0].transform.position + new Vector3(-10.71f, -1.81f, -13.57f);
        leftHand[1].SetHu();
        rightHand[3].startPos = handerCardList[1][handerCardList[1].Count - 1].transform.position +
                                new Vector3(-7.7f, -0.94f, 0.79f);
        rightHand[3].SetHu();
        leftHand[2].startPos = handerCardList[1][0].transform.position + new Vector3(-14.71f, -2.1f, 10.2f);
        leftHand[2].SetHu();
        yield return new WaitForSeconds(5);
        for (int i = 0; i < rightHand.Count; i++)
        {
            Destroy(rightHand[i].gameObject);
        }

        for (int i = 0; i < leftHand.Count; i++)
        {
            Destroy(leftHand[i].gameObject);
        }

        rightHand.Clear();
        leftHand.Clear();
    }

    public String GetDirection(int chairId)
    {
        int listIndex;
        var result = DirectionEnum.Bottom;
        var myChairId = GlobalDataScript.loginResponseData.chairID;
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

    public void RotationPai(int _dir)
    {
        var vector3 = Vector3.zero;
        switch (_dir)
        {
            case 1:
                vector3 = new Vector3(-90, -90, 0);
                break;
            case 2:
                vector3 = new Vector3(-90, -180, 0);
                break;
            case 3:
                vector3 = new Vector3(-90, 90, 0);
                break;
        }

        for (int i = 0; i < handerCardList[_dir].Count; i++)
        {
            handerCardList[_dir][i].transform.localEulerAngles = vector3;
            iTween.RotateBy(handerCardList[_dir][i],
                iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.3f));
        }
    }

    /// <summary>
    /// 碰牌消息
    /// </summary>
    /// <param name="response"></param>
    private void OnOtherPentReply(ClientResponse response) //其他人碰牌
    {
        var cardVo = JsonMapper.ToObject<OtherPengGangBackVO>(response.message);
        SetDirAction();
        //删除桌面上的麻将
        if (cardOnTable != null)
        {
            ReSetOutOnTabelCardPosition(cardOnTable);
            Destroy(cardOnTable);
        }

        if (curDirString == DirectionEnum.Bottom) //自己碰牌
        {
            var removeCount = 0;
            for (int i = 0; i < handerCardList[0].Count; i++)
            {
                var temp = handerCardList[0][i];
                var tempCardPoint = temp.GetComponent<BottomScript>().GetPoint();
                if (tempCardPoint == putOutCardPoint)
                {
                    handerCardList[0].RemoveAt(i);
                    Destroy(temp);
                    i--;
                    removeCount++;
                    if (removeCount == 2)
                    {
                        break;
                    }
                }
            }

            BottomPeng(cardVo.provideUser);
            RefreshCardPos(true);
        }
        else //其他人碰牌
        {
            var tempCardList = handerCardList[curListIndex];
            if (tempCardList != null)
            {
                MyDebug.Log("tempCardList.count======前" + tempCardList.Count);
                //消除其他的人牌碰牌长度
                for (int i = 0; i < 2; i++)
                {
                    var temp = tempCardList[0];
                    Destroy(temp);
                    tempCardList.RemoveAt(0);
                }

                MyDebug.Log("tempCardList.count======前" + tempCardList.Count);
                //排序
                RefreshCardPos(true);
            }

            GameObject obj1;
            var throwItem = new GameObject();
            var item = throwItem.AddComponent<ThrowItem>();
            var listObj = new List<GameObject>();
            GameObject go;
            switch (curDirString)
            {
                case DirectionEnum.Right:
                    for (int j = 0; j < 3; j++)
                    {
                        obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(cardVo.cardPoint,
                            throwItem.transform, Vector3.zero, Vector3.zero);
                        obj1.GetComponent<BottomScript>().SetDeskPointInfo(cardVo.cardPoint);
                        go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                        go.transform.parent = obj1.transform;
                        go.transform.localPosition = new Vector3(0, -0.284f, 0);
                        go.transform.localEulerAngles = new Vector3(90, 0, -180);
                        go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                        listObj.Add(obj1);
                    }

                    item.pengCardPoint = cardVo.cardPoint;
                    item.TestPosition(listObj, 1, putOutCardPointAvarIndex);
                    PengGangList_R.AddItem(item);
                    rightHand[3].startPos = item.transform.position + new Vector3(-8.5f, -3.45f, -1.9f);
                    StartCoroutine(rightHand[3].SetMove());
                    item.transform.position -= new Vector3(0, 0, 2);
                    iTween.MoveBy(item.gameObject,
                        iTween.Hash("z", 2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                            0.3f));
                    break;
                case DirectionEnum.Top:
                    for (int j = 0; j < 3; j++)
                    {
                        obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(cardVo.cardPoint,
                            throwItem.transform, Vector3.zero, Vector3.zero);
                        obj1.GetComponent<BottomScript>().SetDeskPointInfo(cardVo.cardPoint);
                        go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                        go.transform.parent = obj1.transform;
                        go.transform.localPosition = new Vector3(0, -0.284f, 0);
                        go.transform.localEulerAngles = new Vector3(90, 0, -180);
                        go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                        listObj.Add(obj1);
                    }

                    item.pengCardPoint = cardVo.cardPoint;
                    item.TestPosition(listObj, 2, putOutCardPointAvarIndex);
                    PengGangList_T.AddItem(item);
                    item.transform.position += new Vector3(2, 0, 0);
                    rightHand[2].startPos = item.transform.position + new Vector3(1.04f, -3.72f, -7.99f);
                    StartCoroutine(rightHand[2].SetMove());
                    iTween.MoveBy(item.gameObject,
                        iTween.Hash("x", -2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                            0.3f));
                    break;
                case DirectionEnum.Left:
                    for (int j = 0; j < 3; j++)
                    {
                        obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(cardVo.cardPoint,
                            throwItem.transform, Vector3.zero, Vector3.zero);
                        obj1.GetComponent<BottomScript>().SetDeskPointInfo(cardVo.cardPoint);
                        go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                        go.transform.parent = obj1.transform;
                        go.transform.localPosition = new Vector3(0, -0.284f, 0);
                        go.transform.localEulerAngles = new Vector3(90, 0, -180);
                        go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                        listObj.Add(obj1);
                    }

                    item.pengCardPoint = cardVo.cardPoint;
                    item.TestPosition(listObj, 3, putOutCardPointAvarIndex);
                    PengGangList_L.AddItem(item);
                    item.transform.position += new Vector3(0, 0, 2);
                    rightHand[1].startPos = item.transform.position + new Vector3(8.67f, -3.65f, 1.9f);
                    StartCoroutine(rightHand[1].SetMove());
                    iTween.MoveBy(item.gameObject,
                        iTween.Hash("z", -2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                            0.3f));
                    break;
            }
        }
    }

    /// <summary>
    /// 其他人出牌
    /// </summary>
    /// <param name="response"></param>
    private void OnOtherPutOutCardReply(ClientResponse response)
    {
        var sgvo = JsonMapper.ToObject<OutCardVO>(response.message);
        var cardPoint = sgvo.cardID;
        putOutCardPointAvarIndex = curListIndex;
        StartCoroutine(CreatePutOutCardAndPlayAction(cardPoint, putOutCardPointAvarIndex, curDirString,
            otherPickCardItem));
        otherPickCardItem = null;
    }

    /// <summary>
    /// 自己杠牌
    /// </summary>
    /// <param name="response"></param>
    private void OnGangReply(ClientResponse response)
    {
        isGang = true;
        var gangBackVo = JsonMapper.ToObject<GangBackVO>(response.message);
        gangKind = gangBackVo.type;
        //if (gangBackVo.wProvideUser == GlobalDataScript.loginResponseData.chairID)
        //{
        //    gangKind = mineList[gangBackVo.cardPoint] == 3 ? 1 : 0;
        //}
        MyDebug.Log("GangKind:" + gangKind);
        selfGangCardPoint = gangBackVo.cardPoint;
        SetDirAction();
        if (gangKind == 0)
        {
            //明杠，杠的牌必为其他人打的牌
            GameResourceManager.Instance.PlayMGangFx(outparentList[0]);
            //销毁别人打的牌
            if (cardOnTable != null)
            {
                ReSetOutOnTabelCardPosition(cardOnTable);
                Destroy(cardOnTable);
            }

            //销毁手牌中的三张牌
            var removeCount = 0;
            for (int i = 0; i < handerCardList[0].Count; i++)
            {
                var temp = handerCardList[0][i];
                var tempCardPoint = handerCardList[0][i].GetComponent<BottomScript>().GetPoint();
                if (selfGangCardPoint == tempCardPoint)
                {
                    handerCardList[0].RemoveAt(i);
                    Destroy(temp);
                    i--;
                    removeCount++;
                    if (removeCount == 3)
                    {
                        break;
                    }
                }
            }

            mineList[selfGangCardPoint] -= 3;
            GameObject go;
            GameObject obj1;
            var throwItem = new GameObject();
            var item = throwItem.AddComponent<ThrowItem>();
            var listObj = new List<GameObject>();
            for (int j = 0; j < 4; j++)
            {
                obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(selfGangCardPoint, throwItem.transform,
                    Vector3.zero, Vector3.zero);
                obj1.GetComponent<BottomScript>().SetDeskPointInfo(selfGangCardPoint);
                go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                go.transform.parent = obj1.transform;
                go.transform.localPosition = new Vector3(0, -0.284f, 0);
                go.transform.localEulerAngles = new Vector3(90, 0, -180);
                go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                listObj.Add(obj1);
            }

            item.TestPosition(listObj, 0, putOutCardPointAvarIndex);
            PengGangList_B.AddItem(item);
            item.transform.position -= new Vector3(2, 0, 0);
            rightHand[0].startPos = item.transform.position + new Vector3(-1.5f, -3.16f, 8.68f);
            StartCoroutine(rightHand[0].SetMove());
            iTween.MoveBy(item.gameObject,
                iTween.Hash("x", 2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay", 0.3f));
        }
        else if (gangKind == 1)
        {
            //自己杠，自己打的牌
            if (GetPaiInpeng(selfGangCardPoint, DirectionEnum.Bottom) == -1)
            {
                //暗杠
                GameResourceManager.Instance.PlayAGangFx(outparentList[0]);
                var removeCount = 0;
                for (int i = 0; i < handerCardList[0].Count; i++)
                {
                    var temp = handerCardList[0][i];
                    var tempCardPoint = handerCardList[0][i].GetComponent<BottomScript>().GetPoint();
                    if (selfGangCardPoint == tempCardPoint)
                    {
                        handerCardList[0].RemoveAt(i);
                        Destroy(temp);
                        i--;
                        removeCount++;
                        if (removeCount == 4)
                        {
                            break;
                        }
                    }
                }

                mineList[selfGangCardPoint] -= 4;
                GameObject obj1;
                var throwItem = new GameObject();
                var item = throwItem.AddComponent<ThrowItem>();
                var listObj = new List<GameObject>();
                for (int j = 0; j < 4; j++)
                {
                    obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(selfGangCardPoint,
                        throwItem.transform, Vector3.zero, Vector3.zero);
                    obj1.GetComponent<BottomScript>().SetDeskPointInfo(selfGangCardPoint);
                    listObj.Add(obj1);
                }

                item.TestPosition(listObj, 0, 0);
                PengGangList_B.AddItem(item);
                item.transform.position -= new Vector3(2, 0, 0);
                rightHand[0].startPos = item.transform.position + new Vector3(-1.5f, -3.16f, 8.68f);
                StartCoroutine(rightHand[0].SetMove());
                iTween.MoveBy(item.gameObject,
                    iTween.Hash("x", 2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay", 0.3f));
            }
            else //在碰牌数组以内，则一定是自摸的牌
            {
                Destroy(pickCardItem);
                var index = GetPaiInpeng(selfGangCardPoint, DirectionEnum.Bottom);
                //将杠牌放到对应位置
                PengGangList_B.RefreshGang(index);
                mineList[selfGangCardPoint]--;
            }
        }

        RefreshCardPos(false);
        GlobalDataScript.isDrag = true;
        Debug.Log(GlobalDataScript.isDrag);
    }

    /// <summary>
    /// 其他人杠牌
    /// </summary>
    /// <param name="response"></param>
    private void OnotherGangReply(ClientResponse response) //其他人杠牌
    {
        GameObject go;
        isGang = true;
        var gangNotice = JsonMapper.ToObject<GangNoticeVO>(response.message);
        otherGangCard = gangNotice.cardPoint;
        otherGangType = gangNotice.type;
        var tempvector3 = new Vector3(0, 0, 0);
        var tempRotation = Vector3.zero;
        SetDirAction();
        SoundManager.Instance.PlaySoundByAction("gang", 0);
        List<GameObject> tempCardList = null;
        switch (curDirString)
        {
            case DirectionEnum.Right:
                tempCardList = handerCardList[1];
                break;
            case DirectionEnum.Top:
                tempCardList = handerCardList[2];
                break;
            case DirectionEnum.Left:
                tempCardList = handerCardList[3];
                break;
        }

        var tempList = new List<GameObject>();
        var gangIndex = GetPaiInpeng(otherGangCard, curDirString);
        if (gangIndex == -1)
        {
            //删除玩家手牌，当玩家碰牌牌组里面的有碰牌时，不用删除手牌
            for (int i = 0; i < 3; i++)
            {
                var temp = tempCardList[0];
                tempCardList.RemoveAt(0);
                Destroy(temp);
            }

            if (tempCardList != null)
            {
                RefreshCardPos(false);
            }

            //创建杠牌，当玩家碰牌牌组里面的无碰牌，才创建
            if (otherGangType == 0)
            {
                if (cardOnTable != null)
                {
                    ReSetOutOnTabelCardPosition(cardOnTable);
                    Destroy(cardOnTable);
                }

                var throwItem = new GameObject();
                var item = throwItem.AddComponent<ThrowItem>();
                var listObj = new List<GameObject>();
                for (int i = 0; i < 4; i++) //实例化其他人杠牌
                {
                    var obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(otherGangCard,
                        throwItem.transform, Vector3.zero, Vector3.zero);
                    obj1.GetComponent<BottomScript>().SetDeskPointInfo(otherGangCard);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj1.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj1);
                }

                switch (curDirString)
                {
                    case DirectionEnum.Right:
                        GameResourceManager.Instance.PlayMGangFx(outparentList[1]);
                        item.TestPosition(listObj, 1, putOutCardPointAvarIndex);
                        PengGangList_R.AddItem(item);
                        item.transform.position -= new Vector3(0, 0, 2);
                        rightHand[3].startPos = item.transform.position + new Vector3(-8.5f, -3.45f, -1.9f);
                        StartCoroutine(rightHand[3].SetMove());
                        iTween.MoveBy(item.gameObject,
                            iTween.Hash("z", 2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                                0.3f));
                        break;
                    case DirectionEnum.Top:
                        GameResourceManager.Instance.PlayMGangFx(outparentList[2]);
                        item.TestPosition(listObj, 2, putOutCardPointAvarIndex);
                        PengGangList_T.AddItem(item);
                        item.transform.position += new Vector3(2, 0, 0);
                        rightHand[2].startPos = item.transform.position + new Vector3(1.04f, -3.72f, -7.99f);
                        StartCoroutine(rightHand[2].SetMove());
                        iTween.MoveBy(item.gameObject,
                            iTween.Hash("x", -2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                                0.3f));
                        break;
                    case DirectionEnum.Left:
                        GameResourceManager.Instance.PlayMGangFx(outparentList[3]);
                        item.TestPosition(listObj, 3, putOutCardPointAvarIndex);
                        PengGangList_L.AddItem(item);
                        item.transform.position += new Vector3(0, 0, 2);
                        rightHand[1].startPos = item.transform.position + new Vector3(8.67f, -3.65f, 1.9f);
                        StartCoroutine(rightHand[1].SetMove());
                        iTween.MoveBy(item.gameObject,
                            iTween.Hash("z", -2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                                0.3f));
                        break;
                }
            }
            else if (otherGangType == 1)
            {
                Destroy(otherPickCardItem);
                var throwItem = new GameObject();
                var item = throwItem.AddComponent<ThrowItem>();
                var listObj = new List<GameObject>();
                for (int j = 0; j < 4; j++)
                {
                    var obj2 = GameResourceManager.Instance.CreateGameObjectAndReturn(otherGangCard,
                        throwItem.transform, Vector3.zero, Vector3.zero);
                    go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                    go.transform.parent = obj2.transform;
                    go.transform.localPosition = new Vector3(0, -0.284f, 0);
                    go.transform.localEulerAngles = new Vector3(90, 0, -180);
                    go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
                    listObj.Add(obj2);
                }

                switch (curDirString)
                {
                    case DirectionEnum.Right:
                        GameResourceManager.Instance.PlayAGangFx(outparentList[1]);
                        item.TestPosition(listObj, 1, 1);
                        PengGangList_R.AddItem(item);
                        rightHand[3].startPos = item.transform.position + new Vector3(-8.5f, -3.45f, -1.9f);
                        StartCoroutine(rightHand[3].SetMove());
                        item.transform.position -= new Vector3(0, 0, 2);
                        iTween.MoveBy(item.gameObject,
                            iTween.Hash("z", 2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                                0.3f));
                        break;
                    case DirectionEnum.Top:
                        GameResourceManager.Instance.PlayAGangFx(outparentList[2]);
                        item.TestPosition(listObj, 2, 2);
                        PengGangList_T.AddItem(item);
                        rightHand[2].startPos = item.transform.position + new Vector3(1.04f, -3.72f, -7.99f);
                        StartCoroutine(rightHand[2].SetMove());
                        item.transform.position += new Vector3(2, 0, 0);
                        iTween.MoveBy(item.gameObject,
                            iTween.Hash("x", -2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                                0.3f));
                        break;
                    case DirectionEnum.Left:
                        GameResourceManager.Instance.PlayAGangFx(outparentList[3]);
                        item.TestPosition(listObj, 3, 3);
                        PengGangList_L.AddItem(item);
                        rightHand[1].startPos = item.transform.position + new Vector3(8.67f, -3.65f, 1.9f);
                        StartCoroutine(rightHand[1].SetMove());
                        item.transform.position += new Vector3(0, 0, 2);
                        iTween.MoveBy(item.gameObject,
                            iTween.Hash("z", -2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay",
                                0.3f));
                        break;
                }
            }
        }
        else if (gangIndex != -1) //有碰牌时杠
        {
            if (otherPickCardItem != null)
            {
                Destroy(otherPickCardItem);
            }

            switch (curDirString)
            {
                case DirectionEnum.Top:
                    GameResourceManager.Instance.PlayMGangFx(outparentList[2]);
                    PengGangList_T.RefreshGang(gangIndex);
                    break;
                case DirectionEnum.Left:
                    GameResourceManager.Instance.PlayMGangFx(outparentList[3]);
                    PengGangList_L.RefreshGang(gangIndex);
                    break;
                case DirectionEnum.Right:
                    GameResourceManager.Instance.PlayMGangFx(outparentList[1]);
                    PengGangList_R.RefreshGang(gangIndex);
                    break;
            }
        }
    }

    /// <summary>
    /// 设置杠牌的牌点
    /// </summary>
    /// <param name="_num"></param>
    private void SetMyGangPoint(int _num)
    {
        selfGangCardPoint = _num;
    }

    /// <summary>
    /// 设置玩家方向
    /// </summary>
    /// <param name="_str"></param>
    private void SetPlayersDir(string _str)
    {
        curDirString = _str;
        MyDebug.Log("curDirString:" + curDirString);
    }

    /// <summary>
    /// </summary>
    /// <param name="_num"></param>
    private void SetListIndex(int _num)
    {
        curListIndex = _num;
        MyDebug.Log("curListIndex:" + curListIndex);
    }

    /// <summary>
    /// 设置方向计数
    /// </summary>
    /// <param name="_num"></param>
    private void SetDirCount(int _num)
    {
        dirCount = _num;
    }

    /// <summary>
    /// 断线重连
    /// </summary>
    /// <param name="response"></param>
    private void ReturnGameReply(ClientResponse response)
    {
        var returnstr = response.message;
        //1.显示剩余牌的张数和圈数
        var returnJsonData = JsonMapper.ToObject(response.message);
        var surplusCards = returnJsonData["surplusCards"].ToString();
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
            putOutCardPointAvarIndex = curListIndex;
            putOutCardPoint = putOffCardPointTemp; //碰
            SelfAndOtherPutoutCard = putOutCardPoint;
            pickAvatarIndexTemp = int.Parse(returnJsonData["pickAvatarIndex"].ToString()); //当前摸牌牌人的索引
            /**这句代码有可能引发catch  所以后面的 SelfAndOtherPutoutCard = currentCardPointTemp; 可能不执行**/
            currentCardPointTemp = int.Parse(returnJsonData["currentCardPoint"].ToString()); //当前摸得的点数
            SelfAndOtherPutoutCard = currentCardPointTemp;
        }
        catch (Exception)
        {
            throw;
        }

        if (pickAvatarIndexTemp == curListIndex)
        {
            //自己摸牌
            if (currentCardPointTemp == -2)
            {
                MoPaiCardPoint = handerCardList[0][handerCardList[0].Count - 1].GetComponent<BottomScript>().GetPoint();
                SelfAndOtherPutoutCard = MoPaiCardPoint;
                useForGangOrPengOrChi = curAvatarIndexTemp;
                Destroy(handerCardList[0][handerCardList[0].Count - 1]);
                handerCardList[0].Remove(handerCardList[0][handerCardList[0].Count - 1]);
                MoPai();
                SetDirAction();
                GlobalDataScript.isDrag = true;
                Debug.Log(GlobalDataScript.isDrag);
                MyDebug.Log("自己摸牌");
            }
            else
            {
                if ((handerCardList[0].Count) % 3 != 1)
                {
                    MoPaiCardPoint = currentCardPointTemp;
                    MyDebug.Log("摸牌" + MoPaiCardPoint);
                    SelfAndOtherPutoutCard = MoPaiCardPoint;
                    useForGangOrPengOrChi = curAvatarIndexTemp;
                    for (int i = 0; i < handerCardList[0].Count; i++)
                    {
                        if (handerCardList[0][i].GetComponent<BottomScript>().GetPoint() == currentCardPointTemp)
                        {
                            Destroy(handerCardList[0][i]);
                            handerCardList[0].Remove(handerCardList[0][i]);
                            break;
                        }
                    }

                    MoPai();
                    SetDirAction();
                    GlobalDataScript.isDrag = true;
                    Debug.Log(GlobalDataScript.isDrag);
                }
            }
        }
        else
        {
            //别人摸牌
            SetDirAction();
        }

        //光标指向打牌人
        var dirindex = curListIndex;
        cardOnTable = tableCardList[dirindex][tableCardList[dirindex].Count - 1];
        if (tableCardList[dirindex] == null || tableCardList[dirindex].Count == 0)
        {
            //刚启动
        }
        else
        {
            var temp = tableCardList[dirindex][tableCardList[dirindex].Count - 1];
            SetPointGameObject(temp);
        }
    }

    private void BottomChi(int card1, int card2, int card3)
    {
        GameObject go;
        var throwItem = new GameObject();
        var item = throwItem.AddComponent<ThrowItem>();
        var listObj = new List<GameObject>();
        var obj2 = GameResourceManager.Instance.CreateGameObjectAndReturn(card3, throwItem.transform, Vector3.zero,
            new Vector3(0, 0, 0));
        obj2.GetComponent<BottomScript>().SetDeskPointInfo(card3);
        go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
        go.transform.parent = obj2.transform;
        go.transform.localPosition = new Vector3(0, -0.284f, 0);
        go.transform.localEulerAngles = new Vector3(90, 0, -180);
        go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
        listObj.Add(obj2);
        obj2 = GameResourceManager.Instance.CreateGameObjectAndReturn(card2, throwItem.transform, Vector3.zero,
            new Vector3(0, 0, 0));
        obj2.GetComponent<BottomScript>().SetDeskPointInfo(card2);
        go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
        go.transform.parent = obj2.transform;
        go.transform.localPosition = new Vector3(0, -0.284f, 0);
        go.transform.localEulerAngles = new Vector3(90, 0, -180);
        go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
        listObj.Add(obj2);
        obj2 = GameResourceManager.Instance.CreateGameObjectAndReturn(card1, throwItem.transform, Vector3.zero,
            new Vector3(0, 0, 0));
        obj2.GetComponent<BottomScript>().SetDeskPointInfo(card1);
        go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
        go.transform.parent = obj2.transform;
        go.transform.localPosition = new Vector3(0, -0.284f, 0);
        go.transform.localEulerAngles = new Vector3(90, 0, -180);
        go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
        listObj.Add(obj2);
        if (putOutCardPoint == card1)
        {
            item.TestPosition(listObj, 0, 3);
        }
        else if (putOutCardPoint == card2)
        {
            item.TestPosition(listObj, 0, 2);
        }
        else if (putOutCardPoint == card3)
        {
            item.TestPosition(listObj, 0, 1);
        }

        PengGangList_B.AddItem(item);
        item.transform.position -= new Vector3(2, 0, 0);
        rightHand[0].startPos = item.transform.position + new Vector3(-1.5f, -3.16f, 8.68f);
        StartCoroutine(rightHand[0].SetMove());
        iTween.MoveBy(item.gameObject,
            iTween.Hash("x", 2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay", 0.3f));
        mineList[putOutCardPoint] -= 2;
        GlobalDataScript.isDrag = true;
        Debug.Log(GlobalDataScript.isDrag);
    }

    /// <summary>
    /// 自己碰
    /// </summary>
    /// <param name="provideUser">todo: describe provideUser parameter on BottomPeng</param>
    private void BottomPeng(int provideUser)
    {
        GameObject obj1;
        var throwItem = new GameObject();
        var item = throwItem.AddComponent<ThrowItem>();
        var listObj = new List<GameObject>();
        for (int j = 0; j < 3; j++)
        {
            obj1 = GameResourceManager.Instance.CreateGameObjectAndReturn(putOutCardPoint, throwItem.transform,
                Vector3.zero, Vector3.zero);
            obj1.GetComponent<BottomScript>().SetDeskPointInfo(putOutCardPoint);
            var go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
            go.transform.parent = obj1.transform;
            go.transform.localPosition = new Vector3(0, -0.284f, 0);
            go.transform.localEulerAngles = new Vector3(90, 0, -180);
            go.transform.localScale = new Vector3(0.093f, 0.242f, 0.179f);
            listObj.Add(obj1);
        }

        item.pengCardPoint = putOutCardPoint;
        item.TestPosition(listObj, 0, putOutCardPointAvarIndex);
        PengGangList_B.AddItem(item);
        item.transform.position -= new Vector3(2, 0, 0);
        rightHand[0].startPos = item.transform.position + new Vector3(-1.5f, -3.16f, 8.68f);
        StartCoroutine(rightHand[0].SetMove());
        iTween.MoveBy(item.gameObject,
            iTween.Hash("x", 2, "easeType", "easeOutCubic", "loopType", "none", "time", 0.5f, "delay", 0.3f));
        mineList[putOutCardPoint] -= 2;
        GlobalDataScript.isDrag = true;
        Debug.Log(GlobalDataScript.isDrag);
    }

    /// <summary>
    /// 从牌桌上移除最后一次打出来的牌
    /// </summary>
    /// <param name="cardOnTable"></param>
    private void ReSetOutOnTabelCardPosition(GameObject cardOnTable)
    {
        MyDebug.Log("putOutCardPointAvarIndex===========:" + putOutCardPointAvarIndex);
        if (putOutCardPointAvarIndex != -1)
        {
            var objIndex = tableCardList[putOutCardPointAvarIndex].IndexOf(cardOnTable);
            if (objIndex != -1)
            {
                InitPointTween();
                tableCardList[putOutCardPointAvarIndex].RemoveAt(objIndex);
                return;
            }
        }
    }

    /// <summary>
    /// 判断碰牌的牌组里面是否包含某个牌，用于判断是否实例化一张牌还是三张牌
    /// </summary>
    /// <param name="cardPoint">牌点</param>
    /// <param name="direction">方向</param>
    /// <returns>返回-1  代表没有牌,其余牌在list的位置</returns>
    private int GetPaiInpeng(int cardPoint, string direction)
    {
        var jugeList = new List<List<GameObject>>();
        switch (direction)
        {
            case DirectionEnum.Bottom: //自己
                return PengGangList_B.GetPengCard(cardPoint);
            case DirectionEnum.Right:
                return PengGangList_R.GetPengCard(cardPoint);
            case DirectionEnum.Left:
                return PengGangList_L.GetPengCard(cardPoint);
            case DirectionEnum.Top:
                return PengGangList_T.GetPengCard(cardPoint);
        }

        if (jugeList == null || jugeList.Count == 0)
        {
            return -1;
        }

        //循环遍历比对点数
        for (int i = 0; i < jugeList.Count; i++)
        {
            try
            {
                if (jugeList[i][0].GetComponent<BottomScript>().GetPoint() == cardPoint)
                {
                    return i;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        return -1;
    }

    /// <summary>
    /// 初始化列表
    /// </summary>
    private void InitArrayList()
    {
        mineList = new List<int>();
        handerCardList = new List<List<GameObject>>();
        tableCardList = new List<List<GameObject>>();
        for (int i = 0; i < 4; i++)
        {
            handerCardList.Add(new List<GameObject>());
            tableCardList.Add(new List<GameObject>());
        }

        publicCardList.Clear();
        lastCardList.Clear();
    }

    /// <summary>
    /// 初始化牌桌信息--为动画服务
    /// </summary>
    private void InitTable()
    {
        NumberOfGame();
        InitPointTween();
        isGang = false;
        gangCount = 0;
        isShowGlass = false;
        tableAnima.transform.position = new Vector3(0, 3, 0);
        publicCards.transform.position = new Vector3(0, 3.638f, 0);
        publicCards.transform.eulerAngles = new Vector3(0, 90, 0);
        publicB.transform.localPosition = new Vector3(11.66f, 0, 0);
        publicT.transform.localPosition = new Vector3(-11.66f, 0, 0);
        publicR.transform.localPosition = new Vector3(0, 0, 11.66f);
        publicL.transform.localPosition = new Vector3(0, 0, -11.66f);
        ClearAndDestroyListCell(handerCardList, false);
        ClearAndDestroyListCell(tableCardList, false);
        PengGangList_L.Clear();
        PengGangList_R.Clear();
        PengGangList_T.Clear();
        PengGangList_B.Clear();
        DestoryAllChild(pengGangParenTransformB.gameObject);
        DestoryAllChild(pengGangParenTransformR.gameObject);
        DestoryAllChild(pengGangParenTransformT.gameObject);
        DestoryAllChild(pengGangParenTransformL.gameObject);
        ClearAndDestroyListCell(publicCardList);
        ClearAndDestroyListCell(lastCardList);
    }

    /// <summary>
    ///清空 游戏数据
    /// </summary>
    /// <param name="temp"></param>
    /// <param name="isClear">todo: describe isClear parameter on ClearAndDestroyListCell</param>
    private void ClearAndDestroyListCell(List<List<GameObject>> temp, bool isClear = true)
    {
        for (int i = 0; i < temp.Count; i++)
        {
            for (int m = 0; m < temp[i].Count; m++)
            {
                DestroyObject(temp[i][m]);
            }

            temp[i].Clear();
        }

        if (isClear)
            temp.Clear();
    }

    private void DestoryAllChild(GameObject go)
    {
        for (int i = 0; i < go.transform.childCount; i++)
        {
            Destroy(go.transform.GetChild(i).gameObject);
        }
    }

    private void ClearAndDestroyListCell(List<GameObject> temp)
    {
        for (int i = 0; i < temp.Count; i++)
        {
            DestroyObject(temp[i]);
        }

        temp.Clear();
    }

    /// <summary>
    /// 初始化公共牌
    /// </summary>
    /// <param name="cardCount"></param>
    private void InitPublicCardList(int cardCount)
    {
        float pSize = cardCount / 4 / 4;
        for (int i = 0; i < cardCount / 4; i++)
        {
            var gob = GameResourceManager.Instance.CreateGameObjectAndReturn(0, publicB.transform,
                new Vector3(0, -i % 2 * spaceH, (i / 2 - pSize) * spaceW), new Vector3(0, 90, 180));
            if (gob.transform.localPosition.y < 0)
            {
                var go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                go.transform.parent = gob.transform;
                switch (bankerId)
                {
                    case 0:
                        go.transform.localPosition = new Vector3(0, 0.332f, 0.098f);
                        break;
                    case 1:
                        go.transform.localPosition = new Vector3(0.093f, 0.332f, 0.061f);
                        break;
                    case 2:
                        go.transform.localPosition = new Vector3(0f, 0.332f, -0.201f);
                        break;
                    case 3:
                        go.transform.localPosition = new Vector3(-0.093f, 0.332f, 0.061f);
                        break;
                }
            }

            if (gob != null) //
            {
                publicCardList.Add(gob); //增加游戏对象
            }
        }

        for (int i = 0; i < cardCount / 4; i++)
        {
            var gob = GameResourceManager.Instance.CreateGameObjectAndReturn(0, publicR.transform,
                new Vector3((pSize - i / 2) * spaceW, -i % 2 * spaceH, 0), new Vector3(0, 0, 180));
            if (gob.transform.localPosition.y < 0)
            {
                var go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                go.transform.parent = gob.transform;
                go.transform.localEulerAngles = new Vector3(270, 90, 90);
                switch (bankerId)
                {
                    case 0:
                        go.transform.localPosition = new Vector3(-0.093f, 0.332f, 0.061f);
                        break;
                    case 1:
                        go.transform.localPosition = new Vector3(0, 0.332f, 0.098f);
                        break;
                    case 2:
                        go.transform.localPosition = new Vector3(0.093f, 0.332f, 0.061f);
                        break;
                    case 3:
                        go.transform.localPosition = new Vector3(0f, 0.332f, -0.201f);
                        break;
                }
            }

            if (gob != null) //
            {
                publicCardList.Add(gob); //增加游戏对象
            }
        }

        for (int i = 0; i < cardCount / 4; i++)
        {
            var gob = GameResourceManager.Instance.CreateGameObjectAndReturn(0, publicT.transform,
                new Vector3(0, -i % 2 * spaceH, (pSize - i / 2) * spaceW), new Vector3(0, 90, 180));
            if (gob.transform.localPosition.y < 0)
            {
                var go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                go.transform.parent = gob.transform;
                switch (bankerId)
                {
                    case 0:
                        go.transform.localPosition = new Vector3(0f, 0.332f, 0.201f);
                        break;
                    case 1:
                        go.transform.localPosition = new Vector3(0.093f, 0.332f, -0.061f);
                        break;
                    case 2:
                        go.transform.localPosition = new Vector3(0, 0.332f, -0.098f);
                        break;
                    case 3:
                        go.transform.localPosition = new Vector3(-0.093f, 0.332f, -0.061f);
                        break;
                }
            }

            if (gob != null) //
            {
                publicCardList.Add(gob); //增加游戏对象
            }
        }

        for (int i = 0; i < cardCount / 4; i++)
        {
            var gob = GameResourceManager.Instance.CreateGameObjectAndReturn(0, publicL.transform,
                new Vector3((i / 2 - pSize) * spaceW, -i % 2 * spaceH, 0),
                new Vector3(0, 0, 180)); /*Instantiate(Resources.Load(mjPath + 0)) as GameObject;*/
            if (gob.transform.localPosition.y < 0)
            {
                var go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
                go.transform.parent = gob.transform;
                go.transform.localEulerAngles = new Vector3(270, 90, 90);
                switch (bankerId)
                {
                    case 0:
                        go.transform.localPosition = new Vector3(-0.093f, 0.332f, -0.061f);
                        break;
                    case 1:
                        go.transform.localPosition = new Vector3(0f, 0.332f, 0.201f);
                        break;
                    case 2:
                        go.transform.localPosition = new Vector3(0.098f, 0.332f, -0.061f);
                        break;
                    case 3:
                        go.transform.localPosition = new Vector3(0, 0.332f, -0.098f);
                        break;
                }
            }

            if (gob != null) //
            {
                publicCardList.Add(gob); //增加游戏对象-20
            }
        }
    }

    /// <summary>
    /// 根据庄家旋转方向和公共牌
    /// </summary>
    private void SetDeskRotation()
    {
        publicCards.transform.localEulerAngles = new Vector3(0, (1 - dirCount) * 90, 0);

    }

    /// <summary>
    /// 设置红色箭头的显示方向
    /// </summary>
    private void SetDirAction() //设置方向
    {
        for (int i = 0; i < dirLight.Count; i++)
        {
            if (i == (curListIndex - DeskDir + 4) % 4)
            {
                dirLight[i].GetComponent<Renderer>().material = lightDir;
            }
            else
                dirLight[i].GetComponent<Renderer>().material = darkDir;
        }
    }

    /// <summary>
    /// 自己摸牌
    /// </summary>
    private void MoPai()
    {
        pickCardItem =
            GameResourceManager.Instance.CreateGameObjectAndReturn(MoPaiCardPoint, parentList[0], Vector3.zero,
                Vector3.zero); /*Instantiate(Resources.Load(mjPath + MoPaiCardPoint)) as GameObject;*/ //实例化当前摸的牌
        MyDebug.Log("摸牌 === >> " + MoPaiCardPoint);
        if (pickCardItem != null) //有可能没牌了
        {
            pickCardItem.name = "pickCardItem";
            pickCardItem.transform.localEulerAngles = new Vector3(-40, 270, 90);
            pickCardItem.transform.localPosition =
                new Vector3((myCardsCount / 2.0f - PengGangList_B.Count) * spaceW, 2, 0);
            pickCardItem.GetComponent<BottomScript>().OnSendMessage += CardChange; //发送消息
            pickCardItem.GetComponent<BottomScript>().ReSetPoisiton += CardSelect;
            pickCardItem.GetComponent<BottomScript>().InitMyPoint(MoPaiCardPoint); //得到索引
            PickCardAnima(pickCardItem, new Vector3((myCardsCount / 2.0f - PengGangList_B.Count) * spaceW + 550, 0, 0),
                new Vector3(-90, 270, 90));
            RemovePublicCards();
            mineList[MoPaiCardPoint]++;
            InsertCardIntoList(pickCardItem);
        }

        MyDebug.Log("moPai  goblist count === >> " + handerCardList[0].Count);
    }

    /// <summary>
    /// 插入自己列表的牌
    /// </summary>
    /// <param name="item"></param>
    private void InsertCardIntoList(GameObject item) //插入牌的方法
    {
        Debug.Log("GlobalDataScript.isWhiteCard:" + GlobalDataScript.putOutCount);
        if (GlobalDataScript.putOutCount < 3)
            StartCoroutine(PutPickCard());
        var curCardPoint = item.GetComponent<BottomScript>().GetPoint(); //得到当前牌指针
        if (curCardPoint == 33)
        {
            item.GetComponent<BottomScript>().isFlyCard = true;
            handerCardList[0].Insert(0, item); //在
            item = null;
            return;
        }

        for (int i = 0; i < handerCardList[0].Count; i++) //i<游戏物体个数 自增
        {
            if (handerCardList[0][i] == null)
            {
                MyDebug.Log("insertCardIntoList:" + i);
            }

            var cardPoint = handerCardList[0][i].GetComponent<BottomScript>().GetPoint(); //得到所有牌指针
            if (cardPoint > curCardPoint && cardPoint != 33) //牌指针>=当前牌的时候插入
            {
                item.GetComponent<BottomScript>().isFlyCard = true;
                handerCardList[0].Insert(i, item); //在
                item = null;
                return;
            }
        }

        handerCardList[0].Add(item); //游戏对象列表添加当前牌
        item = null;
    }

    IEnumerator PutPickCard()
    {
        Debug.Log("111111111111111");
        yield return new WaitForSeconds(1.5f);
        GlobalDataScript.isDrag = true;
        CardChange(pickCardItem);
    }

    /// <summary>
    /// 从公共牌里移除一个牌
    /// </summary>
    /// <param name="dex"></param>
    private void RemovePublicCards(int dex = 0)
    {
        if (publicCardList.Count <= 0)
        {
            RemoveLastCards();
            return;
        }

        DestroyObject(publicCardList[dex]);
        publicCardList.RemoveAt(dex);
    }

    /// <summary>
    /// 检测杠牌从哪里拿
    /// </summary>
    private void CheckGangRemove()
    {
        if (lastCardList.Count <= 0)
        {
            if (publicCardList.Count == 1)
            {
                RemovePublicCards();
            }
            else
            {
                if (gangCount % 2 == 0)
                {
                    RemoveLastCards(publicCardList.Count - 2);
                }
                else
                {
                    RemoveLastCards(publicCardList.Count - 1);
                }
            }

            return;
        }

        if (lastCardList.Count == 1)
        {
            RemoveLastCards();
            return;
        }

        if (gangCount % 2 == 0)
        {
            RemoveLastCards(lastCardList.Count - 2);
        }
        else
        {
            RemoveLastCards(lastCardList.Count - 1);
        }
    }

    /// <summary>
    /// 剩余牌里删除某个牌--这里的牌是只根据筛子点数留下的牌
    /// </summary>
    /// <param name="dex"></param>
    private void RemoveLastCards(int dex = 0)
    {
        if (lastCardList.Count <= 0)
            return;
        DestroyObject(lastCardList[dex]);
        lastCardList.RemoveAt(dex);
    }

    /// <summary>
    /// 播放开始动画
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayDeskAnima()
    {
        SetDeskRotation();
        yield return new WaitForSeconds(1);
        iTween.MoveBy(tableAnima,
            iTween.Hash("y", -1.8f, "easeType", "easeOutCubic", "loopType", "none", "delay", 0.1));
        yield return new WaitForSeconds(1);
        iTween.MoveBy(publicB, iTween.Hash("x", -2, "easeType", "easeOutCubic", "loopType", "none", "delay", 0.1));
        iTween.MoveBy(publicR, iTween.Hash("z", -2, "easeType", "easeOutCubic", "loopType", "none", "delay", 0.1));
        iTween.MoveBy(publicT, iTween.Hash("x", 2, "easeType", "easeOutCubic", "loopType", "none", "delay", 0.1));
        iTween.MoveBy(publicL, iTween.Hash("z", 2, "easeType", "easeOutCubic", "loopType", "none", "delay", 0.1));
        switch (curDirString)
        {
            case DirectionEnum.Bottom:
                rightHand[0].startPos = new Vector3(0.16f, 4.95f, 13.5f);
                rightHand[0].transform.eulerAngles = new Vector3(-10, 0, 0);
                StartCoroutine(rightHand[0].SetStart());
                break;
            case DirectionEnum.Left:
                rightHand[1].startPos = new Vector3(13.32f, 2.46f, -0.84f);
                StartCoroutine(rightHand[1].SetStart());
                break;
            case DirectionEnum.Top:
                rightHand[2].startPos = new Vector3(-0.17f, 2.46f, -13.48f);
                StartCoroutine(rightHand[2].SetStart());
                break;
            case DirectionEnum.Right:
                rightHand[3].startPos = new Vector3(-13.4f, 2.16f, 0.2f);
                StartCoroutine(rightHand[3].SetStart());
                break;
        }

        yield return new WaitForSeconds(1);
        iTween.MoveBy(tableAnima, iTween.Hash("y", 1.8, "easeType", "easeOutCubic", "loopType", "none", "delay", 0.1));
        iTween.MoveBy(publicCards.gameObject,
            iTween.Hash("y", 1.8f, "easeType", "easeOutCubic", "loopType", "none", "delay", 0.1));
        yield return new WaitForSeconds(1.2f);
        for (int i = 0; i < holeNum1 + holeNum2; i++)
        {
            lastCardList.Add(publicCardList[0]);
            publicCardList.RemoveAt(0);
            lastCardList.Add(publicCardList[0]);
            publicCardList.RemoveAt(0);
        }

        yield return new WaitForSeconds(1);
        InitMyCardListAndOtherCard();
        yield return new WaitForSeconds(2f);
        isShowGlass = true;
    }

    public void TouZi()
    {
        SoundManager.Instance.PlaySoundBGM("touzi");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        holeOne.Play("first" + holeNum1);
        holeTwo.Play("second" + holeNum2);
    }

    /// <summary>
    /// 初始化玩家的手牌
    /// </summary>
    private void InitMyCardListAndOtherCard()
    {
        GameObject gob;
        for (int a = 0; a < mineList.Count; a++) //我的牌13张
        {
            if (mineList[a] > 0)
            {
                for (int b = 0; b < mineList[a]; b++)
                {
                    MyDebug.Log("CArd Point:" + a);
                    gob = GameResourceManager.Instance.CreateGameObjectAndReturn(a, parentList[0],
                        new Vector3(0, -5, 0), new Vector3(180, 0, 0));
                    if (gob != null) //
                    {
                        gob.GetComponent<BottomScript>().OnSendMessage += CardChange; //发送消息fd
                        gob.GetComponent<BottomScript>().ReSetPoisiton += CardSelect;
                        gob.GetComponent<BottomScript>().InitMyPoint(a); //设置指针
                        handerCardList[0].Add(gob); //增加游戏对象
                        MyDebug.Log("--> gob is Sucess"); //游戏对象为空
                    }
                    else
                    {
                        MyDebug.Log("--> gob is null"); //游戏对象为空
                    }
                }
            }
        }

        GameObject temp;
        int randomIndex;
        ///SetHandleLog();
        MyDebug.Log("--> InitMycard 11111111111111111");
        for (int i = 0; i < handerCardList[0].Count; i++)
        {
            temp = handerCardList[0][i];
            randomIndex = UnityEngine.Random.Range(0, handerCardList[0].Count);
            handerCardList[0][i] = handerCardList[0][randomIndex];
            handerCardList[0][randomIndex] = temp;
        }

        MyDebug.Log("--> InitMycard 222222222222222222222");
        if (bankerId == GlobalDataScript.loginResponseData.chairID)
        {
            StartCoroutine(PaiXu(false));
            MyDebug.Log("初始化数据自己为庄家");
        }
        else
        {
            MyDebug.Log("初始化数据其他人的庄家");
            StartCoroutine(PaiXu(false));
        }

        StartCoroutine(InitOtherCardList(1, 13));
        StartCoroutine(InitOtherCardList(2, 13));
        StartCoroutine(InitOtherCardList(3, 13));
        if (bankerId == GlobalDataScript.loginResponseData.chairID)
        {
            StartCoroutine(InitCardPos(true));
            // MoPai();
            MyDebug.Log("初始化数据自己为庄家");
        }
        else
        {
            GlobalDataScript.isBeginGame = true;
            MyDebug.Log("qitaren zhuang  jia ");
            StartCoroutine(OtherPickCardAndCreate(3.5f));
        }
    }

    /// <summary>
    /// 手牌排序
    /// </summary>
    /// <param name="flag"> </param>
    /// <returns></returns>
    IEnumerator PaiXu(bool flag = false)
    {
        myCardsCount = handerCardList[0].Count;
        MyDebug.Log("PaiXu 1111111111111");
        for (int i = 0; i < 13; i++)
        {
            if (i == 4 || i == 8 || i == 12)
            {
                yield return new WaitForSeconds(0.5f);
            }

            RemovePublicCards();
            if (flag)
                handerCardList[0][i].transform.localPosition =
                    new Vector3((i - (myCardsCount - 1) / 2.0f - PengGangList_B.Count - 0.5f) * spaceW, 0,
                        0); //从左到右依次对齐
            else
                handerCardList[0][i].transform.localPosition =
                    new Vector3((i - myCardsCount / 2.0f - PengGangList_B.Count - 0.5f) * spaceW, 0, 0); //从左到右依次对齐
            iTween.RotateBy(handerCardList[0][i],
                iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time",
                    0.5f)); //  iTween.RotateBy(handerCardList[0][i], iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
        }

        MyDebug.Log("PaiXu 2222222222222222");
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 13; i++)
        {
            iTween.RotateBy(handerCardList[0][i],
                iTween.Hash("x", -0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
        }

        yield return new WaitForSeconds(0.5f);
        MyDebug.Log("PaiXu 333333333333333333");
        GameObject t;
        GameObject temp;
        for (int i = 1; i < 13; ++i)
        {
            t = handerCardList[0][i];
            int j = i;
            while ((j > 0) && (handerCardList[0][j - 1].GetComponent<BottomScript>().cardPoint >
                               t.GetComponent<BottomScript>().cardPoint))
            {
                handerCardList[0][j] = handerCardList[0][j - 1];
                --j;
            }

            handerCardList[0][j] = t;
        }

        MyDebug.Log("PaiXu 444444444444444444444444444");
        for (int i = 1; i < 13; ++i)
        {
            if (handerCardList[0][i].GetComponent<BottomScript>().cardPoint == 33)
            {
                temp = handerCardList[0][i];

                handerCardList[0].RemoveAt(i);

                handerCardList[0].Insert(0, temp);
            }
        }

        MyDebug.Log("PaiXu 55555555555555555555555555");
        for (int i = 0; i < 13; i++)
        {
            if (flag)
                handerCardList[0][i].transform.localPosition =
                    new Vector3((i - (myCardsCount - 1) / 2.0f - PengGangList_B.Count - 0.5f) * spaceW, 0,
                        0); //从左到右依次对齐
            else
                handerCardList[0][i].transform.localPosition =
                    new Vector3((i - myCardsCount / 2.0f - PengGangList_B.Count - 0.5f) * spaceW, 0, 0); //从左到右依次对齐
        }

        MyDebug.Log("PaiXu 6666666666666666666666");
        for (int i = 0; i < 13; i++)
        {
            iTween.RotateBy(handerCardList[0][i],
                iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
        }

        MyDebug.Log("PaiXu 7777777777777777777777777777777777");
    }

    /// <summary>
    /// 选择要打出的牌
    /// </summary>
    /// <param name="obj">Object.</param>
    private void CardSelect(GameObject obj)
    {
        for (int i = 0; i < handerCardList[0].Count; i++)
        {
            if (handerCardList[0][i] == null)
            {
                handerCardList[0].RemoveAt(i);
                i--;
            }
            else if (handerCardList[0][i].transform.GetComponent<BottomScript>().isSelected)
            {
                handerCardList[0][i].transform.GetComponent<BottomScript>().SetOldPosition();
            }
        }
    }

    /// <summary>
    /// 自己打出牌处理
    /// </summary>
    /// <param name="obj">Object.</param>
    private void CardChange(GameObject obj)
    {
        Debug.Log(GlobalDataScript.isDrag);
        if (GlobalDataScript.isDrag)
        {
            if (GameMessageManager.ClosePointPrompt != null)
                GameMessageManager.ClosePointPrompt();
            GlobalDataScript.isDrag = false;
            var _card = obj.GetComponent<BottomScript>();
            _card.OnSendMessage -= CardChange;
            _card.ReSetPoisiton -= CardSelect;
            MyDebug.Log("card change over");
            var putOutCardPointTemp = obj.GetComponent<BottomScript>().GetPoint();
            mineList[putOutCardPointTemp]--;
            handerCardList[0].Remove(obj);
            Destroy(obj);
            curDirString = DirectionEnum.Bottom;
            RefreshCardPos();
            MyDebug.Log(putOutCardPointTemp + "--cardchange  goblist count = > " + handerCardList[0].Count);
            //========================================================================
            var outCard = new CMD_C_OutCard();
            outCard.cbCardData = (byte)MaJiangHelper.MaJiangCardToChange(putOutCardPointTemp);
            //SocketSendManager.Instance.SendData((int) GameServer.MDM_GF_GAME, (int) SUB_C.SUB_C_OUT_CARD,
            //    NetUtil.StructToBytes(outCard), Marshal.SizeOf(outCard));
            putOutCardPointAvarIndex = curListIndex;
            // SetHandleLog();
        }
    }

    /// <summary>
    /// 创建打来的的牌对象，并且开始播放动画
    /// </summary>
    /// <param name="cardPoint">Card point.</param>
    /// <param name="curAvatarIndex">Current avatar index.</param>
    /// <param name="st">todo: describe st parameter on CreatePutOutCardAndPlayAction</param>
    /// <param name="go">todo: describe go parameter on CreatePutOutCardAndPlayAction</param>
    private IEnumerator CreatePutOutCardAndPlayAction(int cardPoint, int curAvatarIndex, string st, GameObject go)
    {
        GlobalDataScript.putOutCount++;
        Vector3 majpos1;
        Vector3 majpos2;
        Vector3 majpos;
        Vector3 pos2;
        MyDebug.Log("put out cardPoint" + cardPoint);
        SoundManager.Instance.PlaySound(cardPoint, 0);
        putOutCardPoint = cardPoint;
        SelfAndOtherPutoutCard = cardPoint;
        DestroyPutOutCard(cardPoint);
        if (cardPoint == 33)
        {
            GlobalDataScript.putOutCount = 0;
        }

        if (st == DirectionEnum.Bottom)
            yield break;
        if (GlobalDataScript.putOutCount != 0 && GlobalDataScript.putOutCount < 4)
        {
            Destroy(go);
            yield break;
        }

        var random = new System.Random();
        var num = random.Next(0, handerCardList[curAvatarIndex].Count);
        Destroy(handerCardList[curAvatarIndex][num]);
        handerCardList[curAvatarIndex].RemoveAt(num);
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < handerCardList[curAvatarIndex].Count; i++)
        {
            if (i == handerCardList[curAvatarIndex].Count - 1)
            {
                handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().isFlyCard = false;
            }

            switch (st)
            {
                case DirectionEnum.Top: //上
                    if (handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().isFlyCard)
                    {
                        handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().isFlyCard = false;
                        rightHand[2].startPos = handerCardList[curAvatarIndex][i].transform.position +
                                                new Vector3(-4.4f, -3.65f, -9.7f);
                        rightHand[2].transform.eulerAngles = new Vector3(0f, -180f, 0);
                        rightHand[2].SetChaPai();
                        yield return new WaitForSeconds(0.2f);
                        majpos1 = handerCardList[curAvatarIndex][i].transform.position + new Vector3(0, 1.4f, 0);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos1, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        yield return new WaitForSeconds(0.3f);
                        pos2 = new Vector3((6.5f - i) * spaceW, 5.14f, 10.8f) + new Vector3(-4.4f, -3.65f, -9.7f);
                        iTween.MoveTo(rightHand[2].gameObject,
                            iTween.Hash("position", pos2, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        majpos2 = new Vector3((6.5f - i) * spaceW, 5.14f, 10.8f) + new Vector3(0, 1.4f, 0);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos2, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        yield return new WaitForSeconds(0.3f);
                        majpos = new Vector3((6.5f - i) * spaceW, 5.14f, 10.8f);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                    }
                    else
                    {
                        handerCardList[curAvatarIndex][i].GetComponent<BottomScript>()
                            .SetPosition(new Vector3((6.5f - i) * spaceW, 5.14f, 10.8f));
                    }

                    break;
                case DirectionEnum.Left: //左
                    if (handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().isFlyCard)
                    {
                        handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().isFlyCard = false;
                        rightHand[1].startPos = handerCardList[curAvatarIndex][i].transform.position +
                                                new Vector3(9.65f, -3.8f, -4.2f);
                        rightHand[1].transform.eulerAngles = new Vector3(0f, 90f, 0);
                        rightHand[1].SetChaPai();
                        yield return new WaitForSeconds(0.2f);
                        majpos1 = handerCardList[curAvatarIndex][i].transform.position + new Vector3(0, 1.4f, 0);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos1, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        yield return new WaitForSeconds(0.3f);
                        majpos2 = new Vector3(-12, 5.14f, (handerCardList[curAvatarIndex].Count / 2.0f - i) * spaceW) +
                                  new Vector3(0, 1.4f, 0);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos2, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        rightHand[1].transform.position += new Vector3(0, 1.4f, 0);
                        pos2 = new Vector3(-12, 5.14f, (handerCardList[curAvatarIndex].Count / 2.0f - i) * spaceW) +
                               new Vector3(9.65f, -2.4f, -4.2f);
                        iTween.MoveTo(rightHand[1].gameObject,
                            iTween.Hash("position", pos2, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        yield return new WaitForSeconds(0.3f);
                        majpos = new Vector3(-12, 5.14f, (handerCardList[curAvatarIndex].Count / 2.0f - i) * spaceW);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        var vector = rightHand[1].transform.position - new Vector3(0, 1.4f, 0);
                        iTween.MoveTo(rightHand[1].gameObject,
                            iTween.Hash("position", vector, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                    }
                    else
                    {
                        handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().SetPosition(new Vector3(-12,
                            5.14f, (handerCardList[curAvatarIndex].Count / 2.0f - i) * spaceW));
                    }

                    break;
                case DirectionEnum.Right: //右
                    if (handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().isFlyCard)
                    {
                        handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().isFlyCard = false;
                        rightHand[3].startPos = handerCardList[curAvatarIndex][i].transform.position +
                                                new Vector3(-9.6f, -3.31f, 4.3f);
                        rightHand[3].transform.eulerAngles = new Vector3(0f, -90f, 0);
                        rightHand[3].SetChaPai();
                        yield return new WaitForSeconds(0.2f);
                        majpos1 = handerCardList[curAvatarIndex][i].transform.position + new Vector3(0, 1.4f, 0);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos1, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        yield return new WaitForSeconds(0.3f);
                        pos2 = new Vector3(12, 5.14f, (i - handerCardList[curAvatarIndex].Count / 2.0f) * spaceW) +
                               new Vector3(-9.6f, -3.31f, 4.3f);
                        iTween.MoveTo(rightHand[3].gameObject,
                            iTween.Hash("position", pos2, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        majpos2 = new Vector3(12, 5.14f, (i - handerCardList[curAvatarIndex].Count / 2.0f) * spaceW) +
                                  new Vector3(0, 1.4f, 0);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos2, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                        yield return new WaitForSeconds(0.3f);
                        majpos = new Vector3(12, 5.14f, (i - handerCardList[curAvatarIndex].Count / 2.0f) * spaceW);
                        iTween.MoveTo(handerCardList[curAvatarIndex][i].gameObject,
                            iTween.Hash("position", majpos, "easeType", iTween.EaseType.easeInOutQuart, "loopType",
                                "none", "time", 0.3f));
                    }
                    else
                    {
                        handerCardList[curAvatarIndex][i].GetComponent<BottomScript>().SetPosition(new Vector3(12,
                            5.14f, (i - handerCardList[curAvatarIndex].Count / 2.0f) * spaceW));
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// 销毁出的牌
    /// </summary>
    /// <param name="cardPoint">todo: describe cardPoint parameter on DestroyPutOutCard</param>
    private void DestroyPutOutCard(int cardPoint)
    {
        ThrowBottom(cardPoint);
        if (curDirString != DirectionEnum.Bottom)
        {
            gangKind = 0;
        }
    }

    /// <summary>
    /// 所有玩家打出牌的处理
    /// </summary>
    /// <param name="index"></param>
    private void ThrowBottom(int index)
    {
        var poisVector3 = Vector3.one;
        var rotationV3 = Vector3.zero;
        switch (curDirString)
        {
            case DirectionEnum.Bottom:
                poisVector3 = new Vector3((-2.5f + tableCardList[0].Count % 6) * spaceW, 0,
                    -(int) (tableCardList[0].Count / 6) * spaceL);
                GlobalDataScript.isDrag = false;
                rotationV3 = Vector3.zero;
                break;
            case DirectionEnum.Right:
                rotationV3 = new Vector3(0, 270, 0);
                poisVector3 = new Vector3(tableCardList[1].Count / 6 * spaceL, 0,
                    (-2.5f + tableCardList[1].Count % 6) * spaceW);
                break;
            case DirectionEnum.Top:
                rotationV3 = new Vector3(0, 180, 0);
                poisVector3 = new Vector3((2.5f - tableCardList[2].Count % 6) * spaceW, 0,
                    (int) (tableCardList[2].Count / 6) * spaceL);
                break;
            case DirectionEnum.Left:
                rotationV3 = new Vector3(0, 90, 0);
                poisVector3 = new Vector3(-tableCardList[3].Count / 6 * spaceL, 0,
                    (2.5f - tableCardList[3].Count % 6) * spaceW);
                break;
        }

        switch (curDirString)
        {
            case DirectionEnum.Bottom:
                rightHand[0].startPos = new Vector3(-1.8f + (tableCardList[0].Count % 6 * 1.2f * spaceW), 4.05f,
                    -0.22f - ((int) (tableCardList[0].Count / 6 * spaceL * 1.2f)));
                StartCoroutine(rightHand[0].SetPut2(index, outparentList[curListIndex].gameObject, poisVector3));
                break;
            case DirectionEnum.Right:
                rightHand[3].startPos = new Vector3(-3.45f + tableCardList[1].Count / 6 * spaceL * 1.2f, 1,
                    -3.72f + tableCardList[1].Count % 6 * spaceW * 1.2f);
                StartCoroutine(rightHand[3].SetPut(index, outparentList[curListIndex].gameObject, poisVector3));
                break;
            case DirectionEnum.Top:
                rightHand[2].startPos = new Vector3(2.29f - tableCardList[2].Count % 6 * spaceW * 1.2f, 1.31f,
                    -4.56f + (int) (tableCardList[2].Count / 6) * spaceL * 1.2f);
                StartCoroutine(rightHand[2].SetPut(index, outparentList[curListIndex].gameObject, poisVector3));
                break;
            case DirectionEnum.Left:
                rightHand[1].startPos = new Vector3(0.74f - tableCardList[3].Count / 6 * spaceL * 1.2f, 3.98f,
                    0.63f - tableCardList[3].Count % 6 * spaceW * 1.2f);
                StartCoroutine(rightHand[1].SetPut2(index, outparentList[curListIndex].gameObject, poisVector3));
                break;
        }
    }

    /// <summary>
    /// 设置自己庄时，首次摸牌
    /// </summary>
    /// <param name="flag"></param>
    IEnumerator InitCardPos(bool flag = false)
    {
        yield return new WaitForSeconds(3);
        GlobalDataScript.isBeginGame = true;
        MoPai();

        //myCardsCount = handerCardList[0].Count;
        //MyDebug.TestLog("myCardsCount:" + myCardsCount);
        //if (flag)
        //{
        //    yield return new WaitForSeconds(3.5f);
        //    RemovePublicCards();
        //    handerCardList[0][myCardsCount - 1].transform.localEulerAngles = new Vector3(-40, 270, 90);
        //    handerCardList[0][myCardsCount - 1].transform.localPosition = new Vector3(((myCardsCount - 1) / 2.0f - PengGangList_B.Count) * spaceW, 2, 0);
        //    PickCardAnima(handerCardList[0][myCardsCount - 1], new Vector3(((myCardsCount - 1) / 2.0f - PengGangList_B.Count) * spaceW + 550, 0, 0), new Vector3(-90, 270, 90));
        //    var go = handerCardList[0][myCardsCount - 1];
        //    handerCardList[0].Remove(handerCardList[0][myCardsCount - 1]);
        //    InsertCardIntoList(go);
        //}
    }

    private void RefreshCardPos(bool flag = false)
    {
        switch (curDirString)
        {
            case DirectionEnum.Bottom:
                myCardsCount = handerCardList[0].Count;
                for (int i = 0; i < myCardsCount; i++)
                {
                    if (handerCardList[0][i] == null)
                    {
                        MyDebug.Log("insertCardIntoList:" + i);
                    }

                    if (i == myCardsCount - 1)
                    {
                        handerCardList[0][i].GetComponent<BottomScript>().isFlyCard = false;
                    }

                    if (flag && i == myCardsCount - 1)
                    {
                        handerCardList[0][i].GetComponent<BottomScript>().SetPosition(
                            new Vector3((i - myCardsCount / 2.0f - PengGangList_B.Count) * spaceW + 550, 0, 0));
                    }
                    else
                    {
                        handerCardList[0][i].GetComponent<BottomScript>().SetPosition(
                            new Vector3((i - myCardsCount / 2.0f - PengGangList_B.Count - 0.5f) * spaceW + 550, 0, 0));
                    }
                }

                break;
            case DirectionEnum.Right:
                for (int i = 0; i < handerCardList[1].Count; i++)
                {
                    if (handerCardList[1][i] == null)
                    {
                        MyDebug.Log("insertCardIntoList:" + i);
                    }

                    if (i == handerCardList[1].Count - 1)
                    {
                        handerCardList[1][i].GetComponent<BottomScript>().isFlyCard = false;
                    }

                    if (flag && i == handerCardList[1].Count - 1)
                    {
                        handerCardList[1][i].GetComponent<BottomScript>().SetPosition(new Vector3(12, 5.14f,
                            (i - handerCardList[1].Count / 2.0f + 0.5f) * spaceW));
                    }
                    else
                    {
                        handerCardList[1][i].GetComponent<BottomScript>()
                            .SetPosition(new Vector3(12, 5.14f, (i - handerCardList[1].Count / 2.0f) * spaceW));
                    }
                }

                break;
            case DirectionEnum.Top:
                for (int i = 0; i < handerCardList[2].Count; i++)
                {
                    if (handerCardList[2][i] == null)
                    {
                        MyDebug.Log("insertCardIntoList:" + i);
                    }

                    if (i == handerCardList[2].Count - 1)
                    {
                        handerCardList[2][i].GetComponent<BottomScript>().isFlyCard = false;
                    }

                    if (flag && i == handerCardList[2].Count - 1)
                    {
                        handerCardList[2][i].GetComponent<BottomScript>()
                            .SetPosition(new Vector3((6.5f - i - 0.5f) * spaceW, 5.14f, 10.8f));
                    }
                    else
                    {
                        handerCardList[2][i].GetComponent<BottomScript>()
                            .SetPosition(new Vector3((6.5f - i) * spaceW, 5.14f, 10.8f));
                    }
                }

                break;
            case DirectionEnum.Left:
                for (int i = 0; i < handerCardList[3].Count; i++)
                {
                    if (handerCardList[3][i] == null)
                    {
                        MyDebug.Log("insertCardIntoList:" + i);
                    }

                    if (i == handerCardList[3].Count - 1)
                    {
                        handerCardList[3][i].GetComponent<BottomScript>().isFlyCard = false;
                    }

                    if (flag && i == handerCardList[3].Count - 1)
                    {
                        handerCardList[3][i].GetComponent<BottomScript>().SetPosition(new Vector3(-12, 5.14f,
                            (handerCardList[3].Count / 2.0f - i - 0.5f) * spaceW));
                    }
                    else
                    {
                        handerCardList[3][i].GetComponent<BottomScript>()
                            .SetPosition(new Vector3(-12, 5.14f, (handerCardList[3].Count / 2.0f - i) * spaceW));
                    }
                }

                break;
        }
    }

    Transform trans;

    private void PickCardAnima(GameObject obj, Vector3 pos, Vector3 rota)
    {
        iTween.MoveTo(obj,
            iTween.Hash("position", pos, "easeType", iTween.EaseType.easeInOutQuart, "loopType", "none", "time", 0.5f));
        iTween.RotateTo(obj,
            iTween.Hash("rotation", rota, "easeType", iTween.EaseType.easeOutCubic, "loopType", "none", "time", 0.2f));
        var got = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
        got.transform.parent = obj.transform;
        switch (curDirString)
        {
            case DirectionEnum.Left:
                got.transform.localEulerAngles = new Vector3(0, 0, 0);
                got.transform.localPosition = new Vector3(0.04f, 0.07f, -0.73f);
                got.transform.localScale = new Vector3(0.08f, 0.132f, 0.180f);
                break;
            case DirectionEnum.Right:
                got.transform.localEulerAngles = new Vector3(0, 0, 0);
                got.transform.localPosition = new Vector3(-0.04f, 0.07f, -0.73f);
                got.transform.localScale = new Vector3(0.08f, 0.132f, 0.180f);
                break;
            case DirectionEnum.Top:
                got.transform.localPosition = new Vector3(0, 0, -0.73f);
                got.transform.localEulerAngles = new Vector3(0, 0, 0);
                got.transform.localScale = new Vector3(0.082f, 0.132f, 0.180f);
                break;
        }
    }

    IEnumerator PutOutCardAnima()
    {
        yield return new WaitForEndOfFrame();
    }

    /// <summary>
    /// 最后一张牌上的顶针实现
    /// </summary>
    /// <param name="parent"></param>
    public void SetPointGameObject(GameObject parent)
    {
        if (parent != null)
        {
            iTween.StopByName(Pointertemp, "PointMove");
            Pointertemp.transform.position = parent.transform.position + new Vector3(0, 2, 0);
        }
    }

    private void InitPointTween()
    {
        Pointertemp.transform.position = Vector3.zero;
    }

    /// <summary>
    /// 初始化其他玩家的牌
    /// </summary>
    /// <param name="initDirection"></param>
    IEnumerator InitOtherCardList(int initDiretion, int count)
    {
        GameObject temp;
        MyDebug.Log("InitOtherCard Diretion:" + initDiretion);
        for (int i = 0; i < count; i++)
        {
            if (i == 4 || i == 8 || i == 12)
            {
                yield return new WaitForSeconds(0.5f);
            }

            temp = GameResourceManager.Instance.CreateGameObjectAndReturn(0, parentList[initDiretion], Vector3.zero,
                Vector3.zero); //实例化当前牌
            RemovePublicCards();
            if (temp != null) //有可能没牌了
            {
                switch (initDiretion)
                {
                    case 2: //上
                        temp.transform.localEulerAngles = new Vector3(0, 0, 180);
                        temp.transform.localPosition = new Vector3(6.5f - i * spaceW, 0); //位置
                        iTween.RotateBy(temp,
                            iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                        handerCardList[2].Add(temp);
                        break;
                    case 3: //左
                        temp.transform.localEulerAngles = new Vector3(180, 90, 0);
                        temp.transform.localPosition = new Vector3(0, 0, -(i - count / 2.0f) * spaceW); //位置
                        temp.transform.SetSiblingIndex(0);
                        iTween.RotateBy(temp,
                            iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                        handerCardList[3].Add(temp);
                        break;
                    case 1: //右
                        temp.transform.localEulerAngles = new Vector3(180, 270, 0);
                        temp.transform.localPosition = new Vector3(0, 0, (i - count / 2.0f) * spaceW); //位置
                        iTween.RotateBy(temp,
                            iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                        handerCardList[1].Add(temp);
                        break;
                }
            }
        }

        MyDebug.Log("111111111111111111111111111111111InitOtherCard Diretion:" + initDiretion);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < count; i++)
        {
            switch (initDiretion)
            {
                case 2: //上
                    iTween.RotateBy(handerCardList[2][i],
                        iTween.Hash("x", -0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                    break;
                case 3: //左
                    iTween.RotateBy(handerCardList[3][i],
                        iTween.Hash("x", -0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                    break;
                case 1: //右
                    iTween.RotateBy(handerCardList[1][i],
                        iTween.Hash("x", -0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                    break;
            }
        }

        MyDebug.Log("2222222222222222222222222222222222InitOtherCard Diretion:" + initDiretion);
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < count; i++)
        {
            var got = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
            switch (initDiretion)
            {
                case 2: //上
                    iTween.RotateBy(handerCardList[2][i],
                        iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                    got.transform.parent = handerCardList[2][i].transform;
                    got.transform.localPosition = new Vector3(0, 0, -0.73f);
                    got.transform.localEulerAngles = new Vector3(0, 0, 0);
                    got.transform.localScale = new Vector3(0.082f, 0.132f, 0.180f);
                    break;
                case 3: //左
                    iTween.RotateBy(handerCardList[3][i],
                        iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                    got.transform.parent = handerCardList[3][i].transform;
                    got.transform.localPosition = new Vector3(0.04f, 0.07f, -0.73f);
                    got.transform.localEulerAngles = new Vector3(0, 0, 0);
                    got.transform.localScale = new Vector3(0.08f, 0.132f, 0.180f);
                    break;
                case 1: //右
                    iTween.RotateBy(handerCardList[1][i],
                        iTween.Hash("x", 0.25f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.5f));
                    got.transform.parent = handerCardList[1][i].transform;
                    got.transform.localPosition = new Vector3(-0.04f, 0.07f, -0.73f);
                    got.transform.localEulerAngles = new Vector3(0, 0, 0);
                    got.transform.localScale = new Vector3(0.08f, 0.132f, 0.180f);
                    break;
            }
        }

        MyDebug.Log("3333333333333333333333333333333InitOtherCard Diretion:" + initDiretion);
    }

    /// <summary>
    /// 创建其他人摸的牌
    /// </summary>
    /// <param name="waitTime">todo: describe waitTime parameter on OtherPickCardAndCreate</param>
    private IEnumerator OtherPickCardAndCreate(float waitTime = 0)
    {
        MyDebug.Log("其他人莫牌： 111111111111");
        yield return new WaitForSeconds(waitTime);
        var tempVector3 = new Vector3(0, 0);
        var tempRotation = Vector3.zero;
        var rota = Vector3.zero;
        var pos = Vector3.zero;
        switch (curDirString)
        {
            case DirectionEnum.Top: //上
                tempVector3 = new Vector3(6.5f - handerCardList[2].Count - 0.5f, 2f);
                tempRotation = new Vector3(-40, 90, 90);
                rota = new Vector3(-90, 90, 90);
                pos = new Vector3(6.5f - handerCardList[2].Count - 0.5f, 5.14f, 10.8f);
                break;
            case DirectionEnum.Left: //左
                tempVector3 = new Vector3(0, 2, -handerCardList[3].Count / 2.0f - 0.5f);
                tempRotation = new Vector3(-40, 0, 90);
                rota = new Vector3(-90, 0, 90);
                pos = new Vector3(-12, 5.14f, -handerCardList[3].Count / 2.0f - 0.5f);
                break;
            case DirectionEnum.Right: //右
                tempVector3 = new Vector3(0, 2, handerCardList[1].Count / 2.0f + 0.5f);
                tempRotation = new Vector3(-40, 180, 90);
                rota = new Vector3(-90, 180, 90);
                pos = new Vector3(12, 5.14f, handerCardList[1].Count / 2.0f + 0.5f);
                break;
        }

        MyDebug.Log("其他人莫牌： 22222222222222222");
        if (isGang)
        {
            yield return new WaitForSeconds(0.6f);
            CheckGangRemove();
            gangCount++;
        }
        else
        {
            RemovePublicCards();
        }

        MyDebug.Log("其他人莫牌： 33333333333333333333333");
        isGang = false;
        otherPickCardItem =
            GameResourceManager.Instance.CreateGameObjectAndReturn(0, parentList[curListIndex], tempVector3,
                tempRotation);
        PickCardAnima(otherPickCardItem, pos, rota);
        if (GlobalDataScript.putOutCount < 3)
        {
            yield break;
        }

        var i = UnityEngine.Random.Range(0, handerCardList[curListIndex].Count);
        if (i < handerCardList[curListIndex].Count)
        {
            otherPickCardItem.GetComponent<BottomScript>().isFlyCard = true;
            handerCardList[curListIndex].Insert(i, otherPickCardItem); //在
        }
        else
        {
            handerCardList[curListIndex].Add(otherPickCardItem); //在
        }

        MyDebug.Log("其他人莫牌： 444444444444444444444444444444444444");

      
    }

    /// <summary>
    /// 游戏进行局数
    /// </summary>
    private void NumberOfGame()
    {
        LeavedRoundNumText.text = "第" + GlobalDataScript.roomVo.dwPlayCout + "/" + GlobalDataScript.roomVo.dwPlayTotal + "局";
    }
}