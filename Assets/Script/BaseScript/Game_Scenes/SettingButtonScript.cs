using UnityEngine;

public class SettingButtonScript : MonoBehaviour
{
    public void ShowSetting()
    {
        SoundManager.Instance.PlaySoundBGM("clickbutton");
        UIManager.instance.Show(UIType.UIGameRoomSetting);
    }
}