using UnityEngine;
using UnityEngine.EventSystems; // �����¼�ϵͳ�����ռ�
using UnityEngine.UI; // ����UI�����ռ�

public class HoverChangeImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage; // Ŀ��Image���
    public Sprite hoverSprite; // �����ͣʱ��Sprite
    private Sprite originalSprite; // ԭʼSprite

    private void Start()
    {
        if (targetImage != null)
        {
            originalSprite = targetImage.sprite; // �ڿ�ʼʱ����ԭʼSprite
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.sprite = hoverSprite; // �����ͣʱ�л�Sprite
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.sprite = originalSprite; // ����뿪ʱ�ָ�ԭʼSprite
        }
    }
}
