using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EmailItem : MonoBehaviour {

    public NewsItem mNews;
    public Text title;
    public GameObject tips;
    // Use this for initialization
    public void Init(NewsItem _item)
    {
        gameObject.SetActive(true);
        mNews = _item;
        title.text = mNews.title;
        tips.SetActive(mNews.is_read.Equals("0"));
    }
    public void SetRealRead()
    {
        tips.SetActive(false);
    }
}
