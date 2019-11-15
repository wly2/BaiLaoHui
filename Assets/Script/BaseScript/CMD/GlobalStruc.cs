using System.Runtime.InteropServices;

//======================================麻将=======================================//
#region 麻将结构
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
//类型信息
public struct TagGameKind
{
    public ushort wTypeID; //类型号码
    public ushort wJoinID; //挂接索引
    public ushort wSortID; //排序号码
    public ushort wKindID; //名称号码
    public ushort wGameID; //模块索引
    public uint dwOnLineCount; //在线人数
    public uint dwFullCount; //满员人数

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szKindName; //游戏名字

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szProcessName; //进程名字
}

//游戏房间列表结构
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagGameServer
{
    public ushort wKindID; //名称索引
    public ushort wNodeID; //节点索引
    public ushort wSortID; //排序索引
    public ushort wServerID; //房间索引
    public ushort wServerKind; //房间类型（人数类型）
    public ushort wServerType; //房间类型(房卡金币类型)
    public ushort wServerLevel; //房间等级
    public ushort wServerPort; //房间端口

    public long lCellScore; //单元积分
    public byte cbEnterMember; //进入会员
    public long lEnterScore; //进入积分
    public uint dwServerRule; //房间规则
    public uint dwOnLineCount; //在线人数
    public uint dwAndroidCount; //机器人数
    public uint dwFullCount; //满员人数

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szServerAddr; //地址

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szServerName; //房间名称

    public uint dwSurportType; //支持类型
    public short wTableCount; //桌子数目
}

//登陆成功
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_LogonSuccess
{
    public uint dwUserRight; //用户权限
    public uint dwMasterRight; //管理权限
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
//登录失败
public struct CMD_GR_LogonFailure
{
    public long lErrorCode; //错误代码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szDescribeString; //描述消息
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_ResponseReplayList
{
    public ushort userID; //请求用户ID，用于验证
    public ushort recordNum; //实际的游戏次数，可能小于10
    public uint[,] recordID; //每个游戏录像的ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public uint[] roomNum;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public uint[] time;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public ushort[] gameType; //游戏类型 转转-300 杭州-400

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public uint[] gameRule; //游戏规则 创建游戏房间时的规则数据

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public uint[] playTotalCount; //游戏局数8？16

    public ushort[,] userIDList; //玩家列表
    public int[,] totalScore; //每次游戏的得分统计，8/16局的总得分
    public byte[,,] userName;
    public byte[,,] userHeadHttp;
    public int[,,] score; //8？16局的玩家得分情况[chair][gamelist]chair座位号0-3 gamelist 第几局0-8？ 0-16
}



[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagUserStatus
{
    public ushort wTableID; //桌子索引
    public ushort wChairID; //椅子位置
    public byte cbUserStatus; //用户状态
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_S_UserChat
{
    public ushort wChatLength; //信息长度
    public uint dwChatColor; //信息颜色
    public uint dwSendUserID; //发送用户
    public uint dwTargetUserID; //目标用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szChatString; //聊天信息
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_CM_SystemMessage
{
    public ushort wType; //消息类型
    public ushort wLength; //消息长度

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1024)]
    public byte[] szString; //消息内容
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_C_TableTalk
{
    public byte cbChairID; //座位
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_Match_Wait_Tip
{
    public long lScore; //当前积分
    public ushort wRank; //当前名次
    public ushort wCurTableRank; //本桌名次
    public ushort wUserCount; //当前人数
    public ushort wCurGameCount; //当前局数
    public ushort wGameCount; //总共局数
    public ushort wPlayingTable; //游戏桌数

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szMatchName; //比赛名称
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_MatchResult
{
    public long lGold; //金币奖励
    public uint dwIngot; //元宝奖励
    public uint dwExperience; //经验奖励

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szDescribe; //得奖描述
}

//用户信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagUserInfo
{
    //基本属性
    public uint dwUserID; //用户 I D
    public uint dwGameID; //游戏 I D
    public uint dwGroupID; //社团 I D

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szNickName; //用户昵称

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szGroupName; //社团名字

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szUnderWrite; //个性签名

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szLogonIP; //登录IP

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szHeadHttp; //

    //头像信息
    public ushort wFaceID; //头像索引

    public uint dwCustomID; //自定标识

    //用户资料
    public byte cbGender; //用户性别
    public byte cbMemberOrder; //会员等级

    public byte cbMasterOrder; //管理等级

    //用户状态
    public ushort wTableID; //桌子索引

    //public ushort wLastTableID;                      //游戏桌子
    public ushort wChairID; //椅子索引

    public byte cbUserStatus; //用户状态

    //积分信息
    public long lScore; //用户分数
    public long lGrade; //用户成绩

    public long lInsure; //用户银行

    //public long lGameGold;                                //用户元宝
    //游戏信息
    public uint dwWinCount; //胜利盘数
    public uint dwLostCount; //失败盘数
    public uint dwDrawCount; //和局盘数
    public uint dwFleeCount; //逃跑盘数
    public uint dwUserMedal; //用户奖牌
    public uint dwExperience; //用户经验

    public long lLoveLiness; //用户魅力

    //时间信息
    public TagTimeInfo TimerInfo;
}

//时间信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagTimeInfo
{
    public uint dwEnterTableTimer; //进出桌子时间
    public uint dwLeaveTableTimer; //离开桌子时间
    public uint dwStartGameTimer; //开始游戏时间
    public uint dwEndGameTimer; //离开游戏时间
}

//头像信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagCustomFaceInfo
{
    public uint dwDataSize; //数据大小

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 48 * 48)]
    public uint dwCustomFace; //图片信息
}

//出弹消息结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_ChuZengResult
{
    public byte bAllChuZeng; //是否所有用户选择了出增
    public ushort wCurrentUser; //当前选择出增的用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] cbZengValue; //当前每个用户的出弹情况，0xff代表无效值，1代表出弹，0代表不出弹
}

//出弹消息 出现这个消息 客户端需要显示是否选择出弹按钮，庄家不需要显示，庄家默认出弹
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_ChuZeng
{
    public ushort lSiceCount; //骰子点数
    public ushort wBankerUser; //庄家用户
    public ushort lLianZhuangCount; //连庄计数

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public int[] lGameTotalScore; //每个用户总的分数 每场输赢的总分
}

//组合子项
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_WeaveItem
{
    public byte cbWeaveKind; //组合类型
    public byte cbCenterCard; //中心扑克
    public byte cbPublicCard; //公开标志

    public ushort wProvideUser; //供应用户
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //public byte[] cbCardData;                                     //扑克数据
}

//空闲状态
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_StatusFree
{
    public long lCellScore; //基础金币
    public ushort wBankerUser; //庄家用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] bTrustee; //是否托管

    //public byte bYouDan;                                                      //是否是有弹
    //public byte cbQuanShu;                                                    //几圈局
}

//出增状态
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_StatusChuZeng
{
    public int lCellScore; //基础金币
    public ushort wBankerUser; //庄家用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] bTrustee; //是否托管

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] cbChuDan; //每个是否选择出弹  0xff代表还未做出选择 1代表出弹 0代表未出弹

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public int[] lGameTotalScore; //每个用户总的分数 每场输赢的总分
}

//游戏状态
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_StatusPlay
{
    //游戏变量
    public long lCellScore; //单元积分
    public ushort wBankerUser; //庄家用户

    public ushort wCurrentUser; //当前用户

    //状态变量
    public byte cbActionCard; //动作扑克
    public byte cbActionMask; //动作掩码
    public byte cbLeftCardCount; //剩余数目

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] bTrustee; //是否托管

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public ushort[] wWinOrder;

    //出牌信息
    public ushort wOutCardUser; //出牌用户
    public byte cbOutCardData; //出牌扑克

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] cbDiscardCount; //丢弃数目
                                  //----------------------------------------------------------------------------------------
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public DiscardCard[] cbDiscardCard; //丢弃记录

    //扑克数据
    public byte cbCardCount; //扑克数目

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] cbCardData; //扑克列表

    public byte cbSendCardData; //发送扑克

    //组合扑克
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] cbWeaveCount; //组合数目
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public Weave[] WeaveItemArray; //组合扑克
    ////财神信息
    //public byte cbMagicIndex;                                                //财神信息
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //public byte[] cbChuDan;                   //每个是否选择了出弹  0xff代表还未做出选择 1代表出弹 0代表未出弹
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //public int[] lGameTotalScore;              //每个用户总的分数 每场输赢的总分
}
public struct DiscardCard
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
    public byte[] WeaveItem;
}


