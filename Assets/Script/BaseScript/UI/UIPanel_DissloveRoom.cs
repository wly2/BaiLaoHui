using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using LitJson;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;

/**申请解散房间投票框**/

public class UIPanel_DissloveRoom : UIWindow
{
    public GameObject player;
    public Text CancelRequestName;
    public Button butSure;
    public Button butCancel;
    public List<PlayerResult> playerList;
    public Text timerText;
    private float timer = GameConfig.GAME_DEFALUT_AGREE_TIME;
    private void Awake()
    {
        addListener();
    }
    void Update()
    {
        if (timer >= 0)
        {         
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                if(butSure.gameObject.activeSelf)
                    clickOk();
                return;
            }
            timerText.text = Math.Floor(timer) + "";
        }
    }
    private void OnDestroy()
    {
        removeListener();
    }

    private void addListener()
    {

        SocketEventHandle.Instance.dissolveRoomReply += DissolveRoomReply;

    }
    public void removeListener()
    {
        SocketEventHandle.Instance.dissolveRoomReply -= DissolveRoomReply;
    }

    public void Init(int userID)
    {
        playerList = new List<PlayerResult>();
        for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)
        {
            if (GlobalDataScript.Instance.playerInfos[i].userID == userID)
                CancelRequestName.text = GlobalDataScript.Instance.playerInfos[i].name;
            else 
            {
                if(playerList.Count<=0)
                {
                    PlayerResult pr = player.GetComponent<PlayerResult>();
                    playerList.Add(pr);
                    pr.setInitVal(GlobalDataScript.Instance.playerInfos[i]);
                }
                else
                {
                    GameObject go = Instantiate(player);
                    go.transform.SetParent(player.transform.parent);
                    go.transform.localScale = Vector3.one;
                    PlayerResult pr = go.GetComponent<PlayerResult>();
                    playerList.Add(pr);
                    pr.setInitVal(GlobalDataScript.Instance.playerInfos[i]);

                }
            }

            if(GlobalDataScript.Instance.myGameRoomInfo.userID == userID)
            {
                butSure.gameObject.SetActive(false);
                butCancel.gameObject.SetActive(false);
            }
        }

    }
    

    private void DissolveRoomReply(ClientResponse response)
    {
        DissloveRoomResponseVo req =NetUtil.JsonToObj<DissloveRoomResponseVo>(response.message);
        MyDebug.Log(req.userId + "--------++++++++++++++++++++++++++++++++++++++++++++++++++++--------" + req.result);
       
        if(req.userId==0)
        {
            CloseUI();
            return;
        }
        doDissoliveRoomRequest((int)req.userId, req.result);
    }
    private void doDissoliveRoomRequest(int usId,int agree)
    {
      for(int i = 0;i<playerList.Count;i++)
        {
            if(playerList[i].userId == usId)
            {
                playerList[i].changeResult(agree);
            }
        }
    }
    public void clickOk()
    {           
        doDissoliveRoomRequest(GlobalDataScript.Instance.myGameRoomInfo.userID, 1);
        butSure.gameObject.SetActive(false);
        butCancel.gameObject.SetActive(false);
        CMD_GR_RequestReply req = new CMD_GR_RequestReply();
        req.dwUserID = (uint)GlobalDataScript.Instance.myGameRoomInfo.userID;
        req.dwTableID = (uint)GlobalDataScript.Instance.myGameRoomInfo.tableId;
        req.cbAgree =1;
        SocketSendManager.Instance.SendData((int)GameServer.MDM_GP_Cretate,
                      (int)MDM_GR_PRIVATE.SUB_GR_REQUEST_REPLY, NetUtil.StructToBytes(req), Marshal.SizeOf(req));
    }

    public void clickCancle()
    {
        doDissoliveRoomRequest(GlobalDataScript.Instance.myGameRoomInfo.userID,0);
        butSure.gameObject.SetActive(false);
        butCancel.gameObject.SetActive(false);
        CMD_GR_RequestReply req = new CMD_GR_RequestReply();
        req.dwUserID = (uint)GlobalDataScript.Instance.myGameRoomInfo.userID;
        req.dwTableID = (uint)GlobalDataScript.Instance.myGameRoomInfo.tableId;
        req.cbAgree =0;
        SocketSendManager.Instance.SendData((int)GameServer.MDM_GP_Cretate,
                      (int)MDM_GR_PRIVATE.SUB_GR_REQUEST_REPLY, NetUtil.StructToBytes(req), Marshal.SizeOf(req));
    }
}
