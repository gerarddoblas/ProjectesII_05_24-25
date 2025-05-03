using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    PauseMenu Instance;
    CanvasGroup cg;
    bool menuEnabled = false;
    [SerializeField] Button resumeButton, optionsButton, exitButton;
    [SerializeField] OptionsMenu optionsMenu;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        cg = this.GetComponent<CanvasGroup>();
        DisableMenu();
        resumeButton.onClick.AddListener(delegate () { DisableMenu(); });
        optionsButton.onClick.AddListener(delegate () { EnterOptionsMenu(); });
        optionsMenu.Return.onClick.AddListener (delegate () { ExitOptionsMenu(); });    
        exitButton.onClick.AddListener(delegate (){
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuEnabled)
                EnableMenu();
            else 
                DisableMenu();
        }
    }
    public void EnableMenu()
    {
        Debug.Log("EnablingMenu");
        menuEnabled = true;
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
        Time.timeScale = 0;
        PlayersManager.Instance.HideAllHuds();
        CameraFX.Instance.timer.gameObject.SetActive(false);
    }
    public void DisableMenu()
    {
        Debug.Log("HidingMenu");
        menuEnabled = false;
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        Time.timeScale = 1;
        PlayersManager.Instance.ShowAllHuds();
        CameraFX.Instance.timer.gameObject.SetActive(true);
    }
    public void EnterOptionsMenu()
    {
        resumeButton.gameObject.SetActive(false); 
        optionsButton.gameObject.SetActive(false); 
        exitButton.gameObject.SetActive(false);
        optionsMenu.gameObject.GetComponent<CanvasGroup>().interactable = true;
        optionsMenu.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        optionsMenu.gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }
    public void ExitOptionsMenu()
    {
        resumeButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        optionsMenu.gameObject.GetComponent<CanvasGroup>().interactable = false;
        optionsMenu.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        optionsMenu.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }
}
