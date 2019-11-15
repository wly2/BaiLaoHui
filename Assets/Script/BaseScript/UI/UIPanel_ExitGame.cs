using AssemblyCSharp;
using UnityEngine;

public class UIPanel_ExitGame : UIWindow
{
    public void Exit()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        SoundManager.Instance.SetSoundV(PlayerPrefs.GetFloat("SoundVolume", 1));
        CustomSocket.Instance.SendMsg(new LoginRequest());
        //多态  调用退出登录接口
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}