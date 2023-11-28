using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, Idamager
{
    [Header("HealthEvents")]
    public HealthNeeds HealthRemaining; // calling our new class 
    public UnityEvent GetDamaged; // event for when player gets damaged by the enemy collider

    // Start is called before the first frame update
    void Start()
    {
        HealthRemaining.CurrentValue = HealthRemaining.StartHealthValue; // health is initiated at to the start value which is the max
    }

    // Update is called once per frame
    void Update()
    {
        HealthRemaining.HealthBar.fillAmount = HealthRemaining.PercentageLeft(); // the percentage left fucntion will determine the percentage of health is remaining for the player (depending on the players (healthneeds class) health value) (updates the health bar image)

        if (HealthRemaining.CurrentValue <= 0.0f) // if the health reaches 0
        {
            PlayerDeath(); // die function is run
        }
    }

    public void Healing(float amount) // function for heazling the player with a cache variable called amount in float datatype
    {
        HealthRemaining.Add(amount); // adds health onto the current health the character 
    }

    public void DamageTaken(int DamageAmount) // function for damage taken from enemy
    {
        HealthRemaining.Subtract(DamageAmount); // current players health is subtracted by the damage amount int cache variable
        GetDamaged?.Invoke(); // if the player gets damage then invoke the getdamaged function
    }

    public void PlayerDeath() // function for the death of the player
    {
        GetDamaged?.Invoke(); // the question mark checks if the function isnt null then invoke the method
    }
}

[System.Serializable]
public class HealthNeeds // i created a new class for the different events the health 
{
    [Header("HealthNeeds")]
    public float CurrentValue; // players current health
    public float StartHealthValue; // players health at the start of the game
    public float MaxHealthValue; // players maximum health which shouldnt go above that amount
    public float MinHealthValue; // minimum health of the player
    public float RegenRate; // players regeneration of health after some time
    public float DamageRate; // how many hit points per time the player will take

    public Image HealthBar; // healthbar image will be mannipulated for damagetaken and health 
    public void Add(float amount) // function for adding health
    {
        CurrentValue = Mathf.Min(CurrentValue + amount, MaxHealthValue); // currentvalue is clamped at maxvalue through the mathf.min (the number thats less is always chosen)
    }

    public void Subtract(float amount) // function for taking off health
    {
        CurrentValue = Mathf.Max(CurrentValue - amount, 0.0f); // 0 is the maximum value which will be chosen in the script
    }

    public float PercentageLeft()
    {
        return CurrentValue / MaxHealthValue; // health percentage is made through this function
    }
}

// interface is a set of methods that will target class will implement
// by using interface you are able to access interface fields on different classes.
// so multiple classes will by accessed through the Idamager class 
//this way we can reference the interface rather tahn the class
//this way, more tan one gameobject can implement the Idamager interface class e.g player, enemies etc
// by doing this, we can get a collision in unity and check if its under the idamager interface
//and if it is damage will be dealt since it is in that interface 
public interface Idamager
{
    void DamageTaken(int DamageAmount);
}