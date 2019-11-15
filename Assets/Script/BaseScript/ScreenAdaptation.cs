using UnityEngine;
using UnityEngine.UI;

public class ScreenAdaptation : MonoBehaviour
{
    void Start()
    {
        const float standard_width = 1280f; //初始宽度
        const float standard_height = 720f; //初始高度
        var device_width = 0f; //当前设备宽度
        var device_height = 0f; //当前设备高度
        var adjustor = 0f; //屏幕矫正比例
        //获取设备宽高
        device_width = Screen.width;
        device_height = Screen.height;
        //计算宽高比例
        const float standard_aspect = standard_width / standard_height;
        var device_aspect = device_width / device_height;
        //计算矫正比例
        if (device_aspect < standard_aspect)
        {
            adjustor = standard_aspect / device_aspect;
        }

        adjustor = 0f;
        var canvasScalerTemp = transform.GetComponent<CanvasScaler>();
        if (adjustor == 0)
        {
            canvasScalerTemp.matchWidthOrHeight = 1;
        }
        else
        {
            canvasScalerTemp.matchWidthOrHeight = 0;
        }
    }
}