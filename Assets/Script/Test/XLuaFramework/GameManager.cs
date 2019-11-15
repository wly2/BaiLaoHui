using UnityEngine;
using System.Collections;

namespace XLuaFramework
{
    /// <summary>
    /// xLua框架的主入口
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public int mode;
        void Awake()
        {
            //启动的时候 在自身挂上 LuaManager 脚本
            gameObject.AddComponent<LuaManager>();
        }

        // Use this for initialization
        void Start()
        {
            if (mode == 1)
            {
                UIManager.instance.Show(UIType.UINiuNiu);
            }
            //  LuaManager.Instance.DoString("require'XLuaLogic/Main'");
            else if (mode == 2)
                UIManager.instance.Show(UIType.UIShiSanShui);
            // LuaManager.Instance.DoString("require'XLuaLogic/ShiSanShuiMain'");

        }
    }
}