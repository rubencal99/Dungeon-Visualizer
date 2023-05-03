using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;
using UnityEditor;

public static class MapGenerator
{
    /*public static MapGenerator instance;
    void Start(){ instance = this; }

    [SerializeField]
    private GameObject Grid;
    [SerializeField]
    private GameObject DungeonRooms;
    [SerializeField]
    private bool StepThrough;
    [SerializeField]
    private bool debug;
    [SerializeField]
    public bool debugPartition;
    [SerializeField]
    private bool forceEntry = false;
    [SerializeField]
    private bool extraRooms = false;
    [SerializeField]
    private int numExtraRooms = 0;
    [SerializeField]
    private int maxExtraRooms = 4;
    [SerializeField]
    public bool hasBoss;
    [SerializeField]
    private TileSpritePlacer AutoTiler;
    [SerializeField]
    public GameObject Exit;
    [SerializeField]
    public GameObject SpawnWeapon;

    [SerializeField]
    private GameObject wallVertical, wallHorizontal;
    //[SerializeField]
    //public placecontrols controls;
    public int columns;
    public int rows;
    */
    //public static TileNode[,] map;
    public static Map Map;

    // Min dimensions of rooms
    /*public Vector2Int minRoomDim;
    public Vector2Int minEntryDim;
    public Vector2Int maxEntryDim;

    public Vector2Int minNormalDim;
    public Vector2Int maxNormalDim;

    public Vector2Int minLargeDim;
    public Vector2Int maxLargeDim;

    public Vector2Int minRewardDim;
    public Vector2Int maxRewardDim;
    [Range(0, 100)]
    public int rewardPercent;
    [Range(0, 100)]
    public int auxiliaryPercent;

    public Vector2Int minBossDim;
    public Vector2Int maxBossDim;
    */
    /*public bool UseCellular;
    public string Seed;
    public bool useRandomSeed;
    [Range(0, 100)]
    public int wallPercent;
    public int wallCount;
    
    public int Iterations;
    public int overgrownRooms;
    */

    /*public bool HasEntry = false;
    //public bool HasBoss = false;
    public int numLargeRooms;
    public int NumBossRooms = 1;
    public int NumNormalRooms;
    public int NumRewardRooms = 0;
    public int MaxRewardRooms = 8;


    // Queues for the BSP algorithm
    private List<int[]> queue = new List<int[]>();
    private Queue<Tuple<int[], string>> roomsList = new Queue<Tuple<int[], string>>();
    private List<TileNode> roomTiles = new List<TileNode>();
    private List<RoomNode> Rooms = new List<RoomNode>();
    private List<CorridorNode> Corridors = new List<CorridorNode>();

    //private AstarPath AStar;

    private RoomNode StartRoom;
    private RoomNode EndRoom;
    private RoomNode ShopRoom;
    private RoomNode DoorRoom;*/

    public static Map GenerateMap()
    {
        Debug.Log("////// STARTING FULL MAP GENERATION //////");
        // Creates new Map using user-inputted variables from Settings
        Map = new Map();

        // BSP to split map and create the roomsList that we'll use to inject rooms into

        BinarySpace.BinarySpaceSplit(ref Map);
        Debug.Log("BSP Complete");

        RoomGenerator.AddRooms(Map);
        Debug.Log("Add Rooms Complete");

        CorridorGenerator.AddCorridors(Map);
        Debug.Log("Add Corridors Complete");

        RoomGenerator.AddEndRoom(Map);
        Debug.Log("Add End Room Complete");

        Debug.Log("Num Corridors: " + Map.Corridors.Count);
        Debug.Log("////// ENDING GENERATION //////");
        Debug.Log("");
        Debug.Log("");
        Debug.Log("");
        //map = Map.map;

        /*ClearValues();
        map = new TileNode[columns, rows];

        int count = 0;
        while (count < 3)
        {
            try
            {
                FillMap();
                BinarySpace();
                break;
            }
            catch (System.IndexOutOfRangeException exception)
            {
                Debug.Log("Exception in map generation: " + exception);
                count++;
            }

        }*/

        /*if (!debug)
        {
            DrawMap();
        }*/
        //Debug.Log("After draw map");
        //Grid.transform.Rotate(Vector3.right * 90);

        return Map;
    }

