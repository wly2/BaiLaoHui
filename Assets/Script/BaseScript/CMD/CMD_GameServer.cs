using System.Runtime.InteropServices;
//登录命令
//房间ID登录
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_LogonUserID
{
    public uint dwPlazeVersion; //广场版本
    public uint dwFrameVersion; //框架版本

    public uint dwProcessVersion; //进程版本

    //登录信息
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列

    public ushort wKindID; //类型索引

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szHeadHttp; //头像地址
}

//账号登录
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_LogonAccounts
{
    //版本信息
    public uint dwPlazaVersion; //广场版本
    public uint dwFrameVersion; //框架版本

    public uint dwProcessVersion; //进程版本

    //登录信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szAccounts; //登录帐号

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//升级提示
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_UpdateNotify
{
    //升级标志
    public byte cbMustUpdatePlaza; //强行升级
    public byte cbMustUpdateClient; //强行升级

    public byte cbAdviceUpdateClient; //建议升级

    //当前版本
    public uint dwCurrentPlazaVersion; //当前版本
    public uint dwCurrentFrameVersion; //当前版本
    public uint dwCurrentClientVersion; //当前版本
}


//列表子项
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagColumnItem
{
    public byte cbColumnWidth; //列表宽度
    public byte cbDataDescribe; //字段类型

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] szColumnName; //列表名字
}

//道具配置
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_ConfigProperty
{
    public byte cbPropertyCount; //道具数目

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public TagPropertyInfo PropertyInfo; //道具描述
}

//道具信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagPropertyInfo
{
    //道具信息
    public ushort wIndex; //道具标识
    public ushort wDiscount; //会员折扣

    public ushort wIssueArea; //发布范围

    //销售价格
    public long IPropertyGold; //道具金币

    public double dPropertyCash; //道具价格

    //赠送魅力
    public long ISendLoveLiness; //赠送魅力
    public long IRecvLoveLiness; //接受魅力
}


//用户积分
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagMobileUserScore
{
    //积分信息
    public long lScore; //用户分数

    //输赢信息
    public uint dwWinCount; //胜利盘数
    public uint dwLostCount; //失败盘数
    public uint dwDrawCount; //和局盘数

    public uint dwFleeCount; //逃跑盘数

    //全局信息
    public uint dwExperience; //用户经验
}

//用户积分
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagUserScore
{
    //积分信息
    public long lScore; //用户分数
    public long lGrade; //用户成绩

    public long lInsure; //用户银行

    //输赢信息
    public uint dwWinCount; //胜利盘数
    public uint dwLostCount; //失败盘数
    public uint dwDrawCount; //和局盘数

    public uint dwFleeCount; //逃跑盘数

    //全局信息
    public uint dwUserMedal; //用户奖牌
    public uint dwExperience; //用户经验
    public long lLoveLiness; //用户魅力
}


//用户状态
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct IPC_GF_UserInfo
{
    public byte cbCompanion; //用户关系
    public TagUserInfoHead UserInfoHead; //用户信息
}


//用户基本信息结构
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagUserInfoHead
{
    //用户属性
    public uint dwGameID; //游戏ID
    public uint dwUserID; //用户ID
    public uint dwGroupID; //社团ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szLogonIP; //登录IP

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szHeadHttp; //

    //头像信息
    public ushort wFaceID; //头像索引

    public uint dwCustomID; //自定标识

    //用户属性
    public byte cbGender; //用户性别
    public byte cbMemberOrder; //会员等级

    public byte cbMasterOrder; //管理等级

    //用户状态
    public ushort wTableID; //桌子索引
    public ushort wChairID; //椅子索引

    public byte cbUserStatus; //用户状态

    //积分信息
    public long lScore; //用户分数
    public long lGrade; //用户成绩

    public long lInsure; //用户银行

    //游戏信息
    public uint dwWinCount; //胜利盘数
    public uint dwLostCount; //失败盘数
    public uint dwDrawCount; //和局盘数
    public uint dwFleeCount; //逃跑盘数
    public uint dwUserMedal; //用户奖牌
    public uint dwExperience; //用户经验

    public uint lLoveLiness; //用户魅力
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    //public byte[] dwUserName;			//用户名字
}

//桌子状态
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagTableStatus
{
    public byte cbTableLock; //锁定标志
    public byte cbPlayStatus; //游戏标志
}


//费用提醒
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Match_Fee
{
    public long lMatchFee; //报名费用

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szNotifyContent; //提示内容
}

//费用提醒
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Match_JoinResoult
{
    public ushort wSuccess;
}

