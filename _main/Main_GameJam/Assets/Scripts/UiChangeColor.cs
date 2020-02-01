using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiChangeColor : MonoBehaviour
{
    public Color colorOnEnter;
    public Color colorOnExit;
    public Color colorOnSelected;
    
    
    // Start is called before the first frame update
    public void ChangeColorEnter()
    {
        GetComponent<TextMeshProUGUI>().color = colorOnEnter;
    }
    public void ChangeColorExit()
    {
        GetComponent<TextMeshProUGUI>().color = colorOnExit;
    }
    public void ChangeColorSelected()
    {
        GetComponent<TextMeshProUGUI>().color = colorOnExit;
    }
}
