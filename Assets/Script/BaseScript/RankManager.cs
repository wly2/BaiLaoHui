using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankManager
{
    private static RankManager _instance;

    public static RankManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new RankManager();
            }

            return _instance;
        }
    }

    private List<RankItem> _list = new List<RankItem>();
    private readonly Dictionary<int, RankItem> _win = new Dictionary<int, RankItem>();
    private readonly Dictionary<int, RankItem> _roomCard = new Dictionary<int, RankItem>();

    public Dictionary<int, RankItem> Win
    {
        get
        {
            if (_win.Count == 0)
                InitTalkData();
            return _win;
        }
    }

    public Dictionary<int, RankItem> Roomcard
    {
        get
        {
            if (_roomCard.Count == 0)
                InitTalkData();
            return _roomCard;
        }
    }

    public void InitTalkData()
    {
        for (int i = 0; i < 100; i++)
        {
            RankItem rankItem = new RankItem(Random.Range(0, 9999).ToString(), Random.Range(0, 9999),
                Random.Range(0, 50));
            _list.Add(rankItem);
        }

        _list.Sort(delegate(RankItem x, RankItem y) { return y.winCount.CompareTo(x.winCount); });
        for (int i = 0; i < _list.Count; i++)
        {
            _win.Add(i + 1, _list[i]);
        }

        _list.Sort(delegate(RankItem x, RankItem y) { return y.roomCardCount.CompareTo(x.roomCardCount); });
        for (int i = 0; i < _list.Count; i++)
        {
            _roomCard.Add(i + 1, _list[i]);
        }
    }
}