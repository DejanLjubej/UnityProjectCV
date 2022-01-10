using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintExplosion : MonoBehaviour
{
    [SerializeField]GameObject[] explosionParticles;
    public int AmountOfBallParticles { get; set; }
    public int amountOfBallParticles;

    [SerializeField] Transform[] _startPositions; 
    [SerializeField] Transform[] _endPositions; 

    public void TriggerExplosion()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        for (int i = 0; i < amountOfBallParticles; i++)
        {
            yield return new WaitForSeconds(0.1f);
           
           
           if(_startPositions.Length>=1){
            GameObject explosionParticle = (GameObject)Instantiate(
                explosionParticles[Random.Range(0,explosionParticles.Length)], 
                _startPositions[i].position,
                Quaternion.identity);
            explosionParticle.GetComponent<Rigidbody2D>().AddForce((_endPositions[i].position -_startPositions[i].position).normalized*500);

           }else{
            GameObject explosionParticle     = (GameObject)Instantiate(
                explosionParticles[Random.Range(0,explosionParticles.Length)], 
                transform.position + new Vector3(Random.Range(-0.1f,0.1f), Random.Range(-0.1f, 0.1f), 0),
                Quaternion.identity);
            explosionParticle.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(-2000, 2000), Random.Range(-2000, 2000), 0));
           }
        }
    }
}
