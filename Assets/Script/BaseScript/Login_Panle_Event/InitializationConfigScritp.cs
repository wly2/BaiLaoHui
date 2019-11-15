using UnityEngine;
using AssemblyCSharp;

public class InitializationConfigScritp : MonoBehaviour
{
    int num;
    bool hasPaused;

    void Start()
    {
        //MicroPhoneInput.getInstance ();
        //GlobalDataScript.instance ();
        //CustomSocket.getInstance.Connect();
        //ChatSocket.getInstance;
        //TipsManagerScript.getInstance.parent = gameObject.transform;
        //SoundCtrl.getInstance;
        UpdateScript update = new UpdateScript();
        //StartCoroutine (update.updateCheck ());
        //ServiceErrorListener seriveError = new ServiceErrorListener();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //heartbeatTimer ();
        //heartbeatThread();
    }

    void Awake()
    {
        SocketEventHandle.Instance.disConnetReply += DisConnetReply;
        SocketEventHandle.Instance.lotteryReply += LotteryReply;
        SocketEventHandle.Instance.otherDeviceLoginReply += OtherDeviceLoginReply;
    }

    private void DisConnetReply()
    {
        if (GlobalDataScript.isonLoginPage)
        {
        }
        else
        {
            CleaListener();
            MySceneManager.instance.SceneToLogIn();
        }
    }

    private void OtherDeviceLoginReply(ClientResponse response)
    {
        //	TipsManagerScript.getInstance.setTips ("你的账号在其他设备登录");
        DisConnetReply();
    }

    private void CleaListener()
    {
        /*
		if (SocketEventHandle.getInstance ().LoginCallBack != null) {
			SocketEventHandle.getInstance ().LoginCallBack = null;
		}
*/
        if (SocketEventHandle.Instance.createRoomReply != null)
        {
            SocketEventHandle.Instance.createRoomReply = null;
        }

        if (SocketEventHandle.Instance.joinRoomReply != null)
        {
            SocketEventHandle.Instance.joinRoomReply = null;
        }

        if (SocketEventHandle.Instance.startGameReply != null)
        {
            SocketEventHandle.Instance.startGameReply = null;
        }

        if (SocketEventHandle.Instance.pickCardReply != null)
        {
            SocketEventHandle.Instance.pickCardReply = null;
        }

        if (SocketEventHandle.Instance.otherPickCardReply != null)
        {
            SocketEventHandle.Instance.otherPickCardReply = null;
        }

        if (SocketEventHandle.Instance.otherPutOutCardReply != null)
        {
            SocketEventHandle.Instance.otherPutOutCardReply = null;
        }

        if (SocketEventHandle.Instance.otherPentReply != null)
        {
            SocketEventHandle.Instance.otherPentReply = null;
        }

        if (SocketEventHandle.Instance.gangReply != null)
        {
            SocketEventHandle.Instance.gangReply = null;
        }

        if (SocketEventHandle.Instance.huReply != null)
        {
            SocketEventHandle.Instance.huReply = null;
        }

        if (SocketEventHandle.Instance.otherChiReply != null)
        {
            SocketEventHandle.Instance.otherChiReply = null;
        }

        if (SocketEventHandle.Instance.otherGangReply != null)
        {
            SocketEventHandle.Instance.otherGangReply = null;
        }

        if (SocketEventHandle.Instance.actionBtnReply != null)
        {
            SocketEventHandle.Instance.actionBtnReply = null;
        }

        if (SocketEventHandle.Instance.quitRoomReply != null)
        {
            SocketEventHandle.Instance.quitRoomReply = null;
        }

        if (SocketEventHandle.Instance.dissolveRoomReply != null)
        {
            SocketEventHandle.Instance.dissolveRoomReply = null;
        }

        if (SocketEventHandle.Instance.readyReply != null)
        {
            SocketEventHandle.Instance.readyReply = null;
        }

        if (SocketEventHandle.Instance.messageBoxReply != null)
        {
            SocketEventHandle.Instance.messageBoxReply = null;
        }

        if (SocketEventHandle.Instance.reLoginReply != null)
        {
            SocketEventHandle.Instance.reLoginReply = null;
        }

        /*
		if (SocketEventHandle.getInstance ().RoomBackResponse != null) {
			SocketEventHandle.getInstance ().RoomBackResponse = null;
		}
		*/
        if (SocketEventHandle.Instance.cardChangeReply != null)
        {
            SocketEventHandle.Instance.cardChangeReply = null;
        }

        if (SocketEventHandle.Instance.offlineReply != null)
        {
            SocketEventHandle.Instance.offlineReply = null;
        }

        if (SocketEventHandle.Instance.onlineReply != null)
        {
            SocketEventHandle.Instance.onlineReply = null;
        }

        if (SocketEventHandle.Instance.giftReply != null)
        {
            SocketEventHandle.Instance.giftReply = null;
        }

        if (SocketEventHandle.Instance.returnGameReply != null)
        {
            SocketEventHandle.Instance.returnGameReply = null;
        }

        if (SocketEventHandle.Instance.followBankerReply != null)
        {
            SocketEventHandle.Instance.followBankerReply = null;
        }

        if (SocketEventHandle.Instance.contactInfoReply != null)
        {
            SocketEventHandle.Instance.contactInfoReply = null;
        }

        if (SocketEventHandle.Instance.recordReply != null)
        {
            SocketEventHandle.Instance.recordReply = null;
        }

        if (SocketEventHandle.Instance.recordDetailReply != null)
        {
            SocketEventHandle.Instance.recordDetailReply = null;
        }

        if (SocketEventHandle.Instance.gameBattleReply != null)
        {
            SocketEventHandle.Instance.gameBattleReply = null;
        }
    }

    void FixedUpdate()
    {
        /*
		num++;
		if (num == 150) {
			num = 0;
			CustomSocket.getInstance.sendHeadData ();
		}
		*/
    }

    //private  void heartbeatTimer(){
    //	t = new System.Timers.timer(1000);   //实例化Timer类，设置间隔时间为10000毫秒；
    //	t.Elapsed += new System.Timers.ElapsedEventHandler(doSendHeartbeat); //到达时间的时候执行事件；
    //	t.AutoReset = true;   //设置是执行一次（false）还是一直执行(true)；
    //	t.Enabled = true;     //是否执行System.Timers.timer.Elapsed事件；
    //}
    //  //static Thread heartThread;
    //   private void heartbeatThread(){
    //       Thread heartThread = new Thread (sendHeartbeat);
    //       heartThread.Name = "ttttttttttttttttttt";
    //       heartThread.IsBackground = true;
    //       heartThread.Start();
    //}
    //private static void sendHeartbeat(){
    //       TcpClientHandler.getInstance.sendHeadData();
    //       Thread.Sleep (4000);
    //	sendHeartbeat ();
    //}
    //   public static void CloseThread()
    //   {
    //       //if (heartThread != null)
    //       //{
    //       //    heartThread.Interrupt();
    //       //    heartThread.Abort();
    //       //}
    //   }
    public void DoSendHeartbeat(object source, System.Timers.ElapsedEventArgs e)
    {
        CustomSocket.Instance.SendHeadData();
        //      bool flag =
        //if (!flag) {
        //	if (t != null) {
        //		t.Stop ();
        //	}
        //}
    }

    private void LotteryReply(ClientResponse response)
    {
        int giftTimes = int.Parse(response.message);
        GlobalDataScript.loginResponseData.account.prizecount = giftTimes;
        if (CommonEvent.GetInstance.lotteryCountChange != null)
        {
            CommonEvent.GetInstance.lotteryCountChange();
        }
    }
}