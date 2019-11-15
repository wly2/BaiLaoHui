public class EffectScriptSample : NcEffectBehaviour
{
#if UNITY_EDITOR
    public override string CheckProperty()
    {
        return ""; // no error
    }
#endif
}