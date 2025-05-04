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
        Instance = this;

        cg = this.GetComponent<CanvasGroup>();
        DisableMenu();
        cancelButton.onClick.AddListener(delegate () { DisableMenu(); }); 
        confirmButton.onClick.AddListener(delegate () { Quit(); });
    }
    public void EnableMenu()
    {
        if (menuEnabled) return;
      
        PauseMenu.Instance.HideMenu();
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
        menuEnabled = false;
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        Time.timeScale = 1;
        PlayersManager.Instance.ShowAllHuds();
        CameraFX.Instance.timer.gameObject.SetActive(true);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
