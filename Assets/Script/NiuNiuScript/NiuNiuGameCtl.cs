using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AssemblyCSharp;

public class NiuNiuGameCtl : MonoBehaviour
{

    //=========单列===============//
    public static NiuNiuGameCtl instance;
    public static BottomScript bottomScript;
    //=========房间信息============//
    public Text RoomNum;
    public Text RoomCost;
    public Text PeopelNum;
    public Text RoomJuShu;
    //=======房间内图片信息======//
    public Image Readly;
    //===========设置按钮弹出框===============//
    public GameObject PutDowm;

    public List<PlayerItemScript> playerItems;
    public List<PlayerItemScript> mPlayerItems;
    public List<GameObject> shouPai;
    public GameObject faPai;
    public GameObject pai;
    public GameObject uiShouPai;
    private Color32 _color32 = new Color32(255, 255, 255, 255);
    private Color32 color = new Color32(255, 255, 255, 0);
    private int myChairId = -1;

    public GameObject readyBtn;
    void Awake()
    {
        AddListener();
    }
    public void AddListener()
    {

        SocketEventHandle.Instance.otherJoinRoomReply += OnOtherJoinRoomReply;
        //SocketEventHandle.Instance.quitRoomReply += OnQuitRoomReply;
        SocketEventHandle.Instance.startGameReply += StartGameReply;
    }
    private void InitChair()
    {
        GlobalDataScript.type = ModeType.None;
        mPlayerItems = new List<PlayerItemScript>();
        myChairId = -1;
        if (GlobalDataScript.Instance.roomInfo.playerNum == 3)
        {
            mPlayerItems.Add(playerItems[0]);
            mPlayerItems.Add(playerItems[2]);
            mPlayerItems.Add(playerItems[4]);
        }
        else if (GlobalDataScript.Instance.roomInfo.playerNum == 4)
        {

        }
        for (int i = 0; i < mPlayerItems.Count; i++)
        {
            mPlayerItems[i].gameObject.SetActive(true);
        }
    }

    private void StartGameReply(ClientResponse response)
    {

    }

    private void RemoveListener()
    {
        SocketEventHandle.Instance.otherJoinRoomReply -= OnOtherJoinRoomReply;

       // SocketEventHandle.Instance.quitRoomReply -= OnQuitRoomReply; ;
        SocketEventHandle.Instance.startGameReply -= StartGameReply;
    }

    void Start()
    {
        StartCoroutine(Begin());
    }

    public void OnOtherJoinRoomReply(ClientResponse response)
    {
        PlayerGameRoomInfo avatar = NetUtil.JsonToObj<PlayerGameRoomInfo>(response.message);  
        SetSeat(avatar);
    }
    /// <summary>
    /// 设置当前角色的座位
    /// </summary>
    /// <param name="avatar">Avatar.</param>
    private void SetSeat(PlayerGameRoomInfo avatar)
    {
        MyDebug.Log("avatar.account.uuid:" + avatar.userID + "============== avatar.chairID:" + avatar.chairId);
        if (avatar.userID == GlobalDataScript.userData.dwUserID)
        {
            mPlayerItems[0].SetPlayerInfo(avatar);
        }
        else
        {
            myChairId = GetMyChairId();
            int seatIndex = myChairId - avatar.chairId;
            if (seatIndex < 0)
            {
                seatIndex = GlobalDataScript.Instance.roomInfo.playerNum + seatIndex;
            }

            mPlayerItems[seatIndex].SetPlayerInfo(avatar);
        }
    }

    private int GetMyChairId()
    {
        if (myChairId >= 0)
            return myChairId;
        for (int i = 0; i < GlobalDataScript.Instance.playerInfos.Count; i++)
        {
            if (GlobalDataScript.Instance.playerInfos[i].userID == GlobalDataScript.userData.dwUserID)
            {
                return GlobalDataScript.Instance.playerInfos[i].chairId;
            }
        }
        MyDebug.LogError("Do not found my Chaid id!");
        return -1;
    }


    IEnumerator Begin()
    {
        pai.gameObject.SetActive(true);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < shouPai.Count; j++)
            {
                iTween.MoveTo(faPai,
                    shouPai[j].GetComponentsInChildren<SpriteRenderer>()[i].gameObject.transform.position, 1f);
                iTween.RotateTo(faPai,
                    shouPai[j].GetComponentsInChildren<SpriteRenderer>()[i].gameObject.transform.eulerAngles, 1f);
                iTween.ScaleTo(faPai,
                    shouPai[j].GetComponentsInChildren<SpriteRenderer>()[i].gameObject.transform.localScale, 1f);
                yield return new WaitForSeconds(1);
                faPai.transform.localPosition = new Vector3(0, 2.32f, -1.35f);
                faPai.transform.localScale = Vector3.one;
                shouPai[j].GetComponentsInChildren<SpriteRenderer>()[i].color = _color32;
            }
        }

        faPai.SetActive(false);
        for (int i = 0; i < 4; i++)
        {
            uiShouPai.GetComponentsInChildren<Image>()[i].transform.localEulerAngles = Vector3.zero;
            int cardNum = Random.Range(0, 53);
            
            uiShouPai.GetComponentsInChildren<Image>()[i].sprite =
                Resources.Load("Image/Puke/card_" + cardNum, typeof(Sprite)) as Sprite;
            shouPai[0].GetComponentsInChildren<SpriteRenderer>()[i].color = color;
            uiShouPai.GetComponentsInChildren<Image>()[i].color = _color32;
        }
    }

    IEnumerator DiPai()
    {
        faPai.SetActive(true);
        for (int i = 0; i < shouPai.Count; i++)
        {
            iTween.MoveTo(faPai,
                shouPai[i].GetComponentsInChildren<SpriteRenderer>()[4].gameObject.transform.position, 1f);
            iTween.RotateTo(faPai,
                shouPai[i].GetComponentsInChildren<SpriteRenderer>()[4].gameObject.transform.eulerAngles, 1f);
            iTween.ScaleTo(faPai,
                shouPai[i].GetComponentsInChildren<SpriteRenderer>()[4].gameObject.transform.localScale, 1f);
            yield return new WaitForSeconds(1);
            faPai.transform.localPosition = new Vector3(0, 2.32f, -1.35f);
            faPai.transform.localScale = Vector3.one;
            if (i != 0)
            {
                shouPai[i].GetComponentsInChildren<SpriteRenderer>()[4].color = _color32;
            }
            else
            {
                //生成翻牌UI
            }
        }

        pai.gameObject.SetActive(false);
    }
}