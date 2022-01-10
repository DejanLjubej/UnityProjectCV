using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ChooseThisColor : MonoBehaviour
{
    public static int chosenColorNumber;

    int numberOfThisObject;
    string nameNumber;

    SpawnBall _spawnBall;

    void Start()
    {
        _spawnBall = FindObjectOfType<SpawnBall>();
        nameNumber = this.name.Replace("Color", "").Replace("(Clone)", "").Trim();
        int.TryParse(nameNumber, out numberOfThisObject);
        if(numberOfThisObject == chosenColorNumber)
        {
            this.gameObject.GetComponentInChildren<Button>().interactable = false;
        }
    }

    void Update() {
        if (Input.GetMouseButtonUp(0))
        {
            if (numberOfThisObject != chosenColorNumber)
            {
                this.gameObject.GetComponentInChildren<Button>().interactable = true;
            }
        }
    }

    void CheckChosenColorInteractabillity()
    {
            this.gameObject.GetComponent<Button>().interactable = false;
    }

    public void UseThisColor()
    {
        if (SoundManager.Instance != null)
        SoundManager.Instance.PlayColorPick();
        int.TryParse( nameNumber, out chosenColorNumber);
        CheckChosenColorInteractabillity();
        _spawnBall.ChangeChosenColor();
    }
}
