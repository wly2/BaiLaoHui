using System;
using System.Net.Sockets;
using System.Text;
using AssemblyCSharp;
using System.IO;

public class CustomSocket
{
    //reset对象
    //public ManualResetEvent connectDone = new ManualResetEvent(false);
    //tcp客户端,即是与服务端通讯的组件
    int waitLen;
    bool isWait;

    TcpClient tcpclient = new TcpClient();

    //网络流
    NetworkStream stream;

    //接受到的数据
    public byte[] databuffer = new byte[StateObject.BufferSize]; //数据缓冲区;
    public int offset; //处理的位置
    public int end; //数据缓冲区的长度
    byte[] sources;
    byte[] headBytes;
    private int disConnectCount;
    public static bool hasStartTimer;
    public bool isConnected;
    private static CustomSocket _instance;

    public static CustomSocket Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CustomSocket();
                //_instance.Connect ();
            }

            return _instance;
        }
    }

    public CustomSocket()
    {
        HeadRequest head = new HeadRequest();
        headBytes = head.ToBytes();
    }

    /// <summary>
    /// 连接到服务器
    /// </summary>
    /// <param name="ip">服务器IP</param>
    /// <returns></returns>
    public void Connect()
    {
        //disCountTimer ();
        try
        {
            tcpclient = new TcpClient(APIS.tcpFamily);
            //防止延迟,即时发送!
            tcpclient.NoDelay = true;
            tcpclient.BeginConnect(APIS.socketUrl, 10122, new AsyncCallback(ConnectCallback), tcpclient);
        }
        catch (Exception ex)
        {
            //设置标志,连接服务端失败!
            ShowMessageTip(LocalizationManager.GetInstance.GetValue("KEY.20015"));
            //	ReConnectScript.getInstance().ReConnectToServer();
            MyDebug.Log(ex.ToString());
            isConnected = false;
        }
    }

    /// <summary>
    /// 关闭网络流
    /// </summary>
    private void DisConnect()
    {
        if (tcpclient != null)
        {
            tcpclient.Close();
            tcpclient = null;
        }

        if (stream != null)
        {
            stream.Close();
            stream = null;
        }
    }

    public void SendMsg(ClientRequest client)
    {
        return;
        SendData(client.ToBytes());
    }

    /// <summary>
    /// 发送数据
    /// </summary>
    private void SendData(byte[] data)
    {
        //MyDebug.Log ("send data"+data.ToString ());
        try
        {
            if (stream != null)
            {
                stream.Write(data, 0, data.Length);
            }
            else
            {
                //	showMessageTip("服务器断开连接，请重新运行程序或稍后再试");
                isConnected = false;
                SocketEventHandle.Instance.NoticeDisConect();
                //ReConnectScript.getInstance().ReConnectToServer();
                //Connect();
            }
        }
        catch (Exception ex)
        {
            MyDebug.Log(ex.ToString());
            //	showMessageTip("服务器断开连接，请重新运行程序或稍后再试");
            isConnected = false;
            SocketEventHandle.Instance.NoticeDisConect();
            //ReConnectScript.getInstance().ReConnectToServer();
            //Connect();
        }
    }

    /// <summary>
    /// 发送心跳包
    /// </summary>
    /// *
    public bool SendHeadData()
    {
        MyDebug.Log("send head data");
        try
        {
            if (stream != null && tcpclient.Connected)
            {
                stream.Write(headBytes, 0, headBytes.Length);
                return true;
            }
            else
            {
                isConnected = false;
                SocketEventHandle.Instance.NoticeDisConect();
                return false;
            }
        }
        catch (Exception ex)
        {
            MyDebug.Log(ex.ToString());
            isConnected = false;
            //showMessageTip ("服务器已断开连接，请重新登录");
            isConnected = false;
            SocketEventHandle.Instance.NoticeDisConect();
            return false;
        }
    }

    private void ShowMessageTip(string message)
    {
        ClientResponse temp = new ClientResponse();
        temp.headCode = APIS.TIP_MESSAGE;
        temp.message = message;
        SocketEventHandle.Instance.AddResponse(temp);
    }

    /// <summary>
    /// 异步连接的回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void ConnectCallback(IAsyncResult ar)
    {
        //connectDone.Set();
        if ((tcpclient != null) && (tcpclient.Connected))
        {
            stream = tcpclient.GetStream();
            Asyncread(tcpclient);
            isConnected = true;
            MyDebug.Log("服务器已经连接!");
        }

        TcpClient t = (TcpClient) ar.AsyncState;
        try
        {
            t.EndConnect(ar);
        }
        catch (Exception ex)
        {
            //设置标志,连接服务端失败!
            MyDebug.Log(ex.ToString());
            //	tcpclient.BeginConnect(APIS.socketUrl, 1101, new AsyncCallback(ConnectCallback), tcpclient);
        }
    }

    /// <summary>
    /// 异步读TCP数据
    /// </summary>
    /// <param name="sock"></param>
    private void Asyncread(TcpClient sock)
    {
        StateObject state = new StateObject();
        state.client = sock;
        NetworkStream stream;
        try
        {
            stream = sock.GetStream();
            if (stream.CanRead)
            {
                try
                {
                    IAsyncResult ar = stream.BeginRead(state.buffer, 0, StateObject.BufferSize,
                        new AsyncCallback(TCPReadCallBack), state);
                }
                catch (Exception ex)
                {
                    //设置标志,连接服务端失败!
                    MyDebug.Log(ex.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            //设置标志,连接服务端失败!
            // NetManaged.isConnectServer = false;
            // NetManaged.surcessstate = 0;
            MyDebug.Log(ex.ToString());
        }
    }

    /// <summary>
    /// TCP读数据的回调函数
    /// </summary>
    /// <param name="ar"></param>
    private void TCPReadCallBack(IAsyncResult ar)
    {
        StateObject state = (StateObject) ar.AsyncState;
        //主动断开时
        if ((state.client == null) || (!state.client.Connected))
        {
            CloseSocket();
            return;
        }

        int numberOfBytesRead;
        NetworkStream mas = state.client.GetStream();
        numberOfBytesRead = mas.EndRead(ar);
        state.totalBytesRead += numberOfBytesRead;
        if (numberOfBytesRead > 0)
        {
            byte[] dd = new byte[numberOfBytesRead];
            Array.Copy(state.buffer, 0, dd, 0, numberOfBytesRead);
            if (isWait)
            {
                byte[] temp = new byte[sources.Length + dd.Length];
                sources.CopyTo(temp, 0);
                dd.CopyTo(temp, sources.Length);
                sources = temp;
                if (sources.Length >= waitLen)
                {
                    ReceiveCallBack(sources.Clone() as byte[]);
                    isWait = false;
                    waitLen = 0;
                }
            }
            else
            {
                sources = null;
                ReceiveCallBack(dd);
            }

            mas.BeginRead(state.buffer, 0, StateObject.BufferSize,
                new AsyncCallback(TCPReadCallBack), state);
        }
        else
        {
            //被动断开时
            mas.Close();
            state.client.Close();
            mas = null;
            state = null;
            //设置标志,连接服务端失败!
            MyDebug.Log("客户端被动断开");
        }
    }

    /// <summary>
    /// 读取大端序的int
    /// </summary>
    /// <param name="value"></param>
    public int ReadInt(byte[] intbytes)
    {
        Array.Reverse(intbytes);
        return BitConverter.ToInt32(intbytes, 0);
    }

    public short ReadShort(byte[] intbytes)
    {
        Array.Reverse(intbytes);
        return BitConverter.ToInt16(intbytes, 0);
    }

    private void ReceiveCallBack(byte[] m_receiveBuffer)
    {
        //通知调用端接收完毕
        try
        {
            //	MyDebug.Log("m_receiveBuffer======"+m_receiveBuffer.Length);
            MemoryStream ms = new MemoryStream(m_receiveBuffer);
            BinaryReader buffers = new BinaryReader(ms, UTF8Encoding.Default);
            ReadBuffer(buffers);
        }
        catch (Exception ex)
        {
            MyDebug.Log("socket exception:" + ex.Message);
            throw new Exception(ex.Message);
        }
    }

    private void ReadBuffer(BinaryReader buffers)
    {
        byte flag = buffers.ReadByte();
        int lens = ReadInt(buffers.ReadBytes(4));
        disConnectCount = 0;
        if (!hasStartTimer && lens == 16)
        {
            StartTimer();
            hasStartTimer = true;
        }

        //MyDebug.Log ("lengs ====>>  "+lens);
        if (lens > buffers.BaseStream.Length)
        {
            waitLen = lens;
            isWait = true;
            buffers.BaseStream.Position = 0;
            byte[] dd = new byte[buffers.BaseStream.Length];
            byte[] temp = buffers.ReadBytes((int) buffers.BaseStream.Length);
            Array.Copy(temp, 0, dd, 0, (int) buffers.BaseStream.Length);
            if (sources == null)
            {
                sources = dd;
            }

            return;
        }

        int headcode = ReadInt(buffers.ReadBytes(4));
        int status = ReadInt(buffers.ReadBytes(4));
        short messageLen = ReadShort(buffers.ReadBytes(2));
        if (flag == 1)
        {
            string message = Encoding.UTF8.GetString(buffers.ReadBytes(messageLen));
            ClientResponse response = new ClientResponse();
            response.status = status;
            response.message = message;
            response.headCode = headcode;
            MyDebug.Log("response.headCode = " + response.headCode + "  response.message =   " + message);
            SocketEventHandle.Instance.AddResponse(response);
        }

        if (buffers.BaseStream.Position < buffers.BaseStream.Length)
        {
            ReadBuffer(buffers);
        }
    }

    public void CloseSocket()
    {
        DisConnect();
    }

    System.Timers.Timer t;

    private void StartTimer()
    {
        if (t == null)
        {
            t = new System.Timers.Timer(20000); //实例化Timer类，设置间隔时间为10000毫秒；
            t.Elapsed += new System.Timers.ElapsedEventHandler(Timeout); //到达时间的时候执行事件；
            t.AutoReset = true; //设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true; //是否执行System.Timers.timer.Elapsed事件；
        }
        else
        {
            t.Start();
        }
    }

    public void Timeout(object source, System.Timers.ElapsedEventArgs e)
    {
        //MyDebug.Log ("disConnectCount+" + disConnectCount);
        disConnectCount += 1;
        if (disConnectCount >= 15)
        {
            t.Stop();
            disConnectCount = 0;
            //	closeSocket ();
            isConnected = false;
            SocketEventHandle.Instance.NoticeDisConect();
            return;
        }
    }
}