    public static Map GenerateRooms()
    {
        Debug.Log("////// STARTING ROOM GENERATION //////");
        // Creates new Map using user-inputted variables from Settings
        Map = new Map();

        // BSP to split map and create the roomsList that we'll use to inject rooms into
        BinarySpace.BinarySpaceSplit(ref Map);
        Debug.Log("BSP Complete");

        RoomGenerator.AddRooms(Map);
        Debug.Log("Add Rooms Complete");
        

        Debug.Log("Num Corridors: " + Map.Corridors.Count);
        Debug.Log("////// ENDING ROOM GENERATION //////");
        Debug.Log("");
        Debug.Log("");
        Debug.Log("");

        return Map;
    }

    public static Map GenerateCorridors()
    {
        Debug.Log("////// STARTING CORRIDOR GENERATION //////");

        CorridorGenerator.EraseCorridors(Map);
        Debug.Log("Removed all corridors");

        CorridorGenerator.AddCorridors(Map);
        Debug.Log("Add Corridors Complete");

        RoomGenerator.AddEndRoom(Map);
        Debug.Log("Add End Room Complete");

        Debug.Log("Num Corridors: " + Map.Corridors.Count);
        Debug.Log("////// ENDING CORRIDOR GENERATION //////");
        Debug.Log("");
        Debug.Log("");
        Debug.Log("");

        return Map;
    }
    /*void DrawMap()
    {
        AutoTiler.Clear();
        AutoTiler.PaintFloorTiles(roomTiles);
        AutoTiler.PaintCollisions(roomTiles, map);
    }*/
    // These 2 are in Map
    /*  void FillMap()
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
          queue.Clear();
          roomsList.Clear();
          roomTiles.Clear();
          Rooms.Clear();
          Corridors.Clear();

          numExtraRooms = 0;
          NumRewardRooms = 0;

          HasEntry = false;
          //HasBoss = false;
          StartRoom = null;
          EndRoom = null;
          ShopRoom = null;
          DoorRoom = null;

          foreach(Transform room in DungeonRooms.transform)
              Destroy(room.gameObject);

          if(map != null)
              Array.Clear(map, 0, map.Length);
      }*/

