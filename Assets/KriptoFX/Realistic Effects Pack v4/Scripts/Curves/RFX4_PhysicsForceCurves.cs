using UnityEngine;

public class RFX4_PhysicsForceCurves : MonoBehaviour
{

    public float ForceRadius = 5;
    public float ForceMultiplier = 1;
    public AnimationCurve ForceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public ForceMode ForceMode;
    public float GraphTimeMultiplier = 1, GraphIntensityMultiplier = 1;
    public bool IsLoop;
    public float DestoryDistance = -1;
    public bool UseDistanceScale;
    public AnimationCurve DistanceScaleCurve = AnimationCurve.EaseInOut(1, 1, 1, 1);
    public bool UseUPVector;
    public AnimationCurve DragCurve = AnimationCurve.EaseInOut(0, 0, 0, 1);
    public float DragGraphTimeMultiplier = -1, DragGraphIntensityMultiplier = -1;
    public string AffectedName;

    [HideInInspector] public float forceAdditionalMultiplier = 1;
    private bool canUpdate;
    private float startTime;
    private Transform t;

    private void Awake()
    {
        t = transform;
    }

    private void OnEnable()
    {
        startTime = Time.time;
        canUpdate = true;
        forceAdditionalMultiplier = 1;
    }

    private void FixedUpdate()
    {
        var time = Time.time - startTime;
        if (canUpdate)
        {
            float eval = ForceCurve.Evaluate(time / GraphTimeMultiplier) * GraphIntensityMultiplier;
            var hitColliders = Physics.OverlapSphere(t.position, ForceRadius);
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                var rig = hitColliders[i].GetComponent<Rigidbody>();
                if (rig == null) continue;
                if (AffectedName.Length > 0 && !hitColliders[i].name.Contains(AffectedName))
                {

                    continue;
                }

                Vector3 distVector;
                float dist;
                if (UseUPVector)
                {
                    distVector = Vector3.up;
                    var pos = hitColliders[i].transform.position;
                    dist = 1 - Mathf.Clamp01(pos.y - t.position.y);
                    dist *= 1 - ((hitColliders[i].transform.position - t.position)).magnitude / ForceRadius;
                }
                else
                {
                    distVector = (hitColliders[i].transform.position - t.position);
                    dist = 1 - distVector.magnitude / ForceRadius;
                }

                if (UseDistanceScale)
                    hitColliders[i].transform.localScale =
                        DistanceScaleCurve.Evaluate(dist) * hitColliders[i].transform.localScale;

                if (DestoryDistance > 0 && distVector.magnitude < DestoryDistance)
                {
                    Destroy(hitColliders[i].gameObject);
                }

                rig.AddForce(distVector.normalized * dist * ForceMultiplier * eval * forceAdditionalMultiplier,
                    ForceMode);
                if (DragGraphTimeMultiplier > 0)
                {
                    rig.drag = DragCurve.Evaluate(time / DragGraphTimeMultiplier) * DragGraphIntensityMultiplier;
                    rig.angularDrag = rig.drag / 10;
                }

            }
        }

        if (time >= GraphTimeMultiplier)
        {
            if (IsLoop) startTime = Time.time;
            else canUpdate = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ForceRadius);
    }
}