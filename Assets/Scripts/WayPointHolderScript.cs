using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Followed tutorial from Brackeys: https://www.youtube.com/watch?v=aFxucZQ_5E4&feature=emb_rel_end
public class WayPointHolderScript : MonoBehaviour
{
    //The waypoints are accessible by all scripts
    public static Transform[] points;

    private void Awake()
    {
        //The points array is defined, then every child of this object are added to the array at the corresponding index point
        points = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i);
        }
    }
}
