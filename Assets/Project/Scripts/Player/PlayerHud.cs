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
    public Image manaRadial, knockoutRadial;
    public Transform playerTransform;
    private void Update()
    {
        transform.position = playerTransform.position;

        switch(Mathf.Floor(manaRadial.fillAmount * 3))
        {
            case 0:
                manaRadial.color = Color.black;
                break;
            case 1:
                manaRadial.color = Color.green;
                break;
            case 2:
                manaRadial.color = Color.blue;
                break;
            case 3:
                manaRadial.color = Color.white;
                break;
            default:
                break;
        }

        knockoutRadial.color = (1 - knockoutRadial.fillAmount) * Color.white + knockoutRadial.fillAmount * Color.red;

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
