using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TalkPanelBtn : MonoBehaviour
{
    public List<TalkItemData> speakList;
    [SerializeField] ScrollRectList myScrollRect;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] GameObject rankItem;

    void Start()
    {
        InitScroll();
    }

    void InitScroll()
    {
        speakList = TalkDataManager.Instance.list;
        scrollRect.onValueChanged.AddListener(OnValueChange);
        myScrollRect.createitemobject = delegate(int index, UnityAction<GameObject> action)
        {
            if (rankItem != null)
            {
                GameObject cellObj = Instantiate(rankItem.gameObject);
                action(cellObj);
            }
        };
        myScrollRect.updateItem = delegate(ItemCell o, int index)
        {
            o.item.transform.Find("Text").GetComponent<Text>().text = speakList[index].message;
            o.item.GetComponent<TalkButton>().id = speakList[index].id;
        };
        myScrollRect.Init(speakList.Count);
    }

    void OnValueChange(Vector2 pos)
    {
        myScrollRect.OnValueChange(pos);
    }
}