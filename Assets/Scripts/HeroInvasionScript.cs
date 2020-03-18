using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonMethodCollection;

public class HeroInvasionScript : MonoBehaviour
{
    DungeonMethods methods = new DungeonMethods();

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
                methods.ResetParty(heroRoster,currentParty);
                heroList = methods.GetCharacterScripts(currentParty, heroList);
                partySpeed = methods.GetPartySpeed(heroList);
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
                    //The leader will move towards the next waypoint, the followers will follow the leader
                    case actionState.Exploring:
                        (target, wayPointNum) = methods.MoveToTarget(currentParty[0], target,partySpeed,0.4f,wayPointNum);
                        for (int i = 0; i < currentParty.Length - 1; i++)
                        {
                            methods.FollowLeader(currentParty[0], currentParty[i + 1],partySpeed);
                        }
                        //Performs a check of the party's surroundings every few seconds
                        if (Time.time > timer)
                        {
                            
                            (enemyList, state) = methods.PerformCheck(heroList[0], enemyList);
                            timer = Time.time + checkDelay;
                        }
                        break;
                        //Sets the characters' enemies, then allows them to fight until conclusion
                    case actionState.Fighting:
                        if (enemyList.Count == 0) { state = actionState.Exploring; }
                        else if (enemyList[0].HasEnemy())
                        {
                            for (int i = 0; i < heroList.Count; i++)
                            {
                                //Check the distance, then fight, otherwise move the character
                                if (methods.FightCheck(heroList[i]))
                                { heroList[i].Fight(); }
                                else
                                { methods.MoveToTarget(heroList[i]);}
                            }
                            for (int j = 0; j < enemyList.Count; j++)
                            {
                                if (methods.FightCheck(enemyList[j]))
                                { enemyList[j].Fight(); }
                                else
                                { methods.MoveToTarget(enemyList[j]); }
                            }
                        }
                        else
                        {
                            //If there are enemies about, but no enemies are attacking, sets all participants targets to attack
                            for (int i = 0; i < heroList.Count; i++)
                            {
                                heroList[i].SetEnemy(enemyList[0]);
                            }
                            for (int j = 0; j < enemyList.Count; j++)
                            {
                                enemyList[j].SetEnemy(heroList[0]);
                            }
                        }
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
}
