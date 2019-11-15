using System;
using LitJson;

namespace AssemblyCSharp
{
    public class LoginRequest : ClientRequest
    {
        public LoginRequest(string data)
        {
            headCode = APIS.LOGIN_REQUEST;
            if (data == null)
            {
                LoginVo loginvo = new LoginVo();
                Random ran = new Random();
                string str = ran.Next(100, 1000) + "for" + ran.Next(2000, 5000);
                loginvo.openId = UIPanelLogin.TestId + "";
                loginvo.nickName = UIPanelLogin.TestId + "";
                loginvo.headIcon = "imgicon";
                loginvo.unionid = UIPanelLogin.TestId + "";
                loginvo.province = "21sfsd";
                loginvo.city = "afafsdf";
                loginvo.sex = 1;
                loginvo.IP = GlobalDataScript.Instance.GetIpAddress();
                data = JsonMapper.ToJson(loginvo);
                GlobalDataScript.loginVo = loginvo;
                GlobalDataScript.loginResponseData = new AvatarVO();
                GlobalDataScript.loginResponseData.account = new Account();
                GlobalDataScript.loginResponseData.account.city = loginvo.city;
                GlobalDataScript.loginResponseData.account.openid = loginvo.openId;
                GlobalDataScript.loginResponseData.account.nickname = loginvo.nickName;
                GlobalDataScript.loginResponseData.account.headicon = loginvo.headIcon;
                GlobalDataScript.loginResponseData.account.unionid = loginvo.unionid;
                GlobalDataScript.loginResponseData.account.sex = loginvo.sex;
                GlobalDataScript.loginResponseData.IP = loginvo.IP;
            }

            messageContent = data;
        }

        /**用于重新登录使用**/
        //退出登录
        public LoginRequest()
        {
            headCode = APIS.QUITE_LOGIN;
            if (GlobalDataScript.loginResponseData != null)
            {
                messageContent = GlobalDataScript.loginResponseData.account.uuid + "";
            }
        }
    }
}