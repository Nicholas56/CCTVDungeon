using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScreens : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    public Camera camera3;
    public Camera camera4;
    public Camera camera5;
    public Camera camera6;

    public RenderTexture camTex1;
    public RenderTexture camTex2;
    public RenderTexture camTex3;
    public RenderTexture camTex4;
    public RenderTexture camTex5;
    public RenderTexture camTex6;
    // Start is called before the first frame update
    void Start()
    {
        if(camera1&&camTex1)
            camera1.targetTexture = camTex1;
        if (camera2 && camTex2)
            camera2.targetTexture = camTex2;
        if (camera3 && camTex3)
            camera3.targetTexture = camTex3;
        if (camera4 && camTex4)
            camera4.targetTexture = camTex4;
        if (camera5 && camTex5)
            camera5.targetTexture = camTex5;
        if (camera6 && camTex6)
            camera6.targetTexture = camTex6;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
