using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreCubes : MonoBehaviour
{
    public GameObject CubePrefabs;
    
    

    // Update is called once per frame
    public void Cube()
    {
        Instantiate(CubePrefabs);        
    }
}
