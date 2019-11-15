using UnityEngine;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    private string time;

    void Update()
    {
        time = System.DateTime.Now.ToString("HH : mm");
        GetComponent<Text>().text = time;
    }
}