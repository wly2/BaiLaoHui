using AssemblyCSharp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePayManager : MonoBehaviour
{
    public delegate void GameSetIntEvent(int _num);

    // Use this for initialization
    public static GameSetIntEvent RefershRoomCard;
    private static GamePayManager _instance;

    public static GamePayManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new GamePayManager();
            return _instance;

        }
    }

    public void BuyRoomCard(int goodID)
    {
        HttpManager.instance.GetWXPay(goodID, BuyRoomCardReply);
    }

    public void BuyRoomCardReply(WWW mes)
    {
        MyDebug.Log("BuyRoomCardReply:" + mes.text);
        WebPayItem item = NetUtil.JsonToObj<WebPayItem>(mes.text);
        string str = item.data.noncestr + "&" + item.data.package + "&" + item.data.partnerid + "&" +
                     item.data.prepayid + "&" + item.data.timestamp + "&" + item.data.sign + "&" + item.data.appid;
        UnityPhoneManager.Instance.WXPay(str);
    }

    public void BuyResult()
    {
        SocketEventHandle.Instance.SetClientResponse(APIS.CARD_CHANGE, null);
        MyDebug.Log("购买成功！！！！！！");
    }
}

public class WebPayItem
{
    public string code;
    public string msg;
    public string time;
    public string apiurl;
    public string ApiHash;
    public WebPayDataItem data;

}

public class WebPayDataItem
{
    public string appid;
    public string noncestr;
    public string package;
    public string partnerid;
    public string prepayid;
    public int timestamp;
    public string sign;

}