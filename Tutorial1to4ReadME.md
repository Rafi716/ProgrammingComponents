# Horror game components programming tutorials
**Table of Contents**

[TOCM]

[TOC]

#Tutorial 1 Component 1 Flashlight
* We are going to start of by creating a flashlight for our player so that they can walk around in our map, as well as use the flashlight to attack enemies.

* Opening our Unity scene, assuming you have already created your character and their movement functions, we will be creating child objects inside our parent object which will contain the camera and create another child object of the camera for the flashlight which will contain our light. Right click on the object and insert“light"“spotlight” which should face the direction the player is facing. Here is my example: 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20SS%201.png )

* Follow the settings I have made for my flashlight or create your own flashlight aesthetic with the options under the light panel. Do not forget to add a cookie to create that flashlight circular effect like I did from the standard assets of your choosing: 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20SS%202.png )

* we need the flashlight to have a battery, and an off and on switch, which is what im going to do next by creating the ui first. online I found some ui elements that I can use for educational purposes which I will with the following folder I am using to create the flashlight mechanics.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20SS%203.png )

* After that, create a canvas, and select a panel and put the alpha to 0 to see in the game. Rename the canvas “flashlight overlay” or whatever helps you remember what the panel’s purpose are – and add in your chosen sprites for the battery in the project like I did:

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20SS%204.png )

* On the battery bars sprites, you have chosen, make sure to change the image type to filled, to manipulate the bars when power is running out for the player when they run the program. 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20SS%205.png)

* Once we have set everything up for the flashlight, we will now create the flashlight script for manipulating the spotlight in unity to turn on or off and/or the battery had run out! To start with, we will communicate with unitys Ui in order to manipulate the UI elements:


```Using System.Collections; ```
```using System.Collections.Generic;```
```using UnityEngine;```
```using UnityEngine.UI; ```

* Afterwards, we will create three variables where one of them will be handling the UI image of the battery bars. Another variable in the float datatype which will handle the amount of power held in the battery which will be set to 1.0 since the battery chunks of the battery will be decreasing in intervals of 0.25 to slice the image up by quarters for that battery drain aesthetic. And lastly the drainage of the battery which will be used for the time when the drain should occur on the battery.
```C# 
[Header("BatteryCanvas")]
[SerializeField] Image BatteryBars; // to select the battery bars image to mannipulate in the game
[SerializeField] float Drainage = 5.0f; // float datatype for amount of power will be drained during gameplay
public float BatteryPower = 1.0f; // float datatype for the amount of battery power``` 
```
   
* Now, we must make the variable “BatteryBars” find and capture the Battery Bars image used in unity through string reference (so make sure to spell the name correctly) and then creating an invoke repeat function whereby the function for draining the flashlights battery is carried out.
  
```C#
 void OnEnable()
 {
     BatteryBars = GameObject.Find("BatteryBars").GetComponent<Image>(); // variable already initiating to find the batterybars image by string when ran
 } 
``` 

* Rather than carrying this out in start, I chose OnEnable so that the method is active everytime the gameobject is active.

	Next, we create a new method for when battery drain should occur which will be referenced by the invoke repeat function for when this shall be carried out.

```C#
    void BatteryDrainage() // battery drain function
    {
        if (BatteryPower > 0.0f) // if the battery power is more than 0
        {
            BatteryPower -= 0.25f; // remove 0.25 units which means remove a quater of the ui image 
        }
    }
``` 

* And then in the update, we are just going to update the battery bars image fill amount by the power the battery has remaining.

```C#
    // Update is called once per frame
    void Update()
    {
        BatteryBars.fillAmount = BatteryPower; // batterybars is dependent on the batterypower remaining so the ui element should slice horzontally when its losing power
    }
``` 

* Now, add this script onto the “flashlightoverlay” and reference the battery bars image from the flashlightoverlay object under the canvas and press play!

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20SS%206.png )

*This is what the results should look like:

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%201.png )
![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%202.png )


* To turn on the flashlight as well as it off, we will edit our script which contains the players ability to look around or movement to add in such features. Firstly, we start off with three variables. One variable will hold the flashlightoverlay gameobject, and the other will be a Boolean for whether the flashlight is on or off – and for the last variable, we will interact with the spotlight attached to the player to manipulate the light to turn on and off:

```C# 
    [Header("Flashlight")]
    public GameObject FlashLightOverlay; // reference pur flashlight overlay in our inspector
    private bool FlashLightActive = false;  // bool for on and off flashlight
    private Light FlashlightObj; // were going to get ahold of the light itself
``` 

* In the same script, we will create a new method for when the flashlight is turned on when the key “F” is pressed or turned off when “F” is pressed again. 

```C#
    private void Flashlight()
    {
        if (Input.GetKeyDown(KeyCode.F)) // when key F is pressed
        {
            if (FlashLightActive == false) // if the flashlight isnt active
            {
                FlashLightOverlay.SetActive(true); // flashlightoverlay is on
                FlashlightObj.enabled = true; // spot light turns on
                FlashLightActive = true; // flashlight bool is turned on
            }
            else if (FlashLightActive == true) // else the flashlight is active
            {
                FlashLightOverlay.SetActive(false); // turn off the overlay
                FlashlightObj.enabled = false; // spot light turns off
                FlashLightActive = false; // switch of the flashlight bool
            }
        }
    }
