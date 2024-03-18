using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BulletSlotsDisplay : MonoBehaviour
{
    public AmmoManager ammoManager;
    public Image[] bulletSlots; // ��Inspector������
    public Sprite liveBulletSprite; // ʵ��ͼƬ������Inspector������
    public Sprite blankBulletSprite; // �հ���ͼƬ������Inspector������


    public GameObject lunpan;

    void Start()
    {
        UpdateDisplay();
    }


    public void UpdateDisplay()
    {
        lunpan.SetActive(false);
        for (int i = 0; i < bulletSlots.Length; i++)
        {
            if (i < ammoManager.bulletCount)
            {
                bulletSlots[i].gameObject.SetActive(true);
                bulletSlots[i].sprite = ammoManager.bulletTypes[i] ? liveBulletSprite : blankBulletSprite;
            }
            else
            {
                bulletSlots[i].gameObject.SetActive(false);
            }
        }

        StartCoroutine(jieshu());
    }

    IEnumerator jieshu()
    {
        
        yield return new WaitForSeconds(2f);
        lunpan.SetActive(true);
        for (int i = 0; i < bulletSlots.Length; i++)
        {
            if (i < ammoManager.bulletCount)
            {
                bulletSlots[i].gameObject.SetActive(false);
                bulletSlots[i].sprite = ammoManager.bulletTypes[i] ? liveBulletSprite : blankBulletSprite;
            }
            else
            {
                bulletSlots[i].gameObject.SetActive(false);
            }
        }
    }

}