//出牌命令
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_OutCard
{
    public ushort wOutCardUser; //出牌用户
    public byte cbOutCardData; //出牌扑克
}

//发送扑克
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_SendCard
{
    public byte cbCardData; //扑克数据
    public byte cbActionMask; //动作掩码
    public ushort wCurrentUser; //当前用户
    public byte bTail; //末尾发牌
}

//结果信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_ResultInfo
{
    public uint paiXingMask; //胡牌牌型掩码
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CbCard
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] cbCardData;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct Weave
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public CMD_WeaveItem[] WeaveItem;
}

//操作提示
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_OperateNotify
{
    public ushort wResumeUser; //还原用户
    public byte cbActionMask; //动作掩码
    public byte cbActionCard; //动作扑克
}

//操作命令
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_OperateResult
{
    public ushort wOperateUser; //操作用户
    public ushort wProvideUser; //供应用户
    public byte cbOperateCode; //操作代码

    public byte cbOperateCard; //操作扑克
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //public int[] lGameTotalScore;           //每个用户总的分数 每场输赢的总分
}

//用户托管
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_Trustee
{
    public byte bTrustee; //是否托管
    public ushort wChairID; //托管用户
}

//用户信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_USER_INFO
{
    public ushort dwUserID; //用户ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public string[] szLogonIP; //登陆IP

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public string[] szHeadHttp; //头像地址
}

//聊天命令
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_Chat
{
    public ushort wChairID; //座位号

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public byte[] szTitle;
}

//出牌命令
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_OutCard
{
    public byte cbCardData; //扑克数据
}

//操作命令
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_OperateCard
{
    public byte cbOperateCode; //操作代码
    public byte cbOperateCard; //操作扑克
}

//用户托管
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_Trustee
{
    public byte bTrustee; //是否托管
}

//聊天命令
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_Chat
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public byte[] szTitle; //语音ID
}

//出弹命令
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_ChuZeng
{
    public byte cbChuZengValue; //出弹选择值
}

//私人场房间信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GF_Private_Room_Info
{
    public byte bPlayCoutIdex; //玩家局数0,1
    public byte bGameTypeIdex; //游戏类型
    public uint bGameRuleIdex; //游戏规则
    public byte cb_pay_type;
    public ushort w_player_count;
    public byte bStartGame;
    public uint dwPlayCout; //游戏局数
    public uint dwRoomNum;
    public uint dwCreateUserID;

    public uint dwPlayTotal; //总局数

    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    //public int[] kWinLoseScore;
    public byte cbRoomType;
}

//查询信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GP_QueryAccountInfo
{
    public uint dwUserID; //用户 I D
}

