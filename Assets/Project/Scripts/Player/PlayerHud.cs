using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] GameObject keyboardControls, gamePadControls;
    [SerializeField] Image itemSprite, healthBar;
    
    public void SetHealthbar(float currentHealth, float maxHealth) { healthBar.fillAmount = currentHealth/ maxHealth; }
    public void SetItemSprite(Sprite newItemSprite){ 
        itemSprite.color = new Color(255,255,255,1);
        itemSprite.sprite = newItemSprite; 
    }
    public void ClearItemSprite() {
        itemSprite.color = new Color(255, 255, 255, 0);
        itemSprite.sprite = null; 
    }
    public void SetKeyboardControls()
    {
        keyboardControls.SetActive(true);
        gamePadControls.SetActive(false);
    }
    public void SetGamepdControls()
    {
        keyboardControls.SetActive(false);
        gamePadControls.SetActive(true);
    }
}
