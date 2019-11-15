using UnityEngine;
using System.Collections;
using XLua;

[LuaCallCSharp]
public class LuaHelper
{
    public static UIViewUtil ViewUtil
    {
        get { return UIViewUtil.Instance; }
    }

    public static ResourcesMgr ResourcesMgr
    {
        get { return ResourcesMgr.Instance; }
    }

    public static DataResource DataResourMgr
    {
        get { return DataResource.Instance; }
    }
    public static CanvasGroup GetCanvasGroup(Transform trans)
    {
        return trans.GetComponent<CanvasGroup>();
    }
   
    


    
}