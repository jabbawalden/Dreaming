using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//our json file

public class WriteJSON : MonoBehaviour {
 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


public class Character
{ 
    public int health;
    public Vector3 respawnPoint;
    public bool doubleJEnabled;


    public Character (int health, Vector3 respawnPoint, bool doubleJEnabled)
    {
        this.health = health;
        this.respawnPoint = respawnPoint;
        this.doubleJEnabled = doubleJEnabled;
    }
}
