using System;
using UnityEngine;
using AssemblyCSharp;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Text;

public class GlobalDataScript
{
    public int kingNum;
    public int diamgNum;
    public bool showAllGameEnd;
    public bool isExitGame;
    public bool isSigGameOver;
    public int  iswSpecialType;//是否有特殊牌
    public PlayerGameRoomInfo myGameRoomInfo;
    public List<PlayerGameRoomInfo> playerInfos = new List<PlayerGameRoomInfo>();
    public GameRoomInfo roomInfo;
    public PlayerCardInfo cardInfo;
    public Chitchat messageInfo;
    public static CMD_MB_LogonSuccess userData;
    public static TagGlobalUserData tagUserData;
    public RoomUserInfo[] playersNameList = new RoomUserInfo[6];
    public static int SSS_KIND_ID = 49;//十三水
    public static int NN_KIND_ID = 27;//牛牛
    public static byte GAME_TYPE_SUSONG;
    public static int GAME_TYPE_REPLAY = 2;
    public static ModeType type = ModeType.None;

    public static string szMachineID;
    public static bool isDrag = true;

    /**登陆返回数据**/
    public static AvatarVO loginResponseData;
    /**加入房间返回数据**/
    public static RoomJoinResponseVo roomJoinResponseData;

    /**房间游戏规则信息**/
    public static RoomCreateVo roomVo = new RoomCreateVo();

    /**单局游戏结束服务器返回数据**/
    public static HupaiResponseVo hupaiResponseVo;

    /**全局游戏结束服务器返回数据**/
    public static FinalGameEndVo finalGameEndVo;
    public static TagUserInfoHead userInfo;

    public static int mainUuid;

    /**房间成员信息**/
    public static List<AvatarVO> roomAvatarVoList;

    //	public static Dictionary<int, Account > palyerBaseInfo = new Dictionary<int, Account> ();
    public List<CMD_MB_LogonSuccess> weChatInformationlist;
    public WeChatInformation weChatInformation;
    public static GameObject homePanel; //主界面
    public static GameObject gamePlayPanel; //游戏界面

    /**麻将剩余局数**/
    public static int surplusTimes;

    /**总局数**/
    public static int totalTimes;

    /**重新加入房间的数据**/
    public static RoomJoinResponseVo reEnterRoomData;
    /// <summary>
    /// 声音开关
    /// </summary>
    public static bool soundToggle = true;

    public static float soundVolume = 1;
    public static float musicVolume = 1;
    public static int putOutCount = 100;

    /// <summary>
    /// 单局结算面板
    /// </summary>
    public static List<GameObject> singalGameOverList = new List<GameObject>();


    public static bool isonLoginPage; //是否在登陆页面

    //public SocketEventHandle socketEventHandle;
    /// <summary>
    /// 抽奖数据
    /// </summary>
    public static List<LotteryData> lotteryDatas;

    public static bool isonApplayExitRoomstatus; //是否处于申请解散房间状态
    public static bool isOverByPlayer; //是否由用用户选择退出而退出的游戏
    public static LoginVo loginVo; //登录数据
    public static List<String> noticeMegs = new List<string>();
    public static bool isGameReadly;
    public static bool isBeginGame;



    private static GlobalDataScript _instance;

    public static GlobalDataScript Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GlobalDataScript();      
            }

            return _instance;
        }

    }
    public void ClearGameInfo()
    {                             
        playerInfos.Clear();
    }

    public void Init()
    {
        weChatInformation = new WeChatInformation();
    }

    private void StartGameReply(ClientResponse response)
    {
        cardInfo.cardSpriteList = new Sprite[cardInfo.cardlist.Length];
        for (int i = 0; i < cardInfo.cardlist.Length; i++)
        {
            ResourcesLoader.Load<Sprite>("PuKe/card_" + cardInfo.cardlist[i], (sprite) =>
            {
                cardInfo.cardSpriteList[i] = sprite;
            });
        }
    }

    private void RoomOptionReply(ClientResponse response)
    {
        MyDebug.Log("马牌数值：" + roomInfo.maPaiId);


        ResourcesLoader.Load<Sprite>("PuKe/card_" + roomInfo.maPaiId, (sprite) =>
        {
            roomInfo.maPaiSprite = sprite;
        });
    }



    public string GetIpAddress()
    {
        string tempip = "";
        try
        {
            WebRequest wr = WebRequest.Create("http://www.ip138.com/");
            Stream s = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd(); //读取网站的数据

            int start = all.IndexOf("[") + 1;
            int end = all.IndexOf("]");
            int count = end - start;
            tempip = all.Substring(start, count);
            sr.Close();
            s.Close();
        }
        catch
        {
        }

        return tempip;
    }
}