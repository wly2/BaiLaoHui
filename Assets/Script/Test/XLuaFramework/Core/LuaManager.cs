using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XLua;

namespace XLuaFramework
{
    public class LuaManager : MonoBehaviour
    {
        /// <summary>
        /// 全局的xLua引擎
        /// </summary>
        public static LuaEnv luaEnv;

        public static LuaManager Instance;
       
        void Awake()
        {
            Instance = this;
            luaEnv = new LuaEnv();

            //这里相当于初始化路径 也就是 Application.dataPath 文件夹下 .lua的文件都会被初始化加载
            luaEnv.DoString(string.Format("package.path = '{0}/?.lua'", Application.dataPath));
        }

        // Use this for initialization
        void Start()
        {
          

        }

        /// <summary>
        /// 执行lua脚本
        /// </summary>
        /// <param name="str"></param>
        public void DoString(string str)
        {
            luaEnv.DoString(str);
        }

        // Update is called once per frame
        void Update()
        {
            //luaEnv.GC(); //时刻回收
        }

        void OnDestroy()
        {
            //luaEnv.Dispose(); //释放
        }
    }
}