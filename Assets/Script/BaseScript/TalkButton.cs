using UnityEngine;
using DG.Tweening;
using AssemblyCSharp;

public class TalkButton : MonoBehaviour
{
    UIMaJiangPanel myMaj;
    public int id;

    public void BtnClick(int index)
    {
        index = this.id;
       // SoundManager.Instance.PlayMessageBoxSound(index);
        CustomSocket.Instance.SendMsg(new MessageBoxRequest(index, GlobalDataScript.loginResponseData.account.uuid));
        if (myMaj == null)
        {
            myMaj = GameObject.Find("Panel_GamePlay(Clone)").GetComponent<UIMaJiangPanel>();
        }

        if (myMaj != null)
        {
            myMaj.playerItems[0].ShowChatMessage(index);
        }

        HidePanel();
    }

    public void HidePanel()
    {
        gameObject.transform.DOLocalMove(new Vector3(472, 740), 0.4f);
    }
}