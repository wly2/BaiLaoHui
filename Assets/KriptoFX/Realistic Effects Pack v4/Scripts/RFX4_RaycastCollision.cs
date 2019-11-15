using UnityEngine;
using System.Collections.Generic;

public class RFX4_RaycastCollision : MonoBehaviour
{
    public float RaycastDistance = 100;
    public GameObject[] Effects;
    public float Offset;
    public float TimeDelay;
    public float DestroyTime = 3;
    public bool UsePivotPosition;
    public bool UseNormalRotation = true;
    public bool IsWorldSpace = true;
    public bool RealTimeUpdateRaycast;
    public bool DestroyAfterDisabling;
    [HideInInspector] public float HUE = -1;
    [HideInInspector] public List<GameObject> CollidedInstances = new List<GameObject>();

    private bool isInitialized;
    private bool canUpdate;

    void Start()
    {
        isInitialized = true;
        if (TimeDelay < 0.001f) UpdateRaycast();
        else Invoke("LateEnable", TimeDelay);
    }

    // Use this for initialization
    void OnEnable()
    {
        CollidedInstances.Clear();
        if (isInitialized)
        {
            if (TimeDelay < 0.001f)
            {
                UpdateRaycast();

            }
            else Invoke("LateEnable", TimeDelay);
        }
    }

    void OnDisable()
    {
        if (DestroyAfterDisabling)
        {
            for (int i = 0; i < CollidedInstances.Count; ++i)
            {
                Destroy(CollidedInstances[i]);
            }
        }
    }

    void Update()
    {

        if (canUpdate)
        {
            UpdateRaycast();
        }
    }

    void LateEnable()
    {
        UpdateRaycast();
    }



    private void UpdateRaycast()
    {

        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, RaycastDistance))
        {
            Vector3 position;
            position = UsePivotPosition ? raycastHit.transform.position : raycastHit.point + raycastHit.normal * Offset;

            if (CollidedInstances.Count == 0)
                for (int i = 0; i < Effects.Length; ++i)
                {
                    var instance = Instantiate(Effects[i], position, new Quaternion()) as GameObject;
                    CollidedInstances.Add(instance);
                    if (HUE > -0.9f)
                    {
                        RFX4_ColorHelper.ChangeObjectColorByHUE(instance, HUE);
                    }

                    if (!IsWorldSpace)
                        instance.transform.parent = transform;
                    if (UseNormalRotation)
                        instance.transform.LookAt(raycastHit.point + raycastHit.normal);
                    if (DestroyTime > 0.0001f)
                        Destroy(instance, DestroyTime);
                }
            else
                for (int i = 0; i < CollidedInstances.Count; ++i)
                {
                    if (CollidedInstances[i] == null) continue;
                    CollidedInstances[i].transform.position = position;
                    if (UseNormalRotation)
                        CollidedInstances[i].transform.LookAt(raycastHit.point + raycastHit.normal);
                }
        }

        if (RealTimeUpdateRaycast)
            canUpdate = true;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * RaycastDistance);
    }
}