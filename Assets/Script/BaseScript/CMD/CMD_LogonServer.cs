using System.Runtime.InteropServices;
//获取在线
public class CMD_GP_GetOnline
{
    public ushort wServerCount; //房间数目

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
    public ushort[] wOnLineServerID; //房间标识
}

//类型在线
public class CMD_GP_KindOnline
{
    public ushort wKindCount; //类型数目

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public TagOnLineInfoKind OnLineInfoKind; //类型在线
}

//在线信息
public class TagOnLineInfoKind
{
    public ushort wKindID; //类型标识
    public uint dwOnLineCount; //在线人数
}

//房间在线
public class CMD_GP_ServerOnline
{
    public ushort wServerCount; //房间数目

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
    public TagOnLineInfoServer OnLineInfoServer; //房间在线
}

//在线信息
public class TagOnLineInfoServer
{
    public ushort wServerID; //房间标识
    public uint dwOnLineCount; //在线人数
}

//修改密码
public class CMD_GP_ModifyLogonPass
{
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szDesPassword; //用户密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szScrPassword; //用户密码
}

//修改密码
public class CMD_GP_ModifyInsurePass
{
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szDesPassword; //用户密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szScrPassword; //用户密码
}

//修改推荐人
public class CMD_GP_ModifySpreader
{
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //用户密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szSpreader; //用户密码
}

//修改头像
public class CMD_GP_CustomFaceInfo
{
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //用户密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2304)]
    public uint[] dwCustomFace; //图片信息
}

//银行资料
public class CMD_GP_UserInsureInfo
{
    public ushort wRevenueTake; //税收比例
    public ushort wRevenueTransfer; //税收比例
    public ushort wServerID; //房间标识
    public long lUserScore; //用户金币
    public long lUserInsure; //银行金币
    public long lTransferPrerequisite; //转账条件
}

//存入金币
public class CMD_GP_UserSaveScore
{
    public uint dwUserID; //用户ID
    public long lSaveScore; //存入金币

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//提取金币
public class CMD_GP_UserTakeScore
{
    public uint dwUserID; //用户ID
    public long lTakeScore; //提取金币

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //银行密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//转账金币
public class CMD_GP_UserTransferScore
{
    public uint dwUserID; //用户ID
    public byte cbByNickName; //昵称赠送
    public long lTransferScore; //转账金币

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //银行密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szNickName; //目标用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//银行成功
public class CMD_GP_UserInsureSuccess
{
    public uint dwUserID; //用户ID
    public long lUserScore; //用户金币
    public long lUserInsure; //银行金币

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szDescribeString; //描述消息
}

//银行失败
public class CMD_GP_UserInsureFailure
{
    public uint IResultCode; //错误代码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szDescribeString; //描述消息
}

//提取结果
public class CMD_GP_UserTakeResult
{
    public uint dwUserID; //用户ID
    public long IUserScore; //用户金币
    public long IUserInsure; //银行金币
}

//查询银行
public class CMD_GP_QueryInsureInfo
{
    public uint dwUserID; //用户ID
}

//查询用户
public class CMD_GP_QueryUserInfoRequest
{
    public byte cbByNickName; //昵称赠送

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szNickName; //目标用户
}

//用户信息
public class CMD_GP_UserTransferUserInfo
{
    public uint dwTargetGameID; //目标用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szNickName; //目标用户
}

//个人信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GP_UserAccountInfo
{
    //属性资料
    public ushort wFaceID; //头像标识
    public uint dwUserID; //用户标识
    public uint dwGameID; //游戏标识
    public uint dwGroupID; //社团标识
    public uint dwCustomID; //自定索引
    public uint dwUserMedal; //用户奖牌
    public uint dwExperience; //经验数值
    public uint dwLoveLiness; //用户魅力
    public uint dwSpreaderID; //推广ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szAccounts; //登陆账号

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szNickName; //用户昵称

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szGroupName; //社团名字

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szLogonIp; //登陆IP

    //用户成绩
    public long lUserScore; //用户游戏币

    public long lUserInsure; //用户银行

    //用户资料
    public byte cbGender; //用户性别
    public byte cbMoorMachine; //锁定机器

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szUnderWrite; //个性签名

    //会员资料
    public byte cbMemberOrder; //会员等级
    public Systemtime MemberOverDate; //到期时间
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct Systemtime
{
    public ushort wYear;
    public ushort wMonth;
    public ushort wDayOfWeek;
    public ushort wDay;
    public ushort wHour;
    public ushort wMinute;
    public ushort wSecond;
    public ushort wMilliseconds;
}

//游戏状态
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GP_UserInGameServerID
{
    public uint dwUserID; //用户ID
}

//比赛报名
public class CMD_GP_MatchSignup
{
    //比赛信息
    public ushort wServerID; //房间标识
    public uint dwMatchID; //比赛标识

    public uint dwMatchNO; //比赛场次

    //用户信息
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    //机器信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//取消报名
public class CMD_GP_MatchUnSignup
{
    //比赛信息
    public ushort wServerID; //房间标识
    public uint dwMatchID; //比赛标识

    public uint dwMatchNO; //比赛场次

    //用户信息
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    //机器信息
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//报名结果
public class CMD_GP_MatchSignupResult
{
    public bool bSignup; //报名标识
    public bool bSuccessed; //成功标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szDescribeString; //描述信息
}

//报名结果
public class CMD_GP_MatchGetAward
{
    public uint dwUserID;
    public uint dwMatchID; //比赛标识
    public uint dwMatchNO; //比赛场次
}

/*//排行信息
public class tagMatchAwardkInfo
{
    public ushort MatchRank;               //比赛名次
    public long RewardGold;                //奖励金币
    public uint RewardMedal;               //奖励元宝
    public uint RewardExperience;          //奖励经验
}*/
//游戏记录
public class CMD_GP_GetGameRecordList
{
    public uint dwUserID;
}

//消息查询
public class CMD_GP_SysMessageQuery
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码
}

//读取消息
public class CMD_GP_SysMessageRead
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    public int msgID; //消息ID
}

//出错回复
public class CMD_GP_SysMessageFailed
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szNotifyContent; //提示内容
}

//读成功回应
public class CMD_GP_SysMessageReadSuccess
{
    public uint dwUserID; //用户标识
    public int msgID; //消息ID
}

//游戏记录
public class CMD_GP_GetGameTotalRecord
{
    public uint dwUserID;
    public uint dwRecordID;
}

public class CMD_GP_GetAddBank
{
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //用户密码

