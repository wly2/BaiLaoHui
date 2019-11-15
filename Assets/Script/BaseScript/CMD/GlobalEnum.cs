//=================================麻将=============================//
#region 麻将枚举
public enum MISSIONTYPE
{
    MISSION_LOGIN_ACCOUNT = 1,
    MISSION_LOGIN_GAMEID = 2,
    MISSION_REGISTER = 3,
    MISSION_UPDATE_INFO = 4,
    MISSION_SERVER_INFO = 5,
    MISSION_LOGIN_VISITOR = 6,
    MISSION_REQUEST_REPLAY_LIST = 7,
    MISSION_REQUEST_REPLAY_DATA = 8,
    MISSION_REQUEST_BUY_FANGKA = 9,
    MISSION_REQUEST_SHARE = 10,
}

//列表命令
public enum SUB_MB_LIST : int
{
    //列表信息
    SUB_MB_LIST_KIND = 100, //种类列表
    SUB_MB_LIST_SERVER = 101, //房间列表
    SUB_MB_LIST_MATCH = 102, //比赛列表

    //完成信息
    SUB_MB_LIST_FINISH = 200, //列表完成

    //在线信息
    SUB_MB_GET_ONLINE = 300, //获取在线
    SUB_MB_KINE_ONLINE = 301, //类型在线
    SUB_MB_SERVER_ONLINE = 302, //房间在线

    SUB_MB_AGENT_KIND = 400, //代理列表
}

public enum MDM_GP_LOGONtt
{
}

public enum MDM_CM_SYSTEM : int
{
    SUB_CM_SYSTEM_MESSAGE = 1 //系统消息
}

//
public enum MDM_GF_FRAME : int
{
    SUB_GF_USER_CHAT = 10, //用户聊天
    SUB_GR_TABLE_TALK = 12, //用户聊天
    SUB_GF_USER_EXPRESSION = 11, //用户表情
    SUB_GF_GAME_STATUS = 100, //游戏状态
    SUB_GF_GAME_SCENE = 101, //游戏场景
    SUB_GF_LOOKON_STATUS = 102, //旁观状态
    SUB_GF_SYSTEM_MESSAGE = 200, //系统消息
    SUB_GF_ACTION_MESSAGE = 201, //动作消息
    SUB_GF_GAME_OPTION = 1, //游戏配置
    SUB_GF_USER_READY = 2, //用户准备
    SUB_GF_LOOKON_CONFIG = 3, //旁观配置
    SUB_GR_MATCH_INFO = 403, //比赛信息
    SUB_GR_MATCH_WAIT_TIP = 404, //等待提示
    SUB_GR_MATCH_RESULT = 405 //游戏结果
}

public enum MDM_GR_MATCH : int
{
    SUB_GR_MATCH_FEE = 400, //报名费用
    SUB_GR_MATCH_NUM = 401, //等待人数
    SUB_GR_LEAVE_MATCH = 402, //退出比赛
    SUB_GR_MATCH_INFO = 403, //比赛信息
    SUB_GR_MATCH_WAIT_TIP = 404, //等待提示
    SUB_GR_MATCH_RESULT = 405, //比赛结果
    SUB_GR_MATCH_STATUS = 406, //比赛状态
    SUB_GR_MATCH_GOLDUPDATE = 409, //金币更新
    SUB_GR_MATCH_ELIMINATE = 410, //比赛淘汰
    SUB_GR_MATCH_JOIN_RESOULT = 411 //加入结果
}

//支付模式
public enum enPayMode
{
    ALL_PAY,                                //AA支付
    SINGLE_PAY,                             //房主单独支付
};

public enum DTP_GR_TABL : int
{
    DTP_GR_TABLE_PASSWORD = 1, //桌子密码

    //用户属性
    DTP_GR_NICK_NAME = 10, //用户昵称
    DTP_GR_GROUP_NAME = 11, //社团名字
    DTP_GR_UNDER_WRITE = 12, //个性签名

    //附加信息
    DTP_GR_USER_NOTE = 20, //用户备注
    DTP_GR_CUSTOM_FACE = 21, //自定头像
}

public enum REQUEST_FAILURE : int
{
    REQUEST_FAILURE_NORMAL = 0, //常规原因
    REQUEST_FAILURE_NOGOLD = 1, //金币不足
    REQUEST_FAILURE_NOSCORE = 2, //积分不足
    REQUEST_FAILURE_PASSWORD = 3, //密码错误
}

