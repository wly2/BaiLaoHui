using UnityEngine;
using UnityEngine.UI;

public class yuSetScript : MonoBehaviour
{

    public Text numberText;

    public void SetCount(int count)
    {
        numberText.text = "X" + count;
    }
}