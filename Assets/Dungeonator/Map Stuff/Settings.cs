using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEditor;

public static class Settings
{
    // Queues for the BSP algorithm
    public static List<TileNode> roomTiles = new List<TileNode>();
    public static Queue<Tuple<int[], string>> roomsList = new Queue<Tuple<int[], string>>();
    public static List<RoomNode> Rooms;
    public static List<CorridorNode> Corridors;

    public static bool debugPartition;

    public static int columns = 100;
    public static int rows = 100;

    private static RoomNode StartRoom;
    private static RoomNode EndRoom;
    private static RoomNode ShopRoom;
    private static RoomNode DoorRoom;

    public static bool HasEntry = false;
    public static bool isBossLevel = false;
    public static int numLargeRooms;
    public static int NumBossRooms = 1;
    public static int NumNormalRooms;
    public static int NumRewardRooms = 0;
    public static int MaxRewardRooms = 8;

    private static bool forceEntry = false;
    public static bool extraRooms = false;
    public static int numExtraRooms = 0;
    public static int maxExtraRooms = 4;

    public static int rewardPercent = 50;
    public static int auxiliaryPercent = 33;

    // Min dimensions of rooms
    public static Vector2Int minRoomDim = new Vector2Int(18, 18);
    public static Vector2Int minEntryDim = new Vector2Int(16, 16);
    public static Vector2Int maxEntryDim = new Vector2Int(0, 0);

    public static Vector2Int minNormalDim = new Vector2Int(27, 27);
    public static Vector2Int maxNormalDim = new Vector2Int(28, 28);

    public static Vector2Int minLargeDim = new Vector2Int(35, 35);
    public static Vector2Int maxLargeDim = new Vector2Int(45, 45);

    public static Vector2Int minRewardDim = new Vector2Int(15, 15);
    public static Vector2Int maxRewardDim = new Vector2Int(26, 26);

    public static Vector2Int minBossDim = new Vector2Int(30, 30);
    public static Vector2Int maxBossDim = new Vector2Int(40, 40);



    /// <summary>
    /// Saving Logic
    /// </summary>
    private static string path;
    private static string persistentPath;

    struct SaveData
    {
        public bool DebugPartition;
        public int Columns;
        public int Rows;

        public int RewardPercent;
        public int AuxiliaryPercent;

        public bool IsBossLevel;
    }

    private static void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SettingsData.json";
        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SettingsData.json";
    }

    public static void SaveSettings()
    {
        if (path == null || persistentPath == null)
            SetPaths();

        string savePath = path;

        //Debug.Log("Saving Data at " + savePath);
        string json = JsonUtility.ToJson(
            new SaveData()
            {
                DebugPartition = debugPartition,
                Columns = columns,
                Rows = rows,
                RewardPercent = rewardPercent,
                AuxiliaryPercent = auxiliaryPercent,
                IsBossLevel = isBossLevel,
            });
        //Debug.Log(json);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadSettings()
    {
        SetPaths();

        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        SaveData data = JsonUtility.FromJson<SaveData>(json);
        Debug.Log(data.ToString());

        RestoreData(data);
    }

    static void RestoreData(SaveData data)
    {
        debugPartition = data.DebugPartition;
        columns = data.Columns;
        rows = data.Rows;
        rewardPercent = data.RewardPercent;
        auxiliaryPercent = data.AuxiliaryPercent;
        isBossLevel = data.IsBossLevel;
    }
}

[InitializeOnLoad]
public class StartUp
{
    static StartUp()
    {
        Settings.LoadSettings();
    }

}
