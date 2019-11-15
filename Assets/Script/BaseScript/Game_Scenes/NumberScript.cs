using System;
using UnityEngine;
using UnityEngine.UI;

public class NumberScript : MonoBehaviour
{
    public double Lasttime;
    private float timer;
    private int Direction;

    void Update()
    {
        timer += Time.deltaTime;
        Lasttime = 10 - timer;
        Lasttime = Math.Floor(Lasttime);
        GetComponent<Text>().text = Lasttime.ToString();
    }

    public void Number()
    {
    }
}