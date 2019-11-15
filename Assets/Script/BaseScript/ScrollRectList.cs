using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using AssemblyCSharp;

public class ItemCell : object
{
    public GameObject item;
    public int _index;
}

public class ScrollRectList : MonoBehaviour
{
    public enum Arrangement
    {
        Horizontal,
        Vertical,
    }

    public Arrangement _movement = Arrangement.Horizontal;

    //单行或单列的Item数量
    [Range(1, 20)] public int maxPerLine = 2;

    //Item之间的距离
    [Range(0, 20)] public int cellPadiding = 5;

    //Item的宽高
    public float cellWidth = 100;
    public float cellHeight = 200;

    public Vector3 scale = Vector3.one;

    //默认加载的行数，一般比可显示行数大2~3行
    [Range(0, 20)] public int viewCount = 3;

    //public GameObject itemPrefab;
    public RectTransform _content;

    //记录当前滑动行列序号
    private int _index = -1;
    private List<ItemCell> _itemList;

    private int _dataCount;

    //public int getItemIndex(ItemCell o);
    //public GetItemIndex getItemIndex;
    public delegate void UpdateItem(ItemCell o, int index);

    public UpdateItem updateItem;

    public delegate void CreateItemobject(int index, UnityAction<GameObject> action);

    public CreateItemobject createitemobject;

    //public  void setItemIndex(ItemCell o,int index);
    //public SetItemIndex setItemIndex;
    public delegate void DeleteItemobject(ItemCell o);

    public DeleteItemobject deleteitemobject;

    public delegate void InitAfter();

    public InitAfter initafter;

    //道具是否是同一类型
    public delegate bool ISsynchysis(int oldIndex, int nowIndex);

    public ISsynchysis issynchysis;

    //缓存池
    public List<ItemCell> PoolItems = new List<ItemCell>();

    //public GameObject panelist;
    void Start()
    {
        _content.anchorMin = new Vector2(0, 1);
        _content.anchorMax = new Vector2(0, 1);
        _content.pivot = new Vector2(0, 1);
    }

    public int DataCount
    {
        get { return _dataCount; }
        set
        {
            _dataCount = value;
            UpdateTotalWidth();
        }
    }

    void SetItemIndex(ItemCell itemcell, int value)
    {
        itemcell._index = value;
        itemcell.item.transform.localPosition = GetPosition(value);
        //print ("4444=="+itemcell.item.transform.position);
    }

    public int GetItemIndex(ItemCell itemcell)
    {
        return itemcell._index;
    }

    public void Init(int DataCount)
    {
        //this._content = panelist.GetComponent<RectTransform>();
        DestroyAllItems();
        _index = -1;
        _itemList = new List<ItemCell>();
        this.DataCount = DataCount;
        OnValueChange(Vector2.zero);
        if (initafter != null)
            initafter();
    }

    List<ItemCell> hideitems = new List<ItemCell>();

    public void OnValueChange(Vector2 pos)
    {
        if (_itemList == null)
            return;
        int index = GetPosIndex();
        // if(issynchysis!=null)
        // Debug.Log("index:"+index+",list:"+_itemList.Count+"\n");
        if (_index != index && index > -1)
        {
            _index = index;
            //计算不显示对象
            for (int i = 0; i < _itemList.Count; i++)
            {
                ItemCell item = _itemList[i];
                int itemIndex = GetItemIndex(item);
                if (itemIndex < index * maxPerLine || (itemIndex >= (index + viewCount) * maxPerLine))
                    hideitems.Add(item);
            }

            for (int i = 0; i < hideitems.Count; ++i)
            {
                _itemList.Remove(hideitems[i]);
            }

            //Debug.Log("index:" + index + ",hideitems:" + hideitems.Count);
            //计算显示对象
            for (int i = _index * maxPerLine; i < (_index + viewCount) * maxPerLine; i++)
            {
                if (i < 0)
                    continue;
                if (i > _dataCount - 1)
                    continue;
                if (_itemList.Find(delegate(ItemCell item) { return GetItemIndex(item) == i; }) == null)
                {
                    if (hideitems.Count > 0)
                    {
                        //Debug.Log("get old imte :" + i);
                        bool issys = false;
                        if (issynchysis != null)
                        {
                            issys = issynchysis(hideitems[0]._index, i);
                        }

                        if (!issys)
                        {
                            ItemCell o = hideitems[0];
                            hideitems.Remove(o);
                            _itemList.Add(o);
                            SetItemIndex(o, i);
                            updateItem(o, i);
                        }
                        else
                        {
                            Destroy(hideitems[0].item);
                            hideitems.RemoveAt(0);
                            CreateItem(i);
                        }
                    }
                    else if (PoolItems.Count > 0)
                    {
                        //Debug.Log("get old pool :" + i);
                        ItemCell o = PoolItems[0];
                        o.item.SetActive(true);
                        PoolItems.RemoveAt(0);
                        _itemList.Add(o);
                        SetItemIndex(o, i);
                        updateItem(o, i);
                    }
                    else
                    {
                        //print("create i:" + i + "\n");
                        CreateItem(i);
                    }
                }
            }
        }
        else
        {
            //_content.position = Vector3.zero;
        }

        for (int i = 0; i < PoolItems.Count; ++i)
        {
            Destroy(PoolItems[i].item);
        }

        PoolItems.Clear();
    }

