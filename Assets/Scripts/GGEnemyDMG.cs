using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGEnemyDMG : EnemyBehaviour // we are inheriting from the enemybehaviour script
{
    // Start is called before the first frame update
    protected override void Start() // we can globalise the start function 
    {
        base.Start();
        Damage = 100; // this enemy's damage is at max capacity
        DPS = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
