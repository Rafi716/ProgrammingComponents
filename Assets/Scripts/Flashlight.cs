using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : MonoBehaviour
{
    [Header("BatteryCanvas")]
    [SerializeField] Image BatteryBars; // to select the battery bars image to mannipulate in the game
    [SerializeField] float Drainage = 5.0f; // float datatype for amount of power will be drained during gameplay

    public float BatteryPower = 1.0f; // float datatype for the amount of battery power

    // Start is called before the first frame update
    void OnEnable()
    {
        BatteryBars = GameObject.Find("BatteryBars").GetComponent<Image>(); // variable already initiating to find the batterybars image by string when ran
    }

    // Update is called once per frame
    void Update()
    {
        BatteryBars.fillAmount = BatteryPower; // batterybars is dependent on the batterypower remaining so the ui element should slice horzontally when its losing power
    }

    void BatteryDrainage() // battery drain function
    {
        if (BatteryPower > 0.0f) // if the battery power is more than 0
        {
            BatteryPower -= 0.25f; // remove 0.25 units which means remove a quater of the ui image 
        }
    }

    public void StartDrain() 
    {
        InvokeRepeating("BatteryDrainage", Drainage, Drainage); // invoke repeat the function "batterydrainage" the first Drainage is for the delay of when the function can be called again. The second drainage is for how frequently the function is called after the first delay
    }

    public void StopDrain() // in order for the battery not to drain after the user had turned off the flashlight
    {
        CancelInvoke("BatteryDrainage"); // the invoke function is cancelled in order to stop drainage when the light is off 
    }
}

