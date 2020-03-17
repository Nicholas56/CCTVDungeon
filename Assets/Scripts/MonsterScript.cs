using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : DungeonObject
{
    public enum objectType { Monster, Trap }
    public objectType type;

    public enum enemyChoice { Weakest, Strongest, Healer}
    public enemyChoice choice;

    List<CharacterScript> enemyList = new List<CharacterScript>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetEnemy(List<CharacterScript> newEnemies)
    {
        enemyList = newEnemies;
    }

    public override void TakeDamage(int damage, DungeonObject attacker)
    {
        if (type == objectType.Trap)
        {
            damage *= 2;
        }
        base.TakeDamage(damage, attacker);
    }

    public override void Fight()
    {
        if (type == objectType.Monster)
        {
            if (enemy)
            {
                base.Fight();
            }
            else
            {
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
    }
}
