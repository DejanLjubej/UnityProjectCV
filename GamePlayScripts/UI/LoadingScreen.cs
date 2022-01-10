using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public Animator loadingScreenAnimator;
    CanvasGroup canvasGroup;

    LoadingScreenTipsManager _loadinScreenTipsManager;

    void Start()
    {
        _loadinScreenTipsManager = GetComponentInChildren<LoadingScreenTipsManager>();
        loadingScreenAnimator = GetComponentInChildren<Animator>();
        StartCoroutine(OpenTheCurtains());
        canvasGroup = GetComponent<CanvasGroup>();
    }

    IEnumerator OpenTheCurtains()
    {
        yield return new WaitForSeconds(2.5f);
        canvasGroup.blocksRaycasts = false;
    }

    public void CLoseTheCurtain()
    {
        if(_loadinScreenTipsManager!= null)
        {
            _loadinScreenTipsManager.ChangeShownTip();
        }
        loadingScreenAnimator.SetTrigger("Close");
        canvasGroup.blocksRaycasts = true;
    }

}
