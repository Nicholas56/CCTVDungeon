using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : DungeonObject
{
    public enum objectType { Monster, Trap }
    public objectType type;

    public enum enemyChoice { Weakest, Strongest, Healer}
    public enemyChoice choice;

    public float placeCost;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    public override void TakeDamage(int damage, DungeonObject attacker)
    {
        if (type == objectType.Trap)
        {
            damage *= 2;
        }
        base.TakeDamage(damage, attacker);
    }

    protected override void Death()
    {
        base.Death();
        
        //For each hero in the current party, sets their enemy to null
        for (int i = 0; i < invasionScript.heroRoster.Length; i++)
        {
            invasionScript.heroList[i].enemy = null;
        }
        //Removes this monster from the invasion enemy list
        if (invasionScript.enemyList.Contains(this))
        {
            invasionScript.enemyList.Remove(this);
        }
        //Destroys the gameobject
        Destroy(gameObject,2f);
    }

    public override void Fight(float delay)
    {
        if (type == objectType.Monster)
        {
            if (enemy)
            {
                base.Fight(delay);
            }
            else
            {
                List<CharacterScript> enemyList = invasionScript.heroList;
                //Depending on the setting for the monster, it will pick a target before fighting
                switch (choice)
                {
                    case enemyChoice.Weakest:
                        for (int i = 0; i < enemyList.Count; i++)
                        {
                            if (enemy)
                            {
                                if (enemy.GetHealth() > enemyList[i].GetHealth()) { enemy = enemyList[i]; }
                            }
                            else { enemy = enemyList[i]; }
                        }
                        break;
                    case enemyChoice.Strongest:
                        for (int i = 0; i < enemyList.Count; i++)
                        {
                            if (enemy)
                            {
                                if (enemy.GetHealth() < enemyList[i].GetHealth()) { enemy = enemyList[i]; }
                            }
                            else { enemy = enemyList[i]; }
                        }
                        break;
                    case enemyChoice.Healer:
                        for (int i = 0; i < enemyList.Count; i++)
                        {
                            if (enemy)
                            {
                                if (enemyList[i].GetClass() == CharacterScript.heroClass.Healer) { enemy = enemyList[i]; }
                            }
                            else { enemy = enemyList[i]; }
                        }
                        break;
                }
            }
        }
        else
        {
            base.Fight(delay);
        }
    }

    public void SetChoice(int choiceNum)
    {
        switch (choiceNum)
        {
            case 0: choice = enemyChoice.Weakest; break;                
            case 1: choice = enemyChoice.Strongest; break;
            case 2: choice = enemyChoice.Healer; break;
        }
    }
}
