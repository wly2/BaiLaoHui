using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class SocketBaseEvent<T> : ISocketEvent where T : new()
{
    public bool isDisslove;
    public int nnstatus;
    public bool isDisConnect;
    public int tableId = -1;
    public int chairId =-1;
    public int myStatue = -1;
    public TagGameServer serverInfo;
    public string socketip;
    int socketPort;
    private static T _instance;
    public byte[] send_buffer;
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new T();

            }
            return _instance;
        }
    }
    public void Init()
    {
        isDisslove = false;
        nnstatus = -1;
        isDisConnect = false;
        tableId = -1;
        chairId = -1;
        myStatue = -1;
        serverInfo = new TagGameServer() ;
        socketip = null;
        socketPort = 0;
        send_buffer = null;
        GlobalDataScript.Instance.isExitGame = false;
        GlobalDataScript.Instance.isSigGameOver = false;
    }
    public void ISocketEngineSink(int kind_Id)
    {
        serverInfo = CServerListData.Instance.GetTagServerByKindID(kind_Id);
        socketip = NetUtil.BytesToString(serverInfo.szServerAddr);
        socketPort = serverInfo.wServerPort;
        ISocketEngineSink();
    }
    public void ISocketEngineSinkByServerId(int serverId)
    {
        serverInfo = CServerListData.Instance.GetTagServerByServerID(serverId);
        socketip = NetUtil.BytesToString(serverInfo.szServerAddr);
        socketPort = serverInfo.wServerPort;
        ISocketEngineSink();
    }
    public void ISocketEngineSink(string ip, int port)
    {
        socketip = ip;
        socketPort = port;
        ISocketEngineSink();
    }

    public virtual void ISocketEngineSink()
    {
        SocketEngine.Instance.SocketQuit(false);
        SocketEngine.Instance.SetSocketEvent(this);
        if (socketPort > 0 && socketip != null)
            SocketEngine.Instance.InitSocket(socketip, socketPort);
       // UIManager.instance.Show(UIType.UILoading);
    }

    //心跳包
    public bool OnEventTCPHeartTick()
    {
        throw new System.NotImplementedException();
    }

    //服务器连接错误
    public virtual void OnEventTCPSocketError(int errorCode)
    {

    }

    //连接服务器成功并返回
    public virtual void OnEventTCPSocketLink()
    {

    }

    //返回服务器消息
    public virtual void OnEventTCPSocketRead(int main, int sub, byte[] tmpBuf, int size)
    {

    }

    //断开服务器
    public void OnEventTCPSocketShut()
    {
        throw new System.NotImplementedException();
    }

    //连接服务器
    public virtual void ConnectGameServer()
    {

    }
    //连接服务器
    public virtual void ConnectGameServerByServerID(int serverId)
    {

    }

}