public enum MDM_GF : int
{
    MDM_GF_RECORD = 300 //录像回放命令
}

public enum SUB_REQUEST : int
{
    SUB_REQUEST_REPLAY_LIST = 410, //CMD_C_RequestReplayList//请求录像列表
    SUB_REQUEST_REPLAY_DATA = 411, //CMD_C_RequestReplayData请求录像数据
    SUB_RESPONSE_REPLAY_LIST = 412, //CMD_S_ResponseReplayList//响应录像列表
    SUB_RESPONSE_REPLAY_DATA = 413, //CMD_S_ResponseReplayData响应 录像数据
    SUB_REQUEST_RECHARGE = 414, //充值需求
    SUB_RESPONSE_RECHARGE = 415, //充值结果返回
    SUB_REQUEST_WCHAT_SHARE = 416, //微信分享
    SUB_RESPONSE_WCHAT_SHARE = 417, //微信分享结果返回
}

// 选择创建房间或加入房间
public enum ModeType
{
    None,
    Create,
    Join,
}

//麻将服务器命令结构
public enum SUB_S
{
    #region 麻将
    SUB_S_GAME_START = 100, //游戏开始
    SUB_S_OUT_CARD = 101, //出牌命令
    SUB_S_SEND_CARD = 102, //发送扑克
    SUB_S_OPERATE_NOTIFY = 104, //操作提示
    SUB_S_OPERATE_RESULT = 105, //操作命令
    SUB_S_GAME_END = 106, //游戏结束
    SUB_S_TRUSTEE = 107, //用户托管
    SUB_S_USER_INFO = 110, //用户信息
    SUB_S_CHAT = 112, //聊天命令
    SUB_S_CHU_ZENG = 114, //出弹消息
    SUB_S_CHU_ZENG_RESULT = 113 //出弹消息回应
    #endregion
}

//台州麻将 规则设置
public enum GAME_RULE
{
    GR_WU_DAN = 0,
    GR_YOU_DAN,
    GAME_RULE_NUM
}

//服务定义
public enum CARD
{
    INVALID_VALUE = 0xFF,
    time_start_game = 30, //开始定时器
    TIME_OPERATE_CARD = 15, //操作定时器
    CARD_COLOR_NULL = 0,
    CARD_COLOR_TONG = 1,
    CARD_COLOR_WAN = 2,
    CARD_COLOR_TIAO = 3,
}

//游戏属性
public enum GAME
{
    KIND_ID = 391, //游戏ID
}

//组件属性
public enum GAME_PLAYER
{
    GAME_PLAYER = 4, //游戏人数
}

//状态定义
public enum GAME_SCENE
{
    GAME_SCENE_FREE = 0, //等待开始
    GAME_SCENE_PLAY = 101, //游戏状态
    GAME_SCENE_CHU_ZENG = 102, //出弹状态
}

//常量定义
public enum CONSTANTS
{
    MAX_WEAVE = 4, //最大组合
    MAX_INDEX = 34, //最大索引
    MAX_COUNT = 14, //最大数目
    MAX_REPERTORY = 136, //最大库存
    ZI_PAI_START_INDEX = 27, //东风起始索引
    BAI_BAN_INDEX = 33, //白板索引
    LIU_JU_COUNT = 16, //流局剩余的牌数
    MAX_RIGHT_COUNT = 1, //最大权位DWORD个数
    LEN_ACCOUNTS = 32, //IP
    LEN_USER_NOTE = 256, //用户头像
    PRO_VERSION = 2, //程序版本号
}

//客户端命令结构
public enum SUB_C
{
    SUB_C_OUT_CARD = 1, //出牌命令
    SUB_C_OPERATE_CARD = 3, //操作扑克
    SUB_C_TRUSTEE = 4, //用户托管
    SUB_C_CHAT = 7, //聊天
    //SUB_C_CHU_ZENG = 7,                                    //出增回复
}

public enum MJ_PAI
{
    INVALID_PAI = 0x00,
    YI_TONG = 0x01,
    ER_TONG,
    SAN_TONG,
    SI_TONG,
    WU_TONG,
    LIU_TONG,
    QI_TONG,
    BA_TONG,
    JIU_TONG,
    YI_WAN = 0x11,
    ER_WAN,
    SAN_WAN,
    SI_WAN,
    WU_WAN,
    LIU_WAN,
    QI_WAN,
    BA_WAN,
    JIU_WAN,
    YI_SUO = 0x21,
    ER_SUO,
    SAN_SUO,
    SI_SUO,
    WU_SUO,
    LIU_SUO,
    QI_SUO,
    BA_SUO,
    JIU_SUO,
    DONG_FENG = 0x31,
    NAN_FENG,
    XI_FENG,
    BEI_FENG,
    HONG_ZHONG,
    FA_CAI,
    BAI_BAN
}

