using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VictoryTextHandler : MonoBehaviour
{
    [SerializeField]TMP_Text victoryText;

    [SerializeField] string[] textOptions;

    void Start()
    {
        int textToDisplayNum =  Random.Range(0, textOptions.Length);
        victoryText.text = textOptions[textToDisplayNum];
    }
}
