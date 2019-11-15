using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_ShowCard
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public byte[] cbFrontCard;//头道扑克
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] cbMidCard;//中道扑克
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] cbBackCard;//尾道扑克
    public ushort wSpecialType;                       //是否是特殊牌
};
//玩家摊牌
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_ShowCard
{                      
    public ushort wChairID;                          //摊牌玩家
    public ushort w_special_type;                        //特殊牌型
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public byte[] cbFrontCard;//头道扑克
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] cbMidCard;//中道扑克
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] cbBackCard;//尾道扑克
    public byte m_b_dao_shui;						//倒水
};
//玩家比牌
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_Compare
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] bPlayer;              //是否存在玩家
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ushort[] wSpecialType;         //特殊牌型
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] nSpecialCompareResult; //特殊牌型比较结果

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayThree[] cbFrontCard;		//玩家头道
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayFive[] cbMidCard;			//玩家中道
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayFive[] cbBackCard;			//玩家尾道


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayThreeI[] nCompareResult;		//每一道比较结果
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayThreeS[] w_hand_card_type;  //每一道牌型
                                            //bool								bShoot[GAME_PLAYER][GAME_PLAYER];	//打枪
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] bDaoshui;             //倒水                                                                                              
};
////发送手牌
//[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
//public struct CMD_S_SendCard
//{
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
//    public byte[] cbHandCardData; //扑克数据
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
//    public byte[] bPlayer; //是否是玩家
//    public ushort wSpecialType;//特殊牌型
//    public ushort w_select_card_type;	
//    public byte cb_sorted_card_count;                  //提供几种摆牌(1-4，如果有超过4种的摆牌方式，提供四种)
//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
//    public ArrayThirteen[] cb_sorted_card;          //摆牌，最多提供四种

//    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
//    public ArrayThreeS[] w_sorted_card_type;				//摆牌每一道牌型

//}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
//斷線從連信息
public struct CMD_S_StatusPlaySSS
{
    //扑克信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
    public byte[] cbHandCardData;             //桌面扑克


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayThree[] cbFrontCard;           //头道扑克	 --------


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayFive[] cbMidCard;				//中道扑克


     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayFive[] cbBackCard;				//尾道扑克


     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] bFinishSegment;           //是否完成配牌


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] bGameStatus;              //游戏状态


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayThreeL[] nCompareResult;			//分段倍数


     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ushort[] wSpecialType;             //特殊牌型


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public int[] nSpecialCompareResult;     //特殊牌型比较结果


    public byte cb_sorted_card_count;                  //提供几种摆牌(1-4，如果有超过4种的摆牌方式，提供四种)
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayThirteen[] cb_sorted_card;			//摆牌，最多提供四种

     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayThreeS[] w_sorted_card_type;				//摆牌每一道牌型


     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ArrayThreeS[] w_hand_card_type;		//手牌每一道牌型


     [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lGameScore;              //游戏积分


    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lUserScore;              //玩家分数  


    public byte bEndMode;                              //结束方式(正常结束：0；解散：1)
    public ushort w_select_card_type;						//所有可选牌型
};


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct ArrayThree
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public byte[] arrayItem;
}
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct ArrayFive
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] arrayItem;
}
public struct ArrayThreeS
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public ushort[] arrayItem;
}
public struct ArrayThreeI
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public int[] arrayItem;
}
public struct ArrayThreeL
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public long[] arrayItem;
}
public struct ArrayBFour
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] arrayItem;
}
public struct ArraySFour
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public ushort[] arrayItem;
}
public struct ArrayThirteen
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
    public byte[] arrayItem;
}

