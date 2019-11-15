using AssemblyCSharp;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainNewsDataManager : MonoSingleton<MainNewsDataManager>
{
    public List<QueryInfo> myQureyList;
    public List<NewsItem> newsList;  
    public QueryInfo currentQuery;
    public List<CMD_MB_ROUND_INFO> myRoundQureylist;

    public void SetNewsInfo(string mes)
    {
        newsList = NetUtil.JsonToObj<List<NewsItem>>(mes);
        Debug.Log(newsList.Count);
    }
    public void AddQuerys(tagQueryPersonalRoomUserScore item, int kindId)
    {
        if (item.PersonalUserScoreInfo[0].dwUserID <= 0)
            return;
        if (myQureyList == null)
            myQureyList = new List<QueryInfo>();

        QueryInfo info = new QueryInfo();
        info.kindId = kindId;
        info.timeStr = item.sysDissumeTime.wSecond + item.sysDissumeTime.wMinute 
            * 100 + item.sysDissumeTime.wwHour 
            * 10000 + item.sysDissumeTime.wDay 
            * 1000000 + item.sysDissumeTime.wMonth 
            * 100000000 + item.sysDissumeTime.wYear 
            * 10000000000;
        Debug.Log(info.timeStr);
        info.queryinfo = item;

        SortQueryList(info);
    }
    public void ClearQuerys()
    {
        if (myQureyList == null)
            return;
        myQureyList.Clear();
    }
    public void SortQueryList(QueryInfo info)
    {
        for (int i = 0; i < myQureyList.Count; i++)
        {
            if (info.timeStr > myQureyList[i].timeStr)
            {
                myQureyList.Insert(i, info);
                return;
            }
        }
        myQureyList.Add(info);
    }  
    public void SetMyQueryList(CMD_MB_ROUND_LIST roundList)
    {                              
        if (myRoundQureylist == null)
            myRoundQureylist = new List<CMD_MB_ROUND_INFO>();
      //  myRoundQureylist.Clear();
        for(int i = 0;i<roundList.count;i++)
        {
            if(roundList.roomInfo.Length> i)
                myRoundQureylist.Add(roundList.roomInfo[i]);
        }
        SocketEventHandle.Instance.SetClientResponse(APIS.ZHANJI_DETAIL_REPORTER_REPONSE, null);
    }
}
public class NewsItem
{
    public string id;
    public string title;
    public string content;
    public string create_time;
    public string is_read;
}
public class QueryInfo
{
    public int kindId;
    public long timeStr;
    public tagQueryPersonalRoomUserScore queryinfo;

}


