using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//游戏状态（空闲状态）
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_StatusFreeNN
{
    long lCellScore;                            //基础积分

    //历史积分
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lTurnScore;           //积分信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lCollectScore;            //积分信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szGameRoomName;           //房间名称
    tagRobotConfig RobotConfig;                     //机器人配置
    public long lStartStorage;                     //起始库存
    public long lBonus;
    public byte cbGameMode;                       //游戏模式
};

//游戏状态（叫庄）
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_StatusCallNN
{
    public ushort wCallBanker;                       //叫庄用户
    public byte cbDynamicJoin;                      //动态加入 
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] cbPlayStatus;          //用户状态

    //历史积分
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lTurnScore;           //积分信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lCollectScore;            //积分信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szGameRoomName;           //房间名称
    public tagRobotConfig RobotConfig;                     //机器人配置
    public long lStartStorage;                     //起始库存
    public long lBonus;
    public byte cbGameMode;                       //游戏模式
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public cbHandCardData[] cbHandCardData;           //明牌模式手牌
};

//明牌模式手牌
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct cbHandCardData
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] HandCard;           //明牌手牌
}

//游戏状态（下注）
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_StatusScoreNN
{
    //下注信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] cbPlayStatus;          //用户状态
    public byte cbDynamicJoin;                      //动态加入
    public long lTurnMaxScore;                     //最大下注
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lTableScore;          //下注数目
    public ushort wBankerUser;                       //庄家用户
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szGameRoomName;           //房间名称

    //历史积分
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lTurnScore;           //积分信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lCollectScore;            //积分信息
    public tagRobotConfig RobotConfig;                     //机器人配置
    public long lStartStorage;                     //起始库存
    public long lBonus;
    public byte cbGameMode;                     //游戏模式
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public cbHandCardData[] cbHandCardData;           //明牌模式手牌
};

//游戏状态（翻牌）
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_StatusPlayNN
{
    //状态信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] cbPlayStatus;          //用户状态
    public byte cbDynamicJoin;                      //动态加入
    public long lTurnMaxScore;                     //最大下注
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lTableScore;          //下注数目
    public ushort wBankerUser;                       //庄家用户

    //扑克信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public CARDDATA[] cbHandCardData;//桌面扑克
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ushort[] wSpecialType;         //特殊牌型
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] bOxCard;              //牛牛数据

    //历史积分
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lTurnScore;           //积分信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lCollectScore;            //积分信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szGameRoomName;           //房间名称
    public tagRobotConfig RobotConfig;                     //机器人配置
    public long lStartStorage;                     //起始库存
    public long lBonus;
    public byte cbGameMode;                     //游戏模式
};
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]

public struct tagRobotConfig
{
    public long lRobotScoreMin;
    public long lRobotScoreMax;
    public long lRobotBankGet;
    public long lRobotBankGetBanker;
    public long lRobotBankStoMul;
};
