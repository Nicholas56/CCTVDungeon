using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMethodCollection
{
    public class DungeonMethods 
    {
        public (List<MonsterScript>, HeroInvasionScript.actionState) PerformCheck(CharacterScript leader, List<MonsterScript> monsterList)
        {
            int layerMask = 1 << leader.checkFor;
            //This will search for colliders that belong to a certain layer and upon finding something, change the player to the fighting state
            Collider[] hitColliders = Physics.OverlapSphere(leader.transform.position, leader.checkRadius, layerMask);
            HeroInvasionScript.actionState currentState = HeroInvasionScript.actionState.Exploring;
            if (hitColliders.Length > 0)
            {
                hitColliders = Physics.OverlapSphere(leader.transform.position, leader.checkRadius + 5f, layerMask);
                foreach (Collider col in hitColliders)
                {
                    monsterList.Add(col.gameObject.GetComponent<MonsterScript>());
                    currentState = HeroInvasionScript.actionState.Fighting;
                }
            }
            return (monsterList, currentState);
        }

        public void ResetParty(GameObject[] wholeCollection, List<CharacterScript> currentParty)
        {
            currentParty.Clear();
            //Randomizes four unique numbers to pick from the hero roster
            List<int> randNums = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int randNum = Random.Range(0, wholeCollection.Length);
                if (randNums.Contains(randNum)) { i--; } else { randNums.Add(randNum); }
            }
            for (int i = 0; i < randNums.Count; i++)
            {
                currentParty.Add(wholeCollection[randNums[i]].GetComponent<CharacterScript>());
            }
        }

        public void MoveToTarget(DungeonObject entity)
        {
            Vector3 direction = (entity.enemy.transform.position - entity.transform.position).normalized;
            entity.transform.Translate(direction * entity.charSpeed * Time.deltaTime);
        }

        public (Transform, int) MoveToTarget(GameObject leader, Transform newTarget, float moveSpeed, float checkDistance, int currentWayPoint)
        {
            //For the leader, who is followed by the others, gets the move direction and moves towards the waypoint
            Vector3 direction = (newTarget.position - leader.transform.position).normalized;
            leader.transform.Translate(direction * moveSpeed * Time.deltaTime);

            //When this is close enough to its target, the target is changed to the next one
            if (Vector3.Distance(leader.transform.position, newTarget.position) <= checkDistance)
            {
                currentWayPoint++;
            }
            return (WayPointHolderScript.points[currentWayPoint], currentWayPoint);
        }

        public int RunAway(GameObject leader, float moveSpeed, float checkDistance, int currentWayPoint)
        {
            //For the leader, who is followed by the others, gets the move direction and moves towards the waypoint
            Vector3 direction = (WayPointHolderScript.points[currentWayPoint].position - leader.transform.position).normalized;
            leader.transform.Translate(direction * moveSpeed * Time.deltaTime);

            //When this is close enough to its target, the target is changed to the next one
            if (Vector3.Distance(leader.transform.position, WayPointHolderScript.points[currentWayPoint].position) <= checkDistance)
            {
                currentWayPoint--;
            }
            return currentWayPoint;
        }

        public bool WaitCheck(float timer, HeroInvasionScript.waitSetting setting)
        {
            switch (setting)
            {
                case HeroInvasionScript.waitSetting.SetAmount:
                    if (timer < Time.time) { return true; } else { return false; }
                    break;
                
                case HeroInvasionScript.waitSetting.OnCommand:
                    return false;
                    break;
            }
            return false;
        }

        public void SetEnemies(List<CharacterScript> heroes, List<MonsterScript> monsters)
        {
            for (int i = 0; i < heroes.Count; i++)
            {
                heroes[i].SetEnemy(monsters[0]);
            }
            for (int j = 0; j < monsters.Count; j++)
            {
                monsters[j].SetEnemy(null);
            }
        }

        public bool FightCheck(DungeonObject entity)
        {
            if (entity.HasEnemy())
            {
                if (Vector3.Distance(entity.transform.position, entity.enemy.transform.position) < entity.attackRange)
                { return true; }
                else { return false; }
            }
            else { return false; }
        }

        public bool DeathCheck(MonsterScript attacker, List<CharacterScript> heroes)
        {
            if (heroes.Contains((CharacterScript)attacker.enemy)) { return true; }else { return false; }
        }
        public bool DeathCheck(CharacterScript attacker, List<MonsterScript> monsters)
        {
            if (attacker.GetClass() != CharacterScript.heroClass.Healer)
            {
                if (monsters.Contains((MonsterScript)attacker.enemy)) { return true; } else { return false; }
            }
            else { return true; }
        }

        public void FindTraps(List<MonsterScript> traps)
        {
            traps.Clear();
            GameObject[] objectsFound = GameObject.FindGameObjectsWithTag("Trap");
            foreach(GameObject trap in objectsFound)
            {
                traps.Add(trap.GetComponent<MonsterScript>());
            }
        }

        public void TrapCheck(List<MonsterScript> traps, List<CharacterScript> heroes, float delay)
        {
            for (int i = 0; i < traps.Count; i++)
            {
                if (Vector3.Distance(traps[i].transform.position, heroes[0].transform.position) < 1f)
                {
                    //If the leader of the party is close enough, the trap springs
                    traps[i].SetEnemy(heroes[0]);
                    traps[i].Fight(delay * 2);
                }
                else { if (traps[i].HasEnemy()) { traps[i].SetEnemy(null); } }
            }
        }

        public void FollowLeader(GameObject leader, GameObject follower, float speed)
        {
            //The followers keep within a certain distance of the leader
            if (Vector3.Distance(leader.transform.position, follower.transform.position) >= 1.5f)
            {
                Vector3 direction = (leader.transform.position - follower.transform.position).normalized;
                follower.transform.Translate(direction * speed * Time.deltaTime);
            }
        }

        public float GetPartySpeed(List<CharacterScript> characters)
        {
            //Goes though the party and chooses the lowest speed as the party speed
            float speed = 0;
            foreach (CharacterScript member in characters)
            {
                if (speed == 0) { speed = member.charSpeed; }
                else
                {
                    if (speed > member.charSpeed) { speed = member.charSpeed; }
                }
            }
            return speed;
        }

        public bool FearCheck(List<CharacterScript> characters)
        {
            int fearCount = 0;
            foreach(CharacterScript hero in characters)
            {
                fearCount += hero.fear;
            }
            if (fearCount == characters.Count * 10) { return true; }
            else { return false; }
        }

        public float ReleaseEssence(List<CharacterScript> heroes)
        {
            float totalEssence = 0;
            for (int i = 0; i < heroes.Count; i++)
            {
                totalEssence += heroes[i].Essence;
            }
            return totalEssence;
        }
    }
}
