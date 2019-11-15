using System.Collections.Generic;
using UnityEngine;

public class RecordManager
{
    private static RecordManager _instance;

    public static RecordManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RecordManager();
            }

            return _instance;
        }
    }

    private List<RecordItem> _list = new List<RecordItem>();

    public List<RecordItem> List
    {
        get
        {
            if (_list.Count == 0)
                InitTalkData();
            return _list;
        }
    }

    public void InitTalkData()
    {
        for (int i = 0; i < 25; i++)
        {
            RecordItem recordItem = new RecordItem(i + 1, Random.Range(100000, 999999),
                Random.Range(100, 999).ToString(), Random.Range(3, 9),
                Random.Range(0, 25) + ":" + Random.Range(00, 60));
            _list.Add(recordItem);
        }
    }
}