using UnityEngine;
using UnityEngine.Advertisements;

public class OnRetryUnityInterstitial : MonoBehaviour
{
    public int NumberOfTimesBeforeAd = 2;
    public static int numberOfTimesClicked = 0;
    public void ShowInterstitialOnRetry()
    {
        if(numberOfTimesClicked >= NumberOfTimesBeforeAd)
        {
            if (NoAdsHandler.noAdsTimer <= 0)
            {
                Advertisement.Show("InterstitialOnRetry");
                numberOfTimesClicked = 0;
            }
        }
        else
        {
            int numberToAdd = Random.Range(1, 5);
            numberOfTimesClicked += numberToAdd;
        }
    }
}
