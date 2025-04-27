using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitMenu : MonoBehaviour
{
    public static QuitMenu Instance;
    CanvasGroup cg;
    bool menuEnabled = false;
    [SerializeField] Button confirmButton, cancelButton;

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
        cancelButton.onClick.AddListener(delegate () { DisableMenu(); }); 
        confirmButton.onClick.AddListener(delegate (){
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        });

    }
    public void EnableMenu()
    {
        if (menuEnabled) return;
        Debug.Log("EnablingMenu");
        menuEnabled = true;
        cg.alpha = 1;
        cg.interactable = true;
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
        Time.timeScale = 1;
        PlayersManager.Instance.ShowAllHuds();
        CameraFX.Instance.timer.gameObject.SetActive(true);
    }
}