public enum WIK : byte
{
    WIK_NULL = 0x00, //没有类型
    WIK_LEFT = 0x01, //左吃类型
    WIK_CENTER = 0x02, //中吃类型
    WIK_RIGHT = 0x04, //右吃类型
    WIK_PENG = 0x08, //碰牌类型
    WIK_GANG = 0x10, //杠牌类型
    WIK_CHI_HU = 0x20, //吃胡类型
}

public enum MDM_SERVICE
{
    //个人资料
    SUB_GP_USER_INDIVIDUAL = 301, //个人资料
    SUB_GP_QUERY_INDIVIDUAL = 302, //查询信息
    SUB_GP_MODIFY_INDIVIDUAL = 303, //修改资料
    SUB_GP_QUERY_ACCOUNTINFO = 304, //个人信息
    SUB_GP_QUERY_INGAME_SEVERID = 305, //游戏状态

    //设置推荐人结果
    SUB_GP_SPREADER_RESOULT = 520, //设置推荐人结果

    //操作结果
    SUB_GP_OPERATE_SUCCESS = 900, //操作成功
    SUB_GP_OPERATE_FAILURE = 901, //操作失败
}
#endregion 



//==================================十三水===========================//
#region 十三水枚举
//十三水游戏模式
enum GameMode
{
    RED_WEAVE_MODE = 0x0,   //红波浪
    HORSE_CAED_MODE = 0x1,  //马牌
    KING_MODE = 0x2,    //大小王(百变)	
    NULL_SUPPORT_SPECIAL_TYPE = 0x4,  //特殊牌型
}

// 登录状态
public enum SUB_GP_LOGON_STATE
{
    SUB_MB_LOGON_SUCCESS = 100, //登录成功
    SUB_MB_LOGON_FAILURE = 101, //登录失败
    SUB_MB_MATCH_SIGNUPINFO = 102, //报名信息
    SUB_MB_PERSONAL_TABLE_CONFIG = 103, //私人房间配置
    SUB_MB_UPDATE_NOTIFY = 200, //升级提示
}

//登录界面登陆主命令
public enum MainCmd : int
{
    MDM_GP_LOGON = 1, //登录信息
    MDM_MB_LOGON = 100, //广场登录
    MDM_MB_SERVER_LIST = 101, //列表信息  
    MDM_MB_PERSONAL_SERVICE = 200,//私人房间
}
//登录界面子命令
public enum MDM_GR_LOGON : int
{
    SUB_MB_LOGON_GAMEID = 1, //I D 登录
    SUB_MB_LOGON_ACCOUNTS = 2, //帐号登录
    SUB_MB_REGISTER_ACCOUNTS = 3, //注册帐号
    SUB_MB_LOGON_OTHERPLATFORM = 4, //其他登陆  //微信编码5                                  
    SUB_MB_LOGON_VISITOR = 5, //游客登录
    MDM_MB_LOGON = 100, //广场登陆

}

//游戏框架子命令//100---
public enum SUB_GF_GAME_STATUS : int
{
    SUB_GF_GAME_OPTION = 1,                                   //游戏配置
    SUB_GF_USER_READY = 2,                                  //用户准备
    SUB_GF_LOOKON_CONFIG = 3,                                   //旁观配置

    //聊天命令
    SUB_GF_USER_CHAT = 10,                                  //用户聊天
    SUB_GF_USER_EXPRESSION = 11,                                    //用户表情
    SUB_GF_USER_VOICE = 12,                                 //用户语音

    //游戏信息
    SUB_GF_GAME_STATUS = 100,                                   //游戏状态
    SUB_GF_GAME_SCENE = 101,                                    //游戏场景
    SUB_GF_LOOKON_STATUS = 102,                                    //旁观状态

    //系统消息
    SUB_GF_SYSTEM_MESSAGE = 200,                                //系统消息
    SUB_GF_ACTION_MESSAGE = 201,                                //动作消息
    SUB_GF_PERSONAL_MESSAGE = 202,								//私人房消息
}