//比赛人数
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Match_Num
{
    public uint dwWaitting; //等待人数
    public uint dwTotal; //开赛人数
}

//赛事信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Match_Info
{
    public byte[,] szTitle; //信息标题
    public ushort wGameCount; //游戏局数
    public ushort wRank; //当前名次
}

//金币更新
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_MatchGoldUpdate
{
    public long lCurrGold; //当前金币
    public long lCurrIngot; //当前元宝
    public uint dwCurrExprience; //当前经验
}

//充值
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Recharge
{
    public uint dwUserID; //用户ID
    public uint dwAddType; //充值类型，0表示房卡，1表示金币
    public uint dwAddNum; //充值个数

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public uint[] Resver;
}

//充值返回
//充值
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Recharge_Success
{
    public uint dwUserID; //用户ID
    public uint dwAddType; //充值类型，0表示房卡，1表示金币
    public uint dwRetainNum; //剩余总的房卡个数
}

//私人场信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Private_Info
{
    public ushort wKindID; //游戏标识
    public long lCostGold; //金币

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] bPlayCout; //玩家局数

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public long[] lPlayCost; //消耗点数
}

public enum RoomType
{
    Type_Private = 0,
    Type_Public,
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_UserReady
{
    public ushort wChairID; //用户标识
    public byte bReady; //是否准备
};

//麻将创建房间
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Create_Private
{
    public short cbGameType; //房间类型  公共/私人 0
    public ushort w_player_count;
    public byte bPlayCoutIdex; //游戏局数 0:8,1:16
    public byte cb_pay_type; //房主 0，AA 1
    public byte bGameTypeIdex; //游戏类型 1
    public uint bGameRuleIdex; //游戏规则

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] stHttpChannel; //HTTP获取
}

//十三水创建房间
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_CreateTable
{
    public long lCellScore;                    //底分设置
    public uint dwDrawCountLimit;         //局数限制
    public uint dwDrawTimeLimit;          //时间限制
    public ushort wJoinGamePeopleCount;      //参与游戏的人数
    public uint dwRoomTax;                    //单独一个私人房间的税率，千分比
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword; //密码设置

    //第0位，是否有规则，数值0或1
    //第一位，游戏玩法（GameMode）
    //第二位，游戏人数
    //第三位，游戏局数
    //第四位，支付方式
    //第五位，马牌数据
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public byte[] cbGameRule;      //游戏规则 弟 0 位标识 是否设置规则 0 代表未设置 1 代表设置
};

//创建房间成功
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_Create_Private_Sucess
{
    public long lCurSocre; //当前剩余
    public uint dwRoomNum; //房间ID
}

//加入房间
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Join_Private
{
    public uint dwRoomNum; //房间ID
}

//解散房间
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Dismiss_Private
{
    public byte bDismiss; //解散
}

//重新加入
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Again_Private
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] stHttpChannel;
}

//私人场解散信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_Private_Dismiss_Info
{
    public uint dwDissUserCout;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public uint[] dwDissChairID;

    public uint dwNotAgreeUserCout;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public uint[] dwNotAgreeChairID;
}

//游戏配置
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_GameOption
{
    public byte cbAllowLookon; //旁观标志
    public uint dwFrameVersion; //框架版本
    public uint dwClientVersion; //游戏版本
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public byte[] cb_game_rule;					//房间信息
}

//用户聊天
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_C_UserChat
{
    public ushort wChatLength; //信息长度
    public uint dwChatColor; //信息颜色
    public uint dwTargetUserID; //目标用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szCharString; //聊天信息
}

//用户表情
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_C_UserExpression
{
    public ushort wItemIndex; //表情索引
    public uint dwTargetUserID; //目标用户
}

//游戏环境
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_GameStatus
{
    public byte cbGameStatus; //游戏状态
    public byte cbAllowLookon; //旁观标志
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_RequestReplayList
{
    public ushort askUserID; //请求用户ID
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_RequestReplayData
{
    public ushort userID; //请求用户ID
    public uint recordID; //每个游戏录像的ID
}

//每局信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagOneJuInfo
{
    public uint beginTime; //每局开始时间
    public uint recordID; //每局录像ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public int[] score; //每局玩家的得分情况
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_Share_Return
{
    public uint dwUserId; //分享用户
    public ushort wShareType; //分享类型，0：分享到朋友，1：分享到朋友圈
    public ushort wType; //赠送类型，0：送房卡，1：送金币
    public ushort wNum; //赠送个数
}