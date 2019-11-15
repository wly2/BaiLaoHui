using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public delegate void DGameStateChangeEventHandler(EStateType newState, EStateType oldState);

public interface IGameSys
{
}

public class CGameRoot : MonoBehaviour
{
    public const string cRootName = "_GameRoot";

    //设置FPS最大值
    public int MaxFPSNum = 60;
    static public event DGameStateChangeEventHandler OnStateChange;
    static public event DGameStateChangeEventHandler OnPreStateChange;
    static public event DGameStateChangeEventHandler OnPostStateChange;
    static private CGameRoot _GameRoot;

    static public CGameRoot Instance
    {
        get
        {
            if (_GameRoot == null)
            {
                GameObject go = GameObject.Find(cRootName);
                if (go == null) return null;
                _GameRoot = go.GetComponent<CGameRoot>();
            }

            return _GameRoot;
        }
    }

    static public void SwitchToState(EStateType stateType)
    {
        if (stateType == EStateType.None)
            return;
        Instance._SwitchToState(stateType);
    }

    static public T GetGameSystem<T>()
        where T : CGameSystem
    {
        return Instance == null ? null : Instance._GetGameSystem<T>();
    }

    static public CGameSystem GetGameSystem(Type type)
    {
        return Instance == null ? null : Instance._GetGameSystem(type);
    }

    static public bool HaveSystemRegisted(Type type)
    {
        return Instance._HaveSystemRegisted(type);
    }

    static public EStateType PreState
    {
        get { return Instance != null ? Instance.mPreState : EStateType.None; }
    }

    static public EStateType CurState
    {
        get { return Instance != null ? Instance.mCurState : EStateType.None; }
    }

    static public EStateType NewState
    {
        get { return Instance != null ? Instance.mNewState : EStateType.None; }
    }

    static public bool IsInMatch
    {
        get
        {
            return (CurState == EStateType.PreMatch) ||
                   (CurState == EStateType.Match);
        }
    }

    static public EGameRootCfgType ConfigType
    {
        get { return Instance != null ? Instance.mConfigType : EGameRootCfgType.Game; }
    }

