using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private ESCounter enemyCounter; // finds our enemy counter

    [Header("EnemyDamageEvents")]
    public int Damage; // amount of damage the enemy inflicts with each attack
    public int DPS; // damage per second - the rate of damage

    private List<Idamager> thisshouldgetdamaged = new List<Idamager>(); // list created which will append what objects can be damaged by the enemy

    // Start is called before the first frame update
    void Start()
    {
        enemyCounter = FindObjectOfType<ESCounter>();
        StartCoroutine(DealDamage()); // start the coroutine to deal damage 
    }

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

    private void OnDestroy() // when the object is destroyed
    {
        enemyCounter.TotalSpawned--; // the referenced script with the counter is decremented
    }

    // Update is called once per frame
    void Update()
    {

    }
}