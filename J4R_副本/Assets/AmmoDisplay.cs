using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class AmmoDisplay : MonoBehaviour
{
    public AmmoManager ammoManager;
    public Sprite[] bulletCountSprites; // set the bullet count sprite in the inspector
    public  Image displayImage;

    void Start()
    {
     
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        // 
        if (ammoManager.bulletCount >= 1 && ammoManager.bulletCount <= bulletCountSprites.Length)
        {
            // 
            displayImage.sprite = bulletCountSprites[ammoManager.bulletCount - 1];
           
        }
    }
    
  


}
