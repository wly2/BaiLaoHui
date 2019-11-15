using UnityEngine;

public class FxLifeTime : MonoBehaviour
{
    [SerializeField] float fxLiftTime;

    void Start()
    {
        Destroy(gameObject, fxLiftTime);
    }
}