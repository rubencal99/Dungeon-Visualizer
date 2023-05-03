using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CorridorGenerator
{
    public static int passageSize = 2;

    public static void EraseCorridors(Map map)
    {
        map.ClearCorridors();
    }
    public static void AddCorridors(Map map)
    {
        map.ShuffleRooms();
        //Debug.Log("Rooms count in AddCorridors: " + map.Rooms.Count);
        foreach (RoomNode room in map.Rooms)
        {
            //Debug.Log("Number of potential neighbors: " + room.RoomsByDistance.Count);
            foreach (RoomNode neighbor in room.RoomsByDistance)
            {
                ConnectRooms(map, room, neighbor, map.forceEntry);
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

        AddDistanceFromStart(map);
    }

    public static void AddDistanceFromStart(Map map)
    {
        map.StartRoom.DistanceFromStart = 0;
        Queue<RoomNode> roomsToSearch = new Queue<RoomNode>();
        roomsToSearch.Enqueue(map.StartRoom);
        while (roomsToSearch.Count > 0)
        {
            RoomNode currentRoom = roomsToSearch.Dequeue();
            foreach (RoomNode room in currentRoom.NeighborRooms)
            {
                if (room.DistanceFromStart <= 0 && room != map.StartRoom)
                {
                    room.DistanceFromStart = currentRoom.DistanceFromStart + 1;
                    roomsToSearch.Enqueue(room);
                }
            }
        }
    }

    public static bool RoomsIncompatible(RoomNode room, RoomNode neighbor)
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
    public static void ConnectRooms(Map map, RoomNode room, RoomNode neighbor, bool forcedEntry = false)
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
        CorridorNode corridor = CreateCorridor(room, neighbor, room.CenterTile, neighbor.CenterTile, ref map);
        if (corridor == null)
        {
            corridor = CreatePassage(room, neighbor, room.CenterTile, neighbor.CenterTile, ref map);
        }
        if (corridor == null)
        {
            return;
        }
        //Debug.Log("Corridor Added: " + corridor);
        map.Corridors.Add(corridor);
        RoomNode.ConnectRooms(room, neighbor);
    }
    public static CorridorNode CreateCorridor(RoomNode roomA, RoomNode roomB, TileNode tileA, TileNode tileB, ref Map map)
    {
        TileNode meetPoint;
        Tuple<Vector2Int, Vector2Int> entrances = FindClosestEntrances(roomA, roomB);
        Vector2Int currentRoomCenter = entrances.Item1;
        Vector2Int destination = entrances.Item2;
        var position = currentRoomCenter;
        CorridorNode corridor = new CorridorNode();
        CorridorNode c1;
        CorridorNode c2;
        // Here we randomly choose a directional preference
        if (UnityEngine.Random.Range(0, 100) < 50)
        {
            meetPoint = map.map[currentRoomCenter.x, destination.y];
            c1 = CreatePassage(roomA, roomB, tileA, meetPoint, ref map);
            c2 = CreatePassage(roomA, roomB, meetPoint, tileB, ref map);
            if (c1 == null || c2 == null)
            {
                NullifyTwoCorridors(c1, c2, map);
                meetPoint = map.map[destination.x, currentRoomCenter.y];
                c1 = CreatePassage(roomA, roomB, tileA, meetPoint, ref map);
                c2 = CreatePassage(roomA, roomB, meetPoint, tileB, ref map);
            }
        }
        else
        {
            meetPoint = map.map[destination.x, currentRoomCenter.y];
            c1 = CreatePassage(roomA, roomB, tileA, meetPoint, ref map);
            c2 = CreatePassage(roomA, roomB, meetPoint, tileB, ref map);
            if (c1 == null || c2 == null)
            {
                NullifyTwoCorridors(c1, c2, map);
                meetPoint = map.map[currentRoomCenter.x, destination.y];
                c1 = CreatePassage(roomA, roomB, tileA, meetPoint, ref map);
                c2 = CreatePassage(roomA, roomB, meetPoint, tileB, ref map);
            }
        }
        if (c1 == null || c2 == null)
        {
            NullifyTwoCorridors(c1, c2, map);
            return null;
        }
        c1.MergeCorridors(c2);
        //c2.NullifyCorridor(map);
        return c1;


    }

    public static Tuple<Vector2Int, Vector2Int> FindClosestEntrances(RoomNode roomA, RoomNode roomB)
    {
        Tuple<Vector2Int, Vector2Int> result = Tuple.Create(roomA.roomCenter, roomB.roomCenter);

        if (roomA.Entrances.Count == 0 || roomB.Entrances.Count == 0)
        {
            return result;
        }

        float distance = Vector2.Distance(roomA.roomCenter, roomB.roomCenter);
        foreach (Vector2Int e1 in roomA.Entrances)
        {
            foreach (Vector2Int e2 in roomB.Entrances)
            {
                float d = Vector2.Distance(e1, e2);
                if (d < distance)
                {
                    distance = d;
                    result = Tuple.Create(e1, e2);
                }
            }
        }
        return result;
    }

    public static void NullifyTwoCorridors(CorridorNode c1, CorridorNode c2, Map map)
    {
        if (c1 != null)
        {
            c1.NullifyCorridor(map);
        }
        if (c2 != null)
        {
            c2.NullifyCorridor(map);
        }
    }

    public static CorridorNode CreatePassage(RoomNode roomA, RoomNode roomB, TileNode tileA, TileNode tileB, ref Map map)
    {
        // RoomNode.ConnectRooms(roomA, roomB);
        //Debug.DrawLine(TileNodeToWorldPoint(tileA, ref map), TileNodeToWorldPoint(tileB, ref map), Color.green, 50);

        List<TileNode> line = GetLine(tileA, tileB, ref map.map);

        if (line == null)
        {
            return null;
        }

        List<TileNode> tileList = new List<TileNode>();
        CorridorNode corridor = new CorridorNode();

        foreach (TileNode c in line)
        {
            DrawCircle(tileList, c, passageSize, ref map);
            //c.corridors.Add(corridor);
        }

        corridor.tileList = tileList;
        corridor.TargetRoomList.Add(roomA);
        corridor.TargetRoomList.Add(roomB);
        corridor.ConnectedRoomList.Add(roomA);
        corridor.ConnectedRoomList.Add(roomB);

        if (corridor.IsBorderingRoom(map))
        {
            corridor.NullifyCorridor(map);
            return null;
        }

        if (map.debugPartition)
            Debug.DrawLine(TileNodeToWorldPoint(tileA), TileNodeToWorldPoint(tileB), Color.green, 25);

        return corridor;
    }

    // this function merges all the Connected Rooms of both rooms together 
    /*public static void MergeNeighbors(RoomNode roomA, RoomNode roomB)
    {
        roomA.ConnectedRooms.Union<RoomNode>(roomB.ConnectedRooms);
        roomB.ConnectedRooms.Union<RoomNode>(roomA.ConnectedRooms);
    }*/

    public static void DrawCircle(List<TileNode> tileList, TileNode c, int r, ref Map map)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int drawX = c.x + x;
                    int drawY = c.y + y;
                    if (map.IsInMapRange(drawX, drawY) && map.map[drawX, drawY].value == 0 && !map.IsMapBorder(drawX, drawY))
                    {
                        map.map[drawX, drawY].value = 2;
                        tileList.Add(map.map[drawX, drawY]);
                    }
                }
            }
        }
    }

    public static List<TileNode> GetLine(TileNode from, TileNode to, ref TileNode[,] map)
    {
        List<TileNode> line = new List<TileNode>();

        int x = from.x;
        int y = from.y;

        int dx = to.x - from.x;
        int dy = to.y - from.y;

        bool inverted = false;
        int step = Math.Sign(dx);
        int gradientStep = Math.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = Math.Sign(dy);
            gradientStep = Math.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            if (map[x, y].value == 0)
            {
                line.Add(map[x, y]);
            }

            // this checks if the corridor is bulldozing through another room
            if (map[x, y].value == 1 && map[x, y].room != from.room && map[x, y].room != to.room)
            {
                return null;
            }
            /*if (map[x, y].value == 2)
            {
                break;
            }*/

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }
            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }
        return line;
    }

    public static Vector3 TileNodeToWorldPoint(TileNode tile)
    {
        return new Vector3(tile.x, tile.y, 0);
    }


}
