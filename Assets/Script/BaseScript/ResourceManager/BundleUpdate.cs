using AssemblyCSharp;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class BundleUpdate : MonoSingleton<BundleUpdate>
{
    public delegate void HandleFinishDownload(WWW www);
    //web 资源配置表        
    Dictionary<string, List<string>> DicMD5 = new Dictionary<string, List<string>>();
    //本地资源配置表                  
    Dictionary<string, List<string>> DicMD5Loc = new Dictionary<string, List<string>>();
    //需要更行的资源配置表
    List<string> DicMD5UpdateName = new List<string>();
    int updateCount = -1;
    public bool isDone = false;
    UnityAction DoneLoaded;

    public bool isDisConnect = false;
    private bool isCompare;
    public void CompareUpdate(UnityAction onLoaded)
    {
        DoneLoaded = onLoaded;
        if (AssetBundleManager.instance.LoadMode == AssetBundleLoadMode.LoadFromWWW)
            ReadWebVersion();
    }
    private void ReadWebVersion()
    {
        isCompare = true;
        //string fileName = AssetBundleConfig.ASSETBUNDLE_PATH_D + "StreamingAssets/VersionMD5.xml";
        //// 如果文件不存在，则直接返回
        //if (System.IO.File.Exists(fileName) == false)
        //    return;                                                     
        string fileName = AssetBundleConfig.ASSETBUNDLE_PATH_WEB + "StreamingAssets/VersionMD5.xml";
        StartCoroutine(this.DownLoad(fileName, delegate (WWW w)
        {
            if (w.error != null)
            {
                isDisConnect = true;
                Invoke("ReadWebVersion", 1);
                MyDebug.Log(w.error);
            }
            else
            {
                isDisConnect = false;
                AnalysisXml(new MemoryStream(w.bytes));
            }

            //#if UNITY_IOS
            //         IOsAnalysisXml(w.text);
            //#elif UNITY_ANDROID || UNITY_STANDALONE_WIN || UNITY_EDITOR
            //            AnalysisXml(new MemoryStream(w.bytes));
            //#endif          
            //            //将下载的资源替换本地就的资源       

        }));

    }
    // private void IOsAnalysisXml(string mes)
    // {

    //     XmlDocument xmlDoc = new XmlDocument();

    //     xmlDoc.Load(new StringReader(mes));
    //     XmlElement XmlRoot = xmlDoc.DocumentElement;
    //     foreach (XmlNode node in XmlRoot.ChildNodes)
    //     {
    //         if ((node is XmlElement) == false)
    //             continue;


    //         List<string> tempList = new List<string>();
    //         string file = (node as XmlElement).GetAttribute("FileName");
    //         string md5 = (node as XmlElement).GetAttribute("MD5");
    //         string size = (node as XmlElement).GetAttribute("Size");
    //         tempList.Add(md5);
    //         tempList.Add(size);
    //         if (DicMD5.ContainsKey(file) == false)
    //         {
    //             DicMD5.Add(file, tempList);
    //         }
    //     }
    //     XmlRoot = null;
    //     xmlDoc = null;
    //     IosReadLocalVersion();
    // }

    // private void IosReadLocalVersion()
    // {
    //     string savePath = Application.persistentDataPath + "/StreamingAssets";
    //     if (Directory.Exists(savePath) == false)
    //     {
    //         MyDebug.Log("创建本地资源文件夹");
    //         Directory.CreateDirectory(savePath);

    //     }
    //     XmlDocument sXmlDoc = new XmlDocument();
    //     XmlElement SXmlRoot;


    //     if (System.IO.File.Exists(savePath + "/VersionMD5.xml") == false)
    //     {
    //         //MyDebug.Log("本地资源配置不存在");
    //         //SXmlRoot = sXmlDoc.CreateElement("Files");
    //         //sXmlDoc.AppendChild(SXmlRoot);
    //         //foreach (KeyValuePair<string, List<string>> pair in DicMD5)
    //         //{
    //         //    XmlElement xmlElem = sXmlDoc.CreateElement("File");
    //         //    SXmlRoot.AppendChild(xmlElem);
    //         //    xmlElem.SetAttribute("FileName", pair.Key);
    //         //    xmlElem.SetAttribute("MD5", pair.Value[0]);              
    //         //    xmlElem.SetAttribute("Size", pair.Value[1]);
    //         //}
    //         //sXmlDoc.Save(savePath + "/VersionMD5.xml");
    //         //sXmlDoc = null;
    //     }
    //     else
    //     {
    //string messss = File.ReadAllText (savePath + "/VersionMD5.xml");
    //Debug.Log (messss);
    //sXmlDoc.Load(new StringReader(messss));

    //	SXmlRoot = sXmlDoc.DocumentElement;
    //	foreach (XmlNode node in SXmlRoot.ChildNodes) {
    //		if ((node is XmlElement) == false)
    //			continue;


    //		List<string> tempList = new List<string> ();
    //		string file = (node as XmlElement).GetAttribute ("FileName");
    //		string md5 = (node as XmlElement).GetAttribute ("MD5");
    //		string size = (node as XmlElement).GetAttribute ("Size");
    //		tempList.Add (md5);
    //		tempList.Add (size);


    //		if (DicMD5Loc.ContainsKey (file) == false) {
    //			DicMD5Loc.Add (file, tempList);
    //		}
    //	}
    //	MyDebug.Log ("读取本地资源配置文件");

    //     }
    //     currentNumShow = 0;
    //     CompareVersion();
    //     DownLoadRes();
    // }
    private void AnalysisXml(Stream stream)
    {
        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(stream);
        XmlElement XmlRoot = XmlDoc.DocumentElement;
        foreach (XmlNode node in XmlRoot.ChildNodes)
        {
            if ((node is XmlElement) == false)
                continue;


            List<string> tempList = new List<string>();
            string file = (node as XmlElement).GetAttribute("FileName");
            string md5 = (node as XmlElement).GetAttribute("MD5");
            string size = (node as XmlElement).GetAttribute("Size");
            tempList.Add(md5);
            tempList.Add(size);
            if (DicMD5.ContainsKey(file) == false)
            {
                DicMD5.Add(file, tempList);
            }
        }
        XmlRoot = null;
        XmlDoc = null;
        ReadLocalVersion();
    }
    private void ReadLocalVersion()
    {
        string savePath = Application.persistentDataPath + "/StreamingAssets";
        if (Directory.Exists(savePath) == false)
        {
            MyDebug.Log("创建本地资源文件夹");
            Directory.CreateDirectory(savePath);

        }
        XmlDocument sXmlDoc = new XmlDocument();
        XmlElement SXmlRoot;
        if (System.IO.File.Exists(savePath + "/VersionMD5.xml") == false)
        {
            //MyDebug.Log("本地资源配置不存在");
            //SXmlRoot = sXmlDoc.CreateElement("Files");
            //sXmlDoc.AppendChild(SXmlRoot);
            //foreach (KeyValuePair<string, List<string>> pair in DicMD5)
            //{
            //    XmlElement xmlElem = sXmlDoc.CreateElement("File");
            //    SXmlRoot.AppendChild(xmlElem);
            //    xmlElem.SetAttribute("FileName", pair.Key);
            //    xmlElem.SetAttribute("MD5", pair.Value[0]);
            //    xmlElem.SetAttribute("Size", pair.Value[1]);
            //}
            //sXmlDoc.Save(savePath + "/VersionMD5.xml");
            //sXmlDoc = null;
        }
        else
        {
            sXmlDoc.Load(savePath + "/VersionMD5.xml");
            SXmlRoot = sXmlDoc.DocumentElement;
            foreach (XmlNode node in SXmlRoot.ChildNodes)
            {
                if ((node is XmlElement) == false)
                    continue;


                List<string> tempList = new List<string>();
                string file = (node as XmlElement).GetAttribute("FileName");
                string md5 = (node as XmlElement).GetAttribute("MD5");
                string size = (node as XmlElement).GetAttribute("Size");
                tempList.Add(md5);
                tempList.Add(size);


                if (DicMD5Loc.ContainsKey(file) == false)
                {
                    DicMD5Loc.Add(file, tempList);
                }
            }
            MyDebug.Log("读取本地资源配置文件");
        }
        currentNumShow = 0;
        CompareVersion();
        DownLoadRes();

    }
    long updateSize = 0;
    private void CompareVersion()
    {                                                                  
        foreach (KeyValuePair<string, List<string>> pair in DicMD5)
        {

            if (DicMD5Loc.ContainsKey(pair.Key))
            {
                if (DicMD5Loc[pair.Key][0] == pair.Value[0] && DicMD5Loc[pair.Key][1] == pair.Value[1])
                {
                    continue;
                }
                else
                {
                    MyDebug.Log(pair.Key);
                    updateSize += int.Parse(pair.Value[1]);
                    DicMD5UpdateName.Add(pair.Key);
                }
            }
            else
            {
                MyDebug.Log(pair.Key);
                updateSize += int.Parse(pair.Value[1]);
                DicMD5UpdateName.Add(pair.Key);
            }
        }
        //  updateCount = DicMD5UpdateName.Count;
        MyDebug.Log(updateSize);
        isCompare = false;
    }
    string filePathSigle;
    private void DownLoadRes()
    {
        updateCount++;
        if (DicMD5UpdateName.Count == updateCount)
        {
            MyDebug.Log("==================###########  开始解压  #################===============================");
            StartCoroutine(UnPreccFiles());
            return;

        }
        if (updateCount > 0)
            currentNumShow += int.Parse(DicMD5[DicMD5UpdateName[updateCount - 1]][1]);
        string name = Path.GetFileName(DicMD5UpdateName[updateCount]);
        MyDebug.Log("name:" + name);
        filePathSigle = DicMD5UpdateName[updateCount].Substring(0, DicMD5UpdateName[updateCount].LastIndexOf(name));
        MyDebug.Log(filePathSigle);
        if (Directory.Exists(Application.persistentDataPath + "/" + filePathSigle) == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + filePathSigle);
        }
        if (Directory.Exists(Application.persistentDataPath + "/Zip/" + filePathSigle) == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Zip/" + filePathSigle);
        }
        string file = DicMD5UpdateName[updateCount];

        //  Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++DownLoadRes:" + file);
        // StartCoroutine(DownLoadFile(file));
        currentNum = 0;
        StartCoroutine(this.DownLoad(AssetBundleConfig.ASSETBUNDLE_PATH_WEB + file, delegate (WWW w)
        {
            if (w.error != null)
            {
                isDisConnect = true;
                updateCount--;
                Invoke("DownLoadRes", 1);
                MyDebug.Log(w.error);
            }
            else
            {
                isDisConnect = false;
                ReplaceLocalRes(file, w.bytes);
            }
        }));

    }
    private void ReplaceLocalRes(string fileName, byte[] data)
    {
        // string filePath = LOCAL_RES_PATH + fileName;        
        //  FileStream stream = new FileStream(Application.persistentDataPath + "/Cahe.zip", FileMode.Create);
        FileStream stream = new FileStream(Application.persistentDataPath + "/Zip/" + fileName, FileMode.Create);
        //FileStream stream = new FileStream(Application.persistentDataPath + "/" + fileName + ".zip", FileMode.Create);
        stream.Write(data, 0, data.Length);
        stream.Flush();
        stream.Close();
        DownLoadRes();

    }
    private IEnumerator DownLoad(string url, HandleFinishDownload finishFun)
    {
        WWW www = new WWW(url);
        while (!www.isDone)
        {
            if (DicMD5.Count > 0)
                currentNum = (www.progress * int.Parse(DicMD5[DicMD5UpdateName[updateCount]][1]));
            else
                currentNum = www.progress;
            // LoadPro = (((int)(www.progress * 100)) % 100) + "%";
            //Debug.Log("进度：" + www.progress);
            //Debug.Log("进度：" + currentNum);
            //Debug.Log("进度：" + DicMD5[DicMD5UpdateName[updateCount]][1]);
            yield return 1;
        }
        yield return www;
        if (finishFun != null)
        {
            finishFun(www);
        }
        www.Dispose();
    }

    private IEnumerator UnPreccFiles()
    {
        UIPanel_SceneLoading.percentV = 100 - (DicMD5UpdateName.Count * 100.0f / updateCount);
        UIPanel_SceneLoading.instance.tips.text = "正在解压资源，过程不消耗流量（" + (updateCount - DicMD5UpdateName.Count) + "/" + updateCount + ")";
        yield return new WaitForEndOfFrame();
        if (DicMD5UpdateName.Count <= 0)
        {
            string savePath = Application.persistentDataPath + "/StreamingAssets";
            XmlDocument sXmlDoc = new XmlDocument();
            XmlElement SXmlRoot;
            MyDebug.Log("本地资源配置不存在");
            SXmlRoot = sXmlDoc.CreateElement("Files");
            sXmlDoc.AppendChild(SXmlRoot);
            foreach (KeyValuePair<string, List<string>> pair in DicMD5)
            {
                XmlElement xmlElem = sXmlDoc.CreateElement("File");
                SXmlRoot.AppendChild(xmlElem);
                xmlElem.SetAttribute("FileName", pair.Key);
                xmlElem.SetAttribute("MD5", pair.Value[0]);
                xmlElem.SetAttribute("Size", pair.Value[1]);
            }
            sXmlDoc.Save(savePath + "/VersionMD5.xml");
            sXmlDoc = null;
            isDone = true;
            if (Directory.Exists(Application.persistentDataPath + "/Zip"))
            {
                MyDebug.Log("删除本地资源文件夹");
                Directory.Delete(Application.persistentDataPath + "/Zip", true);

            }
            DoneLoaded();
        }
        else
        {
            string file = DicMD5UpdateName[0];
            DicMD5UpdateName.RemoveAt(0);
            //  Debug.Log("Decompress:" + file);
            DecompressFileLZMA(Application.persistentDataPath + "/Zip/" + file, Application.persistentDataPath + "/" + file);
        }


    }
    SevenZip.Compression.LZMA.Decoder coder;
    private void DecompressFileLZMA(string inFile, string outFile)
    {
        coder = new SevenZip.Compression.LZMA.Decoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile.Replace(".zip", ""), FileMode.Create);

        // Read the decoder properties
        byte[] properties = new byte[5];
        input.Read(properties, 0, 5);

        // Read in the decompress file size.
        byte[] fileLengthBytes = new byte[8];
        input.Read(fileLengthBytes, 0, 8);
        long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

        // Decompress the file.
        coder.SetDecoderProperties(properties);
        coder.Code(input, output, input.Length, fileLength, null);
        output.Flush();
        output.Close();
        input.Flush();
        input.Close();
        StartCoroutine(UnPreccFiles());
        //DownLoadRes();
    }
    //IEnumerator DownLoadFile(string file)

    //{

    //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(AssetBundleConfig.ASSETBUNDLE_PATH_WEB + file);

    //    request.Method = "GET";

    //    HttpWebResponse hw = (HttpWebResponse)request.GetResponse();

    //    Stream stream = hw.GetResponseStream();

    //    FileStream fileStream = new FileStream(Application.persistentDataPath + "/Zip/" + file, FileMode.Create);

    //    long length = hw.ContentLength;

    //    currentNum = 0;

    //    decimal currentProgress = 0;



    //    while (currentNum < length)

    //    {

    //        byte[] buffer = new byte[1024];

    //        currentNum += stream.Read(buffer, 0, buffer.Length);

    //        fileStream.Write(buffer, 0, buffer.Length);

    //        if (currentNum % 1024 == 0)

    //        {

    //            currentProgress = Math.Round(Convert.ToDecimal(Convert.ToDouble(currentNum) / Convert.ToDouble(length) * 100), 4);

    //            //  Debug.Log("当前下载文件大小:" + length/1024.0f+ "字节   当前下载大小:" + currentNum / 1024.0f + "字节 下载进度" + currentProgress.ToString() + "%");

    //        }

    //        else

    //        {

    //            //Debug.Log("当前下载文件大小:" + length / 1024.0f + "字节   当前下载大小:" + currentNum/1024.0f + "字节" + "字节 下载进度" + 100 + "%");

    //        }

    //        // currentnn = currentProgress;

    //        yield return false;

    //    }
    //    currentNumShow += length;

    //    hw.Close();

    //    stream.Close();

    //    fileStream.Close();
    //    currentNum = 0;
    //    DownLoadRes();
    //    //DecompressFileLZMA(Application.persistentDataPath + "/Cahe.zip", Application.persistentDataPath + "/" + file);

    //}

    //  decimal currentNumShow;
    long currentNumShow;
    float currentNum = 0;
    GUIStyle guistyle = new GUIStyle();
    string LoadPro;
    void Update()
    {

        if (isDisConnect)
        {
            UIPanel_SceneLoading.instance.tips.text = "无网络访问，请检查网络连接！";
            return;
        }
        if(isCompare)
        {
            UIPanel_SceneLoading.percentV = currentNum*100;
            UIPanel_SceneLoading.instance.tips.text = "正在校验版本号";
            return;
        }
        if (DicMD5UpdateName.Count <= 0)
        {                                                                                          
            return;
        }     
        //if (updateSize <= 0)
        //    return;
        guistyle.fontSize = 80;
        UIPanel_SceneLoading.percentV = (currentNumShow + currentNum) * 100.0f / updateSize;
        // // GUI.Label(new Rect(50, 50, 50, 50), (currentNumShow + currentNum) + "/" + updateSize, guistyle);
        //  GUI.Label(new Rect(50, 150, 50, 50), UIPanel_SceneLoading.percentV.ToString("f2") + "%", guistyle);
        UIPanel_SceneLoading.instance.tips.text = "正在下载资源文件（" + ((currentNumShow + currentNum) / 1024.0f / 1024).ToString("f2") + "M/" + (updateSize / 1024.0f / 1024).ToString("f2") + "M)";
        //  GUI.Label(new Rect(50, 50, 50, 50), LoadPro, guistyle);
        //  GUI.Label(new Rect(50, 150, 50, 50), updateCount + "/" + DicMD5UpdateName.Count, guistyle);
    }
}
