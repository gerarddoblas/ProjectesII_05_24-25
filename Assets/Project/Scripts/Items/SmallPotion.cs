using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmallPotion : Item
{
    public float incrementMult = 1.5f;
    public float duration = 5;
    private GameObject target;
    Coroutine coroutine;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += delegate (Scene loadedScene, LoadSceneMode loadedSceneMode) {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
                target.transform.localScale = target.transform.localScale * incrementMult;
            }
        };
    }
    override public IEnumerator Effect(GameObject target)
    {
        this.target = target;
        coroutine = target.GetComponent<MonoBehaviour>().StartCoroutine(Grow());
        yield return null;
    }
    IEnumerator Grow()
    {
        if (target.transform.localScale == new Vector3(3.8f, 3.8f, 3.8f))
        {
            LeanTween.scale(target, target.transform.localScale / incrementMult, 1).setEaseInOutBounce();
            yield return new WaitForSeconds(duration);
            LeanTween.scale(target, target.transform.localScale * incrementMult, 1).setEaseInOutBounce();
            coroutine = null;
        }
        yield return null;
    }
}
