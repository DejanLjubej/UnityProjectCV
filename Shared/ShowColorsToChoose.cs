using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowColorsToChoose : MonoBehaviour
{
    public Image colorImage;
    public List<Color> colorList;
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            colorImage.color = colorList[i];
            Instantiate(colorImage, transform);
        }
    }
}
