using System.Collections;
using UnityEngine;
using AssemblyCSharp;
using System.Xml;
/*
 * *简易软件大版本升级
 */

public class UpdateScript
{
    private ServiceVersionVo serviceVersionVo = new ServiceVersionVo();
    private string currentVersion = Application.version; //当前软件版本号
    private string serverVersion; //服务器上软件版本号

    private string downloadPath; //应用下载链接

    /**
	 * 检测升级
	 */
    public IEnumerator UpdateCheck()
    {
        WWW www = new WWW(APIS.UPDATE_INFO_JSON_URL);
        yield return www;
        byte[] buffer = www.bytes;
        string returnxml = NetUtil.BytesToString(buffer);
        MyDebug.Log("returnxml  =  " + returnxml);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(returnxml);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("versions").ChildNodes;
        for (int i = 0; i < nodeList.Count; ++i)
        {
            Version123 temp = new Version123();
            temp.title = nodeList[i].SelectSingleNode("title").InnerText;
            temp.url = nodeList[i].SelectSingleNode("url").InnerText;
            temp.note = nodeList[i].SelectSingleNode("note").InnerText;
            temp.version = nodeList[i].SelectSingleNode("versionname").InnerText;
            XmlElement xe = (XmlElement) nodeList[i];
            if (xe.GetAttribute("id") == "ios")
            {
                serviceVersionVo.ios = temp;
                serviceVersionVo.ios.url += "l=zh&mt=8";
            }
            else if (xe.GetAttribute("id") == "android")
            {
                serviceVersionVo.Android = temp;
            }
        }

        CompareVersion();
    }

    //对比版本虚
    public void CompareVersion()
    {
        int currentVerCode; //当前版本号数字
        int serverVerCode; //服务器上版本号数字
        currentVersion = currentVersion.Replace(".", "");
        currentVerCode = int.Parse(currentVersion);
        Version123 versionTemp = new Version123(); //版本信息
        //versionTemp = serviceVersionVo.Android;
        if (Application.platform == RuntimePlatform.Android)
        {
            versionTemp = serviceVersionVo.Android;
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            versionTemp = serviceVersionVo.ios;
        }

        if (versionTemp != null && versionTemp.version != null)
        {
            serverVersion = versionTemp.version;
            serverVersion = serverVersion.Replace(".", "");
            serverVerCode = int.Parse(serverVersion);
            if (serverVerCode > currentVerCode)
            {
                //服务器上有新版本
                string note = versionTemp.note;
                downloadPath = versionTemp.url;

                TipsManagerScript.getInstance.loadDialog(LocalizationManager.GetInstance.GetValue("KEY.20018"), note,
                    OnSureClick, OnCancle);
            }
        }
    }

    public void OnSureClick()
    {
        if (downloadPath != null)
        {
            Application.OpenURL(downloadPath);
        }
    }

    public void OnCancle()
    {
    }
}