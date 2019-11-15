using System.Collections;
using UnityEngine;
using AssemblyCSharp;

public class HandAnima : MonoBehaviour
{
    public Vector3 endRotation;
    public Animator animator;
    public Vector3 startPos;
    public Vector3 endPos;
    private int index;
    private GameObject parent;
    private Vector3 pos;
    public int dir;

    void Start()
    {
        transform.position = endPos;
    }

    public IEnumerator InitHandPos()
    {
        iTween.MoveTo(gameObject,
            iTween.Hash("position", endPos, "easeType", iTween.EaseType.easeInOutQuart, "loopType", "none", "time",
                0.5f));
        yield return new WaitForSeconds(0.5f);
        gameObject.transform.eulerAngles = endRotation;
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

    public IEnumerator SetStart()
    {
        iTween.MoveTo(gameObject,
            iTween.Hash("position", startPos, "easeType", iTween.EaseType.easeInOutQuart, "loopType", "none", "time",
                0.8f));
        yield return new WaitForEndOfFrame();
        animator.speed = 1.5f;
        animator.Play("Start");
    }

    public void SetHu()
    {
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.position = startPos;
        animator.Play("Hu");
    }

    public void TouZi()
    {
        MyDebug.Log(gameObject.name + "==tttt=" + startPos);
        MaJiangGameCtl.instance.TouZi();
    }

    public void SetChaPai()
    {
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        transform.position = startPos;
        animator.Play("ChaPai");
    }

    public IEnumerator SetPut(int i, GameObject g, Vector3 v)
    {
        index = i;
        parent = g;
        pos = v;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        iTween.MoveTo(gameObject,
            iTween.Hash("position", startPos, "easeType", iTween.EaseType.linear, "loopType", "none", "time", 0.65f));
        yield return new WaitForSeconds(0.3f);
        animator.Play("Put");
    }
    
    public IEnumerator SetPut2(int i, GameObject g, Vector3 v)
    {
        index = i;
        parent = g;
        pos = v;
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        iTween.MoveTo(gameObject,
            iTween.Hash("position", startPos, "easeType", iTween.EaseType.linear, "loopType", "none", "time", 0.65f));
        yield return new WaitForSeconds(0.3f);
        animator.Play("Put2");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IEnumerator Enumerator()
    {
        GameObject obj = GameResourceManager.Instance.CreateGameObjectAndReturn(index, gameObject.transform, new Vector3(0f, 10.18f, -18.5f), Vector3.zero);
        Vector3 vector = parent.transform.position + new Vector3(pos.x * 1.2f, pos.y * 1.2f, pos.z * 1.2f);
        iTween.MoveTo(obj.gameObject,
            iTween.Hash("position", vector, "easeType", iTween.EaseType.linear, "loopType", "none", "time", 0.34f));
        yield return new WaitForSeconds(0.34f);
        obj.transform.SetParent(parent.transform);
        obj.transform.localScale = Vector3.one;
        GameObject go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
        go.transform.parent = obj.transform;
        MaJiangGameCtl.instance.cardOnTable = obj;
        switch (parent.name)
        {
            case "ThrowCardsgobBottom":
                go.transform.localPosition = new Vector3(0, -0.327f, -0.189f);
                go.transform.localScale = new Vector3(0.083f, 0.196f, 0.149f);
                MaJiangGameCtl.instance.tableCardList[0].Add(obj);
                break;
            case "ThrowCardsListLeft":
                go.transform.localPosition = new Vector3(0.171f, -0.327f, 0.001f);
                go.transform.localEulerAngles = new Vector3(90, 90, -90);
                go.transform.localScale = new Vector3(0.069f, 0.231f, 0.149f);
                MaJiangGameCtl.instance.tableCardList[3].Add(obj);
                break;
            case "ThrowCardsListTop":
                go.transform.localPosition = new Vector3(0, -0.327f, 0.189f);
                go.transform.localScale = new Vector3(0.083f, 0.196f, 0.149f);
                MaJiangGameCtl.instance.tableCardList[2].Add(obj);
                break;
            case "ThrowCardsListRight":
                go.transform.localPosition = new Vector3(-0.203f, -0.327f, -0.001f);
                go.transform.localEulerAngles = new Vector3(90, 90, 90);
                go.transform.localScale = new Vector3(0.069f, 0.231f, 0.149f);
                MaJiangGameCtl.instance.tableCardList[1].Add(obj);
                break;
        }

        obj.GetComponent<BottomScript>().SetDeskPointInfo(index);
        MaJiangGameCtl.instance.SetPointGameObject(obj);
    }

    /// <summary>
    /// 生成弃牌
    /// </summary>
    /// <returns></returns>
    public IEnumerator ShengCheng()
    {
        GameObject obj = GameResourceManager.Instance.CreateGameObjectAndReturn(index, gameObject.transform, new Vector3(0f, 10.18f, -18.5f), Vector3.zero);
        Vector3 vector = parent.transform.position + new Vector3(pos.x * 1.2f, pos.y * 1.2f, pos.z * 1.2f);
        iTween.MoveTo(obj.gameObject,
            iTween.Hash("position", vector, "easeType", iTween.EaseType.linear, "loopType", "none", "time", 0.34f));
        yield return new WaitForSeconds(0.34f);
        obj.transform.SetParent(parent.transform);
        obj.transform.localScale = Vector3.one;
        GameObject go = Instantiate(GameResourceManager.Instance.yinying) as GameObject;
        go.transform.parent = obj.transform;
        MaJiangGameCtl.instance.cardOnTable = obj;
        switch (parent.name)
        {
            case "ThrowCardsgobBottom":
                go.transform.localPosition = new Vector3(0, -0.327f, -0.189f);
                go.transform.localScale = new Vector3(0.083f, 0.196f, 0.149f);
                MaJiangGameCtl.instance.tableCardList[0].Add(obj);
                break;
            case "ThrowCardsListLeft":
                go.transform.localPosition = new Vector3(0.171f, -0.327f, 0.001f);
                go.transform.localEulerAngles = new Vector3(90, 90, -90);
                go.transform.localScale = new Vector3(0.069f, 0.231f, 0.149f);
                MaJiangGameCtl.instance.tableCardList[3].Add(obj);
                break;
            case "ThrowCardsListTop":
                go.transform.localPosition = new Vector3(0, -0.327f, 0.189f);
                go.transform.localScale = new Vector3(0.083f, 0.196f, 0.149f);
                MaJiangGameCtl.instance.tableCardList[2].Add(obj);
                break;
            case "ThrowCardsListRight":
                go.transform.localPosition = new Vector3(-0.203f, -0.327f, -0.001f);
                go.transform.localEulerAngles = new Vector3(90, 90, 90);
                go.transform.localScale = new Vector3(0.069f, 0.231f, 0.149f);
                MaJiangGameCtl.instance.tableCardList[1].Add(obj);
                break;
        }

        obj.GetComponent<BottomScript>().SetDeskPointInfo(index);
        MaJiangGameCtl.instance.SetPointGameObject(obj);
    }

    public IEnumerator SetMove()
    {
        gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        iTween.MoveTo(gameObject,
            iTween.Hash("position", startPos, "easeType", iTween.EaseType.easeInOutQuart, "loopType", "none", "time",
                0.7f));
        yield return new WaitForSeconds(0.2f);
        animator.Play("MoveRight");
    }

    public void XuanZhuan()
    {
        MaJiangGameCtl.instance.RotationPai(dir);
    }
}