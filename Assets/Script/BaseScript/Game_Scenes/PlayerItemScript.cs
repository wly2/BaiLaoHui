using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AssemblyCSharp;
using System.Collections.Generic;
using LitJson;
using System;

public class PlayerItemScript : MonoBehaviour
{
    public PlayerGameRoomInfo myselfInfo;
    //==============单例================//
    private int myscore;
    public HomePanelScript homepanelScript;
    private List<TalkItemData> _list;
    public Image headerIcon;
    public Image bankerImg;
    public Text nameText;
    public Image readyImg;
    public Text scoreText;
    public string dir;
    public GameObject chatAction;
    public Image offlineImage; //离线图片
    public Text chatMessage;
    public Image emoticons;
    public GameObject chatPaoPao;
    public GameObject HuFlag;
    public AvatarVO avatarvo;
    private int showTime;
    private int showChatTime;
    private Sprite normalIcon;
    public List<Image> shouPai;
    public Transform[] type;
    public Transform[] typetext;
    public Text[] comparText;
    public Image[] compareType;
    public Image tip;
    public Image img_Banker;//庄家
    public Image oweroom;//房主
    public Text txt_xiazhuNum;//下注倍数
    public string testmmm;
    public Image typeNN;
    public GameObject toudaotransform;
    public GameObject zhongdaotransform;
    public GameObject weidaotransform;
    public GameObject backcard;
    public Image comparereadly;
    public Image complete;
    public List<Image> cardList = new List<Image>();
    public List<ShowMaPuke> tdMalist = new List<ShowMaPuke>();
    public List<ShowMaPuke> zdMalist = new List<ShowMaPuke>();
    public List<ShowMaPuke> wdMalist = new List<ShowMaPuke>();
    public List<Image> tdlist = new List<Image>();
    public List<Image> zdlist = new List<Image>();
    public List<Image> wdlist = new List<Image>();
    public GameObject gun;
    public Image dankong1;
    public Image dankong2;
    public Image mapai;
    public List<int> gunscorelist = new List<int>();
    private int allscore = 0;
    Color mcolor = new Color(0, 0, 0, 0);
    Vector3 euler = new Vector3(0, 0, 0);
    public int gameStatus = -1;
    public Image isQZ;
    public Image aniQZ;

    bool isLoad;
    float loadTime = 0;
    private void Awake()
    {
        if (typeNN != null)
            typeNN.enabled = false;

        normalIcon = headerIcon.sprite;
    }
    void Update()
    {
        if (showTime > 0)
        {
            showTime--;
            if (showTime == 0)
            {
                emoticons.gameObject.SetActive(false);
                chatPaoPao.SetActive(false);
            }
        }

        if (showChatTime > 0)
        {
            showChatTime--;
            if (showChatTime == 0)
            {
                chatAction.SetActive(false);
            }
        }
        if (isLoad)
        {
            loadTime += Time.deltaTime;
            if (loadTime > 5)
            {
                isLoad = false;
                loadTime = 0;
            }

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //  StartCoroutine(LoadImg());
        }
        if (myselfInfo == null)
            return;
        if (!string.IsNullOrEmpty(myselfInfo.userHeadUrl) && !isLoad)
        {
            if (headerIcon.sprite == normalIcon)
            {
                StartCoroutine(LoadImg());
            }
        }
    }
    public void ShowBackCards(int chairId)
    {
        if (chairId != myselfInfo.chairId)
            return;
        comparereadly.gameObject.SetActive(true);
        complete.gameObject.SetActive(false);

    }
    public void SetPlayStatus()
    {
        gameStatus = GameDataSSS.Instance.gameStatusInfo.bFinishSegment[myselfInfo.chairId];
        comparereadly.gameObject.SetActive(gameStatus == 1);
        complete.gameObject.SetActive(gameStatus != 1);
    }
    public void setTouDao()
    {
        for (int i = 0; i < 3; i++)
        {
            if (GlobalDataScript.Instance.roomInfo.maPaiId > 0)
            {
                int a = NetUtil.PuCardChange((PU_KE)GameDataSSS.Instance.sCompare.cbFrontCard[myselfInfo.chairId].arrayItem[i]);
                if (a == GlobalDataScript.Instance.roomInfo.maPaiId)
                {
                    tdMalist[i].mapai.gameObject.SetActive(true);
                }
                else
                {
                    tdMalist[i].mapai.gameObject.SetActive(false);
                }
            }
            ResourcesLoader.Load<Sprite>("PuKe/card_" + NetUtil.PuCardChange((PU_KE)GameDataSSS.Instance.sCompare.cbFrontCard[myselfInfo.chairId].arrayItem[i]), (sprite) =>
            {
                tdlist[i].enabled = true;
                tdlist[i].sprite = sprite;
            });
        }


        ShowType(GameDataSSS.Instance.sCompare.w_hand_card_type[myselfInfo.chairId].arrayItem[0], 0);
        ShowFront(GameDataSSS.Instance.sCompare.nCompareResult[myselfInfo.chairId].arrayItem[0], 0);






    }

