using System;
using UnityEngine;
using System.Collections.Generic;

public class UnityGuiConsole : MonoBehaviour
{
    public static UnityGuiConsole Instance { get; private set; }

    private static readonly int MAX_LOG = 250;
    private static readonly int WND_ID = 0x1435;
    private static readonly float EDGE_X = 16, EDGE_Y = 8;

    public bool Visible;

    private readonly string[] logTypeNames_;
    private readonly Queue<string>[] logList_;
    private readonly Vector2[] scrollPos_;

    private UnityGuiConsole()
    {
        logTypeNames_ = Enum.GetNames(typeof(LogType));
        logList_ = new Queue<string>[logTypeNames_.Length];
        scrollPos_ = new Vector2[logTypeNames_.Length];
        for (int i = 0; i < logList_.Length; ++i)
        {
            logList_[i] = new Queue<string>(MAX_LOG);
            scrollPos_[i] = new Vector2(0, 1);
        }
    }

    void Start()
    {
        Instance = this;
        Application.RegisterLogCallback(LogCallback);
        DontDestroyOnLoad(gameObject);
    }

    private float CoolDown_;

    void Update()
    {
        if ((Input.touches.Length >= 3 || (Input.GetMouseButton(0) && Input.GetMouseButton(1)))
            && Time.time - CoolDown_ > 2.0f)
        {
            Visible = !Visible;
            CoolDown_ = Time.time;
        }
    }

    private int logTypeChoose_ = (int) LogType.Log;
    private Rect rcWindow_;

    void OnGUI()
    {
        if (!Visible)
        {
            return;
        }

        EventType et = Event.current.type;
        if (et == EventType.Repaint || et == EventType.Layout)
        {
            rcWindow_ = new Rect(EDGE_X, EDGE_Y + 15f, Screen.width - EDGE_X * 2, Screen.height - EDGE_Y * 2);
            GUI.Window(WND_ID, rcWindow_, WindowFunc, string.Empty);
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 25f, 0f, 50f, 20f), "清屏"))
        {
            for (int i = 0; i < logList_.Length; ++i)
            {
                logList_[i].Clear();
            }
        }
    }

    void WindowFunc(int id)
    {
        try
        {
            GUILayout.BeginVertical();
            try
            {
                logTypeChoose_ = GUILayout.Toolbar(logTypeChoose_, logTypeNames_);
                var queue = logList_[logTypeChoose_];
                if (queue.Count > 0)
                {
                    scrollPos_[logTypeChoose_] = GUILayout.BeginScrollView(scrollPos_[logTypeChoose_]);
                    try
                    {
                        foreach (var s in queue)
                        {
                            GUILayout.Label(s);
                        }
                    }
                    finally
                    {
                        GUILayout.EndScrollView();
                    }
                }
            }
            finally
            {
                GUILayout.EndVertical();
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    static void Enqueue(Queue<string> queue, string text, string stackTrace)
    {
        while (queue.Count >= MAX_LOG)
        {
            queue.Dequeue();
        }

        queue.Enqueue(text);
        if (!string.IsNullOrEmpty(stackTrace))
        {
            queue.Enqueue(stackTrace);
        }
    }

    void LogCallback(string condition, string stackTrace, LogType type)
    {
        int index = (int) type;
        var queue = logList_[index];
        switch (type)
        {
            case LogType.Exception:
            case LogType.Error:
            case LogType.Warning:
                Enqueue(queue, condition, stackTrace);
                break;
            default:
                Enqueue(queue, condition, null);
                break;
        }

        scrollPos_[index] = new Vector2(0, 100000f);
    }
}