//操作成功
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GP_InGameSeverID
{
    public uint LockKindID;
    public uint LockServerID;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct TagGlobalUserData
{
    //基本资料
    public uint dwUserID; //用户 I D
    public uint dwGameID; //游戏 I D
    public uint dwUserMedal; //用户奖牌
    public uint dwExperience; //用户经验
    public uint dwLoveLiness; //用户魅力
    public uint dwSpreaderID; //推广ID

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szAccounts; //登录帐号

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szNickName; //用户昵称

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szPassword; //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 33)]
    public byte[] szDynamicPass; //动态密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szLogonIP; //登录IP

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szUserChannel; //渠道号

    //用户成绩
    public long lUserScore; //用户游戏币
    public long lUserInsure; //银行游戏币
    public long lUserIngot; //用户元宝

    public double dUserBeans; //用户游戏豆

    //扩展资料
    public byte cbGender; //用户性别
    public byte cbMoorMachine; //锁定机器

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szUnderWrite; //个性签名

    //社团资料
    public uint dwGroupID; //社团索引

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szGroupName; //社团名字

    //会员资料
    public byte cbMemberOrder; //会员等级

    public Systemtime MemberOverDate; //到期时间

    //头像信息
    public ushort wFaceID; //头像索引
    public uint dwCustomID; //自定标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szHeadHttp; //http头像

    //配置信息
    public byte cbInsureEnabled; //银行使能
    public uint dwWinCount; //胜利盘数
    public uint dwLostCount; //失败盘数
    public uint dwDrawCount; //和局盘数
    public uint dwFleeCount; //逃跑盘数
}
#endregion 




//======================================十三水=============================================//
#region 十三水结构

#region 创建房间登录状态结构
//手机登录
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_LogonMobile
{
    //版本信息
    public ushort wGameID;                        //游戏id
    public uint dwProcessVersion;                 //游戏版本

    //桌子区域
    public byte cbDeviceType;                       //设备类型16  
    public ushort wBehaviorFlags;                     //行为标识17
    public ushort wPageTableCount;                    //分页桌数255

    //登录信息
    public uint dwUserID;                         //用户 I D

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword;              //登录密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szServerPasswd;       //房间密码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachineID;      //机器标识
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
    //public byte[] szHeadUrl;      //机器标识

};

//登录失败
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_CreateLogonFailure
{
    public int lErrorCode; //错误代码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szDescribeString;      //描述消息
};

//登录成功
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_CreateLogonSuccess
{
    public uint dwUserRight; //用户权限
    public uint dwMasterRight; //管理权限
}

//登录完成
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_CreateLogonFinish
{
    public byte bGuideTask; //引领任务
}

//升级提示
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_CreateLogonNoTify
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
#endregion

#region 配置命令结构
//列表配置
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_ConfigColumn
{
    byte cbColumnCount; //列表数目

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    byte[] ColumnItem;//列表描述
}

//房间配置
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_ConfigServer
{
    //房间属性
    ushort wTableCount; //列表数目
    ushort wChairCount; //列表数目

    //房间配置
    ushort wServerType; //列表数目
    uint dwServerRule; //列表数目
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    byte[] cb_game_rule;//列表描述
}

//玩家权限
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_ConfigUserRight
{
    uint cbColumnCount; //列表数目
}
#endregion

#region 用户命令结构
//用户信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct tagUserInfoHead
{
    //用户属性
    public uint dwGameID;                         //游戏 I D
    public uint dwUserID;                         //用户 I D
    public uint dwGroupID;                            //社团 I D

    //头像信息
    public ushort wFaceID;                           //头像索引
    public uint dwCustomID;                           //自定标识

    //用户属性
    public byte bIsAndroid;                            //机器标识
    public byte cbGender;                          //用户性别
    public byte cbMemberOrder;                     //会员等级
    public byte cbMasterOrder;                     //管理等级

    //用户状态
    public ushort wTableID;                          //桌子索引
    public ushort wChairID;                          //椅子索引
    public byte cbUserStatus;                      //用户状态

    //积分信息
    public long lScore;                               //用户分数
    public long lGrade;                               //用户成绩
    public long lInsure;                          //用户银行
    public long lIngot;                               //用户元宝
    public long dBeans;                              //用户游戏豆

    //游戏信息
    public uint dwWinCount;                           //胜利盘数
    public uint dwLostCount;                      //失败盘数
    public uint dwDrawCount;                      //和局盘数
    public uint dwFleeCount;                      //逃跑盘数
    public uint dwExperience;                     //用户经验
    public int lLoveLiness;                       //用户魅力
    public long lIntegralCount;                       //积分总数(当前房间)

    //代理信息
    public uint dwAgentID;                          //代理 I D   
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    //public byte[] szNickName; //用户名字
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
    //public byte[] szHeadUrl; //用户头像url
};

//用户信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct tagMobileUserInfoHead
{
    //用户属性
    public uint dwGameID;                         //游戏 I D
    public uint dwUserID;                         //用户 I D

    //头像信息
    public ushort wFaceID;                           //头像索引
    public uint dwCustomID;                           //自定标识

    //用户属性
    public byte cbGender;                          //用户性别
    public byte cbMemberOrder;                     //会员等级

    //用户状态
    public ushort wTableID;                          //桌子索引
    public ushort wChairID;                          //椅子索引
    public byte cbUserStatus;                      //用户状态

    //积分信息
    public long lScore;                               //用户分数
    public long lIngot;                               //用户元宝
    public long dBeans;                              //用户游戏豆

    //游戏信息
    public uint dwWinCount;                           //胜利盘数
    public uint dwLostCount;                      //失败盘数
    public uint dwDrawCount;                      //和局盘数
    public uint dwFleeCount;                      //逃跑盘数
    public uint dwExperience;                     //用户经验
    public long lIntegralCount;                       //积分总数(当前房间)

    //代理信息
    public uint dwAgentID;                            //代理 I D
    public uint dwClientAddr;						//用户地址
    public uint des;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szNickName;
};

//旁观请求
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserLookon
{
    public ushort wTableID;//桌子位置
    public ushort wChairID;//椅子位置
}

//坐下请求
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserSitDown
{
    public ushort wTableID;//桌子位置
    public ushort wChairID;//椅子位置
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword; //桌子密码
}

//起立请求
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserStandUp
{
    public ushort wTableID;//桌子位置
    public ushort wChairID;//椅子位置
    public byte cbForceLeave;//强行离开
}

//邀请用户
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserInvite
{
    public ushort wTableID;//桌子号码
    public uint dwUserID;//用户 I D
}

//邀请用户请求
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserInviteReq
{
    public ushort wTableID;//桌子号码
    public uint dwUserID;//用户 I D
}

//用户分数CMD_GR_UserScore
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserScore
{
    public uint dwUserID;//用户标识
    public tagUserScore UserScore;//积分信息
}

//用户分数 CMD_GR_UserScore -> tagUserScore
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct tagUserScore
{
    //积分信息
    public long lScore;//用户分数
    public long lGrade;//用户成绩
    public long lInsure;//用户银行
    public long lIngot;//用户元宝
    public double dBeans;//用户游戏豆

    //输赢信息
    public uint dwWinCount;//胜利盘数
    public uint dwLostCount;//失败盘数
    public uint dwDrawCount;//和局盘数
    public uint dwFleeCount;//逃跑盘数
    public long lIntegralCount;//积分总数(当前房间)


    public uint dwExperience;//用户经验
    public int lLoveLiness;//用户魅力
}

//用户分数CMD_GR_MobileUserScore
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_MobileUserScore
{
    public uint dwUserID;//用户标识
    public TagMobileUserScore UserScore;//积分信息
}

//用户分数CMD_GR_MobileUserScore -> tagMobileUserScore
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct tagMobileUserScore
{
    //积分信息
    public long lScore;//用户分数
    public double dBeans;//用户游戏豆

    //输赢信息
    public uint dwWinCount;//胜利盘数
    public uint dwLostCount;//失败盘数
    public uint dwDrawCount;//和局盘数
    public uint dwFleeCount;//逃跑盘数
    public long lIntegralCount;//积分总数(当前房间)

    //全局信息
    public uint dwExperience;//用户经验
}

//用户状态
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserStatus
{
    public uint dwUserID;//用户标识
    public tagUserStatus UserStatus;//用户状态
}

//用户状态 -> tagUserStatus
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct tagUserStatus
{
    public ushort wTableID;//桌子索引
    public ushort wChairID;//椅子位置
    public byte cbUserStatus;//用户状态  2坐下，1站立
                             //#define US_NULL						0x00								//没有状态
                             //#define US_FREE						0x01								//站立状态
                             //#define US_SIT						0x02								//坐下状态
                             //#define US_READY					0x03								//同意状态
                             //#define US_LOOKON					0x04								//旁观状态
                             //#define US_PLAYING					0x05								//游戏状态
                             //#define US_OFFLINE					0x06								//断线状态
}

//用户游戏数据
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserGameData
{
    public uint dwUserID;//用户标识    
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2048)]
    public byte[] szUserGameData; //语音ID
}

