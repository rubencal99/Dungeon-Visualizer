using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CorridorNode
{
    public List<TileNode> tileList = new List<TileNode>();
    public List<RoomNode> TargetRoomList = new List<RoomNode>();
    public List<RoomNode> ConnectedRoomList = new List<RoomNode>();

    public CorridorNode()
    {
        tileList = new List<TileNode>();
        TargetRoomList = new List<RoomNode>();
        ConnectedRoomList = new List<RoomNode>();
}

    public void AdjustConnectedRooms()
    {

    }

    public void MergeCorridors(CorridorNode c2)
    {
        //A.Union<int>(B).ToList<int>()
        //tileList = tileList.Union<TileNode>(c2).ToList<TileNode>();
        tileList.AddRange(c2.tileList);
        tileList = tileList.Distinct().ToList();
    }

    public bool IsBorderingRoom(Map map)
    {
        foreach (TileNode tile in tileList)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (map.IsInMapRange(tile.x + x, tile.y + y))
                    {
                        TileNode check = map.map[tile.x + x, tile.y + y];
                        if (check.value == 1 && check.room != TargetRoomList[0] && check.room != TargetRoomList[1])
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public void NullifyCorridor(Map map)
    {
        if (this == null)
        {
            Debug.Log("Corridor nullified early");
            return;
        }
        foreach (TileNode tile in tileList)
        {
            //MapGenerator.map[tile.x, tile.y].corridors.Remove(this);
            //if (MapGenerator.map[tile.x, tile.y].corridors.Count > 0)
            map.map[tile.x, tile.y].value = 0;
            tile.corridors.Remove(this);
        }
        //Debug.Log("Corridor nullified");
    }
}