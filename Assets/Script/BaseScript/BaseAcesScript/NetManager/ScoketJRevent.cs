using AssemblyCSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScoketJRevent : SocketBaseEvent<ScoketJRevent>
{
    public override void ConnectGameServer()
    {
        ISocketEngineSink(APIS.socketUrl, APIS.socketPort);
    }


    //处理服务器返回消息
    public override void OnEventTCPSocketRead(int main, int sub, byte[] tmpBuf, int size)
    {
        switch ((MainCmd)main)
        {
            case MainCmd.MDM_GP_LOGON://1---
                break;
            case MainCmd.MDM_MB_LOGON://100---
                break;
            case MainCmd.MDM_MB_SERVER_LIST://101---
                break;
            case MainCmd.MDM_MB_PERSONAL_SERVICE://200---登录服私人房间
                LogInCommon(sub, tmpBuf, size);
                break;
            default:
                break;
        }
    }

    //登录服私人房间返回消息
    public void LogInCommon(int sub, byte[] buffer, int size)
    {
        switch ((MDM_MB_PERSONAL_SERVICE)sub)
        {
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_GAME_SERVER://200---204
                queryGomeInfo(buffer, size);
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_GAME_SERVER_RESULT://200---205
                resultGomeInfo(buffer, size);
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_SEARCH_SERVER_TABLE://200---206
               
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_SEARCH_RESULT://200---207
                Search_Result(buffer, size);
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_GET_PERSONAL_PARAMETER://200---208
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_PERSONAL_PARAMETER://200---209
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_PERSONAL_ROOM_LIST://200---210
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_PERSONAL_ROOM_LIST_RESULT://200---211
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_PERSONAL_FEE_PARAMETER://200---212
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_DISSUME_SEARCH_SERVER_TABLE://200---213
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_DISSUME_SEARCH_RESULT://200---214
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_USER_ROOM_INFO://200---215
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_GR_USER_QUERY_ROOM_SCORE://200---216
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_GR_USER_QUERY_ROOM_SCORE_RESULT://200---217
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_GR_USER_QUERY_ROOM_SCORE_RESULT_FINSIH://200---218
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_PERSONAL_ROOM_USER_INFO://200---219
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_QUERY_PERSONAL_ROOM_USER_INFO_RESULT://200---220
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_MB_ROOM_CARD_EXCHANGE_TO_SCORE://200---221
                break;
            case MDM_MB_PERSONAL_SERVICE.SUB_GP_EXCHANGE_ROOM_CARD_RESULT://200---222
                break;
            default:
                break;
        }
    }

    #region 读取结构方法
    //查找房间
    public void queryGomeInfo(byte[] tmpBuf, int size)
    {
        CMD_MB_QueryGameServer createLoginFail = NetUtil.BytesToStruct<CMD_MB_QueryGameServer>(tmpBuf);       
        MyDebug.Log("###############################" + (createLoginFail.dwUserID));
        MyDebug.Log("###############################" + createLoginFail.dwUserID);
    }

    //查询结果
    public void resultGomeInfo(byte[] tmpBuf, int size)
    {
        //CMD_MB_QueryGameServerResult queryResult = NetUtil.BytesToStruct<CMD_MB_QueryGameServerResult>(tmpBuf);

        //MyDebug.Log("###############################" + queryResult.dwServerID);
        //MyDebug.Log("###############################" + queryResult.szErrDescrybe);
        //TagGameServer serverInfo = CServerListData.Instance.GetTagServerByServerID((int)queryResult.dwServerID);


        //CMD_MB_SearchServerTable table = new CMD_MB_SearchServerTable();
        //table.szServerID = new byte[14];

        //Array.Copy(send_buffer, table.szServerID, send_buffer.Length);
        //table.dwKindID = serverInfo.wKindID;

        //byte[] bytes = NetUtil.StructToBytes(table);

        //SocketEngine.Instance.SendScoketData((int)MainCmd.MDM_MB_PERSONAL_SERVICE, (int)MDM_MB_PERSONAL_SERVICE.SUB_MB_SEARCH_SERVER_TABLE, bytes, bytes.Length);



    }

    //返回搜索房间结果
    public void Search_Result(byte[] tmpBuf, int size)
    {
        CMD_MB_SearchResult result = NetUtil.BytesToStruct<CMD_MB_SearchResult>(tmpBuf);
       
        if (result.dwServerID == 0)
        {
            MyDebug.Log("返回搜索房间结果");
            SocketEventHandle.Instance.SetTips("房间号不存在！！！" );
            SocketEngine.Instance.SocketQuit();
            return;
        }
        SocketEngine.Instance.SocketQuit(false);
        TagGameServer serverInfo = CServerListData.Instance.GetTagServerByServerID((int)result.dwServerID);

        if (serverInfo.wKindID == GlobalDataScript.SSS_KIND_ID)
        {
            SocketSSSEvent.instance.Init();
            SocketSSSEvent.instance.tableId = (int)result.dwTableID;
            SocketSSSEvent.instance.ConnectGameServerByServerID(serverInfo.wServerID);
        }

        //牛牛待添加...
        else if (serverInfo.wKindID == GlobalDataScript.NN_KIND_ID)
        {
            SocketNiuNiuEvent.instance.Init();
            SocketNiuNiuEvent.instance.tableId = (int)result.dwTableID;
            SocketNiuNiuEvent.instance.ConnectGameServerByServerID(serverInfo.wServerID);
        }
           
    }
    #endregion

    //连接服务器成功返回
    public override void OnEventTCPSocketLink()
    {
        SocketEngine.Instance.SendScoketData((int)MainCmd.MDM_MB_PERSONAL_SERVICE,
           (int)MDM_MB_PERSONAL_SERVICE.SUB_MB_SEARCH_SERVER_TABLE, send_buffer, send_buffer.Length);
    }

    ////游戏服登录发送
    //public void LogInCreateRoom()
    //{
    //    if (buffer != null && buffer.Length > 4)
    //        SocketEngine.Instance.SendScoketData((int)GameServer.MDM_GP_Cretate, (int)MDM_GR_PRIVATE.SUB_GR_CREATE_TABLE, buffer, buffer.Length);
    //}

}

