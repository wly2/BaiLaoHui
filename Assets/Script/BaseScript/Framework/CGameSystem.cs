using UnityEngine;
using System.Collections;

public class CGameSystem : MonoBehaviour, IGameSys
{
    private string mName;

    public string Name
    {
        get
        {
            if (mName == null)
                mName = GetType().Name;
            return mName;
        }
    }

    public EStateType EnableInState
    {
        get { return mEnableInState; }
        set { mEnableInState = value; }
    }

    private EStateType mEnableInState;

    public virtual SEventData[] GetEventMap()
    {
        return new SEventData[] { };
    }

    #region Initial

    public virtual void SysInitial()
    {
        SEventData[] eventMap = GetEventMap();
        for (int i = 0; i < eventMap.Length; ++i)
        {
            CEventMgr.Instance.AddListener(eventMap[i].eventKey, eventMap[i].eventHandle, gameObject);
        }
    }

    #endregion

    #region Finalize

    public virtual void SysFinalize()
    {
        SEventData[] eventMap = GetEventMap();
        for (int i = 0; i < eventMap.Length; ++i)
        {
            CEventMgr.Instance.DetachListener(eventMap[i].eventKey, eventMap[i].eventHandle);
        }
    }

    #endregion

    #region Enter

    public bool SysEnabled
    {
        get { return mSysEnabled; }
    }

    protected bool mSysEnabled;

    public bool _SysEnter()
    {
        return SysEnter();
    }

    public virtual bool SysEnter()
    {
        return false;
    }

    public virtual IEnumerator SysEnterCo()
    {
        yield break;
    }

    public void _EnterFinish()
    {
        mSysEnabled = true;
        EnterFinish();
    }

    public virtual void EnterFinish()
    {
    }

    #endregion

    #region Leave

    public void _SysLeave()
    {
        mSysEnabled = false;
        EnableInState = EStateType.None;
        SysLeave();
    }

    public virtual void SysLeave()
    {
    }

    public virtual void SysLastLeave()
    {
    }

    #endregion

    public virtual void SysUpdate()
    {
    }

    public virtual void SysLateUpdate()
    {
    }

    public virtual void OnStateChangeFinish()
    {
    }
}

public struct SEventData
{
    public int eventKey;
    public DGameEventHandle eventHandle;

    public SEventData(int eventKey, DGameEventHandle eventHandle)
    {
        this.eventKey = eventKey;
        this.eventHandle = eventHandle;
    }
}