using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public Slider manaSlider, knockoutSlider;
    public TextMeshProUGUI scoreText;
    public Image smallImage, midImage, bigImage, backgroundImage;
    [SerializeField]private GameObject keyboardControls, gamePadControls;
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
