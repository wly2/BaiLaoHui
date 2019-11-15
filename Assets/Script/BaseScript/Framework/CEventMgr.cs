using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CEventMgr : CGameSystem
{
    private static CEventMgr _Instance;

    public static CEventMgr Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = CGameRoot.GetGameSystem<CEventMgr>();
            }

            return _Instance;
        }
    }

    class CEventData
    {
        public bool mObjRelate;
        public DGameEventHandle mHandle;
        public GameObject mGameObj;

        public CEventData(DGameEventHandle handle)
        {
            mObjRelate = false;
            mHandle = handle;
            mGameObj = null;
        }

        public CEventData(DGameEventHandle handle, GameObject gameObj)
        {
            mObjRelate = true;
            mHandle = handle;
            mGameObj = gameObj;
        }
    }

    public bool mLimitQueueProcesing;
    public float mQueueProcessTime;
    private Dictionary<int, List<CEventData>> mListenerTable = new Dictionary<int, List<CEventData>>();
    private Queue mEventQueue = new Queue();

    public override bool SysEnter()
    {
        return false;
    }

    public bool AddListener(int eventkey, DGameEventHandle eventHandle)
    {
        if (eventHandle == null)
        {
            return false;
        }

        if (!mListenerTable.ContainsKey(eventkey))
            mListenerTable.Add(eventkey, new List<CEventData>());
        List<CEventData> listenerList = mListenerTable[eventkey];
        listenerList.Add(new CEventData(eventHandle));
        return true;
    }

    /// <summary>
    /// 为避免回调时被调方是一个已经被删除的GameObj上的组件，造成异常，组件需要使用此函数注册
    /// </summary>
    /// <param name="eventkey"></param>
    /// <param name="eventHandle"></param>
    /// <param name="gameObj"></param>
    /// <returns></returns>
    public bool AddListener(int eventkey, DGameEventHandle eventHandle, GameObject gameObj)
    {
        if (eventHandle == null)
        {
            return false;
        }

        if (!mListenerTable.ContainsKey(eventkey))
            mListenerTable.Add(eventkey, new List<CEventData>());
        List<CEventData> listenerList = mListenerTable[eventkey];
        listenerList.Add(new CEventData(eventHandle, gameObj));
        return true;
    }

    public bool DetachListener(int eventKey, DGameEventHandle eventHandle)
    {
        if (!mListenerTable.ContainsKey(eventKey))
            return false;
        List<CEventData> listenerList = mListenerTable[eventKey];
        CEventData find = null;
        for (int i = 0; i < listenerList.Count; ++i)
        {
            if (listenerList[i].mHandle == eventHandle)
            {
                find = listenerList[i];
                break;
            }
        }

        if (find != null)
            listenerList.Remove(find);
        return true;
    }

    /// <summary>
    /// 同步事件触发
    /// </summary>
    /// <param name="evt"></param>
    /// <returns></returns>
    public void TriggerEvent(IEvent evt)
    {
        int eventKey = evt.GetKey();
        List<CEventData> listenerList = null;
        if (mListenerTable.TryGetValue(eventKey, out listenerList))
        {
            //防止在事件处理流程中改变事件列表
            List<CEventData> tmpList = new List<CEventData>(listenerList);
            for (int i = 0; i < tmpList.Count; ++i)
            {
                CEventData evtData = tmpList[i];
                if (evtData.mHandle != null && (!evtData.mObjRelate || evtData.mGameObj != null))
                {
#if PROFILE
//Profiler.BeginSample(string.Format("HandleEvt: {0}, {1}", eventKey, evtData.mGameObj != null ? evtData.mGameObj.name : "null"));
#endif
                    evtData.mHandle(evt);
#if PROFILE
//Profiler.EndSample();
#endif
                }
            }

            for (int i = 0; i < tmpList.Count; ++i)
            {
                if (tmpList[i].mObjRelate && tmpList[i].mGameObj == null)
                {
                    listenerList.Remove(tmpList[i]);
                }
            }
        }
    }

    public bool QueueEvent(IEvent evt)
    {
        mEventQueue.Enqueue(evt);
        return true;
    }

    public override void SysUpdate()
    {
        float timer = 0.0f;
        while (mEventQueue.Count > 0)
        {
            if (mLimitQueueProcesing)
            {
                if (timer > mQueueProcessTime)
                    return;
            }

            IEvent evt = mEventQueue.Dequeue() as IEvent;
            TriggerEvent(evt);
            if (mLimitQueueProcesing)
                timer += Time.deltaTime;
        }
    }

    public override void SysFinalize()
    {
        if (mListenerTable != null)
        {
            mListenerTable.Clear();
            mListenerTable = null;
        }

        if (mEventQueue != null)
        {
            mEventQueue.Clear();
            mEventQueue = null;
        }

        base.SysFinalize();
    }

    bool StartGame()
    {
        return true;
    }
}

public class CGameEvent : IEvent
{
    private int key;
    private object param1;
    private object param2;

    public CGameEvent(int k, object p1)
    {
        key = k;
        param1 = p1;
        param2 = null;
    }

    public CGameEvent(int k, object p1, object p2)
    {
        key = k;
        param1 = p1;
        param2 = p2;
    }

    public CGameEvent(int k)
    {
        key = k;
        param1 = null;
        param2 = null;
    }

    public int GetKey()
    {
        return key;
    }

    public object GetParam1()
    {
        return param1;
    }

    public object GetParam2()
    {
        return param2;
    }
}