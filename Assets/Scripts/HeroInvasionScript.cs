using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DungeonMethodCollection;

public class HeroInvasionScript : MonoBehaviour
{
    DungeonMethods methods = new DungeonMethods();

    public GameObject[] heroRoster;

    float partySpeed;
    public enum actionState { Exploring, Fighting, Fleeing}
    public actionState state;

    public enum dungeonStage { Preparing, Within, Leaving, Out}
    public dungeonStage stage;

    public enum waitSetting { SetAmount, Random, OnCommand}
    public waitSetting setting;

    public float waitAmount = 30f;
    public float checkDelay = 2f;
    float timer = -1f;

    public List<MonsterScript> enemyList = new List<MonsterScript>();
    public List<MonsterScript> trapList = new List<MonsterScript>();
    public List<CharacterScript> heroList = new List<CharacterScript>();

    Transform target;
    int wayPointNum = -1;

    // Update is called once per frame
    void Update()
    {
        switch (stage)
        {
            //This places four heroes into the party, gets their character stats and sets appropriate info, then sets the first waypoint and changes the enum states
            case dungeonStage.Preparing:
                methods.ResetParty(heroRoster,heroList);
                partySpeed = methods.GetPartySpeed(heroList);
                methods.FindTraps(trapList);
                wayPointNum = 0;
                target = WayPointHolderScript.points[wayPointNum];
                stage = dungeonStage.Within;
                state = actionState.Exploring;
                break;
                //The main actions of the heroes will be decided here and will loop through until the situation changes
            case dungeonStage.Within:
                GameManager.dungeonEssence += (methods.ReleaseEssence(heroList))/4;
                methods.TrapCheck(trapList, heroList, checkDelay);
                //Here the actions are based on the state enum
                switch (state)
                {
                    //The leader will move towards the next waypoint, the followers will follow the leader
                    case actionState.Exploring:
                        if (wayPointNum == WayPointHolderScript.points.Length-1) { stage = dungeonStage.Leaving; }
                        (target, wayPointNum) = methods.MoveToTarget(heroList[0].gameObject, target,partySpeed,0.4f,wayPointNum);
                        for (int i = 0; i < heroList.Count - 1; i++)
                        {
                            methods.FollowLeader(heroList[i].gameObject, heroList[i + 1].gameObject,partySpeed);
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
                        GameManager.dungeonEssence += methods.ReleaseEssence(heroList);
                        if (methods.FearCheck(heroList)) { state = actionState.Fleeing; wayPointNum--; }
                        //Checks if there are enemies in range, otherwise sets back to exploring
                        if (enemyList.Count == 0) { state = actionState.Exploring; break; }
                        if (heroList.Count == 0) { stage = dungeonStage.Leaving; break; }
                        methods.SetEnemies(heroList, enemyList);

                        for (int i = 0; i < heroList.Count; i++)
                        {
                            //If there are no more enemies, the party resume exploring
                            if (enemyList.Count == 0) { state = actionState.Exploring; break; }
                            if (!heroList[i].HasEnemy()) { heroList[i].Fight(checkDelay); }
                            //Check the distance, then fight, otherwise move the character
                            if (methods.FightCheck(heroList[i]))
                            {
                                heroList[i].Fight(checkDelay);
                            }
                            else
                            { methods.MoveToTarget(heroList[i]); }
                        }
                        for (int j = 0; j < enemyList.Count; j++)
                        {
                            //If the party wipes out, the stage is changed to the leaving stage
                            if (heroList.Count == 0) { stage = dungeonStage.Leaving; break; }
                            if (!enemyList[j].HasEnemy()) { enemyList[j].Fight(checkDelay); }
                            if (methods.FightCheck(enemyList[j]))
                            {
                                enemyList[j].Fight(checkDelay);
                            }
                            else
                            { methods.MoveToTarget(enemyList[j]); }
                        }
                        
                        
                        break;
                        //When the collective fear of the party members is too much, the party will run away
                    case actionState.Fleeing:
                        if (wayPointNum == 1) { stage = dungeonStage.Leaving; }
                        //Once the heroes begin to flee, they will run to the exit.
                        methods.RunAway(heroList[0].gameObject, partySpeed, 0.5f, wayPointNum);
                        for (int i = 0; i < heroList.Count - 1; i++)
                        {
                            methods.FollowLeader(heroList[i].gameObject, heroList[i + 1].gameObject, partySpeed);
                        }
                        break;
                }
                break;
                //This is where the heroes make further actions after reaching the end of their journey/ also when wiped by dungeon
            case dungeonStage.Leaving:
                //Returns all heroes to the starting area
                for (int i = 0; i < heroList.Count; i++)
                {
                    heroList[i].transform.position = WayPointHolderScript.points[0].position;
                }
                heroList.Clear();
                //Restores the values for the heroes
                for (int i = 0; i < heroRoster.Length; i++)
                {
                    heroRoster[i].GetComponent<CharacterScript>().Restore();
                }
                timer = Time.time + waitAmount;
                //Sets the stage to the Out setting
                stage = dungeonStage.Out;
                break;
                //This will clear info and will then wait until next invasion of heroes
            case dungeonStage.Out:
                if (setting == waitSetting.Random) { timer += Random.Range(-timer, timer); setting = waitSetting.SetAmount; }
                if (methods.WaitCheck(timer, setting)) { stage = dungeonStage.Preparing; }
                break;
        }
    }
}
