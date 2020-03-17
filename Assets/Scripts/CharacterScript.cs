using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : DungeonObject
{
    public enum heroClass { Fighter, Tank, Rogue, Healer}
    public heroClass hero;

    List<MonsterScript> enemyList = new List<MonsterScript>();

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public override void TakeDamage(int damage, DungeonObject attacker)
    {
        if (hero == heroClass.Tank)
        {
            damage = Mathf.FloorToInt(damage*0.5f);
        }
        base.TakeDamage(damage, attacker);
    }

    public void SetEnemy(List<MonsterScript> newEnemies)
    {
        enemyList = newEnemies;
    }

    public heroClass GetClass() { return hero; }

    public override void Fight()
    {
        if (hero == heroClass.Healer)
        {

        }
        else
        {
            base.Fight();
        }
    }
}
