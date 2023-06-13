using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBGColor : MonoBehaviour 
{
    [SerializeField] private GameObject Parent;

    private Image bgImage;

    private Color ogColor;
    private Color newColor;

    // Start is called before the first frame update
    void Start()
    {
        bgImage = Parent.transform.GetComponent<Image>();

        ogColor = bgImage.color; // original
        newColor = new Color(0.0f, 0.0f, 0.0f, 1.0f); // black
    }

    public void ToggleColor()
    {   
        bool isOriginalColor = bgImage.color == ogColor;
        bgImage.color = isOriginalColor ? newColor : ogColor;
    }
}
