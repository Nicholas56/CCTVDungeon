using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonObject : MonoBehaviour
{
    public string charName;

    public float charSpeed;
    public float attackRange;

    public int maxHealth;
    int health;
    public int attack;
    public int attackCost;
    public int maxActionPoints;
    int actionPoints;

    public int level;
    public int exp {get { return exp; }
        set { level = Mathf.FloorToInt(exp / (level * 100)); } }
    public int essence { get { return level / 100; } }

    public float checkRadius;
    public LayerMask checkFor;

    public GameManager.element charElement;

    public DungeonObject enemy;
    public List<DungeonObject> enemyList = new List<DungeonObject>();

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        actionPoints = maxActionPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public virtual void Fight()
    {
        //If enemy is in range and enough action points, attack can occur
        if(Vector3.Distance(transform.position, enemy.transform.position) < attackRange)
        {
            actionPoints -= attackCost;
            if (actionPoints > 0)
            {
                //Applies elemental advantages to attacks
                enemy.TakeDamage(Mathf.FloorToInt(attack * GameManager.CalculateElements(this.charElement,enemy.charElement)), this);
                exp++;
            }
            else { actionPoints += attackCost; }
        }
        else
        {
            //Will move towards enemy at own speed
            Vector3 direction = (enemy.transform.position - transform.position).normalized;
            transform.Translate(direction * charSpeed * Time.deltaTime);
        }
    }
}
