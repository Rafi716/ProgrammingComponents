using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCharge : MonoBehaviour
{
    [Header("BatteryChargeup")]
    public float batteryIncreaseAmount = 0.25f; // battery increases by this amount
    public GameObject playerFlashlightOverlay; // grab the overlay

    private void OnTriggerEnter(Collider other) // when the player enters its trigger
    {
        if (other.CompareTag("Player")) // player tag checked
        {
            Flashlight playerFlashlight = playerFlashlightOverlay.GetComponent<Flashlight>(); // playerflashlight cache variable will take ahold of the flashlight script in the overlay

            if (playerFlashlight != null && playerFlashlight.BatteryPower < 1.0f) // checks if the BatteryPower variable is less than 1.0f 
            {
                playerFlashlight.BatteryPower = Mathf.Clamp(playerFlashlight.BatteryPower, 0.0f, 1.0f); // Battery is clamped at exactly values between 0 and 1
                playerFlashlight.BatteryPower += batteryIncreaseAmount; // Increase player's battery by 0.25
                Destroy(gameObject); // Destroy the battery object
            }
        }
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
