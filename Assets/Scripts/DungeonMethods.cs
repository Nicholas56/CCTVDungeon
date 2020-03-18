using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonMethodCollection
{
    public class DungeonMethods 
    {
        public (List<MonsterScript>, HeroInvasionScript.actionState) PerformCheck(CharacterScript leader, List<MonsterScript> monsterList)
        {
            //This will search for colliders that belong to a certain layer and upon finding something, change the player to the fighting state
            Collider[] hitColliders = Physics.OverlapSphere(leader.transform.position, leader.checkRadius, leader.checkFor);
            HeroInvasionScript.actionState currentState = HeroInvasionScript.actionState.Exploring;
            if (hitColliders.Length > 0)
            {
                hitColliders = Physics.OverlapSphere(leader.transform.position, leader.checkRadius + 5f, leader.checkFor);
                foreach (Collider col in hitColliders)
                {
                    monsterList.Add(col.gameObject.GetComponent<MonsterScript>());
                    currentState = HeroInvasionScript.actionState.Fighting;
                }
            }
            return (monsterList, currentState);
        }

        public void ResetParty(GameObject[] wholeCollection, GameObject[] resetSelection)
        {
            //Randomizes four unique numbers to pick from the hero roster
            List<int> randNums = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                int randNum = Random.Range(0, wholeCollection.Length);
                if (randNums.Contains(randNum)) { i--; } else { randNums.Add(randNum); }
            }
            for (int i = 0; i < randNums.Count; i++)
            {
                resetSelection[i] = wholeCollection[randNums[i]];
            }
        }

        public List<CharacterScript> GetCharacterScripts(GameObject[] partyMembers, List<CharacterScript> charList)
        {
            //Goes through the party to get the character scripts
            charList = new List<CharacterScript>();
            for (int i = 0; i < partyMembers.Length; i++)
            {
                charList.Add(partyMembers[i].GetComponent<CharacterScript>());
            }
            return charList;
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

        public bool FightCheck(DungeonObject entity)
        {
            if (Vector3.Distance(entity.transform.position, entity.enemy.transform.position) < entity.attackRange)
            { return true; }
            else { return false; }
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
    }
}