``` 

* Now, the spotlight as well as the ui elements should disappear once the key “F” is pressed. However, the battery seems to drain even after turning off the flashlight, to avoid this we stop the invoke repeating within our flashlight script like so:

```C#
    public void StopDrain() // in order for the battery not to drain after the user had turned off the flashlight
    {
        CancelInvoke("BatteryDrainage"); // the invoke function is cancelled in order to stop drainage when the light is off 
    }
``` 

* And then add this line of code into our flashlight method within our Character look or movement script: 

```C# 
else if (FlashLightActive == true) // else the flashlight is active
{
    FlashLightOverlay.SetActive(false); // turn off the overlay
    FlashlightObj.enabled = false; // spot light turns off
    FlashLightOverlay.GetComponent<Flashlight>().StopDrain(); // call the function from the flashlight script in order to turn the drainage off 
    FlashLightActive = false; // switch off the flashlight bool
}
``` 

* The following method should be called in update in order for this to work in every frame.

```C# 
 void Update()
 {
     PlayerMove();
     Flashlight();
 }
```

* The results should be the following:

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%203.png )

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%204.png )

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%205.png )



* As you can see, the power no longer drains when turned off and the player is also now able to turn on and turn off the flashlight! Great stuff!

* The final touches for this component, is to cause the flashlight to not work as well as the UI elements to not show once the battery had depleted fully. We first reference back to the player movement or look script and create a new method which will check if the battery had reached 0, and if so, the flashlight is turned off permanently along with its UI elements. 

```C#
    private void BatteryDepleted() // method for when battery runs out
    {
        if (FlashLightOverlay.GetComponent<Flashlight>().BatteryPower <= 0) // if the batterypower from the flashlight script is 0
        {
            FlashLightOverlay.SetActive(false); // flashlightoverlay should be falsed (turned off)
            FlashlightObj.enabled = false; // spotlight is turned off
            FlashLightOverlay.GetComponent<Flashlight>().StopDrain(); // stop drain function is activated since there is nothing to drain
            FlashLightActive = false; // flashlight is definitely turned off 
        }
    }
