using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Nicholas Easterby - EAS12337350
//For each dungeon object, displays health and action points

public class DisplayHealth : MonoBehaviour
{
    public DungeonObject character;
    public Image healthBar;
    public Image actionBar;
    float timer = -1f;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.fillAmount = (float)character.GetHealth() / character.maxHealth;
        actionBar.fillAmount = (float)character.GetAction() / character.maxActionPoints;
    }

    // Update is called once per frame
    void Update()
    {
        //Every two seconds, updates the values of health and action points as percentage of maximum
        if (timer < Time.time)
        {
            healthBar.fillAmount = (float)character.GetHealth() / character.maxHealth;
            actionBar.fillAmount = (float)character.GetAction() / character.maxActionPoints;
            timer = Time.time + 2f;
        }
    }
}
