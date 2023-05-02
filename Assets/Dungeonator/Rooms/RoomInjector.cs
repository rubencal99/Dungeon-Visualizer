using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomInjector
{
    public static string[,] Rotate(string[,] array)
    {
        //[5, 7] becomes
        //[7, 5]
        string[,] newArray = new string[array.GetLength(1), array.GetLength(0)];
        //Debug.Log("Old array dim: (" + array.GetLength(0) + ", " + array.GetLength(1) + ")");
        //Debug.Log("New array dim: (" + newArray.GetLength(0) + ", " + newArray.GetLength(1) + ")");
        int newColumn, newRow = 0;
        //5
        for (int oldColumn = 0; oldColumn < array.GetLength(0); oldColumn++)
        {                                            //7
            newColumn = newArray.GetLength(1) - 1;
            for (int oldRow = 0; oldRow < array.GetLength(1); oldRow++)
            {
                //Debug.Log("Old Column: " + oldColumn);
                //Debug.Log("Old Row: " + oldRow);
                //Debug.Log("New Column: " + newColumn);
                //Debug.Log("New Row: " + newRow);
                newArray[newRow, newColumn] = array[oldRow, oldColumn];
                newColumn--;
            }
            newRow++;
        }
        return newArray;
    }


}