//请求失败
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_RequestFailure
{
    public int lErrorCode; //错误代码

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
    public byte[] szDescribeString; //描述信息
}

//用户聊天CMD_GR_C_UserChat
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_C_UserChat
{
    public ushort wChatLength; //信息长度
    public uint dwChatColor; //信息颜色
    public uint dwSendUserID; //发送用户
    public uint dwTargetUserID; //目标用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szChatString; //描述信息
}

//用户聊天CMD_GR_S_UserChat
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_S_UserChat
{
    public ushort wChatLength; //信息长度
    public uint dwChatColor; //信息颜色
    public uint dwSendUserID; //发送用户
    public uint dwTargetUserID; //目标用户

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szChatString; //描述信息
}

//用户表情CMD_GR_C_UserExpression
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_UserExpression
{
    public ushort wItemIndex;//表情索引
    public uint dwSendUserID;//发送用户
    public uint dwTargetUserID;//目标用户
}

//用户表情CMD_GR_S_UserExpression
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_UserExpression
{
    public ushort wItemIndex;//表情索引
    public uint dwSendUserID;//发送用户
    public uint dwTargetUserID;//目标用户
}

//用户私聊CMD_GR_C_WisperChat
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_WisperChat
{
    public ushort wChatLength;//信息长度
    public uint dwChatColor;//信息颜色
    public uint dwSendUserID;//发送用户
    public uint dwTargetUserID;//目标用户  
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szChatString; //聊天信息
}

//CMD_GR_S_WisperChat
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_WisperChat
{
    public ushort wChatLength;//信息长度
    public uint dwChatColor;//信息颜色
    public uint dwSendUserID;//发送用户
    public uint dwTargetUserID;//目标用户  
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szChatString; //聊天信息
}

//私聊表情 CMD_GR_C_WisperExpression
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_WisperExpression
{
    public ushort wItemIndex;//表情索引
    public uint dwSendUserID;//发送用户
    public uint dwTargetUserID;//目标用户
}

//私聊表情 CMD_GR_S_WisperExpression
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_WisperExpression
{
    public ushort wItemIndex;//表情索引
    public uint dwSendUserID;//发送用户
    public uint dwTargetUserID;//目标用户
}

//用户会话 
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_ColloquyChat
{
    public ushort wChatLength;//信息长度
    public uint dwChatColor;//信息颜色
    public uint dwSendUserID;//发送用户
    public uint dwConversationID;//会话标识
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] dwTargetUserID; //目标用户
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szChatString; //聊天信息
}

//邀请用户 CMD_GR_C_InviteUser
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_InviteUser
{
    public ushort wTableID;//桌子号码
    public uint dwSendUserID;//发送用户
}

//邀请用户 CMD_GR_S_InviteUser
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_InviteUser
{
    public uint dwTargetUserID;//目标用户
}

//用户拒绝黑名单坐下
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_UserRepulseSit
{
    public uint wTableID;//桌子号码
    public uint wChairID;//椅子位置
    public uint dwUserID;//用户 I D
    public uint dwRepulseUserID;//用户 I D
}


//请求用户信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_ChairUserInfoReq
{
    public ushort wTableID;                        //请求用户
    public ushort wChairID;							//桌子位置
};



#endregion

#region 桌子状态命令结构
//列表配置
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_TableInfo
{
    ushort wTableCount; //桌子数目
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
    tagTableStatus[] TableStatusArray;//桌子状态
}

