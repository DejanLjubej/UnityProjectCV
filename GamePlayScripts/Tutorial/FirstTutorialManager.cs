using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class FirstTutorialManager : MonoBehaviour
{
    [SerializeField] GameObject tutorialStepOne;
    [SerializeField] GameObject tutorialStepTwo;
    [SerializeField] GameObject tutorialStepThree;
    [SerializeField] Image painterImage;


    void Start()
    {
        StartCoroutine(NoAming());
        if (!painterImage.gameObject.activeSelf)
            painterImage.gameObject.SetActive(true);
        if (!tutorialStepOne.activeSelf)
            tutorialStepOne.SetActive(true);

        if (tutorialStepTwo.activeSelf)
            tutorialStepTwo.SetActive(false);

        if(tutorialStepThree.activeSelf)
            tutorialStepThree.SetActive(false);
    }

    public void DeactivateFirstActivateSecond()
    {
        tutorialStepOne.SetActive(false);
        tutorialStepTwo.SetActive(true);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        if (Application.installMode == ApplicationInstallMode.Store)
            AnalyticsEvent.TutorialStep(1);
    }
    public void DeactivateSecondACtivateThird()
    {
        tutorialStepTwo.SetActive(false);
        tutorialStepThree.SetActive(true);
        if (SoundManager.Instance!= null)
            SoundManager.Instance.PlayButtonClick();
        if (Application.installMode == ApplicationInstallMode.Store)
            AnalyticsEvent.TutorialStep(2);
    }

    public void DeactivateThirdAndThePainter()
    {
        tutorialStepThree.SetActive(false);
        tutorialStepOne.SetActive(false);
        tutorialStepTwo.SetActive(false);
        painterImage.gameObject.SetActive(false);
        StartCoroutine(CanAim());
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayButtonClick();
        if (Application.installMode == ApplicationInstallMode.Store)
            AnalyticsEvent.TutorialStep(3);
    }


    IEnumerator NoAming()
    {
        yield return new WaitForSeconds(1.5f);
        BallContorller.canYouStartAiming = false;
    }

    IEnumerator CanAim()
    {
        yield return new WaitForSeconds(0.2f);
        BallContorller.canYouStartAiming = true;
    }
}
