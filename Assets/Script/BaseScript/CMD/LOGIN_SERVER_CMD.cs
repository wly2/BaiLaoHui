public class LOGIN_SERVER_CMD
{
    public enum MAIN_CMD
    {
        MDM_GP_LOGON = 1,
        MDM_GP_SERVER_LIST = 2, //列表信息
        MDM_GP_USER_SERVICE = 3, //用户服务
        MDM_GP_REMOTE_SERVICE = 4, //远程服务
        MDM_MB_LOGON = 100, //广场登陆
        MDM_MB_SERVER_LIST = 101, //列表信息
    }

    public enum MDM_GP_LOGON
    {
        //登录模式
        SUB_MB_LOGON_GAMEID = 1, //I D 登录
        SUB_MB_LOGON_ACCOUNTS = 2, //帐号登录
        SUB_MB_REGISTER_ACCOUNTS = 3, //注册帐号
        SUB_MB_LOGON_OTHERPLATFORM = 4, //其他登陆                                    
        SUB_MB_LOGON_VISITOR = 5, //游客登录
        MDM_MB_LOGON = 100, //广场登陆
    }
    public enum MDM_GP_LOGON_RESULT
    {
        //登录结果
        SUB_GP_LOGON_SUCCESS = 100, //登录成功
        SUB_GP_LOGON_FAILURE = 101, //登录失败
        SUB_GP_LOGON_FINISH = 102, //登录完成
        SUB_GP_VALIDATE_MBCARD = 103, //登录失败
        //升级提示
        SUB_GP_UPDATE_NOTIFY = 200, //升级提示
    }

    public enum MDM_GP_SERVER_LIST
    {
        //获取命令
        SUB_GP_GET_LIST = 1, //获取列表
        SUB_GP_GET_SERVER = 2, //获取房间
        SUB_GP_GET_MATCH = 3, //获取比赛
        SUB_GP_GET_ONLINE = 4, //获取在线
        SUB_GP_GET_COLLECTION = 5, //获取收藏

        //列表信息
        SUB_GP_LIST_TYPE = 100, //类型列表
        SUB_GP_LIST_KIND = 101, //种类列表
        SUB_GP_LIST_NODE = 102, //节点列表
        SUB_GP_LIST_PAGE = 103, //定制列表
        SUB_GP_LIST_SERVER = 104, //房间列表
        SUB_GP_LIST_MATCH = 105, //比赛列表
        SUB_GP_VIDEO_OPTION = 106, //视频配置

        //完成信息
        SUB_GP_LIST_FINISH = 200, //发送完成
        SUB_GP_SERVER_FINISH = 201, //房间完成
        SUB_GP_MATCH_FINISH = 202, //比赛完成

        //在线信息
        SUB_GR_KINE_ONLINE = 300, //类型在线
        SUB_GR_SERVER_ONLINE = 301, //房间在线
    }

    public enum MDM_GP_USER_SERVICE
    {
        //帐号服务
        SUB_GP_MODIFY_MACHINE = 100, //修改机器
        SUB_GP_MODIFY_LOGON_PASS = 101, //修改密码
        SUB_GP_MODIFY_INSURE_PASS = 102, //修改密码
        SUB_GP_MODIFY_UNDER_WRITE = 103, //修改签名

        //修改头像
        SUB_GP_USER_FACE_INFO = 120, //头像信息
        SUB_GP_SYSTEM_FACE_INFO = 122, //系统头像
        SUB_GP_CUSTOM_FACE_INFO = 123, //自定头像

        //个人资料
        SUB_GP_USER_INDIVIDUAL = 140, //个人资料
        SUB_GP_QUERY_INDIVIDUAL = 141, //查询信息
        SUB_GP_MODIFY_INDIVIDUAL = 152, //修改资料

        //银行服务
        SUB_GP_USER_ENABLE_INSURE = 160, //开通银行
        SUB_GP_USER_SAVE_SCORE = 161, //存款操作
        SUB_GP_USER_TAKE_SCORE = 162, //取款操作
        SUB_GP_USER_TRANSFER_SCORE = 163, //转帐操作
        SUB_GP_USER_INSURE_INFO = 164, //银行资料
        SUB_GP_QUERY_INSURE_INFO = 165, //查询银行
        SUB_GP_USER_INSURE_SUCCESS = 166, //银行成功
        SUB_GP_USER_INSURE_FAILURE = 167, //银行失败
        SUB_GP_QUERY_USER_INFO_REQUEST = 168, //查询用户
        SUB_GP_QUERY_USER_INFO_RESULT = 169, //用户信息
        SUB_GP_USER_INSURE_ENABLE_RESULT = 170, //开通结果

        //比赛服务
        SUB_GP_MATCH_SIGNUP = 200, //比赛报名
        SUB_GP_MATCH_UNSIGNUP = 201, //取消报名
        SUB_GP_MATCH_SIGNUP_RESULT = 202, //报名结果

        //签到服务
        SUB_GP_CHECKIN_QUERY = 220, //查询签到
        SUB_GP_CHECKIN_INFO = 221, //签到信息
        SUB_GP_CHECKIN_DONE = 222, //执行签到
        SUB_GP_CHECKIN_RESULT = 223, //签到结果

        //任务服务
        SUB_GP_TASK_LOAD = 240, //加载任务
        SUB_GP_TASK_TAKE = 241, //领取任务
        SUB_GP_TASK_REWARD = 242, //任务奖励
        SUB_GP_TASK_INFO = 243, //任务信息
        SUB_GP_TASK_LIST = 244, //任务信息
        SUB_GP_TASK_RESULT = 245, //任务结果

        //低保服务
        SUB_GP_BASEENSURE_LOAD = 260, //加载低保
        SUB_GP_BASEENSURE_TAKE = 261, //领取低保
        SUB_GP_BASEENSURE_PARAMETER = 262, //低保参数
        SUB_GP_BASEENSURE_RESULT = 263, //低保结果

        //推广服务
        SUB_GP_SPREAD_QUERY = 280, //推广奖励
        SUB_GP_SPREAD_INFO = 281, //奖励参数

        //等级服务
        SUB_GP_GROWLEVEL_QUERY = 300, //查询等级
        SUB_GP_GROWLEVEL_PARAMETER = 301, //等级参数
        SUB_GP_GROWLEVEL_UPGRADE = 302, //等级升级

        //兑换服务
        SUB_GP_EXCHANGE_QUERY = 320, //兑换参数
        SUB_GP_EXCHANGE_PARAMETER = 321, //兑换参数
        SUB_GP_PURCHASE_MEMBER = 322, //购买会员
        SUB_GP_PURCHASE_RESULT = 323, //购买结果
        SUB_GP_EXCHANGE_SCORE_BYINGOT = 324, //兑换游戏币
        SUB_GP_EXCHANGE_SCORE_BYBEAN = 325, //兑换游戏币
        SUB_GP_EXCHANGE_RESULT = 326, //兑换结果

        //抽奖服务
        SUB_GP_LOTTERY_CONFIG_REQ = 340, //请求配置
        SUB_GP_LOTTERY_CONFIG = 341, //抽奖配置
        SUB_GP_LOTTERY_USER_INFO = 342, //抽奖信息
        SUB_GP_LOTTERY_START = 343, //开始抽奖
        SUB_GP_LOTTERY_RESULT = 344, //抽奖结果

        //游戏服务
        SUB_GP_QUERY_USER_GAME_DATA = 360, //查询数据

        //操作结果
        SUB_GP_OPERATE_SUCCESS = 500, //操作成功
        SUB_GP_OPERATE_FAILURE = 501, //操作失败
    }

    public enum MDM_GP_REMOTE_SERVICE
    {
        //查找服务
        SUB_GP_C_SEARCH_DATABASE = 100, //数据查找
        SUB_GP_C_SEARCH_CORRESPOND = 101, //协调查找

        //查找服务
        SUB_GP_S_SEARCH_DATABASE = 200, //数据查找
        SUB_GP_S_SEARCH_CORRESPOND = 201, //协调查找
    }

    public enum MDM_MB_LOGON
    {
        //登陆模式
        SUB_MB_LOGON_GAMEID = 1, //I D登陆
        SUB_MB_LOGON_ACCOUNTS = 2, //帐号登陆
        SUB_MB_REGISITER_ACCOUNTS = 3, //注册帐号
        SUB_MB_LOGON_OTHERPLATFORM = 4, //其他登陆
        SUB_MB_LOGON_VISITOR = 5, //游客登录
        SUB_MB_ONLINE_CHECK = 6, //在线检测
        SUB_MB_LOGON_SUCCESS = 100, //登陆成功
        SUB_MB_LOGON_FAILURE = 101, //登录失败

        //升级提示
        SUB_MB_UPDATE_NOTIFY = 200, //升级提示
    }

    public enum MDM_MB_SERVER_LIST
    {
        SUB_MB_LIST_KIND = 100, //种类列表
        SUB_MB_LIST_SERVER = 101, //房间列表
        SUB_MB_MATCH_SERVER = 102, //比赛列表
        SUB_MB_LIST_FINISH = 200, //列表完成
    }
}