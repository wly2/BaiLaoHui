using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIpanelEMail : UIWindow
{
    public Text messageText;
    public List<EmailItem> btnlist;
    int currentId = 0;
    public GameObject deleteBtn;
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        currentId = 0;
        for (int i = 0; i < btnlist.Count; i++)
        {
            btnlist[i].gameObject.SetActive(false);
        }
        if (MainNewsDataManager.instance.newsList == null || MainNewsDataManager.instance.newsList.Count <= 0)
        {
            messageText.enabled = false;
            deleteBtn.SetActive(false);
            return;
        }
      
        for (int i = 0; i < MainNewsDataManager.instance.newsList.Count; i++)
        {
            if (i <= 4)    
                btnlist[i].Init(MainNewsDataManager.instance.newsList[i]);
        }
        SetMessage();

    }
    public void SetValue(int i)
    {
        currentId = i;
        Invoke("SetMessage", 0.1f);
    }
    public void SetMessage()
    {
        messageText.text = MainNewsDataManager.instance.newsList[currentId].content;
        MainNewsDataManager.instance.newsList[currentId].is_read = "1";
        btnlist[currentId].SetRealRead();
        btnlist[currentId].GetComponent<Toggle>().isOn = true;
        HttpManager.instance.SetSystemsNewsStatus(currentId);
    }
    public void SetDeleteMail()
    {
        HttpManager.instance.SetSystemsNewsStatus(currentId,"del");
        MainNewsDataManager.instance.newsList.RemoveAt(currentId);
        Init();
       
    }

}
