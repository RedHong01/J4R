using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]

public class UiCanvasScaler : MonoBehaviour
{
    private CanvasScaler canvasScaler;

    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        AdjustScale();
    }

    void Update()
    {
        AdjustScale();
    }

    private void AdjustScale()
    {
        // 获取当前屏幕的宽高比
        float currentAspectRatio = (float)Screen.width / Screen.height;

        // 目标宽高比为16:9
        float targetAspectRatio = 16f / 9f;

        if (currentAspectRatio >= targetAspectRatio)
        {
            // 如果当前宽高比大于等于目标宽高比，使用Match Width
            canvasScaler.matchWidthOrHeight = 0;
        }
        else
        {
            // 如果当前宽高比小于目标宽高比，使用Match Height
            canvasScaler.matchWidthOrHeight = 1;
        }
    }
}