    public EGameRootCfgType mConfigType = EGameRootCfgType.Game;
    public EStateType mFirstStateName = EStateType.Root;
    private CGameRootCfg mConfig;
    private EStateType mCurState = EStateType.None;
    private EStateType mPreState = EStateType.None;
    private EStateType mNewState = EStateType.None;
    private List<CGameSystem> mLeaveSystems = new List<CGameSystem>();
    private List<CGameSystem> mEnterSystems = new List<CGameSystem>();
    private CGameSystem[] mSystems;
    private Dictionary<Type, CGameSystem> mSystemMap = new Dictionary<Type, CGameSystem>();

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        mConfig = CGameRootCfg.mCfgs[(int) mConfigType];
    }

    public void Start()
    {
        mSystems = mConfig.mInitialDelegate(transform);
        for (int i = 0; i < mSystems.Length; ++i)
        {
            mSystemMap.Add(mSystems[i].GetType(), mSystems[i]);
        }

        for (int i = 0; i < mSystems.Length; ++i)
        {
            mSystems[i].SysInitial();
        }

        if (mFirstStateName != EStateType.None)
            SwitchToState(mFirstStateName);
        else
            _SwitchToState(EStateType.Root);
    }

    public void OnDestroy()
    {
        if (mSystems != null)
        {
            for (int i = mSystems.Length - 1; i >= 0; --i)
            {
                mSystems[i].SysFinalize();
                DestroyImmediate(mSystems[i]);
            }
        }

        UnLoad();
        StopAllCoroutines();
        _GameRoot = null;
        mSystems = null;
        mConfig = null;
        mLeaveSystems.Clear();
        mEnterSystems.Clear();
        mSystemMap.Clear();
    }

    public void UnLoad()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }

    public void Update()
    {
        if (mSystems == null)
            return;
        for (int i = 0; i < mSystems.Length; ++i)
        {
            if (mSystems[i].SysEnabled)
            {
                mSystems[i].SysUpdate();
            }
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < mSystems.Length; ++i)
        {
            if (mSystems[i].SysEnabled)
            {
                mSystems[i].SysLateUpdate();
            }
        }
    }

    #region Switch To State

    private Queue<EStateType> switchQueue = new Queue<EStateType>();

    private void _SwitchToState(EStateType newState)
    {
        switchQueue.Enqueue(newState);
        if (!runing)
        {
            StartCoroutine(HandleSwitchQueue());
        }
    }

    bool runing;

    private IEnumerator HandleSwitchQueue()
    {
        runing = true;
        while (switchQueue.Count != 0)
        {
            yield return StartCoroutine(_SwitchToStateCo(switchQueue.Dequeue()));
        }

        runing = false;
    }

    private IEnumerator _SwitchToStateCo(EStateType newState)
    {
        //设置切换的新状态
        mNewState = newState;
        if (mCurState == newState)
        {
            yield break;
        }

        if (OnPreStateChange != null)
            OnPreStateChange(newState, mCurState);
        CGameState[] oldStates = mConfig.mStateMap[mCurState];
        CGameState[] newStates = mConfig.mStateMap[newState];
        int sameDepth = 0;
        while (sameDepth < newStates.Length && sameDepth < oldStates.Length
                                            && newStates[sameDepth] == oldStates[sameDepth])
        {
            ++sameDepth;
        }

        mLeaveSystems.Clear();
        mEnterSystems.Clear();
        for (int i = oldStates.Length - 1; i >= sameDepth; --i)
        {
            for (int j = 0; j < oldStates[i].mSystems.Length; ++j)
            {
                mLeaveSystems.Add(mSystemMap[oldStates[i].mSystems[j]]);
            }
        }

        for (int i = 0; i < mLeaveSystems.Count; ++i)
        {
            mLeaveSystems[i]._SysLeave();
        }

        if (OnStateChange != null)
            OnStateChange(newState, mCurState);
        for (int i = sameDepth; i < newStates.Length; ++i)
        {
            for (int j = 0; j < newStates[i].mSystems.Length; ++j)
            {
                if (!mSystemMap.ContainsKey(newStates[i].mSystems[j]))
                    throw new Exception(string.Format("SystemMap.ContainsKey({0}) == false",
                        newStates[i].mSystems[j].Name));
                mSystemMap[newStates[i].mSystems[j]].EnableInState = newStates[i].mStateType;
                mEnterSystems.Add(mSystemMap[newStates[i].mSystems[j]]);
            }
        }

        for (int i = 0; i < mEnterSystems.Count; ++i)
        {
            bool haveEnterCo = mEnterSystems[i]._SysEnter();
            if (haveEnterCo)
            {
                yield return StartCoroutine(mEnterSystems[i].SysEnterCo());
            }

            mEnterSystems[i]._EnterFinish();
        }

        //加入了新系统之后，再给旧系统一次清理的机会。
        for (int i = 0; i < mLeaveSystems.Count; ++i)
        {
            mLeaveSystems[i].SysLastLeave();
        }

        for (int i = 0; i < mEnterSystems.Count; ++i)
        {
            mEnterSystems[i].OnStateChangeFinish();
        }

        mPreState = mCurState;
        mCurState = newState;
        if (OnPostStateChange != null)
            OnPostStateChange(newState, mPreState);
    }

    #endregion

    private T _GetGameSystem<T>()
        where T : CGameSystem
    {
        return mSystemMap.ContainsKey(typeof(T)) ? (T) mSystemMap[typeof(T)] : null;
    }

    private CGameSystem _GetGameSystem(Type type)
    {
        return mSystemMap.ContainsKey(type) ? mSystemMap[type] : null;
    }

    private bool _HaveSystemRegisted(Type type)
    {
        return mSystemMap.ContainsKey(type);
    }
}

public delegate void DVoid();

public class CUsingHelper : IDisposable
{
    private DVoid mOnDispose;

    public CUsingHelper(DVoid onCreate, DVoid onDispose)
    {
        if (onCreate != null)
            onCreate();
        mOnDispose = onDispose;
    }

    public void Dispose()
    {
        if (mOnDispose != null)
            mOnDispose();
    }
}