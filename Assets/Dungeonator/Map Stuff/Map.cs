using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map
{
    public TileNode[,] map;

    // Queues for the BSP algorithm
    public List<TileNode> roomTiles = new List<TileNode>();
    public Queue<Tuple<int[], string>> roomsList = new Queue<Tuple<int[], string>>();

    public List<RoomNode> Rooms = new List<RoomNode>();
    public List<CorridorNode> Corridors = new List<CorridorNode>();

    public bool debugPartition = false;

    public int columns = 100;
    public int rows = 100;

    public RoomNode StartRoom = null;
    public RoomNode EndRoom = null;
    public RoomNode ShopRoom = null;
    public RoomNode DoorRoom = null;

    public bool HasEntry = false;
    public bool hasBoss = false;
    public bool isBossLevel = false;
    public int numLargeRooms;
    public int NumBossRooms = 1;
    public int NumNormalRooms;
    public int NumRewardRooms = 0;
    public int MaxRewardRooms = 8;

    public bool forceEntry = false;
    public bool extraRooms = false;
    public int numExtraRooms = 0;
    public int maxExtraRooms = 4;

    public int rewardPercent = 100;
    public int auxiliaryPercent = 30;

    // Min dimensions of rooms
    public Vector2Int minRoomDim;
    public Vector2Int minEntryDim;
    public Vector2Int maxEntryDim;

    public Vector2Int minNormalDim;
    public Vector2Int maxNormalDim;

    public Vector2Int minLargeDim;
    public Vector2Int maxLargeDim;

    public Vector2Int minRewardDim;
    public Vector2Int maxRewardDim;

    public Vector2Int minBossDim;
    public Vector2Int maxBossDim;



    /*public static void NewMap(int c, int r)
    {
        ClearValues();

        columns = c; rows = r;
        map = new TileNode[columns, rows];
        roomTiles = new List<TileNode>();
        Rooms = new List<RoomNode>();
        Corridors = new List<CorridorNode>();

        FillMap();
    }*/

    void SetVariables()
    {
        columns = Settings.columns; rows = Settings.rows;
        map = new TileNode[columns, rows];
        roomTiles = new List<TileNode>();
        Rooms = new List<RoomNode>();
        Corridors = new List<CorridorNode>();

        isBossLevel = Settings.isBossLevel;
    }

    public Map()
    {
        ClearValues();
        SetVariables();

        FillMap();
    }

    void FillMap()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                map[x, y] = new TileNode();
                map[x, y].x = x;
                map[x, y].y = y;
            }
        }
    }

    void ClearValues()
    {
        //queue.Clear();
        roomsList.Clear();
        roomTiles.Clear();
        Rooms.Clear();
        Corridors.Clear();

        numExtraRooms = 0;
        NumRewardRooms = 0;

        //HasEntry = false;
        //HasBoss = false;
        StartRoom = null;
        EndRoom = null;
        ShopRoom = null;
        DoorRoom = null;

        /*foreach (Transform room in DungeonRooms.transform)
            Destroy(room.gameObject);
        */
        if (map != null)
            Array.Clear(map, 0, map.Length);
    }

    public void ClearCorridors()
    {
        foreach(CorridorNode corridor in Corridors)
        {
            corridor.NullifyCorridor(this);
        }
        Corridors.Clear();
        foreach(RoomNode room in Rooms)
        {
            room.RemoveCorridors();
        }
    }

    public void ShuffleRooms()
    {
        Debug.Log("Shuffling Rooms");
        int n = Rooms.Count;
        for(int i = 0; i < n; i++)
        {
            int r = i + Random.Range(0, n - i);

            RoomNode temp = Rooms[r];
            Rooms[r] = Rooms[i];
            Rooms[i] = temp;
        }
    }

    // Checks to see if [x, y] is in map
    public bool IsInMapRange(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    public bool IsMapBorder(int x, int y)
    {
        return x == 0 || x == columns || y == 0 || y == rows;
    }
}
