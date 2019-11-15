public class Define
{
                                                     //数值定义
                                                     //头像大小
                                                     public static int FACE_CX = 48; //头像宽度

                                                     public static int FACE_CY = 48; //头像高度

                                                     //长度定义
                                                     public static int LEN_LESS_ACCOUNTS = 6; //最短账号
                                                     public static int LEN_LESS_NICKNAME = 6; //最短昵称

                                                     public static int LEN_LESS_PASSWORD = 6; //最短密码

                                                     //人数定义
                                                     public static int MAX_CHAIR = 100; //最大椅子
                                                     public static int MAX_TABLE = 512; //最大桌子
                                                     public static int MAX_COLUMN = 32; //最大列表
                                                     public static int MAX_ANDROID = 256; //最大机器
                                                     public static int MAX_PROPERTY = 128; //最大道具
                                                     public static int MAX_WHISPER_USER = 16; //最大私聊

                                                     public static int MAX_CHAIR_GENERAL = 8; //最大椅子

                                                     //列表定义
                                                     public static int MAX_KIND = 128; //最大类型

                                                     public static int MAX_SERVER = 1024; //最大房间

                                                     //参数定义
                                                     public static int INVALID_CHAIR = 0xFFFF; //无效椅子

                                                     public static int INVALID_TABLE = 0xFFFF; //无效桌子

                                                     //税收定义
                                                     public static long REVENUE_BENCHMARK; //税收起点

                                                     public static long REVENUE_DENOMINATOR = 1000L; //税收分母

                                                     //系统参数
                                                     //积分类型
                                                     //游戏状态
                                                     public static int GAME_STATUS_FREE; //空闲状态
                                                     public static int GAME_STATUS_PLAY = 100; //游戏状态

                                                     public static int GAME_STATUS_WAIT = 200; //等待状态

                                                     //系统参数
                                                     public static int LEN_USER_CHAT = 128; //聊天长度
                                                     public static long TIME_USER_CHAT = 1L; //聊天间隔

                                                     public static int TRUMPET_MAX_CHAR = 128; //喇叭长度

                                                     //索引质数
                                                     //列表质数
                                                     public static long PRIME_TYPE = 11L; //种类数目
                                                     public static long PRIME_KIND = 53L; //类型数目
                                                     public static long PRIME_NODE = 101L; //节点数目
                                                     public static long PRIME_PAGE = 53L; //自定数目

                                                     public static long PRIME_SERVER = 1009L; //房间数目

                                                     //人数质数
                                                     public static long PRIME_SERVER_USER = 503L; //房间人数
                                                     public static long PRIME_ANDROID_USER = 503L; //机器人数

                                                     public static long PRIME_PLATFORM_USER = 100003L; //平台人数

                                                     //数据长度
                                                     //资料数据
                                                     public static int LEN_MD5 = 33; //加密密码
                                                     public static int LEN_USERNOTE = 32; //备注长度
                                                     public static int LEN_ACCOUNTS = 32; //账号长度
                                                     public static int LEN_NICKNAME = 32; //昵称长度
                                                     public static int LEN_PASSWORD = 33; //密码长度
                                                     public static int LEN_GROUP_NAME = 32; //社团名字
                                                     public static int LEN_UNDER_WRITE = 32; //个性签名
                                                     public static int LEN_SIGIN = 5; //签到天数
                                                     public static int LEN_BEGINNER = 32; //新手活动长度

                                                     public static int LEN_ADDRANK = 50; //新手活动长度

                                                     //长度宏定义
                                                     public static int NAME_LEN = 32; //名字长度
                                                     public static int PASS_LEN = 33; //密码长度
                                                     public static int EMAIL_LEN = 32; //邮箱长度
                                                     public static int GROUP_LEN = 32; //社团长度
                                                     public static int COMPUTER_ID_LEN = 33; //机器序列

                                                     public static int UNDER_WRITE_LEN = 32; //个性签名

                                                     //数据长度
                                                     public static int LEN_QQ = 16; //QQ号码
                                                     public static int LEN_EMAIL = 33; //电子邮件
                                                     public static int LEN_USER_NOTE = 256; //用户备注
                                                     public static int LEN_SEAT_PHONE = 33; //固定电话
                                                     public static int LEN_MOBILE_PHONE = 12; //移动电话
                                                     public static int LEN_PASS_PORT_ID = 19; //证件号码
                                                     public static int LEN_COMPELLATION = 16; //真实名字

                                                     public static int LEN_DWELLING_PLACE = 128; //联系地址

                                                     //机器标识
                                                     public static int LEN_NETWORK_ID = 13; //网卡长度

                                                     public static int LEN_MACHINE_ID = 33; //序列长度

                                                     //长度宏定义
                                                     public static int LEN_TYPE = 32; //种类长度
                                                     public static int LEN_KIND = 32; //类型长度
                                                     public static int LEN_STATION = 32; //站点长度
                                                     public static int LEN_SERVER = 32; //房间长度
                                                     public static int LEN_MODULE = 32; //进程长度

                                                     public static int MAX_MATCH_DESC = 4;

                                                     //用户关系
                                                     public static int CP_NORMAL; //未知关系
                                                     public static int CP_FRIEND = 1; //好友关系
                                                     public static int CP_DETEST = 2; //厌恶关系

                                                     public static int CP_SHIELD = 3; //屏蔽聊天

                                                     //性别定义
                                                     public static int GENDER_FEMALE; //女性性别

                                                     public static int GENDER_MANKIND = 1; //男性性别

                                                     //游戏模式
                                                     public static int GAME_GENRE_GOLD = 0x0001; //金币类型
                                                     public static int GAME_GENRE_SCORE = 0x0002; //点值类型
                                                     public static int GAME_GENRE_MATCH = 0x0004; //比赛类型

                                                     public static int GAME_GENRE_EDUCATE = 0x0008; //训练类型

                                                     //分数模式
                                                     public static int SCORE_GENRE_NORMAL = 0x0100; //普通模式

                                                     public static int SCORE_GENRE_POSITIVE = 0x0200; //非负模式

                                                     //扣费类型
                                                     public static int MATCH_FEE_TYPE_GOLD; //扣费类型

                                                     public static int MATCH_FEE_TYPE_MEDAL = 0x01; //扣费类型

                                                     //比赛类型
                                                     public static int MATCH_TYPE_LOCKTIME; //定时类型

                                                     public static int MATCH_TYPE_IMMEDIATE = 0x01; //即时类型

                                                     //用户状态
                                                     public static byte US_NULL; //没有状态
                                                     public static byte US_FREE = 0x01; //站立状态
                                                     public static byte US_SIT = 0x02; //坐下状态
                                                     public static byte US_READY = 0x03; //同意状态
                                                     public static byte US_LOOKON = 0x04; //旁观状态
                                                     public static byte US_PLAYING = 0x05; //游戏状态

                                                     public static byte US_OFFLINE = 0x06; //断线状态

                                                     //比赛状态
                                                     public static int MS_NULL; //没有状态
                                                     public static int MS_SIGNUP = 0x01; //报名状态
                                                     public static int MS_MATCHING = 0x02; //比赛状态

                                                     public static int MS_OUT = 0x03; //淘汰状态

                                                     //房间规则
                                                     public static int SRL_LOOKON = 0x00000001; //旁观标志
                                                     public static int SRL_OFFLINE = 0x00000002; //断线标志

                                                     public static int SRL_SAME_IP = 0x00000004; //同网标志

                                                     //房间规则
                                                     public static int SRL_ROOM_CHAT = 0x00000100; //聊天标志
                                                     public static int SRL_GAME_CHAT = 0x00000200; //聊天标志
                                                     public static int SRL_WISPER_CHAT = 0x00000400; //私聊标志

                                                     public static int SRL_HIDE_USER_INFO = 0x00000800; //隐藏标志

                                                     //列表数据
                                                     //无效属性
                                                     public static int UD_NULL; //无效子项
                                                     public static int UD_IMAGE = 100; //图形子项

                                                     public static int UD_CUSTOM = 200; //自定子项

                                                     //基本属性
                                                     public static int UD_GAME_ID = 1; //游戏标识
                                                     public static int UD_USER_ID = 2; //用户标识

                                                     public static int UD_NICKNAME = 3; //用户昵称

                                                     //扩展属性
                                                     public static int UD_GENDER = 10; //用户性别
                                                     public static int UD_GROUP_NAME = 11; //社团名字

                                                     public static int UD_UNDER_WRITE = 12; //个性签名

                                                     //状态信息
                                                     public static int UD_TABLE = 20; //游戏桌号

                                                     public static int UD_CHAIR = 21; //椅子号码

                                                     //积分信息
                                                     public static int UD_SCORE = 30; //用户分数
                                                     public static int UD_GRADE = 31; //用户成绩
                                                     public static int UD_USER_MEDAL = 32; //用户经验
                                                     public static int UD_EXPERIENCE = 33; //用户经验
                                                     public static int UD_LOVELINESS = 34; //用户魅力
                                                     public static int UD_WIN_COUNT = 35; //胜局盘数
                                                     public static int UD_LOST_COUNT = 36; //输局盘数
                                                     public static int UD_DRAW_COUNT = 37; //和局盘数
                                                     public static int UD_FLEE_COUNT = 38; //逃局盘数

                                                     public static int UD_PLAY_COUNT = 39; //总局盘数

                                                     //积分比率
                                                     public static int UD_WIN_RATE = 40; //用户胜率
                                                     public static int UD_LOST_RATE = 41; //用户输率
                                                     public static int UD_DRAW_RATE = 42; //用户和率
                                                     public static int UD_FLEE_RATE = 43; //用户逃率

                                                     public static int UD_GAME_LEVEL = 44; //游戏等级

                                                     //拓展信息
                                                     public static int UD_NOTE_INFO = 50; //用户备注

                                                     public static int UD_LOOKON_USER = 51; //旁观用户

                                                     //图像列表
                                                     public static int UD_IMAGE_FLAG = (UD_IMAGE + 1); //用户标志
                                                     public static int UD_IMAGE_GENDER = (UD_IMAGE + 2); //用户性别

                                                     public static int UD_IMAGE_STATUS = (UD_IMAGE + 3); //用户状态

                                                     //数据库定义
                                                     public static int DB_ERROR = -1; //处理失败
                                                     public static int DB_SUCCESS; //处理成功

                                                     public static int DB_NEEDMB = 18; //处理失败

                                                     //道具标示
                                                     public static int PT_USE_MARK_DOUBLE_SCORE = 0x0001; //双倍积分
                                                     public static int PT_USE_MARK_FOURE_SCORE = 0x0002; //四倍积分
                                                     public static int PT_USE_MARK_GUARDKICK_CARD = 0x0010; //防踢道具
                                                     public static int PT_USE_MARK_POSSESS = 0x0020; //附身道具

                                                     public static int MAX_PT_MARK = 4; //标识数目

                                                     //有效范围
                                                     public static int VALID_TIME_DOUBLE_SCORE = 3600; //有效时间
                                                     public static int VALID_TIME_FOUR_SCORE = 3600; //有效时间
                                                     public static int VALID_TIME_GUARDKICK_CARD = 3600; //防踢时间
                                                     public static int VALID_TIME_POSSESS = 3600; //附身时间

                                                     public static int VALID_TIME_KICK_BY_MANAGER = 3600; //游戏时间

                                                     //设备类型
                                                     public static int DEVICE_TYPE_PC; //PC
                                                     public static int DEVICE_TYPE_ANDROID = 0x10; //Android
                                                     public static int DEVICE_TYPE_ITOUCH = 0x20; //iTouch
                                                     public static int DEVICE_TYPE_IPHONE = 0x40; //iPhone

                                                     public static int DEVICE_TYPE_IPAD = 0x80; //iPad;

                                                     //手机定义
                                                     //视图模式
                                                     public static int VIEW_MODE_ALL = 0x0001; //全部可视

                                                     public static int VIEW_MODE_PART = 0x0002; //部分可视

                                                     //信息模式
                                                     public static int VIEW_INFO_LEVEL_1 = 0x0010; //部分信息
                                                     public static int VIEW_INFO_LEVEL_2 = 0x0020; //部分信息
                                                     public static int VIEW_INFO_LEVEL_3 = 0x0040; //部分信息

                                                     public static int VIEW_INFO_LEVEL_4 = 0x0080; //部分信息

                                                     //其他配置
                                                     public static int RECVICE_GAME_CHAT = 0x0100; //接收聊天
                                                     public static int RECVICE_ROOM_CHAT = 0x0200; //接收聊天

                                                     public static int RECVICE_ROOM_WHISPER = 0x0400; //接收私聊

                                                     //行为标识
                                                     public static int BEHAVIOR_LOGON_NORMAL; //普通登陆

                                                     public static int BEHAVIOR_LOGON_IMMEDIATELY = 0x1000; //立即登录

                                                     //处理结果
                                                     public static int RESULT_ERROR = -1; //处理错误
                                                     public static int RESULT_SUCCESS; //处理成功

                                                     public static int RESULT_FAIL = 1; //处理失败

                                                     //变化原因
                                                     public static int SCORE_REASON_WRITE; //写分变化
                                                     public static int SCORE_REASON_INSURE = 1; //银行变化
                                                     public static int SCORE_REASON_PROPERTY = 2; //道具变化
                                                     public static int SCORE_REASON_MATCH_FEE = 3; //比赛报名

                                                     public static int SCORE_REASON_MATCH_QUIT = 4; //比赛退赛

                                                     //登录房间失败原因
                                                     public static int LOGON_FAIL_SERVER_INVALIDATION = 200; //房间失效

                                                     //控制掩码
                                                     public static ushort SMT_CLOSE_ROOM = 0x0100; //关闭房间
                                                     public static ushort SMT_CLOSE_GAME = 0x0200; //关闭游戏

                                                     public static ushort SMT_CLOSE_LINK = 0x0400; //中断连接

                                                     //类型掩码
                                                     public static ushort SMT_CHAT = 0x0001; //聊天消息
                                                     public static ushort SMT_EJECT = 0x0002; //弹出消息
                                                     public static ushort SMT_GLOBAL = 0x0004; //全局消息
                                                     public static ushort SMT_PROMPT = 0x0008; //提示消息
                                                     public static ushort SMT_TABLE_ROLL = 0x0010; //滚动消息
                                                     public static int GAME_TYPE_DANLAOQI = 0x00000001; //三老起
                                                     public static int GAME_TYPE_ERLAOQI = 0x00000002; //二老起
                                                     public static int GAME_TYPE_HAOGANG = 0x00000004; //豪杠
                                                     public static int GAME_TYPE_10FENG = 0x00000008; //十风
                                                     public static int GAME_TYPE_NOTOP = 0x00000010; //不封顶
                                                     public static int GAME_TYPE_64TOP = 0x00000020; //64封顶
                                                     public static int GAME_TYPE_128TOP = 0x00000040; //128封顶
}