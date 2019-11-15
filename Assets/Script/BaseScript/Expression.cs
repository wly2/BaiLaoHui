using System;
using UnityEngine;
using UnityEngine.UI;

public class Expression : MonoBehaviour
{
    public GameObject game;
    public int id;
    public void SendExpression()
    {
        string mes = "0|" +id;
        SocketSendManager.Instance.ChewTheRag(mes);
        game.SetActive(false);
    }
}