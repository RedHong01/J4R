using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class DynamicTextResizer : MonoBehaviour
{
    public Text textComponent;
    public float baseScreenHeight = 1080f; // 基准屏幕高度
    public int baseFontSize = 30; // 基准字体大小

    void Start()
    {
        if (textComponent == null)
        {
            textComponent = GetComponent<Text>();
        }

        AdjustFontSize();
    }

    void Update()
    {
        AdjustFontSize();
    }

    void AdjustFontSize()
    {
        // 计算当前屏幕高度与基准屏幕高度的比例
        float screenRatio = Screen.height / baseScreenHeight;
        // 根据屏幕比例调整字体大小
        textComponent.fontSize = Mathf.RoundToInt(baseFontSize * screenRatio);
    }
}
