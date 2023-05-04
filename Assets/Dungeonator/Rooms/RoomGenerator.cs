using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Takes Map class, adds rooms to TileNode[,], sorts them
public static class RoomGenerator
{
    public static void AddRooms(Map map)
    {
        Queue<Tuple<int[], string>> roomsList = map.roomsList;

        // This is where we create the rooms
        int tempCount = 0;
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

            /*GameObject tempRoom = new GameObject(tempCount.ToString());
            tempRoom.AddComponent<RoomNode>();
            RoomNode NewRoom = tempRoom.GetComponent<RoomNode>();*/
            
            RoomNode NewRoom = new RoomNode();
            /*TestClass test = new TestClass();
            CorridorNode testC = new CorridorNode();
            Debug.Log("");
            Debug.Log("Test Class: " + test);
            Debug.Log("Test Corridor: " + testC);
            Debug.Log("New ROom after instantiation: " + NewRoom);*/
            

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

            if (!map.HasEntry &&
                roomType != "Reward" &&
                roomType != "Auxiliary")
            {
                NewRoom.RoomType = "Start";
                NewRoom.MaxNeighbors = 1;
                map.HasEntry = true;
                map.StartRoom = NewRoom;
                NewRoom.SetAccessibleFromStart();
                Debug.Log("Created start room");
            }
            /*else if (!HasBoss && roomsList.Count == 0)
            {
                NewRoom.RoomType = "Boss";
                NewRoom.MaxNeighbors = 2;
                HasBoss = true;
            }*/
            else
            {
                NewRoom.RoomType = roomType;
                NewRoom.MaxNeighbors = 3;
            }

            NewRoom.stringRepresentation = tempString;
            NewRoom.PreviousRoomType = NewRoom.RoomType;
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
                            map.map[x1 + i, y1 + j].value = result;
                            if (result == 1)
                            {
                                map.map[x1 + i, y1 + j].room = NewRoom;
                                map.roomTiles.Add(map.map[x1 + i, y1 + j]);
                                NewRoom.validTileList.Add(map.map[x1 + i, y1 + j]);
                            }
                            NewRoom.tileList[i, j] = map.map[x1 + i, y1 + j];
                            NewRoom.tileCount++;
                        }
                        else if (tempString[i, j] == "e")
                        {
                            NewRoom.Entrances.Add(new Vector2Int(x1 + i, y1 + j));
                            map.map[x1 + i, y1 + j].value = 1;
                            map.map[x1 + i, y1 + j].room = NewRoom;
                            map.roomTiles.Add(map.map[x1 + i, y1 + j]);
                            NewRoom.tileList[i, j] = map.map[x1 + i, y1 + j];
                            NewRoom.tileCount++;
                        }
                        else
                        {
                            map.map[x1 + i, y1 + j].value = 1;
                            map.map[x1 + i, y1 + j].obstacleValue = tempString[i, j];
                            map.map[x1 + i, y1 + j].isObstacle = true;
                            map.map[x1 + i, y1 + j].room = NewRoom;
                            map.roomTiles.Add(map.map[x1 + i, y1 + j]);
                            NewRoom.tileList[i, j] = map.map[x1 + i, y1 + j];
                            NewRoom.tileCount++;
                            NewRoom.obstacleTileList.Add(map.map[x1 + i, y1 + j]);

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
                        map.map[x1 + i + 1, y1 + j + 1].value = 1;
                        map.map[x1 + i + 1, y1 + j + 1].room = NewRoom;
                        map.roomTiles.Add(map.map[x1 + i + 1, y1 + j + 1]);
                        NewRoom.tileList[i, j] = map.map[x1 + i + 1, y1 + j + 1];
                        NewRoom.tileCount++;
                        NewRoom.validTileList.Add(map.map[x1 + i + 1, y1 + j + 1]);
                    }
                }
            }
            NewRoom.CalculateCenter();

            //NewRoom.gameObject.name = NewRoom.RoomType;
            //NewRoom.transform.position = new Vector3(NewRoom.roomCenter.x, 0, NewRoom.roomCenter.y);
            ///NewRoom.transform.parent = DungeonRooms.transform;
            
            map.Rooms.Add(NewRoom);

            /*Debug.Log("map.Rooms.Count : " + map.Rooms.Count);
            Debug.Log("New Room Added: " + NewRoom.RoomType);
            Debug.Log("Room Center: " + NewRoom.roomCenter);
            Debug.Log("New Room Object: " + NewRoom);*/
            tempCount++;

            //StartCoroutine(Delay());
        }

        SortRooms(map);
    }

    /*public static void StringToTiles(RoomNode NewRoom, Map map)
    {
        for (int i = 0; i < NewRoom.length; i++)
        {
            for (int j = 0; j < NewRoom.width; j++)
            {
                // Here is where we'd want to randomly choose from a static list
                if (int.TryParse(tempString[i, j], out int result))
                {
                    map.map[x1 + i, y1 + j].value = result;
                    if (result == 1)
                    {
                        map.map[x1 + i, y1 + j].room = NewRoom;
                        map.roomTiles.Add(map.map[x1 + i, y1 + j]);
                        NewRoom.validTileList.Add(map.map[x1 + i, y1 + j]);
                    }
                    NewRoom.tileList[i, j] = map.map[x1 + i, y1 + j];
                    NewRoom.tileCount++;
                }
                else if (tempString[i, j] == "e")
                {
                    NewRoom.Entrances.Add(new Vector2Int(x1 + i, y1 + j));
                    map.map[x1 + i, y1 + j].value = 1;
                    map.map[x1 + i, y1 + j].room = NewRoom;
                    map.roomTiles.Add(map.map[x1 + i, y1 + j]);
                    NewRoom.tileList[i, j] = map.map[x1 + i, y1 + j];
                    NewRoom.tileCount++;
                }
                else
                {
                    map.map[x1 + i, y1 + j].value = 1;
                    map.map[x1 + i, y1 + j].obstacleValue = tempString[i, j];
                    map.map[x1 + i, y1 + j].isObstacle = true;
                    map.map[x1 + i, y1 + j].room = NewRoom;
                    map.roomTiles.Add(map.map[x1 + i, y1 + j]);
                    NewRoom.tileList[i, j] = map.map[x1 + i, y1 + j];
                    NewRoom.tileCount++;
                    NewRoom.obstacleTileList.Add(map.map[x1 + i, y1 + j]);

                }
            }
        }
    }*/

    // This function sorts all Rooms according to their distance from eachother
    // This helps with optimizing corridor creation
    public static void SortRooms(Map map)
    {
        //Debug.Log("In SortRooms");
        //Debug.Log("Rooms count: " + map.Rooms.Count);
        // Go through every room
        foreach (RoomNode room in map.Rooms)
        {
            //Debug.Log("In SortRooms outer for loop");
            // Go through all other rooms
            foreach (RoomNode neighbor in map.Rooms)
            {
                //Debug.Log("In SortRooms for inner loop");
                // Not including itself
                if (neighbor == room)
                {
                    //Debug.Log("Neighbor == Room");
                    continue;
                }

                // Create a sorted list for every room of all their neighbors ranked by distance
                //Debug.Log("Before Sort by distance");
                SortByDistance(room, neighbor);
            }
        }
    }

    // This function goes through current list of rooms and inserts room in question accordingly
    public static void SortByDistance(RoomNode room, RoomNode neighbor)
    {
        //Debug.Log("Room Center: " + room.roomCenter);
        //Debug.Log("Neighbor Center: " + neighbor.roomCenter);
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

    public static void AddEndRoom(Map map)
    {
        int distance = 0;
        RoomNode end = new RoomNode();
        foreach (RoomNode room in map.Rooms)
        {
            if (room.DistanceFromStart > distance &&
                room.RoomType != "Reward" &&
                room.RoomType != "Extra")
            {
                distance = room.DistanceFromStart;
                end = room;
            }
        }
        map.EndRoom = end;
        if (map.hasBoss)
        {
            map.EndRoom.RoomType = "Boss";
        }
        else
        {
            map.EndRoom.RoomType = "Key";
        }
        map.EndRoom.RepurposeRoom(ref map);
        map.SpecialRooms.Add(map.EndRoom);

        //---------------- SHOP ----------------
        RoomNode shop = null;

        foreach (RoomNode room in map.Rooms)
        {
            if (!map.forceEntry && room.DistanceFromStart > 2 &&
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
        map.ShopRoom = shop;
        map.ShopRoom.RepurposeRoom(ref map);
        map.SpecialRooms.Add(map.ShopRoom);
        //---------------- DOOR ----------------
        if (!map.isBossLevel)
        {
            RoomNode door = null;
            for (int i = 0; i < map.StartRoom.RoomsByDistance.Count; i++)
            {
                if (map.StartRoom.RoomsByDistance[i].RoomType != "Start" &&
                    map.StartRoom.RoomsByDistance[i].RoomType != "Reward" &&
                    map.StartRoom.RoomsByDistance[i].RoomType != "Shop" &&
                    map.StartRoom.RoomsByDistance[i].RoomType != "Boss" &&
                    map.StartRoom.RoomsByDistance[i].RoomType != "Key" &&
                    map.StartRoom.RoomsByDistance[i].RoomType != "Extra")
                {
                    door = map.StartRoom.RoomsByDistance[i];
                }
            }
            door.RoomType = "Door";
            map.DoorRoom = door;
            map.DoorRoom.RepurposeRoom(ref map);
            map.SpecialRooms.Add(map.DoorRoom);
        }

        //Debug.Log("Map: " + map);
        //Debug.Log("Map Rooms length: " + map.Rooms.Count);
        /*foreach (RoomNode room in map.Rooms)
            room.transform.name = room.RoomType;*/
    }
}
