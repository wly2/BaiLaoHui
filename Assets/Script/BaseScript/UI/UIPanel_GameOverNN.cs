using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_GameOverNN : UIWindow
{
    public List<PlayerRoomOverItemNN> list;
    public Text roomIdText;
    public Text jushuNUM;
    public Image imgState;
    public Text ShowTimeDay;
    public Text ShowTime;
    private int hour;
    private int mintue;
    private int second;
    private int month;
    private int day;
    private int year;

    private void Start()
    {
        hour = System.DateTime.Now.Hour;
        mintue = System.DateTime.Now.Minute;
        second = System.DateTime.Now.Second;
        year = System.DateTime.Now.Year;
        month = System.DateTime.Now.Month;
        day = System.DateTime.Now.Day;


        ShowTimeDay.text = year + "/" + month + "/" + day;
        ShowTime.text = hour + ":" + mintue + ":" + second;

        roomIdText.text = GlobalDataScript.Instance.roomInfo.roomId;
        jushuNUM.text = GlobalDataScript.Instance.roomInfo.PlayGameCount.ToString() + "/" + GlobalDataScript.Instance.roomInfo.limtNumber;

        for (int i = 0; i < list.Count; i++)
        {
            if (i < GlobalDataScript.Instance.roomInfo.playerNum)
            {
                list[i].SetGameEndValue(i);

            }
            else
            {
                list[i].gameObject.SetActive(false);
            }
        }

    }

    public void Sure()
    {
        CloseUI();
        PlayerGameRoomInfo info = new PlayerGameRoomInfo();

        info.userID = (int)GlobalDataScript.userData.dwUserID;
        SetClientResponse(APIS.OUT_ROOM_RESPONSE, NetUtil.ObjToJson(info));
    }
    public void Share()
    {
        NetUtil.ShotSceneTexture();
    }
    private void SetClientResponse(int code, string mes)
    {
        MyDebug.Log("SetClientResponse:" + code);
        var cr = new ClientResponse
        {
            headCode = code,
            message = mes
        };
        SocketEventHandle.Instance.AddResponse(cr);
    }

}