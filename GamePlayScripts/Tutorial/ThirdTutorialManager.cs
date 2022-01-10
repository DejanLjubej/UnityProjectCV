using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ThirdTutorialManager : MonoBehaviour
{
    [SerializeField] GameObject firstTask;
    [SerializeField] GameObject secondTask;
    [SerializeField] Image painter;

    bool canGoThroughTutorial = false;
    void OnEnable()
    {
        canGoThroughTutorial = false;
        StartCoroutine(MakeSureYouCanNotAim());
        firstTask.SetActive(true);
        if (secondTask.activeSelf)
        secondTask.SetActive(false);
        StartCoroutine(WaitWithTutorial());
    }

    void Update()
    {
        if (canGoThroughTutorial)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (firstTask.activeSelf)
                {
                    DeactivateFirstTutorial();
                }else if (secondTask.activeSelf)
                {
                    DeactivateSecondTutorialAndEnableShooting();
                }
                else
                {
                    StartCoroutine(CanAim());
                }
            }
        }
    }

    public void DeactivateFirstTutorial()
    {
        firstTask.SetActive(false);
        secondTask.SetActive(true);
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void DeactivateSecondTutorialAndEnableShooting()
    {
        secondTask.SetActive(false);
        painter.gameObject.SetActive(false);
        StartCoroutine(CanAim());
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void SkipTutorial()
    {
        BallContorller.canYouStartAiming = true;

        this.gameObject.SetActive(false);
    }

    IEnumerator WaitWithTutorial()
    {
        yield return new WaitForSeconds(2f);
        canGoThroughTutorial = true;
    }

    IEnumerator MakeSureYouCanNotAim()
    {
        yield return new WaitForSeconds(1.21f);
        BallContorller.canYouStartAiming = false;
        yield return new WaitForEndOfFrame();
        if (BallContorller.canYouStartAiming)
            BallContorller.canYouStartAiming = false;
        yield return new WaitForEndOfFrame();
        if (BallContorller.canYouStartAiming)
            BallContorller.canYouStartAiming = false;
    }

    IEnumerator CanAim()
    {
        yield return new WaitForSeconds(0.2f);
        BallContorller.canYouStartAiming = true;
        this.gameObject.SetActive(false);
    }
}
