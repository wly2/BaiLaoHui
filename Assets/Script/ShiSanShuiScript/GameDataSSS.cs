using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataSSS : Singleton<GameDataSSS> {
                                    
    public bool isSss = false;
    public bool isRefreshShouPai;
    public bool isPutOn;
    public int choseCount;
    public List<int> choiceCard;
    public List<int> lastCard;

    public List<int> tdCardId;
    public List<int> zdCardId;
    public List<int> wdCardId;
    public SUB_S_COMMONCARD choiceType;
    public CMD_S_ShowCard playersCard;
    public CMD_S_Compare sCompare;
    public CMD_S_GameEnd gameEnd;
    public CMD_GR_PersonalTableEnd AllgameEnd;
    public CMD_S_StatusPlaySSS gameStatusInfo;//斷線從連信息
    public bool isMouseDown;
    public void ReadyGame()
    {
        Init();

        SocketSendManager.Instance.SendPlayerReady();      
    }
    public void Init()
    {
        playersCard = new CMD_S_ShowCard();
        choiceType = SUB_S_COMMONCARD.HT_INVALID;
        isRefreshShouPai = false;
        isPutOn = false;
        choseCount = 0;
        if (choiceCard == null)
            choiceCard = new List<int>();
        if (lastCard == null)
            lastCard = new List<int>();
        if (tdCardId == null)
            tdCardId = new List<int>();
        if (zdCardId == null)
            zdCardId = new List<int>();
        if (wdCardId == null)
            wdCardId = new List<int>();

        choiceCard.Clear();
        lastCard.Clear();
        tdCardId.Clear();
        zdCardId.Clear();
        wdCardId.Clear();
    }
}
