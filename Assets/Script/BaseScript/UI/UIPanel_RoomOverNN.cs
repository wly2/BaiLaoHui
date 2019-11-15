using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel_RoomOverNN : UIWindow
{
    public List<PlayerRoomOverItemNN> list;
    public Text roomIdText;
    public Text JushuText;
    public Image imgState;
    public Button showimage;
    public Button  hindimage;
    public GameObject overpanel;

    private void Start()
    {
        roomIdText.text = GlobalDataScript.Instance.roomInfo.roomId;
        JushuText.text = GlobalDataScript.Instance.roomInfo.PlayGameCount + "" + "/" + GlobalDataScript.Instance.roomInfo.limtNumber;

        for (int i = 0; i < list.Count; i++)
        {
            if (i < GlobalDataScript.Instance.roomInfo.playerNum)
            {
                list[i].SetValue(i);
            }
            else
            {
                list[i].gameObject.SetActive(false);
            }
        }
    }
    
    public void show()
    {
        overpanel.SetActive(true);
        this.hindimage.gameObject.SetActive(true);
        this.showimage.gameObject.SetActive(false);
    }
    public void Hind()
    {
        overpanel.SetActive(false);
        this.hindimage.gameObject.SetActive(false);
        this.showimage.gameObject.SetActive(true);
    }
    public void Sure()
    {
        GlobalDataScript.Instance.isSigGameOver = true;
        CloseUI();
        MyDebug.LogError("GlobalDataScript.Instance.isExitGame:" + GlobalDataScript.Instance.isExitGame);
        if (GlobalDataScript.Instance.isExitGame)
            GlobalDataScript.Instance.showAllGameEnd = true;
        else
        {
            if (GameDataSSS.Instance.isSss)
            {
                GameDataSSS.Instance.ReadyGame();
            }
            else
            {
                SocketSendManager.Instance.SendPlayerReady();
            }
        }

        GlobalDataScript.Instance.isExitGame = false;

    }


}