//列表配置 -> tagTableStatus
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct tagTableStatus
{
    byte cbTableLock; //锁定标志
    byte cbPlayStatus; //游戏标志
    int lCellScore; //单元积分
}

//桌子状态
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_TableStatus
{
    ushort wTableID; //桌子号码
    byte TableStatus; //桌子状态
}

#endregion

#region 银行命令结构
//开通银行
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_EnableInsureRequest
{
    public byte cbActivityGame; //游戏动作
    public uint dwUserID;//用户I D

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    byte[] szLogonPass;//登录密码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    byte[] szInsurePass;//银行密码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    byte[] szMachineID;//机器序列
}

//查询银行
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_QueryInsureInfoRequest
{
    public byte cbActivityGame; //游戏动作
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    byte[] szInsurePass;//银行密码
}

//取款请求
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_TakeScoreRequest
{
    public byte cbActivityGame; //游戏动作
    public long lTakeScore; //取款数目
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    byte[] szInsurePass;//银行密码
}

//转账金币
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GP_C_TransferScoreRequest
{
    public byte cbActivityGame; //游戏动作
    public long lTransferScore; //转账金币
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    byte[] szAccounts;//目标用户
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    byte[] szInsurePass;//银行密码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    byte[] szTransRemark;//转账备注
}

//查询用户
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_QueryUserInfoRequest
{
    public byte cbActivityGame; //游戏动作
    public byte cbByNickName; //昵称赠送
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    byte[] szAccounts;//目标用户
}

//银行资料
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_UserInsureInfo
{
    public byte cbActivcbActivityGameityGame; //游戏动作
    public byte cbEnjoinTransfer; //转账开关
    public ushort wRevenueTake; //税收比例
    public ushort wRevenueTransfer; //税收比例
    public ushort wRevenueTransferMember; //税收比例
    public ushort wServerID; //房间标识
    public long lUserScore; //用户金币
    public long lUserInsure; //银行金币
    public long lTransferPrerequisite; //转账条件

}

//银行成功
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_UserInsureSuccess
{
    public byte cbActivityGame; //游戏动作
    public long lUserScore; //身上金币
    public long lUserInsure; //银行金币
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    byte[] szDescribeString;//描述消息

}

//银行失败
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_UserInsureFailure
{
    public byte cbActivityGame; //游戏动作
    public long lErrorCode; //错误代码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    byte[] szDescribeString;//描述消息

}

//用户信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_UserTransferUserInfo
{
    public byte cbActivityGame; //游戏动作
    public uint dwTargetGameID; //目标用户
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    byte[] szAccounts;//目标用户

}

//开通结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_UserInsureEnableResult
{
    public byte cbActivityGame; //游戏动作
    public byte dwTargetGameID; //使能标识
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    byte[] szDescribeString;//描述消息

}



#endregion

#region 任务命令结构
//加载任务
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_LoadTaskInfo
{
    public uint dwUserID; //用户标识
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword; //用户密码
}

//放弃任务
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_TakeGiveUp
{
    public ushort wTaskID; //任务标识
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword; //登录密码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachineID; //机器序列
}

//领取任务
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_TakeTask
{
    public ushort wTaskID; //任务标识
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword; //登录密码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachineID; //机器序列
}

//领取奖励
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_TaskReward
{
    public ushort wTaskID; //任务标识
    public uint dwUserID; //用户标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword; //登录密码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachineID; //机器序列
}

//任务信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_TaskRewaCMD_GR_S_TaskInford
{
    public ushort wTaskCount; //任务数量
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] TaskStatus; //任务状态
}

//任务信息 -> tagTaskStatus
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct tagTaskStatus
{
    public ushort wTaskID; //任务标识
    public ushort wTaskProgress; //任务进度
    public byte cbTaskStatus; //任务状态
}

//任务完成
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_TaskFinish
{
    public ushort wFinishTaskID; //任务标识
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szTaskName; //任务名称
}

//任务结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_TaskResult
{
    public bool bSuccessed; //成功标识
    public ushort wCommandID; //命令标识
    public long lCurrScore; //当前金币
    public long lCurrIngot; //当前元宝
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szNotifyContent; //提示内容
}
#endregion

#region 兑换命令指令
//查询参数
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_ExchangeParameter
{
    public uint dwExchangeRate;//元宝游戏币兑换比率
    public uint dwPresentExchangeRate;//魅力游戏币兑换率
    public uint dwRateGold;//游戏豆游戏币兑换率
    public ushort wMemberCount;//会员数目
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public byte[] MemberParameter; //会员参数
}

//查询参数 -> 会员参数tagMemberParameter
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct tagMemberParameter
{
    public byte cbMemberOrder;//会员标识
    public long lMemberPrice;//会员价格
    public long lPresentScore;//赠送游戏币
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szMemberName; //会员名称
}

//购买会员
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_PurchaseMember
{
    public uint dwUserID;//用户标识
    public byte cbMemberOrder;//会员标识
    public ushort wPurchaseTime;//购买时间
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachineID; //机器标识
}

//购买结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_PurchaseResult
{
    public uint bSuccessed;//用户标识
    public byte cbMemberOrder;//会员标识
    public ushort lCurrScore;//购买时间
    public ushort dCurrBeans;//购买时间
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szNotifyContent; //机器标识
}

//兑换游戏币
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_ExchangeScoreByIngot
{
    public uint dwUserID;//用户标识
    public long lExchangeIngot;//兑换数量
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachineID; //机器标识
}

//兑换结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_ExchangeResult
{
    public bool bSuccessed;//成功标识
    public long lCurrScore;//当前游戏币
    public long lCurrIngot;//当前元宝
    public double dCurrBeans;//当前游戏豆
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szNotifyContent; //提示内容
}
#endregion 

#region 道具命令
//道具失败
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_GamePropertyFailure
{
    public int lErrorCode;//错误代码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szDescribeString; //语音ID
}

