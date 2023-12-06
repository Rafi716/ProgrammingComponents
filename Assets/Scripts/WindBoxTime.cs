using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindBoxTime : MonoBehaviour
{
    [Header("BoxWindSettings")]
    
    public float CurrentAmountFilled = 1.0f; // the amount the image will have 
    [SerializeField] float UnWindRate = 0.3f; // the rate at which i want the box to unwind

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>(); // grab the image component
    }

    // Update is called once per frame
    void Update()
    {
        CurrentAmountFilled = Mathf.Clamp01(CurrentAmountFilled - (UnWindRate * Time.deltaTime)); // current filled will contain a value that will be strictly between 1 and 0 (mathclamp01) and will gradually unfill by the unwindrate x by the frame of our computer 
        image.fillAmount = CurrentAmountFilled; // image fill amount is now equal to what currentamount filled is
    }
}