    //async void BinarySpace()
    //{
    /*                                                            // This is now in BinarySpace.BinarySpaceSplit
    // Instantiate first room space (whole map)
    var temp = new int[4];
    temp[0] = 0;
    temp[1] = 0;
    temp[2] = columns - 1;
    temp[3] = rows - 1;
    // This will be our queue of room spaces
    queue.Add(temp);

    int count = 0;
    // Main loop that splits map                            
    while (queue.Count > 0)
    {
        // print("Times looped: " + count);
        if (count > 200)
        {
            Debug.Log("Too many rooms created");
            break;
        }

        int[] space;
        if (HasEntry)
        {
            // note: This is not optimal, very slow
            space = queue[0];
            queue.RemoveAt(0);
        }
        else
        {
            space = queue[queue.Count - 1];
            queue.RemoveAt(queue.Count - 1);
        }
        var x1 = space[0];
        var y1 = space[1];
        var x2 = space[2];
        var y2 = space[3];

        var width = x2 - x1;
        var height = y2 - y1;

        var r = UnityEngine.Random.Range(0, 100);
        bool successfulLarge = false;
        bool successfulNormal = false;
        bool successfulReward = false;
        // This gives us larger rooms
        /*if (!successfulLarge)
        {
            if (width < maxLargeDim.x && height < maxLargeDim.y)
            {
                // Randomly choose which split we prefer
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    if (height >= maxNormalDim.y * 2)
                    {
                        SplitHorizontal(space);
                    }
                    else if (width >= maxNormalDim.x * 2)
                    {
                        SplitVertical(space);
                    }
                    else
                    {
                        roomsList.Enqueue(new Tuple<int[], string>(space, "Large"));
                    }
                }
                else
                {
                    if (width >= maxNormalDim.x * 2)
                    {
                        SplitVertical(space);
                    }
                    else if (height >= maxNormalDim.y * 2)
                    {
                        SplitHorizontal(space);
                    }
                    else
                    {
                        roomsList.Enqueue(new Tuple<int[], string>(space, "Large"));
                    }
                }
                successfulLarge = true;
            }
        }*/
    // This gives us normal rooms
    /*           if (!successfulLarge)
               {
                   if (width > maxNormalDim.x && height > maxNormalDim.y)
                   {
                       //Debug.Log("In BSP Split normal");
                       //Debug.Log("Space Width: " + width);
                       //Debug.Log("Space Height: " + height);
                       // Randomly choose which split we prefer
                       if (UnityEngine.Random.Range(0, 100) < 50)
                       {
                           if (height >= maxNormalDim.y * 2)
                           {
                               SplitHorizontal(space);
                           }
                           else if (width >= maxNormalDim.x * 2)
                           {
                               SplitVertical(space);
                           }
                           else
                           {
                               if (UnityEngine.Random.Range(0, 100) > auxiliaryPercent)
                               {
                                   //Debug.Log("Decided to create room");
                                   roomsList.Enqueue(new Tuple<int[], string>(space, "Normal"));
                               }
                               else
                               {
                                   roomsList.Enqueue(new Tuple<int[], string>(space, "Auxiliary"));
                               }
                           }
                       }
                       else
                       {
                           if (width >= maxNormalDim.x * 2)
                           {
                               SplitVertical(space);
                           }
                           else if (height >= maxNormalDim.y * 2)
                           {
                               SplitHorizontal(space);
                           }
                           else
                           {
                               if (UnityEngine.Random.Range(0, 100) > auxiliaryPercent)
                               {
                                   //Debug.Log("Decided to create room");
                                   roomsList.Enqueue(new Tuple<int[], string>(space, "Normal"));
                               }
                               else
                               {
                                   roomsList.Enqueue(new Tuple<int[], string>(space, "Auxiliary"));
                               }
                           }
                       }
                       successfulNormal = true;
                   }
               }
               // THis gives us reward rooms

               if (!successfulLarge && !successfulNormal)
               {
                   if (width > maxRewardDim.x && height > maxRewardDim.y && NumRewardRooms < MaxRewardRooms)
                   {
                       // Randomly choose which split we prefer
                       if (UnityEngine.Random.Range(0, 100) < 50)
                       {
                           if (height >= maxRewardDim.y * 2)
                           {
                               SplitHorizontal(space);
                           }
                           else if (width >= maxRewardDim.x * 2)
                           {
                               SplitVertical(space);
                           }
                           else if (UnityEngine.Random.Range(0, 100) < rewardPercent)
                           {
                               roomsList.Enqueue(new Tuple<int[], string>(space, "Reward"));
                               NumRewardRooms++;
                           }
                       }
                       else
                       {
                           if (width >= maxRewardDim.x * 2)
                           {
                               SplitVertical(space);
                           }
                           else if (height >= maxRewardDim.y * 2)
                           {
                               SplitHorizontal(space);
                           }
                           else if (UnityEngine.Random.Range(0, 100) < rewardPercent)
                           {
                               roomsList.Enqueue(new Tuple<int[], string>(space, "Reward"));
                               NumRewardRooms++;
                           }
                       }
                       successfulReward = true;
                   }
               }
               // This is where extra rooms are made
               if (!successfulReward && !successfulNormal)
               {
                   if (extraRooms && numExtraRooms < maxExtraRooms)
                   {
                       //Debug.Log("In Extra room creation");
                       roomsList.Enqueue(new Tuple<int[], string>(space, "Extra"));
                       numExtraRooms++;
                   }
               }
               count++;
           }
    */
    // This is where we create the rooms                    // This is now in RoomGenerator.AddRooms
    /*int tempCount = 0;
    Debug.Log("Room Count = " + roomsList.Count);
    while (roomsList.Count > 0)
    {
        (int[] room, string roomType) = roomsList.Dequeue();

        int x1 = (int)room[0];
        int y1 = (int)room[1];
        int x2 = (int)room[2];
        int y2 = (int)room[3];

        int length = (x2 - 1) - (x1 + 1);
        int width = (y2 - 1) - (y1 + 1);

        GameObject tempRoom = new GameObject(tempCount.ToString());
        tempRoom.AddComponent<RoomNode>();

        RoomNode NewRoom = tempRoom.GetComponent<RoomNode>();
        int roomIndex = -1;
        string[,] tempString = null;
        if (roomType == "Normal")
        {
            roomIndex = UnityEngine.Random.Range(0, NormalRooms.RoomList.Count);
            tempString = (string[,])NormalRooms.RoomList[roomIndex];
            // Amount of arrays
            var roomLength = tempString.GetLength(0);
            // Array length
            var roomWidth = tempString.GetLength(1);
            NewRoom.AddDimensions(roomLength, roomWidth);
        }
        else if (roomType == "Auxiliary")
        {
            roomIndex = UnityEngine.Random.Range(0, AuxiliaryRooms.RoomList.Count);
            tempString = (string[,])AuxiliaryRooms.RoomList[roomIndex];
            // Amount of arrays
            var roomLength = tempString.GetLength(0);
            // Array length
            var roomWidth = tempString.GetLength(1);
            NewRoom.AddDimensions(roomLength, roomWidth);
        }
        else if (roomType == "Reward")
        {
            roomIndex = UnityEngine.Random.Range(0, RewardRooms.RoomList.Count);
            tempString = (string[,])RewardRooms.RoomList[roomIndex];
            // Amount of arrays
            var roomLength = tempString.GetLength(0);
            // Array length
            var roomWidth = tempString.GetLength(1);
            NewRoom.AddDimensions(roomLength, roomWidth);
        }
        else
        {
            // The -2 is to give a buffer of 1 tile on all sides of space
            NewRoom.AddDimensions(length - 2, width - 2);
        }

        if (!HasEntry &&
            roomType != "Reward" &&
            roomType != "Auxiliary")
        {
            NewRoom.RoomType = "Start";
            NewRoom.MaxNeighbors = 1;
            HasEntry = true;
            StartRoom = NewRoom;
            NewRoom.SetAccessibleFromStart();
            Debug.Log("Created start room");
        }
        /*else if (!HasBoss && roomsList.Count == 0)
        {
            NewRoom.RoomType = "Boss";
            NewRoom.MaxNeighbors = 2;
            HasBoss = true;
        }*/
    /*          else
              {
                  NewRoom.RoomType = roomType;
                  NewRoom.MaxNeighbors = 3;
              }

              // Here we fill the negative space with empty space 
              // I.e. room creation
              if (roomType == "Normal" || roomType == "Reward" || roomType == "Auxiliary")
              {
                  for (int i = 0; i < NewRoom.length; i++)
                  {
                      for (int j = 0; j < NewRoom.width; j++)
                      {
                          // Here is where we'd want to randomly choose from a static list
                          if (int.TryParse(tempString[i, j], out int result))
                          {
                              map[x1 + i, y1 + j].value = result;
                              if (result == 1)
                              {
                                  map[x1 + i, y1 + j].room = NewRoom;
                                  roomTiles.Add(map[x1 + i, y1 + j]);
                                  NewRoom.validTileList.Add(map[x1 + i, y1 + j]);
                              }
                              NewRoom.tileList[i, j] = map[x1 + i, y1 + j];
                              NewRoom.tileCount++;
                          }
                          else if (tempString[i, j] == "e")
                          {
                              NewRoom.Entrances.Add(new Vector2Int(x1 + i, y1 + j));
                              map[x1 + i, y1 + j].value = 1;
                              map[x1 + i, y1 + j].room = NewRoom;
                              roomTiles.Add(map[x1 + i, y1 + j]);
                              NewRoom.tileList[i, j] = map[x1 + i, y1 + j];
                              NewRoom.tileCount++;
                          }
                          else
                          {
                              map[x1 + i, y1 + j].value = 1;
                              map[x1 + i, y1 + j].obstacleValue = tempString[i, j];
                              map[x1 + i, y1 + j].isObstacle = true;
                              map[x1 + i, y1 + j].room = NewRoom;
                              roomTiles.Add(map[x1 + i, y1 + j]);
                              NewRoom.tileList[i, j] = map[x1 + i, y1 + j];
                              NewRoom.tileCount++;
                              NewRoom.obstacleTileList.Add(map[x1 + i, y1 + j]);

                          }
                      }
                  }
              }
              else
              {
                  for (int i = 0; i < NewRoom.length; i++)
                  {
                      for (int j = 0; j < NewRoom.width; j++)
                      {
                          map[x1 + i + 1, y1 + j + 1].value = 1;
                          map[x1 + i + 1, y1 + j + 1].room = NewRoom;
                          roomTiles.Add(map[x1 + i + 1, y1 + j + 1]);
                          NewRoom.tileList[i, j] = map[x1 + i + 1, y1 + j + 1];
                          NewRoom.tileCount++;
                          NewRoom.validTileList.Add(map[x1 + i + 1, y1 + j + 1]);
                      }
                  }
              }
              NewRoom.CalculateCenter();
              NewRoom.gameObject.name = NewRoom.RoomType;
              NewRoom.transform.position = new Vector3(NewRoom.roomCenter.x, 0, NewRoom.roomCenter.y);
              NewRoom.transform.parent = DungeonRooms.transform;
              Rooms.Add(NewRoom);
              tempCount++;

              StartCoroutine(Delay());
          }*/
    /*
          SortRooms();
          AddCorridors();
          AddEndRoom();
          return;
      }*/

