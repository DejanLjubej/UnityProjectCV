using UnityEngine;
using UnityEngine.Advertisements;

public class OnLossUnityInterstitial : MonoBehaviour
{
    public int NumberOfTimesBeforeAd = 2;
    public static int numberOfTimesLost = 0;
    public void ShowInterstitialOnLoss()
    {
        if(numberOfTimesLost >= NumberOfTimesBeforeAd)
        {
            if (NoAdsHandler.noAdsTimer <= 0)
            {
                Advertisement.Show("InterstitialAfterLoosing");
                numberOfTimesLost = 0;
            }
        }
        else
        {
            int numberToAdd = Random.Range(1, 5);
            numberOfTimesLost += numberToAdd;
        }
    }
}
