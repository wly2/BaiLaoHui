public class CGameKindItem
{
    public TagGameKind m_GameKind; //类型信息

    //更新变量
    public bool m_bUpdateItem; //更新标志

    uint m_dwUpdateTime; //更新时间

    //扩展数据
    public uint m_dwProcessVersion; //游戏版本

    //重载函数
    public virtual ushort GetSortID()
    {
        return m_GameKind.wSortID;
    }
}

public class CGameServerItem
{
    //属性数据
    public TagGameServer m_GameServer; //房间信息

    //用户数据
    public bool m_bSignuped; //报名标识

    //扩展数据
    //辅助变量
    public CGameKindItem m_pGameKindItem; //游戏类型

    //重载函数
    public virtual ushort GetSortID()
    {
        return m_GameServer.wSortID;
    }

    //比赛房间
    public virtual bool IsMatchRoom()
    {
        return (m_GameServer.wServerType & Define.GAME_GENRE_MATCH) != 0;
    }

    //私人房间
    public virtual bool IsPrivateRoom()
    {
        return (m_GameServer.wServerType & Define.GAME_GENRE_EDUCATE) != 0;
    }
}