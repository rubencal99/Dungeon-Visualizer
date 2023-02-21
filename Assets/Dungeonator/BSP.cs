using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BSP : MonoBehaviour
{
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

        return map;
    }

    // Fills map with negative space
    void FillMap()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                map[x, y] = 1;
            }
        }
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
    void BinarySpace()
    {
        // This will be our queue of rooms
        // print("In BSP");
        var temp = new int[4];
        temp[0] = 0;
        temp[1] = 0;
        temp[2] = columns - 1;
        temp[3] = rows - 1;
        queue.Enqueue(temp);

        // Main loop that splits map
        int count = 0;
        while (queue.Count > 0)
        {
            print("COUNT: " + queue.Count);
            print("Times looped: " + count);
            if (count > 100)
            {
                break;
            }
            int[] space = queue.Dequeue();
            var x1 = space[0];
            var y1 = space[1];
            var x2 = space[2];
            var y2 = space[3];

            print("x1, y1, x2, y2: " + x1 + ", " + y1 + ", " + x2 + ", " + y2);

            var width = x2 - x1;
            var height = y2 - y1;

            //print("Width: " + width);
            //print("Height: " + height);

            if (width > min_x_BSP && height > min_y_BSP)
            {
                // print("In if");
                // Randomly choose which split we prefer
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    if (height >= min_y_BSP * 2)
                    {
                        splitHorizontal(space);
                    }
                    else if (width >= min_x_BSP * 2)
                    {
                        splitVertical(space);
                    }
                    else
                    {
                        roomsList.Enqueue(space);
                    }
                }
                else
                {
                    if (width >= min_x_BSP * 2)
                    {
                        splitVertical(space);
                    }
                    else if (height >= min_y_BSP * 2)
                    {
                        splitHorizontal(space);
                    }
                    else
                    {
                        roomsList.Enqueue(space);
                    }
                }
            }
            count++;
        }

        // print("About to create rooms");
        // print("roomsList Count: " + roomsList.Count);
        while (roomsList.Count > 0)
        {
            int[] room = roomsList.Dequeue();

            int x1 = (int)room[0];
            int y1 = (int)room[1];
            int x2 = (int)room[2];
            int y2 = (int)room[3];

            // Here we fill the negative space with empty space 
            // I.e. room creation
            for (int i = y1 + 1; i < y2 - 1; i++)
            {
                for (int j = x1 + 1; j < x2 - 1; j++)
                {
                    map[i, j] = 0;
                }
            }
        }
    }

    /* void OnDrawGizmos()
    {
        if (enemies != null)
        {
            foreach (Vector3 enemy in enemies)
            {
                // print("Enemy X: " + enemy.x);
                // print("Enemy Y: " + enemy.y);
                // print("Enemy Z: " + enemy.z);
                Gizmos.DrawSphere(enemy, displayRadius);
            }
        }*/
    /* for (int x = 0; x < columns; x++)
    {
        for (int y = 0; y < rows; y++)
        {
            if (map[x, y] == 0)
            {
                Gizmos.color = new Color(0, 0, 0, 0f);
            }
            else
            {
                Gizmos.color = new Color(0, 0, 0, 1f);
            }

            Gizmos.DrawCube(new Vector3(x, 0, y), new Vector3(1, 1, 1));
        }
    }
}*/

}