    /// <summary>
    /// 提供给外部的方法，添加指定位置的Item
    /// </summary>
    public void AddItem(int index)
    {
        if (index > _dataCount || index < 0)
        {
            Debug.LogError("添加错误:" + index);
            return;
        }

        AddItemIntoPanel(index);
        DataCount += 1;
    }

    /// <summary>
    /// 提供给外部的方法，删除指定位置的Item
    /// </summary>
    public void DelItem(int index)
    {
        if (index < 0 || index > _dataCount - 1)
        {
            MyDebug.LogError("删除错误:" + index);
            return;
        }

        DataCount -= 1;
        DelItemFromPanel(index);
    }

    private void AddItemIntoPanel(int index)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            ItemCell item = _itemList[i];
            int itemIndex = GetItemIndex(item);
            if (itemIndex >= index)
                SetItemIndex(item, itemIndex + 1);
        }

        CreateItem(index);
    }

    private void DelItemFromPanel(int index)
    {
        int maxIndex = -1;
        int minIndex = int.MaxValue;
        List<ItemCell> delitems = new List<ItemCell>();
        for (int i = 0; i < _itemList.Count; i++)
        {
            ItemCell item = _itemList[i];
            int itemIndex = GetItemIndex(item);
            if (itemIndex == index)
            {
                //GameObject.Destroy(item.gameObject);
                //GameObject.Destroy(item.gameObject);
                //deleteitemobject(item);
                //_itemList.Remove(item);
                delitems.Add(item);
            }

            if (itemIndex > maxIndex)
            {
                maxIndex = itemIndex;
            }

            if (itemIndex < minIndex)
            {
                minIndex = itemIndex;
            }

            if (itemIndex > index)
            {
                SetItemIndex(item, itemIndex - 1);
            }
        }

        for (int i = 0; i < delitems.Count; ++i)
        {
            _itemList.Remove(delitems[i]);
            Destroy(delitems[i].item); //GameObject.Destroy(item.gameObject);
        }

        if (maxIndex < DataCount - 1)
        {
            CreateItem(maxIndex);
        }
    }

    private void CreateItem(int index)
    {
        createitemobject(index, (obj) =>
        {
            if (obj != null)
            {
                obj.transform.SetParent(_content.transform);
                obj.GetComponent<RectTransform>().pivot = new Vector2(0f, 1f);
                obj.GetComponent<RectTransform>().sizeDelta = new Vector2(cellWidth, cellHeight);
                obj.transform.localScale = scale;
                ItemCell itemcell = new ItemCell
                {
                    _index = index,
                    item = obj
                };
                SetItemIndex(itemcell, index);
                updateItem(itemcell, index);
                _itemList.Add(itemcell);
            }
        });
    }

    public void UpdateItemCells()
    {
        if (_itemList != null)
        {
            for (int i = 0; i < _itemList.Count; i++)
            {
                ItemCell item = _itemList[i];
                updateItem(item, item._index);
            }
        }
    }

    private int GetPosIndex()
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                if (_content.anchoredPosition.x > 0)
                    return 0;
                return Mathf.FloorToInt(_content.anchoredPosition.x / -(cellWidth * scale.x + cellPadiding));
            case Arrangement.Vertical:
                if (_content.anchoredPosition.y < 0)
                    return 0;
                return Mathf.FloorToInt(_content.anchoredPosition.y / (cellHeight * scale.y + cellPadiding));
        }

        return 0;
    }

    public Vector3 GetPosition(int i)
    {
        switch (_movement)
        {
            case Arrangement.Horizontal:
                return new Vector3(cellWidth * scale.x * (i / maxPerLine) + cellPadiding,
                    -(cellHeight * scale.y + cellPadiding) * (i % maxPerLine), 0f);
            case Arrangement.Vertical:
                //增加列表居中对齐计算
                float leftrightspace = 0;
                leftrightspace =
                    (_content.sizeDelta.x - cellWidth * scale.x * maxPerLine - cellPadiding * maxPerLine * 2) / 2;
                leftrightspace = leftrightspace < 0 ? (float)(ValueType)0 :
                    leftrightspace > cellPadiding ? leftrightspace - cellPadiding : 0;
              
                return new Vector3(
                    leftrightspace + cellWidth * scale.x * (i % maxPerLine) + (i % maxPerLine) * cellPadiding +
                    cellPadiding, -(cellHeight * scale.y + cellPadiding) * (i / maxPerLine) - cellPadiding, 0f);
        }

        return Vector3.zero;
    }

    private void UpdateTotalWidth()
    {
        int lineCount = Mathf.CeilToInt((float) _dataCount / maxPerLine);
        switch (_movement)
        {
            case Arrangement.Horizontal:
                _content.sizeDelta = new Vector2(cellWidth * scale.y * lineCount + cellPadiding * (lineCount - 1),
                    _content.sizeDelta.y);
                break;
            case Arrangement.Vertical:
                _content.sizeDelta = new Vector2(_content.sizeDelta.x,
                    cellHeight * scale.y * lineCount + cellPadiding * (lineCount + 2));
                break;
        }

        //_content.localPosition = Vector3.zero;
    }

    public void DestroyAllItems()
    {
        if (_itemList == null)
        {
            return;
        }

        if (issynchysis == null)
        {
            for (int i = 0; i < _itemList.Count; ++i)
            {
                if (_itemList[i].item != null)
                {
                    PoolItems.Add(_itemList[i]);
                    _itemList[i].item.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < _itemList.Count; ++i)
            {
                Destroy(_itemList[i].item);
            }
        }

        for (int i = 0; i < hideitems.Count; ++i)
        {
            if (hideitems[i].item != null)
            {
                Destroy(hideitems[i].item);
            }
        }

        hideitems.Clear();
        _itemList = null;
    }

    public void Destroy()
    {
        DestroyAllItems();
        for (int i = 0; i < PoolItems.Count; ++i)
        {
            Destroy(PoolItems[i].item);
        }

        PoolItems.Clear();
    }

    public ItemCell GetItemByIndex(int index)
    {
        for (int i = 0; i < _itemList.Count; i++)
        {
            ItemCell item = _itemList[i];
            int itemIndex = GetItemIndex(item);
            if (itemIndex == index)
                return item;
        }

        return null;
    }

    public Vector3 GetIndexPos(int index)
    {
        if (index > 0)
        {
            switch (_movement)
            {
                case Arrangement.Horizontal:
                    return new Vector3(-cellWidth * scale.x * (index / maxPerLine) + cellPadiding,
                        -(cellHeight * scale.y + cellPadiding) * (index % maxPerLine), 0f);
                case Arrangement.Vertical:
                    //增加列表居中对齐计算
                    float leftrightspace = 0;
                    leftrightspace = (_content.sizeDelta.x - cellWidth * scale.x * maxPerLine -
                                      cellPadiding * maxPerLine * 2) / 2;
                    leftrightspace = leftrightspace < 0 ? (float) (ValueType) 0 :
                        leftrightspace > cellPadiding ? leftrightspace - cellPadiding : 0;
                    return new Vector3(0f, (cellHeight * scale.y + cellPadiding) * (index / maxPerLine) - cellPadiding,
                        0f);
            }
        }

        return Vector3.zero;
    }

    public void PrintList()
    {
        for (int i = 0; i < _itemList.Count; ++i)
        {
            MyDebug.LogError("Item Name: " + _itemList[i].item.name + ", Index: " + _itemList[i]._index);
        }
    }
}