//游戏主命令
public enum GameServer : int
{
    MDM_GR_LOGON = 1,           //创建房间登录信息
    MDM_GR_CONFIG = 2,          //配置信息
    MDM_GR_USER = 3,            //用户信息
    MDM_GR_STATUS = 4,          //状态信息
    MDM_GR_INSURE = 5,          //用户信息
    MDM_GR_TASK = 6,            //任务命令
    MDM_GR_EXCHANGE = 7,        //兑换命令
    MDM_GR_PROPERTY = 8,        //道具命令
    MDM_GF_FRAME = 100,         //获取游戏配置主命令

    MDM_GF_GAME = 200,          //游戏命令
    MDM_GP_Cretate = 210,       //创建桌子   


    #region 旧代码
    //MDM_GR_CONFIG = 2, //配置信息
    //MDM_GR_USER = 3, //用户信息
    //MDM_GR_STATUS = 4, //状态信息
    //MDM_CM_SYSTEM = 1000, //系统命令
    // MDM_GF_GAME = 200, //游戏命令
    //MDM_GF_FRAME = 100, //框架命令
    //MDM_GR_MATCH = 9, //比赛命令
    //MDM_GR_PRIVATE = 10, //比赛命令
    //MDM_GR_CREATE = 11,//创建房间信息
    #endregion
}

//创建房间登录登录模式命令
public enum CreateLogon : int
{
    SUB_GR_LOGON_USERID = 1, //I D 登录
    SUB_GR_LOGON_MOBILE = 2,  //手机登录
    SUB_GR_LOGON_ACCOUNTS = 3, //账户登录

}

//创建房间登录结果命令 1---
public enum ResCretaeLogon : int
{
    SUB_GR_LOGON_SUCCESS = 100,   //登陆成功
    SUB_GR_LOGON_FAILURE = 101,  //登录失败
    SUB_GR_LOGON_FINISH = 102,   //登录完成

    SUB_GR_UPDATE_NOTIFY = 200,//升级提示
}

//配置命令 2---
public enum MDM_GR_CONFIG : int
{
    SUB_GR_CONFIG_COLUMN = 100, //列表配置
    SUB_GR_CONFIG_SERVER = 101, //房间配置
    SUB_GR_CONFIG_PROPERTY = 102, //道具配置
    SUB_GR_CONFIG_FINISH = 103, //配置完成
    SUB_GR_CONFIG_USER_RIGHT = 104, //玩家权限
}

//用户命令 3---
public enum MDM_GR_USER : int
{
    //用户动作
    SUB_GR_USER_RULE = 1, //用户规则
    SUB_GR_USER_LOOKON = 2, //旁观请求
    SUB_GR_USER_SITDOWN = 3, //坐下请求
    SUB_GR_USER_STANDUP = 4, //起立请求
    SUB_GR_USER_INVITE = 5, //用户邀请
    SUB_GR_USER_INVITE_REQ = 6, //邀请请求
    SUB_GR_USER_REPULSE_SIT = 7, //拒绝玩家坐下
    SUB_GR_USER_KICK_USER = 8, //踢出用户
    SUB_GR_USER_INFO_REQ = 9, //请求用户信息
    SUB_GR_USER_CHAIR_REQ = 10, //请求更换位置
    SUB_GR_USER_CHAIR_INFO_REQ = 11, //请求椅子用户信息
    SUB_GR_USER_WAIT_DISTRIBUTE = 12,//等待分配

    //用户状态
    SUB_GR_USER_ENTER = 100, //用户进入
    SUB_GR_USER_SCORE = 101, //用户分数
    SUB_GR_USER_STATUS = 102, //用户状态
    SUB_GR_SIT_FAILED = 103, //请求失败
    SUB_GR_USER_GAME_DATA = 104,//用户游戏数据

    //聊天命令
    SUB_GR_USER_CHAT = 201, //聊天消息
    SUB_GR_USER_EXPRESSION = 202, //表情消息
    SUB_GR_WISPER_CHAT = 203, //私聊消息
    SUB_GR_WISPER_EXPRESSION = 204, //私聊表情
    SUB_GR_COLLOQUY_CHAT = 205, //会话消息
    SUB_GR_COLLOQUY_EXPRESSION = 206, //会话表情

