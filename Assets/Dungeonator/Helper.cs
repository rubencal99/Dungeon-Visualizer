using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class Helper
{
    public static int passageSize = 2;
    public static CorridorNode CreateCorridor(RoomNode roomA, RoomNode roomB, TileNode tileA, TileNode tileB, ref TileNode[,] map)
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
            meetPoint = map[currentRoomCenter.x, destination.y];
            c1 = CreatePassage(roomA, roomB, tileA, meetPoint, ref map);
            c2 = CreatePassage(roomA, roomB, meetPoint, tileB, ref map);
            if (c1 == null || c2 == null)
            {
                NullifyTwoCorridors(c1, c2);
                meetPoint = map[destination.x, currentRoomCenter.y];
                c1 = CreatePassage(roomA, roomB, tileA, meetPoint, ref map);
                c2 = CreatePassage(roomA, roomB, meetPoint, tileB, ref map);
            }
        }
        else
        {
            meetPoint = map[destination.x, currentRoomCenter.y];
            c1 = CreatePassage(roomA, roomB, tileA, meetPoint, ref map);
            c2 = CreatePassage(roomA, roomB, meetPoint, tileB, ref map);
            if (c1 == null || c2 == null)
            {
                NullifyTwoCorridors(c1, c2);
                meetPoint = map[currentRoomCenter.x, destination.y];
                c1 = CreatePassage(roomA, roomB, tileA, meetPoint, ref map);
                c2 = CreatePassage(roomA, roomB, meetPoint, tileB, ref map);
            }
        }
        if (c1 == null || c2 == null)
        {
            NullifyTwoCorridors(c1, c2);
            return null;
        }
        c1.MergeCorridors(c2);
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

    public static void NullifyTwoCorridors(CorridorNode c1, CorridorNode c2)
    {
        if (c1 != null)
        {
            c1.NullifyCorridor();
        }
        if (c2 != null)
        {
            c2.NullifyCorridor();
        }
    }

    public static CorridorNode CreatePassage(RoomNode roomA, RoomNode roomB, TileNode tileA, TileNode tileB, ref TileNode[,] map)
    {
        // RoomNode.ConnectRooms(roomA, roomB);
        //Debug.DrawLine(TileNodeToWorldPoint(tileA, ref map), TileNodeToWorldPoint(tileB, ref map), Color.green, 50);

        List<TileNode> line = GetLine(tileA, tileB, ref map);

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

        if (corridor.IsBorderingRoom())
        {
            corridor.NullifyCorridor();
            return null;
        }

        Debug.DrawLine(TileNodeToWorldPoint(tileA, ref map), TileNodeToWorldPoint(tileB, ref map), Color.green, 50);
        return corridor;
    }

    // this function merges all the Connected Rooms of both rooms together 
    public static void MergeNeighbors(RoomNode roomA, RoomNode roomB)
    {
        roomA.ConnectedRooms.Union<RoomNode>(roomB.ConnectedRooms);
        roomB.ConnectedRooms.Union<RoomNode>(roomA.ConnectedRooms);
    }

    public static void DrawCircle(List<TileNode> tileList, TileNode c, int r, ref TileNode[,] map)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int drawX = c.x + x;
                    int drawY = c.y + y;
                    if (IsInMapRange(drawX, drawY, ref map) && map[drawX, drawY].value == 0 && !IsMapBorder(drawX, drawY, ref map))
                    {
                        map[drawX, drawY].value = 2;
                        tileList.Add(map[drawX, drawY]);
                    }
                }
            }
        }
    }

    // Checks to see if [x, y] is in map
    public static bool IsInMapRange(int x, int y, ref TileNode[,] map)
    {
        int columns = map.GetLength(0);
        int rows = map.GetLength(1);
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    public static bool IsMapBorder(int x, int y, ref TileNode[,] map)
    {
        int columns = map.GetLength(0);
        int rows = map.GetLength(1);
        return x == 0 || x == columns || y == 0 || y == rows;
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

    public static Vector3 TileNodeToWorldPoint(TileNode tile, ref TileNode[,] map)
    {
        return new Vector3(tile.x, tile.y, 0);
    }
}