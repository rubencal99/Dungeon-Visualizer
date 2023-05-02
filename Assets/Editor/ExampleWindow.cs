using PlasticPipe.PlasticProtocol.Messages;
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
        }
        if (GUILayout.Button("Generate New Rooms"))
        {
            MapGenerator.GenerateRooms();
        }
        if (GUILayout.Button("Generate New Corridors"))
        {
            MapGenerator.GenerateCorridors();
        }

        mapName = EditorGUILayout.TextField("Map Name", mapName);

        ///GUILayout.Label("Columns", EditorStyles.boldLabel);
        Columns = EditorGUILayout.IntSlider("Columns", columns, 1, 1000);
        ///GUILayout.Label("Rows", EditorStyles.boldLabel);
        Rows = EditorGUILayout.IntSlider("Rows", rows, 1, 1000);
        AuxiliaryPercent = EditorGUILayout.IntSlider("Auxiliary Percent", auxiliaryPercent, 0, 100);
        RewardPercent = EditorGUILayout.IntSlider("Reward Percent", rewardPercent, 0, 100);

        BossRoomToggle = EditorGUILayout.Toggle("Boss Room", BossRoomToggle);

        /*if ()
        {
            BossRoomToggle = !BossRoomToggle;
        }*/
        Settings.SaveSettings();
    }
}
