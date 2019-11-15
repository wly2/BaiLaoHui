using SevenZip.Compression.LZMA;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class AssetbundleHelper : MonoBehaviour
{
    public static Dictionary<string, List<string>> DicFileMD5 = new Dictionary<string, List<string>>();
    public static List<string> DicCompress = new List<string>();
    public static void CreatMD5XML()
    {
        DicFileMD5.Clear();
        string savePath = Application.dataPath + "/StreamingAssetsZip";
        SetAssetBundleNameAtPath("StreamingAssetsZip", "*.zip");
        if (Directory.Exists(savePath) == false)
            Directory.CreateDirectory(savePath);

        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlRoot = XmlDoc.CreateElement("Files");
        XmlDoc.AppendChild(XmlRoot);
        foreach (KeyValuePair<string, List<string>> pair in DicFileMD5)
        {
            XmlElement xmlElem = XmlDoc.CreateElement("File");
            XmlRoot.AppendChild(xmlElem);
            xmlElem.SetAttribute("FileName", pair.Key);
            // xmlElem.SetAttribute("FileName", Path.GetFileNameWithoutExtension(pair.Key));
            xmlElem.SetAttribute("MD5", pair.Value[0]);
            xmlElem.SetAttribute("Size", pair.Value[1]);
            // xmlElem.SetAttribute("Path", pair.Value[2]);
        }
        XmlDoc.Save(savePath + "/VersionMD5.xml");
        XmlDoc = null;
        AssetDatabase.Refresh();
    }
    public static void SetAssetBundleNameAtPath(string path, string searchPattern)
    {                                                                                             
        MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();
        string targetPath = null;
        List<string> files = new List<string>(Directory.GetFiles("Assets/" + path, searchPattern, SearchOption.AllDirectories));
        for (int i = 0; i < files.Count; ++i)
        {
            string fname = files[i].Replace("\\", "/");
            Debug.Log("fname:" + fname);
            if (fname.Contains(".meta") || fname.Contains("VersionMD5") || fname.Contains(".xml"))
                continue;

            FileStream file = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] hash = md5Generator.ComputeHash(file);
            List<string> tempList = new List<string>();
            string strMD5 = System.BitConverter.ToString(hash);
            string size = file.Length.ToString();
            file.Close();
            tempList.Add(strMD5);
            tempList.Add(size);

            targetPath = fname.Replace("Assets/StreamingAssetsZip", "StreamingAssets");     
            Debug.Log("filePath:" + targetPath);

            //string name = Path.GetFileName(targetPath);
            //Debug.Log("name:" + name);
            //Debug.Log(targetPath.Substring(0, targetPath.LastIndexOf(name)));
                                   

                   
            if (DicFileMD5.ContainsKey(targetPath) == false)
                DicFileMD5.Add(targetPath, tempList);
            else
                Debug.LogWarning("<Two File has the same name> name = " + targetPath);
        }
    }
    public static void Compressbundle()
    {
        DicCompress.Clear();
        SetCompressFileNo("StreamingAssets");
        SetCompressFile("StreamingAssets", "*.unity3d");
        SetCompressFile("StreamingAssets", "*.manifest");
        string savePath;
        for (int i = 0; i < DicCompress.Count; i++)
        {
            string path = DicCompress[i];                          
            string name = Path.GetFileName(path);
            Debug.Log("name:" + name);
            string targetPath = path.Replace("Assets/StreamingAssets/", "");
            targetPath = targetPath.Substring(0, targetPath.LastIndexOf(name));

            savePath = Path.Combine(Application.dataPath, "StreamingAssetsZip") + "/" + targetPath;
            Debug.Log("savePath:" + savePath);
            if (Directory.Exists(savePath) == false)
            {
                Directory.CreateDirectory(savePath);
            }
            if (!targetPath.EndsWith("/"))
            {
                targetPath += "/";
            }
            Debug.Log(path);
            if (path.Length != 0)
            {
                CompressFileLZMA(path, savePath + "/" + name + ".zip");
                AssetDatabase.Refresh();
            }
        }
        //CreatMD5XML();

    }
    public static void SetCompressFileNo(string path)
    {
        string dir = "Assets/"+ path;
        List<string> files = new List<string>(Directory.GetFiles(dir));
        for (int i = 0; i < files.Count; ++i)
        {
            string fname = files[i].Replace("\\", "/");
            if (fname.Contains(".meta") || fname.Contains("VersionMD5") || fname.Contains(".xml"))
                continue;

          //  Debug.Log("fname:" + fname);
            DicCompress.Add(fname);

        }
    }
    public static void SetCompressFile(string path, string searchPattern)
    {                                                                            
        List<string> files = new List<string>(Directory.GetFiles("Assets/" + path, searchPattern, SearchOption.AllDirectories));
        for (int i = 0; i < files.Count; ++i)
        {
            string fname = files[i].Replace("\\", "/");     
            if (fname.Contains(".meta") || fname.Contains("VersionMD5") || fname.Contains(".xml"))
                continue;  
           // Debug.Log("fname:" + fname);            
            DicCompress.Add(fname);

        }
    }
    // 使用LZMA算法压缩文件
    private static void CompressFileLZMA(string inFile, string outFile)
    {
        Encoder coder = new Encoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

        coder.WriteCoderProperties(output);

        byte[] data = BitConverter.GetBytes(input.Length);

        output.Write(data, 0, data.Length);

        coder.Code(input, output, input.Length, -1, null);
        output.Flush();
        output.Close();
        input.Close();
    }


    public static void DecompressFileLZMA(string inFile, string outFile)
    {
        SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
        FileStream input = new FileStream(inFile, FileMode.Open);
        FileStream output = new FileStream(outFile, FileMode.Create);

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
        input.Close();
        AssetDatabase.Refresh();
    }

}
