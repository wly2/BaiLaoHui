using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public class MyDebug
    {
        private static bool flag = false;
        private static bool Testflag = false;
        private static bool socketFlag = false;
        public static List<string> list = new List<string>();

        public MyDebug()
        {
        }

        public static void Log(object message)
        {
            if (flag)
            {
                Debug.Log(message);
                list.Add(message.ToString());
            }
        }

        public static void LogError(object message)
        {                             
            if (flag)
            {
                Debug.LogError(message);

            }
        }

        public static void LogWarning(object message)
        {
            if (flag)
            {
                Debug.LogWarning(message);

            }
        }

        public static void TestLog(object message)
        {
            if (Testflag)
            {
                Debug.Log("Test log --------------------"+message);
                list.Add(message.ToString());
            }

        }

        public static void SocketLog(object message)
        {
            if (socketFlag)
            {
                Debug.Log(message);
                list.Add(message.ToString());
            }
        }
    }
}