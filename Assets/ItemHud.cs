using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemHud : MonoBehaviour
{
    public static ItemHud Instance { get; private set; }

    public GameObject[] objects = new GameObject[3];

    [SerializeField] private Image[] sprites;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < objects.Length; i++) if(objects[i] != null) sprites[i].sprite = objects[i].GetComponent<SpriteRenderer>().sprite;
    }
}
