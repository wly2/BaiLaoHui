using UnityEngine;
using DG.Tweening;
using AssemblyCSharp;

public class MessageBoxScript : MonoBehaviour
{
    UIMaJiangPanel myMaj;

    public void BtnClick(int index)
    {
        //SoundManager.Instance.PlayMessageBoxSound(index);
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

    public void ShowPanel()
    {
        gameObject.transform.DOLocalMove(new Vector3(472, 260), 0.4f);
    }

    public void HidePanel()
    {
        gameObject.transform.DOLocalMove(new Vector3(472, 740), 0.4f);
    }

    public void MessageBoxReply(ClientResponse response)
    {
        string[] arr = response.message.Split(new char[1] {'|'});
        int code = int.Parse(arr[0]);
       // SoundManager.Instance.PlayMessageBoxSound(code);
    }

    public void Destroy()
    {
        SocketEventHandle.Instance.messageBoxReply -= MessageBoxReply;
    }
}