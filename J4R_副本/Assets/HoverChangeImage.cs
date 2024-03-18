using UnityEngine;
using UnityEngine.EventSystems; // 引入事件系统命名空间
using UnityEngine.UI; // 引入UI命名空间

public class HoverChangeImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage; // 目标Image组件
    public Sprite hoverSprite; // 鼠标悬停时的Sprite
    private Sprite originalSprite; // 原始Sprite

    private void Start()
    {
        if (targetImage != null)
        {
            originalSprite = targetImage.sprite; // 在开始时保存原始Sprite
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.sprite = hoverSprite; // 鼠标悬停时切换Sprite
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.sprite = originalSprite; // 鼠标离开时恢复原始Sprite
        }
    }
}
