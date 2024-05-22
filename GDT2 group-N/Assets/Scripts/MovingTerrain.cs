 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTerrain : MonoBehaviour
{
    [SerializeField] Transform terrain;
    private float timer = 5;
    private bool terrainUp = true;
    

    void Update()
    {

        if(terrainUp == true)
        {
            timer -= Time.deltaTime;
            terrain.position = new Vector3(-5.030634f, 5.525137f, 3.337861f);
        }
        
        if(timer <= 0)
        {
            terrainUp = false;
            
        }

        if(timer <= -3)
        {
            terrainUp = true;
            timer = 5;
        }

        if(terrainUp == false) 
        {
            
            terrainUp = true;
            terrain.position = new Vector3(0, -20, 0);
        }
    }
}
