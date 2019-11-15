using UnityEngine;

/*
 * 服务器返回错误
 */
namespace AssemblyCSharp
{
    public class ServiceErrorListener : MonoBehaviour
    {
        public ServiceErrorListener()
        {
            SocketEventHandle.Instance.serviceErrorReply += ServiceErrorReply;
        }

        public void ServiceErrorReply(ClientResponse response)
        {
            TipsManagerScript.getInstance.setTips(response.message);
        }
    }
}