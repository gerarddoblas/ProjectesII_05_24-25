using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Colourpicker : MonoBehaviour
{
    [SerializeField] GameObject container;
    [SerializeField] private Color[] playerColours;
    private bool[] pickedColours = new bool[4];
    private CanvasGroup cg;
    GameObject referencedPlayer, referencedHud;
    private void Awake()
    {
        cg = this.GetComponent<CanvasGroup>();
        for(int i = 0; i <4; i++)
        {
            container.transform.GetChild(i).GetComponent<Image>().color = playerColours[i];
            pickedColours[i] = false;
        }
    }
    public void UnSelectColour(Color c){
        for (int i = 0; i < 4; i++)
        {
            if (c == playerColours[i])
            {
                pickedColours[i] = false;
                container.transform.GetChild(i).GetComponent<Image>().color = c;
            }
        }
    }
    void SetColourPicked(int index)
    {
        if (!pickedColours[index])
        {
            pickedColours[index] = true;
            referencedPlayer.GetComponent<SpriteRenderer>().color = playerColours[index];
            referencedHud.GetComponent<PlayerHud>().SetColour(playerColours[index]);
            referencedPlayer.GetComponent<PlayerInput>().actions.FindAction("Move").started -= selectColour;
            container.transform.GetChild(index).LeanScale(Vector3.one * 1.1f, .5f).setOnComplete(() => {
                Color c = container.transform.GetChild(index).GetComponent<Image>().color;
                c.a /= 2;
                container.transform.GetChild(index).GetComponent<Image>().color = c;
                container.transform.GetChild(index).localScale = Vector3.one;
                Hide(); 
            });
        }
    }
    public void Show(GameObject playerToSetColour, GameObject hudToSetColour) {
        PlayersManager.Instance.SetJoining(false);
        LeanTween.value(0, 1, .5f).setOnUpdate((float r) => { cg.alpha = r; }); 
        referencedPlayer = playerToSetColour;
        referencedHud = hudToSetColour;
        referencedPlayer.GetComponent<PlayerInput> ().actions.FindAction("Move").started +=  selectColour;
    }

    void selectColour(InputAction.CallbackContext context)
    {
        Debug.LogWarning("TryingToSelectColour");
        Vector2 selected = context.ReadValue<Vector2>();
        if (selected == Vector2.up)
            SetColourPicked(0);
        else if (selected == Vector2.right)
            SetColourPicked(1);
        else if (selected == Vector2.down)
            SetColourPicked(2);
        else if (selected == Vector2.left)
            SetColourPicked(3);
    }
    public void Hide() {
        
        LeanTween.value(1, 0, .5f).setOnUpdate((float r) => { cg.alpha = r; }).setOnComplete(() => {
            PlayersManager.Instance.SetJoining(true);
            referencedPlayer.GetComponent<SpriteRenderer>().enabled = true;
            referencedPlayer.GetComponent<Rigidbody2D>().simulated = true;
            referencedPlayer = null;
            referencedHud = null;
        });
    }
}
