using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SettingsInteractor : MonoBehaviour
{
    //public MapGenerator Map;

    void Update()
    {
        UpdateVariables();
    }

    void UpdateVariables()
    {
        settingsAuxiliaryPercent = Settings.auxiliaryPercent;
        settingsRewardPercent = Settings.rewardPercent;
        settingsRewardRoom = Settings.MaxRewardRooms;
        settingsBossRoomToggle = Settings.isBossLevel;
        settingsDoorRoomToggle = !Settings.isBossLevel;
        settingsWidth = Settings.columns;
        settingsHeight = Settings.rows;

    }

    [SerializeField]
    private float settingsAuxiliaryPercent;
    [SerializeField]
    private float auxiliaryPercent;
    [SerializeField]
    private Text auxiliaryText;

    public float AuxiliaryPercent
    {
        get { return auxiliaryPercent; }
        set 
        { 
            auxiliaryPercent = (int)value; 
            auxiliaryText.text = "Auxiliary Percent: " + (int)value;
            Settings.auxiliaryPercent = (int)value;
        }
    }

    [SerializeField]
    private float settingsRewardPercent;
    [SerializeField]
    private float rewardPercent;
    [SerializeField]
    private Text rewardPercentText;

    public float RewardPercent
    {
        get { return rewardPercent; }
        set 
        { 
            rewardPercent = (int)value; 
            rewardPercentText.text = "Reward Percent: " + (int)value;
            Settings.rewardPercent = (int)value;
        }
    }

    [SerializeField]
    private int settingsRewardRoom;
    [SerializeField]
    private int rewardRoom;
    [SerializeField]
    private Text rewardRoomText;

    public string RewardRoom
    {
        get { return rewardRoom.ToString(); }
        set 
        { 
            rewardRoom = int.Parse(value); 
            rewardRoomText.text = "Reward Rooms: " + int.Parse(value);
            Settings.MaxRewardRooms = int.Parse(value);
        }
    }

    [SerializeField]
    private bool settingsBossRoomToggle;
    [SerializeField]
    private bool bossRoomToggle;

    public bool BossRoomToggle
    {
        get { return bossRoomToggle; }
        set 
        { 
            bossRoomToggle = value; 
            Settings.isBossLevel = value;
        }
    }

    [SerializeField]
    private bool settingsDoorRoomToggle;
    [SerializeField]
    private bool doorRoomToggle;

    public bool DoorRoomToggle
    {
        get { return doorRoomToggle; }
        set 
        { 
            doorRoomToggle = value;
        }
    }


    [SerializeField]
    private float settingsWidth;
    [SerializeField]
    private float width;
    [SerializeField]
    private Text widthText;

    public float Width
    {
        get { return width; }
        set 
        { 
            width = (int)value; 
            widthText.text = "Width: " + (int)value;
            Settings.columns = (int)value;
        }
    }

    [SerializeField]
    private float settingsHeight;
    [SerializeField]
    private float height;
    [SerializeField]
    private Text heightText;

    public float Height
    {
        get { return height; }
        set 
        { 
            height = (int)value; 
            heightText.text = "Height: " + (int)value;
            Settings.rows = (int)value;
        }
    }

    void OnDrawGizmos()
    {
        if (MapGenerator.Map == null || MapGenerator.Map.map == null)
            return;

        TileNode[,] map = MapGenerator.Map.map;
        int columns = Settings.columns;
        int rows = Settings.rows;


        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Debug.Log("(x, y) = (" + x + ", " + y+ ")");
                if (map[x, y].value == 0 || map[x, y].isObstacle)
                {
                    Gizmos.color = new Color(0, 0, 0, 1f);
                }
                else if (map[x, y].value == 1 && map[x, y].room != null)
                {
                    if (map[x, y].room.RoomType == "Start")
                    {
                        Gizmos.color = new Color(0, 255, 0, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Boss" || map[x, y].room.RoomType == "Key")
                    {
                        Gizmos.color = new Color(255, 0, 0, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Door")
                    {
                        Gizmos.color = new Color(155 / 255f, 0, 0, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Shop")
                    {
                        Gizmos.color = new Color(210 / 255f, 105 / 255f, 30 / 255f, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Reward")
                    {
                        Gizmos.color = new Color(0, 0, 255, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Auxiliary")
                    {
                        Gizmos.color = new Color(130 / 255f, 115 / 255f, 150 / 255f, 1f);
                    }
                    else if (map[x, y].room.RoomType == "Extra")
                    {
                        Gizmos.color = new Color(150 / 255f, 150 / 255f, 0, 1f);
                    }
                    else
                    {
                        Gizmos.color = new Color(255, 255, 0, 1f);
                    }

                }
                else
                {
                    Gizmos.color = new Color(0, 255, 255, 1f);
                }

                //Gizmos.DrawCube(new Vector3(x, 0, y), new Vector3(1, 1, 1));
                Gizmos.DrawCube(new Vector3(x, y, 0), new Vector3(1, 1, 1));
            }
        }
    }

}
