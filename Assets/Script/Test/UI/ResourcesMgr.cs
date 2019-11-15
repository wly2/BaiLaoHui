//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2015-11-29 15:32:54
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using System.Text;

public class ResourcesMgr: Singleton<ResourcesMgr>
{
    #region ResourceType 资源类型
    /// <summary>
    /// 资源类型
    /// </summary>
    public enum ResourceType
    {
        /// <summary>
        /// 任意
        /// </summary>
        Normal,
        /// <summary>
        /// 场景UI
        /// </summary>
        UIScene,
        /// <summary>
        /// 窗口
        /// </summary>
        UIWindow
    }
    #endregion

    /// <summary>
    /// 预设的列表
    /// </summary>
    private Hashtable m_PrefabTable;

    public ResourcesMgr()
    {
        m_PrefabTable = new Hashtable();
    }

    public GameObject Load(string path)
    {
        GameObject obj = Resources.Load(path) as GameObject;
        return Object.Instantiate(obj);
    }

    #region Load 加载资源
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="type">资源类型</param>
    /// <param name="path">短路径</param>
    /// <param name="cache">是否放入缓存</param>
    /// <returns>预设克隆体</returns>
    public GameObject Load(ResourceType type, string path, bool cache=false)
    {
        GameObject obj = null;
        if (m_PrefabTable.Contains(path))
        {
            obj = m_PrefabTable[path] as GameObject;
        }
        else
        {
            StringBuilder sbr = new StringBuilder();
            switch (type)
            {
                case ResourceType.UIScene:
                    sbr.Append("UIPrefab/UIScene/");
                    break;
                case ResourceType.UIWindow:
                    sbr.Append("UIPrefab/UIWindows/");
                    break;
            }

            sbr.Append(path);

            obj = Resources.Load(sbr.ToString()) as GameObject;
            if (cache)
            {
                m_PrefabTable.Add(path, obj);
            }
        }

        return Object.Instantiate(obj);
    }
    #endregion



    #region Dispose 释放资源
    /// <summary>
    /// 释放资源
    /// </summary>
    public override void Dispose()
    {
        base.Dispose();

        m_PrefabTable.Clear();
    }
    #endregion
}