    public int iRankIdex;
}

public class CMD_GP_GetExchangeHuaFei
{
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //用户密码
}

public class CMD_GP_GetShopInfo
{
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //用户密码
}

//查询信息
public class CMD_GP_QueryIndividual
{
    public uint dwUserID; //用户ID
}

//个人资料
public class CMD_GP_UserIndividual
{
    public uint dwUserID; //用户ID
}

//修改资料
public class CMD_GP_ModifyIndividual
{
    public byte cbGender; //用户性别
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //用户密码
}

//操作成功
public class CMD_GP_SpreaderResoult
{
    public uint IResultCode; //操作代码
    public long lScore;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szDescribeString; //成功消息
}

//操作成功
public class CMD_GP_OperateSuccess
{
    public uint IResultCode; //操作代码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szDescribeString; //成功消息
}

//操作失败
public class CMD_GP_OperateFailure
{
    public uint IResultCode; //错误代码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szDescribeString; //描述消息
}

//签到奖励
public class DBO_GP_CheckInReward
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public long[] lRewardGold; //奖励金额

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] lRewardType; //奖励类型 1金币 2道具

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] lRewardDay; //奖励天数
}

//查询签到
public class CMD_GP_CheckInQueryInfo
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码
}

//签到信息
public class CMD_GP_CheckInInfo
{
    public ushort wSeriesData; //连续日期
    public ushort wAwardDate; //物品日期
    public bool bToday; //签到标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public long[] lRewardGold; //奖励金额

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] lRewardType; //奖励类型 1金币 2道具

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] lRewardDay; //奖励天数
}

//执行签到
public class CMD_GP_CheckInDone
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//签到结果
public class CMD_GP_CheckInResult
{
    public bool bType; //是否是达到天数领取物品
    public bool bSuccessed; //成功标识
    public long lScore; //当前金币

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szNotifyContent; //提示内容
}

//签到查询
public class CMD_GP_SignInQuery
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码
}

//签到执行成功结果
public class CMD_GP_SignInSuccessResult
{
    public ushort wMonth; //当前是几月
    public ushort wDay; //当天是几号
    public ushort wMonthDays; //一个月多少天
    public uint dwSignInMsk; //每天签到掩码
    public byte cbTotalSignInDays; //累计签到天数

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public byte[] cbAwardStatus; //奖励状态 数组0下标代表7天累计签到 1代表14累计签到，状态分为已领取、不能领取、可以领取状态
}

//执行签到
public class CMD_GP_SignInDone
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//领取奖励
public class CMD_GP_SignInGet
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列

    public byte cbWhichAward; //领取第几个奖励 只能为0，1，2
}

//签到操作失败结果回复
public class CMD_GP_SignInFailedResult
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szNotifyContent; //提示内容
}

