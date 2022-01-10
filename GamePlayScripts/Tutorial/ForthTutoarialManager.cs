using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ForthTutoarialManager : MonoBehaviour
{
    [SerializeField] GameObject firstMessage;
    [SerializeField] GameObject secondMessage;
    [SerializeField] GameObject thirdMessage;
    [SerializeField] GameObject ForthdMessage;
    [SerializeField] Image painter;


    bool canGoThroughTutorial = false;
    void OnEnable()
    {

        StartCoroutine(NoAming());

        if (!painter.gameObject.activeSelf)
            painter.gameObject.SetActive(true);
        if(!firstMessage.activeSelf)
            firstMessage.SetActive(true);
        if(secondMessage.activeSelf)
            secondMessage.SetActive(false);
        if(thirdMessage.activeSelf)
            thirdMessage.SetActive(false);
        if (ForthdMessage.activeSelf)
            ForthdMessage.SetActive(false);

        StartCoroutine(WaitWithTutorial());
    }

    void Update()
    {
        if (canGoThroughTutorial)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (firstMessage.activeSelf)
                {
                    FirstDeactivateSecondActivate();
                }else if (secondMessage.activeSelf)
                {
                    SecondDeactivateThirdActivate();
                }else if (thirdMessage.activeSelf)
                {
                    ThirdDeactivateForthActivate();
                }else if (ForthdMessage.activeSelf)
                {
                    DeactivateAll();
                }
            }
        }
    }

    public void SkipTutorial()
    {
        StartCoroutine(CanAim());
    }

    public void FirstDeactivateSecondActivate()
    {
        firstMessage.SetActive(false);
        secondMessage.SetActive(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void SecondDeactivateThirdActivate()
    {
        secondMessage.SetActive(false);
        thirdMessage.SetActive(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void ThirdDeactivateForthActivate()
    {
        thirdMessage.SetActive(false);
        ForthdMessage.SetActive(true);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    public void DeactivateAll()
    {
        ForthdMessage.SetActive(false);
        painter.gameObject.SetActive(false);
        StartCoroutine(CanAim());
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
    }

    IEnumerator WaitWithTutorial()
    {
        yield return new WaitForSeconds(2f);
        canGoThroughTutorial = true;
    }

    IEnumerator NoAming()
    {
        yield return new WaitForSeconds(1.72f);
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
