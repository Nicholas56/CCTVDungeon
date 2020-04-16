using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
//Nicholas Easterby - EAS12337350
//Allows the player to summon the control pad upon pressing the grab button twice in quick succesion

public class SummonPadScript : MonoBehaviour
{
    public Transform controlPad;
    Transform player;
    bool timeToGrab = false;
    float timer;
    float delay = 0f;

    GameObject teleport;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //If the grab boolean is on for the first time, the delay is set and added to the timer
        if (timeToGrab && delay==0f)
        {
            delay = 1f;
            timer = Time.time + delay;            
        }
        //Once the timer runs out the bool and delay are reset
        if (Time.time > timer)
        {
            timeToGrab = false;
            delay = 0f;
        }
        if (teleport = null) { teleport = GameObject.FindGameObjectWithTag("Teleport"); }
        if (GameManager.canTeleport) { teleport.GetComponent<VRTK_BasicTeleport>().enabled = true; }
        else { teleport.GetComponent<VRTK_BasicTeleport>().enabled = false; }
    }

    public void BeginToSummon()
    {
        //This is run once to set the bool, then run a second time to move the object into place
        if (timeToGrab)
        {
            if (player == null) { player = GameObject.FindGameObjectWithTag("Spawn").transform; }
            Vector3 pos = player.position;
            controlPad.position = pos;
        }
        timeToGrab = true;
    }
}
