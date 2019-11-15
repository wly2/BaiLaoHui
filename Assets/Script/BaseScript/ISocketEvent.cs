public interface ISocketEvent
{
    void ISocketEngineSink();//连接服务器
    void ISocketEngineSink(int kind_Id);
    void ISocketEngineSink(string ip, int port);
    void OnEventTCPSocketLink();//连接服务器成功返回
    void OnEventTCPSocketShut();//断开服务器
    void OnEventTCPSocketError(int errorCode);//连接服务器错误
    void OnEventTCPSocketRead(int main, int sub, byte[] tmpBuf, int size);//服务器返回消息处理
    bool OnEventTCPHeartTick();//
}