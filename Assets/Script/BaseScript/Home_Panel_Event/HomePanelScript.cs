using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AssemblyCSharp;
using System;
using DG.Tweening;
using System.Text;
using LitJson;

public class HomePanelScript : MonoBehaviour
{
    public Image headIconImg; //头像路径
    public Text noticeText;
    public Text nickNameText; //昵称
    public Text YinBi; //银币
    public Text Lamuratura; //砖石
    public Text IpText;
    public Text contactInfoContent;
    public GameObject roomCardPanel;
    Texture2D texture2D; //下载的图片
    private string headIcon;
    [HideInInspector]
    public Sprite imgLoad;//保存下载的头像
    public Image gonggao;

    /// <summary>
    /// 这个字段是作为消息显示的列表 ，如果要想通过管理后台随时修改通知信息，
    /// 请接收服务器的数据，并重新赋值给这个字段就行了。
    /// </summary>
    private bool startFlag;
    private int showNum;
    private int noticeCount;

    IEnumerator Start()
    {
        HttpManager.instance.SentHttpRequre(HTTP_TYPE.Paomadeng, PaoMaDengText);
        HttpManager.instance.GetGameStartpic(SetStartpic);
        HttpManager.instance.GetGameGold(RefreshGold);
        InitUI();
        GlobalDataScript.isonLoginPage = false;
        CheckEnterInRoom();   
        yield return new WaitForSeconds(0.1f);
        SoundManager.Instance.PlayBGM("BackAudio1");
        SoundManager.Instance.SetMusicV(PlayerPrefs.GetFloat("MusicVolume", 1));
        GlobalDataScript.Instance.Init();
    }
    public void SetStartpic(WWW mes)
    {
        JsonData json = JsonMapper.ToObject<JsonData>(mes.text);
        StartCoroutine(LoadImgI(json["data"].ToString()));
    }

    public void PaoMaDengText(WWW mes)
    {
        PaoMaDengManager.paoMaDeng = JsonMapper.ToObject<PaoMaDeng>(mes.text);
        if (PaoMaDengManager.paoMaDeng != null)
        {
            StartCoroutine(PaoMaNotice());
        }
    }
    public void RefreshGold(WWW mes)
    {
        JsonData json = JsonMapper.ToObject<JsonData>(mes.text);
        if (json["code"].Equals("0"))
        {
            string jm = json["data"].ToJson();
            MyDebug.Log(jm);
            JsonData jd = JsonMapper.ToObject<JsonData>(jm);
            YinBi.text = jd["zuanshi"].ToString();
            Lamuratura.text = jd["fangka"].ToString();
            GlobalDataScript.Instance.kingNum = int.Parse(YinBi.text);
            GlobalDataScript.Instance.diamgNum = int.Parse(Lamuratura.text);
        }
      
    }

    IEnumerator PaoMaNotice()
    {
        noticeText.transform.parent.gameObject.SetActive(true);
        noticeText.text = PaoMaDengManager.paoMaDeng.data;
        var time = (noticeText.preferredWidth + 1146) / 129f;
        var tweener = noticeText.transform.DOLocalMoveX(-noticeText.preferredWidth - 1146, time).SetRelative();
        tweener.SetEase(Ease.Linear);
        yield return new WaitForSeconds(time);
        noticeText.transform.localPosition =
            new Vector3(noticeText.transform.localPosition.x + noticeText.preferredWidth + 1146,
                noticeText.transform.localPosition.y, noticeText.transform.localPosition.z);
        noticeText.transform.parent.gameObject.SetActive(false);
        yield return new WaitForSeconds(5);
        noticeCount++;
        StartCoroutine(PaoMaNotice());
    }

    void MoveCompleted()
    {
        showNum++;
        if (showNum == GlobalDataScript.noticeMegs.Count)
        {
            showNum = 0;
        }
    }

    //增加服务器返沪数据监听
    public void OnEnable()
    {
        SocketEventHandle.Instance.cardChangeReply += CardChangeReply;
        SocketEventHandle.Instance.contactInfoReply += ContactInfoReply;
        SocketEventHandle.Instance.broadcastNoticeReply += BroadcastNotice;
        SocketEventHandle.Instance.recordReply += RecordReply;
    }

    public void OnDisable()
    {
        SocketEventHandle.Instance.cardChangeReply -= CardChangeReply;
        SocketEventHandle.Instance.contactInfoReply -= ContactInfoReply;
        SocketEventHandle.Instance.broadcastNoticeReply -= BroadcastNotice;
        SocketEventHandle.Instance.recordReply -= RecordReply;
    }

    private void RecordReply(ClientResponse response)
    {
        UIManager.instance.Show(UIType.UIReport);
    }

    //房卡变化处理
    private void CardChangeReply(ClientResponse response)
    {
        HttpManager.instance.GetGameGold(RefreshGold);
    }

    private void ContactInfoReply(ClientResponse response)
    {
        contactInfoContent.text = response.message;
        roomCardPanel.SetActive(true);
    }

    private void BroadcastNotice(ClientResponse response)
    {
    }

    /*
     *初始化显示界面
	 */
    private void InitUI()
    {
        var nickName = GlobalDataScript.userData.szNickName;//昵称
        nickNameText.text = NetUtil.BytesToString(nickName);
        if (nickNameText.text.Length > 4)
        {
            nickNameText.text = nickNameText.text.Substring(0, 6).ToString() + "...";
        }
        //MyDebug.Log("......................................." + nickNameText.text);
        //YinBi.text = GlobalDataScript.userData.lRoomCard.ToString();
        //MyDebug.Log("......................................." + YinBi.text);

        //Lamuratura.text = GlobalDataScript.userData.dUserBeans.ToString();
        //MyDebug.Log("......................................." + Lamuratura.text);

        IpText.text = GlobalDataScript.userData.dwUserID.ToString();
        MyDebug.Log("......................................." + IpText.text);

        //GlobalDataScript.weChatInformation.id = GlobalDataScript.loginResponseData.account.uuid;
        //GlobalDataScript.loginResponseData.account.roomcard = roomCardcount;

        if (imgLoad != null)
        {
            headIconImg.sprite = imgLoad;
            headIconImg.preserveAspect = true;
        }
        else
            StartCoroutine(LoadImg());
    }

