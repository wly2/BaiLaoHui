using UnityEngine;
using System.Collections;

public class UI_Rotation : MonoBehaviour
{
    public float spped;
    Transform m_myTra;

    void Start()
    {
        m_myTra = transform;
    }

    void Update()
    {
        m_myTra.Rotate(Vector3.forward, spped * Time.deltaTime);
    }
}