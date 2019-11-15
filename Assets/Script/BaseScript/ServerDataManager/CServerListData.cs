using AssemblyCSharp;
using System.Collections.Generic;

public class CServerListData
{
    public List<TagGameKind> tagGameKindList;
    public List<TagGameServer> tagGameServerList;
    private static CServerListData _instance;

    public static CServerListData Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CServerListData();
            return _instance;
        }
    }

    //插入类型
    public void InsertGameKind(TagGameKind pGameKind)
    {
        if (tagGameKindList == null)
            tagGameKindList = new List<TagGameKind>();
        tagGameKindList.Add(pGameKind);
    }

    //插入房间
    public void InsertGameServer(TagGameServer pGameServer)
    {
        if (tagGameServerList == null)
            tagGameServerList = new List<TagGameServer>();
        tagGameServerList.Add(pGameServer);
        MyDebug.Log(" pGameServer.wServerID:" + pGameServer.wServerID);
    }

    public TagGameServer GetTagServerByKindID(int wKindID)
    {
        TagGameServer tag = new TagGameServer();
        tag.dwOnLineCount = 200000;
        tag.wServerPort = 9000;
        for (int i = 0; i < tagGameServerList.Count; i++)
        {
            if (tagGameServerList[i].wKindID != wKindID)
                continue;
            if (tag.dwOnLineCount > tagGameServerList[i].dwOnLineCount)
                tag = tagGameServerList[i];
        }

        return tag;
    }

    public TagGameServer GetTagServerByServerID(int serverId)
    {
        TagGameServer tag = new TagGameServer();
        tag.dwOnLineCount = 200000;
        for (int i = 0; i < tagGameServerList.Count; i++)
        {
            if (tagGameServerList[i].wServerID != serverId)
                continue;
            if (tag.dwOnLineCount > tagGameServerList[i].dwOnLineCount)
                tag = tagGameServerList[i];
        }

        return tag;
    }
}