    //等级服务
    SUB_GR_GROWLEVEL_QUERY = 410, //查询等级
    SUB_GR_GROWLEVEL_PARAMETER = 411,//等级参数
    SUB_GR_GROWLEVEL_UPGRADE = 412,//等级升级
    //SUB_GR_PROPERTY_BUY = 300, //购买道具
    //SUB_GR_PROPERTY_SUCCESS = 301, //道具成功
    //SUB_GR_PROPERTY_FAILURE = 302, //道具失败
    //SUB_GR_PROPERTY_EFFECT = 304, //道具效应
    //SUB_GR_PROPERTY_MESSAGE = 303, //道具消息
    //SUB_GR_PROPERTY_TRUMPET = 305, //喇叭消息
    //SUB_GR_GLAD_MESSAGE = 400 //喜报消息
}

//状态命令 4---
public enum MDM_GR_STATUS : int
{
    SUB_GR_TABLE_INFO = 100, //桌子信息
    SUB_GR_TABLE_STATUS = 101 //桌子状态
}

//银行命令 5---
public enum MDM_GR_INSURE : int
{
    //银行命令
    SUB_GR_ENABLE_INSURE_REQUEST = 1,//开通银行
    SUB_GR_QUERY_INSURE_INFO = 2,//查询银行
    SUB_GR_SAVE_SCORE_REQUEST = 3,//存款操作
    SUB_GR_TAKE_SCORE_REQUEST = 4,//取款操作
    SUB_GR_TRANSFER_SCORE_REQUEST = 5,//取款操作
    SUB_GR_QUERY_USER_INFO_REQUEST = 6,//查询用户

    //
    SUB_GR_USER_INSURE_INFO = 100,//银行资料
    SUB_GR_USER_INSURE_SUCCESS = 101,//银行成功
    SUB_GR_USER_INSURE_FAILURE = 102, //银行失败
    SUB_GR_USER_TRANSFER_USER_INFO = 103,//用户资料
    SUB_GR_USER_INSURE_ENABLE_RESULT = 104,//开通结果
}

//任务命令 6---
public enum MDM_GR_TASK : int
{
    SUB_GR_TASK_LOAD_INFO = 1,//加载任务
    SUB_GR_TASK_TAKE = 2,//领取任务
    SUB_GR_TASK_REWARD = 3,//任务奖励
    SUB_GR_TASK_GIVEUP = 4,//任务放弃


    SUB_GR_TASK_INFO = 11,//任务信息
    SUB_GR_TASK_FINISH = 12,//任务完成
    SUB_GR_TASK_LIST = 13,//任务列表
    SUB_GR_TASK_RESULT = 14,//任务结果
    SUB_GR_TASK_GIVEUP_RESULT = 15,//放弃结果
}

//兑换命令 7---
public enum MDM_GR_EXCHANGE : int
{
    SUB_GR_EXCHANGE_LOAD_INFO = 400,//加载信息
    SUB_GR_EXCHANGE_PARAM_INFO = 401,//兑换参数
    SUB_GR_PURCHASE_MEMBER = 402,//购买会员
    SUB_GR_PURCHASE_RESULT = 403,//购买结果
    SUB_GR_EXCHANGE_SCORE_BYINGOT = 404,//兑换游戏币
    SUB_GR_EXCHANGE_SCORE_BYBEANS = 405,//兑换游戏币
    SUB_GR_EXCHANGE_RESULT = 406,//兑换结果
}

//道具命令 8---
public enum MDM_GR_PROPERTY : int
{
    //道具信息
    SUB_GR_QUERY_PROPERTY = 1,//道具查询
    SUB_GR_GAME_PROPERTY_BUY = 2,//购买道具
    SUB_GR_PROPERTY_BACKPACK = 3,//背包道具
    SUB_GR_PROPERTY_USE = 4,//物品使用
    SUB_GR_QUERY_SEND_PRESENT = 5,//查询赠送
    SUB_GR_PROPERTY_PRESENT = 6,//赠送道具
    SUB_GR_GET_SEND_PRESENT = 7,//获取赠送

    SUB_GR_QUERY_PROPERTY_RESULT = 101,//道具查询
    SUB_GR_GAME_PROPERTY_BUY_RESULT = 102,//购买道具
    SUB_GR_PROPERTY_BACKPACK_RESULT = 103,//背包道具
    SUB_GR_PROPERTY_USE_RESULT = 104,//物品使用
    SUB_GR_QUERY_SEND_PRESENT_RESULT = 105,//查询赠送
    SUB_GR_PROPERTY_PRESENT_RESULT = 106,//赠送道具
    SUB_GR_GET_SEND_PRESENT_RESULT = 107,//获取赠送
    SUB_GR_QUERY_PROPERTY_RESULT_FINISH = 111,//道具查询