//购买道具
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_PropertyBuy
{
    public uint dwUserID;//用户 I D
    public uint dwPropertyID;//道具标识
    public uint dwItemCount;//道具标识
    public byte cbConsumeType;//积分消费
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword; //登录密码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szMachineID; //机器序列
}

//购买结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_PropertyBuyResult
{
    public uint dwUserID;//用户 I D
    public uint dwPropertyID;//道具标识
    public uint dwItemCount;//道具数目
    public int lInsureScore;//银行存款
    public int lUserMedal;//用户元宝
    public int lLoveLiness;//魅力值
    public double dCash;//游戏豆
    public byte cbCurrMemberOrder;//会员等级
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szNotifyContent; //提示内容
}

//购买道具
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_GamePropertyUse
{
    public byte cbConsumeType;//积分消费
    public ushort wItemCount;//购买数目
    public ushort wPropertyIndex;//道具索引
    public uint dwUserID;//使用对象
}

//道具成功
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_PropertySuccess
{
    public byte cbRequestArea;//使用环境
    public ushort wKind;//功能类型
    public ushort wItemCount;//购买数目
    public ushort wPropertyIndex;//道具索引
    public uint dwSourceUserID;//目标对象
    public uint dwTargetUserID;//使用对象
}

//道具失败
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_PropertyFailure
{
    public ushort wRequestArea;//请求区域
    public int lErrorCode;//错误代码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
    public byte[] szDescribeString; //描述信息
}

//道具消息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_PropertyMessage
{
    public ushort wPropertyIndex;//道具索引
    public ushort wPropertyCount;//道具数目
    public uint dwSourceUserID;//目标对象
    public uint dwTargerUserID;//使用对象
}

//背包结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct DBO_GR_QueryBackpack
{
    public ushort dwUserID;//道具索引
    public ushort dwStatus;//道具数目
    public ushort dwCount;//目标对象
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
    public byte[] PropertyInfo; //道具信息
}

//道具效应
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_PropertyEffect
{
    public ushort wUserID;//用 户I D
    public byte wPropKind;//道具类型
    public byte cbMemberOrder;//会员等级
    public ushort dwFleeCount;//逃跑局数
    public long lScore;//负分清零
}

//发送喇叭 CMD_GR_C_SendTrumpet
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_SendTrumpet
{
    public byte cbRequestArea;//用 户I D
    public ushort wPropertyIndex;//道具类型
    public uint TrumpetColor;//会员等级
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public byte[] szTrumpetContent; //语音ID
}

//发送喇叭 CMD_GR_S_SendTrumpet
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_SendTrumpet
{
    public ushort wPropertyIndex;//道具索引
    public uint dwSendUserID;//用户 I D
    public uint TrumpetColor;//喇叭颜色
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szSendNickName; //玩家昵称
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szTrumpetContent; //喇叭内容
}

//背包道具 CMD_GR_C_BackpackProperty
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_BackpackProperty
{
    public uint dwUserID;//用户标识
    public uint dwKindID;//道具类型
}

//背包道具 CMD_GR_C_BackpackProperty
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_BackpackProperty
{
    public uint dwUserID;//用户标识
    public uint dwStatus;//状态
    public uint dwCount;//个数
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
    public byte[] PropertyInfo; //道具信息
}

//使用道具 CMD_GR_C_PropertyUse
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_PropertyUse
{
    public uint dwUserID;//使用者
    public uint dwRecvUserID;//对谁使用
    public uint dwPropID;//道具ID
    public ushort wPropCount;//使用数目
}

//使用道具 CMD_GR_S_PropertyUse
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_PropertyUse
{
    public uint dwUserID;//使用者
    public ushort wUseArea;//发布范围
    public ushort wServiceArea;//服务范围
    public uint dwRecvUserID;//对谁使用
    public uint dwPropID;//道具ID
    public uint wPropCount;//使用数目
    public uint dwRemainderPropCount;//剩余数量
    public long Score;//游戏金币

    public long lSendLoveLiness;//赠送魅力
    public long lRecvLoveLiness;//接受魅力
    public uint lUseResultsGold;//获得金币
    public ushort dwPropKind;//道具类型
                             //  public uint tUseTime;//使用的时间
    public uint UseResultsValidTime;//有效时长(秒)
    public uint dwHandleCode;//处理结果
    public byte cbMemberOrder;//会员标识

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szName; //道具名称
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szNotifyContent; //提示内容
}

//道具Buff
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_PropertyBuff
{
    public uint dwUserID;//
    public byte cbBuffCount;//
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public byte[] PropertyBuff; //语音ID
}

//玩家喇叭
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_UserTrumpet
{
    public uint dwTrumpetCount;//小喇叭数
    public uint dwTyphonCount;//大喇叭数
}

//查询赠送 CMD_GR_C_QuerySendPresent
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_QuerySendPresent
{
    public uint dwUserID;//用户 I D
}

//查询赠送 CMD_GR_S_QuerySendPresent
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_QuerySendPresent
{
    public ushort wPresentCount;//赠送次数
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] Present; //
}

//获取赠送 CMD_GR_C_GetSendPresent
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_GetSendPresent
{
    public uint dwUserID;//赠送者
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword; //用户密码
}

//获取赠送 CMD_GR_S_GetSendPresent
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_GetSendPresent
{
    public ushort wPresentCount;//赠送次数
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] Present; //
}

//赠送道具 CMD_GR_C_PropertyPresent
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_C_PropertyPresent
{
    public uint dwUserID;//赠送者
    public uint dwRecvGameID;//道具给谁
    public uint dwPropID;//道具ID
    public ushort wPropCount;//使用数目
    public ushort wType;//目标类型
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szRecvNickName; //对谁使用
}

