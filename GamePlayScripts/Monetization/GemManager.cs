using UnityEngine;
using UnityEngine.UI;

public class GemManager : MonoBehaviour
{
    public static int gemAmount;
    [SerializeField] Text[] gemAmountTexts;

    void Start()
    {
        gemAmount=PlayerPrefs.GetInt("GemAmount", 0);
        UpdateGemAmountText();
    }


    public void UpdateGemAmountText()
    {
        PlayerPrefs.SetInt("GemAmount", gemAmount);
        foreach(Text text in gemAmountTexts)
        {
            if(text != null)
            text.text = gemAmount.ToString();
        }
    }
}