    /*IEnumerator Delay()
    {
        this.enabled = false;
        Debug.Log("Before yield");
        while(!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        Debug.Log("After yield");
        this.enabled = true;
    }*/

    /*
    void AddObstacles()
    {
        foreach(RoomNode room in Rooms)
        {
            if (room.RoomType == "Start" || 
                room.RoomType == "Shop" || 
                room.RoomType == "Boss" ||
                room.RoomType == "Normal" ||
                room.RoomType == "Key" ||
                room.RoomType == "Door" ||
                room.RoomType == "Auxiliary" ||
                room.RoomType == "Reward")
            {
                continue;
            }
            //ObstacleInjector.PlaceObstacles(room);
            ObstacleInjector.PlacePillars(room);
        }
    }*/

    /* RoomNode CreateRoom(int x1, int y1, int x2, int y2)
    {
        RoomNode New = new RoomNode("Normal");

        int area = (x2 - x1) * (y2 - y1);
        // if (area)

        return New;
    }*/

    // This function sorts all Rooms according to their distance from eachother
    // This helps with optimizing corridor creation
    /* void SortRooms()
     {
         // Go through every room
         foreach (RoomNode room in Rooms)
         {
             // Go through all other rooms
             foreach (RoomNode neighbor in Rooms)
             {
                 // Not including itself
                 if (neighbor == room)
                 {
                     continue;
                 }

                 // Create a sorted list for every room of all their neighbors ranked by distance
                 SortByDistance(room, neighbor);
             }
         }
     }

     // This function goes through current list of rooms and inserts room in question accordingly
     void SortByDistance(RoomNode room, RoomNode neighbor)
     {
         for (int i = 0; i < room.RoomsByDistance.Count; i++)
         {
             var check = room.RoomsByDistance[i];
             var distance1 = Vector2.Distance(room.roomCenter, check.roomCenter);
             var distance2 = Vector2.Distance(room.roomCenter, neighbor.roomCenter);
             if (distance2 < distance1)
             {
                 room.RoomsByDistance.Insert(i, neighbor);
                 return;
             }
         }
         // If neighbor is furthest away out of all compared so far add it to back of list
         room.RoomsByDistance.Add(neighbor);
     }

     void AddEndRoom()
     {
         int distance = 0;
         RoomNode end = new RoomNode();
         foreach (RoomNode room in Rooms)
         {
             if (room.DistanceFromStart > distance &&
                 room.RoomType != "Reward" &&
                 room.RoomType != "Extra")
             {
                 distance = room.DistanceFromStart;
                 end = room;
             }
         }
         EndRoom = end;
         if (hasBoss)
         {
             EndRoom.RoomType = "Boss";
         }
         else
         {
             EndRoom.RoomType = "Key";
         }
         EndRoom.RepurposeRoom(ref map, ref roomTiles);


         //---------------- SHOP ----------------
         RoomNode shop = null;

         foreach (RoomNode room in Rooms)
         {
             if (!forceEntry && room.DistanceFromStart > 2 &&
                 room.RoomType != "Reward" &&
                 room.RoomType != "Key" &&
                 room.RoomType != "Boss" &&
                 room.RoomType != "Door")
             {
                 shop = room;
             }
             else if (room.RoomType != "Reward" &&
                 room.RoomType != "Key" &&
                 room.RoomType != "Boss" &&
                 room.RoomType != "Door")
             {
                 shop = room;
             }
         }
         shop.RoomType = "Shop";
         ShopRoom = shop;
         ShopRoom.RepurposeRoom(ref map, ref roomTiles);
         //---------------- DOOR ----------------
         if (!hasBoss)
         {
             RoomNode door = null;
             for (int i = 0; i < StartRoom.RoomsByDistance.Count; i++)
             {
                 if (StartRoom.RoomsByDistance[i].RoomType != "Start" &&
                     StartRoom.RoomsByDistance[i].RoomType != "Reward" &&
                     StartRoom.RoomsByDistance[i].RoomType != "Shop" &&
                     StartRoom.RoomsByDistance[i].RoomType != "Boss" &&
                     StartRoom.RoomsByDistance[i].RoomType != "Key" &&
                     StartRoom.RoomsByDistance[i].RoomType != "Extra")
                 {
                     door = StartRoom.RoomsByDistance[i];
                 }
             }
             door.RoomType = "Door";
             DoorRoom = door;
             DoorRoom.RepurposeRoom(ref map, ref roomTiles);
         }

         foreach(RoomNode room in Rooms)
             room.transform.name = room.RoomType;
     }*/

