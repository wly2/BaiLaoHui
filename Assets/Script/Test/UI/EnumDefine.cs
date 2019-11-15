//===================================================
//作    者：
//创建时间：2015-12-01 21:51:56
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 语言
/// </summary>
public enum Language
{
    CN,
    EN
}

#region SceneType 场景类型
/// <summary>
/// 场景类型
/// </summary>
public enum SceneType
{
    LogOn,
    SelectRole,
    WorldMap,
    GameLevel,
}
#endregion

/// <summary>
/// 消息类型
/// </summary>
public enum MessageViewType
{
    Ok,
    OkAndCancel
}

#region WindowUIType 窗口类型
/// <summary>
/// 窗口类型
/// </summary>
[XLua.LuaCallCSharp]
public enum WindowUIType
{
    /// <summary>
    /// 未设置
    /// </summary>
    None,
    /// <summary>
    /// 登录窗口
    /// </summary>
    LogOn,
    /// <summary>
    /// 注册窗口
    /// </summary>
    Reg,
    /// <summary>
    /// 进入区服
    /// </summary>
    GameServerEnter,
    /// <summary>
    /// 区服选择
    /// </summary>
    GameServerSelect,
    /// <summary>
    /// 角色信息
    /// </summary>
    RoleInfo,
    /// <summary>
    /// 游戏关卡地图
    /// </summary>
    GameLevelMap,
    /// <summary>
    /// 游戏关卡详情
    /// </summary>
    GameLevelDetail,
    /// <summary>
    /// 游戏关卡胜利
    /// </summary>
    GameLevelVictory,
    /// <summary>
    /// 游戏关卡失败
    /// </summary>
    GameLevelFail,
    /// <summary>
    /// 世界地图
    /// </summary>
    WorldMap,
    /// <summary>
    /// 世界地图失败
    /// </summary>
    WorldMapFail,
    /// <summary>
    /// 测试窗口 by NIKEY
    /// </summary>
    TestWindow
}
#endregion

#region WindowUIContainerType UI容器类型
/// <summary>
/// UI容器类型
/// </summary>
public enum WindowUIContainerType
{
    /// <summary>
    /// 左上
    /// </summary>
    TopLeft,
    /// <summary>
    /// 右上
    /// </summary>
    TopRight,
    /// <summary>
    /// 左下
    /// </summary>
    BottomLeft,
    /// <summary>
    /// 右下
    /// </summary>
    BottomRight,
    /// <summary>
    /// 居中
    /// </summary>
    Center
}
#endregion

#region WindowShowStyle 窗口打开方式
/// <summary>
/// 窗口打开方式
/// </summary>
public enum WindowShowStyle
{
    /// <summary>
    /// 正常打开
    /// </summary>
    Normal,
    /// <summary>
    /// 从中间放大
    /// </summary>
    CenterToBig,
    /// <summary>
    /// 从上往下
    /// </summary>
    FromTop,
    /// <summary>
    /// 从下往上
    /// </summary>
    FromDown,
    /// <summary>
    /// 从左向右
    /// </summary>
    FromLeft,
    /// <summary>
    /// 从右向左
    /// </summary>
    FromRight
}
#endregion

#region RoleType 角色类型
/// <summary>
/// 角色类型
/// </summary>
public enum RoleType
{
    /// <summary>
    /// 未设置
    /// </summary>
    None = 0,
    /// <summary>
    /// 当前玩家
    /// </summary>
    MainPlayer = 1,
    /// <summary>
    /// 怪
    /// </summary>
    Monster = 2,
    /// <summary>
    /// 其他玩家
    /// </summary>
    OTherRole = 3
}
#endregion

/// <summary>
/// 角色状态
/// </summary>
public enum HeroState
{
    /// <summary>
    /// 未设置
    /// </summary>
    None = 0,
    /// <summary>
    /// 休息
    /// </summary>
    Rest = 1,
    /// <summary>
    /// 待机
    /// </summary>
    Idle = 2,
    /// <summary>
    /// 移动
    /// </summary>
    Move = 3,
    /// <summary>
    /// 攻击
    /// </summary>
    Attack = 4,
    /// <summary>
    /// 受伤
    /// </summary>
    Damage = 5,
    /// <summary>
    /// 死亡
    /// </summary>
    Die = 6,
    /// <summary>
    /// 胜利
    /// </summary>
    Win = 7,
    /// <summary>
    /// 航行
    /// </summary>
    Sailing = 8
}

public enum HeroRestType
{
    /// <summary>
    /// 伸懒腰
    /// </summary>
    Gym,
    /// <summary>
    /// 躺着
    /// </summary>
    Lying
}

public enum HeroIdleType
{
    /// <summary>
    /// 普通待机
    /// </summary>
    Idle,
    /// <summary>
    /// 炮击手待机
    /// </summary>
    IdleGunner,
    /// <summary>
    /// 游泳中待机
    /// </summary>
    IdleSwim,
}

public enum HeroMoveType
{
    /// <summary>
    /// 陆地上走路
    /// </summary>
    Walk,
    /// <summary>
    /// 炮击手走路
    /// </summary>
    GunnerWalk,
    /// <summary>
    /// 游泳
    /// </summary>
    Swim
}

public enum HeroWinType
{
    /// <summary>
    /// 陆地上胜利
    /// </summary>
    Win,
    /// <summary>
    /// 水中胜利
    /// </summary>
    WinSwim
}