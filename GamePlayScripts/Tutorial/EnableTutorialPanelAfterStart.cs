using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTutorialPanelAfterStart : MonoBehaviour
{
    [SerializeField] GameObject panel;
    void Start()
    {
        StartCoroutine(WaitASecond());
    }

   IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(0.5f);
        panel.SetActive(true);
    }
}
