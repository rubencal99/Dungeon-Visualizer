using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon : MonoBehaviour
{
    TileNode[,] map;
    public MapGenerator BSP;

    public bool trim;
    public bool connect;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
        Debug.Log("After Generate Map");
        //Physics.SyncTransforms();
        //AstarPath.active.Scan();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            GenerateMap();
            Debug.Log("New Map Generated");
        }*/
    }

    public void GenerateMap()
    {
        map = BSP.GenerateMap();
    }
}