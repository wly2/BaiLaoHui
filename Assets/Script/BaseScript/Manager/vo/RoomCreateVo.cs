using System;
using System.Collections.Generic;
using UnityEngine;
namespace AssemblyCSharp
{
    [Serializable]
    public class RoomCreateVo
    {
        public bool hong;
        public int ma;
        public int roomId;

        public int roomType; //1转转；2、划水；3、长沙

        /*局数*/
        public int roundNumber;
        public bool sevenDouble;
        public int ziMo; //1：自摸胡；2、抢杠胡
        public int xiaYu;
        public string name;
        public bool addWordCard;
        public int magnification;
        public byte bPlayCoutIdex; //玩家局数0 1，  8 或者16局
        public uint dwPlayCout; //游戏局数
        public uint dwPlayTotal; //总局数

        public RoomCreateVo()
        {
        }
    }
    [Serializable]
    public class GameRoomInfo
    {
        public uint tableOwnerUserID;                   //桌主 I D
        public string roomId;                //房间号
        public string limtNumber;                    //总局数
        public int PlayGameCount;                        //已玩局数  
        public int playerNum;                                  //房间人数
        public int payType;                                       //支付方式     0enPayMode
        public int gameMode;                             //游戏模式
        public string beishu;                                         //倍数
        public int limPlayer;
        public int maPaiId;
        public Sprite maPaiSprite;//马牌
        public long InitScore;
        public long InitBeishu;
        public List<RoomUserInfo> userList;
    }
    public class RoomUserInfo
    {                         
        public string name;
        public Sprite headIcon;
    }

    

    [Serializable]
    public class PlayerGameRoomInfo
    {
        public string userHeadUrl;
        public int userID;
        public int sex;
        public string name;
        public int Banker;
        public int chairId;
        public int tableId;
        public int wFaceID;
        public int lGameScore;
        public Sprite headSprite;
    }

    [Serializable]
    public class PlayerCardInfo
    {
        public int[] cardlist = new int[13];//手牌
        public Sprite[] cardSpriteList;
        public int[] selectcardList = new int[9];//普通牌型
        public int[] NNspecialCardList = new int[7];//牛牛特殊牌型
        public List<List<int>> sortedList;//摆牌，最多提供四种（一键摆牌）
        public List<List<int>> sorted_cardTypeList;//每一道牌型（对应一键摆牌，头，中，未的类型，比如同花，葫芦，顺子等）
    }


}