//新手活动
public class CMD_GP_BeginnerQueryInfo
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码
}

public class CMD_GP_BeginnerInfo
{
    public enum AwardType
    {
        Type_Gold = 1,
        Type_Phone = 2,
    }

    public ushort wSeriesDate; //连续日期
    public bool bTodayChecked; //签到标识
    public bool bLastCheckIned; //签到标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public long[] lRewardGold; //奖励金额

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] lRewardType; //奖励类型 1金币 2道具
}

public class CMD_GP_BeginnerDone
{
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

public class CMD_GP_BeginnerResult
{
    public bool bSuccessed; //成功标识
    public long lAwardCount; //奖励数量
    public long lAwardType; //奖励类型

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szNotifyContent; //提示内容
}

//领取低保
public class GMD_GP_BaseEnsureTake
{
    public uint dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列
}

//低保参数
public class CMD_GP_BaseEnsureParamter
{
    public long lScoreCondition; //游戏币条件
    public long lScoreAmount; //游戏币数量
    public byte cbTakeTimes; //领取次数
}

//低保结果
public class CMD_GP_BaseEnsureResult
{
    public bool bSuccessed; //成功标识
    public long lGameScore; //当前游戏币

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szNotifyContent; //提示内容
}

//自定义字段查询 公告
public class CMD_GP_QueryNotice
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szKeyName; //关键字
}

public class CMD_GP_PublicNotice
{
    public uint IResultCode; //操作代码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
    public byte[] szDescribeString; //成功消息
}

//账号登录
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GP_LogonAccounts
{
    //系统信息
    public ushort wModuleID;//模块标识
    public uint dwPlazaVersion; //广场版本 
    public byte cbDeviceType; //设备类型

    //登录信息
    public byte cbGender; //用户性别
    public byte cbPlatformID; //平台编号

    [MarshalAs(UnmanagedType.ByValArray, SizeConst =    66)]
    public byte [] szUserUin; //用户Uin

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szNickName; //用户昵称

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szCompellation; //真实名字   

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachineID; //机器标识   

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
    public byte[] szMobilePhone; //电话号码
}

//登录失败
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_MB_LogonFailure
{
    public uint lResultCode; //错误代码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szDescribeString; //错误消息
}

/// <summary>
/// 升级提示
/// </summary>
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_UpdateNotify
{
    public byte cbMustUpdate;//强行升级
    public byte cbAdviceUpdate;//建议升级
    public uint dwCurrentVersion;//当前版本
}

//ID登录
public class CMD_GP_LogonByUserID
{
    //登录信息
    public uint dwGameID; //游戏ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    public byte cbValidateFlags; //校验标识
}

//注册账号
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GP_RegisterAccounts
{
    //系统信息
    public uint dwPlazaVersion; //广场版本

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szMachineID; //机器序列

    //密码变量
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szLogonPass; //登录密码


    //注册信息
    public ushort wFaceID; //头像标识
    public byte cbGender; //用户性别

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szAccounts; //登录帐号

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szNickName; //用户昵称

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 19)]
    public byte[] szPassPortID; //证件号码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] szCompellation; //真实名字

    public byte cbValidateFlags; //校验标识
    public uint dwAgentID;          //代理标识
    public uint dwSpreaderGameID;       //推荐标识
  
}

//游客登录
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public class CMD_MB_LogonVisitor
{
    public ushort wModuleID; //模块标识   
    public uint dwPlazaVersion; //广场版本
    public byte cbDeviceType; //设备类型
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachine; //机器标识
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 24)]
    public byte [] szMobilePhone; //电话号码          
}

//登陆成功
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_LogonSuccess
{
    public ushort wFaceID; //头像标识
    public byte cbGender; //用户性别
    public uint dwCustomID; //自定头像
    public uint dwUserID; //用户 I D
    public uint dwGameID; //游戏 I D
    public uint dwExperience; //经验数值
    public long lLoveLiness; //用户魅力   
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szAccounts; //登录帐号
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szNickName; //用户昵称
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szGroupName; //动态密码

    public long lUserScore;//用户游戏币（银币）
    public long lUserIngot;//用户元宝
    public long lUserInsure; //用户银行
    public long dUserBeans; //用户游戏豆(砖石)       ----Double(Server)

    public byte cbInsureEnabled; //使能标识
    public byte cbIsAgent; //代理标识
    public byte cbMoorMachine; //锁定机器

    public long lRoomCard;//用户房卡
    public uint dwLockServerID; //锁定房间 I D
    public uint dwKindID; //游戏类型
}

//列表配置
public class CMD_GP_ListConfig
{
    public byte bShowOnLineCount; //显示人数
};