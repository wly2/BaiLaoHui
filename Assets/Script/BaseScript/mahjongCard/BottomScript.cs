using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using UnityEngine.UI;
using DG.Tweening;

public class BottomScript : MonoBehaviour
{
    public int cardPoint;
    public Vector3 oldPosition;
    public bool isSelected ;

    public delegate void EventHandler(GameObject obj);

    public event EventHandler OnSendMessage;
    public event EventHandler ReSetPoisiton;

    public bool canSelected;
    private Image puke;
    // public bool isPickCard ;
    public bool isFlyCard;
    private bool dragFlag;
    private GameObject cell;
    public Material darkMater;
    
    public Material lightMater;

    void Start()
    {
        isSelected = true;
        puke = transform.GetComponent<Image>();
        //cell = transform.GetChild(0).gameObject;
        // darkMater = GlobalDataScript.instance.gameObject;
    }

    public void SetPosition(Vector3 pos)
    {
        oldPosition = Vector3.zero;
        isSelected = false;
        StopCoroutine("MoveToAction");
        StartCoroutine(MoveToAction(pos));
    }

    IEnumerator MoveToAction(Vector3 pos)
    {
        yield return new WaitForEndOfFrame();
        if (!isFlyCard)
        {
            yield return new WaitForSeconds(0.4f);
            iTween.MoveTo(gameObject,
                iTween.Hash("position", pos, "easeType", iTween.EaseType.easeInOutQuart, "loopType", "none", "time",
                    0.2f));
        }
        else
        {
            isFlyCard = false;
            Vector3 pos1 = transform.position + new Vector3(0, 1.4f, 0);
            iTween.RotateBy(gameObject,
                iTween.Hash("y", 0.1f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.3f));
            iTween.MoveTo(gameObject,
                iTween.Hash("position", pos1, "easeType", iTween.EaseType.easeInOutQuart, "loopType", "none", "time",
                    0.3f));
            yield return new WaitForSeconds(0.3f);
            Vector3 pos2 = pos + new Vector3(0, 1.4f, 0);
            iTween.MoveTo(gameObject,
                iTween.Hash("position", pos2, "easeType", iTween.EaseType.easeInOutQuart, "loopType", "none", "time",
                    0.3f));
            iTween.RotateBy(gameObject,
                iTween.Hash("y", -0.1f, "easeType", "easeInOutSine", "loopType", "none", "time", 0.3f));
            yield return new WaitForSeconds(0.3f);
            iTween.MoveTo(gameObject,
                iTween.Hash("position", pos, "easeType", iTween.EaseType.easeInOutQuart, "loopType", "none", "time",
                    0.3f));
        }
    }

    IEnumerator OnMouseDown()
    {
        MyDebug.Log(GlobalDataScript.isDrag);
        if (GlobalDataScript.isDrag && canSelected && GlobalDataScript.isGameReadly)
        {
            if (oldPosition == Vector3.zero)
                oldPosition = transform.localPosition;
            //将物体由世界坐标系转化为屏幕坐标系 ，由vector3 结构体变量ScreenSpace存储，以用来明确屏幕坐标系Z轴的位置
            Vector3 ScreenSpace = Camera.main.WorldToScreenPoint(transform.position);
            //完成了两个步骤，1由于鼠标的坐标系是2维的，需要转化成3维的世界坐标系，2只有三维的情况下才能来计算鼠标位置与物体的距离，offset即是距离
            Vector3 offset = transform.position -
                             Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                                 ScreenSpace.z));

            MyDebug.Log("down");
            //当鼠标左键按下时
            while (Input.GetMouseButton(0))
            {
                //得到现在鼠标的2维坐标系位置
                Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, ScreenSpace.z);
                //将当前鼠标的2维位置转化成三维的位置，再加上鼠标的移动量
                Vector3 CurPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;
                //CurPosition就是物体应该的移动向量赋给transform的position属性
                transform.position = CurPosition;
                if (transform.localPosition.y >= 2)
                    dragFlag = true;
                //这个很主要
                yield return new WaitForFixedUpdate();
            }
        }
    }

    void OnMouseUp()
    {
        if (!GlobalDataScript.isGameReadly)
            return;
        if (transform.localPosition.y > 2)
        {
            SendObjectToCallBack();
            return;
        }

        if (dragFlag)
        {
            transform.localPosition = oldPosition;
            isSelected = false;
            dragFlag = false;
            return;
        }

        if (transform.localPosition.y > 0 && isSelected)
        {
            SendObjectToCallBack();
        }
        else
        {
            ReSetPoisitonCallBack();
        }

        MyDebug.Log("Out of collider");
    }

    private void SendObjectToCallBack()
    {
        if (OnSendMessage != null) //发送消息
        {
            OnSendMessage(gameObject); //发送当前游戏物体消息
        }
    }

    private void ReSetPoisitonCallBack()
    {
        if (ReSetPoisiton != null)
        {
            ReSetPoisiton(gameObject);
            SetSlect();
        }
    }

    /// <summary>
    /// 还原手牌位置
    /// </summary>
    public void ResetShouPaiPosition()
    {
        if (ReSetPoisiton != null)
        {
            ReSetPoisiton(gameObject);
        }
    }

    public void SetDeskPointInfo(int _point)
    {
        SetListener();
        cardPoint = _point;
    }

    public void InitMyPoint(int _point)
    {
        cardPoint = _point;
        canSelected = true;
    }

    public int GetPoint()
    {
        return cardPoint;
    }

    public void SetOldPosition()
    {
        isSelected = false;
        transform.localPosition -= new Vector3(0, 0.5f, 0);
    }

    public void SetSlect()
    {
        if (GameMessageManager.ClosePointPrompt != null)
            GameMessageManager.ClosePointPrompt();
        if (GameMessageManager.ShowPointPrompt != null)
            GameMessageManager.ShowPointPrompt(cardPoint);
        isSelected = true;
        if (GlobalDataScript.isDrag)
            transform.localPosition = oldPosition;
        transform.localPosition += new Vector3(0, 0.5f, 0);
    }

    public void SetListener()
    {
        GameMessageManager.ShowPointPrompt += ShowPrompt;
        GameMessageManager.ClosePointPrompt += ClosePrompt;
        GameMessageManager.ClosePutOutPrompt += ClosePutOut;
    }

    private void ShowPrompt(int point)
    {
        //if (point == cardPoint)
            //cell.GetComponent<Renderer>().material = darkMater;
    }

    private void ClosePrompt()
    {
        //cell.GetComponent<Renderer>().material = lightMater;
    }

    private void ClosePutOut(int point)
    {
        //if (point == cardPoint)
            //cell.GetComponent<Renderer>().material = lightMater;
    }

    private void OnDisable()
    {
        //if (GameMessageManager.ClosePutOutPrompt != null)
        //    GameMessageManager.ClosePutOutPrompt(cardPoint);
        GameMessageManager.ShowPointPrompt -= ShowPrompt;
        GameMessageManager.ClosePointPrompt -= ClosePrompt;
        GameMessageManager.ClosePutOutPrompt -= ClosePutOut;
    }
    void Updata()
    {
        MyDebug.Log(puke.rectTransform.position);
        MyDebug.Log("ss");
    }

   
}