    /*
      * 判断进入房间
      */
    private void CheckEnterInRoom()
    {
        if (GlobalDataScript.roomVo != null && GlobalDataScript.roomVo.roomId != 0)
        {

        }
    }

    public IEnumerator LoadImg()
    {
        headIcon = LoginData.wxUserInfo.headimgurl;
        //开始下载图片
        if (headIcon != null && headIcon != "")
        {
            headIconImg.preserveAspect = false;
            var www = new WWW(headIcon);
            yield return www;
            //下载完成，保存图片到路径filePath
            try
            {
                GlobalDataScript.Instance.weChatInformation = new WeChatInformation();
                texture2D = www.texture;
                //将图片赋给场景上的Sprite
                GlobalDataScript.Instance.weChatInformation.headIcon = Sprite.Create(texture2D,
                    new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
                headIconImg.sprite = GlobalDataScript.Instance.weChatInformation.headIcon;
                headIconImg.preserveAspect = true;
            }
            catch (Exception e)
            {
                MyDebug.Log("LoadImg" + e.Message);
            }
        }
        else
        {
            ResourcesLoader.Load<Sprite>("BaseAssets/UI_Online/UI_Public/head" + GlobalDataScript.userData.dwCustomID, (sprite) =>
            {
                headIconImg.sprite = sprite;
                headIconImg.preserveAspect = true;
            });
        }
    }
    public IEnumerator LoadImgI(string url)
    {

        var www = new WWW(url);
        yield return www;
        //下载完成，保存图片到路径filePath
        try
        {
            gonggao.preserveAspect = false;
            GlobalDataScript.Instance.weChatInformation = new WeChatInformation();
            texture2D = www.texture;
            //将图片赋给场景上的Sprite
            GlobalDataScript.Instance.weChatInformation.headIcon = Sprite.Create(texture2D,
                new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0, 0));
            gonggao.sprite = GlobalDataScript.Instance.weChatInformation.headIcon;
            gonggao.preserveAspect = true;
            gonggao.enabled = true;
        }
        catch (Exception e)
        {
            MyDebug.Log("LoadImg" + e.Message);
        }

    }

    public void ShowAuthentication()
    {
        HttpManager.instance.GetSystemsNewsList(getNewsReback);
    }

    private void getNewsReback(WWW message)
    {
        JsonData json = JsonMapper.ToObject<JsonData>(message.text);
        MainNewsDataManager.instance.SetNewsInfo(json["data"].ToJson());
        UIManager.instance.Show(UIType.UIEMail);
    }

    public void SetOneSystemNews(WWW mes)
    {
        UIManager.instance.Show(UIType.UIRealname);
    }

    public void ShowUserInfo()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIUserInfo, InitInfo);
    }

    private void InitInfo(GameObject go)
    {
        go.GetComponent<UIPanel_UserInfo>().SetUIData();
    }

    public void ShowRank()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIRank);
    }

    //游戏设置
    public void ShowSetting()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UISetting);
    }

    //大厅设置
    public void ShowSetHall()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UISetHall);
    }

    public void ShowInvite()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIInvite);
    }

    public void ShowPay()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIRecharge);
    }

    public void ShowGameTutorial()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UICourse);
    }

    public void ShowHelp()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIHelp);
    }

    public void ShowRedBao()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIWithdrawal);
    }

    public void ShowSignIn()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        if (SiginDataManager.siginData == null)
        {
            HttpManager.instance.SentHttpRequre(HTTP_TYPE.GetSigin, GetSignInCallBack);
        }
        else
        {
            UIManager.instance.Show(UIType.UISignIn);
        }

    }

    //代理弹出框
    public void ShowAgent()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIAgent);
    }

    //客服弹出框
    public void KeFu()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIKeFu);
    }

    //玩法弹出框
    public void WanFa()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIWanFa);
    }

    private void GetSignInCallBack(WWW msg)
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SiginDataManager.siginData = JsonMapper.ToObject<SiginData>(msg.text);
        UIManager.instance.Show(UIType.UISignIn);
    }

    public void ShowService()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIActivity);
    }

    public void ShowMilitary()
    {
        MainNewsDataManager.instance.ClearQuerys();
        MainNewsDataManager.instance.Init();
        SocketLoginEvent.instance.Init();
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UILoading);
        SocketLoginEvent.instance.main = (int)MainCmd.MDM_MB_PERSONAL_SERVICE;
        SocketLoginEvent.instance.sub = (int)MDM_GF_FRAME_BLH.SUB_GR_USER_QUERY_ROOM_SCORE;
        SocketLoginEvent.instance.GetQueryPernalScore(GlobalDataScript.NN_KIND_ID);
    }

    public void ShowShare()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIShare);
    }

    public void ShowCreateRoom()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UICreateRoom);
        MyDebug.Log("调出" + UIType.UIsssCreateRoom + "创建房间界面");
    }

    public void ShowJoinRoom()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIJoinRoom);
        MyDebug.Log("调出加入房间界面");
    }
    public void ShowJulebu()
    {
        SocketJuleBu.instance.Link();
    }
}