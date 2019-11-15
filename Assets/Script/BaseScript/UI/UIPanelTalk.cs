using AssemblyCSharp;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPanelTalk : UIWindow
{
    public List<TalkItemData> speakList;
    [SerializeField] ScrollRectList myScrollRect;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject talkItem;
    public GameObject expression;
    public GameObject obj_Message;
    public InputField inputField;

    public void SendButttonClick()
    {
        if (inputField.text != "")
        {
            string mes = "2|" + inputField.text;
            SocketSendManager.Instance.ChewTheRag(mes);
        }
    }

    public void ExpressionButtonClick()
    {
        expression.SetActive(true);
        obj_Message.SetActive(false);
    }

    void Start()
    {
        ShowNormalTalk();
    }

    void InitScroll()
    {
        scrollRect.onValueChanged.AddListener(OnValueChange);
        myScrollRect.createitemobject = delegate(int index, UnityAction<GameObject> action)
        {
            if (talkItem != null)
            {
                GameObject cellObj = Instantiate(talkItem.gameObject);
                action(cellObj);
            }
        };
        myScrollRect.updateItem = delegate(ItemCell o, int index)
        {
            o.item.GetComponent<TalkItem>().Init(speakList[index], CloseUI);
        };
        myScrollRect.Init(speakList.Count);
    }

    void InitRecordScroll()
    {
        scrollRect.onValueChanged.AddListener(OnValueChange);
        myScrollRect.createitemobject = delegate(int index, UnityAction<GameObject> action)
        {
            if (talkItem != null)
            {
                GameObject cellObj = Instantiate(talkItem.gameObject);
                action(cellObj);
            }
        };
        MyDebug.LogError(myScrollRect.createitemobject);
        myScrollRect.updateItem = delegate (ItemCell o, int index)
        {
            o.item.GetComponent<TalkItem>().InitLocal(speakList[index]);
        };
        myScrollRect.Init(speakList.Count);
    }

    void OnValueChange(Vector2 pos)
    {
        myScrollRect.OnValueChange(pos);
    }

    /// <summary>
    /// 聊天窗口
    /// </summary>
    public void ShowNormalTalk()
    {
        scrollRect.onValueChanged.RemoveAllListeners();
        speakList = TalkDataManager.Instance.list;
        InitScroll();
        expression.SetActive(false);
        obj_Message.SetActive(true);
    }

    /// <summary>
    /// 聊天记录窗口
    /// </summary>
    public void ShowRecordTalk()
    {
        scrollRect.onValueChanged.RemoveAllListeners();
        speakList = TalkDataManager.Instance.RecordList;
        InitRecordScroll();
    }
    public void SendExpression(int id)
    {
        string mes = "0|" + id;
        SocketSendManager.Instance.ChewTheRag(mes);
        CloseUI();
    }
}