using UnityEngine;
using UnityEngine.Advertisements;

public class OnWinUnityInterstitial : MonoBehaviour
{
    public int NumberOfTimesBeforeAd = 5;
    public static int numberOfTimesWon = 0;
    public void ShowInterstitialOnWin()
    {
        if(numberOfTimesWon >= NumberOfTimesBeforeAd)
        {
            if (NoAdsHandler.noAdsTimer <= 0)
            {
                Advertisement.Show("InterstitialOnWin");
                numberOfTimesWon = 0;
            }
        }
        else
        {
            int numberToAdd = Random.Range(1, 3);
            numberOfTimesWon += numberToAdd;
        }
    }
}
