using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] GameObject keyboardControls, gamePadControls;
    [SerializeField] Image itemSprite, healthBar;
    [SerializeField] TextMeshProUGUI scoreText;

    private List<Coroutine> scoreAnimations = new List<Coroutine>();
    private List<Coroutine> itemAnimations = new List<Coroutine>();
    
    public void SetColour(Color colour)
    {
        itemSprite.transform.parent.GetComponent<Image>().color = colour;
        //itemSprite.transform.parent.GetChild(0).GetComponent<Image>().color = colour;
    }
    public void SetScoreText(int score)
    {
        scoreText.text = "Score: " + score;
        foreach (var animation in scoreAnimations)
            StopCoroutine(animation);
        scoreAnimations.Clear();
        if(this.gameObject.activeSelf) scoreAnimations.Add(StartCoroutine(ScoreAnimation()));
    }

    private IEnumerator ScoreAnimation()
    {
        scoreText.color = Color.yellow;
        while(scoreText.fontSize < 20)
        {
            scoreText.fontSize += 50 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while(scoreText.fontSize > 12)
        {
            scoreText.fontSize -= 50 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        scoreText.fontSize = 12;
        scoreText.color = Color.white;
    }
    public void SetHealthbar(float currentHealth, float maxHealth) { healthBar.fillAmount = currentHealth/ maxHealth; }
    public void SetItemSprite(Sprite newItemSprite){ 
        itemSprite.color = new Color(255,255,255,1);
        itemSprite.sprite = newItemSprite;
        foreach (var animation in itemAnimations)
            StopCoroutine(animation);
        itemAnimations.Clear();
        if (this.gameObject.activeSelf) itemAnimations.Add(StartCoroutine(ItemAnimation()));
    }

    private IEnumerator ItemAnimation()
    {
        RectTransform rect = itemSprite.gameObject.GetComponent<RectTransform>();
        while (rect.localScale.x < 2.5)
        {
            rect.localScale += 10 * Time.deltaTime * new Vector3(1, 1, 1);
            yield return new WaitForEndOfFrame();
        }
        while (rect.localScale.x > 1)
        {
            rect.localScale -= 50 * Time.deltaTime * new Vector3(1, 1, 1);
            yield return new WaitForEndOfFrame();
        }
        rect.localScale = Vector3.one;
    }

    public void ClearItemSprite() {
        itemSprite.color = new Color(255, 255, 255, 0);
        itemSprite.sprite = null; 
    }
    public void HideControls() {
        keyboardControls.SetActive(false);
        gamePadControls.SetActive(false);
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
