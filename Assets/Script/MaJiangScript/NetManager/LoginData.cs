public class LoginData
{
    public const byte MB_VALIDATE_FLAGS = 0x01; //效验密保
    public const byte LOW_VER_VALIDATE_FLAGS = 0x02; //效验低版本
    public static WxUserInfo wxUserInfo;
    private static uint _plazaVersion = 101122049;

    public static uint PlazaVersion
    {
        get { return _plazaVersion; }
        set { _plazaVersion = value; }
    }

    private static uint _dwProcessVersion = 101056515;

    public static uint DwProcessVersion
    {
        get { return _dwProcessVersion; }
        set { _dwProcessVersion = value; }
    }
}