    public void setZhongDao()
    {

        for (int i = 0; i < 5; i++)
        {
            if (GlobalDataScript.Instance.roomInfo.maPaiId > 0)
            {
                int a = NetUtil.PuCardChange((PU_KE)GameDataSSS.Instance.sCompare.cbMidCard[myselfInfo.chairId].arrayItem[i]);
                if (a == GlobalDataScript.Instance.roomInfo.maPaiId)
                {
                    zdMalist[i].mapai.gameObject.SetActive(true);
                }
                else
                {
                    zdMalist[i].mapai.gameObject.SetActive(false);
                }
            }
            ResourcesLoader.Load<Sprite>("PuKe/card_" + NetUtil.PuCardChange((PU_KE)GameDataSSS.Instance.sCompare.cbMidCard[myselfInfo.chairId].arrayItem[i]), (sprite) =>
            {
                zdlist[i].enabled = true;
                zdlist[i].sprite = sprite;
            });

        }

        ShowType(GameDataSSS.Instance.sCompare.w_hand_card_type[myselfInfo.chairId].arrayItem[1], 1);
        ShowFront(GameDataSSS.Instance.sCompare.nCompareResult[myselfInfo.chairId].arrayItem[1], 1);
    }
    public void setWeiDao()
    {
        for (int i = 0; i < 5; i++)
        {
            if (GlobalDataScript.Instance.roomInfo.maPaiId > 0)
            {
                int a = NetUtil.PuCardChange((PU_KE)GameDataSSS.Instance.sCompare.cbBackCard[myselfInfo.chairId].arrayItem[i]);
                if (a == GlobalDataScript.Instance.roomInfo.maPaiId)
                {
                    wdMalist[i].mapai.gameObject.SetActive(true);
                }
                else
                {
                    wdMalist[i].mapai.gameObject.SetActive(false);
                }
            }
            ResourcesLoader.Load<Sprite>("PuKe/card_" + NetUtil.PuCardChange((PU_KE)GameDataSSS.Instance.sCompare.cbBackCard[myselfInfo.chairId].arrayItem[i]), (sprite) =>
            {
                wdlist[i].enabled = true;
                wdlist[i].sprite = sprite;
            });

        }



        ShowType(GameDataSSS.Instance.sCompare.w_hand_card_type[myselfInfo.chairId].arrayItem[2], 2);
        ShowFront(GameDataSSS.Instance.sCompare.nCompareResult[myselfInfo.chairId].arrayItem[2], 2);

        gunscorelist.Add(GameDataSSS.Instance.sCompare.nCompareResult[myselfInfo.chairId].arrayItem[0]);
        gunscorelist.Add(GameDataSSS.Instance.sCompare.nCompareResult[myselfInfo.chairId].arrayItem[1]);
        gunscorelist.Add(GameDataSSS.Instance.sCompare.nCompareResult[myselfInfo.chairId].arrayItem[2]);
        if (GameDataSSS.Instance.sCompare.wSpecialType[myselfInfo.chairId] != 0)
        {

            for (int i = 0; i < compareType.Length; i++)
            {
                compareType[i].enabled = false;
            }
            for (int i = 0; i < comparText.Length; i++)
            {
                comparText[i].enabled = false;
            }
            ShowSpeicalFont((int)GameDataSSS.Instance.sCompare.nSpecialCompareResult[myselfInfo.chairId]);
            ShowSpeicalType(GameDataSSS.Instance.sCompare.wSpecialType[myselfInfo.chairId]);

        }
    }