    /*void AddCorridors()                                                                   // These next 4 are now in CorridorGenerator
    {
        foreach (RoomNode room in Rooms)
        {
            foreach (RoomNode neighbor in room.RoomsByDistance)
            {
                ConnectRooms(room, neighbor, forceEntry);
                //StartCoroutine(Delay());
            }
        }

        // Second pass in case rooms or sections aren't accessible from start
        // NOTE: This is what creates doubly-connected rooms
        /*foreach (RoomNode room in StartRoom.RoomsByDistance)
        {
            if (room.isAccessibleFromStart)
            {
                continue;
            }
            foreach (RoomNode neighbor in room.RoomsByDistance)
            {
                ConnectRooms(room, neighbor, true);
            }
        }*/
    /*
            AddDistanceFromStart();
        }

        public void AddDistanceFromStart()
        {
            StartRoom.DistanceFromStart = 0;
            Queue<RoomNode> roomsToSearch = new Queue<RoomNode>();
            roomsToSearch.Enqueue(StartRoom);
            while (roomsToSearch.Count > 0)
            {
                RoomNode currentRoom = roomsToSearch.Dequeue();
                foreach (RoomNode room in currentRoom.NeighborRooms)
                {
                    if (room.DistanceFromStart <= 0 && room != StartRoom)
                    {
                        room.DistanceFromStart = currentRoom.DistanceFromStart + 1;
                        roomsToSearch.Enqueue(room);
                    }
                }
            }
        }

        bool RoomsIncompatible(RoomNode room, RoomNode neighbor)
        {
            // Checks if the two rooms are the start and a reward room
            if (room.RoomType == "Start" || room.RoomType == "Reward")
            {
                if (neighbor.RoomType == "Start" || neighbor.RoomType == "Reward")
                {
                    return true;
                }
            }
            return false;
        }
        void ConnectRooms(RoomNode room, RoomNode neighbor, bool forcedEntry = false)
        {
            if (RoomsIncompatible(room, neighbor))
            {
                return;
            }
            if (!forcedEntry)
            {
                if (room.NeighborCount >= room.MaxNeighbors)
                {
                    return;
                }
                if (neighbor.NeighborCount >= neighbor.MaxNeighbors)
                {
                    return;
                }
            }
            CorridorNode corridor = CorridorGenerator.CreateCorridor(room, neighbor, room.CenterTile, neighbor.CenterTile, ref Map);
            if (corridor == null)
            {
                corridor = CorridorGenerator.CreatePassage(room, neighbor, room.CenterTile, neighbor.CenterTile, ref Map);
            }
            if (corridor == null)
            {
                return;
            }

            Corridors.Add(corridor);
            RoomNode.ConnectRooms(room, neighbor);
        }
    */
    // Splits map horizontally
    /*   void SplitHorizontal(int[] roomSpace)
       {
           var x1 = roomSpace[0];
           var y1 = roomSpace[1];
           var x2 = roomSpace[2];
           var y2 = roomSpace[3];

           var buffer = (int)(y2 - y1) / 3;
           var ySplit = UnityEngine.Random.Range(y1 + 1 + buffer, y2 - buffer);
           var room1 = new int[] { x1, y1, x2, ySplit };
           var room2 = new int[] { x1, ySplit + 1, x2, y2 };

           if (debugPartition)
           {
               //Debug.DrawLine(new Vector3(x1, 4, ySplit), new Vector3(x2, 4, ySplit), Color.green, 5);
               Debug.DrawLine(new Vector3(x1, ySplit, 4), new Vector3(x2, ySplit, 4), Color.green, 25);
           }


           if (UnityEngine.Random.Range(0, 100) < 50)
           {
               queue.Add(room1);
               queue.Add(room2);
           }
           else
           {
               queue.Add(room2);
               queue.Add(room1);
           }
       }

       // Splits map vertically
       void SplitVertical(int[] roomSpace)
       {
           var x1 = roomSpace[0];
           var y1 = roomSpace[1];
           var x2 = roomSpace[2];
           var y2 = roomSpace[3];

           var buffer = (int)(x2 - x1) / 3;
           var xSplit = UnityEngine.Random.Range(x1 + 1 + buffer, x2 - buffer);
           var room1 = new int[] { x1, y1, xSplit, y2 };
           var room2 = new int[] { xSplit + 1, y1, x2, y2 };

           if (debugPartition)
           {
               //Debug.DrawLine(new Vector3(xSplit, 4, y1), new Vector3(xSplit, 4, y2), Color.green, 5);
               Debug.DrawLine(new Vector3(xSplit, y1, 4), new Vector3(xSplit, y2, 4), Color.green, 50);
           }

           // print("In vertical.");
           if (UnityEngine.Random.Range(0, 100) < 50)
           {
               queue.Add(room1);
               queue.Add(room2);
           }
           else
           {
               queue.Add(room2);
               queue.Add(room1);
           }

       }
    */

