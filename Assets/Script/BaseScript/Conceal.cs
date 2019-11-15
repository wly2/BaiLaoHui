using UnityEngine;

public class Conceal : MonoBehaviour
{
    void Start()
    {
        Invoke("Hide", 3);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}