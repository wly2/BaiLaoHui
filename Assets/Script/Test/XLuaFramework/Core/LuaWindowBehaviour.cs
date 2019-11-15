/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XLua;
using System;
using UnityEngine.UI;
using XLuaFramework;
using DG.Tweening;
using AssemblyCSharp;

[System.Serializable]
public class Injection
{
    public string name;
    public GameObject value;
}

[LuaCallCSharp]
public class LuaWindowBehaviour : UIWindowViewBase
{
    [CSharpCallLua]
    public delegate void delLuaAwake(GameObject obj);
    delLuaAwake luaAwake;

    [CSharpCallLua]
    public delegate void delLuaStart();
    delLuaStart luaStart;

    [CSharpCallLua]
    public delegate void delLuaUpdate();
    delLuaUpdate luaUpdate;

    [CSharpCallLua]
    public delegate void delLuaOnDestroy();
    delLuaOnDestroy luaOnDestroy;

    private LuaTable scriptEnv;
    private LuaEnv luaEnv;
   

    protected override void OnAwake()
    {
        base.OnAwake();
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

        MyDebug.Log(prefabName);
        luaAwake = scriptEnv.GetInPath<delLuaAwake>(prefabName + ".awake");
        luaStart = scriptEnv.GetInPath<delLuaStart>(prefabName + ".start");
        luaUpdate = scriptEnv.GetInPath<delLuaUpdate>(prefabName + ".update");
        luaOnDestroy = scriptEnv.GetInPath<delLuaOnDestroy>(prefabName + ".ondestroy");

        scriptEnv.Set("self", this);
        if (luaAwake != null)
        {
            luaAwake(gameObject);

        }
    }

    protected override void OnStart()
    {
        base.OnStart();

        Debug.Log("c#的 Start");
        if (luaStart != null)
        {
            luaStart();
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        //备注 调用销毁的话，经常会造成Unity崩溃
        if (luaOnDestroy != null)
        {
            luaOnDestroy();
        }
        luaOnDestroy = null;
        luaUpdate = null;
        luaStart = null;
    }

    //解散房间弹出框
    public void ShowExitConfirmation()
    {
        UIManager.instance.Show(UIType.UIExitGame);
        SoundManager.Instance.PlaySoundBGM("clickbutton");
    }
}
