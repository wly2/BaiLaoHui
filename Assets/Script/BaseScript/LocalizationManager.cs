using UnityEngine;
using System.Collections.Generic;

public class LocalizationManager
{
    //单例模式
    private static LocalizationManager _instance;

    public static LocalizationManager GetInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new LocalizationManager();
            }

            return _instance;
        }
    }

    //选择自已需要的本地语言
    public const string language = "Chinese";
    private Dictionary<string, string> dic = new Dictionary<string, string>();

    /// <summary>
    /// 读取配置文件，将文件信息保存到字典里
    /// </summary>
    public LocalizationManager()
    {
        TextAsset ta = Resources.Load<TextAsset>(language);
        string text = ta.text;

        string[] lines = text.Split("\r\n".ToCharArray());
        for (int i = 0; i < lines.Length; ++i)
        {
            if (string.IsNullOrEmpty(lines[i]))
                continue;
            string[] keyAndValue = lines[i].Split('=');
            //MyDebug.Log("==" + keyAndValue[0] + "==" + keyAndValue[1] + "==");
            dic.Add(keyAndValue[0], keyAndValue[1]);
        }
    }

    /// <summary>
    /// 获取value
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetValue(string key)
    {
        if (!dic.ContainsKey(key))
        {
            return null;
        }

        string value = null;
        dic.TryGetValue(key, out value);
        return value.Replace("\\n", "\n");
    }
}