using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIndicatorManager : MonoBehaviour
{
    [SerializeField] GameObject[] _gameShopActionIndicator;
    [SerializeField] GameObject _cosmeticShopActionIndicator;

    

    void Start()
    {
        UpgradesList.amountOfColorsBought = PlayerPrefs.GetInt("AmountOfColorsBought", 1);
        StartCoroutine(WaitTilllLoaded()); ;
    }

    IEnumerator WaitTilllLoaded()
    {
        yield return new WaitForSecondsRealtime(2f);
        if(_gameShopActionIndicator != null)
            CheckTheConditionsForIndicatorActivationGameItems();
    }
    public void CheckTheConditionsForIndicatorActivationGameItems()
    {
        if((PlayerPrefs.GetInt("CurrentColorPrice", 200)<= PlayerPrefs.GetInt("PlayerCurrentMoney", 0)) ||
            (PlayerPrefs.GetInt("BouncinessPrice",500) <= PlayerPrefs.GetInt("PlayerCurrentMoney", 0)) ||
            (PlayerPrefs.GetInt("BouncesPrice", 400) <= PlayerPrefs.GetInt("PlayerCurrentMoney", 0)) ||
            (PlayerPrefs.GetInt("SpeedPrice", 600) <= PlayerPrefs.GetInt("PlayerCurrentMoney", 0))
        )
        {
            ActivateMarkerForAction();
        }
        else
        {
            DeactivateMarkerForAction();
        }
    }


    public void ActivateMarkerForAction()
    {
         foreach(var shopIndicator in _gameShopActionIndicator)
            shopIndicator.SetActive(true);
    }
    public void DeactivateMarkerForAction()
    {
        foreach(var shopIndicator in _gameShopActionIndicator)
        shopIndicator.SetActive(false);
    }

    public void ActivateMarkerForCosmetics()
    {
        _cosmeticShopActionIndicator.SetActive(true);
    }
    public void DeactivateMarkerForCosmetics()
    {
        _cosmeticShopActionIndicator.SetActive(false);
    }
}
