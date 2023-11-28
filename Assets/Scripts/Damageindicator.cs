using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damageindicator : MonoBehaviour
{
    [Header("ImageFlash")]
    public float FlashSpeed; // variables to control the flashing effect
    public Image DamagePic;

    private Coroutine FlashImageAway; // reference to coroutine that will manage the flash coroutine

    public void Flashing()
    {
        if (FlashImageAway != null) // if the image is active then stop the coroutine
        {
            StopCoroutine(FlashImageAway);
        }
        DamagePic.enabled = true; // activate the image for visibillity
        DamagePic.color = Color.white; // set the image to white
        FlashImageAway = StartCoroutine(FadeTheImage()); // call the fadeimage function to fade out the image

    }

    IEnumerator FadeTheImage() // fades image coroutine
    {
        float imageAlpha = 1.0f; // sets the initial alpha to opaque
        while (imageAlpha > 0.0f) // while loop to gradually decrease the alpha value over time to create the fade
        {
            imageAlpha -= (1.0f / FlashSpeed) * Time.deltaTime; // update the alpha value based on the flashspeed and deltatime
            DamagePic.color = new Color(1.0f, 1.0f, 1.0f, imageAlpha); // set the new color with the updated alpha value
            yield return null; // waits for next frame and doesnt crash
        }
        DamagePic.enabled = false; // once the fade is fnished diable the image
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}