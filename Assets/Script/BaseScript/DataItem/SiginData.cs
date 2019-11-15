using System.Collections.Generic;

public class SiginData
{
    public string code;
    public string msg;
    public string time;
    public string apiurl;
    public string ApiHash;
    public string data;
    public int already_day;
    public bool is_today_sign;
    public Dictionary<string, SignAward> sign_config_list;
}

public class SignAward
{
    public string day;
    public string voucher;
    public string roomcard;
    public string admin_id;
    public string dateline;
    public string thumb;
}

public class SiginResult
{
    public string code;
    public string msg;
    public string time;
    public string apiurl;
    public string ApiHash;
    public string data;
}