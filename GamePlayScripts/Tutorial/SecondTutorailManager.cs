using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SecondTutorailManager : MonoBehaviour
{

    [SerializeField] GameObject ouOfFrameTask;
    [SerializeField] GameObject secondTask;
    [SerializeField] Image painter;


    void OnEnable()
    {
        StartCoroutine(NoAiming());
        ouOfFrameTask.SetActive(true);
        secondTask.SetActive(false);
    }

    public void DeactivateFirstActivateSecond()
    {
        ouOfFrameTask.SetActive(false);
        secondTask.SetActive(true);
        if(SoundManager.Instance !=null)
            SoundManager.Instance.PlayButtonClick();
    }
    public void DeactivateSecondAndPainter()
    {
        secondTask.SetActive(false);
        painter.gameObject.SetActive(false);
        StartCoroutine(CanAim());
        if (SoundManager.Instance!= null)
            SoundManager.Instance.PlayButtonClick();
    }

    IEnumerator NoAiming()
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
