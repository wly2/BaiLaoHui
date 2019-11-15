using System;
using UnityEngine;
using System.Collections;
using XLua;
using XLuaFramework;

[LuaCallCSharp]
public class LuaBehaviour : MonoBehaviour
{
    [CSharpCallLua]
    public delegate void delLuaAwake(GameObject obj);
    LuaBehaviour.delLuaAwake luaAwake;

    [CSharpCallLua]
    public delegate void delLuaStart();
    LuaBehaviour.delLuaStart luaStart;

    [CSharpCallLua]
    public delegate void delLuaUpdate();
    LuaBehaviour.delLuaUpdate luaUpdate;

    [CSharpCallLua]
    public delegate void delLuaOnDestroy();
    LuaBehaviour.delLuaOnDestroy luaOnDestroy;

    private LuaTable scriptEnv;
    private LuaEnv luaEnv;

    public string Tag;

    void Awake()
    {
        luaEnv = LuaManager.luaEnv; //此处要从LuaManager上获取 全局只有一个

        scriptEnv = luaEnv.NewTable();

        LuaTable meta = luaEnv.NewTable();
        meta.Set("__index", luaEnv.Global);
        scriptEnv.SetMetaTable(meta);
        meta.Dispose();

        string prefabName = name;
        if (prefabName.Contains("(Clone)"))
        {
            prefabName = prefabName.Split(new string[] { "(Clone)" }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        prefabName = prefabName.Replace("pan_", "");

        luaAwake = scriptEnv.GetInPath<LuaBehaviour.delLuaAwake>(prefabName + ".awake");
        luaStart = scriptEnv.GetInPath<LuaBehaviour.delLuaStart>(prefabName + ".start");
        luaUpdate = scriptEnv.GetInPath<LuaBehaviour.delLuaUpdate>(prefabName + ".update");
        luaOnDestroy = scriptEnv.GetInPath<LuaBehaviour.delLuaOnDestroy>(prefabName + ".ondestroy");

        scriptEnv.Set("self", this);
        if (luaAwake != null)
        {
            luaAwake(gameObject);
        }
    }

    void Start()
    {
        if (luaStart != null)
        {
            luaStart();
        }
    }

    void Destroy()
    {
        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
    }
}