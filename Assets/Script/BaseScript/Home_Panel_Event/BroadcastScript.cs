using UnityEngine;
using AssemblyCSharp;
using System.Collections.Generic;

public class BroadcastScript : MonoBehaviour
{
    private void AddListener()
    {
        SocketEventHandle.Instance.broadcastNoticeReply += BroadcastNoticeReply;
    }

    private void RemoveListener()
    {
        SocketEventHandle.Instance.broadcastNoticeReply -= BroadcastNoticeReply;
    }

    private void BroadcastNoticeReply(ClientResponse response)
    {
        var noticeString = response.message;
        var noticeList = noticeString.Split(new char[1] {'*'});
        if (noticeList != null)
        {
            GlobalDataScript.noticeMegs = new List<string>();
            for (int i = 0; i < noticeList.Length; i++)
            {
                GlobalDataScript.noticeMegs.Add(noticeList[i]);
            }

            if (CommonEvent.GetInstance.displayBroadcastReply != null)
            {
                CommonEvent.GetInstance.displayBroadcastReply();
            }
        }
    }
}