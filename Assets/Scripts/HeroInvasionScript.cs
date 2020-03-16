﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInvasionScript : MonoBehaviour
{
    public GameObject[] heroRoster;

    public GameObject[] currentParty = new GameObject[4];
    float partySpeed;
    public enum actionState { Exploring, Fighting, Fleeing}
    public actionState state;

    public enum dungeonStage { Preparing, Within, Leaving, Out}
    public dungeonStage stage;

    public float checkDelay = 2f;
    float timer = -1f;

    List<MonsterScript> enemyList = new List<MonsterScript>();
    List<CharacterScript> heroList = new List<CharacterScript>();

    Transform target;
    int wayPointNum = -1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (stage)
        {
            //This places four heroes into the party, gets their character stats and sets appropriate info, then sets the first waypoint and changes the enum states
            case dungeonStage.Preparing:
                ResetParty();
                GetCharacterScripts(currentParty);
                partySpeed = GetPartySpeed(heroList);
                wayPointNum = 0;
                target = WayPointHolderScript.points[wayPointNum];
                stage = dungeonStage.Within;
                state = actionState.Exploring;
                break;
                //The main actions of the heroes will be decided here and will loop through until the situation changes
            case dungeonStage.Within:
                //Here the actions are based on the state enum
                switch (state)
                {
                    case actionState.Exploring:
                        MoveToTarget(currentParty[0], target);
                        for (int i = 0; i < currentParty.Length - 1; i++)
                        {
                            FollowLeader(currentParty[0], currentParty[i + 1]);
                        }
                        //Performs a check of the party's surroundings every few seconds
                        if (Time.time > timer)
                        {
                            PerformCheck(heroList[0]);
                            timer = Time.time + checkDelay;
                        }
                        break;
                    case actionState.Fighting:

                        break;
                    case actionState.Fleeing:

                        break;
                }
                break;
                //This is where the heroes make further actions after reaching the end of their journey/ also when wiped by dungeon
            case dungeonStage.Leaving:

                break;
                //This will clear info and will then wait until next invasion of heroes
            case dungeonStage.Out:

                break;
        }
    }

    void PerformCheck(CharacterScript leader)
    {
        //This will search for colliders that belong to a certain layer and upon finding something, change the player to the fighting state
        Collider[] hitColliders = Physics.OverlapSphere(leader.transform.position, leader.checkRadius, leader.checkFor);
        if (hitColliders.Length > 0)
        {
            foreach (Collider col in hitColliders)
            {
                enemyList.Add(col.gameObject.GetComponent<MonsterScript>());
            }
            state = actionState.Fighting;
        }
    }

    void ResetParty()
    {
        //Randomizes four unique numbers to pick from the hero roster
        List<int> randNums = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            int randNum = Random.Range(0, heroRoster.Length);
            if (randNums.Contains(randNum)) { i--; } else { randNums.Add(randNum); }
        }
        for (int i = 0; i < randNums.Count; i++)
        {
            currentParty[i] = heroRoster[randNums[i]];
        }
    }

    void GetCharacterScripts(GameObject[] partyMembers)
    {
        //Goes through the party to get the character scripts
        heroList = new List<CharacterScript>();
        for (int i = 0; i < partyMembers.Length; i++)
        {
            heroList.Add(partyMembers[i].GetComponent<CharacterScript>());
        }
    }

    void MoveToTarget(GameObject leader,Transform target)
    {
        //For the leader, who is followed by the others, gets the move direction and moves towards the waypoint
        Vector3 direction = (target.position - leader.transform.position).normalized;
        transform.Translate(direction * partySpeed * Time.deltaTime);

        //When this is close enough to its target, the target is changed to the next one
        if (Vector3.Distance(transform.position, target.position) <= 0.4f)
        {
            wayPointNum++;
            target = WayPointHolderScript.points[wayPointNum];
        }
    }

    void FollowLeader(GameObject leader,GameObject follower)
    {
        //The followers keep within a certain distance of the leader
        if (Vector3.Distance(transform.position, target.position) >= 1.5f)
        {
            Vector3 direction = (leader.transform.position - follower.transform.position).normalized;
            transform.Translate(direction * partySpeed * Time.deltaTime);
        }
    }

    float GetPartySpeed(List<CharacterScript> characters)
    {
        //Goes though the party and chooses the lowest speed as the party speed
        float speed = 0;
        foreach(CharacterScript member in characters)
        {
            if (speed == 0) { speed = member.charSpeed; }
            else
            {
                if (speed > member.charSpeed) { speed = member.charSpeed; }
            }
        }
        return speed;
    }


}