    static void OnDrawGizmos()
    {
        if (Map == null || Map.map == null)
            return;

        TileNode[,] map = Map.map;
        int columns = Settings.columns;
        int rows = Settings.rows;


        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Debug.Log("(x, y) = (" + x + ", " + y+ ")");
                if (map[x, y].value == 0 || map[x, y].isObstacle)
                {
                    Gizmos.color = new Color(0, 0, 0, 1f);
                }
                else if (map[x, y].value == 1 && map[x, y].room != null)
                {
                    if (map[x, y].room.RoomType == "Start")
                    {
                        Gizmos.color = new Color(0, 255, 0, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Boss" || map[x, y].room.RoomType == "Key")
                    {
                        Gizmos.color = new Color(255, 0, 0, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Door")
                    {
                        Gizmos.color = new Color(155 / 255f, 0, 0, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Shop")
                    {
                        Gizmos.color = new Color(210 / 255f, 105 / 255f, 30 / 255f, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Reward")
                    {
                        Gizmos.color = new Color(0, 0, 255, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Auxiliary")
                    {
                        Gizmos.color = new Color(130 / 255f, 115 / 255f, 150 / 255f, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Extra")
                    {
                        Gizmos.color = new Color(150 / 255f, 150 / 255f, 0, 1f);
                    }
                    else
                    {
                        Gizmos.color = new Color(255, 255, 0, 1f);
                    }

                }
                else
                {
                    Gizmos.color = new Color(0, 255, 255, 1f);
                }

                //Gizmos.DrawCube(new Vector3(x, 0, y), new Vector3(1, 1, 1));
                Gizmos.DrawCube(new Vector3(x, y, 0), new Vector3(1, 1, 1));
            }
        }
    }
}