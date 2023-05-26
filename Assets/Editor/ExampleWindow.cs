using PlasticPipe.PlasticProtocol.Messages;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class GeneratorWindow : EditorWindow
{
    string mapName = "Name";
    int columns = Settings.columns;
    public int Columns
    {
        get { return columns; }
        set
        { 
            columns = value; 
            Settings.columns = columns;
            //Settings.SaveSettings();
        }
    }

    int rows = Settings.rows;
    public int Rows
    {
        get { return rows; }
        set
        {
            rows = value;
            Settings.rows = rows;
            //Settings.SaveSettings();
        }
    }

    int auxiliaryPercent = Settings.auxiliaryPercent;
    public int AuxiliaryPercent
    {
        get { return auxiliaryPercent; }
        set
        {
            auxiliaryPercent = value;
            Settings.auxiliaryPercent = auxiliaryPercent;
            //Settings.SaveSettings();
        }
    }

    int rewardPercent = Settings.rewardPercent;
    public int RewardPercent
    {
        get { return rewardPercent; }
        set
        {
            rewardPercent = value;
            Settings.rewardPercent = rewardPercent;
            //Settings.SaveSettings();
        }
    }

    bool extraRooms = Settings.extraRooms;
    public bool ExtraRooms
    {
        get { return extraRooms; }
        set
        {
            extraRooms = value;
            Settings.extraRooms = extraRooms;
            //Settings.SaveSettings();
        }
    }

    bool bossRoomToggle = Settings.isBossLevel;
    public bool BossRoomToggle
    {
        get { return bossRoomToggle; }
        set
        {
            bossRoomToggle = value;
            Settings.isBossLevel = bossRoomToggle;
            //Settings.SaveSettings();
        }
    }
    bool doorRoomToggle = !Settings.isBossLevel;
    public bool DoorRoomToggle
    {
        get { return doorRoomToggle; }
        set
        {
            doorRoomToggle = value;
        }
    }

    Texture2D mapTexture = null;
    public Texture2D MapTexture 
    { 
        get { return mapTexture; } 
        set
        {
            if (value != mapTexture)
            {
                AssetDatabase.Refresh();
            }
            mapTexture = value;
        } 
    }

    [MenuItem("Window/Map Generator")]
    public static void ShowWindow()
    {
        GetWindow<GeneratorWindow>("Map Generator");
    }

    // Window code
    private void OnGUI()
    {
        //GUILayout.Label("This is a label.", EditorStyles.boldLabel);
        if (GUILayout.Button("Generate New Map"))
        {
            MapGenerator.GenerateMap();
            MapGenerator.GenerateTexture();
        }
        if (GUILayout.Button("Generate New Rooms"))
        {
            MapGenerator.GenerateRooms();
        }
        if (GUILayout.Button("Generate New Corridors"))
        {
            MapGenerator.GenerateCorridors();
            MapGenerator.GenerateTexture();
        }

        mapName = EditorGUILayout.TextField("Map Name", mapName);

        ///GUILayout.Label("Columns", EditorStyles.boldLabel);
        Columns = EditorGUILayout.IntSlider("Columns", columns, 1, 1000);
        ///GUILayout.Label("Rows", EditorStyles.boldLabel);
        Rows = EditorGUILayout.IntSlider("Rows", rows, 1, 1000);
        AuxiliaryPercent = EditorGUILayout.IntSlider("Auxiliary Percent", auxiliaryPercent, 0, 100);
        RewardPercent = EditorGUILayout.IntSlider("Reward Percent", rewardPercent, 0, 100);

        ExtraRooms = EditorGUILayout.Toggle("Extra Rooms?", ExtraRooms);
        BossRoomToggle = EditorGUILayout.Toggle("Boss Room", BossRoomToggle);

        GUILayout.Label("Current Map:", EditorStyles.boldLabel);
        string mapPath = Application.dataPath + Path.AltDirectorySeparatorChar + "Map.png";
        //MapTexture = (Texture2D)Resources.Load(mapPath, typeof(Texture2D));
        MapTexture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Map.png", typeof(Texture2D));
        GUILayout.Label(MapTexture);

        /*if ()
        {
            BossRoomToggle = !BossRoomToggle;
        }*/
        Settings.SaveSettings();
    }
}
