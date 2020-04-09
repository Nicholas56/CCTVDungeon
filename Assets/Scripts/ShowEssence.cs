using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowEssence : MonoBehaviour
{

    public Text essence;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        essence.text = "Total Essence: " + GameManager.dungeonEssence.ToString("n2");
    }
}
