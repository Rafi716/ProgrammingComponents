using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawner : MonoBehaviour
{
    [Header("SpawnersSettings")]
    public int MaxToSpawn = 8; // maximum to spawn variable
    public float SpawnDelay = 2.5f; // delay for when to spawn
    public GameObject[] EnemyInstantiater; // array for what to spawn depending on the index

    [Header("SpawnCheck")]
    public int SpawnCount = 0; // counter for how many had spawned
    public List<GameObject> spawnedEnemies = new List<GameObject>(); // list to store the enemies that have spawned


    private ESCounter EnemySpawnCounter; // reference the script that contains the enemy counter

    void Start()
    {
        EnemySpawnCounter = FindObjectOfType<ESCounter>();
        StartCoroutine(SpawnObjects()); // coroutine to spawn the object is set in start and called whe played
    }

    void Update()
    {
        
    }

    IEnumerator SpawnObjects()
    {
        while (SpawnCount < MaxToSpawn) // loop till the maximum amount of enemies to spawn had been reached 
        {
            GameObject newEnemy = Instantiate(EnemyInstantiater[SpawnCount % EnemyInstantiater.Length], transform.position, Quaternion.identity); // instantiate an enemy from the array, using the modulo function im able to cycle through the array if i was to add more elements staying in the bounds of my array
            spawnedEnemies.Add(newEnemy); // Add the instantiated enemies to the list we created earlier
            SpawnCount++; // increment the counter
            EnemySpawnCounter.TotalSpawned++; // enemies counter is incremented 
            yield return new WaitForSeconds(SpawnDelay); // allows the spawning to be delayed 
        }
    }
}

