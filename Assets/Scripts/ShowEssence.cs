using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//Nicholas Easterby - EAS12337350
//Shows essence value in text form

public class ShowEssence : MonoBehaviour
{

    public TMP_Text essence;

    void Update()
    {
        //Displays total essence next to value shown to 2 decimal places
        essence.text = "Total Essence: " + GameManager.dungeonEssence.ToString("n2");
    }
}