//赠送道具 CMD_GR_S_PropertyPresent
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_S_PropertyPresent
{
    public uint dwUserID;//赠送者
    public uint dwRecvGameID;//道具给谁
    public uint dwPropID;//道具ID
    public ushort wPropCount;//使用数目
    public ushort wType;//目标类型
    public int nHandleCode;//返回码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szRecvNickName; //对谁使用
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
    public byte[] szNotifyContent; //提示内容
}
#endregion 

#region 登录服私人房间创建结构
//查找房间
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_QueryGameServer
{
    public uint dwUserID;                         //用户I D
    public uint dwKindID;                         //游戏I D
    public byte cbIsJoinGame;						//是否参与游戏
};

//查询结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_QueryGameServerResult
{
    public uint dwServerID;                           //房间I D
    public byte bCanCreateRoom;                //是否可以创建房间

    //错误描述
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public byte[] szErrDescrybe; //错误描述
};
//搜索别人房间
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_SearchServerTable
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] szServerID;                        //房间编号
    public uint dwKindID;			//房间类型
};
//搜索结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_SearchResult
{
    public uint dwServerID;                           //房间 I D
    public uint dwTableID;							//桌子 I D
};

#endregion 

#region 游戏服私人房间创建状态结构
//创建成功
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_CreateSuccess
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] szServerID; //房间编号
    public uint dwDrawCountLimit;             //局数限制
    public uint dwDrawTimeLimit;              //时间限制
    public long dBeans;                              //游戏豆
    public long lRoomCard;							//房卡数量
    public uint dwTableId;              //桌子号
};

//创建失败
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_CreateFailure
{
    int lErrorCode;                            //错误代码
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szDescribeString; //描述消息
};


//请求解散
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_CancelRequest
{
    public uint dwUserID;                         //用户 I D
    public uint dwTableID;                            //桌子 I D
    public uint dwChairID;							//椅子 I D
};

//请求结果
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_RequestResult
{
    public uint dwTableID;                            //桌子 I D
    public byte cbResult;							//请求结果
};

//请求答复
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_RequestReply
{
    public uint dwUserID;                         //用户I D
    public uint dwTableID;                            //桌子 I D
    public byte cbAgree;							//用户答复
};

//提示信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_PersonalTableTip
{
    public uint dwTableOwnerUserID;                   //桌主 I D
    public uint dwDrawCountLimit;                 //局数限制
    public uint dwDrawTimeLimit;                  //时间限制
    public uint dwPlayCount;                      //已玩局数
    public uint dwPlayTime;                           //已玩时间
    public long lCellScore;                            //游戏底分
    public long lIniScore;                             //初始分数
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] szServerID;//房间编号
    public byte cbIsJoinGame;                  //是否参与游戏
    public byte cbIsGoldOrGameScore;	//金币场还是积分场 0 标识 金币场 1 标识 积分场 
};

//结束消息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_PersonalTableEnd
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
    public byte[] szDescribeString; //桌子 I D	
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 200)]
    public long[] lScore;


    int nSpecialInfoLen;            //特殊信息长度
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1000)]
    public byte[] cbSpecialInfo; //特殊信息数据
};

//超时等待
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_WaitOverTime
{
    public uint dwUserID;							//用户 I D
};

//房主强制解散游戏
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_HostDissumeGame
{
    public uint dwUserID;                         //用户 I D
    public uint dwTableID;							//桌子 I D
};

//解散桌子
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GR_DissumeTable
{
    public byte cbIsDissumSuccess;                 //是否解散成功
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] szRoomID; //桌子 I D			
                            // SYSTEMTIME sysDissumeTime;                      //解散时间	
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public tagPersonalUserScoreInfo PersonalUserScoreInfo; //桌子 I D		
};

//解散桌子 --> tagPersonalUserScoreInfo//玩家信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct tagPersonalUserScoreInfo
{
    public uint dwUserID;                         //玩家ID
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szUserNicname; //用户昵称	

    //积分信息
    public long lScore;                               //用户分数
    public long lGrade;                               //用户成绩
    public long lTaxCount;                      //税收总数      
}
#endregion


#region 十三水服务器命令结构
//游戏开始
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_GameStart
{
    public long lUserScore;                           //玩家积分
    public ushort wSpecialType;                      //特殊牌型
    public byte cbBossCount;						//王数量
}


//用户叫庄(返回)
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_SNN_CallBanker
{
    public ushort wCallBanker;                       //叫庄用户
    public byte bFirstTimes;                       //首次叫庄
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] cbPlayerStatus;//玩家状态
};

//用户加注
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_C_AddScore
{
    public long lScore;								//加注数目
};

//用户下注(返回)
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_AddScore
{
    public ushort chairId;                     //加注用户
    public long lAddScoreCount;						//加注数目
};

//发送扑克
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_SSS_SendCard
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
    public byte[] cbHandCardData; //扑克数据
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] bPlayer; //是否有玩家存在
    public ushort wSpecialType;                      //特殊牌型
    public ushort w_select_card_type;                    //所有可选牌型
    public byte cb_sorted_card_count;               //提供几种摆牌(1-4，如果有超过4种的摆牌方式，提供四种)

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public PlaySorted_Card[] cb_sorted_card; //摆牌，最多提供四种（一键摆牌）

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public PlayCard_Type[] w_sorted_card_type; //每一道牌型（对应一键摆牌，头，中，未的类型，比如同花，葫芦，顺子等）
};

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct PlaySorted_Card
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
    public byte[] sorted_card; //摆牌，最多提供四种
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct PlayCard_Type
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public byte[] card_type; //摆牌，最多提供四种
}

//客户端发送用户选牌
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_C_SelectCard
{
    public ushort w_selected_type;                   //用户选择的牌型
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
    public byte[] cb_left_card; //用户未配好的牌
    public byte cb_left_card_count;                    //用户未配好的牌数量
    public byte cb_select_count;					//该牌型第几次选取(从0开始)
};

//服务端返回用户选牌
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_S_SelectCard
{
    public ushort w_selected_type;                   //用户选择的牌型
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] cb_select_card; //选好的牌
    public ushort w_left_card_type;					//选好后剩余的牌的可选牌型
};

