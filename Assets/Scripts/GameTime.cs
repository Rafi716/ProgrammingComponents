using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameTime : MonoBehaviour
{
    [Header("TimeSettings")]
    public TMP_Text Timer; // using the tmp text variable to store our text

    [SerializeField] float CurrentTimer; // the timer that will count down
    [SerializeField] int HourCount; // the count to increment when 60 seconds had gone by

    private const int MaxHour = 6; // static integer variable declared for the number r6
    // Start is called before the first frame update
    void Start()
    {
        CurrentTimer = 60.0f; // currenttimer starts at 60
        TimeUpdate(); // timeupdates 
    }

    // Update is called once per frame
    void Update()
    {
        CurrentTimer -= Time.deltaTime; //this causes the timer to decrement by seconds 
        if (CurrentTimer <= 0) // checks if the timer had reached 0
        {
            HourCount++; // hour increments 
            CurrentTimer = 60.0f; // reset the timer back to 60 for loop
            
            if (HourCount > MaxHour) // if the hourcount reaches the maxhour 
            {
                HourCount = MaxHour; // the hour wont change no longer since the max value had been reached
            }
            TimeUpdate(); // timeupdate is called
        }
    }

    void TimeUpdate() // method for casting int to string
    {
        Timer.text = HourCount.ToString() + " AM";    
    }

}
