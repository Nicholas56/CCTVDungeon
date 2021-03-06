﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Nicholas Easterby - EAS12337350
//Inherits from DungeonObject. Gives the heroes additional functions and specialized behaviour

public class CharacterScript : DungeonObject
{
    public enum heroClass { Fighter, Tank, Rogue, Healer}
    public heroClass hero;
    
    public int fear;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        switch (hero)
        {
            //Ensures the correct animation plays
            case heroClass.Fighter:anim.SetInteger("HeroClass", 1);break;
            case heroClass.Tank: anim.SetInteger("HeroClass", 2); break;
            case heroClass.Rogue: anim.SetInteger("HeroClass", 3); break;
            case heroClass.Healer: anim.SetInteger("HeroClass", 4); break;
        }
    }
    public override void TakeDamage(int damage, DungeonObject attacker)
    {
        anim.SetTrigger("Damage");
        if (hero == heroClass.Tank)
        {
            damage = Mathf.FloorToInt(damage*0.5f);
        }
        base.TakeDamage(damage, attacker);
    }

    protected override void Death()
    {
        base.Death();
        //On death, removes this hero from the hero list
        if (invasionScript.heroList.Contains(this))
        {
            invasionScript.heroList.Remove(this);
        }
        //Moves them to the first waypoint
        transform.position = WayPointHolderScript.points[0].position;

        int deathFear = 3;
        if (hero == heroClass.Healer) { deathFear = 5; }
        for (int i = 0; i < invasionScript.heroList.Count; i++)
        {
            invasionScript.heroList[i].fear += deathFear;
        }
    }

    public heroClass GetClass() { return hero; }

    public void WalkAnim(bool isWalk) { anim.SetBool("Walking", isWalk); }

    public override void Restore()
    {
        base.Restore();
        fear = 0;
    }

    public override void Fight(float delay)
    {
        if (hero == heroClass.Healer)
        {
            if (timer < Time.time)
            {
                enemy = null;
                //Searches for weakest party member
                for (int i = 0; i < invasionScript.heroList.Count; i++)
                {
                    if (enemy)
                    {
                        if (enemy.GetHealth() > invasionScript.heroList[i].GetHealth()) { enemy = invasionScript.heroList[i]; }
                    }
                    else { enemy = invasionScript.heroList[i]; }
                }
                //If target has less than full health
                if (enemy.GetHealth() != enemy.maxHealth)
                {
                    ActionPoints -= attackCost;
                    if (ActionPoints > maxActionPoints) { ActionPoints = maxActionPoints; }
                    if (ActionPoints > 0)
                    {
                        anim.SetTrigger("Attack");
                        //Applies elemental advantages to healing
                        enemy.TakeDamage(Mathf.FloorToInt(Attack * -1 / (GameManager.CalculateElements(this.charElement, enemy.charElement))), this);
                        Exp++;
                    }
                    else { ActionPoints += attackCost; fear++; }
                }
                timer = Time.time + delay;
            }
        }
        else
        {
            //Adds fear each time the hero tries to perfom an action, but can't
            if (timer < Time.time&& ActionPoints <= 0)
            { 
                fear++; 
                timer = Time.time + delay;
            }
            //If this hero has no enemy, but there are still enemies in the enemy list, sets the top one as enemy
            if (!enemy && invasionScript.enemyList.Count > 0) { enemy = invasionScript.enemyList[0]; }
            base.Fight(delay);
            
        }
    }
}