    SUB_GR_PROPERTY_SUCCESS = 201,//道具成功
    SUB_GR_PROPERTY_FAILURE = 202,//道具失败
    SUB_GR_PROPERTY_MESSAGE = 203,//道具消息
    SUB_GR_PROPERTY_EFFECT = 204,//道具效应
    SUB_GR_PROPERTY_TRUMPET = 205,//喇叭消息
    SUB_GR_USER_PROP_BUFF = 206,//道具Buff
    SUB_GR_USER_TRUMPET = 207,//喇叭数量

    SUB_GR_GAME_PROPERTY_FAILURE = 404,//道具失败
};

//框架命令 100---
public enum MDM_GF_FRAME_BLH : int
{
    SUB_GF_GAME_OPTION = 1,//查询房间
    SUB_MB_QUERY_GAME_SERVER_RESULT = 205,//查询结果
    SUB_MB_SEARCH_SERVER_TABLE = 206,//搜索房间桌子
    SUB_MB_SEARCH_RESULT = 207,//搜索结果
    SUB_MB_GET_PERSONAL_PARAMETER = 208,//私人房间配置
    SUB_MB_PERSONAL_PARAMETER = 209,//私人房间配置
    SUB_MB_QUERY_PERSONAL_ROOM_LIST = 210,//请求私人房间列表
    SUB_MB_QUERY_PERSONAL_ROOM_LIST_RESULT = 211,//请求私人房间列表
    SUB_MB_PERSONAL_FEE_PARAMETER = 212,//私人房间配置
    SUB_MB_DISSUME_SEARCH_SERVER_TABLE = 213,//为解散桌子搜索ID
    SUB_MB_DISSUME_SEARCH_RESULT = 214,//解散桌子搜索房间ID结果
    SUB_MB_QUERY_USER_ROOM_INFO = 215,//玩家请求桌子信息
    SUB_GR_USER_QUERY_ROOM_SCORE = 216,//私人房间单个玩家请求房间成绩


}

//登录服私人房间状态 200---
public enum MDM_MB_PERSONAL_SERVICE : int
{
    SUB_MB_QUERY_GAME_SERVER = 204,//查询房间
    SUB_MB_QUERY_GAME_SERVER_RESULT = 205,//查询结果
    SUB_MB_SEARCH_SERVER_TABLE = 206,//搜索房间桌子
    SUB_MB_SEARCH_RESULT = 207,//搜索结果
    SUB_MB_GET_PERSONAL_PARAMETER = 208,//私人房间配置
    SUB_MB_PERSONAL_PARAMETER = 209,//私人房间配置
    SUB_MB_QUERY_PERSONAL_ROOM_LIST = 210,//请求私人房间列表
    SUB_MB_QUERY_PERSONAL_ROOM_LIST_RESULT = 211,//请求私人房间列表
    SUB_MB_PERSONAL_FEE_PARAMETER = 212,//私人房间配置
    SUB_MB_DISSUME_SEARCH_SERVER_TABLE = 213,//为解散桌子搜索ID
    SUB_MB_DISSUME_SEARCH_RESULT = 214,//解散桌子搜索房间ID结果
    SUB_MB_QUERY_USER_ROOM_INFO = 215,//玩家请求桌子信息
    SUB_GR_USER_QUERY_ROOM_SCORE = 216,//私人房间单个玩家请求房间成绩
    SUB_GR_USER_QUERY_ROOM_SCORE_RESULT = 217,//私人房间单个玩家请求房间成绩结果
    SUB_GR_USER_QUERY_ROOM_SCORE_RESULT_FINSIH = 218,//私人房间单个玩家请求房间成绩完成
    SUB_MB_QUERY_PERSONAL_ROOM_USER_INFO = 219,//私人房请求玩家的房卡和游戏豆
    SUB_MB_QUERY_PERSONAL_ROOM_USER_INFO_RESULT = 220,//私人房请求玩家的房卡和游戏豆结果
    SUB_MB_ROOM_CARD_EXCHANGE_TO_SCORE = 221,//房卡兑换游戏币
    SUB_GP_EXCHANGE_ROOM_CARD_RESULT = 222,//房卡兑换游戏币结果
    SUB_MB_GET_SINGLE_PERSONAL_SCORE = 223,// 单局战绩请求
}

public enum SHISANSHUISHOW : int
{
    NULLDAO,
    SHISANSHUITOUDAO = 1,
    SHISANSHUIZHONGDAO = 2,
    SHISANSHUIWEIDAO = 3
}

