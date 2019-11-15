using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
 public class DataResource: Singleton<DataResource>
{

     public List<CMD_MB_LogonSuccess> InfoDate()
     {
         return GlobalDataScript.Instance.weChatInformationlist;
     }
     public GameRoomInfo RoomData()
     {
         return GlobalDataScript.Instance.roomInfo;
     }
     //public PlayerGameRoomInfo[] PlayerInfo()
     //{
     //    return GlobalDataScript.Instance.playerInfos;
     //}
     public void Readly()
     {
        MyDebug.Log("111");
         SocketSendManager.Instance.SendPlayerReady();
     }

}
