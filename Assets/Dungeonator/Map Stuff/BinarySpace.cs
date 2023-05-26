using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BinarySpace
{
    // TODO: Clean up variable usage
    public static List<int[]> queue;
    public static Queue<Tuple<int[], string>> roomsList;

    public static int NumRewardRooms;
    public static int MaxRewardRooms;

    private static bool extraRooms;
    private static int numExtraRooms;
    private static int maxExtraRooms;

    public static int rewardPercent;
    public static int auxiliaryPercent;


    // Min dimensions of rooms
    public static Vector2Int minRoomDim;
    public static Vector2Int minEntryDim;
    public static Vector2Int maxEntryDim;

    public static Vector2Int minNormalDim;
    public static Vector2Int maxNormalDim;

    public static Vector2Int minLargeDim;
    public static Vector2Int maxLargeDim;

    public static Vector2Int minRewardDim;
    public static Vector2Int maxRewardDim;

    public static Vector2Int minBossDim;
    public static Vector2Int maxBossDim;

    static void SetVariables()
    {
        queue = new List<int[]>();
        roomsList = new Queue<Tuple<int[], string>>();

        NumRewardRooms = Settings.NumRewardRooms;
        MaxRewardRooms = Settings.MaxRewardRooms;

        extraRooms = Settings.extraRooms;
        numExtraRooms = Settings.numExtraRooms;
        maxExtraRooms = Settings.maxExtraRooms;

        rewardPercent= Settings.rewardPercent;
        auxiliaryPercent= Settings.auxiliaryPercent;

        minRoomDim = Settings.minRoomDim;
        minEntryDim = Settings.minEntryDim;
        maxEntryDim = Settings.maxEntryDim;

        minNormalDim = Settings.minNormalDim;
        maxNormalDim = Settings.maxNormalDim;

        minLargeDim = Settings.minLargeDim;
        maxLargeDim = Settings.maxLargeDim;

        minRewardDim = Settings.minRewardDim;
        maxRewardDim = Settings.maxRewardDim;

        minBossDim = Settings.minBossDim;
        maxBossDim = Settings.maxBossDim;
    }

    // TODO: Clean up and optimize code
    public static void BinarySpaceSplit(ref Map map)
    {
        int columns = Settings.columns;
        int rows = Settings.rows;

        SetVariables();

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
            /*if (count > 200)
            {
                Debug.Log("Too many rooms created");
                break;
            }*/

            int[] space;
            if (Settings.HasEntry)
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
            if (!successfulLarge)
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

        queue.Clear();
        map.roomsList = roomsList;

    }

    // Splits map horizontally
    static void SplitHorizontal(int[] roomSpace)
    {
        var x1 = roomSpace[0];
        var y1 = roomSpace[1];
        var x2 = roomSpace[2];
        var y2 = roomSpace[3];

        var buffer = (int)(y2 - y1) / 3;
        var ySplit = UnityEngine.Random.Range(y1 + 1 + buffer, y2 - buffer);
        var room1 = new int[] { x1, y1, x2, ySplit };
        var room2 = new int[] { x1, ySplit + 1, x2, y2 };

        if (Settings.debugPartition)
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
    static void SplitVertical(int[] roomSpace)
    {
        var x1 = roomSpace[0];
        var y1 = roomSpace[1];
        var x2 = roomSpace[2];
        var y2 = roomSpace[3];

        var buffer = (int)(x2 - x1) / 3;
        var xSplit = UnityEngine.Random.Range(x1 + 1 + buffer, x2 - buffer);
        var room1 = new int[] { x1, y1, xSplit, y2 };
        var room2 = new int[] { xSplit + 1, y1, x2, y2 };

        if (Settings.debugPartition)
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

}
