using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LettersWrittenWithBalls : MonoBehaviour
{
    public GameObject[] goPoints;
    public GameObject ball;

    bool wasWritten = false;
    
    IEnumerator Write()
    {
        GameObject bull;
        bull =(GameObject)Instantiate(ball, goPoints[0].transform.position, goPoints[0].transform.rotation);
        bull.GetComponent<Rigidbody2D>().simulated = false;
        bull.transform.localScale= new Vector2(0.2f,0.2f);
        for (int i = 0; i < goPoints.Length; i++)
        {
            yield return new WaitForSeconds(0.2f);
            if (i == 0)
            {
            }
            else
            {
                bull.transform.position = goPoints[i].transform.position;
            }
        }
    }

    void OnEnable()
    {
         StartCoroutine(Write());
    }
}
