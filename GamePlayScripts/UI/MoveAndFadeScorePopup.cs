using UnityEngine;

public class MoveAndFadeScorePopup : MonoBehaviour
{
    void Update()
    {
        transform.Translate(((new Vector3(0f, 15f ,0f)) - transform.position) * Time.deltaTime*1.3f);
        transform.localScale -= new Vector3(0.7f, 0.7f, 0)*Time.deltaTime*0.6f;
        if(transform.localScale.x < 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
