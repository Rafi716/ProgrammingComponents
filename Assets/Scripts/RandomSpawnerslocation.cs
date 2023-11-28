using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RandomSpawnerslocation : MonoBehaviour
{
    [Header("TheRandomSpawnPointsSettings")]
    public GameObject ObjectToSpawn; // The prefab you want to spawn
    public int NumberOfClones = 10; // Number of clones to instantiate
    public float DistanceBetweenClones = 20.0f; // Minimum distance between spawned objects

    void Start()
    {
        SpawnObjects(); // method for spawning the object
    }

    void SpawnObjects() 
    {
        for (int i = 0; i < NumberOfClones; i++) // creates a loop to spawn the clones, as long as i is less than number of clones, increment i
        {
            Vector3 randomPosition = GetRandomPosition(i); // get a random position and spawn the object there the current value of i is being used as a way to generate a random position
            Instantiate(ObjectToSpawn, randomPosition, Quaternion.identity); // the object is cloned at that random position with the same rotations (identity)
        }
    }

    Vector3 GetRandomPosition(int iteration)
    {
        Vector3 randomPos = new Vector3(Random.Range(-50f, 50f), 0f, Random.Range(-50f, 50f)); // Adjust these ranges based on your plane size, random number is geneerated between 50 in the x and 50 in the z axis

        Collider[] colliders = Physics.OverlapSphere(randomPos, DistanceBetweenClones * iteration); // list variable called colliders will store the area by creating a sphere in that specific area taking in account the random pos it generated dand the distancebetwwenthe clones and multiply it by iteration in order to create a bigger distance to detect for avoiding spawning next to other spawners
        bool foundCloseObject = false;

        foreach (Collider col in colliders) // iterate through the colliders (called col) in our cache variable for colliders
        {
            if (col.CompareTag("RandSpawn")) // if col variable has the tag randspawn 
            {
                foundCloseObject = true; // spawn is nearby 
                break; // break out of the loop
            }
        }

        if (foundCloseObject) // if the spawner is too close
        {
            return GetRandomPosition(iteration); // Try finding a new position recursively
        }

        return randomPos; // return the random position if there is no objecct nearby
    }
}
