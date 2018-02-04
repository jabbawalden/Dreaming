using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayer : MonoBehaviour {

    public PlayerController player;
    public bool playerIsHere;
    public int energy;
    // Update is called once per frame
    void Start()
    {
        playerIsHere = false;
        energy = 100;
    }

    void Update ()
    {
        if (playerIsHere && energy > 0)
        {
            StartCoroutine(IncreaseHealthIncr());
        }


        EnergyRegeneration();


    }

    void EnergyRegeneration ()
    {
        if (Time.frameCount % 35 == 0 && energy < 100)
        {
            if (!playerIsHere)
            {
                energy += 1;
            }

            if (playerIsHere && player.health == 100)
            {
                energy += 1;
            }
           
        }
    }

    private IEnumerator IncreaseHealthIncr ()
    {
        if (Time.frameCount % 4 == 0)
        {
            if (player.health < 100 && energy > 0)
            {
                player.health++;
                energy--;
            }
        }

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsHere = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsHere = false;
        }
    }
}
