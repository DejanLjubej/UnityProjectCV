using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectsOneByOne : MonoBehaviour
{
    [SerializeField] GameObject[] objectsTorender;
    [SerializeField] float LoadingTime=4f; 
    int numberOfObjectsToActivate;
    float waitTIme;
    // Start is called before the first frame update
    void OnEnable()
    {
        numberOfObjectsToActivate = objectsTorender.Length;
        waitTIme = LoadingTime / numberOfObjectsToActivate;
        StartCoroutine(OneByOne());
    }

    IEnumerator OneByOne()
    {
        foreach(GameObject go in objectsTorender)
        {
            yield return new WaitForSeconds(waitTIme);
            go.gameObject.SetActive(true);
        }
    }
}
