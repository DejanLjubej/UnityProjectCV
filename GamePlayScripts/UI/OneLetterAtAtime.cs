using System.Collections;
using UnityEngine;
public class OneLetterAtAtime : MonoBehaviour
{
    [SerializeField] GameObject[] letters;
    bool hasSigned = false;

    void Start()
    {
        foreach(GameObject letter in letters)
        {
            letter.SetActive(false);
        }
    }

    IEnumerator ActivateLetters()
    {
        foreach(GameObject letter in letters)
        {
            yield return new WaitForSeconds(0.2f);

            letter.SetActive(true);
        }
    }
    void Update()
    {
        if(this.gameObject.activeSelf && !hasSigned)
        {
            StartCoroutine(ActivateLetters());
            hasSigned = true;
        }
    }
}