// 游戏服私人房间状态 210---
public enum MDM_GR_PRIVATE : int
{
    SUB_GR_CREATE_TABLE = 1, //创建桌子
    SUB_GR_CREATE_SUCCESS = 2, //创建成功
    SUB_GR_CREATE_FAILURE = 3, //创建失败
    SUB_GR_CANCEL_TABLE = 4, //解散桌子
    SUB_GR_CANCEL_REQUEST = 5, //请求解散
    SUB_GR_REQUEST_REPLY = 6, //请求答复
    SUB_GR_REQUEST_RESULT = 7, //请求结果
    SUB_GR_WAIT_OVER_TIME = 8,//超市等待
    SUB_GR_PERSONAL_TABLE_TIP = 9, //提示信息
    SUB_GR_PERSONAL_TABLE_END = 10, //结束消息
    SUB_GR_HOSTL_DISSUME_TABLE = 11,//房主强制解散桌子
    SUB_GR_HOST_DISSUME_TABLE_RESULT = 13, //解散桌子
    SUB_GR_CURRECE_ROOMCARD_AND_BEAN = 16, //解散桌子
    SUB_GF_PERSONAL_MESSAGE = 202//银币不足
}

//十三水服务器命令结构 240---
public enum SUB_S_SSS : int
{
    SUB_S_SHOW_CARD = 203,									//玩家摊牌
    SUB_S_GAME_START = 206,									//游戏开始
    SUB_S_SEND_CARD = 215,									//发牌消息
    SUN_S_SELECT_CARD = 216,										//选牌消息
    SUB_S_GAME_END = 204,									//游戏结束
    SUB_S_COMPARE_CARD = 205,									//比较扑克
    SUB_S_PLAYER_EXIT = 210,									//用户强退 未写完
    SUB_S_ANDROID_BANKOPERATOR = 109,                                   //机器人银行操作

    SUB_S_ADMIN_STORAGE_INFO = 110,                         //刷新库存
    SUB_S_RESULT_ADD_USERROSTER = 111,                          //添加用户名单结果
    SUB_S_RESULT_DELETE_USERROSTER = 112,                           //删除用户名单结果
    SUB_S_UPDATE_USERROSTER = 113,                              //更新用户名单
    SUB_S_REMOVEKEY_USERROSTER = 114,                               //移除用户名单
    SUB_S_DUPLICATE_USERROSTER = 115,                               //重复用户名单

}

//牛牛服务器命令结构 250---
public enum MDM_GP_NNGAMESERVER : int
{
    SUB_S_GAME_START = 100,									//游戏开始
    SUB_S_ADD_SCORE = 101,									//加注结果
    SUB_S_PLAYER_EXIT = 102,									//用户强退
    SUB_S_SEND_CARD = 103,										//发牌消息
    SUB_S_GAME_END = 104,									//游戏结束
    SUB_S_OPEN_CARD = 105,									//用户摊牌
    SUB_S_CALL_BANKER = 106,									//用户叫庄
    SUB_S_ALL_CARD = 107,									//发牌消息
    SUB_S_ANDROID_BANKOPERATOR = 109,                                   //机器人银行操作

    SUB_S_ADMIN_STORAGE_INFO = 110,                          //刷新库存
    SUB_S_RESULT_ADD_USERROSTER = 111,                           //添加用户名单结果
    SUB_S_RESULT_DELETE_USERROSTER = 112,                              //删除用户名单结果
    SUB_S_UPDATE_USERROSTER = 113,                               //更新用户名单
    SUB_S_REMOVEKEY_USERROSTER = 114,                               //移除用户名单
    SUB_S_DUPLICATE_USERROSTER = 115,    //重复用户名单
}

//牛牛客户端命令结构 260---
public enum MDM_GP_CLIENT : int
{
    SUB_C_CALL_BANKER = 1,									//用户叫庄
    SUB_C_ADD_SCORE = 2,									//用户加注
    SUB_C_OPEN_CARD = 3,									//用户摊牌
    SUB_C_SPECIAL_CLIENT_REPORT = 4,										//特殊终端
    SUB_C_MODIFY_STORAGE = 104,									//修改库存
    SUB_C_REQUEST_ADD_USERROSTER = 5,									//请求添加用户名单
    SUB_C_REQUEST_DELETE_USERROSTER = 6,									//请求删除用户名单
    SUB_C_REQUEST_UPDATE_USERROSTER = 7,									//请求更新用户名单
}

