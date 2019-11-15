using AssemblyCSharp;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanel_Report : UIWindow
{                                           
    public List<QueryInfo> speakList;
    [SerializeField] ScrollRectList myScrollRect;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject recordItem;     
    public GameObject recordPanel;
    public GameObject mainRecord;
    [SerializeField] ScrollRectList roundScrollRect;
    [SerializeField] ScrollRect roundscrollRect;
    [SerializeField] GameObject roundrecordItem;
    public List<CMD_MB_ROUND_INFO> roundList;
    void Start()
    {
        speakList = MainNewsDataManager.instance.myQureyList; 
        recordPanel.SetActive(false);
        if (speakList == null || speakList.Count <= 0)
            return;
        InitScroll();
    }
    //增加服务器返沪数据监听
    public void OnEnable()
    {
        SocketEventHandle.Instance.recordDetailReply += ShowRoundPanel;       
    }

    public void OnDisable()
    {
        SocketEventHandle.Instance.recordDetailReply -= ShowRoundPanel;      
    }


    void OnValueChange(Vector2 pos)
    {
        myScrollRect.OnValueChange(pos);
    }
    
    /// <summary>
    /// 生成战绩下拉列表
    /// </summary>
    void InitScroll()
    {                 
        scrollRect.onValueChanged.AddListener(OnValueChange);
        myScrollRect.createitemobject = delegate(int index, UnityAction<GameObject> action)
        {
            if (recordItem != null)
            {
                GameObject cellObj = Instantiate(recordItem.gameObject);
                action(cellObj);
            }
        };
        myScrollRect.updateItem = delegate(ItemCell o, int index)
        {
            o.item.GetComponent<RecordButton>().SetValue(speakList[index]);
        };          
        myScrollRect.Init(speakList.Count);     
    }
    public void ShowRoundPanel(ClientResponse resp)
    {
        mainRecord.SetActive(false);
        recordPanel.SetActive(true);
        roundList = MainNewsDataManager.instance.myRoundQureylist;
        if (roundList == null || roundList.Count <= 0)
            return;
        InitRoundScroll();
       
    }
    /// <summary>
    /// 生成战绩下拉列表
    /// </summary>
    void InitRoundScroll()
    {
        roundscrollRect.onValueChanged.AddListener(roundScrollRect.OnValueChange);
        roundScrollRect.createitemobject = delegate (int index, UnityAction<GameObject> action)
        {
            if (roundrecordItem != null)
            {
                GameObject cellObj = Instantiate(roundrecordItem.gameObject);
                action(cellObj);
            }
        };
        roundScrollRect.updateItem = delegate (ItemCell o, int index)
        {
            o.item.GetComponent<RecordButton>().SetroundScore(roundList[index],index);
        };
        roundScrollRect.Init(roundList.Count);
    }    

    public void CloseRoundPanel()
    {
        mainRecord.SetActive(true);
        recordPanel.SetActive(false);
        InitScroll();
    }
}