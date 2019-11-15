using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using AssemblyCSharp;

public class PukeCardItem : MonoBehaviour
{

    public int cardPoint;//牌的牌值
    private bool isSelected;//是否被选中
    public bool isGray;
    public Image puke;
    public Image mapai;
    public SHISANSHUISHOW type;
    // Use this for initialization
    void Start()
    {

        isSelected = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetValue(int point, SHISANSHUISHOW _type = SHISANSHUISHOW.NULLDAO)
    {
        if (point <= 0)
            return;
        type = _type;
        cardPoint = point;
        if (GlobalDataScript.Instance.roomInfo.maPaiId > 0)
        {
            if (cardPoint == GlobalDataScript.Instance.roomInfo.maPaiId)
            {
               mapai.gameObject.SetActive(true);
            }
            else
            {
                mapai.gameObject.SetActive(false);
            }
        }
        ResourcesLoader.Load<Sprite>("PuKe/card_" + point, (sprite) =>
          {
              puke.overrideSprite = sprite;
          });

    }
    public void SetSelect(bool isS)
    {
        if (isS)
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 60, 0); 
            isSelected = true;
        }
        else
        {
            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, 0);
            isSelected = false;
        }                                                                       
    }


    public void OnPukRelease()
    {
        switch (type)
        {
            case SHISANSHUISHOW.SHISANSHUITOUDAO:
                GameDataSSS.Instance.tdCardId.Remove(cardPoint);
                break;
            case SHISANSHUISHOW.SHISANSHUIZHONGDAO:
                GameDataSSS.Instance.zdCardId.Remove(cardPoint);
                break;
            case SHISANSHUISHOW.SHISANSHUIWEIDAO:
                GameDataSSS.Instance.wdCardId.Remove(cardPoint);
                break;
            default:
                break;
        }
        gameObject.SetActive(false);
        mapai.gameObject.SetActive(false);
        GameDataSSS.Instance.lastCard.Add(cardPoint);        
        GameDataSSS.Instance.isRefreshShouPai = true;
    }
    public void PointEnter()
    {
        if (!GameDataSSS.Instance.isMouseDown)
            return;
        GameDataSSS.Instance.choseCount++;
        isGray = true;
        gameObject.GetComponent<Image>().color = Color.gray;
    }
    public void PointClickDown()
    {
        GameDataSSS.Instance.choseCount++;
        isGray = true;
        gameObject.GetComponent<Image>().color = Color.gray;
    }
    public void setPos()
    {
        gameObject.GetComponent<Image>().color = Color.white;
        if (GameDataSSS.Instance.choseCount > 0)
        {
            if (isGray)
            {
                isGray = false;
                if (isSelected)
                {
                    if (GameDataSSS.Instance.choiceCard.Contains(cardPoint))
                    {
                        GameDataSSS.Instance.lastCard.Add(cardPoint);
                        GameDataSSS.Instance.choiceCard.Remove(cardPoint);                    
                    }
                    isSelected = false;
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, 0);
                }
                else
                {

                    isSelected = true;
                    GameDataSSS.Instance.choiceCard.Add(cardPoint);                     
                    if (GameDataSSS.Instance.lastCard.Contains(cardPoint))
                        GameDataSSS.Instance.lastCard.Remove(cardPoint);                  
                    gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 60, 0);
                }
                MyDebug.Log("LastCard:" + GameDataSSS.Instance.lastCard.Count);
                MyDebug.Log("ChoicedCard:" + GameDataSSS.Instance.choiceCard.Count);
            }
        }
        else
        {
            isGray = false; 
            if (GameDataSSS.Instance.isPutOn == true)
                return;
            if (isSelected)
            {
                if (GameDataSSS.Instance.choiceCard.Contains(cardPoint))
                {
                    GameDataSSS.Instance.lastCard.Add(cardPoint);
                    GameDataSSS.Instance.choiceCard.Remove(cardPoint);
                    MyDebug.Log("LastCard:" + GameDataSSS.Instance.lastCard.Count);
                    MyDebug.Log("ChoicedCard:" + GameDataSSS.Instance.choiceCard.Count);
                }
                isSelected = false;
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, 0);
            }
        }

    }

}
