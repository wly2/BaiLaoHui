using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class MyDelegate : MonoBehaviour
{
    public delegate void LogDelegate(string log);   //定义 委托名为LogDelegate,带一个string参数的 委托类型  

    public static LogDelegate LogEvent;             //声明委托对象,委托实例为LogEvent    

    public static void OnLogEvent(string log)       //可以直接 MyDelegate.LogEvent("")调用委托，这么写方便管理，还可以扩展这个方法;  
    {
        if (LogEvent != null)
        {
            LogEvent(log);
        }
    }

}
