using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipPanelScript : MonoBehaviour
{
    public Text label;

    public void SetText(string str)
    {
        label.text = str;
    }

    public void StartAction()
    {
        Invoke("TipsMoveCompleted", 2f);
    }

    private void Move()
    {
        gameObject.transform.DOLocalMove(new Vector3(0, -100), 0.7f).OnComplete(TipsMoveCompleted);
    }

    public void TipsMoveCompleted()
    {
        Destroy(gameObject);
    }
}