```

* I called the Battery depleted method within the flashlight method when the player had turned on the flashlight – so that the player is unable to turn on the flashlight after the battery had reached 0. 

```C#
private void Flashlight()
{
    if (Input.GetKeyDown(KeyCode.F)) // when key F is pressed
    {
        if (FlashLightActive == false) // if the flashlight isnt active
        {
            FlashLightOverlay.SetActive(true); // flashlightoverlay is on
            FlashlightObj.enabled = true; // spot light turns on
            FlashLightActive = true; // flashlight bool is turned on
            BatteryDepleted(); // this method will run if the battery had depleted so that the light doesnt turn on when power outage
        }
```

* finally, in the update we check whether the flashlight bool is on true, if so the method battery drain should run – so that the battery is checked for when it reaches 0. 

```C#
    void Update()
    {
        PlayerMove();
        Flashlight();
        if (FlashLightActive == true) // during the update, if the flashlight is on
        {
            BatteryDepleted(); // this function is ran in order to see if the battery had depleted
        }
    }
```

* The flashlight results should look like this: 

![Alt text](https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%206.png )

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%207.png )

* As you can see, the battery ran out the flashlight turned off! Repeatedly tapping f will not turn the light back on. The flashlight component is now complete!

* An extension of the flashlight, I decided to make the flashlight your only source of weapon…
To do this, I wanted to create another invoke repeating function for when the flashlight is facing an object (which is the enemy) to destroy it after a certain amount of time had went by. Doing this allowed me to not create a health script for the enemies themselves and so that the enemy can also assume that the enemies health regains if the flashlight isnt facing the enemy.
Firstly, I initiated some new variables:

```C#
    public float DestroyDelay = 3.0f; // the delay timer for the invoke repeating  
	RaycastHit hit; // raycast variable 
    private bool Destroyed = false; // bool for when the object had been destroyed
```

* Then I backtracked back to the previous script (movment/look) in which I created a raycast function to check whether the flashlight is true, and if so – then a raycast pointer should face the same direction as the direction of the flashlight is currently facing. 

```C#
    private void RaycastIsOn() // this method will allow the raycast to be on conitnuously under the update function only if the flashlightactive bool is true
    {
        if (FlashLightActive == true) // if the bool for the flashlight is active
        {
            float MaxRayDistance = FlashlightObj.range; // a float cache variable holding the max value for the raycast can reach which in this case is the range the spotlight can reach
            if (Physics.Raycast(FlashlightObj.transform.position, FlashlightObj.transform.forward, out hit, MaxRayDistance, LayerMask.GetMask("Enemy"))) // the ray is positioned in the direction of the light and if the variable "hit" contains the details of the colliders position as well as containing the max distance the ray will travel and lastly specifiying the layer the ray will only hit in the enemy layer 
            {
```

* I then decided to string reference the object using its layer so, I created one called “Target” in unity, and changed its tag to that, once I did so, I wanted to create another invokerepeating function so that there is a delay to when an object is destroyed when the raycast hits the objects collider. Whilst also creating the logic for when the flashlight is facing the enemy layer the invoke should start and when the player moves their light away from the enemy, the invoked function should not continue - additionally i also added a rule for the flashlight, where the distance is capped to the distance of where the light can reach.

```C#
    private void RaycastIsOn() // this method will allow the raycast to be on conitnuously under the update function only if the flashlightactive bool is true
    {
        if (FlashLightActive == true) // if the bool for the flashlight is active
        {
            float MaxRayDistance = FlashlightObj.range; // a float cache variable holding the max value for the raycast can reach which in this case is the range the spotlight can reach
            if (Physics.Raycast(FlashlightObj.transform.position, FlashlightObj.transform.forward, out hit, MaxRayDistance, LayerMask.GetMask("Enemy"))) // the ray is positioned in the direction of the light and if the variable "hit" contains the details of the colliders position as well as containing the max distance the ray will travel and lastly specifiying the layer the ray will only hit in the enemy layer 
            {
                if (!Destroyed) // if an enemy hit by the ray cast and hasnt been destroyed 
                {
                    TargetEnemy = hit.collider.gameObject; // the enemy thats being hit is set to the target that will be destroyed
                    InvokeRepeating("DestroyTarget", DestroyDelay, DestroyDelay); // the invoke repeating function is active in order to destroy said gameobject
                    Destroyed = true; // destroyed is true marking it as destroyed
                }
                else if (hit.collider.gameObject != TargetEnemy && Destroyed) // if a different enemy is hit or the one before had been destroyed
                {
                    CancelInvoke("DestroyTarget"); // cancel the invoke 
                    Destroyed = false; // destroy is set back to false
                }
            }
            else // or the ray hits nothing
            {
                CancelInvoke("DestroyTarget"); // stop the destroy method
                Destroyed = false; // reset to false
            }
        }
    }
```

* The invoke repeating function was then declared below the flashlight method, plus i initiated a new public gameobject variable at the top of the class: 

```C#
    [Header("ReferenceEnemy")]
    public GameObject TargetEnemy;
```

```
    private void DestroyTarget() // method for destroying a gameobject
    {
            Destroy(TargetEnemy); // Destroy the target object
    }
```

* for the destroy to work now, we will have to call our ```RaycastIsOn``` method in the update under the conditons of whether the flashlight is active:

```C#
    void Update()
    {
        PlayerMove();
        Flashlight();
        if (FlashLightActive == true) // during the update, if the flashlight is on
        {
            RaycastIsOn();
            BatteryDepleted(); // this function is ran in order to see if the battery had depleted
        }
    }
```

* The results should look like this:

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%208.png )

![Alt text](https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%201/TUT%201%20RESULT%209.png )

* I later learnt that you can actually create gizmos for your raycast, so if you want to change your raycasts range and want to see the range at which the raycast is going, you may also add this part to your code in order to visualize the raycast.

```C#
 private void OnDrawGizmos() // this allowed me to draw the gizmos of the raycast 
 {
     if (FlashLightActive) // if the flashlight is on, gizmos will be drawn
     {
         float MaxRayDistance = FlashlightObj.range; // local variable for holding the max ray length of the flashlight

         Gizmos.color = Color.red; 
         Vector3 directionofray = FlashlightObj.transform.forward; // calculates the rays direction based on the flashlights forwards direction
         Gizmos.DrawRay(FlashlightObj.transform.position, directionofray * MaxRayDistance); // draw the gizmo from the flashlight in the forwards position and multiply the distance by the flashlughts range by the
     }
 }
```















#Tutorial 2 Component 2 Player Health
* In this tutorial, we will be creating the enemies behaviour to damange our player, as well as the player having a health function in the game.

*  Right on the canvas we created earlier, create a new panel, lower the alpha to zero and add and image to said panel, which we will use as the health bar for the player.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%201.png )

* After we had set up the ui elements we can jump straight into creating a health system script in C#, ill be calling my script PlayerHealth. Before we write anything make sure to write “using UnityEngine.UI” as well as “using UnityEngine.Events” so those libraries are accessible when we code!  


```using System.Collections;```
```using System.Collections.Generic;```
```using UnityEngine;```
```using UnityEngine.Rendering;```
```using UnityEngine.UI;```
```using UnityEngine.Events;```

* We are first going to create a completely new class for the different events the health script will go through when ran whilst also declared a bunch of variables which will be used for the different attributes of the health system of the player.

```C#
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
}
```

* Afterwards, I initiated 3 different methods within our new class for the addition of health to the player, the subtraction of health and finally the percentage of health to display using those values. 

```C#
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
```

* The add method literally adds health to the player, where the amount variable is literally what is going to be added to the player. “CurrentHealth” is increased by the variable “amount” and “Mathf.Min” makes sure that “CurrentHealth” does not go over the maximum Health Value therefore choosing the smallest of values when compared. Therefore, makes this method for preventing the player from increasing their max health.

	The subtract method essentially removes health from the player using the cache variable called “amount”. Mathf.Max ensures that “CurrentHealth” does not go below 0.0f; since a player should not have health that is in the negatives so it will choose the larger number in comparison to the two numbers.

	Percentage left method calculates the percentage of the players health. This is done by dividing “CurrentHealth” by the “MaxHealthValue”

* Before we continue, I had to make sure to declare the class as serializable, in order to expose the custom private fields to the unity inspector. 

```C#
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
```

* After that, we go to the start function and initiate the Startvalue of the health of the player to their current health so that they start of with full health when running the program. 

```C#
    void Start()
    {
        HealthRemaining.CurrentValue = HealthRemaining.StartHealthValue; // health is initiated at to the start value which is the max
    }
```

* Now if we hop back to unity, we can see in the inspector, after we have attached the script to our player, the different fields for the players health! 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%202.png )

* Make sure to drag and drop the healthbar image from your canvas into the healthbar field in the script.

* Once that’s done, we can go back to our script and add an update function whereby the health UI element is updated as the health decreases or increases by using our “PercentageLeft” function. 

```C#
    void Update()
    {
        HealthRemaining.HealthBar.fillAmount = HealthRemaining.PercentageLeft(); // the percentage left fucntion will determine the percentage of health is remaining for the player (depending on the players (healthneeds class) health value) (updates the health bar image)
    }
```

* Now, we go below the update function and create three new functions that will use the sub and add methods from our previous class to determine the players health and whether or not they are taking damage or in fact dead. However, we will first declare a new variable as a unity event for when a player recieves damage. And a variable referencing our new class we created called HealthRemaining. 

```C#
public class PlayerHealth : MonoBehaviour
{
    [Header("HealthEvents")]
    public HealthNeeds HealthRemaining; // calling our new class 
    public UnityEvent GetDamaged; // event for when player gets damaged by the enemy collider
```
```C#
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
```

* The “GetDamaged” event is invoked when the player receives damage. The question mark in front of the invoke is a null check.
Next, we go back to the update function and add an if statement, which will check whether the players health had reaches 0 then the player death function would activate to process the players death. 

```C#
    void Update()
    {
        HealthRemaining.HealthBar.fillAmount = HealthRemaining.PercentageLeft(); // the percentage left fucntion will determine the percentage of health is remaining for the player (depending on the players (healthneeds class) health value) (updates the health bar image)

        if (HealthRemaining.CurrentValue <= 0.0f) // if the health reaches 0
        {
            PlayerDeath(); // die function is run
        }
    }
```

* Now, we are going to create a public interface where multiple classes will be able to share common tropes, such as in this case, anything under the Playerhealth script will be following the protocols created in the interface, which in this scenario means that damage is given to things with health.

```C#
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
```

* Do not forget to add the interface name near the monobehaviour of our class. 

```C#
public class PlayerHealth : MonoBehaviour, Idamager
```

* Heading back over to unity, we need to create a script that will damage our player so that we can see the health going down in real-time, we will add this script to the cube in our world which we will collide with later to see our results.

	Once our script is open, we are going to declare three new variables. 

```C#
public class EnemyBehaviour : MonoBehaviour
{
    [Header("EnemyDamageEvents")]
    public int Damage; // amount of damage the enemy inflicts with each attack
    public int DPS; // damage per second - the rate of damage

    private List<Idamager> thisshouldgetdamaged = new List<Idamager>(); // list created which will append what objects can be damaged by the enemy
```

* The damage variable will be the amount of damage that will be inflicted with each enemy attack.
The DPS variable is the damage rate of the enemy.
Lastly, the list variable thisshouldgetdamaged will append objects into a list which are under the idamager interface that will be damaged by the enemy.

* We start off with defining a coroutine whereby the player will receive damage every time the player is in the collision of the enemy. 

```C#
    IEnumerator DealDamage() // damage will be done with a delay
    {
        while (true) // infinite loop for damage continuously 
        {
            for (int i = 0; i < thisshouldgetdamaged.Count; i++) // loops through what should get damaged
            {
                thisshouldgetdamaged[i].DamageTaken(Damage); // damage is given here to the objects 
            }
            yield return new WaitForSeconds(DPS); // delay for when damage should be dealt after player already collided
        }
    }
```

* lastly, we define two methods for when the player enters and exits the collision in order to give and stop damage dealt to the players health.

```C#
    private void OnCollisionEnter(Collision collision) // when the object / enemy collides
    {
        if (collision.gameObject.GetComponent<Idamager>() != null)  // if the object is under the idamger interface
        {
            thisshouldgetdamaged.Add(collision.gameObject.GetComponent<Idamager>()); // append the object to the list off things to get damaged 
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Idamager>() != null)
        {
            thisshouldgetdamaged.Remove(collision.gameObject.GetComponent<Idamager>()); // remove the object from the list and stop the damage 
        }
    }
```

* Finally, we can head on over back to unity and test out the damage: -You may adjust the dps and damage output to your liking in the inspector.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%203.png )

* As you can see, the damage is given when we collide with the block and damage isn’t given when we leave its collider, additionally, if the player was to stand in the blocks collider and stay there -they will receive continuous damage!

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%201%20RESULT.png )

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%202%20RESULT.png )

* As an extension, I decided to also add in the indication of damage so that the players screen is smothered in blood once they hit an enemy.
First, we create a new script and place it into our canvas with the image we want appearing when the player takes damage. 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%204.png )

* We will first, declare the variables we will be using, additionally we will also define a coroutine. Make sure to add "using UnityEngine.UI"

```using System.Collections;```
```using System.Collections.Generic;```
```using UnityEngine;```
```using UnityEngine.UI;```

```C#
public class Damageindicator : MonoBehaviour
{
    [Header("ImageFlash")]
    public float FlashSpeed; // variables to control the flashing effect
    public Image DamagePic;

    private Coroutine FlashImageAway; // reference to coroutine that will manage the flash coroutine
```

* Then we will create a new method called “flashing” for when the image should appear in the game. 

```C#
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
```

* And lastly, we will declare the Coroutine “FadeTheImage” which will gradulally decrease the alpha of the image that’s been shown to the players screen in a while loop in case the player remains in the collision between themselves and the enemy for a continuous show of the image, as well as it should fade after the image had shown.

```C#
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
```

* To hook this up on unity, we will have to attach the damageindicator script onto our player gameobject under the panel you had created for them.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%205.png )

* Adjust the flashing of your image to your liking as long as its in the 0. As it’ll be too fast to see if its anything above (or is) 1.
Head on over back to your player gameobject – and reference your image from your canvas that will be shown to the player when they receive damage

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%206.png )

* Make sure to choose our flashing function, so that the function runs when the damage occurs.
The results should look like this: 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%203%20RESULT.png )

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%202/TUT%202%20SS%204%20RESULT.png )

* And there you have it, the working health script of the player!

















#Tutorial 3 Component 3 Dropping Objects

* Since we have created the script for the player flashlight, we want to be able to recharge the battery even if the battery had depleted completely so that the player is able to access the flashlight even after using all the battery.

*I am going to create battery objects which the enemy will leave behind once they are destroyed from our scene - so that the player may pick them up for a small recharge of their flashlight, afterall the player is using their flashlight as a weapon too.

* First of all, we are going to create a gameobject which will be the batteries for the player, in this case I’m adding a cylinder and adding a capsule collider as well as a rigid body additionally a box collider which will be set to trigger – in order to interact with the battery gameobject. 

![Alt text](image https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%203/TUT%203%20SS%201.png )

![Alt text](image https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%203/TUT%203%20SS%202.png )

* Once that is completed, we are going to create a new script which will be added onto this gameobject. The script will be called BatteryCharge – but you may call it anything of your choosing.

* We will first declare the variables we will be accessing, in this case a float for the amount of charge the battery will get when the player is interacting with the object, and a gameobject variable in order to access the players flashlight overlay.

```C#
public class BatteryCharge : MonoBehaviour
{
    [Header("BatteryChargeup")]
    public float batteryIncreaseAmount = 0.25f; // battery increases by this amount
    public GameObject playerFlashlightOverlay; // grab the overlay
```

* Since we set the batteries box collider to “is trigger” , we will be able to manipulate what happens to the gameobject once the player enters its trigger zone through “ void on trigger enter “ where we will add a conditional statement to check if the following collision between the trigger and the collider is in fact the player themselves. 

```C#
    private void OnTriggerEnter(Collider other) // when the player enters its trigger
    {
        if (other.CompareTag("Player")) // player tag checked
        {

        }
    }
```

* After, we will reference the flashlight script from the gameobject variable we created earlier and add a condition to check whether the players flashlight battery power is less than 1.0  as well as a null check.

```C#
    private void OnTriggerEnter(Collider other) // when the player enters its trigger
    {
        if (other.CompareTag("Player")) // player tag checked
        {
            Flashlight playerFlashlight = playerFlashlightOverlay.GetComponent<Flashlight>(); // playerflashlight cache variable will take ahold of the flashlight script in the overlay

            if (playerFlashlight != null && playerFlashlight.BatteryPower < 1.0f) // checks if the BatteryPower variable is less than 1.0f 
            {

            }
        }
    }
```

* If this is the case, we will be clamping the players battery between 0 and 1 so that they will not be able to increase battery power above 1.0 as well as below 0. And then, we will increment the battery power by the variable we created earlier that will be the amount of power added. Finally, we will destroy the gameobject so that the player can find the batteries rather than having an unlimited supply of power. 

```C#
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
```

* Now, we will go back to our player controller or the flashlight script, wherever the “DestroyTarget” method is located where the flashlight destroys the enemy target -and will edit the “DestroyTarget” method. 

* We will first declare at the very top of the class, a serialized variable to hold the battery gameobjects and a public gameobject for the enemies we are dealing with.

```C#
	[SerializeField] GameObject BatteryDropped; 
```

```C#
    public GameObject TargetEnemy;
```

* We will then proceed to create an if statement to check if the battery game object is not null and if so, we will cache the location of the enemys position and then instantiate the battery gameobject to the cache variable which will hold the last location of the enemy. And then destroy the gameobject.

```C#
    private void DestroyTarget() // method for destroying a gameobject
    {
        if (BatteryDropped != null)
        {
            Vector3 targetPosition = TargetEnemy.transform.position; // Store the position of the target enemy in our vector 3 local variable
            Instantiate(BatteryDropped, targetPosition, Quaternion.identity); // Instantiate the dropObject at the target's position
            Destroy(TargetEnemy); // Destroy the target object
            Destroyed = false; // reset the destroy to false to allow us to destroy another target 
        }
        else 
        {
            Debug.Log("non existent");
        }
    }
```

* The results should look like this for when the player loses battery, they will replenish (in my case) 0.25f of their battery life back if they collide into a battery game object:

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%203/TUT%203%20SS%201%20RESULT.png )

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%203/TUT%203%20SS%202%20RESULT.png )

* And, when the player has full battery and they collide with the battery gameobject, nothing should occur like so: 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%203/TUT%203%20SS%203%20RESULT.png )

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%203/TUT%203%20SS%204%20RESULT.png )

* As you can see, after hitting the object, the battery wasn’t consumed as well as it rolled away after the interaction.

* Lastly, when the gameobject is destroyed by the flashlight, the battery will be instantiated in its place:

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%203/TUT%203%20SS%205%20RESULT.png )

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%203/TUT%203%20SS%206%20RESULT.png )















#Tutorial 4 Component 4 Enemy AI

* For our last component, we will create a navmesh agent so that the enemy will follow the player once the player is in the distance range the enemy can detect the player and a barrier for when the enemy should change states when the player is undetected from aggressive to passive so that the enemy does not attack the player and the enemy just moves around the map whilst also being idle.
Firstly, we open our project from where we last left it from and select our plane we created, you may have created a terrain or something different to my ground layer, if so, just select the ground for your game. Once selected we will head over to the windows panel, head on over to the ai and select navigation.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%201.png )

* Once selected we will bake our ground layer so that the agent can cover the whole map, you may have different objects in your scene that you may want to keep as obstacles, if so – just bake the gameobjects as “not walkable” To bake the area we are going to head on over to the bake button in the navigation window and simply press bake.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%202.png )

* The next step, is to apply a nav mesh agent to our enemy gameobject so the navmesh being moved is the enemy itself.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%203.png)

* Make sure to manipulate the cylinder inside your gameobject so that it touches the ground layer perfectly for better results.

* Now, we will create a new c# script and call it EnemyAi or whatever you would prefer to call your script and add it onto our enemy gameobject.

* We will declare at the very top that we are using “UnityEngine.AI;” so that we can access the unitys ai libraries in our c# script. 


```using System.Collections;```
```using System.Collections.Generic;```
```using UnityEngine;```
```using UnityEngine.AI;

* Inside our class, we will declare a few variables for different things we will manipulate for the ai agent so that we can change the enemy’s stats and behaviour. 

```C#
    [Header("EnemyStats")]
    public float WalkSpeed; // enemy walk speed
    public float RunSpeed; // enemy run speed
    public float passiveTimer = 5f; // timer for enemy passive state
    public float passiveDuration; // how long the enemy is in passive for

    private bool isPassiveTimerRunning = false; // bool for checking if passive timer is on
```

* As you can see, this will be for the stats of the enemy and to check for the enemy passive behaviour as well as how long it is in passive for.

* Another group of variables we will be creating is for when the enemy is in stray mode so that they can wander the map and act as if they are in idle mode. 

```C#
    [Header("StrayBehaviour")]
    public float MinStrayDistance; // minimum distance the enemy will wander 
    public float MaxStrayDistance; // maximum distance the enemy will wander
    public float MinStrayWaitTimer; // timer for when the enemy will stray, minimum 
    public float MaxStrayWaitTimer; // max time for the enemy to stray
```

* And the last two variables will deal with the navmeshagent itself as well as the players location in real time so that we can manipulate the distance at which the player is detected by the enemy. 

```C#
    [Header("NavMesh")]
    private NavMeshAgent Agent; // the navmesh agent itself

    [Header("Playerlocation")]
    private Transform Playerslocation; // players realtime location 
```

* Before we start anything in our class, we will be declaring two enums, which will deal with the types of enemies such as if you want to create passive mobs in your game that wonder around or aggressive mobs that will attack the player as well as creating an enum for the different states the mob will be in depending on different events. I will be creating one type of mob where the mob will always be an enemy to the player as well as switch from passive to aggressive depending on the players location and if they are nearby to the enemy.

```C#
public enum EnemyType // the two versions of the enemy types
{
    Passive,
    Aggro
}

public enum EnemyState // the three states the enemy will be in 
{
    idleMode,
    StrayMode,
    Attacking
}
public class EnemyAI : MonoBehaviour
```

* Back to our class, we will declare a few more variables to reference our enums as well as two floats for the distance between the player and the enemy and a distance the enemy will detect the player at.

```C#
    [Header("AI")]
    public EnemyType enemyType; // type the enemy can be
    public EnemyState enemyState; // the state the enemy will be in

    public float DistanceBetweenPlayer; // distance from the enemy and the player
    public float DistanceToAvoid; // the distance where the player is out of range to the enemy
```

* We will now declare the void on awake function so that our “Agent” variable grabs the NavMeshAgent component. 

```C#
    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>(); // agent contains the component of navmesh
    }
```

* Next will declare two new methods which will activate only when the enemy is in a particular enemy type as well as its current state.

```C#
    void PassiveUpdate() // method for handling the idle and stray behaviour of the mobs
    {

    }

    void AggressiveUpdate()  // Perform attacking behavior here when in Aggressive mode
    {

    }
```

* To activate the methods we created, we will initialise a switch statement in the update method which will check for the state of the enemy and declare what method should be carried out at each case. Additionally, we will update the players location variable which checks for the distance between the enemy and the player.

```C#
    void Update()
    {
        DistanceBetweenPlayer = Vector3.Distance(transform.position, Playerslocation.transform.position); // updates the distance between the enemy and the player
        switch (enemyState) // switch statements for the different enemy states
        {
            case EnemyState.idleMode: PassiveUpdate(); break; // idle will play in passive 
            case EnemyState.StrayMode: PassiveUpdate(); break; // stray will play in passive
            case EnemyState.Attacking: AggressiveUpdate(); break; // attacking is an aggro enemy type and will be in aggressive
        }
    }
```

* We will create another method, which will declare the state the enemy will be in, and what protocols it should follow through switch statements whilst also allowing the gameobject to move in certain cases and setting the enemies speed in those cases.

```C#
    void SetState(EnemyState NewState) // setting the enemy state, idle, stray, attacking
    {
        enemyState = NewState; // update the enemies state with the cache variable 
        switch (enemyState) // based on newstate, update the enemies state
        {
            case EnemyState.idleMode: // idle mode for enemy 
                {
                    Agent.speed = WalkSpeed; // player is using walkspeed
                    Agent.isStopped = true; // agent is stopped
                    break;
                }
            case EnemyState.StrayMode: // stray mode for enemy
                {
                    Agent.speed = WalkSpeed; // enemy uses walkspeed
                    Agent.isStopped = false; // is moving
                    break;
                }
            case EnemyState.Attacking: // in attack mode
                {
                    Agent.speed = RunSpeed; // enemy uses run speeed
                    Agent.isStopped = false; // enemy is moving    
                    break;
                }
        }
    }
```

* After creating this method, we will create yet another method, which will set the enemys location for where to stroll off to when its in stray mode and not in idle mode. But before we do that, we will have to create a vector 3 method first for finding and storing a random location in the range of the enemys current location and is not too close to the player on the navmesh area we created earlier. After a certain number of attempts of trying to find the best location, it will return the best location to use for the enemy to use. 

```C#
    Vector3 EnemyStrayLocation() // method to find different locations to wander towards
    {
        NavMeshHit hit; // store info on the ray hit on the navmesh area
        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(MinStrayDistance, MaxStrayDistance)), out hit, MaxStrayDistance, NavMesh.AllAreas); // sample a random position within a specific range on the current position of the enemy on the navmeshed area 

        int i = 0; // counter to limit the attempts of finding a different loaction

        while (Vector3.Distance(transform.position, hit.position) < DistanceBetweenPlayer) // while loop to ensure a good position is suitable and isnt close to the player
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(MinStrayDistance, MaxStrayDistance)), out hit, MaxStrayDistance, NavMesh.AllAreas); // another check for a suitable position to stray for the enemy
            i++; // counter increments

            if (i == 35) // break out of loop at 35 tries since a suitable position isnt found
                break;
        }
        return hit.position; // return the final selected position for the enemy to stray in
    }
```

* Previously we spoke about creating a method to set the enemy location, now we will create this method which will be responsible for setting the enemy’s location to wander towards when it is not in “idlemode”. It will change the enemy state to “StrayMode” and set its location so that the enemy patrols the area on the navmeshed map.

```C#
    void NewStrayLocation() // method for finding new destination to wander for the enemy
    {
        if (enemyState != EnemyState.idleMode) // check if the enemy isnt in idle
            return; // dont find a new location just return
        SetState(EnemyState.StrayMode); // change the enemy state to stray 
        Agent.SetDestination(EnemyStrayLocation()); // set a new destination for the enemy
    }
```

* Closer to the end, we will now update our “PassiveUpdate” method in order to get the logic right for when the enemy is in straymode and had reached its destination, to switch to idle mode till a new stray location is given to the enemy after our delay variables we created earlier. The passive timer will also run to consider how long the enemy has been passive for before it changes to a different behaviour (StrayMode). Depending on the distance between the player and the enemy, the enemy type will change from passive to aggro if the player is in detection range, however if it is in un-detected range, the enemy will remain in stray and idle mode. 

```C#
    void PassiveUpdate() // method for handling the idle and stray behaviour of the mobs
    {
        if (enemyState == EnemyState.StrayMode && Agent.remainingDistance < 0.1f) // check if the enemy is in straymode and reached the destination
        {
            SetState(EnemyState.idleMode); // set enemy state to idle
            Invoke("NewStrayLocation", Random.Range(MinStrayWaitTimer, MaxStrayWaitTimer)); // after a certain time, the new location to stray for the enemy is given
        }

        if (enemyType == EnemyType.Passive && !isPassiveTimerRunning) // Check if currently in passive mode and not running the timer
        {
            passiveDuration = Random.Range(MinStrayWaitTimer, MaxStrayWaitTimer); // Start the passive timer
            isPassiveTimerRunning = true; // set the bool for timer is running to true
        }

        if (isPassiveTimerRunning) // Update passive timer
        {
            passiveTimer += Time.deltaTime;

            if (passiveTimer >= passiveDuration) // Check if the passive timer duration has been reached
            {
                SetState(EnemyState.StrayMode); // enemy is set to stray
                isPassiveTimerRunning = false; // timer is set to false
                passiveTimer = 0f; // passive timer resets to 0
            }
        }

        if (DistanceBetweenPlayer <= DistanceToAvoid) // if the distance detected is less than or equal to the distance undetected the player 
        {
            if (DistanceBetweenPlayer < DistanceToAvoid)
            {
                SetState(EnemyState.Attacking); // sets enemy to attacking
                enemyType = EnemyType.Aggro; // sets its type to aggro
            }

            else
            {
                SetState(EnemyState.idleMode); // or stay in idle
                enemyType = EnemyType.Passive; // and be passive
            }
        }
    }
```

* After we had updated our “PassiveUpdate” with the logic of when the player is detected and when the enemy should return to passive from aggro types, we head over to our “AggressiveUpdate” so that the enemy can pursue the player when its not passive. This is done through a conditional statement which will check if the enemy is in detection range, the enemy will change to the attacking state which will follow the players location, and if the player leaves the detection range, the enemy transitions to passive and returns to the idle state. 

```C#
    void AggressiveUpdate()  // Perform attacking behavior here when in Aggressive mode
    {
        if (DistanceBetweenPlayer >= DistanceToAvoid) // checks if distance the is greater than or equal to the distance the player is undetected  
        {
            SetState(EnemyState.idleMode); // if the following is the case, enemy is set to idle
            enemyType = EnemyType.Passive; // enemy is changed to passive
            return;
        }
        Agent.SetDestination(Playerslocation.position); // else follow player
    }
```

* Afterwards, we go to our start function and set the enemies state to straymode so that it is always straying when the program is running. 

```C#
    void Start()
    {
        SetState(EnemyState.StrayMode); // enemy will always start straying
    }
```

* Back in unity, I have gone ahead and put in some values for the min and max values for the Stray behaviour of the enemy, you may change this to your liking:

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%205.png )

* Do not forget to change the enemies stats to your liking too, the passive timer and passive duration should stay at 0 so we can privatise those values in our code since we won’t be changing those values. 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%206.png )

* You may also change the detection in which the player is undetected so when the player reaches a number that is above the undetected, the player will be followed by the enemy. 

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%208.png )

* Finally, we are set up for action!
The following results should occur:

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%201%20RESULT.png )

* As you can see, after the player in a distance above the distance the player should avoid, the gameobject will switch to a passive enemy type as well the state being in either stray or idle mode.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%202%20RESULT.png )

* The object goes into idle and is at a total standstill whilst the passive timer and duration is running, and a random location is chosen after a random value between max and min stray time is selected which will loop the gameobject back into stray mode.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%203%20RESULT.png )

* And when the player is in the detected range, the enemy type is changed to aggro as well as the attacking state initiating the game object to follow the player.

![Alt text]( https://github.com/Rafi716/ProgrammingComponents/blob/main/TUTORIAL%20SS/TUT%204/TUT%204%20SS%204%20RESULT.png )

* And the result from our previous script of enemy behaviour, we can see when the enemy collides with the player – they will take damage!














###End
