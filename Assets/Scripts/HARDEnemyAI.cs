using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HARDEnemyAI : EnemyAI // were inheriting from our other enemy ai script
{
    public float AggressiveRunSpeed = 10f; // new speed for this enemy

    // Start is called before the first frame update
    new void Start() // Using 'new' to hide the Start() method from the base class
    {
        base.Start(); // im calling the start from our base class
        SetState(EnemyState.Attacking); // set the enemy to attack straight away
        enemyType = EnemyType.Aggro; // aggro type 
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update(); // calling our update from our derrived class

        if (enemyState == EnemyState.Attacking) // during the attack mode
        {
            AggressiveUpdate(); // activate the aggressive update
        }
    }

    protected override void AggressiveUpdate()
    {
        Agent.SetDestination(Playerslocation.position);
        Agent.speed = AggressiveRunSpeed; // set teh agents speed to the new speed we created in this script
    }
}
