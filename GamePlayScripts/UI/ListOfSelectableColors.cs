using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListOfSelectableColors : MonoBehaviour
{
    public Image selectableImage;
    public GameObject[] selectableImages = new GameObject[UpgradesList.colorField.Length];
    public List<GameObject> colorsShown { get; protected set; }
    List<GameObject> _instantiatedObj = new List<GameObject>();

    void Awake()
    {
        colorsShown = new List<GameObject>();
        DisplayColorsToChoose();
    }
        /// <summary>
        /// This function instantiates all the colors that have been bought
        /// </summary>
    public void DisplayColorsToChoose()
    {
        foreach (var item in _instantiatedObj)
        {
            Destroy(item);
        }
        _instantiatedObj.Clear();
        colorsShown.Clear();

        //Go through the full list of upgrades
        for (int i = 0; i < selectableImages.Length; i++)
        {
                // String format for upgrade name
                string isSelectableColor = "SelectableColor" + i + "(Clone)";
                //Checks player prefs to see if the item was bought. {0: false; 1: true}
                if (PlayerPrefs.GetInt(isSelectableColor, 0) == 1)
                {
                    selectableImages[i].name = "Color" + i;
                    _instantiatedObj.Add(Instantiate(selectableImages[i], transform));
                    colorsShown.Add(selectableImages[i].gameObject);
                }

        }
    }
}
