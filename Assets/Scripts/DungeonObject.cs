using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObject : MonoBehaviour
{
    public string charName;

    public float charSpeed;

    public float attackRange;


    public int maxHealth;

    int health { get { return health + healthGain; }
        set { health = value; } }

    public int attack { get { return attack + attackGain; }
        set { attack = value; } }

    int actionPoints { get { return actionPoints + actionGain; }
        set { actionPoints = value; } }

    public int attackCost;

    public int maxActionPoints;

    public int level { get { return level; }
        set{healthGain *= level;attackGain *= level;actionGain *= level;}} 
    
    public int healthGain, attackGain, actionGain; 

    public int exp {get { return exp; }
        set { level = Mathf.FloorToInt(exp / (level * 100)); } }

    public int essence { get { return level / 100; } }


    public float checkRadius;

    public LayerMask checkFor;

    public GameManager.element charElement;

    public DungeonObject enemy;

    HeroInvasionScript invasionScript;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        actionPoints = maxActionPoints;
        invasionScript = FindObjectOfType<HeroInvasionScript>();
    }

    public virtual void TakeDamage(int damage, DungeonObject attacker)
    {
        //This is how damage is taken by this entity/ may be modified
        health -= damage;
        if (health > maxHealth) { health = maxHealth; }
        if (health <= 0)
        {
            //The entity dealing damage gains 10% of this' exp for driving this to death
            attacker.GainExp(Mathf.FloorToInt(exp*0.1f));
            Death();
        }
    }

    protected virtual void Death()
    {

    }

    public virtual void GainExp(int expGain)
    {
        exp += expGain;
    }

    public virtual void SetEnemy(DungeonObject newEnemy)
    {
        enemy = newEnemy;
    }

    public bool HasEnemy() { if (enemy) { return true; } else {return false; } }

    public int GetHealth() { return health; }


    public virtual void Fight()
    {
        //If enemy is in range and enough action points, attack can occur
        actionPoints -= attackCost;
        if (actionPoints > 0)
        {
            //Applies elemental advantages to attacks
            enemy.TakeDamage(Mathf.FloorToInt(attack * GameManager.CalculateElements(this.charElement,enemy.charElement)), this);
            exp++;
        }
        else { actionPoints += attackCost; }
        
    }
}
