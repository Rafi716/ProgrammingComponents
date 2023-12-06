using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class REWind : MonoBehaviour
{
    [Header("RewindSettings")]
    [SerializeField] float RefillAmount = 0.03f; // variable for how much should be refilled
    [SerializeField] KeyCode PressE = KeyCode.E; // keycode variable
    [SerializeField] GameObject OPEnemy;

    private WindBoxTime WindBox; // reference the windbox script
    private bool IsPlayerNearby = false; // bool for checking if player is in trigger zone
    private bool PlayerIsRefilling = false; // bool for checking if the key is being held
    private bool OPEnemySpawned = false; // bool flag to stop frequent instantiation in update 

    // Start is called before the first frame update
    void Start()
    {
        WindBox = FindObjectOfType<WindBoxTime>(); // grab windboxtime script and place in variable windbox
    }

    // Update is called once per frame
    void Update()
    {
        RefillChecker();

        if (WindBox.CurrentAmountFilled == 0f && !OPEnemySpawned) // once the refill amount is 0 and the enemy isnt spawned
        {
            SpawnOPEnemy(); // spawn the enemy
            OPEnemySpawned = true; // bool is set to true preventing anymore instantiations 
        }
    }

    private void RefillChecker()
    {
        if (IsPlayerNearby) // checks if the player is nearby the trigger zone and if they are
        {
            if (Input.GetKey(PressE)) // if theyre holding e
            {
                PlayerIsRefilling = true; // player refill is true
            }

            else if (Input.GetKeyUp(PressE)) // else if they let go of e 
            {
                PlayerIsRefilling = false; // player isnt refilling the image any more
            }
        }

        if (PlayerIsRefilling) // if player refilling the image
        {
            RefillWindBox(); // refill method is activated
        }
    }
    void RefillWindBox() // method to refill the image
    {
        if (WindBox.CurrentAmountFilled > 0f) // if the current amount of the image is greater than 0
        {
            WindBox.CurrentAmountFilled = Mathf.Clamp01(WindBox.CurrentAmountFilled + RefillAmount); // between 0 and 1 again, the current amount the image has will be incremented by the refill amount when player presses e 
            if (WindBox.CurrentAmountFilled <= 0f) // once its reached 0
            {
                PlayerIsRefilling = false; // player can no longer refill
                WindBox.CurrentAmountFilled = 0f; // make sure the  current fill is absoulute 0
            }
        }
    }

     void SpawnOPEnemy() // method for spawning the enemy
    {
        Vector3 SpawnLocation = transform.position + Vector3.up * 2f; // creating a cache variable which will store the objects current position plus a vector up for the height timesed by 2
        Instantiate(OPEnemy, SpawnLocation, Quaternion.identity); // spawn the enemy at the location with teh same rotaions
    }

    private void OnTriggerEnter(Collider other) // the trigger zone the player has to enter
    {
        if (other.CompareTag("Player")) // tag of the player
        {
            IsPlayerNearby = true; // bool set to true 
        }
    }

    private void OnTriggerExit(Collider other) // when player exits that trigger zone
    {
        if (other.CompareTag("Player")) 
        {
            IsPlayerNearby = false; // set bool to false
            PlayerIsRefilling = false;
        }
    }
}