    //初始化信息
    public void Init()
    {
        for (int i = 0; i < 5; i++)
        {
            shouPai[i].color = mcolor;
            shouPai[i].transform.localEulerAngles = euler;
        }
        //tip.gameObject.SetActive(false);
    }

    //庄家
    public void ShowBanker(int num)
    {
        if (num <= 0)
            num = 1;
        txt_xiazhuNum.text = "x " + num;
        img_Banker.gameObject.SetActive(true);
    }

    public void HideBanker()
    {
        img_Banker.gameObject.SetActive(false);
    }

    //抢庄
    public void ShowQZ()
    {
        ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/QiangZhuang/qz", (sprite) =>
        {
            isQZ.sprite = sprite;
        });
        StartCoroutine(HideQZ());
        isQZ.gameObject.SetActive(true);
    }

    //不抢庄
    public void NoshowQZ()
    {
        ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/QiangZhuang/noqz", (sprite) =>
        {
            isQZ.sprite = sprite;
        });
        StartCoroutine(HideQZ());
        isQZ.gameObject.SetActive(true);
    }

    IEnumerator HideQZ()
    {
        yield return new WaitForSeconds(2f);
        isQZ.gameObject.SetActive(false);

    }

    //房主
    public void ShowRoomer()
    {
        oweroom.gameObject.SetActive(true);
    }

    public void HideRoomer()
    {
        oweroom.gameObject.SetActive(false);
    }

    public void ShowAniQz()
    {
        aniQZ.gameObject.SetActive(true);
        Invoke("HideAniQz", 1f);
    }

    public void HideAniQz()
    {
        aniQZ.gameObject.SetActive(false);
    }

    public void ShowNN()
    {
        if (shouPai == null || shouPai.Count <= 0)
            return;
        if (GameDataNN.Instance.HandCard.Equals(null) || GameDataNN.Instance.HandCard.cbCardData == null || GameDataNN.Instance.HandCard.cbCardData.Length <= 0)
            return;
        int allCard = 0;
        for (int i = 0; i < 5; i++)
        {
            //shouPai[i].color = mcolor;
            int carid = NetUtil.PuCardChange((PU_KE)GameDataNN.Instance.HandCard.cbCardData[myselfInfo.chairId].CardData[i]);
            ResourcesLoader.Load<Sprite>("PuKe/card_" + carid, (sprite) =>
            {
                shouPai[i].sprite = sprite;
                shouPai[i].transform.localEulerAngles = euler;
                if (carid % 13 < 10 && carid < 53)
                {
                    allCard += carid % 13;
                }
            });
        }
        SetPuKType(GameDataNN.Instance.gameEnd.wSpecialType[myselfInfo.chairId], allCard);
        MyDebug.Log("================游戏结束手牌数据===============");
    }
    public void ShowFront(int textresult, int i)
    {
        comparText[i].enabled = true;
        if (textresult > 0)
        {
            comparText[i].text = "+" + textresult.ToString();
        }
        else
        {
            comparText[i].text = textresult.ToString();
        }

    }
    //显示特殊牌型

    public void ShowSpeicalType(int type)
    {
        compareType[3].enabled = true;
        switch (type)
        {

            case 1:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/yitiaolong", (sprite) =>
                {
                    compareType[3].overrideSprite = sprite;
                });
                break;
            case 2:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/threetonghua", (sprite) =>
                {
                    compareType[3].overrideSprite = sprite;
                });
                break;
            case 4:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/threeshunzi", (sprite) =>
                {
                    compareType[3].overrideSprite = sprite;
                });
                break;
            case 8:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/sixhlaf", (sprite) =>
                {
                    compareType[3].overrideSprite = sprite;
                });
                break;
            case 16:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/qinglong", (sprite) =>
                {
                    compareType[3].overrideSprite = sprite;
                });
                break;
        }
    }
    public void ShowSpeicalFont(int textresult)
    {
        comparText[3].enabled = true;
        if (textresult > 0)
        {
            comparText[3].text = "+" + textresult.ToString();
        }
        else
        {
            comparText[3].text = textresult.ToString();
        }
    }

    public void ShowType(ushort type, int i)
    {

        compareType[i].enabled = true;

        switch ((SUB_S_COMMONCARD)type)
        {
            case SUB_S_COMMONCARD.HT_INVALID:
                break;
            case SUB_S_COMMONCARD.HT_SINGLE:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/wulong", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_ONE_DOUBLE:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/duizi", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_TWO_DOUBLE:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/liangdui", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_THREE:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/santiao", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_LINE:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/shunzi", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_COLOR:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/tonghua", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_THREE_DEOUBLE:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/hulu", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_FOUR_BOOM:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/tiezhi", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_LINE_COLOR:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/tonghuashun", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            case SUB_S_COMMONCARD.HT_FIVE:
                ResourcesLoader.Load<Sprite>("ShiSanShuiAssets/Texture/UI/PeiPai/wutong", (sprite) =>
                {
                    compareType[i].overrideSprite = sprite;
                });
                break;
            default:
                break;
        }


    }


    //牛牛牌型
    public void SetPuKType(ushort type, int cardNum)
    {
        MyDebug.Log(myselfInfo.chairId + "--------------------" + type + "----" + cardNum);


        if ((type == (ushort)SUB_C_SPCIALCARD.TYPE_NULL))
        {
            testmmm = "noneNN";
            ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/UI/GameInfo/noneNN", (sprite) =>
            {
                typeNN.overrideSprite = sprite;
                typeNN.enabled = true;
            });
        }
        if ((type == (ushort)SUB_C_SPCIALCARD.TYPE_NIU) && cardNum % 10 != 0)
        {
            testmmm = "niu:" + cardNum % 10;

            ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/UI/GameInfo/niu_" + cardNum % 10, (sprite) =>
             {
                 typeNN.overrideSprite = sprite;
                 typeNN.enabled = true;
             });
        }
        if ((type == (ushort)SUB_C_SPCIALCARD.TYPE_NIUNIU))
        {
            testmmm = "NN";
            ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/UI/GameInfo/niuniu", (sprite) =>
            {
                typeNN.overrideSprite = sprite;
                typeNN.enabled = true;
            });
        }
        if ((type == (ushort)SUB_C_SPCIALCARD.TYPE_SIHUA))
        {
            testmmm = "sihua";
            ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/UI/GameInfo/sihua", (sprite) =>
            {
                typeNN.enabled = true;
                typeNN.overrideSprite = sprite;
            });
        }
        if ((type == (ushort)SUB_C_SPCIALCARD.TYPE_WUHUA))
        {
            ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/UI/GameInfo/wuhuaniu", (sprite) =>
            {
                typeNN.enabled = true;
                typeNN.overrideSprite = sprite;
            });
        }
        if ((type == (ushort)SUB_C_SPCIALCARD.TYPE_SIZHADAN))
        {
            ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/UI/GameInfo/boom", (sprite) =>
            {
                typeNN.enabled = true;
                typeNN.overrideSprite = sprite;
            });
        }
        if ((type == (ushort)SUB_C_SPCIALCARD.TYPE_WUXIAO))
        {
            ResourcesLoader.Load<Sprite>("NiuNiuAssets/Texture/UI/GameInfo/wuxiaoniu", (sprite) =>
            {
                typeNN.enabled = true;
                typeNN.overrideSprite = sprite;
            });
        }
    }

    public void SetExit(int userId = 0)
    {

        MyDebug.Log("SetExit.................");
        headerIcon.preserveAspect = false;
        if (userId == 0)
        {
            nameText.text = "";
            headerIcon.sprite = normalIcon;
            scoreText.text = "";
            headerIcon.preserveAspect = true;
            myselfInfo = null;
            return;
        }

        if (myselfInfo == null)
            return;
        if (myselfInfo.userID != userId)
            return;
        nameText.text = "";
        headerIcon.sprite = normalIcon;
        scoreText.text = "";
        headerIcon.preserveAspect = true;
        myselfInfo = null;
    }


    public void SetAvatarVo(AvatarVO value)
    {
        if (value != null)
        {
            avatarvo = value;
            readyImg.enabled = avatarvo.isReady;
            nameText.text = avatarvo.account.uuid + "";

            nameText.text = nameText.text.Substring(0, 6).ToString() + "..";

            scoreText.text = avatarvo.scores + "";
            if (dir != "B")
            {

            }
        }
        else
        {
            headerIcon.preserveAspect = false;
            nameText.text = "";
            readyImg.enabled = false;
            bankerImg.enabled = false;
            scoreText.text = "";
            ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/UI_Public/head0", (sprite) =>
            {
                headerIcon.overrideSprite = sprite;
                SetHeadSprite();
            });

            //headerIcon.sprite = Resources.Load("Image/morentouxiang", typeof(Sprite)) as Sprite;
            headerIcon.preserveAspect = true;
        }
    }

    #region 无效的加载路径
    public Sprite tempSp;

    /// <summary>
    /// 加载头像
    /// </summary>
    /// <returns>The image.</returns>
    private IEnumerator LoadImg()
    {
        isLoad = true;
        //if (tempSp == null)
        //{
        var www = new WWW(myselfInfo.userHeadUrl);
        yield return www;
        isLoad = false;
        loadTime = 0;
        //下载完成，保存图片到路径filePath
        if (www != null)
        {
            headerIcon.preserveAspect = false;
            var texture2D = www.texture;
            //var bytes = texture2D.EncodeToPNG();
            //将图片赋给场景上的Sprite
            tempSp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
            headerIcon.sprite = tempSp;
            headerIcon.preserveAspect = true;
            SetHeadSprite();
        }
        else
        {
            MyDebug.Log("没有加载到图片");
        }

        //}else
        //{
        //    headerIcon.sprite = tempSp;
        //    headerIcon.preserveAspect = true;
        //    SetHeadSprite();
        //}
        //开始下载图片

    }
    #endregion

    public void SetbankImgEnable(bool flag)
    {
        bankerImg.enabled = flag;
    }

    public void ShowChatAction()
    {
        showChatTime = 120;
        chatAction.SetActive(true);
    }

    public int GetUuid()
    {
        var result = -1;
        if (avatarvo != null)
        {
            result = avatarvo.account.uuid;
        }

        return result;
    }

    /*设置游戏玩家离线*/
    public void SetPlayerOffline()
    {
        offlineImage.transform.gameObject.SetActive(true);
    }

    /*设置游戏玩家上线*/
    public void SetPlayerOnline()
    {
        offlineImage.transform.gameObject.SetActive(false);
    }

    public void ShowChatMessage(int index)
    {
        MyDebug.Log("-----------------------------------");
        showTime = 200;
        index = index - 1001;
        chatMessage.text = TalkDataManager.Instance.list[index].message;
        chatPaoPao.SetActive(true);
    }

    public void ShowChat(string text)
    {
        if (_list == null || _list.Count <= 0)
            _list = TalkDataManager.Instance.list;
        showTime = 50;
        //var arr = text.Split(new char[1] { '|' });
        var arr = text.Split(new char[1] { '|' });
        if (arr[0] == "0") //表情
        {
            ResourcesLoader.Load<Sprite>("BaseAssets/Emoticons/face_" + arr[1], (sprite) =>
            {
                emoticons.overrideSprite = sprite;
                emoticons.SetNativeSize();
                emoticons.preserveAspect = true;
                emoticons.gameObject.SetActive(true);
            });
        }
        else if (arr[0] == "1") //快捷语
        {
            MyDebug.Log("---------------------------------------");
            for (int i = 0; i < _list.Count; i++)
            {
                if (int.Parse(arr[1]) == _list[i].id)
                {
                    chatMessage.text = _list[i].message;
                    chatPaoPao.SetActive(true);
                    break;
                }
            }
        }
        else if (arr[0] == "2") //输入文字
        {
            chatMessage.text = arr[1];
            chatPaoPao.SetActive(true);
        }
        else//语音
        {
            SocketEventHandle.Instance.SetClientResponse(APIS.MicInput_Response, null);
        }
    }
    public void DisplayAvatorIp()
    {
        if (avatarvo == null)
            return;
        UIManager.instance.Show(UIType.UIUserInfo, InitInfo);
    }

    private void InitInfo(GameObject go)
    {
        go.GetComponent<UIPanel_UserInfo>().SetUIData();
    }

    public void SetHuFlagDisplay()
    {
        HuFlag.SetActive(true);
    }

    public void SetHuFlagHidde()
    {
        HuFlag.SetActive(false);
    }

    public void ShowPosition()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        UIManager.instance.Show(UIType.UIPositionMonitoring);
    }
    public void XiaZhu(int num)
    {
        if (num <= 0)
            num = 1;
        txt_xiazhuNum.text = "x " + num;
    }
    public void SetScore(int num)
    {
        myscore = num;
        scoreText.text = num + "";
    }
    public void SetNormalScore(int num)
    {
        myscore += num;
        scoreText.text = myscore + "";
    }
    /////////////////////////===========New=================///////////////////////////////////
    public void SetPlayerInfo(PlayerGameRoomInfo playerInfo)
    {
        MyDebug.Log("----------------------------------------------------------");
        MyDebug.Log("SET Playerinfo.........");
        myselfInfo = playerInfo;
        nameText.text = myselfInfo.name;

        if (myselfInfo.chairId == 0)
        {
            oweroom.gameObject.SetActive(true);
        }
        if (nameText.text.Length > 8)
        {
            nameText.text = nameText.text.Substring(0, 6).ToString() + "..";
        }
        string url = Url.HOST + "Game/getAccountsFace?time=1&hash=4b50512c9c732419a0d992ab9cd202bc&uid=" + myselfInfo.userID;
        HttpManager.instance.HttpMessage(url, setHeadIcon);
        if (!GameDataSSS.Instance.isSss)
        {

        }
        else
        {
            if (cardList.Count > 13)
                return;
            for (int i = 0; i < 3; i++)
            {
                GameObject obj = Instantiate(backcard);
                obj.transform.SetParent(toudaotransform.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<Image>().enabled = false;
                cardList.Add(obj.GetComponent<Image>());
                tdlist.Add(obj.GetComponent<Image>());
                tdMalist.Add(obj.GetComponent<ShowMaPuke>());


            }
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(backcard);
                obj.transform.SetParent(zhongdaotransform.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<Image>().enabled = false;
                cardList.Add(obj.GetComponent<Image>());
                zdlist.Add(obj.GetComponent<Image>());
                zdMalist.Add(obj.GetComponent<ShowMaPuke>());
            }
            for (int i = 0; i < 5; i++)
            {
                GameObject obj = Instantiate(backcard);
                obj.transform.SetParent(weidaotransform.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<Image>().enabled = false;
                cardList.Add(obj.GetComponent<Image>());
                wdlist.Add(obj.GetComponent<Image>());
                wdMalist.Add(obj.GetComponent<ShowMaPuke>());
            }
        }
    }
    public void setHeadIcon(WWW mes)
    {
        JsonData json = JsonMapper.ToObject<JsonData>(mes.text);
        myselfInfo.userHeadUrl = json["data"].ToString();
        if (myselfInfo.userHeadUrl == null || myselfInfo.userHeadUrl.Length < 10)
        {
            ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/UI_Public/head" + myselfInfo.wFaceID, (sprite) =>
            {
                headerIcon.sprite = sprite;
            });
            SetHeadSprite();//获取本地头像
        }
        else
        {

        }
    }

    public void InitCard(int i)
    {
        MyDebug.Log(i + "=-=-============================================================" + gameObject.name);
        cardList[i].sprite = backcard.GetComponent<Image>().overrideSprite;
        cardList[i].enabled = true;
        if (i == 12)
            complete.gameObject.SetActive(true);
    }
    public void InitSSS()
    {
        for (int i = 0; i < comparText.Length; i++)
        {
            comparText[i].enabled = false;
            compareType[i].enabled = false;
        }
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].enabled = false;
        }
    }
    public void InitNN()
    {
        typeNN.enabled = false;

        txt_xiazhuNum.text = "";
    }
    public void SetHeadSprite()
    {
        for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)
        {
            if (GlobalDataScript.Instance.playerInfos[i].chairId == myselfInfo.chairId)
            {
                GlobalDataScript.Instance.playerInfos[i].headSprite = headerIcon.sprite;
            }
        }
    }
    internal void PlayBankAnima(int chairID, int count)
    {
        if (chairID == myselfInfo.chairId)
            Invoke("ShowAniQz", count * 1);

    }
}