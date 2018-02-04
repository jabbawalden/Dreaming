using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActivation : MonoBehaviour {
    [SerializeField]
    bool enemyAliveCheck;

    public GameObject[] enemyArray;

    public bool enemyRespawn;

	// Use this for initialization
	void Start ()
    {
        enemyRespawn = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*
		if (EnemyActive())
        {
            enemyAliveCheck = true;
        }
        else
        {
            enemyAliveCheck = false;
        }
        */
	}

    /*
    public bool EnemyActive ()
    {
        bool enemyActive = true;

        for (int i = 0; i < enemyArray.Length; i++)
        {
            if(enemyArray[i].activeInHierarchy)
            {enemyActive = true;}
            else
            {enemyActive = false;}
        }

        return enemyActive;
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemyRespawn = true;
        }

    }
    
}