//选好牌型对应的牌
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct STR_SelectedTypeCard
{
    ushort w_selected_type;                   //用户当前选择的牌型
    byte cb_selected_type_count;                //该牌型可以组合的数量(最多提供3种)
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
    public SelectedTypeCard[] cb_select_card; //摆牌，最多提供四种（一键摆牌）
};

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct SelectedTypeCard
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] selectCard; //摆牌，最多提供四种
}



//游戏结束
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_GameEnd
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lGameScore; //游戏积分

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lUserScore; //玩家分数
    public byte bEndMode;                           //结束方式(正常结束：0；解散：1)

}

#endregion

#region 框架命令
//游戏配置
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_GF_GameOptionNew
{
    byte cbAllowLookon;                     //旁观标志
    uint dwFrameVersion;                       //框架版本
    uint dwClientVersion;                  //游戏版本
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    byte[] cb_game_rule;//房间信息
};
#endregion
//游戏命令

#region
#endregion
#endregion



//=============================================牛牛========================================//
//创建房间信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct DBO_GR_CreateSuccess
{
    public uint dwUserID;                 //用户标识
    public long lCellScore;                    //设置底分
    public long dCurBeans;                   //当前游戏豆
    public long lRoomCard;                 //当前房卡
    public uint dwTableID;                    //预分配桌号
    public uint dwDrawCountLimit;         //局数限制
    public uint dwDrawTimeLimit;          //时间限制
    public ushort wJoinGamePeopleCount;      //参与游戏的人数
    public uint dwRoomTax;                    //单独一个私人房间的税率，千分比
    public byte cbIsJoinGame;              //玩家是否参与游戏
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 66)]
    public byte[] szPassword;//密码设置
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] szClientAddr;//创建房间的IP地址
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
    public byte[] cbGameRule;//游戏规则 弟 0 位标识 是否设置规则 0 代表设置 1 代表未设置
};

//游戏开始
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_NN_GameStart
{
    //下注信息
    public long lTurnMaxScore;                     //最大下注
    public ushort wBankerUser;                       //庄家用户
    public byte cbGameMode;                            //游戏模式，分为自由抢庄和通比模式
  
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] cbPlayStatus;//用户状态
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public CARDDATA[] cbCardData;//用户扑克
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ushort[] wSpecialType;//特殊牌型
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] cbCallStatus;//叫庄状态
};

//用户叫庄(发送)
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_C_CallBanker
{
    public byte bBanker;                                        //做庄标志(1叫庄，抢庄，0不叫)
}

//用户叫庄（返回）
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_CallBanker
{
    public ushort wCallBanker;                       //叫庄用户
    public byte bFirst;
    public byte bBanker;                       //叫庄倍数
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] cbPlayerStatus; //玩家状态
};


//发牌数据包(自由)
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_NN_S_SendCard
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public CARDDATA[] cbCardData;//用户扑克
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] wSpecialType;//特殊牌型
};

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CARDDATA
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] CardData;//用户扑克
}

//返回发牌数据包（明牌）
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_S_AllCard
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] bAICount;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public AllCARD[] cbCardData;//用户扑克
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct AllCARD
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
    public byte[] CardData;
}

//用户摊牌()
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_C_OxCard
{
    public byte bOX;								//牛牛标志
};

//用户摊牌
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct CMD_S_Open_Card
{
    public ushort wPlayerID;                         //摊牌用户
    public byte bOpen;                              //摊牌标志(1亮)
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
    public byte[] cbHandCardData;
};

//游戏结束
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_NN_S_GameEnd
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lGameTax;  //游戏税收
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public long[] lGameScore;  //游戏得分
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] cbCardData;  //用户扑克

    public byte cbDelayOverGame;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public byte[] bfiveKing;  //五花牛标识
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
    public ushort[] wSpecialType;  //特殊牌型
};
//玩家请求房间成绩
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GR_QUERY_USER_ROOM_SCORE
{
    public uint dwUserID;                         //用户 I D
    public uint dwKindID;								//房间类型
};
//用户语音
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_C_UserVoice
{
    public uint dwTargetUserID;                       //目标用户
    public uint dwVoiceLength;                        //语音长度
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15000)]
    //public byte[] byVoiceData;		//语音数据
};

//用户语音
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_GF_S_UserVoice
{
    public uint dwSendUserID;                     //发送用户
    public uint dwTargetUserID;                       //目标用户
    public uint dwVoiceLength;                        //语音长度
    //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15000)]
    //public byte[] byVoiceData;		//语音数据
};

//返回战绩
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct tagQueryPersonalRoomUserScore
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] szRoomID;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
    public byte[] szUserNicname;      //用户昵称    .
    public uint dwPlayTurnCount;   //私人放进行游戏的最大局数
    public uint dwPlayTimeLimit;      //私人房进行游戏的最大时间 单位秒
    public SYSTEMTIME sysCreateTime;   //私人房间创建时间
    public SYSTEMTIME sysDissumeTime; //私人房间结束时
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public tagPersonalUserScoreInfo[] PersonalUserScoreInfo;//私人房间所有玩家信息                  
};
//用于请求单个私人房间信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_QueryPersonalScore
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] szRoomID;
};
//返回单个私人房间信息
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_ROUND_LIST
{
    public byte count;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 30)]
    public CMD_MB_ROUND_INFO[] roomInfo;
   
};
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_ROUND_INFO
{
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public CMD_MB_ROUND_SCORE[] roomScore;
};
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public struct CMD_MB_ROUND_SCORE
{
    public uint dwUserID;
    public long IScore;
};


public struct SYSTEMTIME
{
    public ushort wYear;
    public ushort wMonth;
    public ushort wDayOfWeek;
    public ushort wDay;
    public ushort wwHour;
    public ushort wMinute;
    public ushort wSecond;
    public ushort wMillisends;

}



