using System.Runtime.InteropServices;
using AssemblyCSharp;

public class SocketPIndividualEveng : ISocketEvent
{
    private static SocketPIndividualEveng _instance;

    public static SocketPIndividualEveng Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SocketPIndividualEveng();
            }

            return _instance;
        }
    }

    public void ISocketEngineSink()
    {
        SocketEngine.Instance.SetSocketEvent(this);
        CMD_GP_QueryAccountInfo QueryIndividual = new CMD_GP_QueryAccountInfo
        {
            dwUserID = GlobalDataScript.userData.dwUserID
        };
        //SocketSendManager.Instance.SendData((int)MainCmd.MDM_GP_USER_SERVICE,
        //    (int) MDM_SERVICE.SUB_GP_QUERY_ACCOUNTINFO,
        //    NetUtil.StructToBytes(QueryIndividual), Marshal.SizeOf(QueryIndividual));
    }

    public bool OnEventTCPHeartTick()
    {
        return true;
    }

    public void OnEventTCPSocketError(int errorCode)
    {
    }

    public void OnEventTCPSocketLink()
    {
        //变量定义
    }

    public void OnEventTCPSocketRead(int main, int sub, byte[] tmpBuf, int size)
    {
        //if (main != (int) MAIN_CMD.MDM_GP_USER_SERVICE)
        //{
        //    return false;
        //}

        switch ((MDM_SERVICE) sub)
        {
            //个人信息
            case MDM_SERVICE.SUB_GP_QUERY_ACCOUNTINFO:
                OnSubUserAccountInfo(tmpBuf, size);
                break;
            //个人信息
            case MDM_SERVICE.SUB_GP_USER_INDIVIDUAL:
                OnSubUserIndividual(tmpBuf, size);
                break;
            //设置推荐人结果
            case MDM_SERVICE.SUB_GP_SPREADER_RESOULT:
                OnSubSpreaderResoult(tmpBuf, size);
                break;
            //操作成功
            case MDM_SERVICE.SUB_GP_OPERATE_SUCCESS:
                OnSubOperateSuccess(tmpBuf, size);
                break;
            //操作失败
            case MDM_SERVICE.SUB_GP_OPERATE_FAILURE:
                OnSubOperateFailure(tmpBuf, size);
                break;
            case MDM_SERVICE.SUB_GP_QUERY_INGAME_SEVERID:
                Net_InGameServerID(tmpBuf, size);
                break;
        }                 
    }

    void Net_InGameServerID(byte[] tmpBuf, int size)
    {
        if (size != Marshal.SizeOf(typeof(CMD_GP_InGameSeverID))) return;
        CMD_GP_InGameSeverID pNetInfo = NetUtil.BytesToStruct<CMD_GP_InGameSeverID>(tmpBuf);
        SocketEngine.Instance.SocketQuit();
        MyDebug.Log("KindID:"+pNetInfo.LockKindID) ;
        MyDebug.Log("ServerID:" + pNetInfo.LockServerID);
        if (pNetInfo.LockKindID == 0 && pNetInfo.LockServerID == 0)
        {
            UIPanel_Loading.instance.SetLoadPercentShow("Loading.....");
            MySceneManager.instance.ScentToMain();
        }
        else
        {
            SocketSendManager.Instance.ConnectGameServerByServerID((int) pNetInfo.LockServerID);
            UIPanel_Loading.instance.SetLoadPercentShow("Loading.....");
            //断线重连
        }

    }

    private void OnSubOperateFailure(byte[] tmpBuf, int size)
    {
        MyDebug.Log("操作失败!!!!!!!" + NetUtil.GetServerLog(tmpBuf));
        SocketEventHandle.Instance.SetTips("操作失败!!!!!!!" + NetUtil.GetServerLog(tmpBuf));
    }

    private void OnSubOperateSuccess(byte[] tmpBuf, int size)
    {
    }

    private void OnSubSpreaderResoult(byte[] tmpBuf, int size)
    {
    }

    private void OnSubUserIndividual(object data, int size)
    {
    }

    private void OnSubUserAccountInfo(byte[] data, int size)
    {
        CMD_GP_UserAccountInfo info = NetUtil.BytesToStruct<CMD_GP_UserAccountInfo>(data);
        GlobalDataScript.userInfo.szLogonIP = info.szLogonIp;
        GlobalDataScript.Instance.weChatInformation.ip = info.szLogonIp;
        CMD_GP_UserInGameServerID kNetInfo = new CMD_GP_UserInGameServerID
        {
            dwUserID = GlobalDataScript.userInfo.dwUserID
        };
        //SocketSendManager.Instance.SendData((int) MAIN_CMD.MDM_GP_USER_SERVICE,
        //    (int) MDM_SERVICE.SUB_GP_QUERY_INGAME_SEVERID, NetUtil.StructToBytes(kNetInfo), Marshal.SizeOf(kNetInfo));
    }

    public void OnEventTCPSocketShut()
    {
    }

    public void ISocketEngineSink(string ip, int port)
    {
        throw new System.NotImplementedException();
    }

    public void ISocketEngineSink(int kind_Id)
    {
        throw new System.NotImplementedException();
    }
}