using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
{
    [Header("AI")]
    public EnemyType enemyType; // type the enemy can be
    public EnemyState enemyState; // the state the enemy will be in

    public float DistanceBetweenPlayer; // distance from the enemy and the player
    public float DistanceToAvoid; // the distance where the player is out of range to the enemy

    [Header("EnemyStats")]
    public float WalkSpeed; // enemy walk speed
    public float RunSpeed; // enemy run speed
    public float passiveTimer = 5f; // timer for enemy passive state
    public float passiveDuration; // how long the enemy is in passive for

    private bool isPassiveTimerRunning = false; // bool for checking if passive timer is on

    [Header("StrayBehaviour")]
    public float MinStrayDistance; // minimum distance the enemy will wander 
    public float MaxStrayDistance; // maximum distance the enemy will wander
    public float MinStrayWaitTimer; // timer for when the enemy will stray, minimum 
    public float MaxStrayWaitTimer; // max time for the enemy to stray

    [Header("NavMesh")]
    private NavMeshAgent Agent; // the navmesh agent itself

    [Header("Playerlocation")]
    private Transform Playerslocation; // players realtime location 

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>(); // agent contains the component of navmesh
        Playerslocation = GameObject.FindGameObjectWithTag("Player")?.transform; // the variable will contain the information of the tagged transform and checks if it exists otherwise its null
    }

    // Start is called before the first frame update
    void Start()
    {
        SetState(EnemyState.StrayMode); // enemy will always start straying
    }

    // Update is called once per frame
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

    void NewStrayLocation() // method for finding new destination to wander for the enemy
    {
        if (enemyState != EnemyState.idleMode) // check if the enemy isnt in idle
            return; // dont find a new location just return
        SetState(EnemyState.StrayMode); // change the enemy state to stray 
        Agent.SetDestination(EnemyStrayLocation()); // set a new destination for the enemy
    }

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
}