//普通牌型定义
public enum SUB_S_COMMONCARD
{
    HT_INVALID = 0x0000,                            //错误类型

    HT_SINGLE = 0x0001,                         //单牌类型(乌龙)
    HT_ONE_DOUBLE = 0x0002,                         //只有一对
    HT_TWO_DOUBLE = 0x0004,                         //两对牌型
    HT_THREE = 0x0008,                          //三张牌型(头道为三张；中道，尾道为3张+2张单张)
    HT_LINE = 0x0010,                           //顺子
    HT_COLOR = 0x0020,                          //同花
    HT_THREE_DEOUBLE = 0x0040,                          //葫芦
    HT_FOUR_BOOM = 0x0080,                          //铁枝
    HT_LINE_COLOR = 0x0100,							//同花顺
    HT_FIVE = 0x0200,						//5同
}


//客户端命令结构 200---
public enum SUB_C_SSS : int
{
    SUB_C_CALL_BANKER = 1,									//用户叫庄
    SUB_C_ADD_SCORE = 2,									//用户加注
    SUB_C_SHOWCARD = 304,									//用户摊牌
    SUB_C_COMPLETE_COMPARE = 306,										//完成比较
    SUB_C_SELECT_CARD = 307,									//用户选牌
    SUB_C_SPECIAL_CLIENT_REPORT = 4,									//特殊终端
    SUB_C_MODIFY_STORAGE = 6,									//修改库存
    SUB_C_REQUEST_ADD_USERROSTER = 7,                                   //请求添加用户名单

    SUB_C_REQUEST_DELETE_USERROSTER = 8,                         //请求删除用户名单
    SUB_C_REQUEST_UPDATE_USERROSTER = 9,                          //请求更新用户名单
}


#endregion

//====================================牛牛===========================//



//游戏模式
enum enGameMode
{
    INVALID_GAME_MODE,                      //无效游戏模式
    CALL_BANKER_MODE,                       //自由抢庄模式
    ORDINARY_MODE,                          //通比模式
    SHOW_CARD_MODE,                         //明牌抢庄模式
    NIUNIU_BANKER_MODE,						//牛牛抢庄

};

//特殊牌型
public enum SUB_C_SPCIALCARD : int
{
    TYPE_NULL = 0x0000,									//无分(手中找不到和为10的倍数的3张牌)
    TYPE_NIU = 0x0001,									//有分(手中3张牌和为10的倍数，另外2张牌的和不为10的倍数)
    TYPE_NIUNIU = 0x0002,									//牛牛(手中3张牌和为10的倍数，另外2张牌的和为10的倍数)
    TYPE_SIHUA = 0x0003,										//四花(手中4张牌为JQK，牛牛)
    TYPE_WUHUA = 0x0004,									//五花(手中5张牌全为JQK，牛牛)
    TYPE_SIZHADAN = 0x00010,									//四炸弹(五张牌中有4张一样的牌，牛牛)
    TYPE_WUXIAO = 0x00020,									//五小(五张牌加起来小于10点,牛牛)
}



public enum PU_KE
{
    F1 = 0x01,
    F2 = 0x02,
    F3,
    F4,
    F5,
    F6,
    F7,
    F8,
    F9,
    F10,
    F11,
    F12,
    F13,
    M1 = 0x11,
    M2,
    M3,
    M4,
    M5,
    M6,
    M7,
    M8,
    M9,
    M10,
    M11,
    M12,
    M13,
    HONG1 = 0x21,
    HONG2,
    HONG3,
    HONG4,
    HONG5,
    HONG6,
    HONG7,
    HONG8,
    HONG9,
    HONG10,
    HONG11,
    HONG12,
    HONG13,
    HENG1 = 0x31,
    HENG2,
    HENG3,
    HENG4,
    HENG5,
    HENG6,
    HENG7,
    HENG8,
    HENG9,
    HENG10,
    HENG11,
    HENG12,
    HENG13,
    DAWANG = 0x4F,
    XIAOWAGN = 0x4E
}
public enum playStatue
{
    US_NULL = 0x00,                     //没有状态
    US_FREE = 0x01,                         //站立状态
    US_SIT = 0x02,                      //坐下状态
    US_READY = 0x03,                        //同意状态
    US_LOOKON = 0x04,                           //旁观状态
    US_PLAYING = 0x05,                          //游戏状态
    US_OFFLINE = 0x06,
}
