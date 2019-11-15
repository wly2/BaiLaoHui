using LitJson;
using System.Collections.Generic;
using UnityEngine;

public class TalkDataManager
{
    private static TalkDataManager _instance;

    public static TalkDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TalkDataManager();
            }

            return _instance;
        }
    }

    private List<TalkItemData> _list;

    public List<TalkItemData> list
    {
        get
        {
            if (_list == null)
                InitTalkData();
            return _list;
        }
    }

    public void InitTalkData()
    {
        string str = Resources.Load<TextAsset>("Data/TalkItem").text;
        _list = JsonMapper.ToObject<List<TalkItemData>>(str);
    }

    private List<TalkItemData> _olderList;

    public List<TalkItemData> RecordList
    {
        get
        {
            if (_olderList == null)
            {
                _olderList = new List<TalkItemData>();
            }

            return _olderList;
        }
    }

    public void AddTalkItem(TalkItemData item)
    {
        if (_olderList == null)
            _olderList = new List<TalkItemData>();
        _olderList.Add(item);
    }

    public void ClearTalkData()
    {
        if (_olderList == null)
            return;
        _olderList.Clear();
    }
}