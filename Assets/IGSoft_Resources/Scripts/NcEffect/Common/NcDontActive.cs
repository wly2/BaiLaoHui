public class NcDontActive : NcEffectBehaviour
{
    void Awake()
    {
#if UNITY_EDITOR
        if (!IsCreatingEditObject())
#endif
        {
#if (!UNITY_3_5)
            Destroy(gameObject);
#else
			gameObject.active = false;
#endif
        }
    }

    void OnEnable()
    {
#if (!UNITY_3_5)
#else
		gameObject.active = false;
#endif
    }
}