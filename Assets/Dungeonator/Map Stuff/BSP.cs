using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BSP : MonoBehaviour
{
    /*
    // File variables, might be moved to main or GameManager file
    // Size of map
    public int columns;
    public int rows;

    // Min dimensions of rooms
    public int min_x_BSP;
    public int min_y_BSP;

    int[,] map;
    int[,] temp;
    public bool trim;
    public bool connect;

    // Queues for the BSP algorithm
    private Queue<int[]> queue = new Queue<int[]>();
    private Queue<int[]> roomsList = new Queue<int[]>();

    // Poisson Distribution
    List<Vector3> enemies;
    float displayRadius = 1;

    // Main and only callable function
    public int[,] GenerateMap()
    {
        map = new int[columns, rows];
        temp = new int[columns, rows];

        // print("Columns: " + columns);
        // print("Rows: " + rows);

        FillMap();
        BinarySpace();

        /*List<Room> survivingRooms = new List<Room>();
        if (trim) { h.Trim(ref survivingRooms, ref map, ref temp); }

        // After we trim rooms, we want to go through each room and spawn enemies in those rooms
        enemies = PoissonDisc.GeneratePoints(map, 4, new Vector3(columns, 0, rows), 30);
        // print(enemies);
        // print("Enemies: " + enemies);
        // print("Enemies Count: " + enemies.Count);

        if (connect) { h.ConnectClosestRooms(ref survivingRooms, ref map); }

        // Simple loop that makes sure the border is in tact after BSP
        for (int x = 0; x < columns; x++) {
            for (int y = 0; y < rows; y++) {
                if (x == 0 || y == 0 || x == columns || y == rows) {
                    map[x, y] = 1;
                }
            }
        }*/

    /*
        return map;
    }

    // Splits map horizontally
    void splitHorizontal(int[] potentialRoom)
    {
        var x1 = potentialRoom[0];
        var y1 = potentialRoom[1];
        var x2 = potentialRoom[2];
        var y2 = potentialRoom[3];
        var ySplit = UnityEngine.Random.Range(y1 + 1, y2);
        var room1 = new int[] { x1, y1, x2, ySplit };
        var room2 = new int[] { x1, ySplit + 1, x2, y2 };

        // print("In horizontal.");

        queue.Enqueue(room1);
        queue.Enqueue(room2);
    }

    // Splits map vertically
    void splitVertical(int[] potentialRoom)
    {
        var x1 = potentialRoom[0];
        var y1 = potentialRoom[1];
        var x2 = potentialRoom[2];
        var y2 = potentialRoom[3];
        var xSplit = UnityEngine.Random.Range(x1 + 1, x2);
        var room1 = new int[] { x1, y1, xSplit, y2 };
        var room2 = new int[] { xSplit + 1, y1, x2, y2 };

        // print("In vertical.");

        queue.Enqueue(room1);
        queue.Enqueue(room2);
    }

    // Randomly splits map vertically & horizontally
    // to create mismatched grid of rectangles that will hold our rooms
    async void BinarySpace()
    {
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
            /*if (!successfulLarge)
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

    }

    // Splits map horizontally
    void SplitHorizontal(int[] roomSpace)
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

    }*/
}