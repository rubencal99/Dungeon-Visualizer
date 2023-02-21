using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles every tile in our map
// We can expand it to include special tiles ie environmental hazards
public class TileNode
{
    public RoomNode room;
    public List<CorridorNode> corridors;
    // 0 == empty space / out of map
    // 1 = room
    // 2 = corridor
    public int value;
    public string obstacleValue;
    // Gym, lab, lobby, rec, cafeterti, etc.
    public string roomType;
    // Gym: weights, more weights
    // Lab: surgery equipment, eperiment vessels
    // Cafeteria: Lunch tables, trash cans, self-serve area
    public Dictionary<string, List<int>> roomDecorations;
    public int x;
    public int y;
    // public bool isDoor;
    public bool isObstacle = false;

    public Vector3Int visual;

    public TileNode()
    {
        value = 0;
        obstacleValue = "";
        corridors = new List<CorridorNode>();
    }

    public TileNode(int tileX, int tileY)
    {
        x = tileX;
        y = tileY;
        value = 0;
        corridors = new List<CorridorNode>();
    }


    // Start is called before the first frame update
    void Start()
    {
        value = 0;
    }
}