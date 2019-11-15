using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;
using System;
using LitJson;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public class UIPanel_CreateRoom : UIWindow
{
    private int juShu;//局数
    private int renShu;//创建十三水人数
    private int payFor;//支付方式     
    private GameObject gameSence;
    private RoomCreateVo sendVo; //创建房间的信息
    [SerializeField]
    private GameObject sssCreateRoom;
    [SerializeField]
    private GameObject nnCreateRoom;
    //=================十三水创建房间=======================//
    public GameObject People_3;
    public GameObject People_4;
    public Toggle ptog3;
    public GameObject MPS;
    public Toggle[] toggleRule;//十三水玩法toggle
    public Toggle[] toggleMP;//马牌toggle                 
    public Toggle[] nnToggleRule;
    public List<int> aaConstList;
    public List<int> normalConstList;
    public Text constText;
    public Text constTextNN;

    void Start()
    {
        juShu = 10;
        renShu = 3;
        payFor = 0;

        nnPayType = 0;
        nnPlayerCount = 3;
        nnGameCount = 10;
        nnGameMode = 3;
        SocketEventHandle.Instance.createRoomReply += OnCreateRoomReply;
    }
    private void Update()
    {
        if (toggleRule[1].isOn == true)
        {
            MPS.SetActive(true);

        }
        else
        {
            MPS.SetActive(false);
        }

        if (toggleRule[2].isOn == true)
        {
            toggleRule[0].isOn = true;
        }

    }

    public void CloseDialog()
    {
        MyDebug.Log("closeDialog");
        SocketEventHandle.Instance.createRoomReply -= OnCreateRoomReply;
        Destroy(this);
        Destroy(gameObject);
    }

    /*
	 * 创建转转麻将房间
	 */
    public void CreateZhuanzhuanRoom()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        const int roundNumber = 4; //房卡数量
        const bool isZimo = false; //自摸
        const bool hasHong = false; //红中赖子
        const bool isSevenDoube = false; //七小对
        const int maCount = 0;
        sendVo = new RoomCreateVo
        {
            ma = maCount,
            roundNumber = roundNumber,
            ziMo = isZimo ? 1 : 0,
            hong = hasHong,
            sevenDouble = isSevenDoube
        };

        MyDebug.Log(sendVo.roomType);
        var sendmsgstr = JsonMapper.ToJson(sendVo);
        if (GlobalDataScript.loginResponseData.account.roomcard > 0)
        {
            //var haogang = haoGangTog.isOn ? Define.GAME_TYPE_HAOGANG : 0;
            //var shifang = shiFengTog.isOn ? Define.GAME_TYPE_10FENG : 0;
            // var rule = huFa | haogang | beishu | shifang;

            //  SocketSendManager.Instance.CreateRoom(rule, payFor, juShu);

            MyDebug.Log("创建房间成功!");

        }
        else
        {
            TipsManagerScript.getInstance.setTips(LocalizationManager.GetInstance.GetValue("KEY.11044"));
        }
    }

    //十三水创建房间
    public void ssSureCreate()
    {
       //
        SSStoggleChoose();

    }

    public void SSStoggleChoose()
    {
        GlobalDataScript.type = ModeType.Create;
        CMD_GR_CreateTable sssCreate = new CMD_GR_CreateTable();
        // GameMode gameMode = new GameMode();
        sssCreate.lCellScore = 0;
        sssCreate.dwDrawCountLimit = (uint)juShu;
        sssCreate.dwDrawTimeLimit = 0;
        sssCreate.wJoinGamePeopleCount = (ushort)renShu;
        sssCreate.dwRoomTax = 0;
        sssCreate.szPassword = new byte[66];
        sssCreate.szPassword[0] = 0;//密码
        sssCreate.cbGameRule = new byte[100];
      

        //===============马牌选择数据======================//
        if (toggleMP[0].isOn == true)
        {
            sssCreate.cbGameRule[5] = 0x35;
        }

        else if (toggleMP[1].isOn == true)
        {
            sssCreate.cbGameRule[5] = 0x3A;
        }

        else if (toggleMP[2].isOn == true)
        {
            sssCreate.cbGameRule[5] = 0x31;
        }



        var rule = GameMode.RED_WEAVE_MODE;
        //================游戏玩法==============//
        if (toggleRule[1].isOn == true)
        {
            rule = rule | GameMode.HORSE_CAED_MODE;
        }
        else
        {
            sssCreate.cbGameRule[5] = 0;
        }
        if (toggleRule[0].isOn == false)
        {
            rule = rule| GameMode.NULL_SUPPORT_SPECIAL_TYPE;
        }

        else if (toggleRule[2].isOn == true)
        {
            rule = rule | GameMode.KING_MODE;
        }


        sssCreate.cbGameRule[0] = 1;
        sssCreate.cbGameRule[1] = (byte)rule;//游戏玩法
        sssCreate.cbGameRule[2] = (byte)renShu;//游戏人数
        sssCreate.cbGameRule[3] = (byte)juShu;//游戏局数
        sssCreate.cbGameRule[4] = (byte)payFor;//支付模式
        byte[] ret = NetUtil.StructToBytes(sssCreate);
        SocketSSSEvent.instance.Init();
        SocketSSSEvent.instance.send_buffer = ret;
        SocketSSSEvent.instance.ConnectGameServer();
        UIManager.instance.Show(UIType.UILoading);

    }


  

    public void Setjushu(int dex)
    {
        juShu = dex;
        SetConst();
    }
    private void SetConst()
    {
        //AA
        if (payFor == 0)
        {
            constText.text = aaConstList[juShu / 10 - 1] + "";
        }
        else
        {
            if (juShu == 10)
            {
                switch (renShu)
                {
                    case 3:
                        constText.text = normalConstList[0] + "";
                        break;
                    case 4:
                        constText.text = normalConstList[1] + "";
                        break;
                    case 5:
                        constText.text = normalConstList[2] + "";
                        break;
                    case 6:
                        constText.text = normalConstList[3] + "";
                        break;
                    default:
                        break;
                }
            }
            else if (juShu == 20)
            {
                switch (renShu)
                {
                    case 3:
                        constText.text = normalConstList[3] + "";
                        break;
                    case 4:
                        constText.text = normalConstList[4] + "";
                        break;
                    case 5:
                        constText.text = normalConstList[5] + "";
                        break;
                    case 6:
                        constText.text = normalConstList[6] + "";
                        break;
                    default:
                        break;
                }

            }
            else if (juShu == 30)
            {
                switch (renShu)
                {
                    case 3:
                        constText.text = normalConstList[7] + "";
                        break;
                    case 4:
                        constText.text = normalConstList[8] + "";
                        break;
                    case 5:
                        constText.text = normalConstList[6] + "";
                        break;
                    case 6:
                        constText.text = normalConstList[9] + "";
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void SetConstNN()
    {
        //AA
        if (nnPayType == 0)
        {
            constTextNN.text = aaConstList[nnGameCount / 10 - 1] + "";
        }
        else
        {
            if (nnGameCount == 10)
            {
                switch (nnPlayerCount)
                {
                    case 3:
                        constTextNN.text = normalConstList[0] + "";
                        break;
                    case 4:
                        constTextNN.text = normalConstList[1] + "";
                        break;
                    case 5:
                        constTextNN.text = normalConstList[2] + "";
                        break;
                    case 6:
                        constTextNN.text = normalConstList[3] + "";
                        break;
                    default:
                        break;
                }
            }
            else if (nnGameCount == 20)
            {
                switch (nnPlayerCount)
                {
                    case 3:
                        constTextNN.text = normalConstList[3] + "";
                        break;
                    case 4:
                        constTextNN.text = normalConstList[4] + "";
                        break;
                    case 5:
                        constTextNN.text = normalConstList[5] + "";
                        break;
                    case 6:
                        constTextNN.text = normalConstList[6] + "";
                        break;
                    default:
                        break;
                }

            }
            else if (nnGameCount == 30)
            {
                switch (nnPlayerCount)
                {
                    case 3:
                        constTextNN.text = normalConstList[7] + "";
                        break;
                    case 4:
                        constTextNN.text = normalConstList[8] + "";
                        break;
                    case 5:
                        constTextNN.text = normalConstList[6] + "";
                        break;
                    case 6:
                        constTextNN.text = normalConstList[9] + "";
                        break;
                    default:
                        break;
                }
            }
        }
    }


    public void SetRenShu(int dex)
    {
        renShu = dex;
        if (dex == 1)
        {
            renShu = 3;
            ptog3.isOn = true;
            People_4.SetActive(true);
            ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Hall/UIPanel_CreateRoom/3Press", (sprite) =>
            {
                People_3.GetComponent<Image>().overrideSprite = sprite;
            });

        }

        if (dex == 5)
        {
            ptog3.isOn = true;
            People_4.SetActive(false);
            ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Hall/UIPanel_CreateRoom/NN5Press", (sprite) =>
             {
                 People_3.GetComponent<Image>().overrideSprite = sprite;
             });
        }

        if (dex == 6)
        {
            ptog3.isOn = true;
            People_4.SetActive(false);
            ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Hall/UIPanel_CreateRoom/NN6Press", (sprite) =>
            {
                People_3.GetComponent<Image>().overrideSprite = sprite;
            });
        }
        SetConst();

    }

    public void SetPayFo(int dex)
    {
        payFor = dex;
        SetConst();
    }

    public void OnCreateRoomReply(ClientResponse response)
    {
        MyDebug.Log(response.message);
        if (response.status == 1)
        {
            var roomid = Int32.Parse(response.message);
            sendVo.roomId = roomid;

            MyDebug.LogError(sendVo.roomId);

            GlobalDataScript.roomVo = sendVo;
            GlobalDataScript.loginResponseData.roomId = roomid;
            GlobalDataScript.loginResponseData.isOnLine = true;
            GlobalDataScript.type = ModeType.Create;
            if (GlobalDataScript.homePanel != null)
                GlobalDataScript.homePanel.SetActive(false);
            CloseDialog();
        }
        else
        {
            TipsManagerScript.getInstance.setTips(response.message);
        }
    }

    
    public void SssCreateRoom()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        sssCreateRoom.SetActive(true);
        nnCreateRoom.SetActive(false);
        MyDebug.Log("调出" + UIType.UIsssCreateRoom + "创建房间界面");
    }

    //牛牛
    public void NnCreateRoom()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        sssCreateRoom.SetActive(false);
        nnCreateRoom.SetActive(true);
        MyDebug.Log("调出" + UIType.UInnCreateRoom + "创建房间界面");
    }
    //============================================牛牛创建房间==========================================================
    private int nnPlayerCount;
    private int nnGameCount;
    private int nnPayType;
    private int nnGameMode;  //  enGameMode

    public void SetNNplayerCount(int value)
    {
        nnPlayerCount = value;
        SetConstNN();
    }
    public void SetNNgameCount(int value)
    {
        nnGameCount = value;
        SetConstNN();
    }
    public void SetNNPayType(int value)
    {
        nnPayType = value;
        SetConstNN();
    }
    public void SetNNGameMode(int value)
    {
        nnGameMode = value;
    }
    //牛牛创建房间
    public void nnSureCreate()
    {                   
        UIManager.instance.Show(UIType.UILoading);
        CMD_GR_CreateTable nnCreate = new CMD_GR_CreateTable();
        GlobalDataScript.type = ModeType.Create;  

        nnCreate.lCellScore = 0;
        nnCreate.dwDrawCountLimit = (uint)nnGameCount;
        nnCreate.dwDrawTimeLimit = 0;
        nnCreate.wJoinGamePeopleCount = (ushort)nnPlayerCount;
        nnCreate.dwRoomTax = 0;
        nnCreate.szPassword = new byte[66];
        nnCreate.szPassword[0] = 0;//密码
        nnCreate.cbGameRule = new byte[100];

        nnCreate.cbGameRule[0] = 1;
        nnCreate.cbGameRule[1] = (byte)nnGameMode;//游戏玩法
                                            //  GameDataNN.Instance.CreateRoomInfo.cbGameRule[1] = nnCreate.cbGameRule[1];
        nnCreate.cbGameRule[2] = (byte)nnPlayerCount;//游戏人数
        nnCreate.cbGameRule[3] = (byte)nnGameCount;//游戏局数
        nnCreate.cbGameRule[4] = (byte)nnPayType;//支付模式

        byte[] ret = NetUtil.StructToBytes(nnCreate);
        SocketNiuNiuEvent.instance.Init();
        SocketNiuNiuEvent.instance.send_buffer = ret;
        SocketNiuNiuEvent.instance.ConnectGameServer();
    }


}