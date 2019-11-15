using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordButton : MonoBehaviour
{
    public Text timeText;
    public Text roomIdText;
    public Text dataText;
    public List<Text> nameList;
    public List<Text> scoreList;
    public Image type;
    string nameStr;
    string monthstr;
    string daystr;
    string hourStr;
    string secondstr;
    string millistr;
    public QueryInfo info;

    public void Btn()
    {
        MainNewsDataManager.instance.currentQuery = info;
        SocketLoginEvent.instance.GetQueryRoundScore(info.queryinfo.szRoomID);   
    }
    public void SetValue(QueryInfo value)
    {
        info = value;
        ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Hall/UIPanel_Record/Type"+info.kindId, (sprite) => {
            type.sprite = sprite;
            type.SetNativeSize();
        });
        roomIdText.text = NetUtil.BytesToString(info.queryinfo.szRoomID);
        if (info.queryinfo.sysDissumeTime.wMonth < 10)
            monthstr = "0" + info.queryinfo.sysDissumeTime.wMonth;
        else
            monthstr = info.queryinfo.sysDissumeTime.wMonth + "";
        if (info.queryinfo.sysDissumeTime.wDay < 10)
            daystr = "0" + info.queryinfo.sysDissumeTime.wDay;
        else
            daystr = info.queryinfo.sysDissumeTime.wDay + "";

        if (info.queryinfo.sysDissumeTime.wwHour < 10)
            hourStr = "0" + info.queryinfo.sysDissumeTime.wwHour;
        else
            hourStr = info.queryinfo.sysDissumeTime.wwHour + "";
        if (info.queryinfo.sysDissumeTime.wSecond < 10)
            secondstr = "0" + info.queryinfo.sysDissumeTime.wSecond;
        else
            secondstr = "" + info.queryinfo.sysDissumeTime.wSecond;
        if (info.queryinfo.sysDissumeTime.wMinute < 10)
            millistr = "0" + info.queryinfo.sysDissumeTime.wMinute;
        else
            millistr = "" + info.queryinfo.sysDissumeTime.wMinute;


        dataText.text = info.queryinfo.sysDissumeTime.wYear + "-" + monthstr + "-" + daystr;
        timeText.text = hourStr + ":" + millistr + ":" + secondstr ;

        for (int i = 0; i < nameList.Count; i++)
        {
            nameStr = NetUtil.BytesToString(info.queryinfo.PersonalUserScoreInfo[i].szUserNicname);
            if (info.queryinfo.PersonalUserScoreInfo[i].dwUserID <= 0)
            {
                nameList[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                nameList[i].transform.parent.gameObject.SetActive(true);
                nameList[i].text = NetUtil.BytesToString(info.queryinfo.PersonalUserScoreInfo[i].szUserNicname);
                scoreList[i].text = info.queryinfo.PersonalUserScoreInfo[i].lScore + "";
            }
        }
    }
    public void SetroundScore(CMD_MB_ROUND_INFO value,int dex)
    {
        info = MainNewsDataManager.instance.currentQuery;
        ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/Hall/UIPanel_Record/Type" + info.kindId, (sprite) => {
            type.sprite = sprite;
            type.SetNativeSize();
        });          

        dataText.text ="第 "+(dex+1)+" 局";                              

        for (int i = 0; i < nameList.Count; i++)
        {
            nameStr = NetUtil.BytesToString(info.queryinfo.PersonalUserScoreInfo[i].szUserNicname);
            if (info.queryinfo.PersonalUserScoreInfo[i].dwUserID <= 0)
            {
                nameList[i].transform.parent.gameObject.SetActive(false);
            }
            else
            {
                nameList[i].transform.parent.gameObject.SetActive(true);
                nameList[i].text = NetUtil.BytesToString(info.queryinfo.PersonalUserScoreInfo[i].szUserNicname);
                scoreList[i].text = value.roomScore[i].IScore + "";
            }
        }
    }
}