using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOneScript : MonoBehaviour {

    public GameObject enemy;
    [SerializeField]
    bool enemyDead;

    public SpawnActivation spawnCheck, spawnCheck2;
    //ifplayerhitstrigger, set enemyDead to false

	// Use this for initialization
	void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (spawnCheck.enemyRespawn || spawnCheck2.enemyRespawn)
        {
            enemyDead = false;
        }

        CheckEnemyStatus();

        
	}


    void CheckEnemyStatus ()
    {
        if (enemyDead)
        {
            enemy.SetActive(false);
            //setboxcollidertofalse
            
        }
        else
        {
            enemy.SetActive(true);
            //setboxcollidertofalse
            //callEnemyBehaviour
            
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemyDead = true;
            spawnCheck.enemyRespawn = false;
            spawnCheck2.enemyRespawn = false;
        }
    }
}
