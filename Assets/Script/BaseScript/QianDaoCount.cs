using UnityEngine;
using UnityEngine.UI;

public class QianDaoCount : MonoBehaviour
{
    public static QianDaoCount _instance;
    public int count;

    private void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        this.gameObject.GetComponent<Text>().text = count.ToString();
    }
}