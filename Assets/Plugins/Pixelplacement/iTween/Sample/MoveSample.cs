using UnityEngine;

public class MoveSample : MonoBehaviour
{
    public iTween.EaseType mtype;

    void Start()
    {
        //Invoke("test",2);
        //iTween.MoveBy(gameObject, iTween.Hash("y", 2, "easeType", "easeInOutExpo", "loopType", "none", "delay", .1));
    }

    void Test()
    {
        iTween.MoveTo(gameObject, Vector3.zero, 1f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            iTween.MoveBy(gameObject, iTween.Hash("y", 2, "easeType", mtype, "loopType", "none", "delay", 0.1));
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            iTween.MoveBy(gameObject, iTween.Hash("y", -2, "easeType", mtype, "loopType", "none", "delay", 0.1));
        }
    }
}