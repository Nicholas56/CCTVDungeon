using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum element { Fire, Water, Earth, Wind}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static float CalculateElements(element element1, element element2)
    {
        if (element1 == element.Fire)
        {
            if (element2 == element.Water){return 2f;}
            else if(element2 == element.Earth){return 2f;}
            else if(element2 == element.Wind){return 0.25f;}
            else { return 0.5f; }
        }
        else if(element1 == element.Earth)
        {
            if (element2 == element.Water){return 0.25f;}
            else if (element2 == element.Earth){return 0.5f;}
            else if (element2 == element.Wind){return 2f;}
            else { return 0.25f; }
        }
        else if (element1 == element.Wind)
        {
            if (element2 == element.Water) { return 2f; }
            else if (element2 == element.Earth){return 0.25f;}
            else if (element2 == element.Wind){return 0.5f;}
            else { return 2f; }

        }
        else if (element1 == element.Water)
        {
            if (element2 == element.Water){return 0.5f;}
            else if (element2 == element.Earth){return 2f;}
            else if (element2 == element.Wind){return 0.25f;}
            else { return 2f; }

        }
        else { return 0f; }
    }
}
