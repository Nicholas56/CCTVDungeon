using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObject : MonoBehaviour
{
    public string charName;
    public float charSpeed;
    public float attackRange;
    
    public int maxHealth;
    public int maxActionPoints;
    public int startAttackValue = 3;
    public int startLevelValue = 1;

    int health;
    int attack;
    int actionPoints;
    int level;
    int exp;
    float essence;

    int Health 
    { 
        get { return health; }
        set { health = value; } 
    }
    public int Attack 
    {        
        get { return attack + attackGain; }
        set { attack = value; } 
    }
    protected int ActionPoints 
    { 
        get { return actionPoints; }
        set { actionPoints = value; } 
    }
    public int Level 
    { 
        get { return level; }
        set{ level = value; healthGain += level; maxHealth += level; attackGain += level; actionGain += level; maxActionPoints += level; }
    }
    public int Exp 
    {
        get { return exp; }
        set { exp = value; } 
    }
    public float Essence 
    { 
        get { return (float)level / 100; }
    }

    public int healthGain, attackGain, actionGain;
    public int attackCost;

    protected float timer=-1;
    public float checkRadius;
    public LayerMask checkFor;

    public GameManager.element charElement;
    public DungeonObject enemy;
    protected HeroInvasionScript invasionScript;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Health = maxHealth;
        Attack = startAttackValue;
        Level = startLevelValue;
        ActionPoints = maxActionPoints;
        invasionScript = FindObjectOfType<HeroInvasionScript>();
    }

    public virtual void TakeDamage(int damage, DungeonObject attacker)
    {
        //This is how damage is taken by this entity/ may be modified
        Health -= damage;
        if (Health > maxHealth) { Health = maxHealth; }
        if (Health <= 0)
        {
            //The entity dealing damage gains 10% of this' exp for driving this to death
            attacker.GainExp(Mathf.FloorToInt(Exp*0.1f));
            Death();
        }
    }

    protected virtual void Death()
    {
        enemy = null;
        GameManager.dungeonEssence += maxActionPoints + maxHealth;
    }

    public virtual void GainExp(int expGain)
    {
        Exp += expGain;
        if (Exp > Mathf.Pow(2, Level))
        {
            Level++;
        }
    }

    public virtual void SetEnemy(DungeonObject newEnemy)
    {
        enemy = newEnemy;
    }

    public bool HasEnemy() { if (enemy) { return true; } else {return false; } }

    public int GetHealth() { return Health; }
    public int GetAction() { return ActionPoints; }

    public virtual void Restore()
    {
        Health = maxHealth;
        ActionPoints = maxActionPoints;
    }

    public virtual void Fight(float delay)
    {
        if (timer < Time.time)
        {
            //If enemy is in range and enough action points, attack can occur
            ActionPoints -= attackCost;
            if (ActionPoints > maxActionPoints) { ActionPoints = maxActionPoints; }
            if (ActionPoints > 0)
            {
                //Applies elemental advantages to attacks
                enemy.TakeDamage(Mathf.FloorToInt(Attack * GameManager.CalculateElements(this.charElement, enemy.charElement)), this);
                Exp++;
            }
            else { ActionPoints += attackCost; }
            timer = Time.time + delay;
        }
    }
}
